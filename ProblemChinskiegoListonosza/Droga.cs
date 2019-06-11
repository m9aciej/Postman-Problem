using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemChinskiegoListonosza
{
    public class Droga
    {
        public int pktPoczatkowy { get; set; }
        public int pktNastepny { get; set; }
        public int koszt { get; set; }
        public bool czyDotarl { get; set; }

        public Droga(int pktPoczatkowy, int pktNastepny, int koszt, bool czyDotarl = false)
        {
            this.pktPoczatkowy = pktPoczatkowy;
            this.pktNastepny = pktNastepny;
            this.koszt = koszt;
            this.czyDotarl = czyDotarl;
        }
    }
}



