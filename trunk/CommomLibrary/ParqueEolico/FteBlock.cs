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



    public class FteLine : BaseLine
    {
        public FteLine(string text)
        {
            var partes = text.Split(new string[] { ";" }, StringSplitOptions.None).ToList();
            BaseField[] campos = new BaseField[partes.Count()];
            for (int i = 0; i < campos.Count(); i++)
            {
                int startIndex = text.IndexOf(partes[i]) + 1;
                int range = partes[i].Length;
                int endIndex = startIndex + range - 1;
                campos[i] = new BaseField(startIndex, endIndex, "A" + range.ToString(), "");
            }
            CamposCSV = campos;

        }

    }
}
