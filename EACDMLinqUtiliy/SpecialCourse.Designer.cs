namespace EACDMLinqUtiliy
{
    partial class SpecialCourse
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpecialCourse));
            this.btnLoadQ = new System.Windows.Forms.Button();
            this.tbMemo = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.tbMemoNew = new System.Windows.Forms.TextBox();
            this.lbMembers = new System.Windows.Forms.ListBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnLoadQ
            // 
            this.btnLoadQ.Location = new System.Drawing.Point(44, 26);
            this.btnLoadQ.Name = "btnLoadQ";
            this.btnLoadQ.Size = new System.Drawing.Size(75, 23);
            this.btnLoadQ.TabIndex = 0;
            this.btnLoadQ.Text = "Load Q";
            this.btnLoadQ.UseVisualStyleBackColor = true;
            this.btnLoadQ.Click += new System.EventHandler(this.btnLoadQ_Click);
            // 
            // tbMemo
            // 
            this.tbMemo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemo.Location = new System.Drawing.Point(32, 78);
            this.tbMemo.Multiline = true;
            this.tbMemo.Name = "tbMemo";
            this.tbMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMemo.Size = new System.Drawing.Size(1033, 312);
            this.tbMemo.TabIndex = 1;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(961, 25);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // tbMemoNew
            // 
            this.tbMemoNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tbMemoNew.Location = new System.Drawing.Point(32, 414);
            this.tbMemoNew.Multiline = true;
            this.tbMemoNew.Name = "tbMemoNew";
            this.tbMemoNew.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMemoNew.Size = new System.Drawing.Size(1033, 273);
            this.tbMemoNew.TabIndex = 3;
            // 
            // lbMembers
            // 
            this.lbMembers.FormattingEnabled = true;
            this.lbMembers.Items.AddRange(new object[] {
            "Peter Heisen (pheisen)",
            "Joyce Heisen (jheisen)",
            "Christopher Heisen (cjheisen)",
            "Jessica Heisen (jjheisen)"});
            this.lbMembers.Location = new System.Drawing.Point(172, 12);
            this.lbMembers.Name = "lbMembers";
            this.lbMembers.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbMembers.Size = new System.Drawing.Size(174, 95);
            this.lbMembers.TabIndex = 4;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(430, 24);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(190, 23);
            this.btnProcess.TabIndex = 5;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // SpecialCourse
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1092, 715);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.lbMembers);
            this.Controls.Add(this.tbMemoNew);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.tbMemo);
            this.Controls.Add(this.btnLoadQ);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SpecialCourse";
            this.Text = "Special Course Manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadQ;
        private System.Windows.Forms.TextBox tbMemo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.TextBox tbMemoNew;
        private System.Windows.Forms.ListBox lbMembers;
        private System.Windows.Forms.Button btnProcess;
    }
}