using System;

namespace ritaripeli
{
	internal class Goblin : Hirviö
	{
		public Goblin()
		{
			Nimi = "Goblin";
			MaxOsumapisteet = 4;
			Osumapisteet = MaxOsumapisteet;
			KultaPalkkio = 5;
		}

		public override int AnnaVahinko()
		{
			return rng.Next(1, 3); // 1-2 vahinkoa
		}

		public override void OtaVahinkoa(int määrä)
		{
			Osumapisteet -= määrä;
			if (Osumapisteet < 0) Osumapisteet = 0;
		}
	}
}
