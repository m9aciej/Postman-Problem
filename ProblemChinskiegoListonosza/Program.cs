using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GeneticSharp.Domain.Selections;
using GeneticSharp.Domain.Crossovers;
using GeneticSharp.Domain.Mutations;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain;
using GeneticSharp.Domain.Terminations;
using System.IO;
using GeneticSharp.Domain.Chromosomes;
using System.Globalization;
using System.Diagnostics;

namespace ProblemChinskiegoListonosza
{
    class Program
    {
        public static List<Droga> drogi;
       
        static void Main(string[] args)
        {
            // grafy o różnej liczbie wierzchołków h7 h9 h20 h23 h25 h47
            string loc = Directory.GetCurrentDirectory() + "\\Grafy\\h7.txt";

            int[,] data = LoadData.FromFile(loc);
            drogi = new List<Droga>();


            for (int i = 0; i < data.GetLength(0); i++)
            {
                Droga nowaDroga = new Droga(data[i, 0], data[i, 1], data[i, 2]);
                drogi.Add(nowaDroga);
                Droga powrotnaDroga = new Droga(data[i, 1], data[i, 0], data[i, 2]);
                drogi.Add(powrotnaDroga);
            }

            System.Console.WriteLine("Lista drog :");

            foreach (Droga droga in drogi)
            {
                System.Console.WriteLine(droga.pktPoczatkowy + " " + droga.pktNastepny + " " + droga.koszt);
            }

            Console.WriteLine("liczba dróg : " + drogi.Count);
            Console.WriteLine();

            var selection = new EliteSelection();
            var crossover = new ThreeParentCrossover(); //dwóch rodziców dwojka dzieci TwoPointCrossover();//
            var mutation = new TworsMutation(); // zmiana pozycji dwóch genów bybranych losowo
            var fitness = new PostmanFitness();
            var chromosome = new PostmanChromosome(3 * drogi.Count);
            var population = new Population(30, 30, chromosome);

            var ga = new GeneticAlgorithm(population, fitness, selection, crossover, mutation);

            ga.CrossoverProbability = 0.90F;
            ga.MutationProbability = 0.05F;
            ga.Termination = new GenerationNumberTermination(800);

            Console.WriteLine("Prawdopodobieństwo krzyżowania: " + ga.CrossoverProbability);
            Console.WriteLine("Prawdopodobieństwo mutacji: " + ga.MutationProbability);
        
            Stopwatch timer = new Stopwatch();
            timer.Start();

            Console.WriteLine("Obliczanie...");
            ga.Start();
            Console.WriteLine("Koniec obliczeń");

            timer.Stop();

            bool pierwszy = true;
            int miastoStartoweIndex = Convert.ToInt32(ga.BestChromosome.GetGenes()[0].Value);
            int miastoStartowe = drogi[miastoStartoweIndex].pktPoczatkowy;
            int kosztCalkowity = 0;

            foreach (Gene gene in ga.BestChromosome.GetGenes())
            {
                int index = Convert.ToInt32(gene.Value);
                kosztCalkowity += drogi[index].koszt;
                if (PostmanFitness.czyKazdaDrogaZostalaPokonana(drogi) && drogi[index].pktNastepny == miastoStartowe)
                {
                    Console.Write("-" + drogi[index].pktNastepny.ToString());
                    break;
                }
                else
                {
                    if (pierwszy)
                    {
                        Console.Write(drogi[index].pktPoczatkowy.ToString() + "-" + drogi[index].pktNastepny.ToString());
                        pierwszy = false;
                    }
                    else
                    {
                        Console.Write("-" + drogi[index].pktNastepny.ToString());
                    }
                    drogi[index].czyDotarl = true;
                    Droga drogaPowrotna = drogi.Find(a => (a.pktPoczatkowy.ToString() + "-" + a.pktNastepny.ToString()).Equals(drogi[index].pktNastepny.ToString() + "-" + drogi[index].pktPoczatkowy.ToString()));
                    drogaPowrotna.czyDotarl = true;
                }
            }

            TimeSpan ts = timer.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            Console.WriteLine();

            Console.WriteLine("Czas obliczeń : {0}", elapsedTime);
            Console.WriteLine("Całkowity koszt przebytej trasy : {0}", kosztCalkowity);
            
            Console.ReadKey();
        }
    }
}
