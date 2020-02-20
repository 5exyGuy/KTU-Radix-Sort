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
                }
            }

            private int GetMax()
            {
                int mx = Head.Data;

                for (Node node = Head; node != null; node = node.Next)
                {
                    if (node.Data > mx)
                    {
                        mx = node.Data;
                    }
                }

                return mx;
            }

            private void CountSort(int exp)
            {
                int[] output = new int[Count];
                int i;
                int[] count = new int[10];

                for (i = 0; i < 10; i++)
                {
                    count[i] = 0;
                }

                for (Node node = Head; node != null; node = node.Next)
                {
                    count[(node.Data / exp) % 10]++;
                }

                for (i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                }

                for (Node node = Tail; node != null; node = node.Prev)
                {
                    output[count[(node.Data / exp) % 10] - 1] = node.Data;
                    count[(node.Data / exp) % 10]--;
                }

                for (Node node = Head; node != null; node = node.Next)
                {
                    node.Data = output[node.Index];
                }
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

            public Disk(string fileName)
            {
                FileName = fileName;
            }

            public int Head()
            {
                Byte[] data = new Byte[12];
                Index = 0;
                // Previous Node Data
                FileStream.Seek(Index, SeekOrigin.Begin);
                FileStream.Read(data, 0, 4);
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                FileStream.Seek(Index + 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 8);
                CurrentNode = BitConverter.ToInt32(data, 0);
                // Next Node Data
                FileStream.Seek(Index + 8, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                NextNode = BitConverter.ToInt32(data, 0);

                return CurrentNode;
            }

            private int Tail()
            {
                Byte[] data = new Byte[12];
                Index = (Count - 1);
                // Previous Node Data
                FileStream.Seek(Index * 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 4);
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                FileStream.Seek(Index * 4 + 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 8);
                CurrentNode = BitConverter.ToInt32(data, 0);
                // Next Node Data 
                FileStream.Seek(Index * 4 + 8, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                NextNode = BitConverter.ToInt32(data, 0);

                return CurrentNode;
            }

            public int Next()
            {
                if (NextNode == -1)
                {
                    return -1;
                }

                Byte[] data = new Byte[12];
                Index++;
                // Previous Node Data
                FileStream.Seek(Index * 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 4);
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                FileStream.Seek(Index * 4 + 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 8);
                CurrentNode = BitConverter.ToInt32(data, 0);
                // Next Node Data
                FileStream.Seek(Index * 4 + 8, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                NextNode = BitConverter.ToInt32(data, 0);

                return CurrentNode;
            }

            private int Prev()
            {
                if (PrevNode == -1)
                {
                    return -1;
                }

                Byte[] data = new Byte[12];
                Index--;
                // Previous Node Data
                FileStream.Seek(Index * 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 4);
                PrevNode = BitConverter.ToInt32(data, 0);
                // Current Node Data
                FileStream.Seek(Index * 4 + 4, SeekOrigin.Begin);
                FileStream.Read(data, 0, 8);
                CurrentNode = BitConverter.ToInt32(data, 0);
                // Next Node Data
                FileStream.Seek(Index * 4 + 8, SeekOrigin.Begin);
                FileStream.Read(data, 0, 12);
                NextNode = BitConverter.ToInt32(data, 0);

                return CurrentNode;
            }

            public void Set(int newData)
            {
                Byte[] data = new Byte[4];
                BitConverter.GetBytes(newData).CopyTo(data, 0);
                FileStream.Seek((Index + 1) * 4, SeekOrigin.Begin);
                FileStream.Write(data, 0, 4);
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
                            int test = random.Next(10);
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
                }
            }

            private int GetMax()
            {
                int mx = Head();

                for (int i = 0; i < Count; i++)
                {
                    if (Next() > mx)
                    {
                        mx = CurrentNode;
                    }
                }

                return mx;
            }

            private void CountSort(int exp)
            {
                int[] output = new int[Count];
                int i;
                int[] count = new int[10];

                for (i = 0; i < 10; i++)
                {
                    count[i] = 0;
                }

                for (int res = Head(); res != -1 ; res = Next())
                {
                    count[(res / exp) % 10]++;
                }

                for (i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                }

                for (int res = Tail(); res != -1; res = Prev())
                {
                    output[count[(res / exp) % 10] - 1] = res;
                    count[(res / exp) % 10]--;
                }

                for (int res = Head(); res != -1; res = Next())
                {
                    Set(output[Index]);
                }
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
