using System.Collections.Generic;
using System.Windows;

namespace BlockBlast
{
    public class TwoBlockShape : Shape
    {
        public override List<Point> GetCells()
        {
            return new()
            {
                new Point(0,0),
                new Point(1,0)
            };
        }
    }
}