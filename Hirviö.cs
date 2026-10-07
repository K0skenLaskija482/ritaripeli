using System;

namespace ritaripeli
{
    /// <summary>
    /// Tämä on pohjaluokka kaikille pelin hirviöille
    /// </summary>
    internal abstract class Hirviö
    {
        protected static readonly Random rng = new Random();

        public int Osumapisteet { get; set; }
        public int MaxOsumapisteet { get; set; }
        public int KultaPalkkio { get; set; }
        public string Nimi { get; set; }

        public abstract int AnnaVahinko();
        public abstract void OtaVahinkoa(int määrä);
    }
}