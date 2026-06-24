using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BlockBlast
{
    public sealed class BlockBlastGame : IBlockBlastGame
    {
        private const int DefaultSize = 8;

        private readonly ShapeFactory _shapeFactory;
        private readonly IBestScoreStore _bestScoreStore;

        private readonly ShapeSlot[] _slots = new ShapeSlot[3]
        {
            new ShapeSlot(),
            new ShapeSlot(),
            new ShapeSlot()
        };

        private int[,] _board;
        private int _score;
        private int _best;

        private List<Point>? _activeShape;
        private Brush _activeColor = Brushes.Transparent;
        private int _activeIndex = -1;

        public int Size => DefaultSize;
        public int Score => _score;
        public int Best => _best;
        public int[,] Board => _board;
        public IReadOnlyList<ShapeSlot> Shapes => _slots;

        public List<Point> ActiveShape => _activeShape!;
        public Brush ActiveColor => _activeColor;
        public int ActiveIndex => _activeIndex;

        public BlockBlastGame(ShapeFactory shapeFactory, IBestScoreStore bestScoreStore)
        {
            _shapeFactory = shapeFactory;
            _bestScoreStore = bestScoreStore;
            _board = new int[DefaultSize, DefaultSize];
        }

        public void LoadBest()
        {
            _best = _bestScoreStore.Load();
        }

        public void StartNewGame()
        {
            _board = new int[DefaultSize, DefaultSize];
            _score = 0;

            ClearSelection();

            for (int i = 0; i < _slots.Length; i++)
            {
                _slots[i].Cells = null;
                _slots[i].Color = null;
            }

            GenerateShapesIfNeeded();
        }

        public bool SelectShape(int index)
        {
            if (index < 0 || index >= _slots.Length)
                return false;

            if (_slots[index].Cells == null)
                return false;

            _activeIndex = index;
            _activeShape = _slots[index].Cells;
            _activeColor = _slots[index].Color ?? Brushes.Transparent;

            return true;
        }

        public PlacementResult TryPlaceSelectedShape(int startX, int startY)
        {
            var result = new PlacementResult
            {
                Score = _score,
                Best = _best,
                Placed = false,
                GameOver = false
            };

            if (_activeShape == null || _activeIndex < 0)
            {
                ClearSelection();
                return result;
            }

            if (!CanPlace(_activeShape, startX, startY))
            {
                ClearSelection();
                return result;
            }

            PlaceShape(startX, startY);
            _score += _activeShape.Count * 10;

            _slots[_activeIndex].Cells = null;
            _slots[_activeIndex].Color = null;

            ClearFullLines();

            if (AllShapesUsed())
                GenerateShapesIfNeeded();

            if (_score > _best)
            {
                _best = _score;
                _bestScoreStore.Save(_best);
            }

            result.Placed = true;
            result.Score = _score;
            result.Best = _best;
            result.GameOver = !HasMoves();

            ClearSelection();
            return result;
        }

        public bool CanPlace(List<Point> shape, int startX, int startY)
        {
            foreach (Point p in shape)
            {
                int x = startX + (int)p.X;
                int y = startY + (int)p.Y;

                if (x < 0 || y < 0 || x >= DefaultSize || y >= DefaultSize)
                    return false;

                if (_board[x, y] == 1)
                    return false;
            }

            return true;
        }

        private void PlaceShape(int startX, int startY)
        {
            foreach (Point p in _activeShape!)
            {
                int x = startX + (int)p.X;
                int y = startY + (int)p.Y;

                _board[x, y] = 1;
            }
        }

        private void ClearFullLines()
        {
            List<int> fullRows = new List<int>();
            List<int> fullCols = new List<int>();

            for (int y = 0; y < DefaultSize; y++)
            {
                bool full = true;
                for (int x = 0; x < DefaultSize; x++)
                {
                    if (_board[x, y] == 0)
                    {
                        full = false;
                        break;
                    }
                }

                if (full)
                    fullRows.Add(y);
            }

            for (int x = 0; x < DefaultSize; x++)
            {
                bool full = true;
                for (int y = 0; y < DefaultSize; y++)
                {
                    if (_board[x, y] == 0)
                    {
                        full = false;
                        break;
                    }
                }

                if (full)
                    fullCols.Add(x);
            }

            foreach (int y in fullRows)
            {
                for (int x = 0; x < DefaultSize; x++)
                    _board[x, y] = 0;
            }

            foreach (int x in fullCols)
            {
                for (int y = 0; y < DefaultSize; y++)
                    _board[x, y] = 0;
            }

            _score += (fullRows.Count + fullCols.Count) * 100;
        }

        private bool HasMoves()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Cells == null)
                    continue;

                for (int y = 0; y < DefaultSize; y++)
                {
                    for (int x = 0; x < DefaultSize; x++)
                    {
                        if (CanPlace(_slots[i].Cells!, x, y))
                            return true;
                    }
                }
            }

            return false;
        }

        private bool AllShapesUsed()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Cells != null)
                    return false;
            }

            return true;
        }

        private void GenerateShapesIfNeeded()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Cells == null)
                {
                    _slots[i].Cells = _shapeFactory.CreateRandomShape();
                    _slots[i].Color = _shapeFactory.CreateRandomColor();
                }
            }
        }

        private void ClearSelection()
        {
            _activeShape = null;
            _activeColor = Brushes.Transparent;
            _activeIndex = -1;
        }
    }
}