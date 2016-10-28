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
    public partial class Attempts : Form
    {
        public string course_pk;
        private DMClient dmc;
        public Attempts()
        {
            InitializeComponent();
            
        }

        private void Attempts_Load(object sender, EventArgs e)
        {
            dmc = (DMClient)this.Tag;
            DataTable dt = BbQuery.getAttempts(course_pk, dmc.origUrl, dmc.token); 
          //  atGrid.AutoGenerateColumns = false;
            atGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            atGrid.DataSource = dt;
            atGrid.Columns["attempt_date"].DefaultCellStyle.Format = "d";
            atGrid.Columns["score"].DefaultCellStyle.Format = "N2";
            atGrid.Columns["score"].ValueType = typeof(Single);

        }

        private void atGrid_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            AttemptRecord atr = new AttemptRecord();
            atr.Show();
        }
    }
}
