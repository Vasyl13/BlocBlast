using BlocBlast;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BlockBlast
{
    public sealed class ShapeFactory : IShapeFactory
    {
        private readonly Random _rnd = new Random();

        private readonly Brush[] _colors =
        {
            Brushes.DeepSkyBlue,
            Brushes.Gold,
            Brushes.LimeGreen,
            Brushes.HotPink,
            Brushes.Orange,
            Brushes.Violet
        };

        private readonly List<List<Point>> _allShapes = new List<List<Point>>
        {
            new() { new Point(0,0) },

            new()
            {
                new Point(0,0),
                new Point(1,0)
            },

            new()
            {
                new Point(0,0),
                new Point(0,1)
            },

            new()
            {
                new Point(0,0),
                new Point(1,0),
                new Point(2,0)
            },

            new()
            {
                new Point(0,0),
                new Point(0,1),
                new Point(0,2)
            },

            new()
            {
                new Point(0,0),
                new Point(1,0),
                new Point(0,1),
                new Point(1,1)
            },

            new()
            {
                new Point(0,0),
                new Point(1,0),
                new Point(2,0),
                new Point(1,1)
            },

            new()
            {
                new Point(0,0),
                new Point(1,0),
                new Point(2,0),
                new Point(3,0)
            },

            new()
            {
                new Point(0,0),
                new Point(0,1),
                new Point(1,1)
            },

            new()
            {
                new Point(0,0),
                new Point(1,0),
                new Point(1,1)
            }
        };

        public List<Point> CreateRandomShape()
        {
            var source = _allShapes[_rnd.Next(_allShapes.Count)];
            return new List<Point>(source);
        }

        public Brush CreateRandomColor()
        {
            return _colors[_rnd.Next(_colors.Length)];
        }
    }
}