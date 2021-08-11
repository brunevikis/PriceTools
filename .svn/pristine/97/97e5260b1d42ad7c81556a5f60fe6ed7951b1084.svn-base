using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary {
    public interface IRE {
        int AnoFim { get; set; }
        int AnoIni { get; set; }
        int MesEstudo { get; set; }
        int MesFim { get; set; }
        int MesIni { get; set; }
        int Patamar { get; set; }
        double Restricao { get; set; }
        System.Collections.Generic.List<int> Usinas { get; set; }
    }

    public interface IADTERM
    {
        System.Collections.Generic.List<int> Usinas { get; set; }
        int Mes { get; set; }
        double Usina { get; set; }
        double RestricaoP1 { get; set; }
        double RestricaoP2 { get; set; }
        double RestricaoP3 { get; set; }

    }


    public interface IAGRIGNT
    {
        int AnoFim { get; set; }
        int AnoIni { get; set; }
        int MesEstudo { get; set; }
        int MesFim { get; set; }
        int MesIni { get; set; }
        
        double RestricaoP1 { get; set; }
        double RestricaoP2 { get; set; }
        double RestricaoP3 { get; set; }

        System.Collections.Generic.List<Tuple<int,int>> Intercambios { get; set; }
    }

    public interface IINTERCAMBIO
    {
        int AnoFim { get; set; }
        int AnoIni { get; set; }
        int MesEstudo { get; set; }
        int MesFim { get; set; }
        int MesIni { get; set; }

        double RestricaoP1 { get; set; }
        double RestricaoP2 { get; set; }
        double RestricaoP3 { get; set; }

        Tuple<int, int> Intercambios { get; set; }
    }

    public interface IMERCADO
    {
        double SubMercado { get; set; }
        double AnoIni { get; set; }
        double MesEstudo { get; set; }

        double Mes { get; set; }

        double Carga { get; set; }      

       
    }
}
