using System;

namespace ritaripeli
{
	/// <summary>
	/// Nuoli on tavara jonka ritari voi ampua taistelussa.
	/// Nuoli kuluu käytössä eli poistuu repusta ampumisen jälkeen.
	/// </summary>
	internal class Nuoli : Tavara
	{
		public int Vahinko { get; private set; }

		public Nuoli(string nimi, int vahinko)
		{
			Nimi = nimi;
			Vahinko = vahinko;
		}
	}
}
