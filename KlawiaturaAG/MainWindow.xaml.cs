using MindFusion.Spreadsheet.Wpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace KlawiaturaAG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string[] CurrentLayout { get; set; } = { "-=", "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
        public string[] Layouts { get; set; } = { "QWERTY", "Dvorak", "ARENSITO", "Colemak", "Workman", "<Selected>" };

        public const double QwertyValue = 253.19964999999996;
        public double CurrLayoutEvaluation { get; set; } = 0;
        public double ImprovementOverQwerty { get; set; } = 0;
        public int[] ParentValues { get; set; } = { 1, 2 };
        public int[] ChildrenValues { get; set; } = { 1, 2 };
        public string[] SelectionAlgorithms { get; set; } = {"Turniej", "Ruletka"};
        public string[] CrossoverAlgorithms { get; set; } = { "OX", "ERX" };
        public string[] MutationAlgorithms { get; set; } = { "Pair Swap", "Partial Scramble" };
        public string[] SeverityTypes { get; set; } = {"Random", "Continuous", "Spread" };

        GeneticAlgorithm GA = new GeneticAlgorithm();

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainWindow()
        {
            InitializeComponent();
        }
        private void LayoutSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedLayoutID.Content = Layouts[LayoutSelection.SelectedIndex];

            switch (LayoutSelection.SelectedIndex)
            {
                //Layout is saved in a string[] of {2, 12, 11, 10}
                case 0: //QWERTY layout
                    CurrentLayout = new string[] { "-=", "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
                    break;
                case 1: //Dvorak layout
                    CurrentLayout = new string[] { "[]", "',.PYFGCRL?=", "AOEUIDHTNS-", ";QJKXBMWVZ" };
                    break;
                case 2: //ARENSITO layout
                    CurrentLayout = new string[] { "?=", "QL.P';FUDK[]", "ARENBGSITO-", "ZW,HJVBYMX" };
                    break;
                case 3: //Colemak layout
                    CurrentLayout = new string[] { "-=", "QWFPGJLUY;[]", "ARSTDHNEIO'", "ZXCVBKM,.?"};
                    break;
                case 4: //Workman layout
                    CurrentLayout = new string[] { "-=", "QDRWBJFUP;[]", "ASHTGYNEOI'", "ZXMCVKL,.?" };
                    break;
                default: //defaults to QWERTY
                    CurrentLayout = new string[] { "-=", "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
                    break;
            }
            UpdateKeyboardLayout();
            UpdateKeyboardEvaluation();
            UpdateLayout();
        }

        private void UpdateKeyboardLayout()
        {
            //0-th row update
            Key_00.Content = CurrentLayout[0][0];
            Key_01.Content = CurrentLayout[0][1];

            //First row update
            Key_Q.Content = CurrentLayout[1][0];
            Key_W.Content = CurrentLayout[1][1];
            Key_E.Content = CurrentLayout[1][2];
            Key_R.Content = CurrentLayout[1][3];
            Key_T.Content = CurrentLayout[1][4];
            Key_Y.Content = CurrentLayout[1][5];
            Key_U.Content = CurrentLayout[1][6];
            Key_I.Content = CurrentLayout[1][7];
            Key_O.Content = CurrentLayout[1][8];
            Key_P.Content = CurrentLayout[1][9];
            Key_10.Content = CurrentLayout[1][10];
            Key_11.Content = CurrentLayout[1][11];

            //Second row update
            Key_A.Content = CurrentLayout[2][0];
            Key_S.Content = CurrentLayout[2][1];
            Key_D.Content = CurrentLayout[2][2];
            Key_F.Content = CurrentLayout[2][3];
            Key_G.Content = CurrentLayout[2][4];
            Key_H.Content = CurrentLayout[2][5];
            Key_J.Content = CurrentLayout[2][6];
            Key_K.Content = CurrentLayout[2][7];
            Key_K.Content = CurrentLayout[2][8];
            Key_20.Content = CurrentLayout[2][9];
            Key_21.Content = CurrentLayout[2][10];

            //Third row update
            Key_Z.Content = CurrentLayout[3][0];
            Key_X.Content = CurrentLayout[3][1];
            Key_C.Content = CurrentLayout[3][2];
            Key_V.Content = CurrentLayout[3][3];
            Key_B.Content = CurrentLayout[3][4];
            Key_N.Content = CurrentLayout[3][5];
            Key_M.Content = CurrentLayout[3][6];
            Key_30.Content = CurrentLayout[3][7];
            Key_31.Content = CurrentLayout[3][8];
            Key_32.Content = CurrentLayout[3][9];
        }

        private void UpdateKeyboardEvaluation()
        {
            CurrLayoutEvaluation = GeneticAlgorithm.Fn(CurrentLayout);
            OnPropertyChanged(nameof(CurrLayoutEvaluation));
            ImprovementOverQwerty = QwertyValue/CurrLayoutEvaluation;
            OnPropertyChanged(nameof(ImprovementOverQwerty));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
