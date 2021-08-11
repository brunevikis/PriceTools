using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EntdadosDat
{
    public class TmBlock : BaseBlock<TmLine>
    {




    }

    public class TmLine : BaseLine
    {
        public string IdBloco { get { return this[0].ToString(); } set { this[0] = value; } }
        public string DiaInicial { get { return this[1].ToString(); } set { this[1] = value; } }
        public int HoraDiaInicial { get { return (int)this[2]; } set { this[2] = value; } }
        public int MeiaHora { get { return (int)this[3]; } set { this[3] = value; } }
        public float Duracao { get { return (float)this[4]; } set { this[4] = value; } }
        public int Rede { get { return (int)this[5]; } set { this[5] = value; } }
        public string NomePatamar { get { return this[6].ToString(); } set { this[6] = value; } }

        public override BaseField[] Campos { get { return TmCampos; } }

        static readonly BaseField[] TmCampos = new BaseField[] {
                new BaseField(1  , 2 ,"A2"    , "IdBloco"),//
                new BaseField(5  , 6 ,"A2"    , "Dia inicial periodo"),//string pq pode ter F ou I
                new BaseField(10  , 11 ,"I2"    , "Hora diainicial"),//
                new BaseField(15  , 15 ,"I1"    , "Meia Hora"),//
                new BaseField(20  , 24 ,"F5.0"    , "Duracao"),//
                new BaseField(30  , 30 ,"I1"    , "Rede"),
                new BaseField(34  , 39 ,"A6"    , "Patamar"),


            };
    }
}