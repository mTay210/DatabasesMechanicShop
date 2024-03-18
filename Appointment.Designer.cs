namespace MechanicShop
{
    partial class Appointment
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
            label9 = new Label();
            comboBox4 = new ComboBox();
            button2 = new Button();
            label4 = new Label();
            comboBox3 = new ComboBox();
            dataGridView1 = new DataGridView();
            button1 = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(206, 25);
            label9.Name = "label9";
            label9.Size = new Size(97, 15);
            label9.TabIndex = 35;
            label9.Text = "Select Technician";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(206, 52);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(169, 23);
            comboBox4.TabIndex = 34;
            comboBox4.SelectedIndexChanged += comboBox4_SelectedIndexChanged;
            // 
            // button2
            // 
            button2.Location = new Point(512, 52);
            button2.Name = "button2";
            button2.Size = new Size(138, 23);
            button2.TabIndex = 33;
            button2.Text = "Submit Appointment";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(15, 25);
            label4.Name = "label4";
            label4.Size = new Size(85, 15);
            label4.TabIndex = 32;
            label4.Text = "Type of Service";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(15, 52);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(171, 23);
            comboBox3.TabIndex = 31;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(15, 91);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(635, 150);
            dataGridView1.TabIndex = 36;
            // 
            // button1
            // 
            button1.Location = new Point(393, 51);
            button1.Name = "button1";
            button1.Size = new Size(104, 23);
            button1.TabIndex = 37;
            button1.Text = "Add Service";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Appointment
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(665, 273);
            Controls.Add(button1);
            Controls.Add(dataGridView1);
            Controls.Add(label9);
            Controls.Add(comboBox4);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(comboBox3);
            Name = "Appointment";
            Text = "Form4";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label9;
        private ComboBox comboBox4;
        private Button button2;
        private Label label4;
        private ComboBox comboBox3;
        private DataGridView dataGridView1;
        private Button button1;
        private Button button3;
    }
}