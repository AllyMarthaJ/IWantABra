using System;
namespace IWantABra.Brafinder
{
	public record Bra(string Name, string Url, Size[] AvailableSizes);
	public record Size(int BandSize, string CupSize);
	public record BraPage(Bra[] Bras);
}