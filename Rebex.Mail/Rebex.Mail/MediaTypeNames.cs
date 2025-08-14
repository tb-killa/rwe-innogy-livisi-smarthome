namespace Rebex.Mail;

public sealed class MediaTypeNames
{
	public sealed class Application
	{
		public const string Octet = "application/octet-stream";

		public const string Zip = "application/zip";

		public const string Pdf = "application/pdf";

		public const string Rtf = "application/rtf";

		public const string Soap = "application/soap+xml";

		private Application()
		{
		}
	}

	public sealed class Image
	{
		public const string Png = "image/png";

		public const string Jpeg = "image/jpeg";

		public const string Gif = "image/gif";

		public const string Tiff = "image/tiff";

		private Image()
		{
		}
	}

	public sealed class Text
	{
		public const string Plain = "text/plain";

		public const string Html = "text/html";

		public const string Xml = "text/xml";

		public const string Rtf = "text/rtf";

		public const string Enriched = "text/enriched";

		public const string RichText = "text/richtext";

		private Text()
		{
		}
	}

	public sealed class Multipart
	{
		public const string Mixed = "multipart/mixed";

		public const string Alternative = "multipart/alternative";

		public const string Digest = "multipart/digest";

		public const string Related = "multipart/related";

		public const string Parallel = "multipart/parallel";

		public const string Report = "multipart/report";

		public const string Signed = "multipart/signed";

		private Multipart()
		{
		}
	}

	public sealed class Message
	{
		public const string Rfc822 = "message/rfc822";

		public const string Partial = "message/partial";

		private Message()
		{
		}
	}

	private MediaTypeNames()
	{
	}
}
