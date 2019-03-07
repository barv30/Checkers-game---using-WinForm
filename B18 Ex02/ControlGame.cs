using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02
{
    public delegate void DisplayMessageDelegate(string i_Message);

    public delegate void ChangeSoliderBoardDelegate(GameBoard i_GameBoard);

    public delegate void ScoreUpdateDelegate(PlayerInformation i_Player1, PlayerInformation i_Player2);

    public delegate void DisplayWinnerDelegate(PlayerInformation i_Winner);

    public delegate void ShowActivePlayerDelegate(PlayerInformation i_Player1, PlayerInformation i_Player2);

    public delegate void DisableAndEnableSoliderButtonsDelegate(PlayerInformation i_Active, PlayerInformation i_Passive, GameBoard i_Board);

    public class ControlGame
    {
        private readonly GameManager m_manager = new GameManager();
        
        public event DisplayMessageDelegate DisplayMessage;

        public event ChangeSoliderBoardDelegate ChangedSoliderBoard;

        public event ScoreUpdateDelegate UpdatedScore;

        public event DisplayWinnerDelegate DisplayWinner;

        public event ShowActivePlayerDelegate ShowedActivePlayer;

        public event DisableAndEnableSoliderButtonsDelegate DisabledAndEnabled;

        public GameManager ControlTheGame
        {
            get { return m_manager; }
        }

        public ControlGame(
            DisplayMessageDelegate i_DisplayMessage,
            ChangeSoliderBoardDelegate i_ChangeSoliderBoardDelegate,
            ScoreUpdateDelegate i_ScoreUpdateDelegate,
            DisplayWinnerDelegate i_DisplayWinnerDelegate,
            ShowActivePlayerDelegate i_ShowActivePlayerDelegate,
            DisableAndEnableSoliderButtonsDelegate i_DisableAndEnableSoliderButtonsDelegate)
        {
            DisplayMessage += i_DisplayMessage;
            ChangedSoliderBoard += i_ChangeSoliderBoardDelegate;
            DisplayWinner += i_DisplayWinnerDelegate;
            UpdatedScore += i_ScoreUpdateDelegate;
            ShowedActivePlayer += i_ShowActivePlayerDelegate;
            DisabledAndEnabled += i_DisableAndEnableSoliderButtonsDelegate;
        }

        public void InitGame(string i_Player1Name, string i_Player2Name, bool i_HasSecondPlayer, int i_BoardSize)
        {
            m_manager.Player1.PlayerName = i_Player1Name;
            m_manager.Player2.PlayerName = i_Player2Name;
            m_manager.Player2.IsHuman = i_HasSecondPlayer;
            m_manager.StartTheGameOrRound(i_BoardSize);
        }

        public void Run()
        {
            m_manager.TurnsManger();
            UpdatedScore.Invoke(m_manager.Player1, m_manager.Player2);
            ShowedActivePlayer.Invoke(m_manager.Player1, m_manager.Player2);
            DisabledAndEnabled.Invoke(m_manager.Player1, m_manager.Player2, m_manager.CheckersGameBoard);
            ChangedSoliderBoard.Invoke(m_manager.CheckersGameBoard);
        }

        public bool MakeMoveForPlayer(int i_CurrentCol, int i_CurrentRow, int i_NewCol, int i_NewRow)
        {
            bool isKeepKilling = false;
            bool o_IsMakeMove = false;
            GameMoves newMove = new GameMoves();
            int colKilledSolider = GameMoves.Error;
            int rowKilledSolider = GameMoves.Error;
            PlayerInformation currentPlayer;
            PlayerInformation winnerPlayer;
            bool isValidMove = true;

            if (m_manager.Player2.IsPlaying)
            {
                currentPlayer = m_manager.Player2;
            }
            else
            {
                currentPlayer = m_manager.Player1;
            }

            if (!currentPlayer.IsHuman)
            {
                newMove.MakeMoveForComputerPlayer(currentPlayer, m_manager.CheckersGameBoard, out i_CurrentCol, out i_CurrentRow, out i_NewCol, out i_NewRow);
                isValidMove = true;
            }
            else
            {
                isValidMove = newMove.IsLegalMove(currentPlayer, m_manager.CheckersGameBoard, i_CurrentCol, i_CurrentRow, i_NewCol, i_NewRow, ref colKilledSolider, ref rowKilledSolider, currentPlayer.IsHasToKill);
                if (!isValidMove)
                {
                    if (currentPlayer.IsHasToKill)
                    {
                        DisplayMessage.Invoke("You have to kill the enemy solider!");
                    }
                    else
                    {
                        DisplayMessage.Invoke("You should enter a valid move according the the checkers game");
                    }

                    isValidMove = false;
                }
            }

            if (isValidMove)
            {
                o_IsMakeMove = true;
                m_manager.UpdatePlayerInformation(currentPlayer, i_CurrentCol, i_CurrentRow, i_NewCol, i_NewRow, colKilledSolider, rowKilledSolider);
                UpdatedScore.Invoke(m_manager.Player1, m_manager.Player2);

                  Point placeAfterMove = new Point(i_NewCol, i_NewRow);
                  Point newPlaceAfterKiliingAgain;
                  if (currentPlayer.IsHuman)
                  {
                    isKeepKilling = keepKilling(currentPlayer, placeAfterMove, i_CurrentCol, i_CurrentRow, out newPlaceAfterKiliingAgain);
                    if (isKeepKilling)
                    {
                        placeAfterMove = newPlaceAfterKiliingAgain;
                        m_manager.TurnsManger();
                    }
                  }
                  else if (!currentPlayer.IsHuman)
                  {
                     isKeepKilling = keepKilling(currentPlayer, placeAfterMove, i_CurrentCol, i_CurrentRow, out newPlaceAfterKiliingAgain);
                    while (isKeepKilling)
                    { 
                        placeAfterMove = newPlaceAfterKiliingAgain;
                        isKeepKilling = keepKilling(currentPlayer, placeAfterMove, i_CurrentCol, i_CurrentRow, out newPlaceAfterKiliingAgain);
                    }
                  }
                
                  if (!isKeepKilling)
                  { 
                    winnerPlayer = m_manager.IsTheRoundOverAndWhoIsTheWinnerOfTheRound();
                    if (m_manager.IsRoundOver)
                    {
                        m_manager.CalculatePointsForWinner(winnerPlayer);
                        UpdatedScore.Invoke(m_manager.Player1, m_manager.Player2);
                        DisplayWinner.Invoke(winnerPlayer);
                    }
                    else
                    {
                        Run();
                    }
                }
            }

            return o_IsMakeMove;
        }

        private bool keepKilling(PlayerInformation i_CurrentPlayer, Point i_placeAfterMove, int i_LastCol, int i_LastRow, out Point io_NewPlaceAfterKiliingAgain)
        {
            bool o_isKeepKilling = false;
            List<Point> optionsToNewPlace;
            GameMoves newMove = new GameMoves();
            int nextCol;
            int nextRow;

            io_NewPlaceAfterKiliingAgain = new Point(GameMoves.Error, GameMoves.Error);

            if (i_CurrentPlayer.IsHasToKill && newMove.CheckIfHasKill(i_placeAfterMove, m_manager.CheckersGameBoard, i_CurrentPlayer.Solider, out optionsToNewPlace))
            {
                int colKilledSolider = GameMoves.Error;
                int rowKilledSolider = GameMoves.Error;

                if (!i_CurrentPlayer.IsHuman)
                {
                    if (newMove.IfComputerKeepKilling(i_CurrentPlayer.Solider, i_placeAfterMove, m_manager.CheckersGameBoard, out nextCol, out nextRow, ref colKilledSolider, ref rowKilledSolider))
                    {
                        newMove.IsLegalMove(i_CurrentPlayer, m_manager.CheckersGameBoard, i_placeAfterMove.X, i_placeAfterMove.Y, nextCol, nextRow, ref colKilledSolider, ref rowKilledSolider, i_CurrentPlayer.IsHasToKill);
                    }

                    m_manager.UpdatePlayerInformation(i_CurrentPlayer, i_placeAfterMove.X, i_placeAfterMove.Y, nextCol, nextRow, colKilledSolider, rowKilledSolider);
                    UpdatedScore.Invoke(m_manager.Player1, m_manager.Player2);
                    io_NewPlaceAfterKiliingAgain = new Point(nextCol, nextRow);  
                }

                o_isKeepKilling = true;
            }
            else
            {
                nextCol = GameMoves.Error;
                nextRow = GameMoves.Error;
                io_NewPlaceAfterKiliingAgain = new Point(nextCol, nextRow);
                o_isKeepKilling = false;
            }

            return o_isKeepKilling;
        }
    }
}