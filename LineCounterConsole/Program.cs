﻿using System;
using System.IO;

namespace LineCounterConsole
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Line Counter");
            Console.WriteLine("Program assumes valid formatting of code (all code isn't on one line, etc.)");
            String path = GetPath();
            LineCounter counter = new LineCounter(path);
            Console.WriteLine("Lines of code in the file: "+counter.CountLines());
            Console.WriteLine("Lines of significant code in the file: "+counter.CountSignificantLines());
            Console.WriteLine("press enter to exit...");
            Console.ReadLine();
        }

        private static String GetPath()
        {
            bool validPath = false;
            String path = "";
            while (!validPath)
            {
                Console.WriteLine("Please enter the file location as either a full " +
                                  "system path or a local path");
                Console.Write("Path: ");
                path = Console.ReadLine();
                if (!File.Exists(path))
                {
                    Console.WriteLine("Invalid file path. Please enter again");
                }
                else
                {
                    validPath = true;
                }
            }
            return path;
        }
    }
}
