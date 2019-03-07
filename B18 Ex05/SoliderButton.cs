using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace B18_Ex05
{
    public class SoliderButton : Button
    {
        private readonly int m_XPosition;
        private readonly int m_YPosition;
        private bool m_IsClicked = false;

        public SoliderButton(int i_x, int i_y)
        {
            this.m_XPosition = i_x;
            this.m_YPosition = i_y;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, (byte)177);
        }

        public bool IsClicked
        {
            get { return this.m_IsClicked; }
            set { this.m_IsClicked = value; }
        }

        public int YPosition
        {
            get { return this.m_YPosition; }
        }

        public int XPosition
        {
            get { return this.m_XPosition; }
        }
    }
}
