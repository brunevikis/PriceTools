using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.ParqueEolico
{
    public class IndicesGevazpBlock : BaseBlock<IndicesGevazpLine>
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

                    if (item.Valores[i] == item.Valores.Last())
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



    public class IndicesGevazpLine : BaseLine
    {
        public IndicesGevazpLine(string text)
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
        public string Funcao { get { return valores[CamposCSV[0]].ToString(); } set { valores[CamposCSV[0]] = value.ToString(); } }

        public string Desc { get { return valores[CamposCSV[1]].ToString(); } set { valores[CamposCSV[1]] = value.ToString(); } }

        public string Arquivo { get { return valores[CamposCSV[2]].ToString(); } set { valores[CamposCSV[2]] = value.ToString(); } }
    }
}
