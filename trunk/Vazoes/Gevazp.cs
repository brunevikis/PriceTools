using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Compass.Vazoes {
    public class Gevazp {
        static readonly string gevazpProgramFolder = @"H:\TI - Sistemas\GEVAZP\Versão 3.10";
        static readonly string[] gevazpFiles = { "vaz.bat", "datvaz.exe", "gevazp.exe", "prevcen.exe", "gevazp.prm" };
        static readonly string[] deckFiles = { "dadger", "hidr", "modif", "postos", "rv", "prevs", "vazoesc" };

        string workFolder = null;

        public string WorkFolder { get { return workFolder; } }

        string caso = null;

        public File[] Run(string deckFolder) {

            workFolder = GetTemporaryDirectory();

            //copy gevazp
            CopyGevazpFiles(workFolder);

            //copy deck
            caso = CopyDeckFiles(workFolder, deckFolder);

            //run
            RunVaz(workFolder, caso);
            //collect results

            var files = GetResults();

            return files;
        }

        private File[] GetResults() {
            var file = Path.Combine(workFolder, "vazoes." + caso);


            var err = Directory.GetFiles(workFolder, "*.err");

            var resultList = err.Select(f =>
                new File { Content = System.IO.File.ReadAllBytes(f), Name = Path.GetFileName(f), FullPath = f }
                ).ToList();

            if (System.IO.File.Exists(file)) {
                resultList.Add(
                          new File { Content = System.IO.File.ReadAllBytes(file), Name = Path.GetFileName(file), FullPath = file }
                          );
            }

            return resultList.ToArray();
        }

        public void ClearTempFolder() {
            ClearTempFolder(workFolder);
        }

        private void ClearTempFolder(string workFolder) {
            if (workFolder != null) {
                if (Directory.Exists(workFolder)) {

                    var files = Directory.GetFiles(workFolder);
                    foreach (var file in files) {
                        System.IO.File.Delete(file);
                    }
                    Directory.Delete(workFolder, true);
                }
            }
        }

        private void RunVaz(string workFolder, string caso) {

            System.Diagnostics.Process pr = new System.Diagnostics.Process();

            var si = pr.StartInfo;

            si.FileName = Path.Combine(gevazpProgramFolder, "VazoesApp.exe");
            si.Arguments = "\"" + workFolder + "\" " + caso;

            pr.Start();
            pr.WaitForExit();
        }

        private void RunDatvaz() {
            Process pr = new Process();

            var si = pr.StartInfo;
            si.FileName = Path.Combine(workFolder, "Datvaz.EXE");
            si.RedirectStandardInput = true;
            si.UseShellExecute = false;
            si.WorkingDirectory = workFolder;

            pr.StartInfo = si;

            pr.Start();

            Thread.Sleep(500);

            var sw = pr.StandardInput;
            sw.WriteLine(caso);
            sw.Close();

            pr.WaitForExit();
        }
        private void RunGevazp() {
            Process pr = new Process();

            var si = pr.StartInfo;
            si.FileName = Path.Combine(workFolder, "Gevazp.EXE");

            si.WorkingDirectory = workFolder;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();
        }
        private void RunPrevcen() {
            Process pr = new Process();

            var si = pr.StartInfo;
            si.FileName = Path.Combine(workFolder, "Prevcen.EXE");

            si.WorkingDirectory = workFolder;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();
        }


        private static string CopyDeckFiles(string workFolder, string deckFolder) {

            var deck = new Compass.CommomLibrary.Decomp.Deck();
            deck.GetFiles(deckFolder);
            deck.SaveFilesToFolder(workFolder);

            return deck.Caso;
        }

        private static void CopyGevazpFiles(string workFolder) {

            foreach (var file in gevazpFiles) {
                System.IO.File.Copy(
                    Path.Combine(gevazpProgramFolder, file),
                    Path.Combine(workFolder, file)
                    );
            }
        }

        private static string GetTemporaryDirectory() {
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        public class File {
            public string Name { get; set; }
            public string FullPath { get; set; }
            public byte[] Content { get; set; }
        }

        public void OpenTempFolder() {
            if (workFolder != null && Directory.Exists(workFolder)) {
                Process.Start(workFolder);    
            }
            
        }
    }
}
