using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Compass.Services {
    public class Vazoes {
        static readonly string[] gevazpFiles = { "vaz.bat", "datvaz.exe", "gevazp.exe", "prevcen.exe", "gevazp.prm" };

        string workFolder = null;

        public string WorkFolder { get { return workFolder; } }

        string caso = null;

        public File[] Run(string deckFolder, bool saveFileToFolder = false) {

            workFolder = GetTemporaryDirectory();

            //copy gevazp
            CopyGevazpFiles(workFolder);

            //copy deck
            caso = CopyDeckFiles(workFolder, deckFolder);

            //run
            RunVaz();
            //collect results

            var files = GetResults();

            if (saveFileToFolder) {

                //var f = files.Where(x => x.Name.Equals("vazoes." + caso, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                //if (f != null) {
                foreach (var rf in files) {
                    System.IO.File.Copy(
                        rf.FullPath,
                        System.IO.Path.Combine(deckFolder, rf.Name),
                        true);
                }

                //}

            }
            return files;
        }

        public File[] RunDatVazOnly(string deckFolder, bool saveFileToFolder = false) {
            workFolder = GetTemporaryDirectory();

            //copy gevazp
            CopyGevazpFiles(workFolder);

            //copy deck
            caso = CopyDeckFiles(workFolder, deckFolder);

            //run
            RunDatVaz();
            //collect results

            var files = GetResults();

            if (saveFileToFolder) {

                //var f = files.Where(x => x.Name.Equals("vazoes." + caso, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                //if (f != null) {
                foreach (var rf in files) {
                    System.IO.File.Copy(
                        rf.FullPath,
                        System.IO.Path.Combine(deckFolder, rf.Name),
                        true);
                }

                //}

            }
            return files;
        }

        public File[] GetResults() {
            string[] files =
                {   Path.Combine(workFolder, "vazoes." + caso),
                    Path.Combine(workFolder, "PREVCEN.DAT")
                };

            var err = Directory.GetFiles(workFolder, "*.err");
            var rel = Directory.GetFiles(workFolder, "*.rel");

            var resultList =
                    err.Select(f =>
                    new File { Content = System.IO.File.ReadAllBytes(f), Name = Path.GetFileName(f), FullPath = f }
                    )
                .Concat(
                    rel.Select(f =>
                    new File { Content = System.IO.File.ReadAllBytes(f), Name = Path.GetFileName(f), FullPath = f }
                    )
                ).ToList();

            foreach (var file in files) if (System.IO.File.Exists(file)) {

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

        public void RunVaz() {

            System.Diagnostics.Process pr = new System.Diagnostics.Process();

            var si = pr.StartInfo;

            si.FileName = Path.Combine(Location, "Compass.VazoesApp.exe");
            si.Arguments = "\"" + workFolder + "\" " + caso;

            pr.Start();
            pr.WaitForExit();
        }

        private void RunDatVaz() {

            System.Diagnostics.Process pr = new System.Diagnostics.Process();

            var si = pr.StartInfo;

            si.FileName = Path.Combine(Location, "Compass.VazoesApp.exe");
            si.Arguments = "\"" + workFolder + "\" " + caso + " " + "-datvaz";

            pr.Start();
            pr.WaitForExit();
        }

        private static string CopyDeckFiles(string workFolder, string deckFolder) {

            var deck = new Compass.CommomLibrary.Decomp.Deck();
            deck.GetFiles(deckFolder);
            deck.CopyFilesToFolder(workFolder);

            return deck.Caso;
        }

        private static void CopyGevazpFiles(string workFolder) {

            var gevazpProgramFolder = System.Configuration.ConfigurationManager.AppSettings["gevazpPath"];
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

        public string Location {
            get {
                return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            }
        }
    }
}
