using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.RenovaveisDcCSV
{
    public class PEEPOTINSTBlock : BaseBlock<PeePotInstLine>
    {
        public override string ToText()
        {

            var result = new StringBuilder();

            foreach (var item in this)
            {
                string linha = "";
                if (!string.IsNullOrWhiteSpace(item.Comment)) result.AppendLine(item.Comment);

                for (int i = 0; i < item.Valores.Length; i++)
                {

                    var campo = item.Valores[i];

                    //if (item.Valores[i] == item.Valores.Last())
                    if (i == (item.Valores.Length - 1))
                    {
                        linha = linha + campo;
                    }
                    else
                    {
                        linha = linha + campo + ";";
                    }
                }

                result.AppendLine(linha);
            }


            return result.ToString();

        }
    }

    public class PeePotInstLine : BaseLine
    {
        System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        public PeePotInstLine(string text)
        {
            LoadCamposCSV(text);

        }

        public string ID { get { return valores[CamposCSV[0]].ToString(); } set { valores[CamposCSV[0]] = value.ToString(); } }
        public int CodPEE { get { return Convert.ToInt32(valores[CamposCSV[1]]); } set { valores[CamposCSV[1]] = value.ToString(); } }
        public int EstIni { get { return Convert.ToInt32(valores[CamposCSV[2]]); } set { valores[CamposCSV[2]] = value.ToString(); } }
        public int EstFin { get { return Convert.ToInt32(valores[CamposCSV[3]]); } set { valores[CamposCSV[3]] = value.ToString(); } }
        public double PotInst { get { return Convert.ToDouble((string)valores[CamposCSV[4]], Culture.NumberFormat); } set { valores[CamposCSV[4]] = value.ToString(Culture.NumberFormat); } }

    }
}
