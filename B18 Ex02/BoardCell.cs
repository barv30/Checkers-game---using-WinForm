using System;
using System.Collections.Generic;
using System.Text;

namespace B18_Ex02
{
    public class BoardCell
    {
        private const char k_EmptyCell = ' ';
        private char m_CellState = k_EmptyCell;
        private bool m_IsValidCelle;

        public bool IsValidCell
        {
            get { return m_IsValidCelle; }
            set { m_IsValidCelle = value; }
        }

        public char Empty
        {
            get { return k_EmptyCell; }
        }

        public char CellState
        {
            get { return m_CellState; }
            set { m_CellState = value; }
        }
    }
}
