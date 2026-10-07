using System;

namespace ritaripeli
{
	internal class Velho : Hirviö
	{
		public Velho()
		{
			Nimi = "Velho";
			MaxOsumapisteet = 7;
			Osumapisteet = MaxOsumapisteet;
			KultaPalkkio = 14;
		}

		public override int AnnaVahinko()
		{
			return rng.Next(3, 7); // 3-6 vahinkoa, voimakas loitsu
		}

		public override void OtaVahinkoa(int määrä)
		{
			Osumapisteet -= määrä;
			if (Osumapisteet < 0) Osumapisteet = 0;
		}
	}
}
