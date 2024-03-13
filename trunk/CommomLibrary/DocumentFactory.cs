using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary
{
    public class DocumentFactory
    {

        public static BaseDocument Create(string filePath)
        {

            var fileName = System.IO.Path.GetFileName(filePath).ToLowerInvariant();
            BaseDocument doc;

            if (fileName.StartsWith("dadger"))
            {
                doc = BaseDocument.Create<Dadger.Dadger>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.Equals("dger.nwd"))
            {
                doc = BaseDocument.Create<DgerNwd.DgerNwd>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("dger"))
            {
                doc = BaseDocument.Create<DgerDat.DgerDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("dadgnl"))
            {
                doc = BaseDocument.Create<Dadgnl.Dadgnl>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("sistema"))
            {
                doc = BaseDocument.Create<SistemaDat.SistemaDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("curva"))
            {
                doc = BaseDocument.Create<CurvaDat.CurvaDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("ree.dat"))
            {
                doc = BaseDocument.Create<ReeDat.ReeDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("agrint.dat"))
            {
                doc = BaseDocument.Create<AgrintDat.AgrintDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("adterm.dat"))
            {
                doc = BaseDocument.Create<AdtermDat.AdtermDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("re.dat"))
            {
                doc = BaseDocument.Create<ReDat.ReDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("re_base.dat"))
            {
                doc = BaseDocument.Create<ReDat.ReDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("c_adic"))
            {
                doc = BaseDocument.Create<C_AdicDat.C_AdicDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("patamar.dat"))
            {
                doc = new PatamarDat.PatamarDat(filePath);
            }
            else if (fileName.StartsWith("eafpast"))
            {
                doc = BaseDocument.Create<Eafpast.Eafpast>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("dsvagua"))
            {
                doc = BaseDocument.Create<Dsvagua.Dsvagua>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("vazpast"))
            {
                doc = BaseDocument.Create<Vazpast.Vazpast>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("operut"))
            {
                doc = BaseDocument.Create<Operut.Operut>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("ptoper"))
            {
                doc = BaseDocument.Create<PtoperDat.PtoperDat>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("entdados"))
            {
                doc = BaseDocument.Create<EntdadosDat.EntdadosDat>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("parque_eolico_subm.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.Subm>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("hist-ventos.csv"))
            {
                doc = BaseDocument.Create<EolicaNW.HistVentos>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("indices.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.Indices>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("indices_gevazp.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.IndicesGevazp>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("parque_eolico_cadastro.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.Cadastro>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("eolica-cadastro.csv"))
            {
                doc = BaseDocument.Create<EolicaNW.EolicaCad>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("eolica-config.csv"))
            {
                doc = BaseDocument.Create<EolicaNW.EolicaConfig>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("eolica-submercado.csv"))
            {
                doc = BaseDocument.Create<EolicaNW.EolSubmercado>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("eolica-geracao.csv"))
            {
                doc = BaseDocument.Create<EolicaNW.EolicaGeracao>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("eolica-fte.csv"))
            {
                doc = BaseDocument.Create<EolicaNW.Eolicafte>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("eolica-posto.csv"))
            {
                doc = BaseDocument.Create<EolicaNW.EolicaPosto>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("parque_eolico_config.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.Config>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("parque_eolico_fte.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.Fte>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("parque_eolico_geracao.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.Geracao>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("parque_eolico_pot_instalada.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.PotInst>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("pee-posto.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.PeePosto>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("posto-cadastro.csv"))
            {
                doc = BaseDocument.Create<ParqueEolico.PostoCad>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("prevsvel."))
            {
                doc = BaseDocument.Create<ParqueEolico.PrevsVel>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("enasemanal"))
            {
                doc = BaseDocument.Create<EnaSemanalLog.EnaSemanalLog>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("operuh"))
            {
                doc = BaseDocument.Create<Operuh.Operuh>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("deflant"))
            {
                doc = BaseDocument.Create<Deflant.Deflant>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("respot"))
            {
                doc = BaseDocument.Create<Respot.Respot>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("dessem.arq"))
            {
                doc = BaseDocument.Create<DessemArq.DessemArq>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("dadvaz"))
            {
                doc = BaseDocument.Create<Dadvaz.Dadvaz>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("renovaveis"))
            {
                doc = BaseDocument.Create<Renovaveis.Renovaveis>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("areacont"))
            {
                doc = BaseDocument.Create<Areacont.Areacont>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("cotasr11"))
            {
                doc = BaseDocument.Create<Cotasr.Cotasr>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("config"))
            {
                doc = BaseDocument.Create<ConfigDat.ConfigDat>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("decodess.arq"))
            {
                doc = BaseDocument.Create<DecodessArq.DecodessArq>(System.IO.File.ReadAllText(filePath, Encoding.Default));
            }
            else if (fileName.StartsWith("modif"))
            {
                var fileText = System.IO.File.ReadAllText(filePath);
                if (fileText.Length < 1500) doc = BaseDocument.Create<ModifDat.ModifDat>(System.IO.File.ReadAllText(filePath));
                else doc = BaseDocument.Create<ModifDatNW.ModifDatNw>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("exph"))
            {
                doc = BaseDocument.Create<ExphDat.ExphDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("manutt"))
            {
                doc = BaseDocument.Create<ManuttDat.ManuttDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("confhd"))
            {
                doc = BaseDocument.Create<ConfhdDat.ConfhdDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("conft"))
            {
                doc = BaseDocument.Create<ConftDat.ConftDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("clast"))
            {
                doc = BaseDocument.Create<ClastDat.ClastDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("cadterm"))
            {
                doc = BaseDocument.Create<CadTermDat.CadTermDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("term"))
            {
                doc = BaseDocument.Create<TermDat.TermDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("expt"))
            {
                doc = BaseDocument.Create<ExptDat.ExptDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("ghmin"))
            {
                doc = BaseDocument.Create<GhminDat.GhminDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.StartsWith("vazoes"))
            {
                doc = new Compass.CommomLibrary.VazoesC.VazoesC(System.IO.File.ReadAllBytes(filePath));
            }
            else if (fileName.StartsWith("hidr"))
            {
                doc = new Compass.CommomLibrary.HidrDat.HidrDat(System.IO.File.ReadAllBytes(filePath));
            }
            else if (fileName.StartsWith("postos"))
            {
                doc = new Compass.CommomLibrary.PostosDat.PostosDat(System.IO.File.ReadAllBytes(filePath));
            }
            else if (fileName.StartsWith("prevs"))
            {
                doc = BaseDocument.Create<Prevs.Prevs>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.Equals("mlt.dat"))
            {
                doc = new Compass.CommomLibrary.MltDat.MltDat(System.IO.File.ReadAllBytes(filePath));
            }
            else if (fileName.StartsWith("pmo.dat"))
            {
                doc = new Pmo.Pmo(filePath);
            }
            else if (fileName.StartsWith("parp.dat"))
            {
                doc = new ParpDat.ParpDat(filePath);
            }
            else if (fileName.StartsWith("consulta.nwd"))
            {
                doc = new ConsultaNwd.ConsultaNwd(filePath);
            }
            else if (fileName.StartsWith("relato"))
            {
                doc = new Relato.Relato(filePath);
            }
            else if (fileName.StartsWith("inviab_unic"))
            {
                doc = new Inviab.Inviab(filePath);
            }
            else if (fileName.StartsWith("ipdo"))
            {
                doc = BaseDocument.Create<Compass.CommomLibrary.Ipdo.Ipdo>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.Equals("prevcen.dat"))
            {
                doc = BaseDocument.Create<Compass.CommomLibrary.PrevcenDat.PrevcenDat>(System.IO.File.ReadAllText(filePath));
            }
            else if (fileName.EndsWith("_str.dat"))
            {
                doc = BaseDocument.Create<Compass.CommomLibrary.Previvaz.Str>(filePath);
            }
            else
            {
                doc = new Compass.CommomLibrary.DummyDocument(filePath);
                //throw new ArgumentException("Tipo de arquivo não preparado para leitura");
            }

            doc.File = filePath;

            return doc;

        }

    }
}
