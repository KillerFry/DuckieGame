using System;
using System.IO;
using System.Runtime.InteropServices;

namespace DuckieGame
{
    class Program
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

        static void Main(string[] args)
        {

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetDllDirectory(Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory,
                    Environment.Is64BitProcess ? "x64" : "x86"
                ));
            }

            using (TheGame theGame = new TheGame())
            {
                theGame.Run();
            }
        }
    }
}
