namespace onrkn;

internal abstract class qoqui
{
	private vcedo iwdyy;

	public vcedo qeloj => iwdyy;

	public abstract int nimwj { get; }

	public abstract void gjile(byte[] p0, int p1);

	public byte[] szrqi()
	{
		byte[] array = new byte[nimwj];
		gjile(array, 0);
		return array;
	}

	public qoqui(vcedo contentType)
	{
		iwdyy = contentType;
	}
}
