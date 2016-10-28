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

namespace EACDMLinqUtiliy
{
    public partial class CatsForm : Form
    {
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
            DataTable cats =  BbQuery.getCMSCats(dmc.origUrl, dmc.token);
            cats.TableName = "cats";
            DataTable items = BbQuery.getCMSItemCats(dmc.origUrl, dmc.token);
            items.TableName = "items";
            DataSet myCats = new DataSet();
            myCats.Tables.Add(cats);
            myCats.Tables.Add(items);
         
            Category c = new Category();
            DataTableReader dtr = c.CreateDataReader(cats, items);
            c.Load(dtr, LoadOption.OverwriteChanges, c.CATEGORY, c.ITEM_CATEGORY, c.client);
            
            BindingSource masterBindingSource = new BindingSource();
            masterBindingSource.DataSource = c;
            masterBindingSource.DataMember = c.CATEGORY.TableName;
            BindingSource detailsBindingSource = new BindingSource();
            detailsBindingSource.DataSource = masterBindingSource;
            detailsBindingSource.DataMember = c.Relations[0].RelationName;
            catsGrid.DataSource = masterBindingSource;
            itemsGrid.DataSource = detailsBindingSource;

        }

        private void CatsForm_Load(object sender, EventArgs e)
        {
            
            dmc =(DMClient) this.Tag;
           
        }

        private void itemsGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            XElement node = BbQuery.getQData(itemsGrid.Rows[e.RowIndex].Cells["qpk1"].Value.ToString(),dmc.origUrl, dmc.token);
            XElement el1 = node.XPathSelectElement(BbQuery.QuestionText);
            XElement el2 = node.XPathSelectElement(BbQuery.QuestionType);
            
            if (el1 != null)
            {
                MessageBox.Show(Utilities.StripTags(el1.Value),el2.Value);
            }
            else
            {
                MessageBox.Show("Null");
            }
            //StringBuilder sb = new StringBuilder();
            //StringWriter sw = new StringWriter(sb);

            //node.Save(sw);
            //MessageBox.Show(sb.ToString());
            //sw.Close();
        }
    }
}
