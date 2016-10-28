namespace EACDMLinqUtiliy
{
    partial class AttemptRecord
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttemptRecord));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1 = new System.Windows.Forms.Panel();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.aggTools = new System.Windows.Forms.ToolStrip();
            this.aggSaveAs = new System.Windows.Forms.ToolStripButton();
            this.aggPrintPrev = new System.Windows.Forms.ToolStripButton();
            this.aggPrint = new System.Windows.Forms.ToolStripButton();
            this.panel1.SuspendLayout();
            this.aggTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 688);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(794, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.wb);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(794, 688);
            this.panel1.TabIndex = 2;
            // 
            // wb
            // 
            this.wb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wb.Location = new System.Drawing.Point(0, 0);
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.Size = new System.Drawing.Size(794, 688);
            this.wb.TabIndex = 0;
            // 
            // aggTools
            // 
            this.aggTools.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.aggTools.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aggSaveAs,
            this.aggPrintPrev,
            this.aggPrint});
            this.aggTools.Location = new System.Drawing.Point(0, 0);
            this.aggTools.Name = "aggTools";
            this.aggTools.Size = new System.Drawing.Size(794, 25);
            this.aggTools.TabIndex = 67;
            this.aggTools.Text = "toolStrip1";
            // 
            // aggSaveAs
            // 
            this.aggSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aggSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("aggSaveAs.Image")));
            this.aggSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aggSaveAs.Name = "aggSaveAs";
            this.aggSaveAs.Size = new System.Drawing.Size(23, 22);
            this.aggSaveAs.ToolTipText = "Save As";
            // 
            // aggPrintPrev
            // 
            this.aggPrintPrev.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aggPrintPrev.Image = ((System.Drawing.Image)(resources.GetObject("aggPrintPrev.Image")));
            this.aggPrintPrev.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aggPrintPrev.Name = "aggPrintPrev";
            this.aggPrintPrev.Size = new System.Drawing.Size(23, 22);
            this.aggPrintPrev.Text = "toolStripButton1";
            this.aggPrintPrev.ToolTipText = "Print Preview";
            // 
            // aggPrint
            // 
            this.aggPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.aggPrint.Image = ((System.Drawing.Image)(resources.GetObject("aggPrint.Image")));
            this.aggPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aggPrint.Name = "aggPrint";
            this.aggPrint.Size = new System.Drawing.Size(23, 22);
            this.aggPrint.ToolTipText = "Print";
            // 
            // AttemptRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 710);
            this.Controls.Add(this.aggTools);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AttemptRecord";
            this.Text = "Attempt Record";
            this.panel1.ResumeLayout(false);
            this.aggTools.ResumeLayout(false);
            this.aggTools.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser wb;
        private System.Windows.Forms.ToolStrip aggTools;
        private System.Windows.Forms.ToolStripButton aggSaveAs;
        private System.Windows.Forms.ToolStripButton aggPrintPrev;
        private System.Windows.Forms.ToolStripButton aggPrint;
    }
}