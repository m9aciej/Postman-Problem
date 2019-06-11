using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace ProblemChinskiegoListonosza
{
    class PostmanFitness : IFitness
    {
        public static bool czyKazdaDrogaZostalaPokonana(List<Droga> drogi)
        {
            foreach (var droga in drogi)
            {
                if (droga.czyDotarl == false)
                {
                    return false;
                }
            }
            return true;
        }
        public double Evaluate(IChromosome chromosome)
        {
            List<Droga> listaDrog = Program.drogi;
            var geny = chromosome.GetGenes();
            var sumaOdleglosci = 0.0;
            var ostatni = 0;
            for (int i = 0; i < geny.Length; i++)
            {
                var IndexDrogi = Convert.ToInt32(geny[i].Value, CultureInfo.InvariantCulture);
                Droga drogaPowrotna = listaDrog.Find(a => (a.pktPoczatkowy.ToString() + "-" + a.pktNastepny.ToString()).Equals(listaDrog[IndexDrogi].pktNastepny.ToString() + "-" + listaDrog[IndexDrogi].pktPoczatkowy.ToString()));
                listaDrog[IndexDrogi].czyDotarl = true; // ustawienie drogi jako przebytą
                drogaPowrotna.czyDotarl = true; // ustawienie odwrotności drogi jako przebytą

                if (i != 0 && listaDrog[IndexDrogi].pktPoczatkowy != listaDrog[Convert.ToInt32(geny[i - 1].Value)].pktNastepny)
                {
                    sumaOdleglosci += listaDrog[IndexDrogi].koszt * 1000;            
                }
                else
                {
                    sumaOdleglosci += listaDrog[IndexDrogi].koszt;
                }
                if (czyKazdaDrogaZostalaPokonana(listaDrog))
                {
                    if (listaDrog[IndexDrogi].pktNastepny == listaDrog[Convert.ToInt32(geny[0].Value)].pktPoczatkowy)
                    {
                        break;
                    }

                }
                ostatni = IndexDrogi;
            }
            if (!czyKazdaDrogaZostalaPokonana(listaDrog) || (listaDrog[ostatni].pktPoczatkowy != listaDrog[Convert.ToInt32(geny[0].Value)].pktPoczatkowy))
            {
                sumaOdleglosci = sumaOdleglosci * 999;
            }
            foreach (Droga droga in Program.drogi)
            {
                droga.czyDotarl = false;
            }

            var fitness = 1.0 / sumaOdleglosci;


            return fitness;
        }
    }
}
