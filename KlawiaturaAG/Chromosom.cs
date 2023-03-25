using System;

namespace KlawiaturaAG
{
    public class Chromosom
    {
        public string[] layout { get; set; } = { "-=", "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
        public double fitness { get; set; } = 0;
    }
}
