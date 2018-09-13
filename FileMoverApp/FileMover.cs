using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileMoverApp
{
    public class FileMover
    {
        DateTime _startTime;
        long _totalSize = 0;
        List<string> _files = new List<string>();
        private string target, source;

        public void Start()
        {
            Console.Write("Source path: (D:\\a\\folder\\): ");
            source = Console.ReadLine();
            Console.Write("Target path: (D:\\a\\folder\\): ");
            target = Console.ReadLine();
            Console.Write("File types to move (jpg,png,mpeg): ");
            var extensions = Console.ReadLine().Split(',');

            _files = GetAllFiles(extensions);
            _totalSize = CalculateSize(_files);

            Console.Clear();
            Console.Write($"Found {_files.Count} matching files, with total size of {_totalSize / 1000000}MB. \n Move files?(Y/N): ");
            var input = Console.ReadLine().ToLower();
            while ((!input.Equals("y") && !input.Equals("n")))
            {
                Console.Clear();
                Console.WriteLine("Invalid input. Move files? (Y/N): ");
                input = Console.ReadLine();
            }
            if (input.ToLower().Equals("y"))
            {
                _startTime = DateTime.Now;
                MoveFiles();
                DateTime endTime = DateTime.Now;
                TimeSpan time = endTime.Subtract(_startTime);

                Console.WriteLine($"Moved {_files.Count} files in {time.Hours} hours, {time.Minutes}, {time.Seconds} seconds.");
            }
            Console.ReadLine();
        }

        private long CalculateSize(List<string> files)
        {
            long size = 0;
            _files.ToList().ForEach(x => { size += new System.IO.FileInfo(x).Length; });
            return size;
        }

        private List<string> GetAllFiles(string[] extensions)
        {
            string[] tempFiles = Directory.GetFiles(source, "", SearchOption.AllDirectories);
            List<string> files = new List<string>();
            tempFiles.ToList().ForEach(x =>
            {
                extensions.ToList().ForEach(y =>
                {
                    if (x.EndsWith(y)) files.Add(x);
                });
            });
            return files;
        }

        private void MoveFiles()
        {
            _files.ToList().ForEach(x =>
            {
                var file = x.Split(@"\").Last();
                var extension = file.Split(".").Last();
                var filename = file.Remove(file.Length - (extension.Length + 1));
                int append = 0;

                while (System.IO.File.Exists(target + "\\" + filename + "." + extension))
                {
                    Console.Write($"File {filename} already exists! ");

                    append += 1;
                    var appendAS = append.ToString();
                    if (append != 1) filename.Remove(filename.Length - 1);
                    filename += $"_{appendAS}";

                    Console.WriteLine($"Renaming to {filename}");

                }
                System.IO.File.Copy(x, target + "\\" + filename + "." + extension);
            });
        }
    }
}