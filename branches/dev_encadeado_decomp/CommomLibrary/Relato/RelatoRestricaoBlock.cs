using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compass.CommomLibrary.Relato {
    public class RelatoRestricaoEletricaBlock : BaseBlock<RelatoRestricaoEletricaLine> {
    }


    public class RelatoRestricaoEletricaLine : BaseLine {
        public static readonly BaseField[] campos = new BaseField[] {                
                new BaseField(0  , 0  ,"I3"    , "ESTAGIO"),
                new BaseField(0  , 0  ,"A5"    , "Patamar"),
                new BaseField(7  , 9  ,"I3"  , "Restricao"),
                new BaseField(16 , 27 ,"A12"  , "Usina"),
                new BaseField(29 , 33 ,"F7.2"  , "Fator"),
                new BaseField(37 , 45 ,"F7.2"  , "MWmed"),
                new BaseField(47 , 55 ,"F7.2"  , "Produto"),
                new BaseField(57 , 89 ,"A33"   , "Limites"),
                new BaseField(0  , 0  ,"F7.2"  , "Limite Inf"),
                new BaseField(0  , 0  ,"F7.2"  , "Limite Sup"),
                new BaseField(0  , 0  ,"A33"  , "Observacao"),

        };


        public override BaseField[] Campos {
            get { return campos; }
        }
    }
}
