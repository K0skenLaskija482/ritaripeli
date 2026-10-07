using System;
using System.Collections.Generic;

namespace ritaripeli
{
    internal class Reppu
    {
        private List<Tavara> _tavarat = new List<Tavara>();

        public IReadOnlyList<Tavara> Tavarat => _tavarat.AsReadOnly();

        public void Lisää(Tavara t)
        {
            if (t != null)
                _tavarat.Add(t);
        }

        /// <summary>
        /// Poistaa ja palauttaa tavaran 0-indeksoidusta paikasta.
        /// Käytä tätä yhdessä NäytäSisältö()-metodin kanssa vähentämällä
        /// käyttäjän syöttämästä numerosta yksi ennen kutsua.
        /// </summary>
        public Tavara Poista(int indeksi)
        {
            if (indeksi >= 0 && indeksi < _tavarat.Count)
            {
                Tavara t = _tavarat[indeksi];
                _tavarat.RemoveAt(indeksi);
                return t;
            }
            return null;
        }

        public void NäytäSisältö()
        {
            if (_tavarat.Count == 0)
            {
                Print.Line("Reppu on tyhjä.");
                return;
            }
            for (int i = 0; i < _tavarat.Count; i++)
            {
                Print.Line($"{i + 1}: {_tavarat[i].Nimi}");
            }
        }
    }
}