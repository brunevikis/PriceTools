using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.HidrDat {
    public class HidrBlock : BaseBlock<HidrLine> {
    }

    public class HidrLine : BaseLine {
        public static int Size = 792;
        public static BaseField[] _campos = new BaseField[] {
            new BaseField(  1, 1, "", "Cod"          ),
            new BaseField(1,12,"A","Usina"),
            new BaseField(25,28,"I","Sistema"),
            new BaseField(29,32,"I","Empresa"),
            new BaseField(13,16,"I","Posto"),
            new BaseField(17,24,"A","Posto BDH"),
            new BaseField(33,36,"I","Jusante"),
            new BaseField(37,40,"I","Desvio"),
            new BaseField(45,48,"F","Vol.Máx.(hm3)"),
            new BaseField(41,44,"F","Vol.min.(hm3)"),
            new BaseField(61,64,"F","Cota Máx.(m)"),
            new BaseField(57,60,"F","Cota min.(m)"),
            new BaseField(49,52,"F","Vol.Vert.(hm3)"),
            new BaseField(53,56,"F","Vol.Desv.(hm3)"),
            new BaseField(65,68,"F","PCV(0)"),
            new BaseField(69,72,"F","PCV(1)"),
            new BaseField(73,76,"F","PCV(2)"),
            new BaseField(77,80,"F","PCV(3)"),
            new BaseField(81,84,"F","PCV(4)"),
            new BaseField(85,88,"F","PAC(0)"),
            new BaseField(89,92,"F","PAC(1)"),
            new BaseField(93,96,"F","PAC(2)"),
            new BaseField(97,100,"F","PAC(3)"),
            new BaseField(101,104,"F","PAC(4)"),
            new BaseField(105,108,"I","Evap.Men.(1)"),
            new BaseField(109,112,"I","Evap.Men.(2)"),
            new BaseField(113,116,"I","Evap.Men.(3)"),
            new BaseField(117,120,"I","Evap.Men.(4)"),
            new BaseField(121,124,"I","Evap.Men.(5)"),
            new BaseField(125,128,"I","Evap.Men.(6)"),
            new BaseField(129,132,"I","Evap.Men.(7)"),
            new BaseField(133,136,"I","Evap.Men.(8)"),
            new BaseField(137,140,"I","Evap.Men.(9)"),
            new BaseField(141,144,"I","Evap.Men.(10)"),
            new BaseField(145,148,"I","Evap.Men.(11)"),
            new BaseField(149,152,"I","Evap.Men.(12)"),
            new BaseField(537,540,"F","Prod.Esp.(MW/m3/s/m)"),
            new BaseField(693,696,"F","Canal Fuga Médio(m)"),
            new BaseField(725,728,"F","TEIF(%)"),
            new BaseField(729,732,"F","IP(%)"),
            new BaseField(717,720,"I","Tipo Turbina"),
            new BaseField(153,156,"I","Num.Conj.Máq."),
            new BaseField(545,548,"I","Num.Pols.Jus."),
            new BaseField(701,704,"F","Fat.Carga Máx.(%)"),
            new BaseField(705,708,"F","Fat.Carga mín.(%)"),
            new BaseField(733,736,"I","Tipo Perdas(1=%/2=M/3=K)"),
            new BaseField(541,544,"F","Valor Perdas"),
            new BaseField(709,712,"I","Vazão Mín.Hist.(m3/s)"),
            new BaseField(713,716,"I","Num.Unid.Base"),
            new BaseField(697,700,"I","Infl.Vert.Canal de Fuga"),
            new BaseField(721,724,"I","Rep.Conj(0=aprox/1=det/2=simp)"),
            new BaseField(157,160,"I","#Maq(1)"),
            new BaseField(177,180,"F","PotEf(1)"),
            new BaseField(517,520,"I","QEf(1)"),
            new BaseField(497,500,"F","HEf(1)"),
            new BaseField(161,164,"I","#Maq(2)"),
            new BaseField(181,184,"F","PotEf(2)"),
            new BaseField(521,524,"I","QEf(2)"),
            new BaseField(501,504,"F","HEf(2)"),
            new BaseField(165,168,"I","#Maq(3)"),
            new BaseField(185,188,"F","PotEf(3)"),
            new BaseField(525,528,"I","QEf(3)"),
            new BaseField(505,508,"F","HEf(3)"),
            new BaseField(169,172,"I","#Maq(4)"),
            new BaseField(189,192,"F","PotEf(4)"),
            new BaseField(529,532,"I","QEf(4)"),
            new BaseField(509,512,"F","HEf(4)"),
            new BaseField(173,176,"I","#Maq(5)"),
            new BaseField(193,196,"F","PotEf(5)"),
            new BaseField(533,536,"I","QEf(5)"),
            new BaseField(513,516,"F","HEf(5)"),
            new BaseField(197,200,"F","QHTA0(1)"),
            new BaseField(201,204,"F","QHTA1(1)"),
            new BaseField(205,208,"F","QHTA2(1)"),
            new BaseField(209,212,"F","QHTA3(1)"),
            new BaseField(213,216,"F","QHTA4(1)"),
            new BaseField(217,220,"F","QHTA0(2)"),
            new BaseField(221,224,"F","QHTA1(2)"),
            new BaseField(225,228,"F","QHTA2(2)"),
            new BaseField(229,232,"F","QHTA3(2)"),
            new BaseField(233,236,"F","QHTA4(2)"),
            new BaseField(237,240,"F","QHTA0(3)"),
            new BaseField(241,244,"F","QHTA1(3)"),
            new BaseField(245,248,"F","QHTA2(3)"),
            new BaseField(249,252,"F","QHTA3(3)"),
            new BaseField(253,256,"F","QHTA4(3)"),
            new BaseField(257,260,"F","QHTA0(4)"),
            new BaseField(261,264,"F","QHTA1(4)"),
            new BaseField(265,268,"F","QHTA2(4)"),
            new BaseField(269,272,"F","QHTA3(4)"),
            new BaseField(273,276,"F","QHTA4(4)"),
            new BaseField(277,280,"F","QHTA0(5)"),
            new BaseField(281,284,"F","QHTA1(5)"),
            new BaseField(285,288,"F","QHTA2(5)"),
            new BaseField(289,292,"F","QHTA3(5)"),
            new BaseField(293,296,"F","QHTA4(5)"),
            new BaseField(297,300,"F","QHGA0(1)"),
            new BaseField(301,304,"F","QHGA1(1)"),
            new BaseField(305,308,"F","QHGA2(1)"),
            new BaseField(309,312,"F","QHGA3(1)"),
            new BaseField(313,316,"F","QHGA4(1)"),
            new BaseField(317,320,"F","QHGA0(2)"),
            new BaseField(321,324,"F","QHGA1(2)"),
            new BaseField(325,328,"F","QHGA2(2)"),
            new BaseField(329,332,"F","QHGA3(2)"),
            new BaseField(333,336,"F","QHGA4(2)"),
            new BaseField(337,340,"F","QHGA0(3)"),
            new BaseField(341,344,"F","QHGA1(3)"),
            new BaseField(345,348,"F","QHGA2(3)"),
            new BaseField(349,352,"F","QHGA3(3)"),
            new BaseField(353,356,"F","QHGA4(3)"),
            new BaseField(357,360,"F","QHGA0(4)"),
            new BaseField(361,364,"F","QHGA1(4)"),
            new BaseField(365,368,"F","QHGA2(4)"),
            new BaseField(369,372,"F","QHGA3(4)"),
            new BaseField(373,376,"F","QHGA4(4)"),
            new BaseField(377,380,"F","QHGA0(5)"),
            new BaseField(381,384,"F","QHGA1(5)"),
            new BaseField(385,388,"F","QHGA2(5)"),
            new BaseField(389,392,"F","QHGA3(5)"),
            new BaseField(393,396,"F","QHGA4(5)"),
            new BaseField(397,400,"F","PHA0(1)"),
            new BaseField(401,404,"F","PHA1(1)"),
            new BaseField(405,408,"F","PHA2(1)"),
            new BaseField(409,412,"F","PHA3(1)"),
            new BaseField(413,416,"F","PHA4(1)"),
            new BaseField(417,420,"F","PHA0(2)"),
            new BaseField(421,424,"F","PHA1(2)"),
            new BaseField(425,428,"F","PHA2(2)"),
            new BaseField(429,432,"F","PHA3(2)"),
            new BaseField(433,436,"F","PHA4(2)"),
            new BaseField(437,440,"F","PHA0(3)"),
            new BaseField(441,444,"F","PHA1(3)"),
            new BaseField(445,448,"F","PHA2(3)"),
            new BaseField(449,452,"F","PHA3(3)"),
            new BaseField(453,456,"F","PHA4(3)"),
            new BaseField(457,460,"F","PHA0(4)"),
            new BaseField(461,464,"F","PHA1(4)"),
            new BaseField(465,468,"F","PHA2(4)"),
            new BaseField(469,472,"F","PHA3(4)"),
            new BaseField(473,476,"F","PHA4(4)"),
            new BaseField(493,496,"F","PHA4(4)"),
            new BaseField(477,480,"F","PHA0(5)"),
            new BaseField(481,484,"F","PHA1(5)"),
            new BaseField(485,488,"F","PHA2(5)"),
            new BaseField(489,492,"F","PHA3(5)"),
            new BaseField(549,552,"F","PJA0(1)"),
            new BaseField(553,556,"F","PJA1(1)"),
            new BaseField(557,560,"F","PJA2(1)"),
            new BaseField(561,564,"F","PJA3(1)"),
            new BaseField(565,568,"F","PJA4(1)"),
            new BaseField(669,672,"F","PJRM(1)"),
            new BaseField(569,572,"F","PJA0(2)"),
            new BaseField(573,576,"F","PJA1(2)"),
            new BaseField(577,580,"F","PJA2(2)"),
            new BaseField(581,584,"F","PJA3(2)"),
            new BaseField(585,588,"F","PJA4(2)"),
            new BaseField(673,676,"F","PJRM(2)"),
            new BaseField(589,592,"F","PJA0(3)"),
            new BaseField(593,596,"F","PJA1(3)"),
            new BaseField(597,600,"F","PJA2(3)"),
            new BaseField(601,604,"F","PJA3(3)"),
            new BaseField(605,608,"F","PJA4(3)"),
            new BaseField(677,680,"F","PJRM(3)"),
            new BaseField(609,612,"F","PJA0(4)"),
            new BaseField(613,616,"F","PJA1(4)"),
            new BaseField(617,620,"F","PJA2(4)"),
            new BaseField(621,624,"F","PJA3(4)"),
            new BaseField(625,628,"F","PJA4(4)"),
            new BaseField(681,684,"F","PJRM(4)"),
            new BaseField(629,632,"F","PJA0(5)"),
            new BaseField(633,636,"F","PJA1(5)"),
            new BaseField(637,640,"F","PJA2(5)"),
            new BaseField(641,644,"F","PJA3(5)"),
            new BaseField(645,648,"F","PJA4(5)"),
            new BaseField(685,688,"F","PJRM(5)"),
            new BaseField(737,744,"A","Data"),
            new BaseField(745,787,"A","Obs"),
            new BaseField(788,791,"F","Vol.Ref."),
            new BaseField(792,792,"A","Reg"),



        };

        public override BaseField[] Campos {
            get {
                return _campos;
            }
        }

        public int Cod { get { return this[0]; } }
        public float VolMax { get { return this["Vol.Máx.(hm3)"]; } set { this["Vol.Máx.(hm3)"] = value; } }
        public float VolMin { get { return this["Vol.min.(hm3)"]; } set { this["Vol.min.(hm3)"] = value; } }
        public float VolRef { get { return this["Vol.Ref."]; } set { this["Vol.Ref."] = value; } }
        public int Jusante { get { return this["Jusante"]; } set { this["Jusante"] = value; } }
        public int Posto { get { return this["Posto"]; } set { this["Posto"] = value; } }

        
        public float CanalFugaMed { get { return this["Canal Fuga Médio(m)"]; } set { this["Canal Fuga Médio(m)"] = value; } }
        public float ProdEsp { get { return this["Prod.Esp.(MW/m3/s/m)"]; } set { this["Prod.Esp.(MW/m3/s/m)"] = value; } }
        public float PerdaVal { get { return this["Valor Perdas"]; } set { this["Valor Perdas"] = value; } }
        public int PerdaTipo { get { return this["Tipo Perdas(1=%/2=M/3=K)"]; } set { this["Tipo Perdas(1=%/2=M/3=K)"] = value; } }
        public string Reg { get { return this["Reg"]; } set { this["Reg"] = value; } }

        public float CotaMax { get { return this["Cota Máx.(m)"]; } set { this["Cota Máx.(m)"] = value; } }
        public float CotaMin { get { return this["Cota min.(m)"]; } set { this["Cota min.(m)"] = value; } }

        public float PCV0 { get { return this["PCV(0)"]; } set { this["PCV(0)"] = value; } }
        public float PCV1 { get { return this["PCV(1)"]; } set { this["PCV(1)"] = value; } }
        public float PCV2 { get { return this["PCV(2)"]; } set { this["PCV(2)"] = value; } }
        public float PCV3 { get { return this["PCV(3)"]; } set { this["PCV(3)"] = value; } }
        public float PCV4 { get { return this["PCV(4)"]; } set { this["PCV(4)"] = value; } }

        public int Sistema { get { return this["Sistema"]; } set { this["Sistema"] = value; } }

        public string Usina { get { return this["Usina"]; } set { this["Usina"] = value; } }

        public int NumUnidadesBase { get { return this["Num.Unid.Base"]; } set { this["Num.Unid.Base"] = value; } }
        

        
    }
}
