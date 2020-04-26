namespace PenAndPaperSudoku
{
    partial class FormMain
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
            this.BtnPencil = new System.Windows.Forms.Button();
            this.BtnMarker = new System.Windows.Forms.Button();
            this.BtnHighlight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnPencil
            // 
            this.BtnPencil.Location = new System.Drawing.Point(12, 48);
            this.BtnPencil.Name = "BtnPencil";
            this.BtnPencil.Size = new System.Drawing.Size(98, 30);
            this.BtnPencil.TabIndex = 0;
            this.BtnPencil.Text = "Pencil";
            this.BtnPencil.UseVisualStyleBackColor = true;
            this.BtnPencil.Click += new System.EventHandler(this.BtnPencil_Click);
            // 
            // BtnMarker
            // 
            this.BtnMarker.Location = new System.Drawing.Point(12, 12);
            this.BtnMarker.Name = "BtnMarker";
            this.BtnMarker.Size = new System.Drawing.Size(98, 30);
            this.BtnMarker.TabIndex = 1;
            this.BtnMarker.Text = "Marker";
            this.BtnMarker.UseVisualStyleBackColor = true;
            this.BtnMarker.Click += new System.EventHandler(this.BtnMarker_Click);
            // 
            // BtnHighlight
            // 
            this.BtnHighlight.Location = new System.Drawing.Point(12, 84);
            this.BtnHighlight.Name = "BtnHighlight";
            this.BtnHighlight.Size = new System.Drawing.Size(98, 30);
            this.BtnHighlight.TabIndex = 2;
            this.BtnHighlight.Text = "Highlighter";
            this.BtnHighlight.UseVisualStyleBackColor = true;
            this.BtnHighlight.Click += new System.EventHandler(this.BtnHighlight_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.BtnHighlight);
            this.Controls.Add(this.BtnMarker);
            this.Controls.Add(this.BtnPencil);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormMain";
            this.Text = "Pen&Paper Sudoku";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnPencil;
        private System.Windows.Forms.Button BtnMarker;
        private System.Windows.Forms.Button BtnHighlight;
    }
}

