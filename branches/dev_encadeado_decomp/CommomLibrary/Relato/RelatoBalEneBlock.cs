using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Relato {
    public class RelatoBalEneBlock : BaseBlock<RelatoBalEneLine> {        
    }


    public class RelatoBalEneLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(0  , 0  ,"A2"    , "Subsistema"),
                new BaseField(0  , 0  ,"I3"    , "ESTAGIO"),
                new BaseField(5  , 9  ,"A5"    , "Patamar"),
                new BaseField(11 , 17 ,"F7.2"  , "Carga"),
                new BaseField(19 , 25 ,"F7.2"  , "Bacia"),
                new BaseField(27 , 33 ,"F7.2"  , "Cbomba"),
                new BaseField(35 , 41 ,"F7.2"  , "Ghid"),
                new BaseField(43 , 49 ,"F7.2"  , "Gter"),
                new BaseField(51 , 57 ,"F7.2"  , "GterAT"),
                new BaseField(59 , 65 ,"F7.2"  , "Deficit"),
                new BaseField(67 , 73 ,"F7.2"  , "Compra"),
                new BaseField(75 , 81 ,"F7.2"  , "Venda"),
                new BaseField(87 , 94 ,"F7.2"  , "Intercambio Liq"),
                new BaseField(98 , 104,"F7.2"  , "Itaipu50"),
                new BaseField(106, 112,"F7.2"  , "Itaipu60"),
        };



        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class RelatoIntercBlock : BaseBlock<RelatoIntercLine> {

    }


    public class RelatoIntercLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(0  , 0  ,"A2"    , "Subsistema"),
                new BaseField(0  , 0  ,"I3"    , "ESTAGIO"),
                new BaseField(0  , 0  ,"A5"    , "Patamar"),
                new BaseField(84 , 85 ,"A2"    , "Destino"),
                new BaseField(87 , 94 ,"F7.2"  , "Intercambio"),

        };
                
        
        public override BaseField[] Campos {
            get { return campos; }
        }
    }

    public class RelatoEarmSistBlock : BaseBlock<RelatoEarmSistLine> {

    }


    public class RelatoEarmSistLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {
                new BaseField(0  , 0  ,"A2"    , "Subsistema"),
                new BaseField(0  , 0  ,"I3"    , "ESTAGIO"),
                new BaseField(15 , 21 ,"F7.2"  , "EARM_ini"),
                new BaseField(38 , 44 ,"F7.2"  , "ENA"),
                new BaseField(65 , 71 ,"F7.2"  , "EARM_fim"),

        };


        public override BaseField[] Campos {
            get { return campos; }
        }
    }



}
