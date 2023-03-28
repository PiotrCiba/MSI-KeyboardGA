using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlawiaturaAG
{
    public class Settings
    {
        //population settings
        public int popSize { get; set; } = 25;
        public int childNumber { get; set; } = 1;
        public bool elitism { get; set; } = false;
        public double elitismRate { get; set; } = 10;
        public bool culling { get; set; } = false;
        public double cullingRate { get; set; } = 10;

        //re-population settings
        public int currSel { get; set; } = 0;
        public int currX { get; set; } = 0;
        public int currMut { get; set; } = 0;
        public double mutChance { get; set; } = 0.01;
        public int mutSeverity { get; set; } = 1;

        //GA overall settings
        public bool fullMemory { get; set; } = true;
        public int currStopMode { get; set; } = 0;
        public int gensToRun { get; set; } = 25;
        public double epsToStopAt { get; set; } = 0.01;
        
    }
}
