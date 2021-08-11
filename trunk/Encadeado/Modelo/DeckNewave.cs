using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Encadeado.Modelo {
    public class DeckNewave : Compass.CommomLibrary.Newave.Deck {

        public Encadeado.Estudo EstudoPai { get; set; }

        public List<Hidro> hidros = null;
        public List<Hidro> Hidros {
            get {
                if (hidros == null) LoadHidros();
                return hidros;
            }
        }

        private void LoadHidros() {
            

        }

        internal void EscreverListagemNwlistop(bool Todos = false) {

            using (var sw = System.IO.File.CreateText(
                    System.IO.Path.Combine(this.Folder, "nwlistop.dat")
                )) {

                sw.WriteLine(" 2");
                sw.WriteLine("FORWARD  (ARQ. DE DADOS)    : forward.dat");
                sw.WriteLine("FORWARDH (ARQ. CABECALHOS)  : forwarh.dat");
                sw.WriteLine("NEWDESP  (REL. CONFIGS)     : newdesp.dat");
                sw.WriteLine("-----------------------------------------");
                sw.WriteLine(" XXX XXX    PERIODOS INICIAL E FINAL");
                sw.WriteLine("   1 " + (60 - (this.Dger.MesEstudo + 1)).ToString());
                sw.WriteLine(" 1-CMO           2-DEFICITS         3-ENA LIQUIDA  4-EARM FINAL     5-FIO DAGUA");
                sw.WriteLine(" 6-EVAPORACAO    7-VERTIMENTO       8-VAZAO MIN.   9-GER.HIDR.CONT 10-GER. TERMICA");
                sw.WriteLine("11-INTERCAMBIOS 12-MERCADO LIQUIDO 13-VALOR AGUA  14-VOLUME MORTO  15-EXCESSO");
                sw.WriteLine("16-GHMAX        17-OUTROS USOS     18-BENEF. INT. 19-FAT. CORR. EC 20-GER.HID.TOTAL  21-ENA BRUTA   22-ACOPLAMENTO");
                sw.WriteLine();
                sw.WriteLine(" XX XX XX XX XX XX XX XX XX XX XX XX XX XX XX XX XX XX XX XX XX (SE 99 CONSIDERA TODAS)");
                if (Todos) {
                    sw.WriteLine(" 99");
                } else {
                    sw.WriteLine("  1  4 29");

                    //    codigos[decknwCMARG] = 1;
                    //    codigos[decknwDEFICIT] = 2;
                    //    codigos[decknwEAF] = 3;
                    //    codigos[decknwEARMF] = 4;
                    //    codigos[decknwGHFIO] = 5;
                    //    codigos[decknwEVAP] = 6;
                    //    codigos[decknwVERT] = 7;
                    //    codigos[decknwVAZMIN] = 8;
                    //    codigos[decknwGHIDRO] = 9;
                    //    codigos[decknwGTERT] = 10;
                    //    codigos[decknwINTER] = 11;
                    //    codigos[decknwMERCADO] = 12;
                    //    codigos[decknwVALORAGUA] = 13;
                    //    codigos[decknwVOLMORTO] = 14;
                    //    codigos[decknwEXCESSO] = 15;
                    //    codigos[decknwGHMAX] = 16;
                    //    codigos[decknwDESVIOCONT] = 17;
                    //    codigos[decknwFATORESCOR] = 19;
                    //    codigos[decknwGHTOT] = 20;
                    //    codigos[decknwEAFB] = 21;
                    //    codigos[decknwGFIOL] = 29;
                }
            }
        }
    }

    public class Hidro {
        

    }
}
