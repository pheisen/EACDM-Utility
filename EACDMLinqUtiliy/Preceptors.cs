using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using QTIUtility;
using System.IO;

namespace EACDMLinqUtiliy
{
    public partial class Preceptors : Form
    {
        private Button btnCheckThem;
        private string course_pk = null;
        private string qpk1 = null;
        private DMClient dmc;
        private static string ConnectionPrefix = (string)Properties.Settings.Default["ConnectionPrefix"];
        private static string oledbConnection = ConnectionPrefix + @"C:\utility\EACDMUtiliy\EACDM Utility\EACDMLinqUtiliy\WorkingData\Preceptors.mdb";
        private QTIUtility.Preceptors.PreceptorsDataTable preceptors;
        public Preceptors()
        {
            InitializeComponent();
            cGrid.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(cGrid_DataBindingComplete);
            
        }

        void cGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (cGrid.SelectedRows != null && cGrid.SelectedRows.Count > 0)
            {
                course_pk = cGrid.SelectedRows[0].Cells["PK1"].Value.ToString();
            }
        }

        private void btnGetCourses_Click(object sender, EventArgs e)
        {
            if (dmc.token == null)
            {
                dmc.token = dmc.getToken();
            }
            DataTable courses = BbQuery.GetEACSpecialCourses("NU-", dmc.origUrl, dmc.token);

            cGrid.DataSource = courses;
        }
        // Dsn=Excel Files;dbq=C:\utility\EACDMUtiliy\EACDM Utility\EACDMLinqUtiliy\WorkingData\preceptors.xls;defaultdir=C:\utility\EACDMUtiliy\EACDM Utility\EACDMLinqUtiliy\WorkingData;driverid=1046;maxbuffersize=2048;pagetimeout=5
        private void Preceptors_Load(object sender, EventArgs e)
        {
            dmc = (DMClient)this.Tag;
        }

        private void btnGetPreceptors_Click(object sender, EventArgs e)
        {
            
            preceptors = BbQuery.getPreceptors(oledbConnection);
            pSource1.DataSource = preceptors;
            pGrid.DataSource = pSource1;
            pNav1.BindingSource = pSource1;
            btnCheckThem = new Button();
            btnCheckThem.Image = Properties.Resources.undo_16;
            btnCheckThem.Location = new System.Drawing.Point(pGrid.RowHeadersWidth, 0);
            btnCheckThem.Name = "btnCheckThem";
            btnCheckThem.Size = new System.Drawing.Size(new Point(pGrid.Columns[0].HeaderCell.Size.Width, pGrid.Columns[0].HeaderCell.Size.Height));
            btnCheckThem.TabIndex = 3;
            btnCheckThem.UseVisualStyleBackColor = true;
            btnCheckThem.Click += new EventHandler(btnCheckThem_Click);
            pGrid.Controls.Add(btnCheckThem);
            pGrid.Columns["user_pk"].ReadOnly = true;
            pGrid.Columns["user_id"].ReadOnly = true;
            pGrid.Columns["dtcreated"].ReadOnly = true;
            
            //pGrid.Columns["check"] = new DataGridViewCheckBoxColumn();
        }
        void btnCheckThem_Click(object sender, EventArgs e)
        {
            // bool warned = false;
            int checkedCount = 0;
            int lastCheck = -1;
            for (int i = 0; i < pGrid.Rows.Count; i++)
            {
                if (!(pGrid.Rows[i].Cells[0].Selected))
                {
                    (pGrid.Rows[i].Cells[0] as DataGridViewCheckBoxCell).Value = false;
                    continue;
                }
                else
                {

                    checkedCount++;
                    (pGrid.Rows[i].Cells[0] as DataGridViewCheckBoxCell).Value = true;
                    lastCheck = i;

                }
            }
            if (lastCheck > -1)
            {
                pGrid.EndEdit();
            }
        }

