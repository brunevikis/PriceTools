﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary
{
    public interface IRE
    {
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

        System.Collections.Generic.List<Tuple<int, int>> Intercambios { get; set; }
    }

    public interface IMODIF
    {
        int Usina { get; set; }

        int MesEstudo { get; set; }
        int Mes { get; set; }

        int Ano { get; set; }
        double Valor { get; set; }
        string Minemonico { get; set; }

        List<double> ModifCampos { get; set; }
    }

    public interface IREMODIF
    {
        int Usina { get; set; }

        int MesInicio { get; set; }

        int AnoInicio { get; set; }
        double Valor { get; set; }
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
    public interface ICURVA
    {
        double REE { get; set; }
        double Ano { get; set; }
        double MesEstudo { get; set; }

        double Mes { get; set; }

        double Porc { get; set; }
    }

    public interface IREEDAT
    {
        // int numREE { get; set; }
        int mesesAvan { get; set; }
        //int mesEst { get; set; }
    }

    public interface IADTERMDAD
    {
        double usina { get; set; }
        double ano { get; set; }
        double mes { get; set; }
        double estagio { get; set; }
        double PT1 { get; set; }
        double PT2 { get; set; }
        double PT3 { get; set; }
    }

    public interface IRESTELECSV
    {
        string Formula { get; set; }
        int MesEstudo { get; set; }
        //int Patamar { get; set; }
        DateTime DataIni { get; set; }
        DateTime DataFim { get; set; }
        double LimInfPt1 { get; set; }
        double LimSupPt1 { get; set; }
        double LimInfPt2 { get; set; }
        double LimSupPt2 { get; set; }
        double LimInfPt3 { get; set; }
        double LimSupPt3 { get; set; }
    }
}
