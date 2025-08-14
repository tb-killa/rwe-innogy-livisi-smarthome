using System.IO;

namespace Rebex.Mime.Headers;

public interface IHeader
{
	void Encode(TextWriter writer);

	IHeader Clone();
}
