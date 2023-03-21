using System.Windows;
using System.Windows.Controls;

namespace KlawiaturaAG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string[] Layouts { get; set; } = { "QWERTY", "Dvorak", "ARENSITO", "Colemak", "<Selected>" };
        public int[] ParentValues { get; set; } = { 1, 2 };
        public int[] ChildrenValues { get; set; } = { 1, 2 };
        public string[] SelectionAlgorithms { get; set; } = {"Turniej", "Ruletka"};
        public string[] CrossoverAlgorithms { get; set; } = { "OX", "ERX" };
        public string[] MutationAlgorithms { get; set; } = { "Pair Swap", "Partial Scramble" };
        public string[] SeverityTypes { get; set; } = {"Random", "Continuous", "Spread" };


        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LayoutSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
