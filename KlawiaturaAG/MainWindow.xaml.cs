using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Input;
using CommunityToolkit.Mvvm;
using System.Security.RightsManagement;

namespace KlawiaturaAG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //Window binding fields
        public string[] CurrentLayout { get; set; } = { "-=", "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
        public bool isShowingCost { get; set; } = false;
        public string[] Layouts { get; set; } = { "QWERTY", "Dvorak", "ARENSITO", "Colemak", "Workman", "<Selected>" };

        public const double QwertyValue = 253.19964999999996;
        public double CurrLayoutEvaluation { get; set; } = 0;
        public double ImprovementOverQwerty { get; set; } = 0;
        public int[] ChildrenValues { get; set; } = { 1, 2 };
        public string[] SelectionAlgorithms { get; set; } = { "Turniej", "Ruletka" };
        public string[] CrossoverAlgorithms { get; set; } = { "OX", "ERX" };
        public string[] MutationAlgorithms { get; set; } = { "Pair Swap", "Partial Scramble" };
        public string[] SeverityTypes { get; set; } = { "Random", "Continuous" };

        GeneticAlgorithm GA = new GeneticAlgorithm();

        //GA's starting values, to be set upt before strting the algorithm
        public int popSize { get; set; } = 25;
        public int childNumber { get; set; } = 1;
        public int currSel { get; set; } = 0;
        public int currX { get; set; } = 0;
        public int currMut { get; set; } = 0;
        public double mutChance { get; set; } = 0.01;
        public int currMutSev {get;set;} = 0;
        public int mutSeverity { get; set; } = 1;
        public int popsToRun { get; set; } = 25;
        public double epsToStopAt { get; set; } = 0.01;
        public int currStopMode { get; set; } = 0;


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowCostButton_Click(object sender, RoutedEventArgs e)
        {
            if (isShowingCost)
            {
                UpdateKeyboardLayout();
                isShowingCost = false;
            }
            else
            {
                ShowCosts();
                isShowingCost = true;
            }
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
            if (isShowingCost)
                isShowingCost = false;
            UpdateKeyboardEvaluation();
        }

        public void UpdateKeyboardLayout()
        {
            SolidColorBrush white = new SolidColorBrush(System.Windows.Media.Colors.White );
            SolidColorBrush lightGray = new SolidColorBrush(System.Windows.Media.Colors.LightGray);

            //0-th row update
            Key_00.Content = CurrentLayout[0][0];
            Rect_00.Fill = lightGray;
            Key_01.Content = CurrentLayout[0][1];
            Rect_01.Fill = lightGray;

            //First row update
            Key_Q.Content = CurrentLayout[1][0];
            Rect_Q.Fill = white;
            Key_W.Content = CurrentLayout[1][1];
            Rect_W.Fill = white;
            Key_E.Content = CurrentLayout[1][2];
            Rect_E.Fill = white;
            Key_R.Content = CurrentLayout[1][3];
            Rect_R.Fill = white;
            Key_T.Content = CurrentLayout[1][4];
            Rect_T.Fill = white;
            Key_Y.Content = CurrentLayout[1][5];
            Rect_Y.Fill = white;
            Key_U.Content = CurrentLayout[1][6];
            Rect_U.Fill = white;
            Key_I.Content = CurrentLayout[1][7];
            Rect_I.Fill = white;
            Key_O.Content = CurrentLayout[1][8];
            Rect_O.Fill = white;
            Key_P.Content = CurrentLayout[1][9];
            Rect_P.Fill = white;
            Key_10.Content = CurrentLayout[1][10];
            Rect_10.Fill = lightGray;
            Key_11.Content = CurrentLayout[1][11];
            Rect_11.Fill = lightGray;

            //Second row update
            Key_A.Content = CurrentLayout[2][0];
            Rect_A.Fill = white;
            Key_S.Content = CurrentLayout[2][1];
            Rect_S.Fill = white;
            Key_D.Content = CurrentLayout[2][2];
            Rect_D.Fill = white;
            Key_F.Content = CurrentLayout[2][3];
            Rect_F.Fill = white;
            Key_G.Content = CurrentLayout[2][4];
            Rect_G.Fill = white;
            Key_H.Content = CurrentLayout[2][5];
            Rect_H.Fill = white;
            Key_J.Content = CurrentLayout[2][6];
            Rect_J.Fill = white;
            Key_K.Content = CurrentLayout[2][7];
            Rect_K.Fill = white;
            Key_L.Content = CurrentLayout[2][8];
            Rect_L.Fill = white;
            Key_20.Content = CurrentLayout[2][9];
            Rect_20.Fill = white;
            Key_21.Content = CurrentLayout[2][10];
            Rect_21.Fill = white;

            //Third row update
            Key_Z.Content = CurrentLayout[3][0];
            Rect_Z.Fill = white;
            Key_X.Content = CurrentLayout[3][1];
            Rect_X.Fill = white;
            Key_C.Content = CurrentLayout[3][2];
            Rect_C.Fill = white;
            Key_V.Content = CurrentLayout[3][3];
            Rect_V.Fill = white;
            Key_B.Content = CurrentLayout[3][4];
            Rect_B.Fill = white;
            Key_N.Content = CurrentLayout[3][5];
            Rect_N.Fill = white;
            Key_M.Content = CurrentLayout[3][6];
            Rect_M.Fill = white;
            Key_30.Content = CurrentLayout[3][7];
            Rect_30.Fill = white;
            Key_31.Content = CurrentLayout[3][8];
            Rect_31.Fill = white;
            Key_32.Content = CurrentLayout[3][9];
            Rect_32.Fill = white;
        }

        public void ShowCosts()
        {
            double[][] koszt = {
                new double[] { 5, 5},
                new double[] { 4, 2, 2, 3, 4, 5, 3, 2, 2, 4, 4, 5 },
                new double[] { 1.5, 1, 1, 1, 3, 3, 1, 1, 1, 1.5, 3 },
                new double[] { 4, 4, 3, 2, 5, 3, 2, 3, 4, 4 }
            };

            SolidColorBrush one = new SolidColorBrush(Color.FromRgb(158, 203, 81));
            SolidColorBrush onepointfive = new SolidColorBrush(Color.FromRgb(179, 212, 111));
            SolidColorBrush two = new SolidColorBrush(Color.FromRgb(199, 222, 145));
            SolidColorBrush three = new SolidColorBrush(Color.FromRgb(239, 132, 135));
            SolidColorBrush four = new SolidColorBrush(Color.FromRgb(221, 47, 57));
            SolidColorBrush five = new SolidColorBrush(Color.FromRgb(176, 37, 44));

            //0-th row update
            Key_00.Content = koszt[0][0];
            Rect_00.Fill = five;
            Key_01.Content = koszt[0][1];
            Rect_01.Fill = five;

            //First row update
            Key_Q.Content = koszt[1][0];
            Rect_Q.Fill = four;
            Key_W.Content = koszt[1][1];
            Rect_W.Fill = two;
            Key_E.Content = koszt[1][2];
            Rect_E.Fill = two;
            Key_R.Content = koszt[1][3];
            Rect_R.Fill = three;
            Key_T.Content = koszt[1][4];
            Rect_T.Fill = four;
            Key_Y.Content = koszt[1][5];
            Rect_Y.Fill = five;
            Key_U.Content = koszt[1][6];
            Rect_U.Fill = three;
            Key_I.Content = koszt[1][7];
            Rect_I.Fill = two;
            Key_O.Content = koszt[1][8];
            Rect_O.Fill = two;
            Key_P.Content = koszt[1][9];
            Rect_P.Fill = four;
            Key_10.Content = koszt[1][10];
            Rect_10.Fill = four;
            Key_11.Content = koszt[1][11];
            Rect_11.Fill = five;

            //Second row update
            Key_A.Content = koszt[2][0];
            Rect_A.Fill = onepointfive;
            Key_S.Content = koszt[2][1];
            Rect_S.Fill = one;
            Key_D.Content = koszt[2][2];
            Rect_D.Fill = one;
            Key_F.Content = koszt[2][3];
            Rect_F.Fill = one;
            Key_G.Content = koszt[2][4];
            Rect_G.Fill = three;
            Key_H.Content = koszt[2][5];
            Rect_H.Fill = three;
            Key_J.Content = koszt[2][6];
            Rect_J.Fill = one;
            Key_K.Content = koszt[2][7];
            Rect_K.Fill = one;
            Key_L.Content = koszt[2][8];
            Rect_L.Fill = one;
            Key_20.Content = koszt[2][9];
            Rect_20.Fill = onepointfive;
            Key_21.Content = koszt[2][10];
            Rect_21.Fill = three;

            //Third row update
            Key_Z.Content = koszt[3][0];
            Rect_Z.Fill = four;
            Key_X.Content = koszt[3][1];
            Rect_X.Fill = four;
            Key_C.Content = koszt[3][2];
            Rect_C.Fill = three;
            Key_V.Content = koszt[3][3];
            Rect_V.Fill = two;
            Key_B.Content = koszt[3][4];
            Rect_B.Fill = five;
            Key_N.Content = koszt[3][5];
            Rect_N.Fill = three;
            Key_M.Content = koszt[3][6];
            Rect_M.Fill = two;
            Key_30.Content = koszt[3][7];
            Rect_30.Fill = three;
            Key_31.Content = koszt[3][8];
            Rect_31.Fill = four;
            Key_32.Content = koszt[3][9];
            Rect_32.Fill = four;
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
            if (IsFullMemory.IsChecked == true)
            {

            }
            else
            {

            }
        }

        private void PopSizeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            popSize =  int.Parse(PopSizeBox.Text);
        }

        private void ChildrenCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            childNumber = ChildrenValues[ChildrenCBox.SelectedIndex];
        }

        private void ChoiceAlgorithmCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currSel = ChoiceAlgorithmCBox.SelectedIndex;
        }

        private void CrossoverAlgorithmCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currX = CrossoverAlgorithmCBox.SelectedIndex;
        }

        private void MutationTypeCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currMut = MutationTypeCBox.SelectedIndex;
        }

        private void MutationChanceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mutChance = double.Parse(MutationChanceBox.Text);
        }

        private void MutationSevCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currMutSev = MutationSevCBox.SelectedIndex;
        }

        private void MutationSeverityBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mutSeverity = int.Parse(MutationSeverityBox.Text);
        }

        private void PopStopChoice_Checked(object sender, RoutedEventArgs e)
        {
            currStopMode = 0;
        }

        private void EpsStopChoice_Checked(object sender, RoutedEventArgs e)
        {
            currStopMode = 1;
        }

        
    }
}

