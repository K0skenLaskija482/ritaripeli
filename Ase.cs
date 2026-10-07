using System;

namespace ritaripeli
{
	/// <summary>
	/// Ase on tavara jota ritari pitää kädessään taistelun aikana.
	/// Eri aseet aiheuttavat eri määrän vahinkoa hyökätessä.
	/// </summary>
	internal class Ase : Tavara
	{
		public int Vahinko { get; private set; }

		public Ase(string nimi, int vahinko)
		{
			Nimi = nimi;
			Vahinko = vahinko;
		}
	}
}
