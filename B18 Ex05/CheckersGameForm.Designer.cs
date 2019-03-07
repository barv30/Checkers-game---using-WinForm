using System;
using System.Drawing;

namespace B18_Ex05
{
    public partial class CheckersGameForm
    {
        private System.ComponentModel.IContainer components = null;

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
            this.labelPlayerOne = new System.Windows.Forms.Label();
            this.labelPlayerTwo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelPlayerOne
            // 
            this.labelPlayerOne.AutoSize = true;
            this.labelPlayerOne.BackColor = System.Drawing.SystemColors.MenuBar;
            this.labelPlayerOne.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPlayerOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelPlayerOne.Location = new System.Drawing.Point(34, 15);
            this.labelPlayerOne.Name = "labelPlayerOne";
            this.labelPlayerOne.Size = new System.Drawing.Size(66, 15);
            this.labelPlayerOne.TabIndex = 1;
            this.labelPlayerOne.Text = "Player1: 0";
            // 
            // labelPlayerTwo
            // 
            this.labelPlayerTwo.AutoSize = true;
            this.labelPlayerTwo.BackColor = System.Drawing.SystemColors.MenuBar;
            this.labelPlayerTwo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelPlayerTwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.labelPlayerTwo.Location = new System.Drawing.Point(178, 15);
            this.labelPlayerTwo.Name = "labelPlayerTwo";
            this.labelPlayerTwo.Size = new System.Drawing.Size(66, 15);
            this.labelPlayerTwo.TabIndex = 1;
            this.labelPlayerTwo.Text = "Player2: 0";
            // 
            // CheckersGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ClientSize = new System.Drawing.Size(658, 439);
            this.Controls.Add(this.labelPlayerTwo);
            this.Controls.Add(this.labelPlayerOne);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "CheckersGameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Damka";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CheckersGameForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void initSoliderBoard()
        {
            m_SoliderBoard = new SoliderButton[m_GameSettings.BoardSize, m_GameSettings.BoardSize];
            this.Size = new Size((m_GameSettings.BoardSize * k_ButtonSize) + 30, (m_GameSettings.BoardSize * k_ButtonSize) + 100);

            initSoliderButtonsInBoard();
        }

        private void initSoliderButtonsInBoard()
        {
            for (int i = 0; i < m_GameSettings.BoardSize; i++)
            {
                for (int j = 0; j < m_GameSettings.BoardSize; j++)
                {
                    m_SoliderBoard[i, j] = new SoliderButton(j, i);
                    m_SoliderBoard[i, j].Size = new Size(k_ButtonSize, k_ButtonSize);

                    if (((i + j) % 2) == 0)
                    {
                        m_SoliderBoard[i, j].Enabled = false;
                        m_SoliderBoard[i, j].BackColor = Color.Black;
                    }

                    else
                    {
                        m_SoliderBoard[i, j].BackColor = Color.Lavender;
                    }

                    m_SoliderBoard[i, j].Click += new EventHandler(GameButton_Clicked);
                    this.Controls.Add(m_SoliderBoard[i, j]);
                }
            }

            setLocationsForSoliderButtons();
        }

        private void GameButton_Clicked(object sender, EventArgs e)
        {
            bool isHaveMoveDone = false;
           
            if (m_FirstButtonToClick == null)
            {
                m_FirstButtonToClick = sender as SoliderButton;
                m_LastColorOfFirstSoliderButton = m_FirstButtonToClick.BackColor;
                (sender as SoliderButton).BackColor = Color.LightBlue;
                (sender as SoliderButton).IsClicked = true;
            }
            else
            {
                if (m_FirstButtonToClick != (sender as SoliderButton))
                {
                    isHaveMoveDone = m_ControlGame.MakeMoveForPlayer(
                    m_FirstButtonToClick.XPosition,
                    m_FirstButtonToClick.YPosition,
                    (sender as SoliderButton).XPosition,
                    (sender as SoliderButton).YPosition);

                    if (m_IsNewGame)
                    {
                        m_IsNewGame = false;
                        m_ControlGame.Run();
                        return;
                    }
                    else if (isHaveMoveDone)
                    { 
                        m_FirstButtonToClick.BackColor = Color.LemonChiffon;
                    }
                    else
                    {
                        m_FirstButtonToClick.BackColor = m_LastColorOfFirstSoliderButton;
                    }
                }
                else
                {
                    (sender as SoliderButton).BackColor = m_LastColorOfFirstSoliderButton;
                }

                m_FirstButtonToClick.IsClicked = false;
                m_FirstButtonToClick = null;
                (sender as SoliderButton).IsClicked = false;

                if (isHaveMoveDone && !m_GameSettings.HasTwoPlayers)
                {
                    m_ControlGame.MakeMoveForPlayer(0, 0, 0, 0);
                    if (m_IsNewGame)
                    {
                        m_IsNewGame = false;
                        m_ControlGame.Run();
                    }
                }
            }
        }

        private void setLocationsForSoliderButtons()
        {
            int xPos = 10;
            int yPos = 50;

            for (int i = 0; i < m_GameSettings.BoardSize; i++)
            {
                for (int j = 0; j < m_GameSettings.BoardSize; j++)
                {
                    m_SoliderBoard[i, j].Location = new Point(xPos, yPos);
                    xPos = m_SoliderBoard[i, j].Right;
                }

                xPos = 10;
                yPos += k_ButtonSize;
            }
        }

        #endregion

        private System.Windows.Forms.Label labelPlayerOne;
        private System.Windows.Forms.Label labelPlayerTwo;
    }
}