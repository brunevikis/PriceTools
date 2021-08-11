using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dessem
{
    public enum DeckDocument
    {
        operut,
        areacont,
        cortdeco,
        cotasr11,
        curvtviag,
        dadvaz,
        deflant,
        dessem,
        entdados,
        hidr,
        ils_tri,
        infofcf,
        mapcut,
        mlt,
        operuh,
        ptoper,
        rampas,
        renovaveis,
        respot,
        respotele,
        restseg,
        rstlpp,
        termdat



    }
    public class Deck : BaseDeck
    {


        Dictionary<string, DeckFile> documents = new Dictionary<string, DeckFile> {
            {"operut.dat"   , null},
            {"areacont.dat", null },
            {"cortdeco.", null },
            {"cotasr11.dat", null },
            {"curvtviag.dat", null },
            {"dadvaz.dat", null },
            {"deflant.dat", null },
            {"dessem.arq", null },
            {"entdados.dat", null },
            {"hidr.dat", null },
            {"ils_tri.dat", null },
            {"infofcf.dat", null },
            {"mapcut.", null },
            {"mlt.dat", null },
            {"operuh.dat", null },
            {"ptoper.dat", null },
            {"rampas.dat", null },
            {"renovaveis.dat", null },
            {"respot.dat", null },
            {"respotele.dat", null },
            {"restseg.dat", null },
            {"rstlpp.dat", null },
            {"termdat.dat", null }



        };
        public override Dictionary<string, DeckFile> Documents { get { return documents; } }

        public DeckFile this[DeckDocument doc]
        {

            get
            {
                switch (doc)
                {
                    case DeckDocument.operut:
                        return Documents["operut.dat"];
                    case DeckDocument.entdados:
                        return Documents["entdados.dat"];
                    case DeckDocument.ptoper:
                        return Documents["ptoper.dat"];
                    case DeckDocument.dessem:
                        return Documents["dessem.arq"];
                    case DeckDocument.renovaveis:
                        return Documents["renovaveis.dat"];
                    case DeckDocument.dadvaz:
                        return Documents["dadvaz.dat"];
                    default:
                        return null;
                }
            }
            set
            {
                switch (doc)
                {
                    case DeckDocument.operut:
                        documents["operut.dat"] = value;
                        break;
                    case DeckDocument.ptoper:
                        documents["ptoper.dat"] = value;
                        break;
                    case DeckDocument.dessem:
                        documents["dessem.arq"] = value;
                        break;
                    case DeckDocument.renovaveis:
                        documents["renovaveis.dat"] = value;
                        break;
                    case DeckDocument.entdados:
                        documents["entdados.dat"] = value;
                        break;
                    case DeckDocument.dadvaz:
                        documents["dadvaz.dat"] = value;
                        break;
                    default:
                        break;
                }
            }
        }

        public string Folder { get; set; }
        public int Rev { get; set; }

        public string caso;
        public string Caso { get { return caso; } set { caso = value; } }

        public override void GetFiles(string baseFolder)
        {

            BaseFolder = baseFolder;

            var folderFiles = System.IO.Directory.GetFiles(baseFolder).Where(x => !x.EndsWith(".bak", StringComparison.OrdinalIgnoreCase));


            var q = from doc in documents
                    from file in folderFiles
                    let filename = System.IO.Path.GetFileName(file)
                    where (doc.Key.EndsWith(".") && filename.StartsWith(doc.Key, StringComparison.OrdinalIgnoreCase))
                    || (filename.Equals(doc.Key, StringComparison.OrdinalIgnoreCase))
                    select new { doc.Key, file };

            foreach (var item in q.ToList())
            {
                documents[item.Key] = new DeckFile(item.file);
            }
        }
        Result result = null;
        public override Result GetResults()
        {
            return null;
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
                System.IO.File.Copy(doc.Value.BasePath, doc.Value.Path, true);
            }




        }


    }
}
