using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Compass.VazoesApp {
    class Program {

        static bool error = false;
        static void Main(string[] args) {
            //arg 1 - folder
            //arg 2 - rv
            //arg 3 - -datvaz
            error = false;
            var workFolder = args[0];
            var caso = args[1];
            RunDatvaz(workFolder, caso);

            if (args.Length > 2 && args[2].Equals("-datvaz", System.StringComparison.OrdinalIgnoreCase))
                return;

            RunGevazp(workFolder);
            RunPrevcen(workFolder);


            if (error) {


                System.Console.WriteLine();
                System.Console.WriteLine("Verifique a ocorrencia de erros");
                System.Console.WriteLine("Pressione qualquer tecla para finalizar");
                System.Console.ReadKey();
            }
        }

        private static void RunDatvaz(string workFolder, string caso) {
            Process pr = new Process();

            var si = pr.StartInfo;
            si.FileName = Path.Combine(workFolder, "Datvaz.EXE");
            si.RedirectStandardInput = true;
            si.UseShellExecute = false;
            si.WorkingDirectory = workFolder;
            si.RedirectStandardOutput = true;


            pr.StartInfo = si;

            pr.Start();

            Thread.Sleep(500);

            var sw = pr.StandardInput;
            sw.WriteLine(caso);
            Thread.Sleep(50);
            if (!pr.HasExited) {
                sw.WriteLine("S");
            }
            sw.Close();

            pr.WaitForExit();

            var outString = pr.StandardOutput.ReadToEnd();
            System.Console.WriteLine();
            System.Console.WriteLine(outString);
            if (outString.Contains("STATUS=ERR")) error = true;

        }
        private static void RunGevazp(string workFolder) {
            Process pr = new Process();

            var si = pr.StartInfo;
            si.UseShellExecute = false;
            si.RedirectStandardOutput = true;
            si.FileName = Path.Combine(workFolder, "Gevazp.EXE");


            si.WorkingDirectory = workFolder;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();

            var outString = pr.StandardOutput.ReadToEnd();
            System.Console.WriteLine();
            System.Console.WriteLine(outString);
            if (outString.Contains("STATUS=ERR")) error = true;
        }
        private static void RunPrevcen(string workFolder) {
            Process pr = new Process();

            var si = pr.StartInfo;
            //si.UseShellExecute = false;
            //si.RedirectStandardOutput = true;
            si.FileName = Path.Combine(workFolder, "Prevcen.EXE");

            si.WorkingDirectory = workFolder;

            si.CreateNoWindow = true;

            pr.StartInfo = si;

            pr.Start();

            pr.WaitForExit();

            //var outString = pr.StandardOutput.ReadToEnd();
            //System.Console.WriteLine();
            //System.Console.WriteLine(outString);
            //if (outString.Contains("STATUS=ERR")) error = true;
        }
    }
}
