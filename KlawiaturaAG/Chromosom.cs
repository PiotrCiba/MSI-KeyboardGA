using System;

namespace KlawiaturaAG
{
    public class Chromosom
    {
        string[] layout { get; set; } = { "-=", "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
        public void randomise()
        {
            Random rnd = new Random();

        }
        public string[] getLayout()
        {
            return layout;
        }
    }
}
