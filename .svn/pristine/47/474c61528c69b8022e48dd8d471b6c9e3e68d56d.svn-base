using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.Services {
    public class Previvaz {
        static readonly string[] previvazFiles = { /*"previvaz.exe", "previvaz.dll",*/ "ENCAD.DAT" };

        string previvazExe = "";
        string workFolder = null;

        public string WorkFolder { get { return workFolder; } }

        public File[] Run(string inpFolder/*, bool saveFileToFolder = false*/) {

            workFolder = inpFolder;

            //copy gevazp
            CopyPrevivazFiles(workFolder);

            //copy deck
            //caso = CopyDeckFiles(workFolder, deckFolder);

            //run
            RunPrevivaz();
            //collect results

            var files = GetResults();

            //if (saveFileToFolder) {

            //    //var f = files.Where(x => x.Name.Equals("vazoes." + caso, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            //    //if (f != null) {
            //    foreach (var rf in files) {
            //        System.IO.File.Copy(
            //            rf.FullPath,
            //            System.IO.Path.Combine(deckFolder, rf.Name),
            //            true);
            //    }

            //    //}

            //}
            return files;
        }

        public File[] GetResults() {
            //string[] files =
            //    {   Path.Combine(workFolder, "vazoes." + caso),
            //        Path.Combine(workFolder, "PREVCEN.DAT")
            //    };

            //var err = Directory.GetFiles(workFolder, "*.err");
            //var rel = Directory.GetFiles(workFolder, "*.rel");

            //var resultList =
            //        err.Select(f =>
            //        new File { Content = System.IO.File.ReadAllBytes(f), Name = Path.GetFileName(f), FullPath = f }
            //        )
            //    .Concat(
            //        rel.Select(f =>
            //        new File { Content = System.IO.File.ReadAllBytes(f), Name = Path.GetFileName(f), FullPath = f }
            //        )
            //    ).ToList();

            //foreach (var file in files) if (System.IO.File.Exists(file)) {

            //        resultList.Add(
            //                  new File { Content = System.IO.File.ReadAllBytes(file), Name = Path.GetFileName(file), FullPath = file }
            //                  );
            //    }

            //return resultList.ToArray();

            return null;
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

        public void RunPrevivaz() {

            System.Diagnostics.Process pr = new System.Diagnostics.Process();

            var si = pr.StartInfo;

            si.FileName = previvazExe;

            si.WorkingDirectory = workFolder;

            si.CreateNoWindow = true;
            si.UseShellExecute = false;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();
        }

        //private static string CopyDeckFiles(string workFolder, string deckFolder) {

        //    var deck = new Compass.CommomLibrary.Decomp.Deck();
        //    deck.GetFiles(deckFolder);
        //    deck.CopyFilesToFolder(workFolder);

        //    return deck.Caso;
        //}

        private void CopyPrevivazFiles(string workFolder) {

            var previvazProgramFolder = System.Configuration.ConfigurationManager.AppSettings["previvazPath"];
            foreach (var file in previvazFiles) {
                System.IO.File.Copy(
                    Path.Combine(previvazProgramFolder, file),
                    Path.Combine(workFolder, file)
                    );
            }
            previvazExe = Path.Combine(previvazProgramFolder, "previvaz.exe");
        }

        //private static string GetTemporaryDirectory() {
        //    string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        //    Directory.CreateDirectory(tempDirectory);
        //    return tempDirectory;
        //}

        public class File {
            public string Name { get; set; }
            public string FullPath { get; set; }
            public byte[] Content { get; set; }
        }

        //public void OpenTempFolder() {
        //    if (workFolder != null && Directory.Exists(workFolder)) {
        //        Process.Start(workFolder);
        //    }

        //}

        public string Location {
            get {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            }
        }
    }
}
