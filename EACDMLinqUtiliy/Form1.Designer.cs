namespace EACDMLinqUtiliy
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ssLb1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.mnFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mnSql = new System.Windows.Forms.ToolStripMenuItem();
            this.outcomesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gradebookToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speicalCoursesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managePreceptorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnBackground = new System.Windows.Forms.Panel();
            this.btnLogNB = new System.Windows.Forms.Button();
            this.btnMCats = new System.Windows.Forms.Button();
            this.btnUrlencode = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.bntStudents = new System.Windows.Forms.Button();
            this.btnRegex = new System.Windows.Forms.Button();
            this.btnLoginX = new System.Windows.Forms.Button();
            this.btnScripting = new System.Windows.Forms.Button();
            this.btnLoginMobile = new System.Windows.Forms.Button();
            this.btnLewisLogin = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btnGetSurvey = new System.Windows.Forms.Button();
            this.btnMD5 = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnUser = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnProperties = new System.Windows.Forms.Button();
            this.tbMemo = new System.Windows.Forms.TextBox();
            this.btnFiles = new System.Windows.Forms.Button();
            this.btnToken = new System.Windows.Forms.Button();
            this.cbClients = new System.Windows.Forms.ComboBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.pnBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ssLb1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 1132);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 21, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1934, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // ssLb1
            // 
            this.ssLb1.Name = "ssLb1";
            this.ssLb1.Size = new System.Drawing.Size(0, 17);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnFile,
            this.mnTools});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(9, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1934, 32);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // mnFile
            // 
            this.mnFile.Name = "mnFile";
            this.mnFile.Size = new System.Drawing.Size(53, 28);
            this.mnFile.Text = "File";
            // 
            // mnTools
            // 
            this.mnTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnSql,
            this.outcomesToolStripMenuItem,
            this.gradebookToolStripMenuItem,
            this.speicalCoursesToolStripMenuItem,
            this.managePreceptorsToolStripMenuItem});
            this.mnTools.Name = "mnTools";
            this.mnTools.Size = new System.Drawing.Size(70, 28);
            this.mnTools.Text = "Tools";
            // 
            // mnSql
            // 
            this.mnSql.Name = "mnSql";
            this.mnSql.Size = new System.Drawing.Size(268, 28);
            this.mnSql.Text = "SQL...";
            this.mnSql.Click += new System.EventHandler(this.mnSql_Click);
            // 
            // outcomesToolStripMenuItem
            // 
            this.outcomesToolStripMenuItem.Name = "outcomesToolStripMenuItem";
            this.outcomesToolStripMenuItem.Size = new System.Drawing.Size(268, 28);
            this.outcomesToolStripMenuItem.Text = "Outcomes...";
            this.outcomesToolStripMenuItem.Click += new System.EventHandler(this.outcomesToolStripMenuItem_Click);
            // 
            // gradebookToolStripMenuItem
            // 
            this.gradebookToolStripMenuItem.Name = "gradebookToolStripMenuItem";
            this.gradebookToolStripMenuItem.Size = new System.Drawing.Size(268, 28);
            this.gradebookToolStripMenuItem.Text = "Gradebook...";
            this.gradebookToolStripMenuItem.Click += new System.EventHandler(this.gradebookToolStripMenuItem_Click);
            // 
            // speicalCoursesToolStripMenuItem
            // 
            this.speicalCoursesToolStripMenuItem.Name = "speicalCoursesToolStripMenuItem";
            this.speicalCoursesToolStripMenuItem.Size = new System.Drawing.Size(268, 28);
            this.speicalCoursesToolStripMenuItem.Text = "Special Courses...";
            this.speicalCoursesToolStripMenuItem.Click += new System.EventHandler(this.speicalCoursesToolStripMenuItem_Click);
            // 
            // managePreceptorsToolStripMenuItem
            // 
            this.managePreceptorsToolStripMenuItem.Name = "managePreceptorsToolStripMenuItem";
            this.managePreceptorsToolStripMenuItem.Size = new System.Drawing.Size(268, 28);
            this.managePreceptorsToolStripMenuItem.Text = "Manage Preceptors...";
            this.managePreceptorsToolStripMenuItem.Click += new System.EventHandler(this.managePreceptorsToolStripMenuItem_Click);
            // 
            // pnBackground
            // 
            this.pnBackground.BackColor = System.Drawing.Color.Transparent;
            this.pnBackground.Controls.Add(this.btnLogNB);
            this.pnBackground.Controls.Add(this.btnMCats);
            this.pnBackground.Controls.Add(this.btnUrlencode);
            this.pnBackground.Controls.Add(this.button2);
            this.pnBackground.Controls.Add(this.bntStudents);
            this.pnBackground.Controls.Add(this.btnRegex);
            this.pnBackground.Controls.Add(this.btnLoginX);
            this.pnBackground.Controls.Add(this.btnScripting);
            this.pnBackground.Controls.Add(this.btnLoginMobile);
            this.pnBackground.Controls.Add(this.btnLewisLogin);
            this.pnBackground.Controls.Add(this.button1);
            this.pnBackground.Controls.Add(this.btnGetSurvey);
            this.pnBackground.Controls.Add(this.btnMD5);
            this.pnBackground.Controls.Add(this.btnLogout);
            this.pnBackground.Controls.Add(this.btnLogin);
            this.pnBackground.Controls.Add(this.btnAdd);
            this.pnBackground.Controls.Add(this.btnEdit);
            this.pnBackground.Controls.Add(this.btnUser);
            this.pnBackground.Controls.Add(this.btnClear);
            this.pnBackground.Controls.Add(this.btnProperties);
            this.pnBackground.Controls.Add(this.tbMemo);
            this.pnBackground.Controls.Add(this.btnFiles);
            this.pnBackground.Controls.Add(this.btnToken);
            this.pnBackground.Controls.Add(this.cbClients);
            this.pnBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBackground.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnBackground.Location = new System.Drawing.Point(0, 32);
            this.pnBackground.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnBackground.Name = "pnBackground";
            this.pnBackground.Size = new System.Drawing.Size(1934, 1100);
            this.pnBackground.TabIndex = 2;
            // 
            // btnLogNB
            // 
            this.btnLogNB.Location = new System.Drawing.Point(1213, 61);
            this.btnLogNB.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLogNB.Name = "btnLogNB";
            this.btnLogNB.Size = new System.Drawing.Size(112, 35);
            this.btnLogNB.TabIndex = 28;
            this.btnLogNB.Text = "Login NB";
            this.btnLogNB.UseVisualStyleBackColor = true;
            this.btnLogNB.Click += new System.EventHandler(this.btnLogNB_Click);
            // 
            // btnMCats
            // 
            this.btnMCats.Location = new System.Drawing.Point(1530, 4);
            this.btnMCats.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMCats.Name = "btnMCats";
            this.btnMCats.Size = new System.Drawing.Size(166, 35);
            this.btnMCats.TabIndex = 27;
            this.btnMCats.Text = "Moddle Cats";
            this.btnMCats.UseVisualStyleBackColor = true;
            this.btnMCats.Click += new System.EventHandler(this.btnMCats_Click);
            // 
            // btnUrlencode
            // 
            this.btnUrlencode.Location = new System.Drawing.Point(1019, 62);
            this.btnUrlencode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUrlencode.Name = "btnUrlencode";
            this.btnUrlencode.Size = new System.Drawing.Size(177, 35);
            this.btnUrlencode.TabIndex = 26;
            this.btnUrlencode.Text = "Urlencode";
            this.btnUrlencode.UseVisualStyleBackColor = true;
            this.btnUrlencode.Click += new System.EventHandler(this.btnUrlencode_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(830, 61);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(177, 35);
            this.button2.TabIndex = 25;
            this.button2.Text = "Update File";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // bntStudents
            // 
            this.bntStudents.AutoSize = true;
            this.bntStudents.Location = new System.Drawing.Point(37, 61);
            this.bntStudents.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bntStudents.Name = "bntStudents";
            this.bntStudents.Size = new System.Drawing.Size(184, 42);
            this.bntStudents.TabIndex = 24;
            this.bntStudents.Text = "Student Surveys";
            this.bntStudents.UseVisualStyleBackColor = true;
            this.bntStudents.Click += new System.EventHandler(this.bntStudents_Click);
            // 
            // btnRegex
            // 
            this.btnRegex.Location = new System.Drawing.Point(462, 62);
            this.btnRegex.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRegex.Name = "btnRegex";
            this.btnRegex.Size = new System.Drawing.Size(177, 35);
            this.btnRegex.TabIndex = 23;
            this.btnRegex.Text = "Regex Test";
            this.btnRegex.UseVisualStyleBackColor = true;
            this.btnRegex.Click += new System.EventHandler(this.btnRegex_Click);
            // 
            // btnLoginX
            // 
            this.btnLoginX.Location = new System.Drawing.Point(1784, 61);
            this.btnLoginX.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoginX.Name = "btnLoginX";
            this.btnLoginX.Size = new System.Drawing.Size(112, 35);
            this.btnLoginX.TabIndex = 22;
            this.btnLoginX.Text = "Login X";
            this.btnLoginX.UseVisualStyleBackColor = true;
            this.btnLoginX.Click += new System.EventHandler(this.btnLoginX_Click);
            // 
            // btnScripting
            // 
            this.btnScripting.Location = new System.Drawing.Point(645, 62);
            this.btnScripting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnScripting.Name = "btnScripting";
            this.btnScripting.Size = new System.Drawing.Size(177, 35);
            this.btnScripting.TabIndex = 21;
            this.btnScripting.Text = "Scrpting Test";
            this.btnScripting.UseVisualStyleBackColor = true;
            this.btnScripting.Click += new System.EventHandler(this.btnScripting_Click);
            // 
            // btnLoginMobile
            // 
            this.btnLoginMobile.Location = new System.Drawing.Point(1596, 62);
            this.btnLoginMobile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLoginMobile.Name = "btnLoginMobile";
            this.btnLoginMobile.Size = new System.Drawing.Size(177, 35);
            this.btnLoginMobile.TabIndex = 20;
            this.btnLoginMobile.Text = "LoginMobile";
            this.btnLoginMobile.UseVisualStyleBackColor = true;
            this.btnLoginMobile.Click += new System.EventHandler(this.btnLoginMobile_Click);
            // 
            // btnLewisLogin
            // 
            this.btnLewisLogin.Location = new System.Drawing.Point(1404, 62);
            this.btnLewisLogin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLewisLogin.Name = "btnLewisLogin";
            this.btnLewisLogin.Size = new System.Drawing.Size(177, 35);
            this.btnLewisLogin.TabIndex = 19;
            this.btnLewisLogin.Text = "Lewis Login";
            this.btnLewisLogin.UseVisualStyleBackColor = true;
            this.btnLewisLogin.Click += new System.EventHandler(this.btnLewisLogin_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1719, 5);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(183, 35);
            this.button1.TabIndex = 15;
            this.button1.Text = "Test clients.mdb";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnGetSurvey
            // 
            this.btnGetSurvey.Location = new System.Drawing.Point(270, 65);
            this.btnGetSurvey.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGetSurvey.Name = "btnGetSurvey";
            this.btnGetSurvey.Size = new System.Drawing.Size(143, 35);
            this.btnGetSurvey.TabIndex = 14;
            this.btnGetSurvey.Text = "Get Sauce";
            this.btnGetSurvey.UseVisualStyleBackColor = true;
            this.btnGetSurvey.Click += new System.EventHandler(this.btnGetSurvey_Click);
            // 
            // btnMD5
            // 
            this.btnMD5.Location = new System.Drawing.Point(1377, 5);
            this.btnMD5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnMD5.Name = "btnMD5";
            this.btnMD5.Size = new System.Drawing.Size(112, 35);
            this.btnMD5.TabIndex = 13;
            this.btnMD5.Text = "Md5";
            this.btnMD5.UseVisualStyleBackColor = true;
            this.btnMD5.Click += new System.EventHandler(this.btnMD5_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(1256, 5);
            this.btnLogout.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(112, 35);
            this.btnLogout.TabIndex = 12;
            this.btnLogout.Text = "Logout";
            this.btnLogout.UseVisualStyleBackColor = true;
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(1137, 5);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(112, 35);
            this.btnLogin.TabIndex = 11;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(1018, 5);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(112, 35);
            this.btnAdd.TabIndex = 10;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(900, 5);
            this.btnEdit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(112, 35);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUser
            // 
            this.btnUser.Location = new System.Drawing.Point(782, 5);
            this.btnUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnUser.Name = "btnUser";
            this.btnUser.Size = new System.Drawing.Size(112, 35);
            this.btnUser.TabIndex = 8;
            this.btnUser.Text = "User";
            this.btnUser.UseVisualStyleBackColor = true;
            this.btnUser.Click += new System.EventHandler(this.btnUser_Click);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(42, 1026);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(112, 35);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnProperties
            // 
            this.btnProperties.Location = new System.Drawing.Point(663, 5);
            this.btnProperties.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(112, 35);
            this.btnProperties.TabIndex = 6;
            this.btnProperties.Text = "Properties";
            this.btnProperties.UseVisualStyleBackColor = true;
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // tbMemo
            // 
            this.tbMemo.AcceptsReturn = true;
            this.tbMemo.AcceptsTab = true;
            this.tbMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemo.Location = new System.Drawing.Point(42, 134);
            this.tbMemo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbMemo.Multiline = true;
            this.tbMemo.Name = "tbMemo";
            this.tbMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMemo.Size = new System.Drawing.Size(1852, 898);
            this.tbMemo.TabIndex = 5;
            // 
            // btnFiles
            // 
            this.btnFiles.Location = new System.Drawing.Point(544, 5);
            this.btnFiles.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnFiles.Name = "btnFiles";
            this.btnFiles.Size = new System.Drawing.Size(112, 35);
            this.btnFiles.TabIndex = 4;
            this.btnFiles.Text = "Get Files";
            this.btnFiles.UseVisualStyleBackColor = true;
            this.btnFiles.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnToken
            // 
            this.btnToken.Location = new System.Drawing.Point(426, 5);
            this.btnToken.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnToken.Name = "btnToken";
            this.btnToken.Size = new System.Drawing.Size(112, 35);
            this.btnToken.TabIndex = 2;
            this.btnToken.Text = "Get Token";
            this.btnToken.UseVisualStyleBackColor = true;
            this.btnToken.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbClients
            // 
            this.cbClients.FormattingEnabled = true;
            this.cbClients.Location = new System.Drawing.Point(37, 5);
            this.cbClients.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbClients.Name = "cbClients";
            this.cbClients.Size = new System.Drawing.Size(376, 32);
            this.cbClients.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1934, 1154);
            this.Controls.Add(this.pnBackground);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "EACDM Utiliy";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnBackground.ResumeLayout(false);
            this.pnBackground.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel pnBackground;
        private System.Windows.Forms.ComboBox cbClients;
        private System.Windows.Forms.Button btnToken;
        private System.Windows.Forms.Button btnFiles;
        private System.Windows.Forms.TextBox tbMemo;
        private System.Windows.Forms.Button btnProperties;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnUser;
        private System.Windows.Forms.ToolStripStatusLabel ssLb1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.ToolStripMenuItem mnFile;
        private System.Windows.Forms.ToolStripMenuItem mnTools;
        private System.Windows.Forms.ToolStripMenuItem mnSql;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.ToolStripMenuItem outcomesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gradebookToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speicalCoursesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managePreceptorsToolStripMenuItem;
        private System.Windows.Forms.Button btnMD5;
        private System.Windows.Forms.Button btnGetSurvey;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnLewisLogin;
        private System.Windows.Forms.Button btnLoginMobile;
        private System.Windows.Forms.Button btnScripting;
        private System.Windows.Forms.Button btnLoginX;
        private System.Windows.Forms.Button btnRegex;
        private System.Windows.Forms.Button bntStudents;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnUrlencode;
        private System.Windows.Forms.Button btnMCats;
        private System.Windows.Forms.Button btnLogNB;
    }
}

