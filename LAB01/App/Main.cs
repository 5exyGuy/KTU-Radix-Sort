using ConsoleTables;
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
        private int ItemNumber = 0;
        private Enviroment CurrentEnv;

        private Container Container = new Container(FileName);
        private LinkedList LinkedList = new LinkedList(FileName);

        public void startApplication()
        {
            PrintHeader();           
            ChooseItem();
        }

        private void ChooseItem()
        {
            string item = Console.ReadLine();

            if (int.TryParse(item, out ItemNumber))
            {
                switch(ItemNumber)
                {
                    case 3:
                        RunPerformanceTest();
                        return;
                    case 4:
                        Environment.Exit(0);
                        return;
                    default:
                        ReturnMain();
                        return;
                }
            } 
            else
            {
                if (item == "1m" || item == "1M")
                {
                    ItemNumber = 1;
                    CurrentEnv = Enviroment.Memory;
                    OpenStructMenu();
                    return;
                }

                if (item == "1d" || item == "1D")
                {
                    ItemNumber = 1;
                    CurrentEnv = Enviroment.Disk;
                    OpenStructMenu();
                    return;
                }

                if (item == "2m" || item == "2M")
                {
                    ItemNumber = 2;
                    CurrentEnv = Enviroment.Memory;
                    OpenStructMenu();
                    return;
                }

                if (item == "2d" || item == "2D")
                {
                    ItemNumber = 2;
                    CurrentEnv = Enviroment.Disk;
                    OpenStructMenu();
                    return;
                }

                ReturnMain();
            }
        }

        private void OpenStructMenu()
        {
            Console.Clear();

            string enviroment = CurrentEnv == Enviroment.Memory ? "Operatyvioji atmintis" : "Diskas";
            string dataStruct = ItemNumber == 1 ? "Masyvas" : "Sąrašas";

            Console.WriteLine($"┌───────────────────────────┐\r\n" +
                              string.Format("| {0, -26}", dataStruct) + "|\r\n" +
                              string.Format("| {0, -25}", enviroment) + " |\r\n" +
                              $"└───────────────────────────┘\r\n");
            Console.WriteLine("╔═ (1) Generuoti\r\n" +
                              "╠═ (2) Spausdinti\r\n" +
                              "╠═ (3) Rikiuoti\r\n" +
                              "╚═ (4) Grįžti\r\n");

            ChooseStructItem();
        }

        private void ChooseStructItem()
        {
            string item = Console.ReadLine();
            int itemNum = 0;

            if (int.TryParse(item, out itemNum))
            {
                switch (itemNum)
                {
                    case 1:
                        Generate();
                        return;
                    case 2:
                        Print();
                        return;
                    case 3:
                        Sort();
                        return;
                    case 4:
                        ReturnMain();
                        return;
                    default:
                        OpenStructMenu();
                        return;
                }
            } 
            else
            {
                OpenStructMenu();
            }
        }

        private void Generate()
        {
            Console.Clear();
            Console.WriteLine("Nurodykite elementų kiekį\r\n");

            if (ItemNumber == 1 && CurrentEnv == Enviroment.Memory)
            {
                int n = 0;
                
                if (int.TryParse(Console.ReadLine(), out n)) {
                    Container.MemoryEnv.Generate(n);
                }

                OpenStructMenu();
                return;
            }

            if (ItemNumber == 1 && CurrentEnv == Enviroment.Disk)
            {
                int n = 0;

                if (int.TryParse(Console.ReadLine(), out n))
                {
                    Container.DiskEnv.Generate(n);
                }

                OpenStructMenu();
                return;
            }

            if (ItemNumber == 2 && CurrentEnv == Enviroment.Memory)
            {
                int n = 0;

                if (int.TryParse(Console.ReadLine(), out n))
                {
                    LinkedList.MemoryEnv.Generate(n);
                }

                OpenStructMenu();
                return;
            }

            if (ItemNumber == 2 && CurrentEnv == Enviroment.Disk)
            {
                int n = 0;

                if (int.TryParse(Console.ReadLine(), out n))
                {
                    LinkedList.DiskEnv.Generate(n);
                }

                OpenStructMenu();
                return;
            }

            OpenStructMenu();
        }

        private void Print()
        {
            if (ItemNumber == 1 && CurrentEnv == Enviroment.Memory)
            {
                Console.Clear();
                Container.MemoryEnv.Print();
                Console.ReadLine();
                OpenStructMenu();
                return;
            }

            if (ItemNumber == 1 && CurrentEnv == Enviroment.Disk)
            {
                Console.Clear();
                using (Container.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    Container.DiskEnv.Print();
                }
                Console.ReadLine();
                OpenStructMenu();
                return;
            }

            if (ItemNumber == 2 && CurrentEnv == Enviroment.Memory)
            {
                Console.Clear();
                LinkedList.MemoryEnv.Print();
                Console.ReadLine();
                OpenStructMenu();
                return;
            }

            if (ItemNumber == 2 && CurrentEnv == Enviroment.Disk)
            {
                Console.Clear();
                using (LinkedList.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    LinkedList.DiskEnv.Print();
                }
                Console.ReadLine();
                OpenStructMenu();
                return;
            }

            OpenStructMenu();
        }

        private void Sort()
        {
            if (ItemNumber == 1 && CurrentEnv == Enviroment.Memory)
            {
                Container.MemoryEnv.RadixSort();
                OpenStructMenu();
                return;
            }

            if (ItemNumber == 1 && CurrentEnv == Enviroment.Disk)
            {
                using (Container.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    Container.DiskEnv.RadixSort();
                }
                OpenStructMenu();
                return;
            }

            if (ItemNumber == 2 && CurrentEnv == Enviroment.Memory)
            {
                LinkedList.MemoryEnv.RadixSort();
                OpenStructMenu();
                return;
            }

            if (ItemNumber == 2 && CurrentEnv == Enviroment.Disk)
            {
                using (LinkedList.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    LinkedList.DiskEnv.RadixSort();
                }
                OpenStructMenu();
                return;
            }

            OpenStructMenu();
        }

        private void RunPerformanceTest()
        {
            int[] size = new int[] { 500, 1000, 2000, 4000, 8000, 16000, 32000 };
            Stopwatch stopwatch = new Stopwatch();
            TimeSpan timeM, timeD;

            Console.Clear();

            /* Masyvas */
            Console.WriteLine("Masyvas");
            ConsoleTable arrayTable = new ConsoleTable("Kiekis", "Laikas (M)", "Laikas (D)", "Operacijos (M)", "Operacijos (D)");

            for (int i = 0; i < size.Length; i++)
            {
                Container.MemoryEnv.ResetOperationCount();
                Container.DiskEnv.ResetOperationCount();

                /* Laikas (M) */
                Container.MemoryEnv.Generate(size[i]);
                stopwatch.Start();
                Container.MemoryEnv.RadixSort();
                timeM = stopwatch.Elapsed;
                stopwatch.Reset();
                /* Laikas (D) */
                Container.DiskEnv.Generate(size[i]);
                stopwatch.Start();
                using (Container.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    Container.DiskEnv.RadixSort();
                }
                timeD = stopwatch.Elapsed;
                stopwatch.Reset();

                arrayTable.AddRow(size[i], timeM, timeD, Container.MemoryEnv.OperationCount, Container.DiskEnv.OperationCount);
            }

            arrayTable.Write(Format.Alternative);

            Console.WriteLine();

            /* Sąrašas */
            Console.WriteLine("Sąrašas");
            ConsoleTable listTable = new ConsoleTable("Kiekis", "Laikas (M)", "Laikas (D)", "Operacijos (M)", "Operacijos (D)");

            for (int i = 0; i < size.Length; i++)
            {
                LinkedList.MemoryEnv.ResetOperationCount();
                LinkedList.DiskEnv.ResetOperationCount();

                /* Laikas (M) */
                LinkedList.MemoryEnv.Generate(size[i]);
                stopwatch.Start();
                LinkedList.MemoryEnv.RadixSort();
                timeM = stopwatch.Elapsed;
                stopwatch.Reset();
                /* Laikas (D) */
                LinkedList.DiskEnv.Generate(size[i]);
                stopwatch.Start();
                using (LinkedList.DiskEnv.FileStream = new FileStream(FileName, FileMode.Open, FileAccess.ReadWrite))
                {
                    LinkedList.DiskEnv.RadixSort();
                }
                timeD = stopwatch.Elapsed;
                stopwatch.Reset();

                listTable.AddRow(size[i], timeM, timeD, LinkedList.MemoryEnv.OperationCount, LinkedList.DiskEnv.OperationCount);
            }

            listTable.Write(Format.Alternative);

            Console.ReadLine();
            ReturnMain();
        }

        private void ReturnMain()
        {
            ItemNumber = 0;
            Console.Clear();
            PrintHeader();
            ChooseItem();
        }

        private void PrintHeader()
        {
            Console.WriteLine($"┌───────────────────────────┐\r\n" +
                              $"|       Valdymo panelė      |\r\n" +
                              $"└───────────────────────────┘\r\n");
            Console.WriteLine("╔═ (1) Masyvas ══ (m - Operatyvioji atmintis, d - Diskas)\r\n" +
                              "╠═ (2) Sąrašas ══ (m - Operatyvioji atmintis, d - Diskas)\r\n" +
                              "╠═ (3) Greitaveika\r\n" +
                              "╚═ (4) Uždaryti\r\n");
        }
    }
}
