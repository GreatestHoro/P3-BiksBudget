using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler_test
{
    public class Opskrift
    {
        public string _Name;
        public string _beskrivelse;
        public List<string> _Ingrediens;

        public Opskrift(string name, string beskrivese, List<string> Ingrediens)
        {
            _Name = name;
            _beskrivelse = beskrivese;
            _Ingrediens = Ingrediens;

        }

        public static void SaveRecipie(List<Opskrift> opskrifts)
        {
            using (FileStream fs = File.Create(@"D:\kogebog.txt"))
            {
                foreach (var op in opskrifts)
                {
                    // Add some text to file    
                    Byte[] navn = new UTF8Encoding(true).GetBytes("$" + op._Name);
                    fs.Write(navn, 0, navn.Length);
                    string comptest = "@";
                    foreach (var ind in op._Ingrediens)
                    {
                        comptest = comptest + "%" + ind + "@";

                    }
                    byte[] ingrdiens = new UTF8Encoding(true).GetBytes(comptest.Replace("@", " " + System.Environment.NewLine));
                    fs.Write(ingrdiens, 0, ingrdiens.Length);

                    byte[] Beskrivels = new UTF8Encoding(true).GetBytes("¤" + op._beskrivelse + "¤");
                    fs.Write(Beskrivels, 0, Beskrivels.Length);
                    byte[] end = new UTF8Encoding(true).GetBytes("------------------------------------------------------------------------------------");
                    fs.Write(end, 0, end.Length);

                    byte[] endspace = new UTF8Encoding(true).GetBytes(System.Environment.NewLine);
                    fs.Write(endspace, 0, endspace.Length);
                }
            }
        }
    }
}
