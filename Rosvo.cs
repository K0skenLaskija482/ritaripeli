using System;

namespace ritaripeli
{
	internal class Rosvo : Hirviö
	{
		public Rosvo()
		{
			Nimi = "Rosvo";
			MaxOsumapisteet = 6;
			Osumapisteet = MaxOsumapisteet;
			KultaPalkkio = 8;
		}

		public override int AnnaVahinko()
		{
			return rng.Next(1, 4); // 1-3 vahinkoa
		}

		public override void OtaVahinkoa(int määrä)
		{
			Osumapisteet -= määrä;
			if (Osumapisteet < 0) Osumapisteet = 0;
		}
	}
}
