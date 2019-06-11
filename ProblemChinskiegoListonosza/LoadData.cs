using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProblemChinskiegoListonosza
{
    static class LoadData
    {
        public static int[,] FromFile(string sciezka)
        {
            string[] fileLineContent = File.ReadAllLines(sciezka);
            int[,] dataContent = new int[fileLineContent.Length, 3];
            for (int i = 0; i < fileLineContent.Length; i++)
            {
                string[] splitted = fileLineContent[i].Split(' ');
                for (int j = 0; j < 3; j++)
                {
                    dataContent[i, j] = Int32.Parse(splitted[j]);
                }
            }
            return dataContent;
        }
    }
}
