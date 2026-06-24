using System.Collections.Generic;
using System.Windows;

namespace BlockBlast
{
    public class TShape : Shape
    {
        public override List<Point> GetCells()
        {
            return new()
            {
                new Point(0,1),
                new Point(1,1),
                new Point(2,1),
                new Point(1,0)
            };
        }
    }
}