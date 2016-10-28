namespace EACDMLinqUtiliy
{
    partial class Gradebook
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
            this.btnFillCourses = new System.Windows.Forms.Button();
            this.cGrid = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.gGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.cGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFillCourses
            // 
            this.btnFillCourses.Location = new System.Drawing.Point(46, 21);
            this.btnFillCourses.Name = "btnFillCourses";
            this.btnFillCourses.Size = new System.Drawing.Size(152, 23);
            this.btnFillCourses.TabIndex = 0;
            this.btnFillCourses.Text = "Fill this year\'s courses";
            this.btnFillCourses.UseVisualStyleBackColor = true;
            this.btnFillCourses.Click += new System.EventHandler(this.btnFillCourses_Click);
            // 
            // cGrid
            // 
            this.cGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cGrid.Location = new System.Drawing.Point(46, 63);
            this.cGrid.Name = "cGrid";
            this.cGrid.Size = new System.Drawing.Size(817, 269);
            this.cGrid.TabIndex = 1;
            this.cGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.cGrid_RowHeaderMouseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 615);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(895, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // gGrid
            // 
            this.gGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gGrid.Location = new System.Drawing.Point(46, 359);
            this.gGrid.Name = "gGrid";
            this.gGrid.Size = new System.Drawing.Size(817, 236);
            this.gGrid.TabIndex = 3;
            // 
            // Gradebook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 637);
            this.Controls.Add(this.gGrid);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.cGrid);
            this.Controls.Add(this.btnFillCourses);
            this.Name = "Gradebook";
            this.Text = "Gradebook";
            this.Load += new System.EventHandler(this.Gradebook_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFillCourses;
        private System.Windows.Forms.DataGridView cGrid;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView gGrid;
    }
}