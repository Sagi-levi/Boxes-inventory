using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLService.InnerData
{
    class BoxByTime
    {
        public double X { get; set; }
        public double Y { get; set; }
        public DateTime Date { get; set; }
        public BoxByTime(double x, double y)
        {
            X = x;
            Y = y;
            Date = DateTime.Now;
        }
        public override string ToString()
        {
            return $"X size: {X}, Y size: {Y}";
        }

    }
}
