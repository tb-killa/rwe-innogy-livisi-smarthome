using System;
using System.Collections;
using System.Collections.Generic;
using Rebex.Security.Cryptography.Pkcs;
using onrkn;

namespace Rebex.Mail;

public class SubjectInfoCollection : ICollection, IEnumerable<SubjectInfo>, IEnumerable
{
	private readonly ArrayList qmxbo;

	public int Count => qmxbo.Count;

	public SubjectInfo this[int index] => (SubjectInfo)qmxbo[index];

	public bool IsSynchronized => false;

	public object SyncRoot => qmxbo.SyncRoot;

	internal SubjectInfoCollection()
	{
		qmxbo = new ArrayList();
	}

	internal SubjectInfoCollection(EnvelopedData data)
		: this()
	{
		IEnumerator<RecipientInfo> enumerator = data.RecipientInfos.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				RecipientInfo current = enumerator.Current;
				qmxbo.Add(new SubjectInfo(current, data));
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	internal SubjectInfoCollection(SignedData data)
		: this()
	{
		MailSignatureStyle style = ((data.Detached ? true : false) ? MailSignatureStyle.Detached : MailSignatureStyle.Enveloped);
		IEnumerator<SignerInfo> enumerator = data.SignerInfos.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				SignerInfo current = enumerator.Current;
				qmxbo.Add(new SubjectInfo(current, style));
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	public IEnumerator GetEnumerator()
	{
		return qmxbo.GetEnumerator();
	}

	private IEnumerator<SubjectInfo> sqsps()
	{
		return this.eaqmu<SubjectInfo>().GetEnumerator();
	}

	IEnumerator<SubjectInfo> IEnumerable<SubjectInfo>.GetEnumerator()
	{
		//ILSpy generated this explicit interface implementation from .override directive in sqsps
		return this.sqsps();
	}

	private void sjzaj(Array p0, int p1)
	{
		qmxbo.CopyTo(p0, p1);
	}

	void ICollection.CopyTo(Array p0, int p1)
	{
		//ILSpy generated this explicit interface implementation from .override directive in sjzaj
		this.sjzaj(p0, p1);
	}

	public void CopyTo(SubjectInfo[] array, int index)
	{
		((ICollection)this).CopyTo((Array)array, index);
	}
}
