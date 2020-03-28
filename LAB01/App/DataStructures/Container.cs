using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB01.App.Data_Structures
{
    class Container
    {
        public Memory MemoryEnv { private set; get; }
        public Disk DiskEnv { private set; get; }

        public Container(string fileName)
        {
            MemoryEnv = new Memory();
            DiskEnv = new Disk(fileName);
        }

        public class Memory
        {
            public int Count { get { return Array.Length; } }
            public int[] Array { private set; get; }
            public int this[int i] { 
                get {
                    return Array[i];
                } 
                set
                {
                    Array[i] = value;
                }
            }
            public int OperationCount { private set; get; }

            public Memory() { }

            public void ResetOperationCount()
            {
                OperationCount = 0;
            }

            public void Generate(int n)
            {
                Array = new int[n];

                Random random = new Random(2019);
                for (int i = 0; i < n; i++)
                {
                    Array[i] = random.Next(Int32.MaxValue);
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
                int mx = Array[0];

                for (int i = 1; i < Count; i++)
                {
                    if (Array[i] > mx)
                    {
                        mx = Array[i];
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

                for (int i = 0; i < Count; i++)
                {
                    count[(Array[i] / exp) % 10]++;
                    OperationCount += 2;
                }

                for (int i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                    OperationCount += 2;
                }

                for (int i = Count - 1; i >= 0; i--)
                {
                    output[count[(Array[i] / exp) % 10] - 1] = Array[i];
                    count[(Array[i] / exp) % 10]--;
                    OperationCount += 3;
                }

                for (int i = 0; i < Count; i++)
                {
                    Array[i] = output[i];
                    OperationCount += 2;
                }

                OperationCount += 7;
            }

            public void Print()
            {
                for (int i = 0; i < Count; i++)
                {
                    Console.WriteLine($"{Array[i]} ");
                }
            }
        }

        public class Disk
        {
            public string FileName { private set; get; }
            public FileStream FileStream { set; get; }
            public int Count { private set; get; }
            public int this[int index]
            {
                get
                {
                    Byte[] data = new Byte[4];
                    FileStream.Seek(4 * index, SeekOrigin.Begin);
                    FileStream.Read(data, 0, 4);
                    int result = BitConverter.ToInt32(data, 0);
                    OperationCount += 5;
                    return result;
                }
                set
                {
                    Byte[] data = new Byte[4]; 
                    BitConverter.GetBytes(value).CopyTo(data, 0);
                    FileStream.Seek(4 * index, SeekOrigin.Begin);
                    FileStream.Write(data, 0, 4);
                    OperationCount += 4;
                }
            }
            public int OperationCount { private set; get; }

            public Disk(string fileName)
            {
                FileName = fileName;
            }

            public void ResetOperationCount()
            {
                OperationCount = 0;
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
                        for (int i = 0; i < n; i++)
                        {
                            writer.Write(random.Next(Int32.MaxValue));
                            Count++;
                        }
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
                int mx = this[0]; 

                for (int i = 1; i < Count; i++)
                {
                    if (this[i] > mx)
                    {
                        mx = this[i];
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

                for (int i = 0; i < Count; i++)
                {
                    count[(this[i] / exp) % 10]++;
                    OperationCount += 2;
                }

                for (int i = 1; i < 10; i++)
                {
                    count[i] += count[i - 1];
                    OperationCount += 2;
                }

                for (int i = Count - 1; i >= 0; i--)
                {
                    output[count[(this[i] / exp) % 10] - 1] = this[i];
                    count[(this[i] / exp) % 10]--;
                    OperationCount += 3;
                }

                for (int i = 0; i < Count; i++)
                {
                    this[i] = output[i];
                    OperationCount += 2;
                }

                OperationCount += 7;
            }

            public void Print()
            {
                for (int i = 0; i < Count; i++)
                {
                    Console.WriteLine($"{this[i]} ");
                }
            }
        }
    }
}
