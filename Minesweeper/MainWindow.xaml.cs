using System;
using System.Windows;
using System.Windows.Controls;

namespace Minesweeper
{
    public partial class MainWindow : Window
    {
        private const int GridSize = 10;         // 10x10 mező
        private const int MineCount = 10;        // 10 akna
        private Button[,] buttons = new Button[GridSize, GridSize];
        private bool[,] hasMine = new bool[GridSize, GridSize];

        public MainWindow()
        {
            InitializeComponent();
            GenerateGrid();
            PlaceMines();
        }

        private void GenerateGrid()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    Button button = new Button();
                    button.Tag = (row, col);
                    button.Click += Cell_Click;
                    buttons[row, col] = button;
                    MinefieldGrid.Children.Add(button);
                }
            }
        }

        private void PlaceMines()
        {
            Random rand = new Random();
            int placed = 0;
            while (placed < MineCount)
            {
                int row = rand.Next(GridSize);
                int col = rand.Next(GridSize);
                if (!hasMine[row, col])
                {
                    hasMine[row, col] = true;
                    placed++;
                }
            }
        }

        private void Cell_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            var (row, col) = ((int, int))clickedButton.Tag;

            if (hasMine[row, col])
            {
                clickedButton.Content = "💣";
                clickedButton.Background = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Game Over!");
                RevealAll();
            }
            else
            {
                int mineCount = CountAdjacentMines(row, col);
                clickedButton.Content = mineCount.ToString();
                clickedButton.IsEnabled = false;
            }
        }

        private int CountAdjacentMines(int row, int col)
        {
            int count = 0;
            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (r >= 0 && r < GridSize && c >= 0 && c < GridSize)
                    {
                        if (hasMine[r, c]) count++;
                    }
                }
            }
            return count;
        }

        private void RevealAll()
        {
            for (int row = 0; row < GridSize; row++)
            {
                for (int col = 0; col < GridSize; col++)
                {
                    if (hasMine[row, col])
                    {
                        buttons[row, col].Content = "💣";
                        buttons[row, col].Background = System.Windows.Media.Brushes.LightGray;
                    }
                    buttons[row, col].IsEnabled = false;
                }
            }
        }
    }
}
