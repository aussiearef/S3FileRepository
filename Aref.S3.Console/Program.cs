/*
 * 
 *      S3 FILE REPOSITORY DEMONSTRATION PROGRAM
 *      WRITTEN BY AREF KARIMI OCTOBER 2014
 *      WWW.ASPGUY.WORDPRESS.COM
 *      
 */

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aref.S3.Lib.Interfaces;
using Aref.S3.Lib.Strategies;

namespace Aref.S3.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IFileRepository repository = new FileRepository();
                //In a real app try to get an instance of IFileRepository through IOC

            System.Console.Clear();
            System.Console.WriteLine("www.AspGuy.WordPress.com\n\r");


            System.Console.WriteLine("Root folders :");
            IEnumerable<string> rootFolders = repository.GetSubdirNames().ToList();
            Parallel.ForEach(rootFolders, f => System.Console.WriteLine("{0}", f));


            System.Console.WriteLine("Sub-folders along with their files and sub-folders:");

            // S3FileRepository class is not guarranteed to be thread safe so I use it in a normal foreach
            foreach (string rootFolder in rootFolders)
            {
                repository.ChangeDir(string.Format("/{0}", rootFolder));
                IEnumerable<string> files = repository.GetFileNames(string.Empty);
                Parallel.ForEach(files, f => System.Console.WriteLine("File = {0}", f));
                IEnumerable<string> subDirs = repository.GetSubdirNames();
                Parallel.ForEach(subDirs, s => System.Console.WriteLine("Folder = {0}/{1}", rootFolder, s));
            }

            System.Console.WriteLine();
            System.Console.ReadKey();
        }
    }
}