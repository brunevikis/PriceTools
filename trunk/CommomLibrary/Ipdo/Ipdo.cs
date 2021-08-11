using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.CommomLibrary.Ipdo {
    public class Ipdo : BaseDocument {

        Dictionary<string, IBlock<BaseLine>> blocos = new Dictionary<string, IBlock<BaseLine>>() {
                    {"Balanco"               , new BalancoBlock()},
                    {"Ger Termica"               , new GerTermicaBlock()},
                    {"Balanco Detalhado"               , new BalancoDetalhadoBlock()},
                    {"Energia Armazenada"               , new EnergiaBlock()},

                    
        };

        public override Dictionary<string, IBlock<BaseLine>> Blocos {
            get {
                return blocos;
            }
        }

        public override void Load(string fileContent) {
            ((BalancoBlock)Blocos["Balanco"]).Load(fileContent);
            ((BalancoDetalhadoBlock)Blocos["Balanco Detalhado"]).Load(fileContent);
            ((GerTermicaBlock)Blocos["Ger Termica"]).Load(fileContent);
            ((EnergiaBlock)Blocos["Energia Armazenada"]).Load(fileContent);
        }


        public BalancoDetalhadoBlock BalancoDetalhado { get { return ((BalancoDetalhadoBlock)Blocos["Balanco Detalhado"]); } }

        public EnergiaBlock Energia { get { return (EnergiaBlock)Blocos["Energia Armazenada"]; } }
    }
}
