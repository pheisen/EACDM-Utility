namespace EACDMLinqUtiliy
{
    partial class Attempts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Attempts));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pnBsck = new System.Windows.Forms.Panel();
            this.aggTools = new System.Windows.Forms.ToolStrip();
            this.aggSaveAs = new System.Windows.Forms.ToolStripButton();
            this.aggPrintPrev = new System.Windows.Forms.ToolStripButton();
            this.aggPrint = new System.Windows.Forms.ToolStripButton();
            this.atGrid = new System.Windows.Forms.DataGridView();
            this.Item = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.due_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.student = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Grader = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.score = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.possible = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attempt_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnBsck.SuspendLayout();
            this.aggTools.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.atGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Location = new System.Drawing.Point(0, 835);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1150, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pnBsck
            // 
            this.pnBsck.Controls.Add(this.atGrid);
            this.pnBsck.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBsck.Location = new System.Drawing.Point(0, 25);
            this.pnBsck.Name = "pnBsck";
            this.pnBsck.Size = new System.Drawing.Size(1150, 810);
            this.pnBsck.TabIndex = 2;
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
            this.aggTools.Size = new System.Drawing.Size(1150, 25);
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
            // atGrid
            // 
            this.atGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.atGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item,
            this.due_date,
            this.student,
            this.Grader,
            this.score,
            this.possible,
            this.attempt_date});
            this.atGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atGrid.Location = new System.Drawing.Point(0, 0);
            this.atGrid.Name = "atGrid";
            this.atGrid.Size = new System.Drawing.Size(1150, 810);
            this.atGrid.TabIndex = 1;
            this.atGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.atGrid_RowHeaderMouseClick);
            // 
            // Item
            // 
            this.Item.DataPropertyName = "Item";
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.Item.DefaultCellStyle = dataGridViewCellStyle5;
            this.Item.HeaderText = "Item";
            this.Item.Name = "Item";
            this.Item.Width = 200;
            // 
            // due_date
            // 
            this.due_date.DataPropertyName = "due_date";
            this.due_date.HeaderText = "Due Date";
            this.due_date.Name = "due_date";
            this.due_date.Width = 150;
            // 
            // student
            // 
            this.student.DataPropertyName = "theStudent";
            this.student.HeaderText = "Student";
            this.student.Name = "student";
            // 
            // Grader
            // 
            this.Grader.DataPropertyName = "Grader";
            this.Grader.HeaderText = "Grader";
            this.Grader.Name = "Grader";
            // 
            // score
            // 
            this.score.DataPropertyName = "score";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle6.Format = "N2";
            dataGridViewCellStyle6.NullValue = null;
            this.score.DefaultCellStyle = dataGridViewCellStyle6;
            this.score.HeaderText = "Score";
            this.score.Name = "score";
            // 
            // possible
            // 
            this.possible.DataPropertyName = "possible";
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = null;
            this.possible.DefaultCellStyle = dataGridViewCellStyle7;
            this.possible.HeaderText = "Possible";
            this.possible.Name = "possible";
            // 
            // attempt_date
            // 
            this.attempt_date.DataPropertyName = "attempt_date";
            dataGridViewCellStyle8.Format = "d";
            dataGridViewCellStyle8.NullValue = null;
            this.attempt_date.DefaultCellStyle = dataGridViewCellStyle8;
            this.attempt_date.HeaderText = "Attempt Date";
            this.attempt_date.Name = "attempt_date";
            this.attempt_date.Width = 150;
            // 
            // Attempts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1150, 857);
            this.Controls.Add(this.pnBsck);
            this.Controls.Add(this.aggTools);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Attempts";
            this.Text = "Attempts";
            this.Load += new System.EventHandler(this.Attempts_Load);
            this.pnBsck.ResumeLayout(false);
            this.aggTools.ResumeLayout(false);
            this.aggTools.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.atGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel pnBsck;
        private System.Windows.Forms.ToolStrip aggTools;
        private System.Windows.Forms.ToolStripButton aggSaveAs;
        private System.Windows.Forms.ToolStripButton aggPrintPrev;
        private System.Windows.Forms.ToolStripButton aggPrint;
        private System.Windows.Forms.DataGridView atGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item;
        private System.Windows.Forms.DataGridViewTextBoxColumn due_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn student;
        private System.Windows.Forms.DataGridViewTextBoxColumn Grader;
        private System.Windows.Forms.DataGridViewTextBoxColumn score;
        private System.Windows.Forms.DataGridViewTextBoxColumn possible;
        private System.Windows.Forms.DataGridViewTextBoxColumn attempt_date;
    }
}