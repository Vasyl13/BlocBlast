using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BlockBlast
{
    public interface IBlockBlastGame
    {
        int Size { get; }
        int Score { get; }
        int Best { get; }
        int[,] Board { get; }

        IReadOnlyList<ShapeSlot> Shapes { get; }

        List<Point> ActiveShape { get; }
        Brush ActiveColor { get; }
        int ActiveIndex { get; }

        void LoadBest();
        void StartNewGame();
        bool SelectShape(int index);
        PlacementResult TryPlaceSelectedShape(int startX, int startY);
        bool CanPlace(List<Point> shape, int startX, int startY);
    }

    public sealed class ShapeSlot
    {
        public List<Point>? Cells { get; set; }
        public Brush? Color { get; set; }
    }

    public sealed class PlacementResult
    {
        public bool Placed { get; set; }
        public bool GameOver { get; set; }
        public int Score { get; set; }
        public int Best { get; set; }
    }
}