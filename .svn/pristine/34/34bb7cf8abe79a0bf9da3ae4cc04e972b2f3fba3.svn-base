using Compass.CommomLibrary.Decomp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compass.Services {
    public class DecompNextRev {

        public static void CreateNextRev(Compass.CommomLibrary.Decomp.Deck baseDeck, string outPath) {

            //avançar dadgnl
            var dadgnl = (Compass.CommomLibrary.Dadgnl.Dadgnl)
                Compass.CommomLibrary.DocumentFactory.Create(
                    baseDeck[DeckDocument.dadgnl].BasePath
                );

            //avançar dadger
            var dadger = (Compass.CommomLibrary.Dadger.Dadger)
                Compass.CommomLibrary.DocumentFactory.Create(
                    baseDeck[DeckDocument.dadger].BasePath
                );
            
            //gravar arquivos
            baseDeck.Rev++;
            baseDeck.Caso = "rv" + (baseDeck.Rev).ToString();

            dadgnl.File = System.IO.Path.Combine(outPath, "dadgnl." + baseDeck.Caso);
            dadger.File = System.IO.Path.Combine(outPath, "dadger." + baseDeck.Caso);

            dadgnl.SaveToFile();
            dadger.SaveToFile();

        }
    }
}
