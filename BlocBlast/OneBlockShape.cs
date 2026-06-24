using System.Collections.Generic;
using System.Windows;

namespace BlockBlast
{
    public class OneBlockShape : Shape
    {
        public override List<Point> GetCells()
        {
            return new()
            {
                new Point(0,0)
            };
        }
    }
}