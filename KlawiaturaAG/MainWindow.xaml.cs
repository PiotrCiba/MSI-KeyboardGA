using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace KlawiaturaAG
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //Window binding fields
        public string[] CurrentLayout { get; set; } = { "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
        public bool isShowingCost { get; set; } = false;
        public string[] Layouts { get; set; } = { "QWERTY", "Dvorak", "ARENSITO", "Colemak", "Workman", "<Selected>" };

        public const double QwertyValue = 251.43964999999992;
        public double CurrLayoutEvaluation { get; set; } = 0;
        public double ImprovementOverQwerty { get; set; } = 0;
        public int[] ChildrenValues { get; set; } = { 1, 2 };
        public string[] CarryoverAlgorithms { get; set; } = { "No carry-over", "Elityzm (%)"};
        public string[] SelectionAlgorithms { get; set; } = { "Turniej", "Ruletka", "Rank" };
        public string[] CrossoverAlgorithms { get; set; } = { "OX", "CX", "ERX", "AEX"};
        public string[] MutationAlgorithms { get; set; } = { "Pair Swap", "Partial Scramble", "Inversion Mutation" };

        public Settings settings = new Settings();

        //GA's outputs, to be used in datagrids

        public Summary[] GenerationSummaries { get; set; } = new Summary[0];
        public Chromosom[] CurrSelGeneration { get; set; } = new Chromosom[0];
        public List<Chromosom[]> AllGenerations { get; set; } = new List<Chromosom[]>();

        private Progress<int> progress { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public MainWindow()
        {
            progress = new Progress<int>();
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
                    CurrentLayout = new string[] { "QWERTYUIOP[]", "ASDFGHJKL;'", "ZXCVBNM,.?" };
                    break;
                case 1: //Dvorak layout
                    CurrentLayout = new string[] { "',.PYFGCRL?=", "AOEUIDHTNS-", ";QJKXBMWVZ" };
                    break;
                case 2: //ARENSITO layout
                    CurrentLayout = new string[] { "QL.P';FUDK[]", "ARENBGSITO-", "ZW,HJVBYMX" };
                    break;
                case 3: //Colemak layout
                    CurrentLayout = new string[] { "QWFPGJLUY;[]", "ARSTDHNEIO'", "ZXCVBKM,.?" };
                    break;
                case 4: //Workman layout
                    CurrentLayout = new string[] { "QDRWBJFUP;[]", "ASHTGYNEOI'", "ZXMCVKL,.?" };
                    break;
            }
            UpdateKeyboardLayout();
            if (isShowingCost)
                isShowingCost = false;
            UpdateKeyboardEvaluation();
        }

        public void UpdateKeyboardLayout()
        {
            SolidColorBrush white = new SolidColorBrush(System.Windows.Media.Colors.White);
            SolidColorBrush lightGray = new SolidColorBrush(System.Windows.Media.Colors.LightGray);
            /*
            //0-th row update
            Key_00.Content = CurrentLayout[0][0];
            Rect_00.Fill = lightGray;
            Key_01.Content = CurrentLayout[0][1];
            Rect_01.Fill = lightGray;
            */

            //First row update
            Key_Q.Content = CurrentLayout[0][0];
            Rect_Q.Fill = white;
            Key_W.Content = CurrentLayout[0][1];
            Rect_W.Fill = white;
            Key_E.Content = CurrentLayout[0][2];
            Rect_E.Fill = white;
            Key_R.Content = CurrentLayout[0][3];
            Rect_R.Fill = white;
            Key_T.Content = CurrentLayout[0][4];
            Rect_T.Fill = white;
            Key_Y.Content = CurrentLayout[0][5];
            Rect_Y.Fill = white;
            Key_U.Content = CurrentLayout[0][6];
            Rect_U.Fill = white;
            Key_I.Content = CurrentLayout[0][7];
            Rect_I.Fill = white;
            Key_O.Content = CurrentLayout[0][8];
            Rect_O.Fill = white;
            Key_P.Content = CurrentLayout[0][9];
            Rect_P.Fill = white;
            Key_10.Content = CurrentLayout[0][10];
            Rect_10.Fill = lightGray;
            Key_11.Content = CurrentLayout[0][11];
            Rect_11.Fill = lightGray;

            //Second row update
            Key_A.Content = CurrentLayout[1][0];
            Rect_A.Fill = white;
            Key_S.Content = CurrentLayout[1][1];
            Rect_S.Fill = white;
            Key_D.Content = CurrentLayout[1][2];
            Rect_D.Fill = white;
            Key_F.Content = CurrentLayout[1][3];
            Rect_F.Fill = white;
            Key_G.Content = CurrentLayout[1][4];
            Rect_G.Fill = white;
            Key_H.Content = CurrentLayout[1][5];
            Rect_H.Fill = white;
            Key_J.Content = CurrentLayout[1][6];
            Rect_J.Fill = white;
            Key_K.Content = CurrentLayout[1][7];
            Rect_K.Fill = white;
            Key_L.Content = CurrentLayout[1][8];
            Rect_L.Fill = white;
            Key_20.Content = CurrentLayout[1][9];
            Rect_20.Fill = white;
            Key_21.Content = CurrentLayout[1][10];
            Rect_21.Fill = white;

            //Third row update
            Key_Z.Content = CurrentLayout[2][0];
            Rect_Z.Fill = white;
            Key_X.Content = CurrentLayout[2][1];
            Rect_X.Fill = white;
            Key_C.Content = CurrentLayout[2][2];
            Rect_C.Fill = white;
            Key_V.Content = CurrentLayout[2][3];
            Rect_V.Fill = white;
            Key_B.Content = CurrentLayout[2][4];
            Rect_B.Fill = white;
            Key_N.Content = CurrentLayout[2][5];
            Rect_N.Fill = white;
            Key_M.Content = CurrentLayout[2][6];
            Rect_M.Fill = white;
            Key_30.Content = CurrentLayout[2][7];
            Rect_30.Fill = white;
            Key_31.Content = CurrentLayout[2][8];
            Rect_31.Fill = white;
            Key_32.Content = CurrentLayout[2][9];
            Rect_32.Fill = white;
        }

        public void ShowCosts()
        {
            double[][] koszt = {
                new double[] { 4, 2, 2, 3, 4, 4, 3, 2, 2, 4, 5, 5 },
                new double[] { 1.5, 1, 1, 1, 3, 3, 1, 1, 1, 1.5, 3 },
                new double[] { 4, 4, 3, 2, 4, 4, 2, 3, 4, 4 }
            };

            SolidColorBrush one = new SolidColorBrush(Color.FromRgb(158, 203, 81));
            SolidColorBrush onepointfive = new SolidColorBrush(Color.FromRgb(179, 212, 111));
            SolidColorBrush two = new SolidColorBrush(Color.FromRgb(199, 222, 145));
            SolidColorBrush three = new SolidColorBrush(Color.FromRgb(239, 132, 135));
            SolidColorBrush four = new SolidColorBrush(Color.FromRgb(221, 47, 57));
            SolidColorBrush five = new SolidColorBrush(Color.FromRgb(176, 37, 44));

            /*
            //0-th row update
            Key_00.Content = koszt[0][0];
            Rect_00.Fill = five;
            Key_01.Content = koszt[0][1];
            Rect_01.Fill = five;
            */

            //First row update
            Key_Q.Content = koszt[0][0];
            Rect_Q.Fill = four;
            Key_W.Content = koszt[0][1];
            Rect_W.Fill = two;
            Key_E.Content = koszt[0][2];
            Rect_E.Fill = two;
            Key_R.Content = koszt[0][3];
            Rect_R.Fill = three;
            Key_T.Content = koszt[0][4];
            Rect_T.Fill = four;
            Key_Y.Content = koszt[0][5];
            Rect_Y.Fill = four;
            Key_U.Content = koszt[0][6];
            Rect_U.Fill = three;
            Key_I.Content = koszt[0][7];
            Rect_I.Fill = two;
            Key_O.Content = koszt[0][8];
            Rect_O.Fill = two;
            Key_P.Content = koszt[0][9];
            Rect_P.Fill = four;
            Key_10.Content = koszt[0][10];
            Rect_10.Fill = five;
            Key_11.Content = koszt[0][11];
            Rect_11.Fill = five;

            //Second row update
            Key_A.Content = koszt[1][0];
            Rect_A.Fill = onepointfive;
            Key_S.Content = koszt[1][1];
            Rect_S.Fill = one;
            Key_D.Content = koszt[1][2];
            Rect_D.Fill = one;
            Key_F.Content = koszt[1][3];
            Rect_F.Fill = one;
            Key_G.Content = koszt[1][4];
            Rect_G.Fill = three;
            Key_H.Content = koszt[1][5];
            Rect_H.Fill = three;
            Key_J.Content = koszt[1][6];
            Rect_J.Fill = one;
            Key_K.Content = koszt[1][7];
            Rect_K.Fill = one;
            Key_L.Content = koszt[1][8];
            Rect_L.Fill = one;
            Key_20.Content = koszt[1][9];
            Rect_20.Fill = onepointfive;
            Key_21.Content = koszt[1][10];
            Rect_21.Fill = three;

            //Third row update
            Key_Z.Content = koszt[2][0];
            Rect_Z.Fill = four;
            Key_X.Content = koszt[2][1];
            Rect_X.Fill = four;
            Key_C.Content = koszt[2][2];
            Rect_C.Fill = three;
            Key_V.Content = koszt[2][3];
            Rect_V.Fill = two;
            Key_B.Content = koszt[2][4];
            Rect_B.Fill = four;
            Key_N.Content = koszt[2][5];
            Rect_N.Fill = four;
            Key_M.Content = koszt[2][6];
            Rect_M.Fill = two;
            Key_30.Content = koszt[2][7];
            Rect_30.Fill = three;
            Key_31.Content = koszt[2][8];
            Rect_31.Fill = four;
            Key_32.Content = koszt[2][9];
            Rect_32.Fill = four;
        }

        private void UpdateKeyboardEvaluation()
        {
            CurrLayoutEvaluation = GeneticAlgorithm.Fn(CurrentLayout);
            OnPropertyChanged(nameof(CurrLayoutEvaluation));
            ImprovementOverQwerty = QwertyValue / CurrLayoutEvaluation;
            OnPropertyChanged(nameof(ImprovementOverQwerty));
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //Re-setting the progressbar
            currentProgressbar.Value = 0;

            //setting up the progress updates
            progress = new Progress<int>( value => currentProgressbar.Value = value );

            //disabling GUI controls
            //this.IsEnabled = false;

            //calling GA asympotically, to get unpdates on the progressbar
            (List<Summary>, List<Chromosom[]>) output = await StartTask();

            //enabling GUI controls
            //this.IsEnabled = true;

            //sending Start returns to both DataGrids
            GenerationSummaries = output.Item1.ToArray();
            AllGenerations = output.Item2;
            GenerationsDataGrid.SelectedIndex = 0;
            ChromosomeDataGrid.SelectedIndex = 0;
            CurrSelGeneration = AllGenerations.First();
            OnPropertyChanged(nameof(GenerationSummaries));
            OnPropertyChanged(nameof(CurrSelGeneration));
        }

        public async Task<(List<Summary>, List<Chromosom[]>)> StartTask()
        {
            //calling Start method to execute GA
            return await Task.Run(() => GeneticAlgorithm.Start(settings, progress));
        }

        /*
        private void UpdateProgressBar(int value)
        {
            currentProgressbar.Value = value;
        }
        */

        private void PopSizeBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int temp;
            if (int.TryParse(PopSizeBox.Text, out temp))
            {
                settings.popSize = temp;
                PopSizeBox.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                PopSizeBox.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void ChildrenCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.childNumber = ChildrenValues[ChildrenCBox.SelectedIndex];
        }

        private void ChoiceAlgorithmCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.currSel = ChoiceAlgorithmCBox.SelectedIndex;
        }

        private void CrossoverAlgorithmCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.currX = CrossoverAlgorithmCBox.SelectedIndex;
        }

        private void MutationTypeCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.currMut = MutationTypeCBox.SelectedIndex;
        }

        private void MutationChanceBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (double.TryParse(MutationChanceBox.Text, out temp))
            {
                settings.mutChance = temp;
                MutationChanceBox.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                MutationChanceBox.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void MutationSeverityBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int temp;
            if (int.TryParse(MutationSeverityBox.Text, out temp))
            {
                settings.mutSeverity = temp;
                MutationSeverityBox.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                MutationSeverityBox.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void SelectionPressureBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (double.TryParse(SelectionPressureBox.Text, out temp))
            {
                settings.SelPressure = temp;
                SelectionPressureBox.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                SelectionPressureBox.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void GenSizeStopConditionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int temp;
            if (int.TryParse(GensToRunStopConditionBox.Text, out temp))
            {
                settings.gensToRun = temp;
                GensToRunStopConditionBox.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                GensToRunStopConditionBox.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void EpsValueStopConditionBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (double.TryParse(EpsValueStopConditionBox.Text, out temp))
            {
                settings.epsToStopAt = temp;
                EpsValueStopConditionBox.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                EpsValueStopConditionBox.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void GenStopChoice_Checked(object sender, RoutedEventArgs e)
        {
            settings.currStopMode = false;
        }

        private void EpsStopChoice_Checked(object sender, RoutedEventArgs e)
        {
            settings.currStopMode = true;
        }

        private void GenerationsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AllGenerations.Count > 1 && GenerationsDataGrid.SelectedIndex >= 0)
            {
                CurrSelGeneration = AllGenerations[GenerationsDataGrid.SelectedIndex];
                OnPropertyChanged(nameof(CurrSelGeneration));
                ChromosomeDataGrid.SelectedIndex = 0;
            }
        }

        private void ChromosomeDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChromosomeDataGrid.SelectedIndex >= 0)
            {
                LayoutSelection.SelectedIndex = 5;
                if (AllGenerations.Count == 1)
                {
                    SelectedLayoutID.Content = "Gen " + GenerationSummaries.Length + ", Ch " + ChromosomeDataGrid.SelectedIndex;
                }
                else
                {
                    SelectedLayoutID.Content = "Gen " + GenerationsDataGrid.SelectedIndex + ", Ch " + ChromosomeDataGrid.SelectedIndex;
                }
                string tempLayout = CurrSelGeneration[ChromosomeDataGrid.SelectedIndex].layout;
                CurrentLayout = GeneticAlgorithm.StringToLayout(tempLayout);
                UpdateKeyboardLayout();
                if (isShowingCost)
                    isShowingCost = false;
                UpdateKeyboardEvaluation();
            }
        }

        private void CarryOverCBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            settings.carryoverType = CarryOverCBox.SelectedIndex;
        }

        private void CarryoverVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (double.TryParse(CarryoverVariable.Text, out temp))
            {
                settings.carryVar = temp;
                CarryoverVariable.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                CarryoverVariable.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void CullingCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (settings.culling)
                settings.culling = false;
            else
                settings.culling = true;
        }

        private void CullingBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            double temp;
            if (double.TryParse(CullingBox.Text, out temp))
            {
                settings.cullingRate = temp;
                CullingBox.Background = new SolidColorBrush(Color.FromArgb(85, 158, 203, 81));
            }
            else
            {
                CullingBox.Background = new SolidColorBrush(Color.FromArgb(85, 239, 132, 135));
            }
        }

        private void IsFullMemory_Checked(object sender, RoutedEventArgs e)
        {
            if (settings.fullMemory)
                settings.fullMemory = false;
            else
                settings.fullMemory = true;
        }

        private void IsAnimated_Checked(object sender, RoutedEventArgs e)
        {
            if (settings.isanimated)
                settings.isanimated = false;
            else
                settings.isanimated = true;
        }
    }
}