using IWantABra.Brafinder;
using IWantABra.BraSites;

namespace IWantABra;

class Program {
	public static async Task Main (string [] args)
	{
		var ccSite = new CityChicPushUp ();
		var finder = new Finder (ccSite, new Size (Environ.BAND_SIZE, Environ.CUP_SIZE));

		var braSender = new Brasender (Environ.WEBHOOK_ENDPOINT, finder);

		while (true) {
			var bras = await braSender.GetNewBrasAsync ();

			Console.WriteLine ("<---- NEW ROUND OF BRAS ---->");
			foreach (var bra in bras) {
				Console.WriteLine ("<---- SENDING ---->");
				await braSender.SendBraAsync (bra);
			}

			Console.WriteLine ("Now waiting 30 minutes.");
			await Task.Delay (30 * 1000 * 60 * 60);

		}

		Console.WriteLine ("Yeet.");
	}
}