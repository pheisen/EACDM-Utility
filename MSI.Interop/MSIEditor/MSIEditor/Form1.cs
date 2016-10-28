using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MSI = Pahvant.MSI;

namespace MSIEditor
{
    public partial class Form1 : Form
    {
        private string msifile = @"C:\Program Files\Educational Assessments Corporation\EACZipInstallerMaker\CurrentMsi\EACOutcomes.msi";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbMemo.Clear();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            // open file
         //   MSI.Package mpkg = new MSI.Package(msifile);
            // list tables and records
           
            // WindowsInstaller.View view = database.OpenView("SELECT * FROM _table_name);
            MSI.Database mdb =   new MSI.Database(msifile, MSI.Database.OpenMode.ReadOnly);
            mdb.Export("registry", @"C:\Program Files\Educational Assessments Corporation\EACZipInstallerMaker\CurrentMsi\", "RegExport.txt");
            MSI.View mdbv = mdb.OpenView("SELECT * FROM Registry");
            mdbv.Execute();
            MSI.Record rmdb = mdbv.Fetch();
            int total = rmdb.FieldCount;
            while (rmdb != null)
            {
                for (int i = 1; i <= total; i++)
                {
                    if (rmdb.GetString(i) != null)
                    {
                        string temp = rmdb.GetString(i);
                      //  temp = temp.Replace(txtFind.Text, txtReplace.Text);
                       // record.set_StringData(i, temp);
                       // view.Modify(MsiViewModify.msiViewModifyReplace, record);
                        tbMemo.AppendText(temp);
                    }
                }
                rmdb = mdbv.Fetch();
            }
            // edit record
            // save changes and file
            
        }
        /*
        public void UpdateMsiData(string msiFilePath)
        {
            Type installerType = Type.GetTypeFromProgID("WindowsInstaller.Installer");
            Installer installer = (Installer)Activator.CreateInstance(installerType);
            database =
         installer.OpenDatabase(msiFilePath, MsiOpenDatabaseMode.msiOpenDatabaseModeTransact);
            // WindowsInstaller.View view = database.OpenView("SELECT * FROM _table_name);
            WindowsInstaller.View view = database.OpenView("SELECT * FROM control");

            view.Execute(null);
            Record record = view.Fetch();
            int total = record.FieldCount;
            while (record != null)
            {
                for (int i = 1; i <= total; i++)
                {
                    if (record.get_StringData(i).Contains(txtFind.Text))
                    {
                        string temp = record.get_StringData(i);
                        temp = temp.Replace(txtFind.Text, txtReplace.Text);
                        record.set_StringData(i, temp);
                        view.Modify(MsiViewModify.msiViewModifyReplace, record);
                    }
                }
                record = view.Fetch();
            }
            view.Close();
            database.Commit();
        }
         * */
    }
}
