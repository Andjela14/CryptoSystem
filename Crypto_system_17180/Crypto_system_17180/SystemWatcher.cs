using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crypto_system_17180
{
    
    class SystemWatcher
    {
        private FileSystemWatcher watcher;
        private string watchedDir;
        private string destinationDir;
        private bool watcherOn;
        private int algorithm = 0;
        private bool pcbcOn = false;

        public SystemWatcher()
        {

            watcher = new FileSystemWatcher();
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Changed += OnChanged;
            watcher.Filter = "*.txt";
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = false;
            watcherOn = false;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(baseDir + @"\watchfolder"))
            {
                Directory.CreateDirectory(baseDir + @"\watchfold");
            }
            watchedDir = baseDir + @"\watchfold";
            watcher.Path = watchedDir;
            destinationDir = string.Empty;
            this.pcbcOn = false;
        }
  
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Console.WriteLine(value);

            if (destinationDir.Length != 0)
            {
                this.EncryptTextFile(e.FullPath, this.destinationDir);
            }
            else
            {
                Console.WriteLine("Seting destionation diretory failed!");
            }
               
        }
        public void SetAlgorithm(int algorithm)
        {
            this.algorithm = algorithm;
        }

        public void PCBC_ON_OFF(bool on)
        {
            this.pcbcOn = on;
        }
        public void SetDestinationDirectory(string dir)
        {
            if (!watcherOn)
            {
                if (watchedDir.CompareTo(dir) != 0)
                {
                    this.destinationDir = @""+dir;
                }
                    
            }
        }

        public bool SystemWatcherOn()
        {
            return this.watcherOn;
        }

        public string GetDestinationDirectory()
        {
            return this.destinationDir;
        }

        public void StartSystemWatcher()
        {
            if (this.destinationDir.Equals(string.Empty))
            {
                throw new Exception("Please set destination folder!");
            }
            if (!watcherOn)
            {
                watcherOn = true;
                watcher.EnableRaisingEvents = true;
            }
        }

        public void StopSystemWatcher()
        {
            if (watcherOn)
            {
                watcherOn = false;
                watcher.EnableRaisingEvents = false;
            }
        }

        public void EncryptFolderFiles(string folderPath)
        {
            if (this.destinationDir.Equals(string.Empty))
            {
                throw new Exception("Destination folder not set!");
            }
            List<string> fileEntries = new List<string>(Directory.GetFiles(folderPath, "*.txt"));
            if (fileEntries.Count != 0)
            {
                foreach (string txtFile in fileEntries)
                {
                    string k = txtFile;
                    this.EncryptTextFile(txtFile, this.destinationDir);
                }
            }
        }

        private bool EncryptTextFile(string fullFileName, string outputDirectory)
        {
            string outputFileName = outputDirectory + @"\" + Path.GetFileName(fullFileName); 
            string inputText = this.ReadTextFile(fullFileName);
            string outputText;
            bool encifer_procedure = false;
            if(this.algorithm == 0)
            {
                encifer_procedure = SimpleSubstitution.Encipher(inputText, outputFileName, out outputText);
            }
            else
            {
                encifer_procedure = Solitaire.Encipher(inputText, outputFileName, out outputText,pcbcOn);
            }
            if(encifer_procedure)
            {
                this.WriteToTextFile(outputFileName, outputText);
                return true;
            }
            return false;
        }

        public bool DectyptTextFile(string fullFileName, string targetFolder)
        {
            if (this.watcherOn)
            {
                return false;
            }
            string outputFileName = targetFolder + @"\" + Path.GetFileName(fullFileName);
            string inputText = this.ReadTextFile(fullFileName);
            string outputText;
            bool dencifer_procedure = false;
            if (this.algorithm == 0)
            {
                dencifer_procedure = SimpleSubstitution.Decipher(inputText, fullFileName, out outputText);
            }
            else
            {
                dencifer_procedure = Solitaire.Decipher(inputText, fullFileName, out outputText, pcbcOn);
            }
           
            if (dencifer_procedure)
            {
                this.WriteToTextFile(outputFileName, outputText);
                return true;
            }
            return false;

        }
    
        private string ReadTextFile(string path)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                return sr.ReadToEnd();
            }
        }

        private void WriteToTextFile(string path, string content)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(path, FileMode.Create);
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    sw.Write(content);
                }

            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Dispose();
                }
            }
        }
         
    }
}
