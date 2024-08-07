using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary
{
    public class CargasData
    {
        public DateTime Data { get; set; }
        public DateTime Revisao { get; set; }
        public double LOAD_sMMGD { get; set; }
        public double Base_CGH { get; set; }
        public double Base_EOL { get; set; }
        public double Base_UFV { get; set; }
        public double Base_UTE { get; set; }
        public double Base_MMGD { get; set; }
        public double LOAD_cMMGD { get; set; }
        public double Exp_CGH { get; set; }
        public double Exp_EOL { get; set; }
        public double Exp_UFV { get; set; }
        public double Exp_UTE { get; set; }
        public double Exp_MMGD { get; set; }
        public string Tipo { get; set; }
        public string Submercado { get; set; }

        public int SubNum
        {
            get
            {
                switch (Submercado)
                {
                    case "SUDESTE":
                        return 1;

                    case "SUL":
                        return 2;

                    case "NORDESTE":
                        return 3;

                    case "NORTE":
                        return 4;
                    default:
                        return 0;

                }

            }
        }
    }
}
