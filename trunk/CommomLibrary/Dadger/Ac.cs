using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger {
    public class AcBlock : BaseBlock<AcLine> {
        public override AcLine CreateLine(string line = null) {

            var mne = line.Substring(9, 6);
            var l = CreateLineFromMnemonico(mne);

            l.Load(line);

            return l;


        }

        public AcLine CreateLineFromMnemonico(string mnemonico) {
            AcLine l;
            switch (mnemonico) {
                case "NOMEUH":
                case "TIPUSI":
                    l = (AcLine)BaseLine.Create<AcA12Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "NUMPOS":
                case "NUMJUS":
                case "NUMCON":
                case "VERTJU":
                case "VAZMIN":
                case "NUMBAS":
                case "TIPTUR":
                case "TIPERH":
                case "JUSENA":
                    l = (AcLine)BaseLine.Create<AcI5Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "DESVIO":
                case "POTEFE":
                case "ALTEFE":
                case "NCHAVE":
                    l = (AcLine)BaseLine.Create<AcI5F10Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "VOLMIN":
                case "VOLMAX":
                
                case "PERHID":
                case "JUSMED":
                case "VSVERT":
                case "VMDESV":
                    l = (AcLine)BaseLine.Create<AcF10Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "PROESP":
                    l = (AcLine)BaseLine.Create<AcF10Lineext>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "COFEVA":
                case "NUMMAQ":
                case "VAZEFE":
                    l = (AcLine)BaseLine.Create<Ac2I5Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "COTVOL":
                case "COTARE":
                    l = (AcLine)BaseLine.Create<AcI5E15Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "COTVAZ":
                    l = (AcLine)BaseLine.Create<Ac2I5E15Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                case "VAZCCF":
                    l = (AcLine)BaseLine.Create<AcF10I5F10Line>();
                    l.Mnemonico = mnemonico;
                    return l;
                default:
                    return base.CreateLine();
            }
        }
    }

    public class AcLine : BaseLine {

        public AcLine()
            : base() {
            this[0] = "AC";
        }

        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 59,"A40"   , "Parametros"), // replace
                new BaseField( 60 , 60,"A1"    , ""), // replace
                new BaseField( 61 , 61,"A1"    , ""), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }

        public string Mnemonico { get { return this[2] ?? ""; } set { this[2] = value; } }
        public int Usina { get { return this[1]; } set { this[1] = value; } }

        public virtual dynamic P1 { get { return this[3]; } set { this[3] = value; } }
        public virtual dynamic P2 { get { return this[4]; } set { this[4] = value; } }
        public virtual dynamic P3 { get { return this[5]; } set { this[5] = value; } }

        public string Mes {
            get { return this[6] ?? ""; }
            set { this[6] = value; }
        }
        public int? Semana {
            get {
                dynamic val = this[7];
                return val;
            }
            set {
                if (value.HasValue)
                    this[7] = value.Value;
                else
                    this[7] = null;
            }

        }
        public int? Ano {
            get {
                dynamic val = this[8];
                // if (val is int)
                return val;
                //else {

                //    int r;
                //    if (int.TryParse(val.ToString().Trim(), out r))
                //        return r;
                //    else
                //        return (int?)null;
                //}
            }
            set {
                if (value.HasValue)
                    this[8] = value.Value;
                else
                    this[8] = null;
            }
        }
    }

    public class AcA1Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 20,"A1"    , "p1"), // replace
                new BaseField( 60 , 60,"A1"    , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class AcA12Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 31,"A12"   , "p1"), // replace
                new BaseField( 60 , 60,"A1"    , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class AcI5Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 24,"I5"    , "p1"), // replace
                new BaseField( 60 , 60,"A1"    , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class Ac2I5Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 24,"I5"    , "p1"), // replace
                new BaseField( 25 , 29,"I5"    , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class AcF10Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 29,"f10.2"    , "p1"), // replace
                new BaseField( 60 , 60,"A1"    , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class AcF10Lineext : AcLine
    {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 29,"f10.6"    , "p1"), // replace
                new BaseField( 60 , 60,"A1"    , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos
        {
            get { return AcCampos; }
        }
    }

    public class AcI5F10Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 24,"I5"    , "p1"), // replace
                new BaseField( 25 , 34,"F10.0" , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class AcF10I5F10Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 29,"F10.0" , "p1"), // replace
                new BaseField( 30 , 34,"I5"    , "p2"), // replace
                new BaseField( 35 , 44,"F10.0" , "p3"), // replace                
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class AcI5E15Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 24,"I5"    , "p1"), // replace
                new BaseField( 25 , 39,"E15.7" , "p2"), // replace
                new BaseField( 61 , 61,"A1"    , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }

    public class Ac2I5E15Line : AcLine {
        static readonly BaseField[] AcCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 15,"A6"    , "Mnemonico"),
                new BaseField( 20 , 24,"I5"    , "p1"), // replace
                new BaseField( 25 , 29,"I5"    , "p2"), // replace
                new BaseField( 30 , 44,"E15.7" , "p3"), // replace
                new BaseField( 70 , 73,"A3"    , "Mes"),
                new BaseField( 75 , 75,"I1"    , "Semana"),
                new BaseField( 77 , 80,"I4"    , "Ano"),
            };

        public override BaseField[] Campos {
            get { return AcCampos; }
        }
    }


}
