using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.PostosDat
{
    public class PostosBlock : BaseBlock<PostosLine>
    {
    }

    public class PostosLine : BaseLine
    {
        public static int Size = 20;
        public static BaseField[] _campos = new BaseField[] {
            new BaseField(  1, 1, "", "Cod"          ),
            new BaseField(1 ,12 ,"A12","Posto"),
            new BaseField(13,16,"I4","InicioHistorico"),
            new BaseField(17,20,"I4","FinalHistorico"),
        };

        public override BaseField[] Campos
        {
            get
            {
                return _campos;
            }
        }

        public int Cod { get { return this[0]; } }


        public string Usina { get { return this["Posto"]; } set { this["Posto"] = value; } }
        public int FinalHistorico { get { return this["FinalHistorico"]; } set { this["FinalHistorico"] = value; } }


    }
}
