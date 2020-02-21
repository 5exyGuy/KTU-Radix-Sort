using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB01.App.Data_Structures
{
    class LinkedList
    {
        public Memory MemoryEnv { private set; get; }
        public Disk DiskEnv { private set; get; }

        public LinkedList(string fileName)
        {
            MemoryEnv = new Memory();
            DiskEnv = new Disk(fileName);
        }

        public class Memory
        {
            public Node Head { private set; get; }
            public Node Tail { private set; get; }
            public int Count { private set; get; }
            public int OperationCount { private set; get; }

            public Memory() { }

            public void ResetOperationCount()
            {
                OperationCount = 0;
            }

            public void Generate(int n)
            {
                Random random = new Random(2019);
                for (int i = 0; i < n; i++)
                {
                    Append(random.Next(Int32.MaxValue));
                }
            }

            public void Append(int data)
            {
                Node node = new Node(data, Count);

                if (Count == 0)
                {
                    Head = node;
                }
                else
                {
                    Tail.Next = node;
                    node.Prev = Tail;
                }

                Tail = node;
                Count++;
                return;
            }

            public void RadixSort()
            {
                int m = GetMax();

                for (int exp = 1; m / exp > 0; exp *= 10)
                {
                    CountSort(exp);
                    OperationCount += 2;
                }

                OperationCount += 2;
            }

            private int GetMax()
            {
                int mx = Head.Data;

                for (Node node = Head; node != null; node = node.Next)
                {
                    if (node.Data > mx)
                    {
                        mx = node.Data;
                        OperationCount++;
                    }

                    OperationCount += 2;
                }

                OperationCount += 3;

                return mx;
            }

            private void CountSort(int exp)
            {
                int[] output = new int[Count];
                int[] count = new int[10];

                for (int i = 0; i < 10; i++)
                {
                    count[i] = 0;
                    OperationCount += 2;
                }

                for (Node node = Head; node != null; node = node.Next)
                {
                    count[(node.Data / exp) % 10]++;
                    OperationCount += 2;
                }

                for (int i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                    OperationCount += 2;
                }

                for (Node node = Tail; node != null; node = node.Prev)
                {
                    output[count[(node.Data / exp) % 10] - 1] = node.Data;
                    count[(node.Data / exp) % 10]--;
                    OperationCount += 3;
                }

                for (Node node = Head; node != null; node = node.Next)
                {
                    node.Data = output[node.Index];
                    OperationCount += 2;
                }

                OperationCount += 7;
            }

            public void Print()
            {
                for (Node node = Head; node != null; node = node.Next)
                {
                    Console.WriteLine(node.Data);
                }
            }
        }

        public class Disk
        {
            public string FileName { private set; get; }
            public FileStream FileStream { set; get; }
            public int Count { private set; get; }
            public int CurrentNode { private set; get; }
            public int Index { private set; get; }
            public int NextNode { private set; get; }
            public int PrevNode { private set; get; }
            public int OperationCount { private set; get; }

            public Disk(string fileName)
            {
                FileName = fileName;
            }

            public void ResetOperationCount()
            {
                OperationCount = 0;
            }

            public int Head()
            {
                Byte[] data = new Byte[12];
                Index = 0;
                FileStream.Seek(Index, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                // Previous Node Data
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                CurrentNode = BitConverter.ToInt32(data, 4);
                // Next Node Data
                NextNode = BitConverter.ToInt32(data, 8);

                OperationCount += 8;
                
                return CurrentNode;
            }

            private int Tail()
            {
                Byte[] data = new Byte[12];
                Index = Count - 1;
                FileStream.Seek(Index * 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                // Previous Node Data
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                CurrentNode = BitConverter.ToInt32(data, 4);
                // Next Node Data 
                NextNode = BitConverter.ToInt32(data, 8);

                OperationCount += 8;

                return CurrentNode;
            }

            public int Next()
            {
                if (NextNode == -1)
                {
                    OperationCount += 1;
                    return -1;
                }

                Byte[] data = new Byte[12];
                Index++;
                FileStream.Seek(Index * 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                // Previous Node Data
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                CurrentNode = BitConverter.ToInt32(data, 4);
                // Next Node Data
                NextNode = BitConverter.ToInt32(data, 8);

                OperationCount += 9;

                return CurrentNode;
            }

            private int Prev()
            {
                if (PrevNode == -1)
                {
                    OperationCount++;
                    return -1;
                }

                Byte[] data = new Byte[12];
                Index--;
                FileStream.Seek(Index * 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                // Previous Node Data
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                CurrentNode = BitConverter.ToInt32(data, 4);
                // Next Node Data
                NextNode = BitConverter.ToInt32(data, 8);

                OperationCount += 9;

                return CurrentNode;
            }

            public void Set(int newData)
            {
                Byte[] data = new Byte[4];
                BitConverter.GetBytes(newData).CopyTo(data, 0);
                FileStream.Seek((Index + 1) * 4, SeekOrigin.Begin);
                FileStream.Write(data, 0, 4);

                OperationCount += 4;
            }

            public void Generate(int n)
            {
                Count = 0;

                Random random = new Random(2019);

                if (File.Exists(FileName)) File.Delete(FileName);

                try
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(FileName, FileMode.Create)))
                    {
                        writer.Write(-1);
                        for (int i = 0; i < n; i++)
                        {
                            int test = random.Next(Int32.MaxValue);
                            writer.Write(test);
                            Count++;
                        }
                        writer.Write(-1);
                    }
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            public void RadixSort()
            {
                int m = GetMax();

                for (int exp = 1; m / exp > 0; exp *= 10)
                {
                    CountSort(exp);
                    OperationCount += 2;
                }

                OperationCount += 2;
            }

            private int GetMax()
            {
                int mx = Head();

                for (int i = 0; i < Count; i++)
                {
                    if (Next() > mx)
                    {
                        mx = CurrentNode;
                        OperationCount++;
                    }

                    OperationCount += 2;
                }

                OperationCount += 3;

                return mx;
            }

            private void CountSort(int exp)
            {
                int[] output = new int[Count];
                int[] count = new int[10];

                for (int i = 0; i < 10; i++)
                {
                    count[i] = 0;
                    OperationCount += 2;
                }

                for (int res = Head(); res != -1 ; res = Next())
                {
                    count[(res / exp) % 10]++;
                    OperationCount += 2;
                }

                for (int i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                    OperationCount += 2;
                }

                for (int res = Tail(); res != -1; res = Prev())
                {
                    output[count[(res / exp) % 10] - 1] = res;
                    count[(res / exp) % 10]--;
                    OperationCount += 3;
                }

                for (int res = Head(); res != -1; res = Next())
                {
                    Set(output[Index]);
                    OperationCount += 2;
                }

                OperationCount += 7;
            }

            public void Print()
            {
                Head();
                for (int i = 0; i < Count; i++)
                {
                    Console.WriteLine(CurrentNode);
                    Next();
                }
            }
        }

        public class Node
        {
            public int Data;
            public Node Next;
            public Node Prev;
            public int Index;

            public Node() { }

            public Node(int data, int index)
            {
                Data = data;
                Index = index;
                Next = null;
                Prev = null;
            }
        }
    }
}
