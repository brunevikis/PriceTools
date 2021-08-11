using Microsoft.VisualStudio.Tools.Applications;
using Microsoft.VisualStudio.Tools.Applications.Deployment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.DeployActions
{
    public class DeployActions : IAddInPostDeploymentAction
    {
        public void Execute(AddInPostDeploymentActionArgs args)
        {

            string sourcePath = args.AddInPath;
            Uri deploymentManifestUri = args.ManifestLocation;
            string destPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string destFile = System.IO.Path.Combine(destPath, "DecompToolsInstall.log");


            var txt = DateTime.Now.ToString() + "\r\n";
            txt += sourcePath + "\r\n";
            txt += args.ManifestLocation + "\r\n";
            txt += args.Version + "\r\n";
            txt += args.InstallationStatus + "\r\n";

            File.WriteAllText(destFile, txt);


            switch (args.InstallationStatus)
            {
                case AddInInstallationStatus.InitialInstall:
                case AddInInstallationStatus.Update:
                {
                    File.Copy(Path.Combine(sourcePath, "DecompTools.dll.config"), Path.Combine(sourcePath, "Compass.DecompToolsShellX.exe.config"));
                    SetRegistry(sourcePath);
                    break;
                }
                case AddInInstallationStatus.Uninstall:
                    RemoveRegistry();

                    break;

            }
        }


        static string anchorKey = @"SOFTWARE\Classes\*\shell\decompToolsShellX";
        static string ctxMenu = @"SOFTWARE\Classes\*\ContextMenus\decompToolsShellX";
        static string anchorKeyD = @"SOFTWARE\Classes\directory\shell\decompToolsShellX";
        static string ctxMenuD = @"SOFTWARE\Classes\directory\ContextMenus\decompToolsShellX";
        static string anchorKeyDb = @"SOFTWARE\Classes\directory\background\shell\decompToolsShellX";
        static string ctxMenuDb = @"SOFTWARE\Classes\directory\ContextMenus\background\decompToolsShellX";


        private void RemoveRegistry()
        {
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(anchorKey);
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ctxMenu);
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(anchorKeyD);
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ctxMenuD);
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(anchorKeyDb);
            Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(ctxMenuDb);
        }

        private void SetRegistry(string sourcePath)
        {

            var title = "Compass - Price Tools";

            var exe = Path.Combine(sourcePath, "Compass.DecompToolsShellX.exe");

            var comms = new Dictionary<string, string>() {
                {"Abrir em Excel", exe + " abrir " + "\"%1\""},
                //{"Rodar Vazoes", exe + " vazoes " + "\"%1\""},
                {"Rodar Vazoes 6", exe + " vazoes6 " + "\"%1\""},
                {"Rodar Previvaz", exe + " previvaz " + "\"%1\""},
                {"Ver EARM", exe + " earm " + "\"%1\""},
                {"Ver Resultado", exe + " resultado " + "\"%1\""},
                {"Alterar Cortes e TH...", exe + " corte " + "\"%1\""},
                {"Criar Dger.NWD", exe + " dgernwd " + "\"%1\""},
                {"Converter para CCEE", exe + " ons2ccee " + "\"%1\""},
                {"Tratar Inviabilidades", exe + " inviab " + "\"%1\""},
                {"Tendencia Hidrologica ...", exe + " tendhidr " + "\"%1\""},
                {"Converter deck Dessem ONS para CCEE", exe + " dessem2ccee " + "\"%1\""},
                {"Conversão Decodess", exe + " convdecodess " + "\"%1\""},
                {"PLD Dessem", exe + " plddessem " + "\"%1\""},

            };
            {
                var k = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(anchorKey);
                k.SetValue("ExtendedSubCommandsKey", ctxMenu.Replace(@"SOFTWARE\Classes\", ""), Microsoft.Win32.RegistryValueKind.String);
                k.SetValue("MUIVerb", title, Microsoft.Win32.RegistryValueKind.String);

                var k2 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ctxMenu);
                k2 = k2.CreateSubKey("shell");
                int i = 1;
                foreach (var comm in comms)
                {
                    var k2_1 = k2.CreateSubKey("cmd" + i.ToString());
                    k2_1.SetValue("MUIVerb", comm.Key, Microsoft.Win32.RegistryValueKind.String);
                    k2_1.CreateSubKey("command").SetValue("", comm.Value);
                    i++;
                }
            }


            var commsD = new Dictionary<string, string>() {
                {"Ver Resultado", exe + " resultado " + "\"%1\""},
                {"Alterar Cortes e TH...", exe + " corte " + "\"%1\""},
                {"Ver EARM", exe + " earm " + "\"%1\""},
                {"Duplicar Deck", exe + " duplicar " + "\"%1\""},
                //{"Rodar Vazoes", exe + " vazoes " + "\"%1\""},
                {"Rodar Vazoes 6", exe + " vazoes6 " + "\"%1\""},
                {"Rodar Previvaz", exe + " previvaz " + "\"%1\""},
                {"Criar Dger.NWD", exe + " dgernwd " + "\"%1\""},
                {"Converter para CCEE", exe + " ons2ccee " + "\"%1\""},
                {"Tratar Inviabilidades", exe + " inviab " + "\"%1\""},
                {"Tendencia Hidrologica ...", exe + " tendhidr " + "\"%1\""},
                {"Converter deck Dessem ONS para CCEE", exe + " dessem2ccee " + "\"%1\""},
                {"Conversão Decodess", exe + " convdecodess " + "\"%1\""},
                {"PLD Dessem", exe + " plddessem " + "\"%1\""},


            };


            {
                var k = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(anchorKeyD);
                k.SetValue("ExtendedSubCommandsKey", ctxMenuD.Replace(@"SOFTWARE\Classes\", ""), Microsoft.Win32.RegistryValueKind.String);
                k.SetValue("MUIVerb", title, Microsoft.Win32.RegistryValueKind.String);

                var k2 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ctxMenuD);
                k2 = k2.CreateSubKey("shell");


                int i = 1;
                foreach (var comm in commsD)
                {
                    var k2_1 = k2.CreateSubKey("cmd" + i.ToString());
                    k2_1.SetValue("MUIVerb", comm.Key, Microsoft.Win32.RegistryValueKind.String);
                    k2_1.CreateSubKey("command").SetValue("", comm.Value);
                    i++;
                }
            }

            var commsDb = new Dictionary<string, string>() {
                {"Alterar Todos Cortes e THs...", exe + " cortes " + "\"%V\""},
                {"Ver Todos Resultados", exe + " resultados " + "\"%V\""},
                {"Tendencia Hidrologica ...", exe + " tendhidr " + "null"},
                {"Atualizar UH Dessem", exe + " uhdessem " + "\"%V\""},


            };


            {
                var k = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(anchorKeyDb);
                k.SetValue("ExtendedSubCommandsKey", ctxMenuDb.Replace(@"SOFTWARE\Classes\", ""), Microsoft.Win32.RegistryValueKind.String);
                k.SetValue("MUIVerb", title, Microsoft.Win32.RegistryValueKind.String);

                var k2 = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(ctxMenuDb);
                k2 = k2.CreateSubKey("shell");

                int i = 1;
                foreach (var comm in commsDb)
                {
                    var k2_1 = k2.CreateSubKey("cmd" + i.ToString());
                    k2_1.SetValue("MUIVerb", comm.Key, Microsoft.Win32.RegistryValueKind.String);
                    k2_1.CreateSubKey("command").SetValue("", comm.Value);
                    i++;
                }
            }
        }
    }
}
