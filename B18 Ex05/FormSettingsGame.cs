using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace B18_Ex05
{
    public partial class FormSettingsGame : Form
    {
            private const string k_DefaultComputerPlayerName = "Computer";
            private int m_BoardSize;
            private bool m_HasSecondPlayer = false;

            public int BoardSize
            {
                get { return this.m_BoardSize; }
            }

            public string PlayerOneName
            {
                get { return this.textBox1.Text; }
            }

            public string PlayerTwoName
            {
                get { return this.textBox2.Text; }
            }

            public bool HasTwoPlayers
            {
                get { return this.m_HasSecondPlayer; }
            }

            public FormSettingsGame()
            {
                this.InitializeComponent();
                this.textBox2.Text = k_DefaultComputerPlayerName;
            }

            private void checkBoxPlayer2_CheckedChanged(object sender, EventArgs e)
            {
                if (this.checkBoxPlayer2.Checked)
                {
                    this.textBox2.Enabled = true;
                    this.m_HasSecondPlayer = true;
                }
                else
                {
                    this.m_HasSecondPlayer = false;
                    this.textBox2.Enabled = false;
                    this.textBox2.Text = k_DefaultComputerPlayerName;
                }
            }

            private void FormGameSettings_KeyDown(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.DialogResult = DialogResult.Cancel;
                }
            }

            private void buttonDone_Click(object sender, EventArgs e)
            {
                if (this.textBox1.Text.Length == 0 || this.textBox1.Text.Contains(" ") || this.textBox2.Text.Length == 0 || this.textBox2.Text.Contains(" "))
                {
                    const string warningMessage = "Player names can not be empty or contain spaces.";
                    MessageBox.Show(warningMessage, "Damka", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else if (!this.radioButton1.Checked && !this.radioButton2.Checked && !this.radioButton3.Checked)
                {
                    const string warningMessage = "You should choose the size of the borad. ";
                    MessageBox.Show(warningMessage, "Damka", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
                else
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }

            private void FormGameSettings_Activated(object sender, EventArgs e)
            {
                this.ActiveControl = this.textBox1;
            }

            private void radioButtonSize_CheckedChanged(object sender, EventArgs e)
            {
                if (((RadioButton)sender).Checked == true)
                {
                    this.m_BoardSize = int.Parse(((RadioButton)sender).Tag.ToString());
                }
           }
    }
}