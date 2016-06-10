using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisToolkit.Histograms
{
    public class Histogram<TVal> : SortedDictionary<TVal, int>
    {

        public Histogram()
        {

        }


        public void Add()
        {

        }

        public void Remove()
        {

        }

        public static Histogram<string> Create(string s, string[] values)
        {
            int zero = 0;
            int i = zero, j;
            int iLength = values.Length;
            int jLength = s.Length;
            int count = zero;
            string value;
            int vLength;
            int vjLength;
            Histogram<string> hist = new Histogram<string>();
            for (; i < iLength; i++)
            {
                value = values[i];
                vLength = value.Length;
                vjLength = jLength - vLength;
                count = zero;
                for (j = zero; j < vjLength; j++)
                {
                    if (s.Substring(j, vLength) == value)
                    {
                        count++;
                    }
                }
                hist.Add(value, count);
            }
            return hist;
        }


    }
}
