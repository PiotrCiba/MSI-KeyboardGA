using System;

namespace KlawiaturaAG
{
    internal class Chromosom
    {
        string[] layout { get; set; }
        public void randomise()
        {
            Random rnd = new Random();

        }
        public char getLetter(int row, int col)
        {
            return layout[row][col];
        }
    }
}
