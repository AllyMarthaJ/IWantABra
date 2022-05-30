using System;
namespace IWantABra
{
	public class Environ
	{
		public static string WEBHOOK_ENDPOINT = Environment.GetEnvironmentVariable ("WEBHOOK_ENDPOINT");
		public static int BAND_SIZE = Int32.Parse(Environment.GetEnvironmentVariable ("BAND_SIZE"));
		public static string CUP_SIZE = Environment.GetEnvironmentVariable ("CUP_SIZE");
	}
}

