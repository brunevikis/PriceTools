using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.EnaSemanalLog
{
    public class DataSemanalBlock : BaseBlock<DataSemanalLine>
    {

      

    }

    public class DataSemanalLine : BaseLine
    {

        public string texto { get { return this[0].ToString(); } set { this[0] = value; } }
        public string sem1 { get { return this[1].ToString(); } set { this[1] = value; } }
        public string sem2 { get { return this[2].ToString(); } set { this[2] = value; } }
        public string sem3 { get { return this[3].ToString(); } set { this[3] = value; } }
        public string sem4 { get { return this[4].ToString(); } set { this[4] = value; } }
        public string sem5 { get { return this[5].ToString(); } set { this[5] = value; } }
        public string sem6 { get { return this[6].ToString(); } set { this[6] = value; } }
        public string sem7 { get { return this[7].ToString(); } set { this[7] = value; } }
        public string sem8 { get { return this[8].ToString(); } set { this[8] = value; } }
        public string sem9 { get { return this[9].ToString(); } set { this[9] = value; } }
        public string sem10 { get { return this[10].ToString(); } set { this[10] = value; } }
        public string sem11 { get { return this[11].ToString(); } set { this[11] = value; } }
        public string sem12 { get { return this[12].ToString(); } set { this[12] = value; } }
        




        public override BaseField[] Campos { get { return DataSemanalCampos; } }

        static readonly BaseField[] DataSemanalCampos = new BaseField[] {
                new BaseField(1  , 22 ,"A22"    , "texto"),
                new BaseField(23  , 35 ,"A13"    , "data1"),
                new BaseField(38  , 50 ,"A13"    , "data2"),
                new BaseField(53  , 65 ,"A13"    , "data3"),
                new BaseField(68  , 80 ,"A13"    , "data4"),
                new BaseField(83  , 95 ,"A13"    , "data5"),
                new BaseField(98  , 110 ,"A13"    , "data6"),
                new BaseField(113  , 125 ,"A13"    , "data7"),
                new BaseField(128  , 140 ,"A13"    , "data8"),
                new BaseField(143  , 155 ,"A13"    , "data9"),
                new BaseField(158  , 170 ,"A13"    , "data10"),
                new BaseField(173  , 185 ,"A13"    , "data11"),
                new BaseField(188  , 200 ,"A13"    , "data12"),
                

            };
    }
}
