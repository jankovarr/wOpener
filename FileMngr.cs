using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wOpener
{
    public static class FileMngr
    {
        class trsCount
        {
            string trsName;
            int count;
        }

        static List<trsCount> trsList = new List<trsCount>();
        static List<string> trsRaw = new List<string>();
        static string path = @"c:\Users\i347980\source\repos\wOpener\wOpener\bin\Release\";

        public static void ReadTransactions()
        {

            string[] lines = System.IO.File.ReadAllLines
                (path + "transactions.txt");
            foreach (string line in lines)
            {
                trsRaw.Add(line.Split('\t')[0]);                
            }
        }

        static Dictionary<string, List<string>> parsList = new Dictionary<string, List<string>>();
        static List<string> Pars = new List<string>();

        public static void ReadPars()
        {
            foreach (string trs in trsRaw)
            {
                string[] lines = System.IO.File.ReadAllLines
                 (path + trs + ".txp");
                foreach (string line in lines)
                {
                    if (!parsList.ContainsKey(trs))
                        parsList.Add(trs, new List<string>(line.Split(';')));
                }
            }
        }

        public static bool IsTrsThere(string trs)
        {
            return parsList.ContainsKey(trs);
        }

        public static List<string> givePars(string trs)
        {
            return parsList[trs];
        }

        public static string gluePars(Dictionary<string, string> inMatrix)
        {
            string ret = string.Empty;
            foreach (var rec in inMatrix)
            {
               ret = rec.Key + "=" + rec.Value + ";";
            }
            return ret;
        }


    }
}
