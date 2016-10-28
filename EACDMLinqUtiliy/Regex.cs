using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using QTIUtility;

using System.Windows.Forms;

namespace EACDMLinqUtiliy
{
    public partial class RegexTest : Form
    {
        public RegexTest()
        {
            InitializeComponent();//(^\w{2})(?=-)(-[a-z]\w{1,})+(?=-)((-[a-z]\w{1,}(-\d\w{2,})+)|(-\d\w{2,}))+(?=-)(-\d{2})+(?=-)(-\d{6})
            tbPattern.Text = @"(^\w{2})(?=-)(-[a-z]\w{1,})+(?=-)((-[a-z]\w{1,}(-\d\w{2,})+)|(-\d\w{2,}))+(?=-)(-\d{2})+(?=-)(-\d{6})";
            tbInput.Text = "ED-GECE-631-GELP-556-GSCE-631-61-201210";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbInput.Text) || string.IsNullOrEmpty(tbPattern.Text))
            {
                MessageBox.Show("Fill input boxes.");
                return;
            }
           //  getCQCourses(tbInput.Text.Trim())
            tbMemo.AppendText(BbQuery.getCQCourses(tbInput.Text.Trim()) + "\n\n");
            try
            {
                Regex rgx = new Regex(tbPattern.Text.Trim(), RegexOptions.IgnoreCase);
                MatchCollection mc = rgx.Matches(tbInput.Text.Trim());
                tbMemo.AppendText(tbInput.Text.Trim() + " yields " + mc.Count.ToString() + " matches\n");
                tbMemo.AppendText("school: " + mc[0].Groups[1].Captures[0].Value + "\n");
                string schl = mc[0].Groups[1].Captures[0].Value;
                int l = mc[0].Groups.Count - 2;
                tbMemo.AppendText("tail: " + mc[0].Groups[l+1].Captures[0].Value + "\n");
                string tail = mc[0].Groups[l+1].Captures[0].Value;
                string groups = mc[0].Groups.Count.ToString();
                string[] sections = new string[mc[0].Groups[l].Captures.Count];
                for (int z = 0; z < mc[0].Groups[l].Captures.Count;z++ )
                {
                    sections[z] = mc[0].Groups[l].Captures[z].Value;
                }
                tbMemo.AppendText("Groups: " + groups+"\n");
                StringBuilder sb = new StringBuilder();
                string[] delim = {"-"};
                List<string> doubles = new List<string>();
                List<string> singles = new List<string>();
                for (int g = 0; g < mc[0].Groups.Count; g++)
                {
                   // tbMemo.AppendText("g: " +g.ToString()+" "+ mc[0].Groups[g].Value + "\n");
                    for (int c = 0; c < mc[0].Groups[g].Captures.Count; c++)
                    {
                        Capture cc = mc[0].Groups[g].Captures[c];
                        tbMemo.AppendText("g: " + g.ToString() + " c: "+c.ToString()+" " +cc.Value + " "+cc.Index.ToString()+" "+cc.Length.ToString()+ "\n");
                        if (g >= 2 && g < (mc[0].Groups.Count - 2))
                        {
                            if ((cc.Value.Split('-')).Length > 2)
                            {
                                if (!doubles.Contains(cc.Value))
                                {
                                    doubles.Add(cc.Value);
                                    tbMemo.AppendText("double: " + cc.Value + "\n");
                                }
                            }
                            else
                            {
                                if (!singles.Contains(cc.Value))
                                {
                                    singles.Add(cc.Value);
                                    tbMemo.AppendText("single: " + cc.Value + "\n");
                                }
                            }
                        }

                    }
                }
                foreach (string d in doubles)
                {
                    foreach (string s in sections)
                    {
                        sb.Append("," + schl + d + s + tail);
                    }
                }
                for (int p = 0; p < singles.Count-1; p++)
                {
                    for (int q = 1; q < singles.Count; q++)
                    {
                        if (p != q)
                        {
                            if (!doubles.Contains(singles[p]+"-"+singles[q]))
                            {
                                foreach (string s in sections)
                                {
                                    sb.Append("," + schl + singles[p] + "-" + singles[q] + s + tail);
                                }
                            }
                        }
                    }
                }
                string bigCourseid = sb.ToString();
                tbMemo.AppendText(bigCourseid + "\n");
            }
            catch (Exception ex)
            {
                tbMemo.AppendText(ex.Message + "\n");
            }

        }
    }
}
