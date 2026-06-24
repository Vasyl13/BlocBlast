using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BlockBlast
{
    public class ShapeFactory : IShapeFactory
    {
        private readonly Random _rnd = new();

        private readonly List<Shape> _shapes = new()
        {
            new OneBlockShape(),
            new TwoBlockShape(),
            new ThreeBlockShape(),
            new SquareShape(),
            new LShape(),
            new MirrorLShape(),
            new TShape()
        };

        private readonly Brush[] _colors =
        {
            Brushes.DeepSkyBlue,
            Brushes.Gold,
            Brushes.LimeGreen,
            Brushes.HotPink,
            Brushes.Orange,
            Brushes.Violet
        };

        public List<Point> CreateRandomShape()
        {
            Shape shape = _shapes[_rnd.Next(_shapes.Count)];

            return shape.GetCells();
        }

        public Brush CreateRandomColor()
        {
            return _colors[_rnd.Next(_colors.Length)];
        }
    }
}