using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using B18_Ex02;

namespace B18_Ex05
{
    public partial class CheckersGameForm : Form
    {
        private const int k_ButtonSize = 50;
        private readonly ControlGame m_ControlGame;
        private readonly FormSettingsGame m_GameSettings;
        private SoliderButton[,] m_SoliderBoard;
        private SoliderButton m_FirstButtonToClick = null;
        private bool m_IsNewGame = false;
        private Color m_LastColorOfFirstSoliderButton;

        public CheckersGameForm()
        {
            m_ControlGame = new ControlGame(DisplayMessage, ChangeBoardForm, UpdatePlayersInfoInForm, DisplayWinnerInEndRoundOrGame, ShowActivePlayer, EnableAndDisableSoliderButtons);
            m_GameSettings = new FormSettingsGame();

            bool canStartGame;
            startGameSettings(out canStartGame);
            if (canStartGame)
            {
                InitializeComponent();
                m_ControlGame.InitGame(m_GameSettings.PlayerOneName, m_GameSettings.PlayerTwoName, m_GameSettings.HasTwoPlayers, m_GameSettings.BoardSize);
                initSoliderBoard();
                m_ControlGame.Run();
                m_ControlGame.ControlTheGame.IsFirstGame = false;
                ShowDialog();
            }
        }

        public void EnableAndDisableSoliderButtons(PlayerInformation i_Player1, PlayerInformation i_Player2, GameBoard i_Board)
        {
        for (int i = 0; i < this.m_GameSettings.BoardSize; i++)
        {
            for (int j = 0; j < this.m_GameSettings.BoardSize; j++)
             {
                 if (i_Player1.IsPlaying)
                   {
                        if (i_Board.GetCellInBoard(i, j) == i_Player2.Solider || i_Board.GetCellInBoard(i, j) == i_Player2.King)
                        {
                            m_SoliderBoard[i, j].Enabled = false;
                            m_SoliderBoard[i, j].BackColor = Color.LavenderBlush;
                        }
                        else if (i_Board.GetCellInBoard(i, j) == i_Player1.Solider || i_Board.GetCellInBoard(i, j) == i_Player1.King)
                        {
                            m_SoliderBoard[i, j].Enabled = true;
                            m_SoliderBoard[i, j].BackColor = Color.Pink;
                        }
                    }
                    else
                    {
                        if (i_Board.GetCellInBoard(i, j) == i_Player1.Solider || i_Board.GetCellInBoard(i, j) == i_Player1.King)
                        {
                            m_SoliderBoard[i, j].Enabled = false;
                            m_SoliderBoard[i, j].BackColor = Color.LavenderBlush;
                        }
                        else if (i_Board.GetCellInBoard(i, j) == i_Player2.Solider || i_Board.GetCellInBoard(i, j) == i_Player2.King)
                        {
                            m_SoliderBoard[i, j].Enabled = true;
                            m_SoliderBoard[i, j].BackColor = Color.Pink;
                        }
                    }

                    if (i_Board.BoardGame[i, j].IsValidCell && i_Board.GetCellInBoard(i, j) == SoliderTypes.Empty)
                    {
                        m_SoliderBoard[i, j].Enabled = true;
                        m_SoliderBoard[i, j].BackColor = Color.LemonChiffon;
                    }
                }
            }
  }

        public void ShowActivePlayer(PlayerInformation i_Player1, PlayerInformation i_Player2)
        {
            if (i_Player1.IsPlaying)
            {
                this.labelPlayerOne.BackColor = Color.Pink;
                this.labelPlayerTwo.BackColor = this.BackColor;
            }
            else
            {
                this.labelPlayerOne.BackColor = this.BackColor;
                this.labelPlayerTwo.BackColor = Color.Pink;
            }
        }

        public void DisplayMessage(string i_MessageToDisplay)
        {
            MessageBox.Show(i_MessageToDisplay, "Damka", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
        }

        public void DisplayWinnerInEndRoundOrGame(PlayerInformation i_Winner)
        {
            DialogResult dialogResult = DialogResult.None;
            string messageToUser;
            if (i_Winner != null)
            {
                messageToUser = string.Format("{0} Won! and now has: {2} wins! {1} Another round?", i_Winner.PlayerName, Environment.NewLine, i_Winner.Victories);
                dialogResult = MessageBox.Show(messageToUser, "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }
            else if (i_Winner == null)
            {
                messageToUser = string.Format("There is a Tie! {0} Another round?", Environment.NewLine);
                dialogResult = MessageBox.Show(messageToUser, "Damka", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            }

            if (dialogResult == DialogResult.No)
            {
                PlayerInformation finalWinner = m_ControlGame.ControlTheGame.TheFinalWinner();
                messageToUser = string.Format("The final winner is : {0}{1} Bye Bye!", finalWinner.PlayerName, Environment.NewLine);
                dialogResult = MessageBox.Show(messageToUser, "Damka", MessageBoxButtons.OK, MessageBoxIcon.Question);
                this.Close();
            }
            else if (dialogResult == DialogResult.Yes)
            {
                m_ControlGame.InitGame(m_GameSettings.PlayerOneName, m_GameSettings.PlayerTwoName, m_GameSettings.HasTwoPlayers, m_GameSettings.BoardSize);
                setLocationsForSoliderButtons();
                m_IsNewGame = true;
            }
        }

        public void UpdatePlayersInfoInForm(PlayerInformation i_Player1, PlayerInformation i_Player2)
        { 
            this.labelPlayerOne.Text = string.Format(
@"{0} ({1}) : Points:{2}
              Wins:{3}"
               , i_Player1.PlayerName,
                i_Player1.Solider,
                i_Player1.Points,
                i_Player1.Victories);

            this.labelPlayerTwo.Text = string.Format(
@"{0} ({1}) : Points:{2}
              Wins:{3}"
                ,i_Player2.PlayerName,
                i_Player2.Solider,
                i_Player2.Points,
                i_Player2.Victories);
        }

        public void ChangeBoardForm(GameBoard i_GameBoard)
        {
            for (int i = 0; i < this.m_GameSettings.BoardSize; i++)
            {
                for (int j = 0; j < this.m_GameSettings.BoardSize; j++)
                {
                    this.m_SoliderBoard[i, j].Text = i_GameBoard.GetCellInBoard(i, j).ToString();
                }
            }
        }

        private void startGameSettings(out bool o_CanStartGame)
        {
            o_CanStartGame = false;

            this.m_GameSettings.ShowDialog();

            if (this.m_GameSettings.DialogResult == DialogResult.OK)
            {
                o_CanStartGame = true;
            }
        }

        private void CheckersGameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
