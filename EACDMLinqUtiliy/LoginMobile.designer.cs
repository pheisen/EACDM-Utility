using System.Windows.Forms;
namespace EACDMLinqUtiliy
{
    partial class LoginMobile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginMobile));
            this.pnBottom = new System.Windows.Forms.Panel();
            this.btnGoToLogin = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tsStatus = new System.Windows.Forms.Label();
            this.pnTop = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUser_id = new System.Windows.Forms.TextBox();
            this.pnTextbox = new System.Windows.Forms.Panel();
            this.tbMemo = new System.Windows.Forms.TextBox();
            this.pnBottom.SuspendLayout();
            this.pnTop.SuspendLayout();
            this.pnTextbox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.SystemColors.Control;
            this.pnBottom.Controls.Add(this.btnGoToLogin);
            this.pnBottom.Controls.Add(this.btnCancel);
            this.pnBottom.Controls.Add(this.tsStatus);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 802);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(920, 38);
            this.pnBottom.TabIndex = 0;
            // 
            // btnGoToLogin
            // 
            this.btnGoToLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGoToLogin.Location = new System.Drawing.Point(674, 3);
            this.btnGoToLogin.Name = "btnGoToLogin";
            this.btnGoToLogin.Size = new System.Drawing.Size(121, 29);
            this.btnGoToLogin.TabIndex = 2;
            this.btnGoToLogin.Text = "Go to Login Page";
            this.btnGoToLogin.UseVisualStyleBackColor = true;
            this.btnGoToLogin.Visible = false;
            this.btnGoToLogin.Click += new System.EventHandler(this.btnGoToLogin_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(821, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 29);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tsStatus
            // 
            this.tsStatus.AutoSize = true;
            this.tsStatus.Location = new System.Drawing.Point(13, 13);
            this.tsStatus.Name = "tsStatus";
            this.tsStatus.Size = new System.Drawing.Size(85, 16);
            this.tsStatus.TabIndex = 0;
            this.tsStatus.Text = "Please wait...";
            // 
            // pnTop
            // 
            this.pnTop.Controls.Add(this.btnClear);
            this.pnTop.Controls.Add(this.btnLogin);
            this.pnTop.Controls.Add(this.tbPassword);
            this.pnTop.Controls.Add(this.tbUser_id);
            this.pnTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnTop.Location = new System.Drawing.Point(0, 0);
            this.pnTop.Name = "pnTop";
            this.pnTop.Size = new System.Drawing.Size(920, 53);
            this.pnTop.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(649, 13);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(101, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(481, 13);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(101, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(267, 12);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(189, 23);
            this.tbPassword.TabIndex = 1;
            this.tbPassword.Text = "password1";
            // 
            // tbUser_id
            // 
            this.tbUser_id.Location = new System.Drawing.Point(48, 14);
            this.tbUser_id.Name = "tbUser_id";
            this.tbUser_id.Size = new System.Drawing.Size(189, 23);
            this.tbUser_id.TabIndex = 0;
            this.tbUser_id.Text = "cjheisen";
            // 
            // pnTextbox
            // 
            this.pnTextbox.Controls.Add(this.tbMemo);
            this.pnTextbox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnTextbox.Location = new System.Drawing.Point(0, 607);
            this.pnTextbox.Name = "pnTextbox";
            this.pnTextbox.Size = new System.Drawing.Size(920, 195);
            this.pnTextbox.TabIndex = 2;
            // 
            // tbMemo
            // 
            this.tbMemo.Location = new System.Drawing.Point(32, 17);
            this.tbMemo.Multiline = true;
            this.tbMemo.Name = "tbMemo";
            this.tbMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMemo.Size = new System.Drawing.Size(839, 153);
            this.tbMemo.TabIndex = 0;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(920, 840);
            this.Controls.Add(this.pnTextbox);
            this.Controls.Add(this.pnTop);
            this.Controls.Add(this.pnBottom);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Login_KeyDown);
            this.pnBottom.ResumeLayout(false);
            this.pnBottom.PerformLayout();
            this.pnTop.ResumeLayout(false);
            this.pnTop.PerformLayout();
            this.pnTextbox.ResumeLayout(false);
            this.pnTextbox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel pnBottom;
        private Button btnCancel;
        private Label tsStatus;
        private Button btnGoToLogin;
        private Panel pnTop;
        private Button btnLogin;
        private TextBox tbPassword;
        private TextBox tbUser_id;
        private Panel pnTextbox;
        private TextBox tbMemo;
        private Button btnClear;


    }
}