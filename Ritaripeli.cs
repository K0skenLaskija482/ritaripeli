using System;
using System.Collections.Generic;

namespace ritaripeli
{
    internal class Ritaripeli
    {
        // Kuinka moni hirviö pitää voittaa TAI kuinka paljon kultaa pitää kerätä,
        // jotta peli päättyy voittoon.
        private const int TavoiteVoitot = 5;
        private const int TavoiteKulta = 50;

        Ritari pelaaja;
        List<IKauppa> kaupat;
        List<Func<Hirviö>> hirviöTehtaat;
        Random satunnais = new Random();
        int voitetutHirviot = 0;

        public Ritaripeli()
        {
            pelaaja = new Ritari(aloitusOsumapisteet: 10, aloitusRahat: 10);

            // kaupat[0] = asekauppa, kaupat[1] = ravintola (katso PeliSilmukka)
            kaupat = new List<IKauppa>()
            {
                new Asekauppa(),
                new Ravintola()
            };

            // erilaiset hirviöt joita voidaan arpoa taisteluun
            hirviöTehtaat = new List<Func<Hirviö>>()
            {
                () => new Goblin(),
                () => new Rosvo(),
                () => new Velho()
            };
        }

        public void PeliSilmukka()
        {
            Print.Line("Tervetuloa suureen seikkailuun!");

            bool peliKäynnissä = true;
            while (peliKäynnissä)
            {
                NäytäTilanne(pelaaja);
                Print.Line("Valitse toiminto:");
                Print.Line("1 Mene asekauppaan");
                Print.Line("2 Mene ravintolaan");
                Print.Line("3 Lähde taisteluun");
                Print.Line("4 Käytä repussa olevia esineitä");
                Print.Line("5 Lopeta peli");
                Print.Write("> ");
                string valinta = Console.ReadLine();

                switch (valinta)
                {
                    case "1":
                        kaupat[0].Toimi(pelaaja);
                        break;
                    case "2":
                        kaupat[1].Toimi(pelaaja);
                        break;
                    case "3":
                        TaisteluTila();
                        break;
                    case "4":
                        KäytäEsineValikko();
                        break;
                    case "5":
                        Print.Line("Suljetaan peli. Näkemiin!");
                        peliKäynnissä = false;
                        break;
                    default:
                        Print.Line("Tuntematon valinta. Yritä uudelleen.");
                        break;
                }

                // Tarkista onko peli päättynyt
                if (pelaaja.Osumapisteet <= 0)
                {
                    Print.LineColor("Kaaduit seikkailussa. Peli päättyy.", ConsoleColor.Red);
                    peliKäynnissä = false;
                }
                else if (voitetutHirviot >= TavoiteVoitot || pelaaja.Rahapussi.Rahoja >= TavoiteKulta)
                {
                    Print.LineColor(
                        $"Olet voittanut {voitetutHirviot} hirviötä ja kerännyt {pelaaja.Rahapussi.Rahoja} kultarahaa.",
                        ConsoleColor.Green);
                    Print.LineColor("Nimesi jää historiaan legendaarisena ritarina. Peli päättyy voittoon!", ConsoleColor.Green);
                    peliKäynnissä = false;
                }
            }

            Print.Line("Kiitos kun pelasit!");
        }

        public void TaisteluTila()
        {
            Hirviö vastustaja = ArvoHirviö();
            Print.Line($"Kohtaat hirmuisen {vastustaja.Nimi} hirviön!");

            bool paossa = false;
            while (vastustaja.Osumapisteet > 0 && pelaaja.Osumapisteet > 0 && !paossa)
            {
                NäytäTaisteluTilanne(vastustaja);
                Print.Line("1. Hyökkää");
                Print.Line("2. Käytä esinettä");
                Print.Line("3. Pakene");
                Print.Write("> ");
                string valinta = Console.ReadLine();

                bool vuoroKului = true;

                switch (valinta)
                {
                    case "1":
                        int vahinko = pelaaja.AseKädessä.Vahinko;
                        vastustaja.OtaVahinkoa(vahinko);
                        Print.Line($"{pelaaja.AseKädessä.Nimi} aiheutti vastustajalle {vahinko} vahinkoa");
                        break;

                    case "2":
                        vuoroKului = KäytäEsineTaistelussa(vastustaja);
                        break;

                    case "3":
                        Print.Line("Pakenit taistelusta!");
                        paossa = true;
                        vuoroKului = false;
                        break;

                    default:
                        Print.Line("Tuntematon valinta.");
                        vuoroKului = false;
                        break;
                }

                if (paossa)
                    break;

                // Jos hirviöllä on osumapisteitä jäljellä, se hyökkää takaisin
                if (vuoroKului && vastustaja.Osumapisteet > 0)
                {
                    int hirviönVahinko = vastustaja.AnnaVahinko();
                    pelaaja.OtaVahinkoa(hirviönVahinko);
                    Print.Line($"{vastustaja.Nimi} lyö sinua ja aiheuttaa {hirviönVahinko} vahinkoa");
                }
            }

            if (paossa)
            {
                return;
            }

            if (pelaaja.Osumapisteet <= 0)
            {
                Print.LineColor("Kaadut taistelussa...", ConsoleColor.Red);
                return;
            }

            if (vastustaja.Osumapisteet <= 0)
            {
                Print.LineColor("Voitat hirviön!", ConsoleColor.Green);
                Print.LineColor($"Saat {vastustaja.KultaPalkkio} kultarahaa", ConsoleColor.Yellow);
                pelaaja.Rahapussi.LisääRahaa(vastustaja.KultaPalkkio);
                voitetutHirviot++;
            }
            // Kun taistelu loppuu, palataan PeliSilmukkaan
        }

