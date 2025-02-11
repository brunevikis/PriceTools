﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Dadger
{
    public class UhNwBlock : BaseBlock<UhNwLine>
    {

    }
    public class UhNwLine : BaseLine
    {

        public UhNwLine()
            : base()
        {
            this[0] = "UH";
        }

        static readonly BaseField[] UhNwCampos = new BaseField[] {
                new BaseField( 1  , 2 ,"A2"    , "Id"),
                new BaseField( 5  , 7 ,"I3"    , "Usina"),
                new BaseField( 10 , 11,"I2"    , "Subsistema"),
                new BaseField( 15 , 24,"f10.2" , "Volume Ini"),
                new BaseField( 25 , 34,"F10.0" , "Vazao Deflu Min"),
                new BaseField( 35 , 36,"I2"    , "Num valores FPEA"),
                new BaseField( 40 , 40,"I1"    , "Evaporacao"),
                new BaseField( 45 , 46,"I2"    , "Estagio"),
                new BaseField( 50 , 59,"F10.0"    , "Volume Morto Ini"),
                new BaseField( 60 , 69,"F10.0"    , "Limite Vertimento"),
                new BaseField( 70 , 70,"I1"    , "Bal Hidr Patamar"),
                new BaseField( 72 , 73,"A2"    , "Flag NW"),

            };

        public override BaseField[] Campos { get { return UhNwCampos; } }
        public double VolIniPerc
        {
            get { return this[3] == null ? 0d : (double)this[3]; }
            set
            {
                if (value > 100)
                {
                    value = 100.0f;
                }
                if (value < 0.1 && value > 0)
                {
                    value = 0.1f;
                }
                this[3] = value;
            }
        }
        public int Usina { get { return (int)this[1]; } set { this[1] = value; } }
        public int Sistema { get { return (int)this[2]; } set { this[2] = value; } }

        public bool Evaporacao { get { return this[6] == 1 ? true : false; } set { this[6] = value ? 1 : 0; } }

        public string NW { get { return (string)this[11]; } set { this[11] = value; } }

    }

}
