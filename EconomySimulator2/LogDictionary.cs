using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EconomySimulator2
{
    public class LogDictionary : Dictionary<int,double>
    {
        public new void Add(int key, double value)
        {
            if (Count > 0)
            {
                for (int k = key - 1; !ContainsKey(k); k--)
                {
                    base.Add(k, 0);
                }
            }
            else
            {
                for (int k = key - 1; k>=0; k--)
                {
                    base.Add(k, 0);
                }
            }
            base.Add(key, value);
        }

        public void ToGraph(out double[] xpointer,out double[] ypointer,int maximum)
        {
            //Debug.Print("tograph");
            xpointer = new double[Count > maximum ? maximum : Count];
            ypointer = new double[Count > 100 ? 100 : Count];

            for (int i = 0; i < xpointer.Length; i++)
            {
                int index = Count - xpointer.Length + i;
                xpointer[i] = index;
                ypointer[i] = this[index];
            }
        }
    }
}
