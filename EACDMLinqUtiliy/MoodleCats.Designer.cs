namespace EACDMLinqUtiliy
{
    partial class MoodleCats
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
            this.tbXml = new System.Windows.Forms.TextBox();
            this.tbMemo = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbXml
            // 
            this.tbXml.Location = new System.Drawing.Point(68, 65);
            this.tbXml.Multiline = true;
            this.tbXml.Name = "tbXml";
            this.tbXml.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbXml.Size = new System.Drawing.Size(736, 271);
            this.tbXml.TabIndex = 0;
            // 
            // tbMemo
            // 
            this.tbMemo.Location = new System.Drawing.Point(68, 358);
            this.tbMemo.Multiline = true;
            this.tbMemo.Name = "tbMemo";
            this.tbMemo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbMemo.Size = new System.Drawing.Size(736, 355);
            this.tbMemo.TabIndex = 1;
            // 
            // btnGo
            // 
            this.btnGo.AutoSize = true;
            this.btnGo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.Location = new System.Drawing.Point(340, 13);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 28);
            this.btnGo.TabIndex = 2;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // MoodleCats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 752);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.tbMemo);
            this.Controls.Add(this.tbXml);
            this.Name = "MoodleCats";
            this.Text = "MoodleCats";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbXml;
        private System.Windows.Forms.TextBox tbMemo;
        private System.Windows.Forms.Button btnGo;
    }
}