namespace MechanicShop
{
    partial class TechServices
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
            SuspendLayout();
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(205, 17);
            label9.Name = "label9";
            label9.Size = new Size(97, 15);
            label9.TabIndex = 42;
            label9.Text = "Select Technician";
            // 
            // comboBox4
            // 
            comboBox4.FormattingEnabled = true;
            comboBox4.Location = new Point(205, 44);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(169, 23);
            comboBox4.TabIndex = 41;
            // 
            // button2
            // 
            button2.Location = new Point(413, 43);
            button2.Name = "button2";
            button2.Size = new Size(175, 23);
            button2.TabIndex = 40;
            button2.Text = "Submit Technician Service";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(14, 17);
            label4.Name = "label4";
            label4.Size = new Size(85, 15);
            label4.TabIndex = 39;
            label4.Text = "Type of Service";
            // 
            // comboBox3
            // 
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(14, 44);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(171, 23);
            comboBox3.TabIndex = 38;
            // 
            // TechServices
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(618, 103);
            Controls.Add(label9);
            Controls.Add(comboBox4);
            Controls.Add(button2);
            Controls.Add(label4);
            Controls.Add(comboBox3);
            Name = "TechServices";
            Text = "TechServices";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label9;
        private ComboBox comboBox4;
        private Button button2;
        private Label label4;
        private ComboBox comboBox3;
    }
}