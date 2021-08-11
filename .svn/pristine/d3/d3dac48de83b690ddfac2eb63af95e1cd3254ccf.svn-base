using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Newave
{
    public class Deck : BaseDeck
    {

        public enum DeckDocument
        {
            sistema,
            patamar,
            modif,
            exph,
            confhd,
            cadterm,
            expt,
            term,
            conft,
            dger,
            ree,
            vazpast,
            vazoes,
            re,
            agrint,
            manutt,
            clast,
            dsvagua,
            cadic,
            postos,
            ghmin,
            gtminAgente,
            adterm,
        }

        Dictionary<string, DeckFile> documents = new Dictionary<string, DeckFile> {
            {"ADTERM.DAT"  , null},
            {"AGRINT.DAT"      , null},
            {"ARQUIVOS.DAT"  , null},
            {"BID.DAT"     , null},
            {"CADTERM.DAT"    , null},
            {"CASO.DAT"   , null},
            {"CDEFVAR.DAT"   , null},
            {"CLAST.DAT"  , null},
            {"CONFHD.DAT"   , null},
            {"CONFT.DAT"  , null},
            {"CURVA.DAT"  , null},
            {"CVAR.DAT"     , null},
            {"C_ADIC.DAT"   , null},
            {"DefPatMen.Ini"    , null},
            {"DGER.DAT"     , null},
            {"DSVAGUA.DAT"     , null},
            {"eafpast.dat"    , null},
            {"ELNINO.DAT" , null},
            {"ENSOAUX.DAT"    , null},
            {"EXPH.DAT"  , null},
            {"EXPT.DAT"      , null},
            {"FORMAT.TMP"     , null},
            {"GtminAgenteCDE.xlsx"     , null},
            {"GHMIN.DAT"     , null},
            {"GTMINPAT.DAT"     , null},
            {"HIDR.DAT"       , null},
            {"ITAIPU.DAT"    , null},
            {"LOSS.DAT"    , null},
            {"MANUTT.DAT"   , null},
            {"MENSAG.TMP" , null},
            {"MODIF.DAT"  , null},
            {"NewaveMsgPortug.txt"    , null},
            {"PATAMAR.DAT"   , null},
            {"PENALID.DAT"   , null},
            {"POSTOS.DAT"  , null},
            {"SAR.DAT"   , null},
            {"SHIST.DAT"  , null},
            {"SISTEMA.DAT"  , null},
            {"TERM.DAT"     , null},
            {"VAZOES.DAT"   , null},
            {"VAZPAST.DAT"    , null},
            {"REE.DAT"    , null},
            {"RE.DAT"    , null},
            {"SELCOR.DAT"    , null},
            {"TECNO.DAT"    , null},

        };

        public override Dictionary<string, DeckFile> Documents { get { return documents; } }

        public DeckFile this[DeckDocument doc]
        {
            get
            {
                switch (doc)
                {
                    case DeckDocument.sistema:
                        return Documents["SISTEMA.DAT"];
                    case DeckDocument.patamar:
                        return Documents["PATAMAR.DAT"];
                    case DeckDocument.modif:
                        return Documents["MODIF.DAT"];
                    case DeckDocument.exph:
                        return Documents["EXPH.DAT"];
                    case DeckDocument.confhd:
                        return Documents["CONFHD.DAT"];
                    case DeckDocument.expt:
                        return Documents["EXPT.DAT"];
                    case DeckDocument.term:
                        return Documents["TERM.DAT"];
                    case DeckDocument.cadterm:
                        return Documents["CADTERM.DAT"];
                    case DeckDocument.conft:
                        return Documents["CONFT.DAT"];
                    case DeckDocument.dger:
                        return Documents["DGER.DAT"];
                    case DeckDocument.ree:
                        return Documents["REE.DAT"];
                    case DeckDocument.vazpast:
                        return Documents["VAZPAST.DAT"];
                    case DeckDocument.vazoes:
                        return Documents["VAZOES.DAT"];
                    case DeckDocument.re:
                        return Documents["RE.DAT"];
                    case DeckDocument.agrint:
                        return Documents["AGRINT.DAT"];
                    case DeckDocument.manutt:
                        return Documents["MANUTT.DAT"];
                    case DeckDocument.clast:
                        return Documents["CLAST.DAT"];
                    case DeckDocument.dsvagua:
                        return Documents["DSVAGUA.DAT"];
                    case DeckDocument.cadic:
                        return Documents["C_ADIC.DAT"];
                    case DeckDocument.postos:
                        return Documents["POSTOS.DAT"];
                    case DeckDocument.ghmin:
                        return Documents["GHMIN.DAT"];
                    case DeckDocument.gtminAgente:
                        return Documents["GtminAgenteCDE.xlsx"];
                    case DeckDocument.adterm:
                        return Documents["ADTERM.DAT"];
                    default:
                        return null;
                }
            }
            set
            {

                switch (doc)
                {
                    case DeckDocument.sistema:
                        Documents["SISTEMA.DAT"] = value;
                        break;
                    case DeckDocument.patamar:
                        Documents["PATAMAR.DAT"] = value;
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public string Folder { get; set; }

        public override void GetFiles(string baseFolder)
        {

            var folderFiles = System.IO.Directory.GetFiles(baseFolder).Where(x => !x.EndsWith(".bak", StringComparison.OrdinalIgnoreCase));

            var q = from doc in documents
                    from file in folderFiles
                    where System.IO.Path.GetFileName(file).Equals(doc.Key, StringComparison.OrdinalIgnoreCase)
                    select new { doc.Key, file };

            q.ToList().ForEach(x =>
            {
                documents[x.Key] = new DeckFile(x.file);
            });
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

        public void SaveFilesToFolder(string folder)
        {
            Folder = folder;

            if (!System.IO.Directory.Exists(folder))
            {
                System.IO.Directory.CreateDirectory(folder);
            }

            foreach (var doc in documents.Where(x => x.Value != null))
            {

                doc.Value.Folder = folder;
                doc.Value.Document.SaveToFile(doc.Value.Path);
            }

        }


        Result result = null;
        public override Result GetResults()
        {

            if (result != null) return result;

            try
            {

                result = new Result(this.BaseFolder) { Tipo = "NW" };

                var files = Directory.GetFiles(BaseFolder).ToList();

                var pmoPath = files.FirstOrDefault(x => Path.GetFileNameWithoutExtension(x).ToLower().Equals("pmo"));

                //Compass.CommomLibrary.Pmo.Pmo pmo = null;

                Compass.CommomLibrary.ReeDat.ReeDat ree = null;
                Compass.CommomLibrary.SistemaDat.SistemaDat sistema = null;

                Compass.CommomLibrary.ParpDat.ParpDat parp = null;
                Compass.CommomLibrary.ConsultaNwd.ConsultaNwd consulta = null;


                var parpPath = files.FirstOrDefault(x => Path.GetFileName(x).ToLower().Equals("parp.dat"));
                if (parpPath != null)
                    parp = (Compass.CommomLibrary.ParpDat.ParpDat)DocumentFactory.Create(parpPath);

                var consultaPath = files.FirstOrDefault(x => Path.GetFileName(x).ToLower().Equals("consulta.nwd"));
                if (consultaPath != null)
                    consulta = (Compass.CommomLibrary.ConsultaNwd.ConsultaNwd)DocumentFactory.Create(consultaPath);

                var reePath = files.FirstOrDefault(x => Path.GetFileName(x).ToLower().Equals("ree.dat"));
                if (reePath != null)
                    ree = (Compass.CommomLibrary.ReeDat.ReeDat)DocumentFactory.Create(reePath);

                var sistemaPath = files.FirstOrDefault(x => Path.GetFileName(x).ToLower().Equals("sistema.dat"));
                if (sistemaPath != null)
                    sistema = (Compass.CommomLibrary.SistemaDat.SistemaDat)DocumentFactory.Create(sistemaPath);


                var mercados = (from def in sistema.Blocos["Deficit"]
                                join rees in ree on def[0] equals rees[2]
                                select new { ReeId = rees[0], Ree = rees[1].Trim(), SistemaId = def[0], Sistema = def[1].Trim() })
                               .GroupBy(x => (int)x.SistemaId);


                if (consulta != null && parp != null)
                {

                    //cmo
                    var patW = consulta.Patamar.OrderBy(x => x[0]).Select(x => (double)x[1]).ToArray();

                    foreach (var cmo in consulta.Cmo.ToList().GroupBy(x =>
                    {
                        switch (x.Mercado.Trim())
                        {
                            case "SUDESTE": return SistemaEnum.SE;
                            case "SUL": return SistemaEnum.S;
                            case "NORDESTE": return SistemaEnum.NE;
                            case "NORTE": return SistemaEnum.N;
                            default: throw new Exception();
                        };
                    }
                        ))
                    {
                        result[cmo.Key].Cmo_pat1 = cmo.Where(x => x.Pat == 1).Sum(x => x.Cmo);
                        result[cmo.Key].Cmo_pat2 = cmo.Where(x => x.Pat == 2).Sum(x => x.Cmo);
                        result[cmo.Key].Cmo_pat3 = cmo.Where(x => x.Pat == 3).Sum(x => x.Cmo);
                        result[cmo.Key].Cmo = cmo.Sum(x => x.Cmo * patW[x.Pat - 1]);
                    }
                    foreach (var mer in consulta.Mercado)
                    {
                        result[mer.Mercado].DemandaMes = mer.DemandaBru;
                        result[mer.Mercado].GerHidr = mer.GerHidr;
                        result[mer.Mercado].GerTerm = mer.GerTerm;
                        result[mer.Mercado].GerPeq = mer.GerPeq;
                    }

                    var mesAtualIdx = 0;


                    foreach (var c_t in Compass.CommomLibrary.ParpDat.MLTLine.campos)
                    {
                        if (c_t.Nome == consulta.Mes) break;
                        mesAtualIdx++;
                    }


                    foreach (var mercado in mercados)
                    {

                        var rees = consulta.Ree.Where(x =>
                            mercado.Select(y => y.Ree).Contains(x.Ree)
                            );

                        var mlts = parp.MLT.Where(x => mercado.Select(y => y.Ree).Contains(x.Ree));


                        if (BaseDeck.EnaMLT == null) BaseDeck.EnaMLT = new Dictionary<SistemaEnum, double[]>();
                        if (!BaseDeck.EnaMLT.ContainsKey((SistemaEnum)mercado.Key))
                        {
                            BaseDeck.EnaMLT[(SistemaEnum)mercado.Key] = new double[12];
                        }

                        BaseDeck.EnaMLT[(SistemaEnum)mercado.Key][mesAtualIdx - 1] = mlts.Sum(x => (double)x[mesAtualIdx]);
                        BaseDeck.EnaMLT[(SistemaEnum)mercado.Key][mesAtualIdx == 1 ? 11 : mesAtualIdx - 2] = mlts.Sum(x => (double)x[mesAtualIdx == 1 ? 12 : mesAtualIdx - 1]);

                        result[mercado.Key].EarmI = rees.Sum(x => x.Earmi) / rees.Sum(x => x.Earmx);

                        var ena = rees.Sum(x => (double)x["ENA " + consulta.Mes]);
                        var enaMLT = mlts.Sum(x => (double)x[consulta.Mes]);

                        result[mercado.Key].Ena = ena;
                        result[mercado.Key].EnaMLT = (ena / enaMLT);


                        result[mercado.Key].EnaTH = rees.Sum(x => (double)x["ENA " + consulta.MesAnterior]);
                        result[mercado.Key].EnaTHMLT = result[mercado.Key].EnaTH / mlts.Sum(x => (double)x[consulta.MesAnterior]);

                    }
                }
            }
            catch { }
            return result;
        }

        public DgerDat.DgerDat Dger
        {
            get
            {
                return this[DeckDocument.dger].Document as DgerDat.DgerDat;
            }
        }

        public SistemaDat.SistemaDat Sistema
        {
            get
            {
                return this[DeckDocument.sistema].Document as SistemaDat.SistemaDat;
            }
        }

        public ReeDat.ReeDat Ree
        {
            get
            {
                return this[DeckDocument.ree].Document as ReeDat.ReeDat;
            }
        }


    }
}
