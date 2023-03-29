using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KlawiaturaAG
{
    public static class CarryModule
    {
        public static (Chromosom[], Chromosom[]) Select(Chromosom[] pop, int type, double variable, bool isCulling, double cullingMargin)
        {
            (Chromosom[] carry, Chromosom[] parents) output;
            output.carry = new Chromosom[0];
            output.parents = new Chromosom[0];

            switch (type)
            {
                case 0:
                    output.parents = pop.ToArray();
                    output.carry = new Chromosom[0];
                    break;
                case 1:
                    output = Elityzm(pop,variable,isCulling,cullingMargin);
                    break;
            }

            return output;
        }

        public static (Chromosom[], Chromosom[]) Elityzm(Chromosom[] pop, double topMargin, bool culling, double cullingVar)
        {
            (Chromosom[] carry, Chromosom[] parents) output;

            //take topmargin% off the top of the pop and carry it over
            output.carry = pop.OrderBy(p => p.fitness).Take((int)(topMargin / pop.Length)).ToArray();

            //if culling is on copy over , 
            if (culling)
                output.parents = pop.OrderBy(p => p.fitness).Take(pop.Length - (int)(cullingVar / pop.Length)).ToArray();
            else
                output.parents = pop;

            return output;
        }
    }
}
