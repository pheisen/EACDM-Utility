using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using tempuri.org.Clients.xsd;
using System.IO;
using System.Threading;
using System.Collections;
using QTIUtility;

namespace EACDMLinqUtiliy
{

    public partial class Form1 : Form
    {
        private bool dirty = false;
        private string dataFile = null;//@"C:\utility\EACDMUtiliy\EACDM Utility\EACDMLinqUtiliy\UtilityData\clientData.xml";
        public Clients c;
        public Form1()
        {
            InitializeComponent();

          //  string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            dataFile = "clientData.xml";
            
            c = Clients.Load(dataFile);
            dirty = false;
            PopulateComboBox();
#if(mil)
            bntLoginMil.Visible = true;

#else
          //  bntLoginMil.Visible = false;;
#endif

        }

        private void PopulateComboBox()
        {
            cbClients.Items.Clear();
            var r = c.DMClients.OrderBy(t => t.Name).AsEnumerable();
            foreach (var t in r)
            {
                
                cbClients.Items.Add(new myListItem(t.id, t.Name));
            }
            cbClients.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // get selected index and the id
            Cursor.Current = Cursors.WaitCursor;
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            string token = dmc.getToken();
            if (token != null && (c.DMClients.Single(t => t.id == dmc.id).token == null || !c.DMClients.Single(t => t.id == dmc.id).token.Equals(token)))
            {
                c.DMClients.Single(t => t.id == dmc.id).token = token;
                Save();
            }
            tbMemo.AppendText(token); //t(dmc.getToken() + Environment.NewLine);
            Cursor.Current = Cursors.Default;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            string html = dmc.listFiles();
            tbMemo.AppendText(html);
            Cursor.Current = Cursors.Default;
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            tbMemo.AppendText(dmc.listProperties()); //t(dmc.getToken() + Environment.NewLine);
            Cursor.Current = Cursors.Default;

        }

        private void btnClear_Click(object sender, EventArgs e)
        {

            tbMemo.Clear();
        }

        private void btnUser_Click(object sender, EventArgs e)
        {
            UserForm uf = new UserForm();
            string firstname = "PPPPPPPPPPPPPPPPP";
            string lastname = "PPPPPPPPPPPPPPPPPPPP";
            DialogResult dr = uf.ShowDialog();
            if (dr == DialogResult.OK)
            {
                firstname = uf.tbFirstName.Text;
                lastname = uf.tbLastName.Text;

            }
            uf.Dispose();
            if (dr == DialogResult.OK)
            {
                Cursor.Current = Cursors.WaitCursor;
                DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
                tbMemo.AppendText(dmc.getUser(firstname, lastname)); //t(dmc.getToken() + Environment.NewLine);
                Cursor.Current = Cursors.Default;

            }
        }
        private void Save()
        {
            ssLb1.Text = "Saving to database...";
            Application.DoEvents();
            c.Save(dataFile);
            Thread.Sleep(500);
            ssLb1.Text = "";
            Application.DoEvents();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddEdit ae = new AddEdit();
            DMClient dmc = new DMClient();
            string btnName = (sender as Button).Name;
            if (btnName.Equals("btnAdd"))
            {
                ae.Text = "Add Record";
                ae.button1.Text = "Add";
            }
            else
            {
                ae.Text = "Edit Record";
                ae.button1.Text = "Save";
                dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
                ae.tbName.Text = dmc.name;
                ae.tbUrl.Text = dmc.origUrl;

            }
            DialogResult dr = ae.ShowDialog();
            if (dr == DialogResult.OK)
            {
                // save or create

                if (btnName.Equals("btnEdit"))
                {
                    bool dirty = false;
                    if (!dmc.name.Equals(ae.tbName.Text.Trim()))
                    {
                        dmc.name = ae.tbName.Text.Trim();
                        c.DMClients.Single(t => t.id == dmc.id).Name = dmc.name;
                        myListItem nItem = new myListItem(dmc.id, dmc.name);
                        cbClients.Items[cbClients.SelectedIndex] = nItem;
                        cbClients.Update();
                        Application.DoEvents();
                        dirty = true;
                    }
                    if (!dmc.origUrl.Equals(ae.tbUrl.Text.Trim()))
                    {
                        string baseUrl = ae.tbUrl.Text.Trim();
                        // check if only base
                        if (!(baseUrl.Contains("moodle") || baseUrl.EndsWith("php")))
                        {
                            if (!(baseUrl.Contains("webapp") || baseUrl.EndsWith(".aspx")))
                            {
                                baseUrl += DMClient.pluginStem + DMClient.xgetFile;
                                dmc.origUrl = baseUrl;
                            }
                        }
                        dmc.origUrl = baseUrl;
                        c.DMClients.Single(t => t.id == dmc.id).Url = dmc.origUrl;
                        dirty = true;


                    }

                    if (dirty)
                    {
                        dmc.modified = DateTime.Now;
                        c.DMClients.Single(t => t.id == dmc.id).modified = dmc.modified;
                        Save();
                    }

                }
                else // add
                {
                    dmc.name = ae.tbName.Text.Trim();
                    string baseUrl = ae.tbUrl.Text.Trim();
                    // check if only base
                    if (!baseUrl.Contains("moodle"))
                    {
                        if (!(baseUrl.Contains("webapp") || baseUrl.EndsWith(".aspx")))
                        {
                            baseUrl += DMClient.pluginStem + DMClient.xgetFile;
                            dmc.origUrl = baseUrl;
                        }
                    }
                    dmc.origUrl = baseUrl;
                    dmc.created = DateTime.Now;
                    dmc.modified = dmc.created;
                    int id = c.DMClients.Max(t => t.id);
                    dmc.id = id + 1;
                    Clients.DMClientsLocalType newClient = new Clients.DMClientsLocalType();
                    newClient.id = dmc.id;
                    newClient.Name = dmc.name;
                    newClient.created = dmc.created;
                    newClient.modified = dmc.modified;
                    newClient.Url = dmc.origUrl;
                    c.DMClients.Add(newClient);
                    Save();
                    PopulateComboBox();
                    foreach (myListItem o in cbClients.Items)
                    {
                        if (o.name.Equals(dmc.name))
                        {
                            cbClients.SelectedItem = o;
                        }

                    }
                    cbClients.Update();


                }

            }

        }

