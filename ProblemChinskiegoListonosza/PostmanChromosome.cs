using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;

namespace ProblemChinskiegoListonosza
{
    class PostmanChromosome : ChromosomeBase
    {
        private int liczbaGenow;

        public PostmanChromosome(int _liczbaGenow) : base(_liczbaGenow)
        {
            List<Droga> drogi = Program.drogi;
            liczbaGenow = _liczbaGenow;
            int[] IndeksyDrog = new int[liczbaGenow]; 

            Random random = new Random();
            IndeksyDrog[0] = random.Next(drogi.Count); // Losowanie pierwszej drogi
            ReplaceGene(0, new Gene(IndeksyDrog[0]));

            for (int i = 1; i < liczbaGenow; i++)
            {
                List<Droga> szukanieDrog = drogi.Where(a => a.pktPoczatkowy.Equals(drogi[IndeksyDrog[i - 1]].pktNastepny)).ToList(); //nowa lista dróg w których są odpowiednie połączenia
                int indeksWylowowanejDrogi = random.Next(szukanieDrog.Count); //losowanie drogi z pozostałych dróg
                IndeksyDrog[i] = drogi.IndexOf(szukanieDrog[indeksWylowowanejDrogi]); // uswawianie drogi jako następna                                                                                      
                ReplaceGene(i, new Gene(IndeksyDrog[i]));
            }
        }

        public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(RandomizationProvider.Current.GetInt(0, liczbaGenow));
        }

        public override IChromosome CreateNew()
        {
            return new PostmanChromosome(liczbaGenow);
        }

        public override IChromosome Clone()
        {
            var clone = base.Clone() as PostmanChromosome;
            return clone;
        }
    }
}
