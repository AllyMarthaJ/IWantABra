using System;
using System.Text.Json;
using IWantABra.Brafinder;

namespace IWantABra {
	public class Brasender {
		public string WebhookEndpoint { get; }
		public Finder Finder { get; }

		private HttpClient client = new ();

		private Bra [] lastBras = Array.Empty<Bra> ();

		public Brasender (string webhook, Finder finder)
		{
			this.WebhookEndpoint = webhook;
			this.Finder = finder;

			loadCache ();
		}

		private void loadCache ()
		{
			if (!File.Exists ("cache.json"))
				return;

			var cache = File.ReadAllText ("cache.json");

			lastBras = JsonSerializer.Deserialize<Bra []> (cache);
		}

		private void saveCache ()
		{
			var cache = JsonSerializer.Serialize (lastBras);

			File.WriteAllText ("cache.json", cache);
		}

		public async Task<Bra []> GetNewBrasAsync ()
		{
			Bra [] bras;
			try {
				bras = await this.Finder.FindAllBrasAsync ();
			} catch {
				return lastBras;
			}

			var lastBraNames = lastBras.Select (b => b.Name).ToArray ();
			var braNames = bras.Select (b => b.Name).ToArray ();

			var comp = new List<Bra> ();

			// name check only; we don't care what size since we're already filtering on size
			for (int i = 0; i < braNames.Length; i++) {
				var newBraName = braNames [i];

				if (!lastBraNames.Contains (newBraName)) {
					comp.Add (bras [i]);
				}
			}

			lastBras = bras;
			saveCache ();

			return comp.ToArray ();
		}

		public async Task SendBraAsync (Bra bra)
		{
			var braEmbed = new {
				type = "rich",
				title = $"Stock Alert - {bra.Name}",
				description = "Sizes available: " + String.Join(", ", bra.AvailableSizes.Select(s => $"{s.BandSize}{s.CupSize}")),
				color = 0xbd21cb,
				url = bra.Url
			};
			var braForm = JsonSerializer.Serialize (new {
				embeds = new [] {
					braEmbed
				}
			});

			var result = await client.PostAsync (this.WebhookEndpoint,
				new StringContent (braForm, encoding: System.Text.Encoding.UTF8, mediaType: "application/json"));
			var content = await result.Content.ReadAsStringAsync ();
		}
	}
}

