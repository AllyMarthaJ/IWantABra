using System;
using System.Text.RegularExpressions;
using IWantABra.Brafinder;

namespace IWantABra.BraSites
{
	public class CityChicPushUp : IBraSite
	{
		const string BRA_URL = "https://www.citychic.com.au/lingerie/plus-size-bras/plus-size-push-up-bras";
		const string BRA_PAGE_APPEND = "?p=";
		const string PAGES_REGEX = "<div class=\"amount\" id=\"toolbar-amount\"> ([0-9]+) products </div>";
		const string BRAS_REGEX = "<a class=\"product-item-link\" href=\"(.*?)\">[\\w\\W]+?<\\/a>";

		const string BRA_NAME_REGEX = "<span class=\"base\" data-ui-id=\"page-title-wrapper\" itemprop=\"name\">(.*?)\\W+<\\/span>";

		private HttpClient giveMeABraClient = new HttpClient ();

		public Task<Bra> GetBraAsync (string url)
		{
			throw new NotImplementedException ();
		}

		public async Task<string []> GetBrasAsync (int page)
		{
			var url = BRA_URL + BRA_PAGE_APPEND + page;
			var result = await giveMeABraClient.GetAsync (url);
			var content = await result.Content.ReadAsStringAsync ();

			var braRegexResult = Regex.Matches (content, BRAS_REGEX)
				.Select (w => w.Groups [1].Value)
				.ToArray ();

			return braRegexResult;
		}

		public Task<BraPage []> GetBrasAsync (int [] pages)
		{
			throw new NotImplementedException ();
		}

		public (int minimum, int maximum) GetPages (string source)
		{
			// 48 bras pp.
			var products = Int32.Parse(Regex.Match (source, PAGES_REGEX).Groups[1].Value);
			return (1, products / 48 + 1);
		}

		public async Task<string> InitSourceAsync ()
		{
			var result = await giveMeABraClient.GetAsync (BRA_URL);
			return await result.Content.ReadAsStringAsync ();
		}
	}
}

