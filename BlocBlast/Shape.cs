using System.Collections.Generic;
using System.Windows;

namespace BlockBlast
{
    public abstract class Shape
    {
        public abstract List<Point> GetCells();
    }
}