using IWantABra.Brafinder;
using IWantABra.BraSites;

namespace IWantABra;

class Program {
	public static async Task Main (string [] args)
	{
		var ccSite = new CityChicPushUp ();
		var finder = new Finder (ccSite, new Size (24, "D"));

		var bras = await finder.FindAllBrasAsync ();

		foreach (var bra in bras) {
			Console.WriteLine ($"Bra: {bra.Name}");

			Console.WriteLine ($"Sizes available: {String.Join (", ", bra.AvailableSizes.Select(s => s.BandSize + "" + s.CupSize))}");
		}

		Console.WriteLine ("Yeet.");
	}
}