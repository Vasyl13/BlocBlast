using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BlockBlast
{
    public partial class MainWindow : Window
    {
        private const int Size = 8;
        private const int CellSize = 62;
        private const int BlockSize = 58;

        private readonly IBlockBlastGame _game;

        private Border[,] cells = new Border[Size, Size];

        public MainWindow()
        {
            InitializeComponent();

            _game = new BlockBlastGame(
                new ShapeFactory(),
                new FileBestScoreStore("bestscore.txt"));

            _game.LoadBest();

            CreateBoard();
            _game.StartNewGame();
            RenderBoard();
            RenderShapes();
            UpdateHud();

            ShapeCanvas1.MouseLeftButtonDown += Shape_MouseDown;
            ShapeCanvas2.MouseLeftButtonDown += Shape_MouseDown;
            ShapeCanvas3.MouseLeftButtonDown += Shape_MouseDown;

            BoardGrid.MouseMove += Board_MouseMove;
            BoardGrid.MouseLeftButtonUp += Board_MouseUp;
        }

        private void UpdateHud()
        {
            ScoreText.Text = _game.Score.ToString();
            BestText.Text = _game.Best.ToString();
        }

        private void CreateBoard()
        {
            BoardGrid.Children.Clear();

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    Border b = new Border
                    {
                        Width = BlockSize,
                        Height = BlockSize,
                        Margin = new Thickness(2),
                        CornerRadius = new CornerRadius(10),
                        Background = new SolidColorBrush(Color.FromRgb(60, 60, 90))
                    };

                    cells[x, y] = b;
                    BoardGrid.Children.Add(b);
                }
            }
        }

        private void RenderBoard()
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    cells[x, y].Background =
                        _game.Board[x, y] == 1
                            ? cells[x, y].Background
                            : new SolidColorBrush(Color.FromRgb(60, 60, 90));
                }
            }

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (_game.Board[x, y] == 1)
                    {
                        cells[x, y].Background =
                            new SolidColorBrush(Color.FromRgb(80, 160, 255));
                    }
                }
            }
        }

        private void RenderShapes()
        {
            DrawShape(ShapeCanvas1, _game.Shapes[0]);
            DrawShape(ShapeCanvas2, _game.Shapes[1]);
            DrawShape(ShapeCanvas3, _game.Shapes[2]);
        }

        private void DrawShape(Canvas canvas, ShapeSlot slot)
        {
            canvas.Children.Clear();

            if (slot.Cells == null)
                return;

            foreach (Point p in slot.Cells)
            {
                Border b = new Border
                {
                    Width = 42,
                    Height = 42,
                    Background = slot.Color,
                    CornerRadius = new CornerRadius(8)
                };

                Canvas.SetLeft(b, p.X * 44 + 35);
                Canvas.SetTop(b, p.Y * 44 + 35);

                canvas.Children.Add(b);
            }
        }

        private void Shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender == ShapeCanvas1)
                _game.SelectShape(0);
            else if (sender == ShapeCanvas2)
                _game.SelectShape(1);
            else if (sender == ShapeCanvas3)
                _game.SelectShape(2);
        }

        private void Board_MouseMove(object sender, MouseEventArgs e)
        {
            if (_game.ActiveShape == null)
                return;

            PreviewCanvas.Children.Clear();

            Point pos = e.GetPosition(BoardGrid);

            int gridX = (int)(pos.X / CellSize);
            int gridY = (int)(pos.Y / CellSize);

            foreach (Point p in _game.ActiveShape)
            {
                int x = gridX + (int)p.X;
                int y = gridY + (int)p.Y;

                if (x < 0 || y < 0 || x >= Size || y >= Size)
                    continue;

                Border b = new Border
                {
                    Width = BlockSize,
                    Height = BlockSize,
                    Background = _game.ActiveColor,
                    Opacity = 0.45,
                    CornerRadius = new CornerRadius(10)
                };

                Canvas.SetLeft(b, x * CellSize + 2);
                Canvas.SetTop(b, y * CellSize + 2);

                PreviewCanvas.Children.Add(b);
            }
        }

        private void Board_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (_game.ActiveShape == null)
                return;

            Point pos = e.GetPosition(BoardGrid);

            int gridX = (int)(pos.X / CellSize);
            int gridY = (int)(pos.Y / CellSize);

            PlacementResult result = _game.TryPlaceSelectedShape(gridX, gridY);

            PreviewCanvas.Children.Clear();

            if (result.Placed)
            {
                RenderBoard();
                RenderShapes();
                UpdateHud();

                if (result.GameOver)
                {
                    MessageBox.Show($"Game Over!\nScore: {result.Score}");
                    RestartGame();
                }
            }
        }

        private void RestartGame()
        {
            _game.StartNewGame();
            CreateBoard();
            RenderBoard();
            RenderShapes();
            UpdateHud();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            MenuPanel.Visibility = Visibility.Collapsed;
            GamePanel.Visibility = Visibility.Visible;

            RestartGame();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            GamePanel.Visibility = Visibility.Collapsed;
            MenuPanel.Visibility = Visibility.Visible;
        }
    }
}