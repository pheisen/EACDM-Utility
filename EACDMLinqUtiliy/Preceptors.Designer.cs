namespace EACDMLinqUtiliy
{
    partial class Preceptors
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preceptors));
            this.btnGetCourses = new System.Windows.Forms.Button();
            this.btnGetPreceptors = new System.Windows.Forms.Button();
            this.cGrid = new System.Windows.Forms.DataGridView();
            this.pGrid = new System.Windows.Forms.DataGridView();
            this.notify = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.aGrid = new System.Windows.Forms.DataGridView();
            this.qGrid = new System.Windows.Forms.DataGridView();
            this.btnEnroll = new System.Windows.Forms.Button();
            this.pNav1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.pSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.btnAttemps = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.cGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pNav1)).BeginInit();
            this.pNav1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnGetCourses
            // 
            this.btnGetCourses.Location = new System.Drawing.Point(34, 35);
            this.btnGetCourses.Name = "btnGetCourses";
            this.btnGetCourses.Size = new System.Drawing.Size(139, 23);
            this.btnGetCourses.TabIndex = 0;
            this.btnGetCourses.Text = "Get Courses";
            this.btnGetCourses.UseVisualStyleBackColor = true;
            this.btnGetCourses.Click += new System.EventHandler(this.btnGetCourses_Click);
            // 
            // btnGetPreceptors
            // 
            this.btnGetPreceptors.Location = new System.Drawing.Point(480, 35);
            this.btnGetPreceptors.Name = "btnGetPreceptors";
            this.btnGetPreceptors.Size = new System.Drawing.Size(160, 23);
            this.btnGetPreceptors.TabIndex = 1;
            this.btnGetPreceptors.Text = "Get Preceptors";
            this.btnGetPreceptors.UseVisualStyleBackColor = true;
            this.btnGetPreceptors.Click += new System.EventHandler(this.btnGetPreceptors_Click);
            // 
            // cGrid
            // 
            this.cGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cGrid.Location = new System.Drawing.Point(34, 101);
            this.cGrid.Name = "cGrid";
            this.cGrid.Size = new System.Drawing.Size(336, 309);
            this.cGrid.TabIndex = 2;
            this.cGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.cGrid_RowHeaderMouseClick);
            // 
            // pGrid
            // 
            this.pGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.notify});
            this.pGrid.Location = new System.Drawing.Point(480, 101);
            this.pGrid.Name = "pGrid";
            this.pGrid.RowHeadersWidth = 5;
            this.pGrid.Size = new System.Drawing.Size(606, 655);
            this.pGrid.TabIndex = 3;
            // 
            // notify
            // 
            this.notify.HeaderText = "Column1";
            this.notify.Name = "notify";
            this.notify.Width = 25;
            // 
            // aGrid
            // 
            this.aGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.aGrid.Location = new System.Drawing.Point(34, 430);
            this.aGrid.Name = "aGrid";
            this.aGrid.Size = new System.Drawing.Size(336, 150);
            this.aGrid.TabIndex = 4;
            this.aGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.aGrid_RowHeaderMouseClick);
            // 
            // qGrid
            // 
            this.qGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.qGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.qGrid.Location = new System.Drawing.Point(34, 606);
            this.qGrid.Name = "qGrid";
            this.qGrid.Size = new System.Drawing.Size(336, 150);
            this.qGrid.TabIndex = 5;
            this.qGrid.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.qGrid_RowHeaderMouseClick);
            // 
            // btnEnroll
            // 
            this.btnEnroll.Location = new System.Drawing.Point(684, 35);
            this.btnEnroll.Name = "btnEnroll";
            this.btnEnroll.Size = new System.Drawing.Size(93, 23);
            this.btnEnroll.TabIndex = 6;
            this.btnEnroll.Text = "Enroll";
            this.btnEnroll.UseVisualStyleBackColor = true;
            this.btnEnroll.Click += new System.EventHandler(this.btnEnroll_Click);
            // 
            // pNav1
            // 
            this.pNav1.AddNewItem = this.bindingNavigatorAddNewItem;
            this.pNav1.CountItem = this.bindingNavigatorCountItem;
            this.pNav1.DeleteItem = this.bindingNavigatorDeleteItem;
            this.pNav1.Dock = System.Windows.Forms.DockStyle.None;
            this.pNav1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pNav1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.pNav1.Location = new System.Drawing.Point(480, 70);
            this.pNav1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.pNav1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.pNav1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.pNav1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.pNav1.Name = "pNav1";
            this.pNav1.PositionItem = this.bindingNavigatorPositionItem;
            this.pNav1.Size = new System.Drawing.Size(262, 25);
            this.pNav1.TabIndex = 7;
            this.pNav1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(42, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAttemps
            // 
            this.btnAttemps.Location = new System.Drawing.Point(237, 34);
            this.btnAttemps.Name = "btnAttemps";
            this.btnAttemps.Size = new System.Drawing.Size(95, 24);
            this.btnAttemps.TabIndex = 8;
            this.btnAttemps.Text = "Attempts";
            this.btnAttemps.UseVisualStyleBackColor = true;
            this.btnAttemps.Click += new System.EventHandler(this.btnAttemps_Click);
            // 
            // Preceptors
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1197, 817);
            this.Controls.Add(this.btnAttemps);
            this.Controls.Add(this.pNav1);
            this.Controls.Add(this.btnEnroll);
            this.Controls.Add(this.qGrid);
            this.Controls.Add(this.aGrid);
            this.Controls.Add(this.pGrid);
            this.Controls.Add(this.cGrid);
            this.Controls.Add(this.btnGetPreceptors);
            this.Controls.Add(this.btnGetCourses);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Preceptors";
            this.Text = "Preceptor Manager";
            this.Load += new System.EventHandler(this.Preceptors_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pNav1)).EndInit();
            this.pNav1.ResumeLayout(false);
            this.pNav1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetCourses;
        private System.Windows.Forms.Button btnGetPreceptors;
        private System.Windows.Forms.DataGridView cGrid;
        private System.Windows.Forms.DataGridView pGrid;
        private System.Windows.Forms.DataGridViewCheckBoxColumn notify;
        private System.Windows.Forms.DataGridView aGrid;
        private System.Windows.Forms.DataGridView qGrid;
        private System.Windows.Forms.Button btnEnroll;
        private System.Windows.Forms.BindingNavigator pNav1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.BindingSource pSource1;
        private System.Windows.Forms.Button btnAttemps;
    }
}