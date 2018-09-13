using System;

namespace FileMoverApp
{
    class Program
    {
        static void Main(string[] args)
        {
            FileMover fileMover = new FileMover();
            fileMover.Start();
        }
    }
}
