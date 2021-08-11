using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class EntidadosDat : BaseDocument
    {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"RD", new RdBlock()},
                    {"RIVAR", new RivarBlock()},
                    {"TM", new TmBlock()},
                    {"SIST", new SistBlock()},
                    {"REE", new ReeBlock()},
                    {"UH", new UhBlock()},
                    {"TVIAG", new TviagBlock()},
                    {"UT", new UtBlock()},
                    {"USIE", new UsieBlock()},
                    {"DP", new DpBlock()},
                    {"DE", new DeBlock()},
                    {"CD", new CdBlock()},
                    {"PQ", new PqBlock()},
                    {"IT", new ItBlock()},
                    {"RI", new RiBlock()},
                    {"IA", new IaBlock()},
                    {"GP", new GpBlock()},
                    {"NI", new NiBlock()},
                    {"VE", new VeBlock()},
                    {"CI CE", new CiceBlock()},
                    {"RE LU FH FT FI FE FR FC" , new RheBlock()},
                    {"AC", new AcBlock()},
                    {"DA", new DaBlock()},
                    {"FP", new FpBlock()},
                    {"TX", new TxBlock()},
                    {"EZ", new EzBlock()},
                    {"AG", new AgBlock()},
                    {"SECR", new SecrBlock()},
                    {"MH", new MhBlock()},
                    {"CR", new CrBlock()},
                    {"R11", new R11Block()},
                    {"MT", new MtBlock()},
                    {"VR", new VrBlock()},
                    {"PD", new PdBlock()},
                    {"VM", new VmBlock()},
                    {"DF", new DfBlock()},
                    {"ME", new MeBlock()},
                    {"META", new MetaBlock()},
                    {"SH", new ShBlock()},
                    {"TF", new TfBlock()},
                    {"RS", new RsBlock()},
                    {"SP", new SpBlock()},
                    {"PS", new PsBlock()},
                    {"PP", new PpBlock()},




                };
        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get { return blocos; }
        }

        public TmBlock BlocoTm { get { return (TmBlock)Blocos["TM"]; } set { Blocos["TM"] = value; } }
        public PpBlock BlocoPp { get { return (PpBlock)Blocos["PP"]; } set { Blocos["PP"] = value; } }
        public PsBlock BlocoPs { get { return (PsBlock)Blocos["PS"]; } set { Blocos["PS"] = value; } }
        public SpBlock BlocoSp { get { return (SpBlock)Blocos["SP"]; } set { Blocos["SP"] = value; } }
        public RsBlock BlocoRs { get { return (RsBlock)Blocos["RS"]; } set { Blocos["RS"] = value; } }
        public TxBlock BlocoTx { get { return (TxBlock)Blocos["TX"]; } set { Blocos["TX"] = value; } }
        public TfBlock BlocoTf { get { return (TfBlock)Blocos["TF"]; } set { Blocos["TF"] = value; } }
        public ShBlock BlocoSh { get { return (ShBlock)Blocos["SH"]; } set { Blocos["SH"] = value; } }
        public NiBlock BlocoNi { get { return (NiBlock)Blocos["NI"]; } set { Blocos["NI"] = value; } }
        public GpBlock BlocoGp { get { return (GpBlock)Blocos["GP"]; } set { Blocos["GP"] = value; } }
        public AgBlock BlocoAg { get { return (AgBlock)Blocos["AG"]; } set { Blocos["AG"] = value; } }
        public DaBlock BlocoDa { get { return (DaBlock)Blocos["DA"]; } set { Blocos["DA"] = value; } }
        public VrBlock BlocoVr { get { return (VrBlock)Blocos["VR"]; } set { Blocos["VR"] = value; } }
        public RdBlock BlocoRd { get { return (RdBlock)Blocos["RD"]; } set { Blocos["RD"] = value; } }
        public PdBlock BlocoPd { get { return (PdBlock)Blocos["PD"]; } set { Blocos["PD"] = value; } }
        public SistBlock BlocoSist { get { return (SistBlock)Blocos["SIST"]; } set { Blocos["SIST"] = value; } }
        public IaBlock BlocoIa { get { return (IaBlock)Blocos["IA"]; } set { Blocos["IA"] = value; } }
        public ReeBlock BlocoRee { get { return (ReeBlock)Blocos["REE"]; } set { Blocos["REE"] = value; } }
        public UhBlock BlocoUh { get { return (UhBlock)Blocos["UH"]; } set { Blocos["Uh"] = value; } }
        public UtBlock BlocoUt { get { return (UtBlock)Blocos["UT"]; } set { Blocos["UT"] = value; } }
        public UsieBlock BlocoUsie { get { return (UsieBlock)Blocos["USIE"]; } set { Blocos["USIE"] = value; } }
        public SecrBlock BlocoSecr { get { return (SecrBlock)Blocos["SECR"]; } set { Blocos["SECR"] = value; } }
        public PqBlock BlocoPq { get { return (PqBlock)Blocos["PQ"]; } set { Blocos["PQ"] = value; } }
        public DeBlock BlocoDe { get { return (DeBlock)Blocos["DE"]; } set { Blocos["DE"] = value; } }
        public CiceBlock BlocoCice { get { return (CiceBlock)Blocos["CI CE"]; } set { Blocos["CI CE"] = value; } }
        public DpBlock BlocoDp { get { return (DpBlock)Blocos["DP"]; } set { Blocos["DP"] = value; } }
        public CdBlock BlocoCd { get { return (CdBlock)Blocos["CD"]; } set { Blocos["CD"] = value; } }
        public VeBlock BlocoVe { get { return (VeBlock)Blocos["VE"]; } set { Blocos["VE"] = value; } }
        public VmBlock BlocoVm { get { return (VmBlock)Blocos["VM"]; } set { Blocos["VM"] = value; } }
        public DfBlock BlocoDf { get { return (DfBlock)Blocos["DF"]; } set { Blocos["DF"] = value; } }
        public TviagBlock BlocoTviag { get { return (TviagBlock)Blocos["TVIAG"]; } set { Blocos["TVIAG"] = value; } }
        public FpBlock BlocoFp { get { return (FpBlock)Blocos["FP"]; } set { Blocos["FP"] = value; } }
        public EzBlock BlocoEz { get { return (EzBlock)Blocos["EZ"]; } set { Blocos["EZ"] = value; } }
        public AcBlock BlocoAc { get { return (AcBlock)Blocos["AC"]; } set { Blocos["AC"] = value; } }
        public CrBlock BlocoCr { get { return (CrBlock)Blocos["CR"]; } set { Blocos["CR"] = value; } }
        public MhBlock BlocoMh { get { return (MhBlock)Blocos["MH"]; } set { Blocos["MH"] = value; } }
        public MtBlock BlocoMt { get { return (MtBlock)Blocos["MT"]; } set { Blocos["MH"] = value; } }
        public MeBlock BlocoMe { get { return (MeBlock)Blocos["ME"]; } set { Blocos["ME"] = value; } }
        public RheBlock BlocoRhe { get { return (RheBlock)Blocos["RE LU FH FT FI FE FR FC"]; } set { Blocos["RE LU FH FT FI FE FR FC"] = value; } }
        public MetaBlock BlocoMeta { get { return (MetaBlock)Blocos["META"]; } set { Blocos["META"] = value; } }
        public ItBlock BlocoIt { get { return (ItBlock)Blocos["IT"]; } set { Blocos["IT"] = value; } }
        public RiBlock BlocoRi { get { return (RiBlock)Blocos["RI"]; } set { Blocos["RI"] = value; } }
        public R11Block BlocoR11 { get { return (R11Block)Blocos["R11"]; } set { Blocos["R11"] = value; } }
        public RivarBlock BlocoRivar { get { return (RivarBlock)Blocos["RIVAR"]; } set { Blocos["RIVAR"] = value; } }

        public override void Load(string fileContent)
        {
            var lines = fileContent.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);


            string comments = null;
            foreach (var line in lines)
            {
                if (IsComment(line))
                {
                    comments = comments == null ? line : comments + Environment.NewLine + line;
                }
                else
                {
                    var cod = line.Split(' ').First();
                    //var cod = (line + "  ").Substring(0, 2);

                    if (Blocos.Keys.Any(k => k.Split(' ').Contains(cod)))
                    {
                        var block = Blocos.First(k => k.Key.Split(' ').Contains(cod)).Value;
                        var newLine = block.CreateLine(line);

                        newLine.Comment = comments;
                        comments = null;
                        block.Add(newLine);
                    }
                }
            }

            if (comments != null)
            {
                BottonComments = comments;
            }
        }

        public override bool IsComment(string line)
        {
            return line.StartsWith("&");
        }
    }
}
