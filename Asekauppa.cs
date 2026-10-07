using System;
using System.Collections.Generic;

namespace ritaripeli
{
	/// <summary>
	/// Asekauppa myy parempia aseita ritarin käteen.
	/// Lisäksi kaupasta voi tilata yksilöllisen mittatilausaseen.
	/// </summary>
	internal class Asekauppa : IKauppa
	{
		private List<TavaraJaHinta> tavarat;

		public Asekauppa()
		{
			tavarat = new List<TavaraJaHinta>
			{
				new TavaraJaHinta(new Ase("Tikari", 4), 8),
				new TavaraJaHinta(new Ase("Kirves", 6), 15),
				new TavaraJaHinta(new Ase("Pitkämiekka", 8), 25),
				new TavaraJaHinta(new Ase("Sotavasara", 11), 40),
			};
		}

		public List<TavaraJaHinta> ListaaTavarat()
		{
			return tavarat;
		}

		public Tavara? OstaTavara(int valittuTavara, Lompakko rahapussi)
		{
			if (valittuTavara < 0 || valittuTavara >= tavarat.Count)
				return null;

			TavaraJaHinta valinta = tavarat[valittuTavara];
			if (rahapussi.OtaRahaa(valinta.Hinta) > 0)
			{
				Ase alkuperäinen = (Ase)valinta.Esine;
				return new Ase(alkuperäinen.Nimi, alkuperäinen.Vahinko);
			}
			return null;
		}

		public void Toimi(Ritari pelaaja)
		{
			Print.Line("Tervetuloa asekauppaan!");
			bool poistu = false;
			while (!poistu)
			{
				Ritaripeli.NäytäTilanne(pelaaja);
				Print.Line("Valitse toiminto:");
				Print.Line("1 Osta mittatilausase");
				Print.Line("2 Listaa kaupan tavarat");
				Print.Line("3 Osta tavara");
				Print.Line("4 Poistu");
				Print.Write("> ");
				string valinta = Console.ReadLine();

				switch (valinta)
				{
					case "1":
						OstaMittatilausAse(pelaaja);
						break;
					case "2":
						NäytäTavarat();
						break;
					case "3":
						OstaValikosta(pelaaja);
						break;
					case "4":
						poistu = true;
						break;
					default:
						Print.Line("Tuntematon valinta.");
						break;
				}
			}
		}

		private void NäytäTavarat()
		{
			Print.Line("Asekauppa tavarat:");
			for (int i = 0; i < tavarat.Count; i++)
			{
				Print.Line($"{i + 1}: {tavarat[i].Esine.Nimi,-14} {tavarat[i].Hinta} kr");
			}
		}

		private void OstaValikosta(Ritari pelaaja)
		{
			Print.Line("Minkä tavaran haluat ostaa?");
			NäytäTavarat();
			Print.Write("> ");

			if (!int.TryParse(Console.ReadLine(), out int valinta))
			{
				Print.Line("Tuntematon valinta.");
				return;
			}

			int indeksi = valinta - 1;
			if (indeksi < 0 || indeksi >= tavarat.Count)
			{
				Print.Line("Sellaista tavaraa ei ole.");
				return;
			}

			int hinta = tavarat[indeksi].Hinta;
			Tavara? ostettu = OstaTavara(indeksi, pelaaja.Rahapussi);
			if (ostettu != null)
			{
				pelaaja.Reppu.Lisää(ostettu);
				Print.Line($"Ostit tavaran {ostettu.Nimi}. Käytit {hinta} kultarahaa.");
			}
			else
			{
				Print.Line("Sinulla ei ole tarpeeksi kultarahaa.");
			}
		}

		private void OstaMittatilausAse(Ritari pelaaja)
		{
			Print.Line("Minkä nimisen aseen haluat teettää?");
			Print.Write("> ");
			string nimi = Console.ReadLine();
			if (string.IsNullOrWhiteSpace(nimi)) nimi = "Mittatilausase";

			Print.Line("Kuinka paljon vahinkoa aseen pitäisi aiheuttaa (1-15)?");
			Print.Write("> ");
			if (!int.TryParse(Console.ReadLine(), out int vahinko))
			{
				Print.Line("Tuntematon valinta.");
				return;
			}
			vahinko = Math.Clamp(vahinko, 1, 15);

			int hinta = vahinko * 4;
			Print.Line($"Mittatilausase '{nimi}' ({vahinko} vahinkoa) maksaa {hinta} kultarahaa. Ostetaanko? (k/e)");
			Print.Write("> ");
			string vastaus = Console.ReadLine();
			if (vastaus?.ToLower() != "k")
			{
				Print.Line("Peruit tilauksen.");
				return;
			}

			if (pelaaja.Rahapussi.OtaRahaa(hinta) > 0)
			{
				Ase ase = new Ase(nimi, vahinko);
				pelaaja.Reppu.Lisää(ase);
				Print.Line($"Ostit mittatilausaseen {ase.Nimi}. Käytit {hinta} kultarahaa.");
			}
			else
			{
				Print.Line("Sinulla ei ole tarpeeksi kultarahaa.");
			}
		}
	}
}
