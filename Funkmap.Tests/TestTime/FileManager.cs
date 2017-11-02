using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace Funkmap.Tests.TestTime
{
   public class FileManager
    {
        private string part = ".//time//";

        public FileManager()
        {
            DirectoryInfo directory = new DirectoryInfo(".//time");
            if (!directory.Exists)
                directory.Create();
            string[] nameFile =
            {
                "GetUserEntitiesCountInfo",
                "Update",
                "CheckIfLoginExist",
                "GetAllFilteredLogins",
                "GetFiltered",
                "getAll",
                "getnearest",
                "GetFullNearest",
                "GetSpecific",
                "GetUserEntitiesLogins",
                "GetAllAsync",
                "GetAsync",
                "CreateAsync",
                "DeleteAsync",
                "GetSpecificFullAsync"
            };
            foreach (string name in nameFile)
            {
                FileInfo fileInfo = new FileInfo(part + name + ".txt");
                if (!fileInfo.Exists)
                    fileInfo.Create();
            }
        }

        public void CreatingAndWritingText(string nameText, Stopwatch stopwatch, string description)
        {
            DateTime time = DateTime.Now;
            FileInfo outFile = new FileInfo(part + nameText + ".txt");
            if (!outFile.Exists)
                outFile.Create();



            string text =
                "**************** \n" +
                time + "\n" +
                "in Milliseconds \n" +
                "time = " + stopwatch.Elapsed.Milliseconds + "\n" +
                +(stopwatch.Elapsed.Milliseconds - GetMinTime(outFile)) + "\n" +
                description + "\n";
            using (StreamWriter sw = new StreamWriter(outFile.FullName, true))
            {
                sw.Write(text);
                sw.Close();
            }
        }

        private int GetMinTime(FileInfo file)
        {
            int minTime = new int();
            minTime = 1000;
            using (StreamReader sr = new StreamReader(file.FullName))
            {
                while (sr.Peek() != -1)
                {
                    string text = sr.ReadLine();
                    if (text.Length > 7 && text.Substring(0, 7).Equals("time = "))
                    {
                        int time = text.Substring(7, text.Length - 7).ToInt();
                        if (time < minTime)
                            minTime = time;
                    }
                }
            }
            return minTime;
        }
    }
}
