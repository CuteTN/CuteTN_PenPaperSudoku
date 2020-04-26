namespace PenAndPaperSudoku
{
    partial class FormNewGameInfo
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton_Hard = new System.Windows.Forms.RadioButton();
            this.radioButton_Medium = new System.Windows.Forms.RadioButton();
            this.radioButton_Easy = new System.Windows.Forms.RadioButton();
            this.button_OK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton_Hard);
            this.groupBox1.Controls.Add(this.radioButton_Medium);
            this.groupBox1.Controls.Add(this.radioButton_Easy);
            this.groupBox1.Location = new System.Drawing.Point(33, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(119, 155);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "difficulty";
            // 
            // radioButton_Hard
            // 
            this.radioButton_Hard.AutoSize = true;
            this.radioButton_Hard.Checked = true;
            this.radioButton_Hard.Location = new System.Drawing.Point(24, 108);
            this.radioButton_Hard.Name = "radioButton_Hard";
            this.radioButton_Hard.Size = new System.Drawing.Size(60, 21);
            this.radioButton_Hard.TabIndex = 2;
            this.radioButton_Hard.TabStop = true;
            this.radioButton_Hard.Text = "Hard";
            this.radioButton_Hard.UseVisualStyleBackColor = true;
            // 
            // radioButton_Medium
            // 
            this.radioButton_Medium.AutoSize = true;
            this.radioButton_Medium.Location = new System.Drawing.Point(24, 72);
            this.radioButton_Medium.Name = "radioButton_Medium";
            this.radioButton_Medium.Size = new System.Drawing.Size(78, 21);
            this.radioButton_Medium.TabIndex = 1;
            this.radioButton_Medium.Text = "Medium";
            this.radioButton_Medium.UseVisualStyleBackColor = true;
            // 
            // radioButton_Easy
            // 
            this.radioButton_Easy.AutoSize = true;
            this.radioButton_Easy.Location = new System.Drawing.Point(24, 36);
            this.radioButton_Easy.Name = "radioButton_Easy";
            this.radioButton_Easy.Size = new System.Drawing.Size(60, 21);
            this.radioButton_Easy.TabIndex = 0;
            this.radioButton_Easy.Text = "Easy";
            this.radioButton_Easy.UseVisualStyleBackColor = true;
            // 
            // button_OK
            // 
            this.button_OK.Location = new System.Drawing.Point(101, 212);
            this.button_OK.Name = "button_OK";
            this.button_OK.Size = new System.Drawing.Size(75, 23);
            this.button_OK.TabIndex = 1;
            this.button_OK.Text = "Okie";
            this.button_OK.UseVisualStyleBackColor = true;
            this.button_OK.Click += new System.EventHandler(this.button_OK_Click);
            // 
            // FormNewGameInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(188, 247);
            this.Controls.Add(this.button_OK);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormNewGameInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New game option";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_Hard;
        private System.Windows.Forms.RadioButton radioButton_Medium;
        private System.Windows.Forms.RadioButton radioButton_Easy;
        private System.Windows.Forms.Button button_OK;
    }
}