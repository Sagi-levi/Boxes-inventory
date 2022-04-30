using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLService.InnerData
{
    class Box
    {
        public DataX DataX { get; set; }
        public DataY DataY { get; set; }

        public int AmountToPurchse { get; set; }
        public Box(DataX dataX,DataY dataY,int amount)
        {
            DataX = dataX;
            DataY = dataY;
            AmountToPurchse = amount;
        }
        public override string ToString()
        {
            return $"the box is x: {DataX.X},y:{DataY.Y} and we have {AmountToPurchse}from it";
        }
    }
}
