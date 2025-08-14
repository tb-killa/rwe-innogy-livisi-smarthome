namespace onrkn;

internal abstract class qntin : bpnki
{
	protected readonly byte[] xudys;

	protected readonly byte[] ueiwb;

	public qntin(byte[] salt)
	{
		xudys = new byte[12];
		ueiwb = new byte[13];
		salt.CopyTo(xudys, 0);
	}
}
