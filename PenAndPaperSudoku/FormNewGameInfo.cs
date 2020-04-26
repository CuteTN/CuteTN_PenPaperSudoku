using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PenAndPaperSudoku
{
    public partial class FormNewGameInfo : Form
    {
        public DifficultyType DifficultyResult;

        public FormNewGameInfo()
        {
            InitializeComponent();
            CustomInit();
        }

        private void CustomInit()
        {
            radioButton_Hard.Checked = true;
            DialogResult = DialogResult.Cancel;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            if(radioButton_Easy.Checked)
                DifficultyResult = DifficultyType.Easy;
            if(radioButton_Medium.Checked)
                DifficultyResult = DifficultyType.Medium;
            if(radioButton_Hard.Checked)
                DifficultyResult = DifficultyType.Hard;

            this.Close();
        }
    }
}
