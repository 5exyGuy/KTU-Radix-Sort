using LAB01.App;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace LAB01
{
    public class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr handle, out int mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int handle);

        static void Main(string[] args)
        {
            SetupConsoleColors();

            Main application = new Main("Remeika Aurėjus IFZ-8/2", 1);
            application.startApplication();
        }

        static void SetupConsoleColors()
        {
            var handle = GetStdHandle(-11);
            int mode;
            GetConsoleMode(handle, out mode);
            SetConsoleMode(handle, mode | 0x4);

            Console.OutputEncoding = Encoding.UTF8;
        }
    }
}
