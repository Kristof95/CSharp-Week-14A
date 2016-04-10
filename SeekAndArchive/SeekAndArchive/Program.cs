using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace SeekAndArchive
{
    class Program
    {
        static List<FileInfo> FoundFiles;
        static List<FileSystemWatcher> watchers;
        static List<DirectoryInfo> archiveDirs;

        static void RecursiveSearch(List<FileInfo> foundFiles, string fileName, DirectoryInfo currentDirectory)
        {
            foreach (FileInfo file in currentDirectory.GetFiles())
            {
                if (file.Name == fileName)
                {
                    foundFiles.Add(file);
                }
            }
            foreach (DirectoryInfo dir in currentDirectory.GetDirectories())
            {
                RecursiveSearch(foundFiles, fileName, dir);
            }
        }

        static void WatcherChanged(object sender, FileSystemEventArgs e)
        {

            Console.WriteLine("{0} has been changed!", e.FullPath);

            FileSystemWatcher senderWatcher = (FileSystemWatcher)sender;
            int index = watchers.IndexOf(senderWatcher, 0);

            ArchiveFile(archiveDirs[index], FoundFiles[index]);
        }

        static void ArchiveFile(DirectoryInfo archiveDir, FileInfo fileToArchice)
        {
            FileStream input = fileToArchice.OpenRead();
            FileStream output = File.Create(archiveDir.FullName + @"\" + fileToArchice.Name + ".gz");

            Console.WriteLine(archiveDir.FullName + @"\" + fileToArchice.Name + ".gz");

            GZipStream Compressor = new GZipStream(output, CompressionMode.Compress);
            int b = input.ReadByte();

            while(b != -1)
            {
                Compressor.WriteByte((byte)b);
                b = input.ReadByte();
            }

            Compressor.Close();
            input.Close();
            output.Close();

        }

        static void Main(string[] args)
        {
            string fileName = args[0];
            string directoryName = args[1];
            FoundFiles = new List<FileInfo>();
            watchers = new List<FileSystemWatcher>();


            DirectoryInfo rootDir = new DirectoryInfo(directoryName);

            Console.WriteLine("Searching...");
            Console.WriteLine();

            RecursiveSearch(FoundFiles, fileName, rootDir);

            if (!rootDir.Exists)
            {
                Console.WriteLine("The specified directory does not exist.");
                return;
            }

            Console.WriteLine("Found files:{0}", FoundFiles.Count);

            foreach (FileInfo item in FoundFiles)
            {
                FileSystemWatcher newWatcher = new FileSystemWatcher(item.DirectoryName, item.Name);
                newWatcher.Changed += new FileSystemEventHandler(WatcherChanged);

                newWatcher.EnableRaisingEvents = true;
                watchers.Add(newWatcher);
            }

            archiveDirs = new List<DirectoryInfo>();
            for (int i = 0; i < FoundFiles.Count; i++)
            {
                archiveDirs.Add(Directory.CreateDirectory("Archive " + i.ToString()));
                Console.WriteLine("Archive_" + i.ToString());
            }
            Console.WriteLine("Archive dir(s):"+archiveDirs.Count);
            Console.ReadLine();
        }
    }
}