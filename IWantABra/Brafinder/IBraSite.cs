using System;
namespace IWantABra.Brafinder
{
	public interface IBraSite
	{
		/// <summary>
		/// An initial request to provide metadata back about how we scrape the site further.
		/// </summary>
		/// <returns>The source of the initial information page</returns>
		public Task<string> InitSourceAsync ();

		/// <summary>
		/// Furthering from the initial request; require the number of pages.
		/// </summary>
		/// <param name="source">The source of the request to parse.</param>
		/// <returns>Number of pages available</returns>
		public (int minimum, int maximum) GetPages (string source);

		/// <summary>
		/// Get a list of URLs to available bras in the given size.
		/// </summary>
		/// <param name="page">The page number</param>
		/// <returns>List of URLs to each available bra</returns>
		public Task<string[]> GetBrasAsync (int page);

		/// <summary>
		/// Fetch information about a bra.
		/// </summary>
		/// <param name="url">The URL to fetch bra information from.</param>
		/// <returns>Bra information</returns>
		public Task<Bra> GetBraAsync (string url);

		/// <summary>
		/// Fetch all available bras.
		/// </summary>
		/// <param name="pages">The pages to fetch</param>
		/// <returns></returns>
		public Task<BraPage []> GetBrasAsync (int [] pages);
	}
}

