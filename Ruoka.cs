using System;

namespace ritaripeli
{
    internal class Ruoka : Tavara
    {
        public int Paranna { get; private set; }

        /// <summary>
        /// Luo nimetyn ruoka-annoksen, esim. new Ruoka("Leipäpala", 3).
        /// </summary>
        public Ruoka(string nimi, int paranna)
        {
            Paranna = paranna;
            Nimi = nimi;
        }

        /// <summary>
        /// Luo nimettömän ruoka-annoksen, jolle keksitään nimi parannusmäärän perusteella.
        /// </summary>
        public Ruoka(int paranna = 5)
        {
            Paranna = paranna;
            Nimi = $"Ruoka (+{Paranna} hp)";
        }
    }
}