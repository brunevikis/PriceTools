using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Compass.CommomLibrary
{


    [DataContract]
    public class Propagacao
    {
        public bool OK { get; set; }
        public Propagacao()
        {
            VazaoIncremental = new Dictionary<DateTime, double>();
            VazaoNatural = new Dictionary<DateTime, double>();
            medSemanalNatural = new Dictionary<DateTime, double>();
            calMedSemanal = new Dictionary<DateTime, double>();
            medSemanalIncremental = new Dictionary<DateTime, double>();
            PostoMontantes = new List<PostoMontante>();
            Modelo = new List<ModeloSmap>();
            //PostoAcomph = new List<int>();
        }

        [DataMember]
        public string NomePostoFluv { get; set; }

        [DataMember]
        public int IdPosto { get; set; }

        [DataMember]
        public Dictionary<DateTime, double> medSemanalNatural { get; set; }

        [DataMember]
        public Dictionary<DateTime, double> calMedSemanal { get; set; }

        [DataMember]
        public Dictionary<DateTime, double> medSemanalIncremental { get; set; }

        [DataMember]
        public Dictionary<DateTime, double> VazaoIncremental { get; set; }

        [DataMember]
        public Dictionary<DateTime, double> VazaoNatural { get; set; }

        [DataMember]
        public List<PostoMontante> PostoMontantes { get; set; }

        [DataMember]
        public List<ModeloSmap> Modelo { get; set; }
    }
    public class PostoMontante
    {
        public Propagacao Propaga { get; set; }
        public double TempoViagem { get; set; }
    }

    public class ModeloSmap
    {
        public ModeloSmap()
        {
            FatorDistribuicao = 1;
        }
        public double FatorDistribuicao { get; set; }
        public string NomeVazao { get; set; }
        public double TempoViagem { get; set; }

    }
}
