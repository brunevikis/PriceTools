using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Previvaz
{

    public enum DeckDocument
    {
        caso,
        inp,
        str,
        lim
    }

    public class Deck : BaseDeck
    {

        public string Folder { get; set; }
        public string Posto { get; set; }

        public Deck(string posto = null)
        {
            this.Posto = posto;
        }


        Dictionary<string, DeckFile> documents = new Dictionary<string, DeckFile> {
            {"CASO.DAT"   , null},
            {"STR"  , null},
            {"INP"  , null},
            {"LIM"   , null}
        };

        public override Dictionary<string, DeckFile> Documents { get { return documents; } }

        public DeckFile this[DeckDocument doc]
        {
            get
            {
                switch (doc)
                {
                    case DeckDocument.caso:
                        return Documents["CASO.DAT"];
                    case DeckDocument.inp:
                        return Documents["INP"];
                    case DeckDocument.str:
                        return Documents["STR"];
                    case DeckDocument.lim:
                        return Documents["LIM"];
                    default:
                        return null;
                }
            }
            set
            {
                switch (doc)
                {
                    case DeckDocument.caso:
                        Documents["CASO.DAT"] = value; break;
                    case DeckDocument.inp:
                        Documents["INP"] = value; break;
                    case DeckDocument.str:
                        Documents["STR"] = value; break;
                    case DeckDocument.lim:
                        Documents["LIM"] = value; break;
                }
            }
        }

        public override void GetFiles(string baseFolder)
        {
            var folderFiles = System.IO.Directory.GetFiles(baseFolder)
                .Where(x => !x.EndsWith(".bak", StringComparison.OrdinalIgnoreCase));

            var q = from file in folderFiles
                    select new { Key = System.IO.Path.GetFileName(file), file };

            if (q.Any(x => x.Key == "CASO.DAT"))
            {
                var f = q.Where(x => x.Key == "CASO.DAT").First().file;
                this[DeckDocument.caso] = new DeckFile(f);
            }


            if (this[DeckDocument.caso] == null)
            {
                var inp = q.First(x =>
                    string.IsNullOrWhiteSpace(this.Posto) ?
                    x.Key.EndsWith(".inp", StringComparison.OrdinalIgnoreCase) :
                    x.Key.Equals(this.Posto + ".inp", StringComparison.OrdinalIgnoreCase));
                this[DeckDocument.caso] = new DeckFile(System.IO.Path.Combine(baseFolder, "CASO.DAT"));
                this[DeckDocument.caso].Document = new CasoDat() { Caso = inp.Key };

                this[DeckDocument.inp] = new DeckFile(inp.file);
            }
            else
            {

                this[DeckDocument.inp] = new DeckFile(System.IO.Path.Combine(baseFolder, ((CasoDat)this[DeckDocument.caso].Document).Caso));
            }

            var inpDoc = new Inp(this[DeckDocument.inp].BasePath);
            this[DeckDocument.inp].Document = inpDoc;

            var str = inpDoc.StrFile;

            if (System.IO.File.Exists(System.IO.Path.Combine(baseFolder, str)))
            {
                this[DeckDocument.str] = new DeckFile(System.IO.Path.Combine(baseFolder, str));
                var strDoc = new Str(this[DeckDocument.str].BasePath, inpDoc.NumSemanasHist);
                this[DeckDocument.str].Document = strDoc;
            }
            var lim = inpDoc.LimFile;
            if (System.IO.File.Exists(System.IO.Path.Combine(baseFolder, lim)))
            {
                this[DeckDocument.lim] = new DeckFile(System.IO.Path.Combine(baseFolder, lim));
            }
        }

        public override void CopyFilesToFolder(string folder)
        {
            Folder = folder;

            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }

            foreach (var doc in documents.Where(x => x.Value != null))
            {

                doc.Value.Folder = folder;
                if (doc.Key == "LIM")
                {
                    if (System.IO.File.Exists(doc.Value.Path)) System.IO.File.Delete(doc.Value.Path);

                    System.IO.File.Copy(doc.Value.BasePath, doc.Value.Path);
                }
                else
                {
                    doc.Value.Folder = folder;
                    doc.Value.Document.SaveToFile(doc.Value.Path);
                    doc.Value.Document.File = doc.Value.Path;
                }
            }
        }

        public override Result GetResults()
        {
            throw new NotImplementedException();
        }

        //public Dictionary<string, object> GetFut()
        public List<object> GetFut()
        {

            var result = new List<object>();

            var inp = (Inp)this[DeckDocument.inp].Document;
            var fut = System.IO.Path.Combine(Folder, inp.FutFile);

            var fcontent = System.IO.File.ReadAllLines(fut);

            if (fcontent.Length > 1)
            {
                var arr = fcontent[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                result.AddRange(new object[] { int.Parse(arr[0]), arr[1], arr[2], int.Parse(arr[3]),
                    double.Parse(arr[4], System.Globalization.NumberFormatInfo.InvariantInfo),
                    double.Parse(arr[5], System.Globalization.NumberFormatInfo.InvariantInfo),
                    double.Parse(arr[6], System.Globalization.NumberFormatInfo.InvariantInfo),
                    double.Parse(arr[7], System.Globalization.NumberFormatInfo.InvariantInfo),
                    double.Parse(arr[8], System.Globalization.NumberFormatInfo.InvariantInfo),
                    double.Parse(arr[9], System.Globalization.NumberFormatInfo.InvariantInfo)
                });
            }

            return result;

        }
    }
}
