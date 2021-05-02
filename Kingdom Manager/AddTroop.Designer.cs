
namespace Kingdom_Manager
{
    partial class AddTroop
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
            this.TroopLevyRadioButton = new System.Windows.Forms.RadioButton();
            this.MercenaryRadioButton = new System.Windows.Forms.RadioButton();
            this.TroopTypeLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.TroopNumberLabel = new System.Windows.Forms.Label();
            this.TroopNumberBox = new System.Windows.Forms.TextBox();
            this.CountyComboBox = new System.Windows.Forms.ComboBox();
            this.CountyLabel = new System.Windows.Forms.Label();
            this.GoldCostBox = new System.Windows.Forms.TextBox();
            this.GoldCostLabel = new System.Windows.Forms.Label();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TroopLevyRadioButton
            // 
            this.TroopLevyRadioButton.AutoSize = true;
            this.TroopLevyRadioButton.Location = new System.Drawing.Point(7, 22);
            this.TroopLevyRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.TroopLevyRadioButton.Name = "TroopLevyRadioButton";
            this.TroopLevyRadioButton.Size = new System.Drawing.Size(63, 22);
            this.TroopLevyRadioButton.TabIndex = 0;
            this.TroopLevyRadioButton.TabStop = true;
            this.TroopLevyRadioButton.Text = "Levy";
            this.TroopLevyRadioButton.UseVisualStyleBackColor = true;
            this.TroopLevyRadioButton.CheckedChanged += new System.EventHandler(this.TroopLevyRadioButton_CheckedChanged);
            // 
            // MercenaryRadioButton
            // 
            this.MercenaryRadioButton.AutoSize = true;
            this.MercenaryRadioButton.Location = new System.Drawing.Point(7, 52);
            this.MercenaryRadioButton.Margin = new System.Windows.Forms.Padding(4);
            this.MercenaryRadioButton.Name = "MercenaryRadioButton";
            this.MercenaryRadioButton.Size = new System.Drawing.Size(108, 22);
            this.MercenaryRadioButton.TabIndex = 1;
            this.MercenaryRadioButton.TabStop = true;
            this.MercenaryRadioButton.Text = "Mercenary";
            this.MercenaryRadioButton.UseVisualStyleBackColor = true;
            this.MercenaryRadioButton.CheckedChanged += new System.EventHandler(this.MercenaryRadioButton_CheckedChanged);
            // 
            // TroopTypeLabel
            // 
            this.TroopTypeLabel.AutoSize = true;
            this.TroopTypeLabel.Location = new System.Drawing.Point(4, 0);
            this.TroopTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TroopTypeLabel.Name = "TroopTypeLabel";
            this.TroopTypeLabel.Size = new System.Drawing.Size(102, 18);
            this.TroopTypeLabel.TabIndex = 2;
            this.TroopTypeLabel.Text = "Troop Type:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.TroopTypeLabel);
            this.panel1.Controls.Add(this.MercenaryRadioButton);
            this.panel1.Controls.Add(this.TroopLevyRadioButton);
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 80);
            this.panel1.TabIndex = 3;
            // 
            // TroopNumberLabel
            // 
            this.TroopNumberLabel.AutoSize = true;
            this.TroopNumberLabel.Location = new System.Drawing.Point(2, 129);
            this.TroopNumberLabel.Name = "TroopNumberLabel";
            this.TroopNumberLabel.Size = new System.Drawing.Size(158, 18);
            this.TroopNumberLabel.TabIndex = 4;
            this.TroopNumberLabel.Text = "Number of troops:";
            this.TroopNumberLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // TroopNumberBox
            // 
            this.TroopNumberBox.Location = new System.Drawing.Point(166, 126);
            this.TroopNumberBox.Name = "TroopNumberBox";
            this.TroopNumberBox.Size = new System.Drawing.Size(100, 27);
            this.TroopNumberBox.TabIndex = 5;
            // 
            // CountyComboBox
            // 
            this.CountyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CountyComboBox.FormattingEnabled = true;
            this.CountyComboBox.Location = new System.Drawing.Point(166, 94);
            this.CountyComboBox.Name = "CountyComboBox";
            this.CountyComboBox.Size = new System.Drawing.Size(135, 26);
            this.CountyComboBox.TabIndex = 6;
            this.CountyComboBox.SelectedIndexChanged += new System.EventHandler(this.CountyComboBox_SelectedIndexChanged);
            // 
            // CountyLabel
            // 
            this.CountyLabel.AutoSize = true;
            this.CountyLabel.Location = new System.Drawing.Point(88, 97);
            this.CountyLabel.Name = "CountyLabel";
            this.CountyLabel.Size = new System.Drawing.Size(72, 18);
            this.CountyLabel.TabIndex = 7;
            this.CountyLabel.Text = "County:";
            // 
            // GoldCostBox
            // 
            this.GoldCostBox.Location = new System.Drawing.Point(166, 153);
            this.GoldCostBox.Name = "GoldCostBox";
            this.GoldCostBox.Size = new System.Drawing.Size(100, 27);
            this.GoldCostBox.TabIndex = 9;
            this.GoldCostBox.Visible = false;
            // 
            // GoldCostLabel
            // 
            this.GoldCostLabel.AutoSize = true;
            this.GoldCostLabel.Location = new System.Drawing.Point(65, 156);
            this.GoldCostLabel.Name = "GoldCostLabel";
            this.GoldCostLabel.Size = new System.Drawing.Size(95, 18);
            this.GoldCostLabel.TabIndex = 8;
            this.GoldCostLabel.Text = "Gold Cost:";
            this.GoldCostLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.GoldCostLabel.Visible = false;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(81, 201);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(124, 29);
            this.SubmitButton.TabIndex = 10;
            this.SubmitButton.Text = "Create Troop";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // AddTroop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 235);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.GoldCostBox);
            this.Controls.Add(this.GoldCostLabel);
            this.Controls.Add(this.CountyLabel);
            this.Controls.Add(this.CountyComboBox);
            this.Controls.Add(this.TroopNumberBox);
            this.Controls.Add(this.TroopNumberLabel);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddTroop";
            this.Text = "Add a new troop";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton TroopLevyRadioButton;
        private System.Windows.Forms.RadioButton MercenaryRadioButton;
        private System.Windows.Forms.Label TroopTypeLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label TroopNumberLabel;
        private System.Windows.Forms.TextBox TroopNumberBox;
        public System.Windows.Forms.ComboBox CountyComboBox;
        private System.Windows.Forms.Label CountyLabel;
        private System.Windows.Forms.TextBox GoldCostBox;
        private System.Windows.Forms.Label GoldCostLabel;
        private System.Windows.Forms.Button SubmitButton;
    }
}