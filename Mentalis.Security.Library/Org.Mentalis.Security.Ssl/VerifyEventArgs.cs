namespace Org.Mentalis.Security.Ssl;

public class VerifyEventArgs
{
	public bool Valid { get; set; }

	public VerifyEventArgs()
		: this(valid: true)
	{
	}

	public VerifyEventArgs(bool valid)
	{
		Valid = valid;
	}
}