        /// <summary>
        /// Näyttää repun sisällön ja käyttää valitun esineen taistelun aikana.
        /// Palauttaa true jos toiminto kulutti pelaajan vuoron (jolloin hirviö
        /// pääsee hyökkäämään takaisin), false jos vuoro ei kulunut.
        /// </summary>
        private bool KäytäEsineTaistelussa(Hirviö vastustaja)
        {
            if (pelaaja.Reppu.Tavarat.Count == 0)
            {
                Print.Line("Reppu on tyhjä!");
                return false;
            }

            Print.Line("Valitse esine");
            pelaaja.Reppu.NäytäSisältö();
            Print.Write("> ");

            if (!int.TryParse(Console.ReadLine(), out int valinta))
            {
                Print.Line("Tuntematon valinta.");
                return false;
            }

            Tavara tavara = pelaaja.Reppu.Poista(valinta - 1);
            if (tavara == null)
            {
                Print.Line("Sellaista esinettä ei ole.");
                return false;
            }

            switch (tavara)
            {
                case Nuoli nuoli:
                    vastustaja.OtaVahinkoa(nuoli.Vahinko);
                    Print.Line($"{nuoli.Nimi} aiheutti vastustajalle {nuoli.Vahinko} vahinkoa");
                    return true;

                case Ruoka ruoka:
                    pelaaja.Paranna(ruoka.Paranna);
                    Print.Line($"Söit annoksen {ruoka.Nimi} ja sait {ruoka.Paranna} osumapistettä lisää");
                    return true;

                case Ase ase:
                    pelaaja.VaihdaAse(ase);
                    Print.Line($"Vaihdoit aseeksi {ase.Nimi}");
                    return true;

                default:
                    pelaaja.Reppu.Lisää(tavara);
                    Print.Line("Et voi käyttää tätä esinettä nyt.");
                    return false;
            }
        }

        /// <summary>
        /// Näyttää repun sisällön ja käyttää valitun esineen päävalikosta
        /// (taistelun ulkopuolella). Nuolia ei voi käyttää täällä.
        /// </summary>
        private void KäytäEsineValikko()
        {
            if (pelaaja.Reppu.Tavarat.Count == 0)
            {
                Print.Line("Reppu on tyhjä!");
                return;
            }

            Print.Line("Valitse esine");
            pelaaja.Reppu.NäytäSisältö();
            Print.Write("> ");

            if (!int.TryParse(Console.ReadLine(), out int valinta))
            {
                Print.Line("Tuntematon valinta.");
                return;
            }

            Tavara tavara = pelaaja.Reppu.Poista(valinta - 1);
            if (tavara == null)
            {
                Print.Line("Sellaista esinettä ei ole.");
                return;
            }

            switch (tavara)
            {
                case Ruoka ruoka:
                    pelaaja.Paranna(ruoka.Paranna);
                    Print.Line($"Söit annoksen {ruoka.Nimi} ja sait {ruoka.Paranna} osumapistettä lisää");
                    break;

                case Ase ase:
                    pelaaja.VaihdaAse(ase);
                    Print.Line($"Vaihdoit aseeksi {ase.Nimi}");
                    break;

                case Nuoli nuoli:
                    Print.Line($"{nuoli.Nimi} kannattaa säästää taisteluun. Laitoit sen takaisin reppuun.");
                    pelaaja.Reppu.Lisää(nuoli);
                    break;
            }
        }

        private Hirviö ArvoHirviö()
        {
            int valinta = satunnais.Next(hirviöTehtaat.Count);
            return hirviöTehtaat[valinta]();
        }

        private void NäytäTaisteluTilanne(Hirviö vastustaja)
        {
            Print.WriteColor("Oma op ", ConsoleColor.White);
            Print.WriteColor($"({pelaaja.Osumapisteet}/{pelaaja.MaxOsumapisteet}) ", ConsoleColor.Green);
            Print.WriteColor("Vihollinen ", ConsoleColor.White);
            Print.LineColor($"({vastustaja.Osumapisteet}/{vastustaja.MaxOsumapisteet})", ConsoleColor.Red);
        }

        /// <summary>
        /// Näyttää pelaajan tilanteen. Julkinen ja staattinen, jotta myös
        /// kauppaluokat (esim. Asekauppa, Ravintola) voivat näyttää saman rivin.
        /// </summary>
        public static void NäytäTilanne(Ritari pelaaja)
        {
            Print.WriteColor("Tilanne: ", ConsoleColor.White);
            Print.WriteColor("Sinulla on ", ConsoleColor.White);
            Print.WriteColor($"{pelaaja.Osumapisteet}", ConsoleColor.Green);
            Print.WriteColor(" osumapistettä ja ", ConsoleColor.White);
            Print.WriteColor($"{pelaaja.Rahapussi.Rahoja}", ConsoleColor.Yellow);
            Print.LineColor(" kultarahaa.", ConsoleColor.White);
        }
    }
}
