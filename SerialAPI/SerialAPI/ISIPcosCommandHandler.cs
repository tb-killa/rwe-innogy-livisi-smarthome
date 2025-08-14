using System.Collections.Generic;

namespace SerialAPI;

public interface ISIPcosCommandHandler
{
	HandlingResult Handle(SIPcosHeader header, List<byte> message);
}
