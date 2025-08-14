using System;
using System.ComponentModel;
using onrkn;

namespace Rebex.Security.Certificates;

public class ValidationResult
{
	private long yyjry;

	private bool lggxb;

	private int jdbzc;

	public ValidationStatus Status => (ValidationStatus)yyjry;

	public bool Valid => lggxb;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[wptwl(false)]
	[Obsolete("ErrorCode property has been deprecated and will be removed. Use NativeErrorCode instead.", true)]
	public int ErrorCode => 0;

	public int NativeErrorCode => jdbzc;

	internal ValidationResult(bool valid, long status, int errorCode)
	{
		lggxb = valid;
		yyjry = status;
		jdbzc = errorCode;
	}

	public ValidationResult(ValidationStatus status)
		: this(status == (ValidationStatus)0L, (long)status, 0)
	{
	}

	internal void mpqyw(bool p0, long p1)
	{
		lggxb &= p0;
		yyjry |= p1;
	}
}
