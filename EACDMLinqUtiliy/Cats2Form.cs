using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QTIUtility;
using System.Xml.Linq;
using System.Xml.XPath;
using System.IO;
using LINQtoXSDLib;
using QTIUtility.App_Data.LocalCatsTableAdapters;
using QTIUtility.App_Data;
using System.Data.OleDb;

namespace EACDMLinqUtiliy
{
    public partial class CatsForm : Form
    {
        private DataTable cats;
        private DMClient dmc;
        public CatsForm()
        {
            InitializeComponent();
            
        }

        private void btnFillCats_Click(object sender, EventArgs e)
        {
            if(dmc.token == null)
            {
                dmc.token = dmc.getToken();
            }
            cats =  BbQuery.getCMSCats(dmc.origUrl, dmc.token);
            cats.TableName = "cats";
            catsGrid.DataSource = cats;
            
            
            
            //DataSet myCats = new DataSet();
            //myCats.Tables.Add(cats);
            //myCats.Tables.Add(items);
            //DataRelation catItem = new DataRelation("catItem", cats.Columns["description"], items.Columns["description"]);
            ////Category c = new Category();
            ////DataTableReader dtr = c.CreateDataReader(cats, items);
            ////c.Load(dtr, LoadOption.OverwriteChanges, c.CATEGORY, c.ITEM_CATEGORY, c.client);
            
            //BindingSource masterBindingSource = new BindingSource();
            //masterBindingSource.DataSource = myCats;
            //masterBindingSource.DataMember = "cats";
            //BindingSource detailsBindingSource = new BindingSource();
            //detailsBindingSource.DataSource = masterBindingSource;
            //detailsBindingSource.DataMember = "catItem";
            //catsGrid.DataSource = masterBindingSource;
            //itemsGrid.DataSource = detailsBindingSource;

        }

        private void CatsForm_Load(object sender, EventArgs e)
        {
            dmc =(DMClient) this.Tag;
            dmc.isOracle = dmc.getOracle();
        }

        private void itemsGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            

        }

        private void catsGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataTable items = BbQuery.getCMSCatsByDescriptionDetails(catsGrid.Rows[e.RowIndex].Cells["Category"].Value.ToString(), dmc.origUrl, dmc.token);
            items.TableName = "items";

            // present in second grid
            itemsGrid.DataSource = items;
        }

        private void btnFillCMS_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            int adds = BbQuery.updateCMSCatLibrary(cats,dmc.isOracle, dmc.origUrl, dmc.token);
            lbAdds.Text = adds.ToString();
            Cursor.Current = Cursors.Default;
            Application.DoEvents();
        }

        private void btnCatsFillLocal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string connectionPrefix = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            string client = new Uri(dmc.origUrl).Host;
            string filename = client+".mdb";
            string app_data = Path.GetDirectoryName(Application.ExecutablePath) + @"\App_data\";
            string dest = app_data + filename;
            if (!File.Exists(dest))
            {
                File.Copy(app_data + "LocalCats.mdb", dest);
            }
            string connection = connectionPrefix + dest;
            using (TableAdapterManager tam = new TableAdapterManager())
            {
                OleDbConnection myConn = new OleDbConnection(connection);
                tam.Connection = myConn;
                tam.CategoryTableAdapter = new CategoryTableAdapter(myConn);
                tam.CategoryTableAdapter.ClearBeforeFill = true;
                tam.clientTableAdapter = new clientTableAdapter(myConn);
                tam.clientTableAdapter.ClearBeforeFill = true;
                LocalCats myCats = new LocalCats();
                tam.clientTableAdapter.Fill(myCats.client);
                tam.CategoryTableAdapter.Fill(myCats.Category);
                if (myCats.client == null || myCats.client.Rows.Count == 0)
                {
                    DataRow dr = myCats.client.NewRow();
                    dr["cmsurl"] = client;
                    myCats.client.Rows.Add(dr);
                }
                DataTableReader dtr = myCats.CreateDataReader(cats);
                myCats.Load(dtr, LoadOption.Upsert, myCats.Category,myCats.client);
                tam.UpdateAll(myCats);
            }
            Cursor.Current = Cursors.Default;
            return;

            
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            catsGrid.DataSource = null;
            catsGrid.Rows.Clear();
        }

        private void btnCatsFromLocal_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            string connectionPrefix = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
            string client = new Uri(dmc.origUrl).Host;
            string filename = client + ".mdb";
            string app_data = Path.GetDirectoryName(Application.ExecutablePath) + @"\App_data\";
            string dest = app_data + filename;
            if (!File.Exists(dest))
            {
                MessageBox.Show("Cannot find data file");
                return;
            }
            string connection = connectionPrefix + dest;
            using (CategoryTableAdapter catsda = new CategoryTableAdapter())
            {

                catsda.SetConnection(new OleDbConnection(connection));
                LocalCats.CategoryDataTable cats = new LocalCats.CategoryDataTable();
                catsda.Fill(cats);
                catsGrid.DataSource = cats;
            }
            Cursor.Current = Cursors.Default;
            return;

        }
    }
    
}
