using LAB01.App.Data_Structures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB01.App
{
    class Main
    {
        public const string FileName = "data.dat";

        public string Author { private set; get; }
        public int LabNum { private set; get; }
        public string Header { private set; get; }

        public Main(string author, int labNum)
        {
            Author = author;
            LabNum = labNum;
            Header = $"{string.Format("{0, 20} {1, 30}", "Autorius: ", $"{author}")}\n" +
                $"{string.Format("{0, 20} {1, 30}", "Darbo numeris: ", $"{labNum}")}\n";
        }

        public void startApplication()
        {
            Stopwatch stopwatch = new Stopwatch();

            Console.WriteLine(Header);

            //Container container = new Container(FileName);
            //container.DiskEnv.Generate(10);

            //using (container.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
            //{
            //    container.DiskEnv.Print();
            //    stopwatch.Start();
            //    container.DiskEnv.RadixSort();
            //    stopwatch.Stop();
            //    Console.WriteLine();
            //    container.DiskEnv.Print();
            //    Console.WriteLine($"Laikas: {stopwatch.ElapsedMilliseconds} ms");
            //    stopwatch.Reset();
            //}

            LinkedList linkedList = new LinkedList(FileName);
            linkedList.DiskEnv.Generate(10);

            using (linkedList.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
            {
                linkedList.DiskEnv.Print();
                Console.WriteLine();
                linkedList.DiskEnv.RadixSort();
                linkedList.DiskEnv.Print();
                //linkedList.DiskEnv.RadixSort();
                //stopwatch.Start();
                //linkedList.DiskEnv.RadixSort();
                //stopwatch.Stop();
                //Console.WriteLine();
                //linkedList.DiskEnv.Print();
                //Console.WriteLine($"Laikas: {stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}
