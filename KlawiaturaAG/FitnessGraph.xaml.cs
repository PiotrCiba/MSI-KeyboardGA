using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KlawiaturaAG
{
    /// <summary>
    /// Logika interakcji dla klasy FitnessGraph.xaml
    /// </summary>
    public partial class FitnessGraph : Window
    {
        public FitnessGraph(Summary[] generations)
        {
            InitializeComponent();
            UpdateGraphPoints(generations);
        }

        public void UpdateGraphPoints(Summary[] generations)
        {
            List<double> id = new List<double>();
            List<double> fit = new List<double>();
            
            foreach(var g in generations)
            {
                id.Add(g.IdPokolenia);
                fit.Add(g.BestFitness);
            }

            FitnessPlot.Plot.AddScatter(id.ToArray(), fit.ToArray());
            FitnessPlot.Refresh();
        }
    }
}
