using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EACDMLinqUtiliy
{
    public partial class StudentSurveys : Form
    {
        private string BbShadowCourseId = "EACD_";
        private string BbShadowCoursePrefix = "EACD_";
        private string ft = "01/01/2050";
        private DMClient dmc;
        public StudentSurveys()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            dmc = (DMClient)this.Tag;
            string token = dmc.token;
            string tbSql = null;
            // sql goes here
            StringBuilder sb = new StringBuilder();
            sb.Append("select c.pk1 as cpk1,c.course_name,c.course_id,c.dtmodified as cmod,cnt.pk1 as cntpk1, cnt.title,cnt.start_date,cnt.end_date, ca.pk1 as capk1 ");
            sb.Append("  ,(case  when cnt.end_date is null then {d '" + ft + "'}  else cnt.end_date  end) as sort_date");
            // sb.Append(" ,(select pk1 from course_main where pk1 = replace(c.course_id,'" + BbShadowCoursePrefix + "','')) as underlyingPk1 ");

            sb.Append("  ,(case when (select count(pk1) from course_main ");
            sb.Append(" where pk1 = replace(c.course_id,'" + BbShadowCoursePrefix + "','')) > 0 then  ");
            sb.Append(" (select pk1 from course_main where pk1 = replace(c.course_id,'" + BbShadowCoursePrefix + "','')) ");
            sb.Append(" else c.pk1  end) as underlyingpk1  ");
            sb.Append(" ,ca.multiple_attempts_ind as m_ind ");
            sb.Append(" ,(select count(a.pk1) from attempt a inner join gradebook_grade gb on a.gradebook_grade_pk1 = gb.pk1 where gb.gradebook_main_pk1 = gm.pk1 and gb.course_users_pk1 = cu.pk1) as acount ");

            sb.Append(" ,(select min(a.status) from attempt a inner join gradebook_grade gb on a.gradebook_grade_pk1 = gb.pk1 where gb.gradebook_main_pk1 = gm.pk1 and gb.course_users_pk1 = cu.pk1 ) as status ");
            sb.Append(" ,cu.available_ind,cu.row_status ");
            sb.Append(" from course_main c inner join course_contents cnt on cnt.crsmain_pk1 = c.pk1 ");
            sb.Append("  inner join course_assessment ca on ca.pk1 = (select link_source_pk1 from link where course_contents_pk1 = cnt.pk1)  ");
            sb.Append(" inner join course_users cu on cu.crsmain_pk1 = c.pk1 ");
            sb.Append(" inner join users u on u.pk1 = cu.users_pk1 ");
            sb.Append(" inner join gradebook_main gm on gm.qti_asi_data_pk1 = ca.qti_asi_data_pk1 and gm.crsmain_pk1 = c.pk1");
            sb.Append(" where u.user_id = '" + tbUsername.Text.Trim() + "' and cnt.CNTHNDLR_HANDLE  = 'resource/x-bb-asmt-survey-link' ");
            sb.Append(" and cnt.AVAILABLE_IND = 'Y' and cu.role = 'S' ");
          //  sb.Append(" and cu.available_ind = 'Y' and cu.row_status = 0 ");
            sb.Append("and (c.course_id like '" + BbShadowCourseId + "%' escape '!') "); // for only shadow courses
          //  sb.Append(" and ((cnt.end_date is null) or (not cnt.end_date is null and cnt.end_date >= {ts '" + mt + "'})) ");
          //  sb.Append(" and ((cnt.start_date is null) or (not cnt.start_date is null and cnt.start_date <= {ts '" + mt + "'})) ");
            sb.Append(" order by sort_date,cnt.title,c.course_name,c.course_id");
            tbSql = sb.ToString();

            tbMemo.AppendText(tbSql + Environment.NewLine);

            if (!String.IsNullOrEmpty(tbSql))
            {
                if (token == null || token.Equals(""))
                {
                    dmc.token = dmc.getToken();
                }
            }
         //   QTIUtility.Logger.__SpecialLogger("SQL: " + dmc.name + Environment.NewLine + tbSql.Text + Environment.NewLine + "-- end --" + Environment.NewLine);
            tbMemo.AppendText( DMClient.getSql(tbSql, dmc));
        }
    }
}
