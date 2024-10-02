using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class FteBlock : BaseBlock<FteLine>
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



    public class FteLine : BaseLine
    {
        System.Globalization.CultureInfo Culture = System.Globalization.CultureInfo.InvariantCulture;

        public FteLine(string text)
        {
            LoadCamposCSV(text);

            //var partes = text.Split(new string[] { ";" }, StringSplitOptions.None).ToList();
            //BaseField[] campos = new BaseField[partes.Count()];
            //for (int i = 0; i < campos.Count(); i++)
            //{
            //    int startIndex = text.IndexOf(partes[i]) + 1;
            //    int range = partes[i].Length;
            //    int endIndex = startIndex + range - 1;
            //    campos[i] = new BaseField(startIndex, endIndex, "A" + range.ToString(), "");
            //}
            //CamposCSV = campos;

        }
        public string ID { get { return valores[CamposCSV[0]].ToString(); } set { valores[CamposCSV[0]] = value.ToString(); } }
        public int CodPEE { get { return Convert.ToInt32(valores[CamposCSV[1]]); } set { valores[CamposCSV[1]] = value.ToString(); } }
        public int PeriodoIni { get { return Convert.ToInt32(valores[CamposCSV[2]]); } set { valores[CamposCSV[2]] = value.ToString(); } }
        public string PeriodoFim { get { return valores[CamposCSV[3]].ToString(); } set { valores[CamposCSV[3]] = value.ToString(); } }
        public double A0 { get { return Convert.ToDouble((string)valores[CamposCSV[4]],Culture.NumberFormat); } set { valores[CamposCSV[4]] = value.ToString(Culture.NumberFormat); } }
        public double A1 { get { return Convert.ToDouble((string)valores[CamposCSV[5]], Culture.NumberFormat); } set { valores[CamposCSV[5]] = value.ToString(Culture.NumberFormat); } }

    }
}
