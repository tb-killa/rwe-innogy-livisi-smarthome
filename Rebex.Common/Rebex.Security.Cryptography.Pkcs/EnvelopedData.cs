using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class EnvelopedData : PkcsBase, lnabj
{
	[NonSerialized]
	private zjcch lsziq;

	[NonSerialized]
	private qptdi mzkfp;

	[NonSerialized]
	private RecipientInfoCollection pkyax;

	[NonSerialized]
	private anrmr hmatz;

	[NonSerialized]
	private AlgorithmIdentifier rqrfp;

	[NonSerialized]
	private CryptographicAttributeCollection rqrew;

	[NonSerialized]
	private byte[] pxdti;

	[NonSerialized]
	private bool tkpkm;

	[NonSerialized]
	private ContentInfo pgovr;

	[NonSerialized]
	private rhegb fmqdt;

	[NonSerialized]
	private ICertificateFinder tlnki;

	[NonSerialized]
	private bool xzbzf;

	private static Func<RecipientInfo, Certificate, bool> mtwgb;

	private static Func<RecipientInfo, Certificate, bool> xgnfm;

	public ICertificateFinder CertificateFinder
	{
		get
		{
			return tlnki;
		}
		set
		{
			if (tlnki == value)
			{
				return;
			}
			tlnki = value;
			CertificateStore certificateStore = new CertificateStore((ICollection)mzkfp.blnog);
			try
			{
				IEnumerator<RecipientInfo> enumerator = pkyax.GetEnumerator();
				try
				{
					while (enumerator.MoveNext() ? true : false)
					{
						RecipientInfo current = enumerator.Current;
						current.oyacr(certificateStore, tlnki);
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
			finally
			{
				if (certificateStore != null && 0 == 0)
				{
					((IDisposable)certificateStore).Dispose();
				}
			}
		}
	}

	public bool Silent
	{
		get
		{
			return xzbzf;
		}
		set
		{
			xzbzf = value;
		}
	}

	public bool IsEncrypted => tkpkm;

	public CryptographicAttributeCollection UnprotectedAttributes => rqrew;

	public CertificateCollection Certificates => mzkfp.blnog;

	public CertificateRevocationListCollection CertificateRevocationLists => mzkfp.urrfw;

	public RecipientInfoCollection RecipientInfos => pkyax;

	public ContentInfo ContentInfo
	{
		get
		{
			if (hmatz == null || 1 == 0)
			{
				return null;
			}
			if (pgovr == null || 1 == 0)
			{
				pgovr = new ContentInfo(hmatz.mhtzz.scakm, hmatz.pgfiz());
			}
			return pgovr;
		}
		set
		{
			if (tkpkm && 0 == 0)
			{
				throw new CryptographicException("Cannot change content of an encrypted message, decrypt it first.");
			}
			pgovr = value;
			AlgorithmIdentifier wkske = hmatz.wkske;
			hmatz = new anrmr(pgovr, wkske);
		}
	}

	public AlgorithmIdentifier ContentEncryptionAlgorithm
	{
		get
		{
			if (hmatz == null || false || hmatz.wkske == null)
			{
				return null;
			}
			rqrfp = hmatz.wkske.evxkk();
			return rqrfp;
		}
	}

	public bool HasPrivateKey
	{
		get
		{
			IEnumerator<RecipientInfo> enumerator = pkyax.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					RecipientInfo current = enumerator.Current;
					if (current.vqzfk && 0 == 0)
					{
						return true;
					}
				}
			}
			finally
			{
				if (enumerator != null && 0 == 0)
				{
					enumerator.Dispose();
				}
			}
			return false;
		}
	}

	public bool AcquirePrivateKey()
	{
		IEnumerator<RecipientInfo> enumerator = pkyax.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				RecipientInfo current = enumerator.Current;
				if (current.zqjdy(xzbzf) && 0 == 0)
				{
					return true;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return false;
	}

	internal int nsmfw()
	{
		if (pxdti != null && 0 == 0)
		{
			return pxdti.Length;
		}
		throw new InvalidOperationException("Private key is not available yet.");
	}

	public byte[] GetSymmetricKey()
	{
		if (pxdti != null && 0 == 0)
		{
			return pxdti;
		}
		IEnumerator<RecipientInfo> enumerator = pkyax.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				RecipientInfo current = enumerator.Current;
				byte[] array = current.ghozk(xzbzf);
				if (array != null && 0 == 0)
				{
					pxdti = array;
					return array;
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		return null;
	}

	public SymmetricAlgorithm GetSymmetricAlgorithm()
	{
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = isgoo();
		if (symmetricKeyAlgorithm == null || 1 == 0)
		{
			return null;
		}
		return symmetricKeyAlgorithm.hwqsr();
	}

	internal SymmetricKeyAlgorithm isgoo()
	{
		byte[] symmetricKey = GetSymmetricKey();
		if (symmetricKey == null || 1 == 0)
		{
			return null;
		}
		if (hmatz == null || 1 == 0)
		{
			throw new CryptographicException("No message has been loaded yet.");
		}
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = hmatz.pxelf();
		symmetricKeyAlgorithm.SetKey(symmetricKey);
		return symmetricKeyAlgorithm;
	}

	public void Encrypt()
	{
		rgetd(azsca.uiamg);
	}

	internal void rgetd(azsca p0)
	{
		if (tkpkm && 0 == 0)
		{
			throw new CryptographicException("The message is already encrypted.");
		}
		blyzj(p0, p1: true);
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = isgoo();
		try
		{
			if (symmetricKeyAlgorithm == null || 1 == 0)
			{
				throw new CryptographicException("Cannot retrieve the symmetric key.");
			}
			ICryptoTransform cryptoTransform = symmetricKeyAlgorithm.CreateEncryptor();
			try
			{
				darjd(cryptoTransform);
				tkpkm = true;
			}
			finally
			{
				if (cryptoTransform != null && 0 == 0)
				{
					cryptoTransform.Dispose();
				}
			}
		}
		finally
		{
			if (symmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)symmetricKeyAlgorithm).Dispose();
			}
		}
	}

	public void Decrypt()
	{
		grqku(azsca.uiamg);
	}

	internal void grqku(azsca p0)
	{
		if (!tkpkm || 1 == 0)
		{
			throw new CryptographicException("The message is already decrypted.");
		}
		blyzj(p0, p1: false);
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = isgoo();
		try
		{
			if (symmetricKeyAlgorithm == null || 1 == 0)
			{
				throw new CryptographicException("Cannot retrieve the symmetric key.");
			}
			ICryptoTransform cryptoTransform = symmetricKeyAlgorithm.CreateDecryptor();
			try
			{
				darjd(cryptoTransform);
				tkpkm = false;
			}
			finally
			{
				if (cryptoTransform != null && 0 == 0)
				{
					cryptoTransform.Dispose();
				}
			}
		}
		finally
		{
			if (symmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)symmetricKeyAlgorithm).Dispose();
			}
		}
	}

	private void blyzj(azsca p0, bool p1)
	{
		Func<RecipientInfo, Certificate, bool> func;
		switch (p0)
		{
		case azsca.xtlsb:
			return;
		case azsca.uiamg:
			if (mtwgb == null || 1 == 0)
			{
				mtwgb = bjmet;
			}
			func = mtwgb;
			break;
		case azsca.iuqjz:
			if (xgnfm == null || 1 == 0)
			{
				xgnfm = rvqqn;
			}
			func = xgnfm;
			break;
		default:
			throw new InvalidOperationException("Unsupported key usage check type.");
		}
		bool flag = false;
		IEnumerator<RecipientInfo> enumerator = pkyax.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				RecipientInfo current = enumerator.Current;
				Certificate certificate = current.Certificate;
				if (certificate != null && 0 == 0)
				{
					flag = true;
					if (func(current, certificate) && 0 == 0)
					{
						return;
					}
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		if (flag && 0 == 0)
		{
			throw new CryptographicException(brgjd.edcru("No recipient has a certificate with correct key usage for {0}.", (p1 ? true : false) ? "encryption" : "decryption"));
		}
	}

	private void darjd(ICryptoTransform p0)
	{
		int inputBlockSize = p0.InputBlockSize;
		int num = 32768 / inputBlockSize;
		byte[] array = new byte[num * inputBlockSize];
		byte[] array2 = new byte[num * p0.OutputBlockSize];
		opjbe opjbe = new opjbe();
		Stream stream = hmatz.pgfiz().ymxwm();
		try
		{
			int num2;
			while (true)
			{
				num2 = zrwmt.ewhcy(stream, array, 0, array.Length);
				if (num2 == 0 || false || ((num2 % inputBlockSize != 0) ? true : false))
				{
					break;
				}
				int count = p0.TransformBlock(array, 0, num2, array2, 0);
				opjbe.Write(array2, 0, count);
			}
			array2 = p0.TransformFinalBlock(array, 0, num2);
			opjbe.Write(array2, 0, array2.Length);
		}
		finally
		{
			if (stream != null && 0 == 0)
			{
				((IDisposable)stream).Dispose();
			}
		}
		AlgorithmIdentifier wkske = hmatz.wkske;
		hmatz = new anrmr(new ContentInfo(opjbe), wkske);
		pgovr = null;
	}

	private void syjlt(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in syjlt
		this.syjlt(p0, p1, p2);
	}

	private lnabj znnmf(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return lsziq;
		case 65536:
			return mzkfp;
		case 1:
			return pkyax;
		case 2:
			hmatz = new anrmr();
			return hmatz;
		case 65537:
			return new rwknq(rqrew, 1, rmkkr.wguaf);
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in znnmf
		return this.znnmf(p0, p1, p2);
	}

	private void wpzog(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in wpzog
		this.wpzog(p0, p1, p2);
	}

	private void btfsn()
	{
		if (hmatz == null || 1 == 0)
		{
			throw new CryptographicException("Enveloped data does not contain an encrypted content info.");
		}
		int num = lsziq.kybig();
		if (num < 0)
		{
			num = -1 - num;
			lsziq = new zjcch(num);
			tkpkm = false;
		}
		else
		{
			tkpkm = true;
		}
		CertificateStore certificateStore = new CertificateStore((ICollection)mzkfp.blnog);
		try
		{
			IEnumerator<RecipientInfo> enumerator = pkyax.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					RecipientInfo current = enumerator.Current;
					current.bvglb(this);
					current.oyacr(certificateStore, tlnki);
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
		finally
		{
			if (certificateStore != null && 0 == 0)
			{
				((IDisposable)certificateStore).Dispose();
			}
		}
	}

	void lnabj.somzq()
	{
		//ILSpy generated this explicit interface implementation from .override directive in btfsn
		this.btfsn();
	}

	private void huqrm(fxakl p0)
	{
		int num;
		if (pkyax.qgzsb && 0 == 0)
		{
			num = 3;
			if (num != 0)
			{
				goto IL_009f;
			}
		}
		if ((mzkfp.kchbe ? true : false) || rqrew.Count == 0 || 1 == 0)
		{
			num = 0;
			if (num == 0)
			{
				goto IL_009f;
			}
		}
		num = 0;
		IEnumerator<RecipientInfo> enumerator = pkyax.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				RecipientInfo current = enumerator.Current;
				if (current.bilco != 0 && 0 == 0)
				{
					num = 2;
					if (num != 0)
					{
						break;
					}
				}
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		goto IL_009f;
		IL_009f:
		if (!tkpkm || 1 == 0)
		{
			num = -1 - num;
		}
		lsziq = new zjcch(num);
		ArrayList arrayList = new ArrayList();
		arrayList.Add(lsziq);
		if (!mzkfp.kchbe || 1 == 0)
		{
			arrayList.Add(new rwknq(mzkfp, 0, rmkkr.osptv));
		}
		arrayList.Add(pkyax);
		arrayList.Add(hmatz);
		if (rqrew.Count > 0)
		{
			arrayList.Add(new rwknq(rqrew, 1, rmkkr.wguaf));
		}
		p0.aiflg(rmkkr.osptv, arrayList);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in huqrm
		this.huqrm(p0);
	}

	private Stream khxve()
	{
		epeku(null);
		return new hfnnn(fmqdt);
	}

	public void Decode(byte[] encodedMessage)
	{
		if (encodedMessage == null || 1 == 0)
		{
			throw new ArgumentNullException("encodedMessage");
		}
		Stream stream = khxve();
		stream.Write(encodedMessage, 0, encodedMessage.Length);
		stream.Close();
	}

	public byte[] Encode()
	{
		eorvm eorvm = new eorvm();
		Save(eorvm);
		return eorvm.ozoyw();
	}

	public static bool IsEnvelopedData(byte[] data, int offset, int count)
	{
		if (!hfnnn.hfmsu(data, offset, count) || 1 == 0)
		{
			return false;
		}
		rhegb rhegb = new rhegb(detectOnly: true);
		hfnnn.oalpn(rhegb, data, offset, count);
		return rhegb.nsfih.Value == "1.2.840.113549.1.7.3";
	}

	public void Load(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		Stream p = khxve();
		p = PkcsBase.ghxwz(input, p, "CMS");
		input.alskc(p);
		p.Close();
		input.Close();
	}

	public void Save(Stream output)
	{
		if (output == null || 1 == 0)
		{
			throw new ArgumentNullException("output");
		}
		if (!tkpkm || 1 == 0)
		{
			throw new CryptographicException("The message is not encrypted. Call the Encrypt method first.");
		}
		fxakl fxakl = new fxakl(output);
		fxakl.kfyej(fmqdt);
		fxakl.imfsc();
	}

	internal void epeku(rhegb p0)
	{
		if (p0 == null || 1 == 0)
		{
			p0 = new rhegb(this, new ObjectIdentifier("1.2.840.113549.1.7.3"));
		}
		int num;
		if (pkyax != null && 0 == 0)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0037;
			}
			goto IL_004d;
		}
		goto IL_005b;
		IL_0037:
		pkyax[num].bvglb(null);
		num++;
		goto IL_004d;
		IL_005b:
		lsziq = new zjcch(0);
		mzkfp = new qptdi();
		pkyax = new RecipientInfoCollection(this);
		hmatz = null;
		rqrfp = null;
		rqrew = new CryptographicAttributeCollection();
		pxdti = null;
		tkpkm = false;
		pgovr = null;
		fmqdt = p0;
		return;
		IL_004d:
		if (num < pkyax.Count)
		{
			goto IL_0037;
		}
		goto IL_005b;
	}

	public EnvelopedData Clone()
	{
		return new EnvelopedData(this);
	}

	private EnvelopedData(EnvelopedData source)
	{
		lsziq = new zjcch(0);
		fmqdt = new rhegb(this, new ObjectIdentifier("1.2.840.113549.1.7.3"));
		mzkfp = new qptdi();
		IEnumerator<Certificate> enumerator = source.mzkfp.blnog.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Certificate current = enumerator.Current;
				mzkfp.blnog.Add(current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		IEnumerator<CertificateRevocationList> enumerator2 = source.mzkfp.urrfw.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext() ? true : false)
			{
				CertificateRevocationList current2 = enumerator2.Current;
				mzkfp.urrfw.Add(current2);
			}
		}
		finally
		{
			if (enumerator2 != null && 0 == 0)
			{
				enumerator2.Dispose();
			}
		}
		pkyax = new RecipientInfoCollection(this);
		IEnumerator<RecipientInfo> enumerator3 = source.pkyax.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext() ? true : false)
			{
				RecipientInfo current3 = enumerator3.Current;
				pkyax.Add(current3.krqfr());
			}
		}
		finally
		{
			if (enumerator3 != null && 0 == 0)
			{
				enumerator3.Dispose();
			}
		}
		if (source.hmatz != null && 0 == 0)
		{
			hmatz = source.hmatz.toait();
		}
		rqrew = new CryptographicAttributeCollection();
		IEnumerator<CryptographicAttributeNode> enumerator4 = source.rqrew.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext() ? true : false)
			{
				CryptographicAttributeNode current4 = enumerator4.Current;
				rqrew.Add(current4.rtdcm());
			}
		}
		finally
		{
			if (enumerator4 != null && 0 == 0)
			{
				enumerator4.Dispose();
			}
		}
		if (source.pxdti != null && 0 == 0)
		{
			pxdti = (byte[])source.pxdti.Clone();
		}
		tkpkm = source.tkpkm;
		tlnki = source.tlnki;
		xzbzf = source.xzbzf;
	}

	public EnvelopedData()
		: this(null, null)
	{
	}

	public EnvelopedData(ContentInfo contentInfo)
		: this(contentInfo, new ObjectIdentifier("1.2.840.113549.3.7"))
	{
	}

	public EnvelopedData(ContentInfo contentInfo, ObjectIdentifier encryptionAlgorithm)
		: this(contentInfo, encryptionAlgorithm, 0)
	{
	}

	public EnvelopedData(ContentInfo contentInfo, ObjectIdentifier encryptionAlgorithm, int keyLength)
	{
		xzbzf = true;
		tlnki = Rebex.Security.Cryptography.Pkcs.CertificateFinder.Default;
		epeku(null);
		if (contentInfo == null || 1 == 0)
		{
			contentInfo = new ContentInfo();
		}
		if (encryptionAlgorithm == null || 1 == 0)
		{
			encryptionAlgorithm = new ObjectIdentifier("1.2.840.113549.3.7");
		}
		string value = encryptionAlgorithm.Value;
		SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm(value);
		try
		{
			if (value == "1.2.840.113549.3.2" && 0 == 0 && keyLength > 0)
			{
				symmetricKeyAlgorithm.EffectiveKeySize = keyLength;
			}
			symmetricKeyAlgorithm.GenerateIV();
			symmetricKeyAlgorithm.GenerateKey();
			AlgorithmIdentifier encryptionAlgorithm2 = new AlgorithmIdentifier(symmetricKeyAlgorithm);
			pxdti = symmetricKeyAlgorithm.GetKey();
			pgovr = contentInfo;
			hmatz = new anrmr(pgovr, encryptionAlgorithm2);
		}
		finally
		{
			if (symmetricKeyAlgorithm != null && 0 == 0)
			{
				((IDisposable)symmetricKeyAlgorithm).Dispose();
			}
		}
	}

	private static bool bjmet(RecipientInfo p0, Certificate p1)
	{
		return p0.oaeit();
	}

	private static bool rvqqn(RecipientInfo p0, Certificate p1)
	{
		if (!p0.oaeit() || 1 == 0)
		{
			return false;
		}
		string[] enhancedUsage = p1.GetEnhancedUsage();
		if (enhancedUsage != null && 0 == 0)
		{
			return enhancedUsage.babpw("1.3.6.1.5.5.7.3.4", "2.5.29.37.0") >= 0;
		}
		return true;
	}
}
