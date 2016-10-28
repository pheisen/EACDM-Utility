namespace EACDMLinqUtiliy
{
    partial class CatsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnFillCats = new System.Windows.Forms.Button();
            this.catsGrid = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.itemsGrid = new System.Windows.Forms.DataGridView();
            this.btnFillCMS = new System.Windows.Forms.Button();
            this.lbAdds = new System.Windows.Forms.Label();
            this.btnCatsFillLocal = new System.Windows.Forms.Button();
            this.btnCatsFromLocal = new System.Windows.Forms.Button();
            this.btnclear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.catsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFillCats
            // 
            this.btnFillCats.Location = new System.Drawing.Point(12, 22);
            this.btnFillCats.Name = "btnFillCats";
            this.btnFillCats.Size = new System.Drawing.Size(123, 23);
            this.btnFillCats.TabIndex = 0;
            this.btnFillCats.Text = "Fill Cats From CMS";
            this.btnFillCats.UseVisualStyleBackColor = true;
            this.btnFillCats.Click += new System.EventHandler(this.btnFillCats_Click);
            // 
            // catsGrid
            // 
            this.catsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.catsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.catsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle20.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.catsGrid.DefaultCellStyle = dataGridViewCellStyle20;
            this.catsGrid.Location = new System.Drawing.Point(12, 71);
            this.catsGrid.Name = "catsGrid";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.catsGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.catsGrid.RowHeadersWidth = 25;
            this.catsGrid.Size = new System.Drawing.Size(313, 634);
            this.catsGrid.TabIndex = 1;
            this.catsGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.catsGrid_RowHeaderMouseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 878);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1019, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // itemsGrid
            // 
            this.itemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.itemsGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle22;
            this.itemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle23.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.itemsGrid.DefaultCellStyle = dataGridViewCellStyle23;
            this.itemsGrid.Location = new System.Drawing.Point(358, 71);
            this.itemsGrid.Name = "itemsGrid";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle24.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle24.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle24.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle24.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle24.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.itemsGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle24;
            this.itemsGrid.Size = new System.Drawing.Size(649, 659);
            this.itemsGrid.TabIndex = 3;
            this.itemsGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.catsGrid_RowHeaderMouseClick);
            // 
            // btnFillCMS
            // 
            this.btnFillCMS.Enabled = false;
            this.btnFillCMS.Location = new System.Drawing.Point(582, 22);
            this.btnFillCMS.Name = "btnFillCMS";
            this.btnFillCMS.Size = new System.Drawing.Size(210, 23);
            this.btnFillCMS.TabIndex = 4;
            this.btnFillCMS.Text = "Fill/Update EACCatLibrary from grid";
            this.btnFillCMS.UseVisualStyleBackColor = true;
            this.btnFillCMS.Visible = false;
            this.btnFillCMS.Click += new System.EventHandler(this.btnFillCMS_Click);
            // 
            // lbAdds
            // 
            this.lbAdds.AutoSize = true;
            this.lbAdds.Location = new System.Drawing.Point(358, 52);
            this.lbAdds.Name = "lbAdds";
            this.lbAdds.Size = new System.Drawing.Size(35, 13);
            this.lbAdds.TabIndex = 5;
            this.lbAdds.Text = "label1";
            // 
            // btnCatsFillLocal
            // 
            this.btnCatsFillLocal.Location = new System.Drawing.Point(176, 22);
            this.btnCatsFillLocal.Name = "btnCatsFillLocal";
            this.btnCatsFillLocal.Size = new System.Drawing.Size(123, 23);
            this.btnCatsFillLocal.TabIndex = 6;
            this.btnCatsFillLocal.Text = "Fill/Update Local";
            this.btnCatsFillLocal.UseVisualStyleBackColor = true;
            this.btnCatsFillLocal.Click += new System.EventHandler(this.btnCatsFillLocal_Click);
            // 
            // btnCatsFromLocal
            // 
            this.btnCatsFromLocal.Location = new System.Drawing.Point(340, 22);
            this.btnCatsFromLocal.Name = "btnCatsFromLocal";
            this.btnCatsFromLocal.Size = new System.Drawing.Size(201, 23);
            this.btnCatsFromLocal.TabIndex = 7;
            this.btnCatsFromLocal.Text = "Fill Cats Grid From Local";
            this.btnCatsFromLocal.UseVisualStyleBackColor = true;
            this.btnCatsFromLocal.Click += new System.EventHandler(this.btnCatsFromLocal_Click);
            // 
            // btnclear
            // 
            this.btnclear.Location = new System.Drawing.Point(820, 21);
            this.btnclear.Name = "btnclear";
            this.btnclear.Size = new System.Drawing.Size(123, 23);
            this.btnclear.TabIndex = 8;
            this.btnclear.Text = "Empty Cats Grid";
            this.btnclear.UseVisualStyleBackColor = true;
            this.btnclear.Click += new System.EventHandler(this.btnclear_Click);
            // 
            // CatsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 900);
            this.Controls.Add(this.btnclear);
            this.Controls.Add(this.btnCatsFromLocal);
            this.Controls.Add(this.btnCatsFillLocal);
            this.Controls.Add(this.lbAdds);
            this.Controls.Add(this.btnFillCMS);
            this.Controls.Add(this.itemsGrid);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.catsGrid);
            this.Controls.Add(this.btnFillCats);
            this.Name = "CatsForm";
            this.Text = "Eac Outcomes Utilities";
            this.Load += new System.EventHandler(this.CatsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.catsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.itemsGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private QTIUtility.Category cats;
        private System.Windows.Forms.Button btnFillCats;
        private System.Windows.Forms.DataGridView catsGrid;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView itemsGrid;
        private System.Windows.Forms.Button btnFillCMS;
        private System.Windows.Forms.Label lbAdds;
        private System.Windows.Forms.Button btnCatsFillLocal;
        private System.Windows.Forms.Button btnCatsFromLocal;
        private System.Windows.Forms.Button btnclear;
    }
}