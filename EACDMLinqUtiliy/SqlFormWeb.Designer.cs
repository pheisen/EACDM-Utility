namespace EACDMLinqUtiliy
{
    partial class SqlFormWeb
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SqlFormWeb));
            this.pnBackground = new System.Windows.Forms.Panel();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbResults = new System.Windows.Forms.WebBrowser();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnExec = new System.Windows.Forms.Button();
            this.tbSql = new System.Windows.Forms.TextBox();
            this.btnReadSQL = new System.Windows.Forms.Button();
            this.pnBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBackground
            // 
            this.pnBackground.BackColor = System.Drawing.Color.Maroon;
            this.pnBackground.Controls.Add(this.btnReadSQL);
            this.pnBackground.Controls.Add(this.btnClear);
            this.pnBackground.Controls.Add(this.tbResults);
            this.pnBackground.Controls.Add(this.btnClose);
            this.pnBackground.Controls.Add(this.btnExec);
            this.pnBackground.Controls.Add(this.tbSql);
            this.pnBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBackground.Location = new System.Drawing.Point(0, 0);
            this.pnBackground.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnBackground.Name = "pnBackground";
            this.pnBackground.Size = new System.Drawing.Size(1178, 798);
            this.pnBackground.TabIndex = 0;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(845, 741);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbResults
            // 
            this.tbResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbResults.Location = new System.Drawing.Point(37, 294);
            this.tbResults.MinimumSize = new System.Drawing.Size(20, 20);
            this.tbResults.Name = "tbResults";
            this.tbResults.Size = new System.Drawing.Size(1106, 414);
            this.tbResults.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1006, 741);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnExec
            // 
            this.btnExec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExec.Location = new System.Drawing.Point(37, 737);
            this.btnExec.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(87, 28);
            this.btnExec.TabIndex = 1;
            this.btnExec.Text = "Execute";
            this.btnExec.UseVisualStyleBackColor = true;
            this.btnExec.Click += new System.EventHandler(this.btnExec_Click);
            // 
            // tbSql
            // 
            this.tbSql.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSql.Location = new System.Drawing.Point(37, 39);
            this.tbSql.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbSql.Multiline = true;
            this.tbSql.Name = "tbSql";
            this.tbSql.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbSql.Size = new System.Drawing.Size(1106, 202);
            this.tbSql.TabIndex = 0;
            // 
            // btnReadSQL
            // 
            this.btnReadSQL.AutoSize = true;
            this.btnReadSQL.Location = new System.Drawing.Point(1032, 9);
            this.btnReadSQL.Name = "btnReadSQL";
            this.btnReadSQL.Size = new System.Drawing.Size(82, 28);
            this.btnReadSQL.TabIndex = 6;
            this.btnReadSQL.Text = "Read SQL";
            this.btnReadSQL.UseVisualStyleBackColor = true;
            this.btnReadSQL.Click += new System.EventHandler(this.btnReadSQL_Click);
            // 
            // SqlFormWeb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1178, 798);
            this.Controls.Add(this.pnBackground);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SqlFormWeb";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Execute SQL";
            this.pnBackground.ResumeLayout(false);
            this.pnBackground.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnBackground;
        private System.Windows.Forms.TextBox tbSql;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnExec;
        private System.Windows.Forms.WebBrowser tbResults;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnReadSQL;
    }
}