        private void mnSql_Click(object sender, EventArgs e)
        {
            SqlFormWeb sf = new SqlFormWeb();
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            sf.Text = dmc.pluginUrl;
            sf.Tag = dmc;
            //  sf.ShowDialog();
            sf.Show();
        }
        private void outcomesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //CatsForm cat = new CatsForm();
            //DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            //cat.Text = dmc.pluginUrl;
            //cat.Tag = dmc;
            //cat.Show();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            BbQuery.cc = new System.Net.CookieContainer();
            BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
            {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
               // Login lg = new Login();
                BbQuery.getBbParams();
                LoginXP lg = new LoginXP();

                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            GC.Collect();

        }
        private void btnLogNB_Click(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            BbQuery.cc = new System.Net.CookieContainer();
            BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
            {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
                // Login lg = new Login();
                BbQuery.getBbParams();
                LoginXPXB lg = new LoginXPXB();

                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            GC.Collect();

        }


        private void btnLogout_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            BbQuery.BbLogout();
            Cursor.Current = Cursors.Default;
        }

        private void gradebookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Gradebook gr = new Gradebook();
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            gr.Text = dmc.pluginUrl;
            gr.Tag = dmc;
            gr.Show();

        }

        private void speicalCoursesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialCourse sp = new SpecialCourse();
            sp.Show();
        }

        private void managePreceptorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Preceptors pr = new Preceptors();
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            // pr.Text = dmc.pluginUrl;
            pr.Tag = dmc;
            pr.Show();

        }

        private void btnMD5_Click(object sender, EventArgs e)
        {
            MD5Maker m = new MD5Maker();
            m.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            LoginAuth2Test la = new LoginAuth2Test();
            la.ShowDialog();
        }

        private void bntLoginMil_Click(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            //BbQuery.cc = new System.Net.CookieContainer();
            //BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
           {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
                //LoginMil lg = new LoginMil();
                //lg.ShowDialog();
                //lg.Dispose();
                //lg = null;
            }
            GC.Collect();
        }

        private void btnMil2_Click(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            BbQuery.cc = new System.Net.CookieContainer();
            BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
            {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
                //LoginMil2 lg = new LoginMil2();
                //lg.ShowDialog();
                //lg.Dispose();
                //lg = null;
            }
            GC.Collect();
        }

        private void btnLoginMil3_Click(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            BbQuery.cc = new System.Net.CookieContainer();
            BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
            {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
                //LoginMil3 lg = new LoginMil3();
                //lg.ShowDialog();
                //lg.Dispose();
                //lg = null;
            }
            GC.Collect();
        }

        private void btnLewisLogin_Click(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            BbQuery.cc = new System.Net.CookieContainer();
            BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
            {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
                LewisLogin lg = new LewisLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            GC.Collect();
        }

        private void btnLoginMobile_Click(object sender, EventArgs e)
        {

            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            BbQuery.cc = new System.Net.CookieContainer();
            BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
            {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
                LoginMobile lg = new LoginMobile();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            GC.Collect();

        }

        private void btnLoginX_Click(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            BbQuery.cc = new System.Net.CookieContainer();
            BbQuery.cc.Capacity = 1000;
            BbQuery.BbUrl = dmc.origUrl;
            if (BbQuery.isMoodle(BbQuery.BbUrl))
            {
                MoodleLogin lg = new MoodleLogin();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            else
            {
                LoginX lg = new LoginX();
                lg.ShowDialog();
                lg.Dispose();
                lg = null;
            }
            GC.Collect();
        }

        private void btnScripting_Click(object sender, EventArgs e)
        {
            Script s = new Script();
            s.ShowDialog();
        }

        private void btnRegex_Click(object sender, EventArgs e)
        {
            RegexTest r = new RegexTest();
            r.Show();
        }

        private void btnGetSurvey_Click(object sender, EventArgs e)
        {
            Sauce s = new Sauce();
            s.Show();
        }

        private void bntStudents_Click(object sender, EventArgs e)
        {
            StudentSurveys sf = new StudentSurveys();
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            sf.Text = dmc.pluginUrl;
            sf.Tag = dmc;
            sf.Show();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DMClient dmc = new DMClient(((myListItem)cbClients.SelectedItem).id, c);
            UpdateFile uf = new UpdateFile();
            uf.dmc = dmc;
            uf.ShowDialog();
        }

        private void btnUrlencode_Click(object sender, EventArgs e)
        {
            var url = new Urlencode();
            url.Show();
        }

        private void btnMCats_Click(object sender, EventArgs e)
        {
            //var mcats = new MoodleCats();
            //mcats.Show();
        }

        

        

    }

    public class myListItem
    {
        public int id;
        public string name;
        public myListItem(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
        public override string ToString()
        {
            return this.name;
        }
    }
}
