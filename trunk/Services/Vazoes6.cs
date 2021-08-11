using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Compass.Services {
    public class Vazoes6 {
        static readonly string[] gevazpFiles = { "gevazp.exe", "gevazp.lic", "MODIF.DAT", "REGRAS.DAT", "ARQUIVOS.DAT", "GEVAZP.DAT" };

        string workFolder = null;

        public string WorkFolder { get { return workFolder; } }

        string caso = null;

        public File[] Run(string deckFolder, bool saveFileToFolder = false) {

            workFolder = GetTemporaryDirectory();

            //copy gevazp
            CopyGevazpFiles(workFolder);

            //copy deck
            caso = CopyDeckFiles(workFolder, deckFolder);


            var arqArr = System.IO.File.ReadAllLines(Path.Combine(workFolder, "ARQUIVOS.DAT"));
            arqArr[9] = "LISTA ARQUIVOS DECOMP       : " + caso;
            System.IO.File.Delete(Path.Combine(workFolder, "ARQUIVOS.DAT"));
            System.IO.File.WriteAllLines(Path.Combine(workFolder, "ARQUIVOS.DAT"), arqArr);

            System.IO.File.WriteAllText(
                Path.Combine(workFolder, "caso.dat")
                , "arquivos.dat");
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

        public File[] GetResults() {
            string[] files =
                {   Path.Combine(workFolder, "vazoes." + caso),
                    Path.Combine(workFolder, "PREVCEN.DAT"),
                    Path.Combine(workFolder, "GEVAZP.OUT"),
                    Path.Combine(workFolder, "VAZTOTGD.DAT")
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

            si.FileName = Path.Combine(workFolder, "gevazp.exe");
            si.WorkingDirectory = workFolder;

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

            var gevazpProgramFolder = System.Configuration.ConfigurationManager.AppSettings["gevazp6Path"];
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




        public static void IncorporarVazpast(CommomLibrary.VazoesC.VazoesC vazoes, CommomLibrary.Vazpast.Vazpast vazpast, DateTime dataReferenciaVazpast) {


            for (DateTime dt = dataReferenciaVazpast.AddMonths(-11); dt < dataReferenciaVazpast; dt = dt.AddMonths(1)) {

                var q = from vq in vazoes.Conteudo
                        let dataq = new DateTime(vq.Ano, vq.Mes, 1)
                        where dataq == dt
                        select vq;
                if (q.Count() == 0) vazoes.AdicionarAnos(1);
                q.ToList().ForEach(vc => {
                    var vaz = vazpast.Conteudo.FirstOrDefault(x => x.Posto == vc.Posto);
                    if (vaz != null) vc.Vazao = (int)vaz[dt];
                }
               );

            }
        }
    }
}
