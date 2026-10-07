using System;

namespace ritaripeli
{
    internal class Ritari
    {
        public int Osumapisteet { get; private set; }
        public int MaxOsumapisteet { get; private set; }
        public Reppu Reppu { get; private set; }
        public Lompakko Rahapussi { get; private set; }
        public Ase AseKädessä { get; private set; }

        public Ritari(int aloitusOsumapisteet, int aloitusRahat)
        {
            Osumapisteet = aloitusOsumapisteet;
            MaxOsumapisteet = aloitusOsumapisteet;
            Rahapussi = new Lompakko(aloitusRahat);
            Reppu = new Reppu();

            // Ritarilla on alussa vain miekka kädessään
            AseKädessä = new Ase("Miekka", 3);
        }

        public void OtaVahinkoa(int määrä)
        {
            Osumapisteet -= määrä;
            if (Osumapisteet < 0) Osumapisteet = 0;
        }

        public void Paranna(int maara)
        {
            Osumapisteet += maara;
            if (Osumapisteet > MaxOsumapisteet) Osumapisteet = MaxOsumapisteet;
        }

        /// <summary>
        /// Vaihtaa kädessä olevan aseen. Vanha ase ei katoa,
        /// vaan palautuu reppuun talteen.
        /// </summary>
        public void VaihdaAse(Ase uusiAse)
        {
            if (uusiAse == null) return;

            if (AseKädessä != null)
            {
                Reppu.Lisää(AseKädessä);
            }
            AseKädessä = uusiAse;
        }
    }
}