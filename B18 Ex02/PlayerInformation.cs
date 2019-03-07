using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02
{
    public class PlayerInformation
    {
        private List<Point> placesOfSoliders = new List<Point>();
        private int m_AmountOfSoliders;
        private string m_NamePlayer;
        private char m_ShapeSolider;
        private char m_ShapeKing;
        private bool m_nowPlay = false;
        private bool m_IsHuman = true;
        private int m_Points;
        private int m_TotalPoints;
        private int m_Victories = 0;
        private bool m_isHasToKill = false;
        private int m_xCurrent, m_yCurrent, m_xFuture, m_yFuture;

         public PlayerInformation(char i_TypeSolider , char i_KingSolider)
        {
            m_ShapeSolider = i_TypeSolider;
            m_ShapeKing = i_KingSolider;
        }

        public bool IsHasToKill
        {
            get { return m_isHasToKill; }
            set { m_isHasToKill = value; }
        }

        public int Points
        {
            get { return m_Points; }
            set { m_Points = value; }
        }

        public int TotalPoints
        {
            get { return m_TotalPoints; }
            set { m_TotalPoints = value; }
        }

        public int Victories
        {
            get { return m_Victories; }
            set { m_Victories = value; }
        }

        public List<Point> ListOfSoliders
        {
            get { return placesOfSoliders; }
        }

        public int AmountOfSoliders
        {
            get { return m_AmountOfSoliders; }
            set { m_AmountOfSoliders = value; }
        }

        public bool IsPlaying
        {
            get { return m_nowPlay; }
            set { m_nowPlay = value; }
        }

        public bool IsHuman
        {
            get { return m_IsHuman; }
            set { m_IsHuman = value; }
        }

        public string PlayerName
        {
            get { return m_NamePlayer; }
            set { m_NamePlayer = value; }
        }

        public char Solider
        {
            get { return m_ShapeSolider; }
        }

        public char King
        {
            get { return m_ShapeKing; }
        }

        public int xCurrent
        {
            get { return m_xCurrent; }
            set { m_xCurrent = value; }
        }

        public int yCurrent
        {
            get { return m_yCurrent; }
            set { m_yCurrent = value; }
        }

        public int xFuture
        {
            get { return m_xFuture; }
            set { m_xFuture = value; }
        }

        public int yFuture
        {
            get { return m_yFuture; }
            set { m_yFuture = value; }
        }
    }
}
