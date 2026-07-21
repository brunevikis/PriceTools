using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.HidrDat
{
    public class HidrDat : BaseDocument
    {

        HidrBlock conteudo;
        //var conteudo;
        public override Dictionary<string, IBlock<BaseLine>> Blocos
        {
            get
            {
                return new Dictionary<string, IBlock<BaseLine>>() {
                    {"Hidr", conteudo}
                };
            }
        }

        public HidrLine this[int cod]
        {
            get { return conteudo[cod]; }
        }

        public HidrLine this[string nome]
        {
            get
            {
                return conteudo.Where(x => x.Usina.Trim().Equals(nome.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            }
        }

        public HidrBlock Data { get { return conteudo; } }

        public HidrDat()
        {
            conteudo = new HidrBlock();
        }

        public HidrDat(byte[] content, bool novo = false)
            : this()
        {
            if (novo == true)
            {
                HidrLine.Size = 832;
            }

            var regNum = content.Length / HidrLine.Size;



            for (int i = 0; i < regNum; i++)
            {

                var regBytes = content.Skip(HidrLine.Size * i).Take(HidrLine.Size).ToArray();


                HidrLine reg = new HidrLine(novo);

                reg[0] = i + 1;
                for (int c = 1; c < reg.Campos.Length; c++)
                {
                    dynamic val = reg.Campos[c].ExtractValue(regBytes);
                    reg[c] = val;
                }

                conteudo.Add(reg);
            }
        }

        public override void Load(string fileContent)
        {
            base.Load(fileContent);
        }

        public override void SaveToFile(string filePath = null, bool createBackup = false)
        {

            filePath = filePath ?? File;

            bool novo = false;

            if (System.IO.File.Exists(File))
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(File);
                double tamanho = fileInfo.Length / 792;
                if (tamanho > 320)
                {
                    novo = true;
                }
            }
            

            if (createBackup && System.IO.File.Exists(filePath))
            {
                var bkp = filePath + DateTime.Now.ToString("_yyyyMMddHHmmss.bak");
                System.IO.File.Copy(filePath, bkp);
            }

            var content = ToBytes(novo);
            System.IO.File.WriteAllBytes(filePath, content);

        }

        //public override void SaveToFile(bool createBackup = false) {


        //    if (createBackup && System.IO.File.Exists(File)) {
        //        var bkp = File + DateTime.Now.ToString("_yyyyMMddHHmmss.bak");
        //        System.IO.File.Copy(File, bkp);
        //    }

        //    var content = ToBytes();
        //    System.IO.File.WriteAllBytes(File, content);


        //}

        //public override void SaveToFile(string filePath) {
        //    var content = ToBytes();
        //    System.IO.File.WriteAllBytes(filePath, content);
        //}

        byte[] ToBytes(bool novo = false)
        {

            var result = new List<byte>();

            if (novo == true)
            {
                HidrLine.Size = 832;
            }
            foreach (var reg in conteudo)
            {
                var regBytes = new Byte[HidrLine.Size];



                for (int i = 0; i < reg.Campos.Length; i++)
                {
                    reg.Campos[i].InsertValue(regBytes, reg[i]);
                }

                result.InsertRange(result.Count, regBytes);
            }

            return result.ToArray();
        }


    }
}
