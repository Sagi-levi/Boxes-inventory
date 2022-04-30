using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace BLService.InnerData
{
    class DataX : IComparable<DataX>
    {
        public double X { get; set; }
        public BST<DataY> YTree { get; set; }
        public double MaxY { get; set; }
        public DataX(double x)
        {
            X = x;
            YTree = new BST<DataY>();
        }

        public int CompareTo(DataX other)
        {
            if (other != null)
                return this.X.CompareTo(other.X);
            else
                throw new ArgumentException("Object is not a DataX");
        }
    }
}
