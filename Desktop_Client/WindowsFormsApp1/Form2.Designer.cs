namespace WindowsFormsApp1
{
    partial class Form2
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
            this.txtNume = new System.Windows.Forms.TextBox();
            this.txtPret = new System.Windows.Forms.TextBox();
            this.cmbCategorie = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dgvAdmin = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmin)).BeginInit();
            this.SuspendLayout();
            // 
            // txtNume
            // 
            this.txtNume.Location = new System.Drawing.Point(32, 73);
            this.txtNume.Name = "txtNume";
            this.txtNume.Size = new System.Drawing.Size(100, 22);
            this.txtNume.TabIndex = 0;
            // 
            // txtPret
            // 
            this.txtPret.Location = new System.Drawing.Point(333, 73);
            this.txtPret.Name = "txtPret";
            this.txtPret.Size = new System.Drawing.Size(100, 22);
            this.txtPret.TabIndex = 1;
            // 
            // cmbCategorie
            // 
            this.cmbCategorie.FormattingEnabled = true;
            this.cmbCategorie.Location = new System.Drawing.Point(167, 118);
            this.cmbCategorie.Name = "cmbCategorie";
            this.cmbCategorie.Size = new System.Drawing.Size(121, 24);
            this.cmbCategorie.TabIndex = 2;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(44, 292);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "Add!";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(358, 292);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete!";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dgvAdmin
            // 
            this.dgvAdmin.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdmin.Location = new System.Drawing.Point(492, 118);
            this.dgvAdmin.Name = "dgvAdmin";
            this.dgvAdmin.RowHeadersWidth = 51;
            this.dgvAdmin.RowTemplate.Height = 24;
            this.dgvAdmin.Size = new System.Drawing.Size(240, 150);
            this.dgvAdmin.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(56, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(364, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Price";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvAdmin);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.cmbCategorie);
            this.Controls.Add(this.txtPret);
            this.Controls.Add(this.txtNume);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdmin)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNume;
        private System.Windows.Forms.TextBox txtPret;
        private System.Windows.Forms.ComboBox cmbCategorie;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dgvAdmin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}