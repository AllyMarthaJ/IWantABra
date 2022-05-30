using System;
namespace IWantABra.Brafinder {
	public class Finder {
		public Size SizeFilter { get; }
		public IBraSite BraSite { get; }

		public Finder (IBraSite site, Size sizeFilter)
		{
			this.BraSite = site;
			this.SizeFilter = sizeFilter;
		}

		public async Task<Bra []> FindAllBrasAsync ()
		{
			string source = await this.BraSite.InitSourceAsync ();

			(int minimum, int maximum) = this.BraSite.GetPages (source);
			int [] pages = Enumerable.Range (minimum, maximum - minimum + 1).ToArray ();

			var allBras = await this.BraSite.GetBrasAsync (pages);

			return allBras
				.Select (o => o.Bras)
				.Aggregate ((c, o) => c.Concat (o).ToArray())
				.Where (b => b.AvailableSizes.Contains (this.SizeFilter))
				.ToArray();
		}
	}
}

