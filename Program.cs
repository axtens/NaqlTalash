using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NaqlTalash
{
    class Program
    {
        static void Main(string[] args)
        {
            // masterDir copyDir
            // output HTML and CMD
            if (args.Length == 2)
            {
                //File.WriteAllText("\\tmp\\NaqlTalash.txt", "");
                var sourceList = from file in Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories)
                                 let finfo = new System.IO.FileInfo(file)
                                 orderby -finfo.Length
                                 where finfo.Length > 0
                                 select finfo;

                var destList = from file in Directory.GetFiles(args[1], "*.*", SearchOption.AllDirectories)
                               let finfo = new System.IO.FileInfo(file)
                               orderby -finfo.Length
                               where finfo.Length > 0
                               select finfo;

                foreach (var finfo in sourceList)
                {
                    var matchFound = false;
                    var matching = from item in destList where item.Length == finfo.Length select item;
                    foreach (var match in matching)
                    {
                        if (FileEquals(match.FullName, finfo.FullName))
                        {
                            if (!matchFound)
                            {
                                Console.WriteLine("REM " + finfo.FullName);
                                Console.WriteLine("REM " + finfo.Length);
                            }
                            matchFound = true;
                            Console.WriteLine("\tDEL \"" + match.FullName + "\"");
                        }
                    }
                }
            }
        }

        static bool FileEquals(string path1, string path2)
        {
            var file1 = File.ReadAllBytes(path1);
            var file2 = File.ReadAllBytes(path2);
            if (file1.Length == file2.Length)
            {
                for (int i = 0; i < file1.Length; i++)
                {
                    if (file1[i] != file2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}
