using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary
{
    public class WeolSM
    {
        public DateTime SemanaIni { get; set; }
        public DateTime SemanaFim { get; set; }
        public double CargaPat1 { get; set; }
        public double CargaPat2 { get; set; }
        public double CargaPat3 { get; set; }
      
        public string Submercado { get; set; }

        public int SubNum
        {
            get
            {
                switch (Submercado.ToUpper())
                {
                    case "SUDESTE":
                    case "SE":
                        return 1;

                    case "SUL":
                    case "S":
                        return 2;

                    case "NORDESTE":
                    case "NE":
                        return 3;

                    case "NORTE":
                    case "N":
                        return 4;
                    default:
                        return 0;

                }

            }
        }
    }
}
