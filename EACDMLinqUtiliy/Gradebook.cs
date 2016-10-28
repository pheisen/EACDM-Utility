using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QTIUtility;

namespace EACDMLinqUtiliy
{
    public partial class Gradebook : Form
    {
        private DMClient dmc;
        public Gradebook()
        {
            InitializeComponent();
        }

        private void btnFillCourses_Click(object sender, EventArgs e)
        {
            DateTime fDate = DateTime.Now.AddYears(-1);
                
            string fd = fDate.ToString("yyyy-MM-dd");
            fd = "{d '" + fd + "'}";
            if(dmc.token == null)
            {
                dmc.token = dmc.getToken();
            }
            DataTable courses =  BbQuery.GetRecentCourses(fd,dmc.origUrl, dmc.token);

            cGrid.DataSource = courses;

            

        }

        private void Gradebook_Load(object sender, EventArgs e)
        {
            dmc = (DMClient) this.Tag;
        }

        private void cGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // get gradebook stuff
            DataTable dt = BbQuery.getGradebookDetail(cGrid.Rows[e.RowIndex].Cells["PK1"].Value.ToString(), dmc.origUrl, dmc.token);
            // present in second grid
            gGrid.DataSource = dt;
        }
    }
}
