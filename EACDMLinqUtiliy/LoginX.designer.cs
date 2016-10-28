using System.Windows.Forms;
namespace EACDMLinqUtiliy
{
    partial class LoginX
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginX));
            this.pnBottom = new System.Windows.Forms.Panel();
            this.btnGoToLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tsLabel1 = new System.Windows.Forms.Label();
            this.pnLogin = new System.Windows.Forms.Panel();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUser_id = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbUsername = new System.Windows.Forms.Label();
            this.pbBusy = new System.Windows.Forms.PictureBox();
            this.pnBottom.SuspendLayout();
            this.pnLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBusy)).BeginInit();
            this.SuspendLayout();
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnBottom.Controls.Add(this.pbBusy);
            this.pnBottom.Controls.Add(this.btnGoToLogin);
            this.pnBottom.Controls.Add(this.btnCancel);
            this.pnBottom.Controls.Add(this.tsLabel1);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 666);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(911, 45);
            this.pnBottom.TabIndex = 0;
            // 
            // btnGoToLogin
            // 
            this.btnGoToLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGoToLogin.Location = new System.Drawing.Point(665, 8);
            this.btnGoToLogin.Name = "btnGoToLogin";
            this.btnGoToLogin.Size = new System.Drawing.Size(121, 29);
            this.btnGoToLogin.TabIndex = 0;
            this.btnGoToLogin.TabStop = false;
            this.btnGoToLogin.Text = "Go to Login Page";
            this.btnGoToLogin.UseVisualStyleBackColor = true;
            this.btnGoToLogin.Visible = false;
            this.btnGoToLogin.Click += new System.EventHandler(this.btnGoToLogin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(812, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tsLabel1
            // 
            this.tsLabel1.AutoSize = true;
            this.tsLabel1.Location = new System.Drawing.Point(13, 13);
            this.tsLabel1.Name = "tsLabel1";
            this.tsLabel1.Size = new System.Drawing.Size(0, 16);
            this.tsLabel1.TabIndex = 0;
            // 
            // pnLogin
            // 
            this.pnLogin.Controls.Add(this.wb);
            this.pnLogin.Controls.Add(this.btnLogin);
            this.pnLogin.Controls.Add(this.tbPassword);
            this.pnLogin.Controls.Add(this.tbUser_id);
            this.pnLogin.Controls.Add(this.lbPassword);
            this.pnLogin.Controls.Add(this.lbUsername);
            this.pnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLogin.Location = new System.Drawing.Point(0, 0);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Size = new System.Drawing.Size(911, 666);
            this.pnLogin.TabIndex = 0;
            // 
            // wb
            // 
            this.wb.Location = new System.Drawing.Point(16, 220);
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.ScriptErrorsSuppressed = true;
            this.wb.Size = new System.Drawing.Size(871, 400);
            this.wb.TabIndex = 0;
            this.wb.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.Location = new System.Drawing.Point(157, 155);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(89, 35);
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(154, 104);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(226, 23);
            this.tbPassword.TabIndex = 2;
            // 
            // tbUser_id
            // 
            this.tbUser_id.Location = new System.Drawing.Point(154, 57);
            this.tbUser_id.Name = "tbUser_id";
            this.tbUser_id.Size = new System.Drawing.Size(226, 23);
            this.tbUser_id.TabIndex = 1;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(60, 107);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(63, 16);
            this.lbPassword.TabIndex = 1;
            this.lbPassword.Text = "Password";
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Location = new System.Drawing.Point(57, 62);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(66, 16);
            this.lbUsername.TabIndex = 0;
            this.lbUsername.Text = "Username";
            // 
            // pbBusy
            // 
            this.pbBusy.Image = ((System.Drawing.Image)(resources.GetObject("pbBusy.Image")));
            this.pbBusy.Location = new System.Drawing.Point(235, 12);
            this.pbBusy.Name = "pbBusy";
            this.pbBusy.Size = new System.Drawing.Size(20, 20);
            this.pbBusy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbBusy.TabIndex = 51;
            this.pbBusy.TabStop = false;
            this.pbBusy.Visible = false;
            // 
            // LoginX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(911, 711);
            this.Controls.Add(this.pnLogin);
            this.Controls.Add(this.pnBottom);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LoginX";
            this.Text = "Login";
            this.Activated += new System.EventHandler(this.LoginX_Activated);
            this.Load += new System.EventHandler(this.Login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Login_KeyDown);
            this.pnBottom.ResumeLayout(false);
            this.pnBottom.PerformLayout();
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBusy)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnBottom;
        private Button btnCancel;
        private Label tsLabel1;
        private Button btnGoToLogin;
        private Panel pnLogin;
        private Button btnLogin;
        private TextBox tbPassword;
        private TextBox tbUser_id;
        private Label lbPassword;
        private Label lbUsername;
        public WebBrowser wb;
        public PictureBox pbBusy;


    }
}