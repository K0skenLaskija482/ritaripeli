using System;
using System.Collections.Generic;

namespace ritaripeli
{
    /// <summary>
    /// Ravintolasta voi ostaa erikokoisia ruoka-annoksia,
    /// jotka palauttavat ritarin osumapisteitä syötäessä.
    /// </summary>
    internal class Ravintola : IKauppa
    {
        private List<TavaraJaHinta> ruokalista;

        public Ravintola()
        {
            ruokalista = new List<TavaraJaHinta>
            {
                new TavaraJaHinta(new Ruoka("Leipäpala", 3), 2),
                new TavaraJaHinta(new Ruoka("Liha-annos", 6), 5),
                new TavaraJaHinta(new Ruoka("Kalapiirakka", 10), 8),
            };
        }

        public List<TavaraJaHinta> ListaaTavarat()
        {
            return ruokalista;
        }

        public Tavara? OstaTavara(int valittuTavara, Lompakko rahapussi)
        {
            if (valittuTavara < 0 || valittuTavara >= ruokalista.Count)
                return null;

            TavaraJaHinta valinta = ruokalista[valittuTavara];
            if (rahapussi.OtaRahaa(valinta.Hinta) > 0)
            {
                Ruoka alkuperäinen = (Ruoka)valinta.Esine;
                return new Ruoka(alkuperäinen.Nimi, alkuperäinen.Paranna);
            }
            return null;
        }

        public void Toimi(Ritari pelaaja)
        {
            Print.Line("Tervetuloa ravintolaan!");
            bool poistu = false;
            while (!poistu)
            {
                Ritaripeli.NäytäTilanne(pelaaja);
                Print.Line("Valitse toiminto:");
                Print.Line("1 Listaa ruokalista");
                Print.Line("2 Osta ruokaa");
                Print.Line("3 Poistu");
                Print.Write("> ");
                string valinta = Console.ReadLine();

                switch (valinta)
                {
                    case "1":
                        NäytäRuokalista();
                        break;
                    case "2":
                        OstaRuokaa(pelaaja);
                        break;
                    case "3":
                        poistu = true;
                        break;
                    default:
                        Print.Line("Tuntematon valinta.");
                        break;
                }
            }
        }

        private void NäytäRuokalista()
        {
            Print.Line("Ravintolan ruokalista:");
            for (int i = 0; i < ruokalista.Count; i++)
            {
                Ruoka ruoka = (Ruoka)ruokalista[i].Esine;
                Print.Line($"{i + 1}: {ruoka.Nimi,-14} (+{ruoka.Paranna} hp) {ruokalista[i].Hinta} kr");
            }
        }

        private void OstaRuokaa(Ritari pelaaja)
        {
            Print.Line("Minkä ruoka-annoksen haluat ostaa?");
            NäytäRuokalista();
            Print.Write("> ");

            if (!int.TryParse(Console.ReadLine(), out int valinta))
            {
                Print.Line("Tuntematon valinta.");
                return;
            }

            int indeksi = valinta - 1;
            if (indeksi < 0 || indeksi >= ruokalista.Count)
            {
                Print.Line("Sellaista annosta ei ole.");
                return;
            }

            int hinta = ruokalista[indeksi].Hinta;
            Tavara? ostettu = OstaTavara(indeksi, pelaaja.Rahapussi);
            if (ostettu != null)
            {
                pelaaja.Reppu.Lisää(ostettu);
                Print.Line($"Ostit tavaran {ostettu.Nimi}. Käytit {hinta} kultarahaa.");
            }
            else
            {
                Print.Line("Sinulla ei ole tarpeeksi kultarahaa.");
            }
        }
    }
}