namespace EACDMLinqUtiliy
{
    partial class MD5Maker
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lbHash1 = new System.Windows.Forms.Label();
            this.tbHash = new System.Windows.Forms.TextBox();
            this.tbSHA1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbb64md5 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbPlaintext = new System.Windows.Forms.TextBox();
            this.tbSHAHash = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lbResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(75, 60);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(484, 26);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(75, 129);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 1;
            this.button1.Text = "Get Hashes";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lbHash1
            // 
            this.lbHash1.AutoSize = true;
            this.lbHash1.Location = new System.Drawing.Point(70, 272);
            this.lbHash1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbHash1.Name = "lbHash1";
            this.lbHash1.Size = new System.Drawing.Size(40, 20);
            this.lbHash1.TabIndex = 2;
            this.lbHash1.Text = "Md5";
            // 
            // tbHash
            // 
            this.tbHash.Location = new System.Drawing.Point(196, 262);
            this.tbHash.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbHash.Name = "tbHash";
            this.tbHash.Size = new System.Drawing.Size(632, 26);
            this.tbHash.TabIndex = 3;
            // 
            // tbSHA1
            // 
            this.tbSHA1.Location = new System.Drawing.Point(196, 322);
            this.tbSHA1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSHA1.Name = "tbSHA1";
            this.tbSHA1.Size = new System.Drawing.Size(632, 26);
            this.tbSHA1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(70, 332);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Sha1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(72, 214);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Number";
            // 
            // tbb64md5
            // 
            this.tbb64md5.Location = new System.Drawing.Point(196, 394);
            this.tbb64md5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbb64md5.Name = "tbb64md5";
            this.tbb64md5.Size = new System.Drawing.Size(632, 26);
            this.tbb64md5.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 405);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 20);
            this.label3.TabIndex = 8;
            this.label3.Text = "Base64(md5)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(74, 530);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(76, 599);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 20);
            this.label5.TabIndex = 10;
            this.label5.Text = "{SHA}";
            // 
            // tbPlaintext
            // 
            this.tbPlaintext.Location = new System.Drawing.Point(196, 524);
            this.tbPlaintext.Name = "tbPlaintext";
            this.tbPlaintext.Size = new System.Drawing.Size(632, 26);
            this.tbPlaintext.TabIndex = 11;
            // 
            // tbSHAHash
            // 
            this.tbSHAHash.Location = new System.Drawing.Point(196, 593);
            this.tbSHAHash.Multiline = true;
            this.tbSHAHash.Name = "tbSHAHash";
            this.tbSHAHash.Size = new System.Drawing.Size(683, 90);
            this.tbSHAHash.TabIndex = 12;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(80, 704);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 13;
            this.button2.Text = "Validate";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbResult
            // 
            this.lbResult.AutoSize = true;
            this.lbResult.Location = new System.Drawing.Point(254, 711);
            this.lbResult.Name = "lbResult";
            this.lbResult.Size = new System.Drawing.Size(62, 20);
            this.lbResult.TabIndex = 14;
            this.lbResult.Text = "(empty)";
            // 
            // MD5Maker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 820);
            this.Controls.Add(this.lbResult);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tbSHAHash);
            this.Controls.Add(this.tbPlaintext);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbb64md5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbSHA1);
            this.Controls.Add(this.tbHash);
            this.Controls.Add(this.lbHash1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MD5Maker";
            this.Text = "MD5Maker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lbHash1;
        private System.Windows.Forms.TextBox tbHash;
        private System.Windows.Forms.TextBox tbSHA1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbb64md5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbPlaintext;
        private System.Windows.Forms.TextBox tbSHAHash;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbResult;
    }
}