        private void cGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // fill aGrid with clinical forms
            course_pk = cGrid.Rows[e.RowIndex].Cells["PK1"].Value.ToString();
            DataTable dt = BbQuery.getGradebookAssessments(cGrid.Rows[e.RowIndex].Cells["PK1"].Value.ToString(), dmc.origUrl, dmc.token);
            aGrid.DataSource = dt;
        }

        private void aGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            qpk1 = aGrid.Rows[e.RowIndex].Cells["QPK1"].Value.ToString();
            DataTable dt = BbQuery.getAssessment(aGrid.Rows[e.RowIndex].Cells["QPK1"].Value.ToString(), dmc.origUrl, dmc.token);
            qGrid.DataSource = dt;
        }

        private void btnEnroll_Click(object sender, EventArgs e)
        {
            bool isChecked = false;
            DataTable enroll =((DataTable) ((BindingSource) pGrid.DataSource).DataSource).Clone();
            enroll.Columns.Add("course_pk", typeof(string));
            for (int i = 0; i < pGrid.Rows.Count; i++)
            {
                if ((pGrid.Rows[i].Cells[0] as DataGridViewCheckBoxCell).Value != null && ( (bool) (pGrid.Rows[i].Cells[0] as DataGridViewCheckBoxCell).Value == true))
                {
                    isChecked = true;
                    DataRow en = enroll.NewRow();
                    en.ItemArray = ((DataRowView)pGrid.Rows[i].DataBoundItem).Row.ItemArray;
                    en["course_pk"] = course_pk;
                    enroll.Rows.Add(en);
                }
            }
            if (!isChecked)
            {
                MessageBox.Show("No preceptors checked.");
                return;
            }
            DataSet enrollment = new DataSet("enrollment");
            enroll.TableName = "enrollee";
            enrollment.Tables.Add(enroll);
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings ws = new XmlWriterSettings();
            ws.Indent = false;
            ws.NewLineHandling = NewLineHandling.Replace;
            ws.NewLineChars = "";
            XmlWriter xw = XmlWriter.Create(sb,ws);
            string tableName = enroll.TableName;
            enrollment.WriteXml(xw);
            xw.Close();
            string deploys = sb.ToString();
            // get enroll url
            string path = dmc.origUrl.Substring(0, dmc.origUrl.LastIndexOf('/'));
            path = path + "/" + BbQuery.BbSpecialFunctionsUrl;
            DataTable enrollees = BbQuery.enroll(path, deploys);
            if (preceptors.PrimaryKey == null)
            {
                preceptors.PrimaryKey = new DataColumn[] { preceptors.Columns["localId"]};
            }
            foreach (DataRow en in enrollees.Rows)
            {
                DataRow pr = preceptors.Select("localid = "+en["localid"].ToString())[0];
                if (pr != null)
                {
                    pr["user_id"] = en["user_id"];
                    pr["user_pk"] = en["user_pk"];
                    pr["dtcreated"] = en["dtcreated"];
                }
            }
            
            //Update underlying datasource
            BbQuery.updatePreceptors(oledbConnection,preceptors);
            pGrid.Refresh();
            return;
        }

        private void qGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            string theQpk1 = qGrid.Rows[e.RowIndex].Cells["QPK1"].Value.ToString();
            string theQuestion = qGrid.Rows[e.RowIndex].Cells["Question"].Value.ToString();
            string path = dmc.origUrl.Substring(0, dmc.origUrl.LastIndexOf('/'));
            path = path + "/" + BbQuery.BbSpecialFunctionsUrl;
            Cursor.Current = Cursors.WaitCursor;
            if (BbQuery.UpdateData(course_pk, theQpk1, theQuestion, path, dmc.origUrl, dmc.token))
            {
                DataTable dt = BbQuery.getOneAssessment(theQpk1, dmc.origUrl, dmc.token);
                object[] vs = new object[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++ )
                {
                    vs[i] = dt.Rows[0][i].ToString();
                }
                qGrid.Rows[e.RowIndex].SetValues(vs);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnAttemps_Click(object sender, EventArgs e)
        {
            // basic query
            Attempts att = new Attempts();
            att.course_pk = course_pk;
            att.Tag = dmc;
            att.ShowDialog();
        }
    }
}
