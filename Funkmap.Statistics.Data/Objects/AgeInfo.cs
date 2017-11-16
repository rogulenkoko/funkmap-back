using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Funkmap.Statistics.Data.Objects
{
    public class AgeInfo
    {
        public AgeInfo(int Start, int Finish)
        {
            this.Finish = Finish;
            this.Start = Start;
            Descr = this.ToString();
        }
        public int Start;
        public int Finish;
        public string Descr;
        public override string ToString()
        {
            var text = Start.ToString() + " - " + Finish.ToString();
            return text;
        }
    }
}
