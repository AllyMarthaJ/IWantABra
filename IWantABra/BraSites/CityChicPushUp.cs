using System;
using System.Text.RegularExpressions;
using CloudFlareUtilities;
using IWantABra.Brafinder;

namespace IWantABra.BraSites
{
	public class CityChicPushUp : IBraSite
	{
		const string BRA_URL = "https://www.citychic.com.au/lingerie/plus-size-bras/plus-size-push-up-bras";
		const string BRA_PAGE_APPEND = "?p=";
		const string PAGES_REGEX = "<div class=\"amount\" id=\"toolbar-amount\"> ([0-9]+) products </div>";
		const string BRAS_REGEX = "<a class=\"product-item-link\" href=\"(.*?)\">[\\w\\W]+?<\\/a>";

		const string BRA_NAME_REGEX = "<title>Women&#039;s Plus Size (.*?)\\W*<\\/title>";
		const string BRA_SIZE_REGEX = "<span class=\"size\">([0-9]+[A-Z]*)?(?: \\/ )?([0-9]+[A-Z]*)?<\\/span>";
		const string BRA_REGEX = "([0-9]+)([A-Z]*)";

		private HttpClient giveMeABraClient;

		public CityChicPushUp()
		{
			var handler = new ClearanceHandler {
				MaxRetries = 2
			};
			giveMeABraClient = new HttpClient (handler);
		}

		public async Task<Bra> GetBraAsync (string url)
		{
			var result = await giveMeABraClient.GetAsync (url);
			var content = await result.Content.ReadAsStringAsync ();

			var braName = Regex.Match (content, BRA_NAME_REGEX).Groups[1].Value;

			var braSizeRegexResults = Regex.Matches (content, BRA_SIZE_REGEX);

			var sizes = new List<Size> ();
			foreach (Match bsMatch in braSizeRegexResults) {
				// only care about au size
				var braSizeRaw = bsMatch.Groups [1].Value;
				var bsParse = Regex.Match (braSizeRaw, BRA_REGEX);

				int bandSize = Int32.Parse (bsParse.Groups [1].Value);
				string cupSize = "";
				if (bsParse.Groups.Count >= 3) {
					cupSize = bsParse.Groups [2].Value;
				}

				sizes.Add (new Size (bandSize, cupSize));
			}

			var bra = new Bra (braName, url, sizes.ToArray ());

			return bra;
		}

		public async Task<string []> GetBrasAsync (int page)
		{
			var url = BRA_URL + BRA_PAGE_APPEND + page;
			var result = await giveMeABraClient.GetAsync (url);
			var content = await result.Content.ReadAsStringAsync ();

			var braRegexResult = Regex.Matches (content, BRAS_REGEX)
				.Select (w => w.Groups [1].Value)
				.Select (w => System.Web.HttpUtility.HtmlDecode (w))
				.ToArray ();

			return braRegexResult;
		}

		public async Task<BraPage []> GetBrasAsync (int [] pages)
		{
			var braPages = new List<BraPage> ();

			foreach (var page in pages) {
				var brasOnThisPage = await this.GetBrasAsync (page);
				List<Bra> allBras = new ();

				foreach (var bra in brasOnThisPage) {
					allBras.Add (await this.GetBraAsync (bra));
				}

				braPages.Add (new BraPage (allBras.ToArray ()));
			}

			return braPages.ToArray ();
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

