namespace DataGridViewRadioButtonElementsSample
{
    partial class Form1
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.RadioButtonColumn1 = new DataGridViewRadioButtonElements.DataGridViewRadioButtonColumn();
            this.RadioButtonColumn2 = new DataGridViewRadioButtonElements.DataGridViewRadioButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RadioButtonColumn1,
            this.RadioButtonColumn2});
            this.dataGridView1.Location = new System.Drawing.Point(4, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(355, 217);
            this.dataGridView1.TabIndex = 0;
            // 
            // RadioButtonColumn1
            // 
            this.RadioButtonColumn1.HeaderText = "RadioButtonColumn1";
            this.RadioButtonColumn1.Items.AddRange(new object[] {
            "Green",
            "Yellow",
            "Orange",
            "Red",
            "Black"});
            this.RadioButtonColumn1.Name = "RadioButtonColumn1";
            this.RadioButtonColumn1.Width = 125;
            // 
            // RadioButtonColumn2
            // 
            this.RadioButtonColumn2.DisplayMember = "Name";
            this.RadioButtonColumn2.HeaderText = "RadioButtonColumn2";
            this.RadioButtonColumn2.Name = "RadioButtonColumn2";
            this.RadioButtonColumn2.ValueMember = "ZipCode";
            this.RadioButtonColumn2.Width = 125;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 226);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form1";
            this.Text = "DataGridViewRadioButtonElements Sample";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private DataGridViewRadioButtonElements.DataGridViewRadioButtonColumn RadioButtonColumn1;
        private DataGridViewRadioButtonElements.DataGridViewRadioButtonColumn RadioButtonColumn2;
    }
}

