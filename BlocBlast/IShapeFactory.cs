using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BlockBlast
{
    public interface IShapeFactory
    {
        List<Point> CreateRandomShape();
        Brush CreateRandomColor();
    }
}