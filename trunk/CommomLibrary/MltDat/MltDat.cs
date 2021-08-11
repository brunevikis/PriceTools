using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.MltDat {

    public class MltDat : BaseDocument {

        MltBlock conteudo;
        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return new Dictionary<string, IBlock<BaseLine>>() {
                    {"Mlt", conteudo}
                };
            }
        }


        public MltDat() {
            conteudo = new MltBlock();
        }

        public MltDat(byte[] content)
            : this() {


            var regNum = 12;

            for (int i = 0; i < regNum; i++) {

                var regBytes = content.Skip(MltLine.Size * i).Take(MltLine.Size).ToArray();


                MltLine reg = new MltLine();

                reg[0] = i + 1;
                for (int c = 1; c < reg.Campos.Length; c++) {
                    dynamic val = reg.Campos[c].ExtractValue(regBytes);
                    reg[c] = val;
                }
                conteudo.Add(reg);
            }
        }
        public override void SaveToFile(string filePath = null, bool createBackup = false)
        {

            filePath = filePath ?? File;

            if (createBackup && System.IO.File.Exists(filePath))
            {
                var bkp = filePath + DateTime.Now.ToString("_yyyyMMddHHmmss.bak");
                System.IO.File.Copy(filePath, bkp);
            }

            var content = ToBytes();
            System.IO.File.WriteAllBytes(filePath, content);

        }
        byte[] ToBytes()
        {

            var result = new List<byte>();

            foreach (var reg in conteudo)
            {
                var regBytes = new Byte[MltLine.Size];



                for (int i = 0; i < reg.Campos.Length; i++)
                {
                    reg.Campos[i].InsertValue(regBytes, reg[i]);
                }

                result.InsertRange(result.Count, regBytes);
            }

            return result.ToArray();
        }
    }
    


    public class MltBlock : BaseBlock<MltLine> {
    }
    public class MltLine : BaseLine {
        public static int Size = 1280;
        public static BaseField[] _campos = new BaseField[] {
            new BaseField(  1, 1, "", "Mes"          ),
new BaseField(1,4,"I","Usina 1"),
new BaseField(5,8,"I","Usina 2"),
new BaseField(9,12,"I","Usina 3"),
new BaseField(13,16,"I","Usina 4"),
new BaseField(17,20,"I","Usina 5"),
new BaseField(21,24,"I","Usina 6"),
new BaseField(25,28,"I","Usina 7"),
new BaseField(29,32,"I","Usina 8"),
new BaseField(33,36,"I","Usina 9"),
new BaseField(37,40,"I","Usina 10"),
new BaseField(41,44,"I","Usina 11"),
new BaseField(45,48,"I","Usina 12"),
new BaseField(49,52,"I","Usina 13"),
new BaseField(53,56,"I","Usina 14"),
new BaseField(57,60,"I","Usina 15"),
new BaseField(61,64,"I","Usina 16"),
new BaseField(65,68,"I","Usina 17"),
new BaseField(69,72,"I","Usina 18"),
new BaseField(73,76,"I","Usina 19"),
new BaseField(77,80,"I","Usina 20"),
new BaseField(81,84,"I","Usina 21"),
new BaseField(85,88,"I","Usina 22"),
new BaseField(89,92,"I","Usina 23"),
new BaseField(93,96,"I","Usina 24"),
new BaseField(97,100,"I","Usina 25"),
new BaseField(101,104,"I","Usina 26"),
new BaseField(105,108,"I","Usina 27"),
new BaseField(109,112,"I","Usina 28"),
new BaseField(113,116,"I","Usina 29"),
new BaseField(117,120,"I","Usina 30"),
new BaseField(121,124,"I","Usina 31"),
new BaseField(125,128,"I","Usina 32"),
new BaseField(129,132,"I","Usina 33"),
new BaseField(133,136,"I","Usina 34"),
new BaseField(137,140,"I","Usina 35"),
new BaseField(141,144,"I","Usina 36"),
new BaseField(145,148,"I","Usina 37"),
new BaseField(149,152,"I","Usina 38"),
new BaseField(153,156,"I","Usina 39"),
new BaseField(157,160,"I","Usina 40"),
new BaseField(161,164,"I","Usina 41"),
new BaseField(165,168,"I","Usina 42"),
new BaseField(169,172,"I","Usina 43"),
new BaseField(173,176,"I","Usina 44"),
new BaseField(177,180,"I","Usina 45"),
new BaseField(181,184,"I","Usina 46"),
new BaseField(185,188,"I","Usina 47"),
new BaseField(189,192,"I","Usina 48"),
new BaseField(193,196,"I","Usina 49"),
new BaseField(197,200,"I","Usina 50"),
new BaseField(201,204,"I","Usina 51"),
new BaseField(205,208,"I","Usina 52"),
new BaseField(209,212,"I","Usina 53"),
new BaseField(213,216,"I","Usina 54"),
new BaseField(217,220,"I","Usina 55"),
new BaseField(221,224,"I","Usina 56"),
new BaseField(225,228,"I","Usina 57"),
new BaseField(229,232,"I","Usina 58"),
new BaseField(233,236,"I","Usina 59"),
new BaseField(237,240,"I","Usina 60"),
new BaseField(241,244,"I","Usina 61"),
new BaseField(245,248,"I","Usina 62"),
new BaseField(249,252,"I","Usina 63"),
new BaseField(253,256,"I","Usina 64"),
new BaseField(257,260,"I","Usina 65"),
new BaseField(261,264,"I","Usina 66"),
new BaseField(265,268,"I","Usina 67"),
new BaseField(269,272,"I","Usina 68"),
new BaseField(273,276,"I","Usina 69"),
new BaseField(277,280,"I","Usina 70"),
new BaseField(281,284,"I","Usina 71"),
new BaseField(285,288,"I","Usina 72"),
new BaseField(289,292,"I","Usina 73"),
new BaseField(293,296,"I","Usina 74"),
new BaseField(297,300,"I","Usina 75"),
new BaseField(301,304,"I","Usina 76"),
new BaseField(305,308,"I","Usina 77"),
new BaseField(309,312,"I","Usina 78"),
new BaseField(313,316,"I","Usina 79"),
new BaseField(317,320,"I","Usina 80"),
new BaseField(321,324,"I","Usina 81"),
new BaseField(325,328,"I","Usina 82"),
new BaseField(329,332,"I","Usina 83"),
new BaseField(333,336,"I","Usina 84"),
new BaseField(337,340,"I","Usina 85"),
new BaseField(341,344,"I","Usina 86"),
new BaseField(345,348,"I","Usina 87"),
new BaseField(349,352,"I","Usina 88"),
new BaseField(353,356,"I","Usina 89"),
new BaseField(357,360,"I","Usina 90"),
new BaseField(361,364,"I","Usina 91"),
new BaseField(365,368,"I","Usina 92"),
new BaseField(369,372,"I","Usina 93"),
new BaseField(373,376,"I","Usina 94"),
new BaseField(377,380,"I","Usina 95"),
new BaseField(381,384,"I","Usina 96"),
new BaseField(385,388,"I","Usina 97"),
new BaseField(389,392,"I","Usina 98"),
new BaseField(393,396,"I","Usina 99"),
new BaseField(397,400,"I","Usina 100"),
new BaseField(401,404,"I","Usina 101"),
new BaseField(405,408,"I","Usina 102"),
new BaseField(409,412,"I","Usina 103"),
new BaseField(413,416,"I","Usina 104"),
new BaseField(417,420,"I","Usina 105"),
new BaseField(421,424,"I","Usina 106"),
new BaseField(425,428,"I","Usina 107"),
new BaseField(429,432,"I","Usina 108"),
new BaseField(433,436,"I","Usina 109"),
new BaseField(437,440,"I","Usina 110"),
new BaseField(441,444,"I","Usina 111"),
new BaseField(445,448,"I","Usina 112"),
new BaseField(449,452,"I","Usina 113"),
new BaseField(453,456,"I","Usina 114"),
new BaseField(457,460,"I","Usina 115"),
new BaseField(461,464,"I","Usina 116"),
new BaseField(465,468,"I","Usina 117"),
new BaseField(469,472,"I","Usina 118"),
new BaseField(473,476,"I","Usina 119"),
new BaseField(477,480,"I","Usina 120"),
new BaseField(481,484,"I","Usina 121"),
new BaseField(485,488,"I","Usina 122"),
new BaseField(489,492,"I","Usina 123"),
new BaseField(493,496,"I","Usina 124"),
new BaseField(497,500,"I","Usina 125"),
new BaseField(501,504,"I","Usina 126"),
new BaseField(505,508,"I","Usina 127"),
new BaseField(509,512,"I","Usina 128"),
new BaseField(513,516,"I","Usina 129"),
new BaseField(517,520,"I","Usina 130"),
new BaseField(521,524,"I","Usina 131"),
new BaseField(525,528,"I","Usina 132"),
new BaseField(529,532,"I","Usina 133"),
new BaseField(533,536,"I","Usina 134"),
new BaseField(537,540,"I","Usina 135"),
new BaseField(541,544,"I","Usina 136"),
new BaseField(545,548,"I","Usina 137"),
new BaseField(549,552,"I","Usina 138"),
new BaseField(553,556,"I","Usina 139"),
new BaseField(557,560,"I","Usina 140"),
new BaseField(561,564,"I","Usina 141"),
new BaseField(565,568,"I","Usina 142"),
new BaseField(569,572,"I","Usina 143"),
new BaseField(573,576,"I","Usina 144"),
new BaseField(577,580,"I","Usina 145"),
new BaseField(581,584,"I","Usina 146"),
new BaseField(585,588,"I","Usina 147"),
new BaseField(589,592,"I","Usina 148"),
new BaseField(593,596,"I","Usina 149"),
new BaseField(597,600,"I","Usina 150"),
new BaseField(601,604,"I","Usina 151"),
new BaseField(605,608,"I","Usina 152"),
new BaseField(609,612,"I","Usina 153"),
new BaseField(613,616,"I","Usina 154"),
new BaseField(617,620,"I","Usina 155"),
new BaseField(621,624,"I","Usina 156"),
new BaseField(625,628,"I","Usina 157"),
new BaseField(629,632,"I","Usina 158"),
new BaseField(633,636,"I","Usina 159"),
new BaseField(637,640,"I","Usina 160"),
new BaseField(641,644,"I","Usina 161"),
new BaseField(645,648,"I","Usina 162"),
new BaseField(649,652,"I","Usina 163"),
new BaseField(653,656,"I","Usina 164"),
new BaseField(657,660,"I","Usina 165"),
new BaseField(661,664,"I","Usina 166"),
new BaseField(665,668,"I","Usina 167"),
new BaseField(669,672,"I","Usina 168"),
new BaseField(673,676,"I","Usina 169"),
new BaseField(677,680,"I","Usina 170"),
new BaseField(681,684,"I","Usina 171"),
new BaseField(685,688,"I","Usina 172"),
new BaseField(689,692,"I","Usina 173"),
new BaseField(693,696,"I","Usina 174"),
new BaseField(697,700,"I","Usina 175"),
new BaseField(701,704,"I","Usina 176"),
new BaseField(705,708,"I","Usina 177"),
new BaseField(709,712,"I","Usina 178"),
new BaseField(713,716,"I","Usina 179"),
new BaseField(717,720,"I","Usina 180"),
new BaseField(721,724,"I","Usina 181"),
new BaseField(725,728,"I","Usina 182"),
new BaseField(729,732,"I","Usina 183"),
new BaseField(733,736,"I","Usina 184"),
new BaseField(737,740,"I","Usina 185"),
new BaseField(741,744,"I","Usina 186"),
new BaseField(745,748,"I","Usina 187"),
new BaseField(749,752,"I","Usina 188"),
new BaseField(753,756,"I","Usina 189"),
new BaseField(757,760,"I","Usina 190"),
new BaseField(761,764,"I","Usina 191"),
new BaseField(765,768,"I","Usina 192"),
new BaseField(769,772,"I","Usina 193"),
new BaseField(773,776,"I","Usina 194"),
new BaseField(777,780,"I","Usina 195"),
new BaseField(781,784,"I","Usina 196"),
new BaseField(785,788,"I","Usina 197"),
new BaseField(789,792,"I","Usina 198"),
new BaseField(793,796,"I","Usina 199"),
new BaseField(797,800,"I","Usina 200"),
new BaseField(801,804,"I","Usina 201"),
new BaseField(805,808,"I","Usina 202"),
new BaseField(809,812,"I","Usina 203"),
new BaseField(813,816,"I","Usina 204"),
new BaseField(817,820,"I","Usina 205"),
new BaseField(821,824,"I","Usina 206"),
new BaseField(825,828,"I","Usina 207"),
new BaseField(829,832,"I","Usina 208"),
new BaseField(833,836,"I","Usina 209"),
new BaseField(837,840,"I","Usina 210"),
new BaseField(841,844,"I","Usina 211"),
new BaseField(845,848,"I","Usina 212"),
new BaseField(849,852,"I","Usina 213"),
new BaseField(853,856,"I","Usina 214"),
new BaseField(857,860,"I","Usina 215"),
new BaseField(861,864,"I","Usina 216"),
new BaseField(865,868,"I","Usina 217"),
new BaseField(869,872,"I","Usina 218"),
new BaseField(873,876,"I","Usina 219"),
new BaseField(877,880,"I","Usina 220"),
new BaseField(881,884,"I","Usina 221"),
new BaseField(885,888,"I","Usina 222"),
new BaseField(889,892,"I","Usina 223"),
new BaseField(893,896,"I","Usina 224"),
new BaseField(897,900,"I","Usina 225"),
new BaseField(901,904,"I","Usina 226"),
new BaseField(905,908,"I","Usina 227"),
new BaseField(909,912,"I","Usina 228"),
new BaseField(913,916,"I","Usina 229"),
new BaseField(917,920,"I","Usina 230"),
new BaseField(921,924,"I","Usina 231"),
new BaseField(925,928,"I","Usina 232"),
new BaseField(929,932,"I","Usina 233"),
new BaseField(933,936,"I","Usina 234"),
new BaseField(937,940,"I","Usina 235"),
new BaseField(941,944,"I","Usina 236"),
new BaseField(945,948,"I","Usina 237"),
new BaseField(949,952,"I","Usina 238"),
new BaseField(953,956,"I","Usina 239"),
new BaseField(957,960,"I","Usina 240"),
new BaseField(961,964,"I","Usina 241"),
new BaseField(965,968,"I","Usina 242"),
new BaseField(969,972,"I","Usina 243"),
new BaseField(973,976,"I","Usina 244"),
new BaseField(977,980,"I","Usina 245"),
new BaseField(981,984,"I","Usina 246"),
new BaseField(985,988,"I","Usina 247"),
new BaseField(989,992,"I","Usina 248"),
new BaseField(993,996,"I","Usina 249"),
new BaseField(997,1000,"I","Usina 250"),
new BaseField(1001,1004,"I","Usina 251"),
new BaseField(1005,1008,"I","Usina 252"),
new BaseField(1009,1012,"I","Usina 253"),
new BaseField(1013,1016,"I","Usina 254"),
new BaseField(1017,1020,"I","Usina 255"),
new BaseField(1021,1024,"I","Usina 256"),
new BaseField(1025,1028,"I","Usina 257"),
new BaseField(1029,1032,"I","Usina 258"),
new BaseField(1033,1036,"I","Usina 259"),
new BaseField(1037,1040,"I","Usina 260"),
new BaseField(1041,1044,"I","Usina 261"),
new BaseField(1045,1048,"I","Usina 262"),
new BaseField(1049,1052,"I","Usina 263"),
new BaseField(1053,1056,"I","Usina 264"),
new BaseField(1057,1060,"I","Usina 265"),
new BaseField(1061,1064,"I","Usina 266"),
new BaseField(1065,1068,"I","Usina 267"),
new BaseField(1069,1072,"I","Usina 268"),
new BaseField(1073,1076,"I","Usina 269"),
new BaseField(1077,1080,"I","Usina 270"),
new BaseField(1081,1084,"I","Usina 271"),
new BaseField(1085,1088,"I","Usina 272"),
new BaseField(1089,1092,"I","Usina 273"),
new BaseField(1093,1096,"I","Usina 274"),
new BaseField(1097,1100,"I","Usina 275"),
new BaseField(1101,1104,"I","Usina 276"),
new BaseField(1105,1108,"I","Usina 277"),
new BaseField(1109,1112,"I","Usina 278"),
new BaseField(1113,1116,"I","Usina 279"),
new BaseField(1117,1120,"I","Usina 280"),
new BaseField(1121,1124,"I","Usina 281"),
new BaseField(1125,1128,"I","Usina 282"),
new BaseField(1129,1132,"I","Usina 283"),
new BaseField(1133,1136,"I","Usina 284"),
new BaseField(1137,1140,"I","Usina 285"),
new BaseField(1141,1144,"I","Usina 286"),
new BaseField(1145,1148,"I","Usina 287"),
new BaseField(1149,1152,"I","Usina 288"),
new BaseField(1153,1156,"I","Usina 289"),
new BaseField(1157,1160,"I","Usina 290"),
new BaseField(1161,1164,"I","Usina 291"),
new BaseField(1165,1168,"I","Usina 292"),
new BaseField(1169,1172,"I","Usina 293"),
new BaseField(1173,1176,"I","Usina 294"),
new BaseField(1177,1180,"I","Usina 295"),
new BaseField(1181,1184,"I","Usina 296"),
new BaseField(1185,1188,"I","Usina 297"),
new BaseField(1189,1192,"I","Usina 298"),
new BaseField(1193,1196,"I","Usina 299"),
new BaseField(1197,1200,"I","Usina 300"),
new BaseField(1201,1204,"I","Usina 301"),
new BaseField(1205,1208,"I","Usina 302"),
new BaseField(1209,1212,"I","Usina 303"),
new BaseField(1213,1216,"I","Usina 304"),
new BaseField(1217,1220,"I","Usina 305"),
new BaseField(1221,1224,"I","Usina 306"),
new BaseField(1225,1228,"I","Usina 307"),
new BaseField(1229,1232,"I","Usina 308"),
new BaseField(1233,1236,"I","Usina 309"),
new BaseField(1237,1240,"I","Usina 310"),
new BaseField(1241,1244,"I","Usina 311"),
new BaseField(1245,1248,"I","Usina 312"),
new BaseField(1249,1252,"I","Usina 313"),
new BaseField(1253,1256,"I","Usina 314"),
new BaseField(1257,1260,"I","Usina 315"),
new BaseField(1261,1264,"I","Usina 316"),
new BaseField(1265,1268,"I","Usina 317"),
new BaseField(1269,1272,"I","Usina 318"),
new BaseField(1273,1276,"I","Usina 319"),
new BaseField(1277,1280,"I","Usina 320"),

        };

        public override BaseField[] Campos {
            get {
                return _campos;
            }
        }
    }
}
