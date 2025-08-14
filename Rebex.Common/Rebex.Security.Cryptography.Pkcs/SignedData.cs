using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Rebex.Security.Certificates;
using onrkn;

namespace Rebex.Security.Cryptography.Pkcs;

public class SignedData : PkcsBase, lnabj
{
	private sealed class tjvtl
	{
		public IHashTransform fdjar;

		public void pyafw(byte[] p0, int p1)
		{
			fdjar.Process(p0, 0, p1);
		}
	}

	[NonSerialized]
	private zjcch xiloh;

	[NonSerialized]
	private zmdyp fgstu;

	[NonSerialized]
	private ArrayList hecty;

	[NonSerialized]
	private lxxka qddfu;

	[NonSerialized]
	private CertificateCollection ritvm;

	[NonSerialized]
	private CertificateIncludeOption ypnxc;

	[NonSerialized]
	private CertificateRevocationListCollection grukv;

	[NonSerialized]
	private SignerInfoCollection dzoif;

	[NonSerialized]
	private ContentInfo jdxxk;

	[NonSerialized]
	private bool jzroi;

	[NonSerialized]
	private rhegb aaewo;

	[NonSerialized]
	private ICertificateFinder rbxbo;

	[NonSerialized]
	private bool wnihb;

	internal ObjectIdentifier sldbz => qddfu.rrcbj.scakm;

	public CertificateIncludeOption IncludeOption
	{
		get
		{
			return ypnxc;
		}
		set
		{
			ypnxc = value;
		}
	}

	public ICertificateFinder CertificateFinder
	{
		get
		{
			return rbxbo;
		}
		set
		{
			if (rbxbo == value)
			{
				return;
			}
			rbxbo = value;
			CertificateStore certificateStore = new CertificateStore((ICollection)ritvm);
			try
			{
				IEnumerator<SignerInfo> enumerator = dzoif.GetEnumerator();
				try
				{
					while (enumerator.MoveNext() ? true : false)
					{
						SignerInfo current = enumerator.Current;
						current.daovh(certificateStore, rbxbo);
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
			return wnihb;
		}
		set
		{
			wnihb = value;
		}
	}

	public CertificateCollection Certificates => ritvm;

	public CertificateRevocationListCollection CertificateRevocationLists => grukv;

	public SignerInfoCollection SignerInfos => dzoif;

	public ContentInfo ContentInfo
	{
		get
		{
			return jdxxk;
		}
		set
		{
			int num = 0;
			if (num != 0)
			{
				goto IL_0006;
			}
			goto IL_0030;
			IL_0006:
			if (dzoif[num].ogvst && 0 == 0)
			{
				throw new CryptographicException("Cannot change content of a message that was already signed.");
			}
			num++;
			goto IL_0030;
			IL_0030:
			if (num >= dzoif.Count)
			{
				qddfu = new lxxka(value, jzroi);
				jdxxk = value;
				fgstu.lquvo.Clear();
				hecty.Clear();
				return;
			}
			goto IL_0006;
		}
	}

	public bool Detached
	{
		get
		{
			return jzroi;
		}
		set
		{
			jzroi = value;
		}
	}

	internal byte[] nddnj(AlgorithmIdentifier p0)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0009;
		}
		goto IL_004f;
		IL_0009:
		if (p0.Oid.Value == fgstu[num].Oid.Value && 0 == 0)
		{
			return (byte[])hecty[num];
		}
		num++;
		goto IL_004f;
		IL_004f:
		if (num >= fgstu.Count)
		{
			p0 = new AlgorithmIdentifier(p0.Oid, new mdvaz().ionjf());
			byte[] array = kyarw(p0);
			fgstu.Add(p0);
			hecty.Add(array);
			return array;
		}
		goto IL_0009;
	}

	private byte[] kyarw(AlgorithmIdentifier p0)
	{
		HashingAlgorithmId hashingAlgorithmId = p0.vvmoi(p0: false);
		if (hashingAlgorithmId == (HashingAlgorithmId)0 || false || !HashingAlgorithm.IsSupported(hashingAlgorithmId) || 1 == 0)
		{
			return null;
		}
		HashingAlgorithm hashingAlgorithm = new HashingAlgorithm(hashingAlgorithmId);
		Action<byte[], int> action = null;
		tjvtl tjvtl = new tjvtl();
		tjvtl.fdjar = hashingAlgorithm.CreateTransform();
		try
		{
			ContentInfo contentInfo = jdxxk;
			if (action == null || 1 == 0)
			{
				action = tjvtl.pyafw;
			}
			contentInfo.gfzrv(action);
			return tjvtl.fdjar.GetHash();
		}
		finally
		{
			if (tjvtl.fdjar != null && 0 == 0)
			{
				tjvtl.fdjar.Dispose();
			}
		}
	}

	public void Sign()
	{
		mjncq((SignatureOptions)0L, azsca.uiamg);
	}

	public void Sign(SignatureOptions options)
	{
		mjncq(options, azsca.uiamg);
	}

	internal void mjncq(SignatureOptions p0, azsca p1)
	{
		int num = 0;
		if (num != 0)
		{
			goto IL_0006;
		}
		goto IL_003d;
		IL_0006:
		if (!dzoif[num].ogvst || 1 == 0)
		{
			dzoif[num].xzonp(p0, p1);
		}
		num++;
		goto IL_003d;
		IL_003d:
		if (num >= dzoif.Count)
		{
			return;
		}
		goto IL_0006;
	}

	public SignatureValidationResult Validate()
	{
		return hezvv(p0: false, ValidationOptions.None, CertificateChainEngine.Auto, azsca.uiamg);
	}

	public SignatureValidationResult Validate(bool verifySignatureOnly, ValidationOptions options)
	{
		return hezvv(verifySignatureOnly, options, CertificateChainEngine.Auto, azsca.uiamg);
	}

	public SignatureValidationResult Validate(bool verifySignatureOnly, ValidationOptions options, CertificateChainEngine engine)
	{
		return hezvv(verifySignatureOnly, options, engine, azsca.uiamg);
	}

	internal SignatureValidationResult hezvv(bool p0, ValidationOptions p1, CertificateChainEngine p2, azsca p3)
	{
		if (dzoif.Count == 0 || 1 == 0)
		{
			throw new CryptographicException("The message is not signed by any signers.");
		}
		SignatureValidationResult signatureValidationResult = new SignatureValidationResult();
		IEnumerator<SignerInfo> enumerator = dzoif.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				SignerInfo current = enumerator.Current;
				SignatureValidationResult p4 = current.lmlip(p0, p1, p2, p3);
				signatureValidationResult.leusz(p4);
			}
			return signatureValidationResult;
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
	}

	private void zepra(rmkkr p0, bool p1, int p2)
	{
		hfnnn.xmjay(rmkkr.osptv, p0, p1);
	}

	void lnabj.zkxnk(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in zepra
		this.zepra(p0, p1, p2);
	}

	private lnabj phhvr(rmkkr p0, bool p1, int p2)
	{
		switch (p2)
		{
		case 0:
			return xiloh;
		case 1:
			return fgstu;
		case 2:
			qddfu = new lxxka();
			return qddfu;
		case 65536:
			return new rwknq(ritvm, 0, rmkkr.wguaf);
		case 65537:
			grukv = new CertificateRevocationListCollection();
			return new rwknq(grukv, 1, rmkkr.wguaf);
		case 3:
			return dzoif;
		default:
			return null;
		}
	}

	lnabj lnabj.qaqes(rmkkr p0, bool p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in phhvr
		return this.phhvr(p0, p1, p2);
	}

	private void tjplh(byte[] p0, int p1, int p2)
	{
	}

	void lnabj.lnxah(byte[] p0, int p1, int p2)
	{
		//ILSpy generated this explicit interface implementation from .override directive in tjplh
		this.tjplh(p0, p1, p2);
	}

	private void bqade()
	{
		aipxl aipxl = qddfu.lywza();
		if (aipxl == null || 1 == 0)
		{
			if (jdxxk == null || 1 == 0)
			{
				jdxxk = new ContentInfo();
			}
			if (jdxxk.ContentType.Value != qddfu.rrcbj.scakm.Value && 0 == 0)
			{
				throw new CryptographicException("Detached content type does not correspond to encapsulated content type.");
			}
			jzroi = true;
		}
		else
		{
			jzroi = false;
			jdxxk = new ContentInfo(qddfu.rrcbj.scakm, aipxl);
		}
		int num = 0;
		if (num != 0)
		{
			goto IL_00a6;
		}
		goto IL_00cc;
		IL_00a6:
		AlgorithmIdentifier p = fgstu[num];
		byte[] value = kyarw(p);
		hecty.Add(value);
		num++;
		goto IL_00cc;
		IL_00cc:
		if (num < fgstu.Count)
		{
			goto IL_00a6;
		}
		CertificateStore certificateStore = new CertificateStore((ICollection)ritvm);
		try
		{
			IEnumerator<SignerInfo> enumerator = dzoif.GetEnumerator();
			try
			{
				while (enumerator.MoveNext() ? true : false)
				{
					SignerInfo current = enumerator.Current;
					current.lndip(this);
					current.daovh(certificateStore, rbxbo);
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
		//ILSpy generated this explicit interface implementation from .override directive in bqade
		this.bqade();
	}

	private void xspty(fxakl p0)
	{
		int num;
		if (qddfu.rrcbj.scakm.Value != "1.2.840.113549.1.7.1" && 0 == 0)
		{
			num = 3;
			if (num != 0)
			{
				goto IL_007a;
			}
		}
		num = 1;
		IEnumerator<SignerInfo> enumerator = dzoif.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				SignerInfo current = enumerator.Current;
				if (current.byjeu == 3)
				{
					num = 3;
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
		goto IL_007a;
		IL_007a:
		xiloh = new zjcch(num);
		if ((jzroi ? true : false) || jdxxk == null)
		{
			qddfu.taeog(null);
		}
		else
		{
			qddfu.taeog(jdxxk.jphgq());
		}
		ArrayList arrayList = new ArrayList();
		arrayList.Add(xiloh);
		arrayList.Add(new isxih(fgstu));
		arrayList.Add(qddfu);
		if (ritvm != null && 0 == 0 && ritvm.Count > 0)
		{
			arrayList.Add(new rwknq(ritvm, 0, rmkkr.wguaf));
		}
		if (grukv != null && 0 == 0 && grukv.Count > 0)
		{
			arrayList.Add(new rwknq(grukv, 1, rmkkr.wguaf));
		}
		arrayList.Add(new isxih(dzoif));
		p0.aiflg(rmkkr.osptv, arrayList);
	}

	void lnabj.vlfdh(fxakl p0)
	{
		//ILSpy generated this explicit interface implementation from .override directive in xspty
		this.xspty(p0);
	}

	private Stream oqczo()
	{
		ixwfs(null);
		return new hfnnn(aaewo);
	}

	public void Decode(byte[] encodedMessage)
	{
		if (encodedMessage == null || 1 == 0)
		{
			throw new ArgumentNullException("encodedMessage");
		}
		Stream stream = oqczo();
		stream.Write(encodedMessage, 0, encodedMessage.Length);
		stream.Close();
	}

	public byte[] Encode()
	{
		eorvm eorvm = new eorvm();
		Save(eorvm);
		return eorvm.ozoyw();
	}

	public static bool IsSignedData(byte[] data, int offset, int count)
	{
		if (!hfnnn.hfmsu(data, offset, count) || 1 == 0)
		{
			return false;
		}
		rhegb rhegb = new rhegb(detectOnly: true);
		hfnnn.oalpn(rhegb, data, offset, count);
		return rhegb.nsfih.Value == "1.2.840.113549.1.7.2";
	}

	public void Load(Stream input)
	{
		if (input == null || 1 == 0)
		{
			throw new ArgumentNullException("input");
		}
		Stream p = oqczo();
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
		int num = 0;
		if (num != 0)
		{
			goto IL_001e;
		}
		goto IL_0048;
		IL_008d:
		int num2;
		dzoif[num2].zljkd(ritvm, ypnxc);
		num2++;
		goto IL_00ae;
		IL_00ae:
		if (num2 < dzoif.Count)
		{
			goto IL_008d;
		}
		goto IL_00bc;
		IL_001e:
		if (!dzoif[num].ogvst || 1 == 0)
		{
			throw new CryptographicException("Some of the signers don't have a signature. Please call the Sign method prior to encoding the data.");
		}
		num++;
		goto IL_0048;
		IL_0105:
		int num3;
		if (num3 < dzoif.Count)
		{
			goto IL_00db;
		}
		goto IL_0114;
		IL_0134:
		int num4;
		if (num4 >= fgstu.Count)
		{
			fxakl fxakl = new fxakl(output);
			fxakl.kfyej(aaewo);
			fxakl.imfsc();
			return;
		}
		goto IL_00c4;
		IL_0048:
		if (num < dzoif.Count)
		{
			goto IL_001e;
		}
		if (ypnxc != CertificateIncludeOption.LeaveExisting && dzoif.Count > 0)
		{
			ritvm.Clear();
			if (ypnxc != CertificateIncludeOption.None && 0 == 0)
			{
				num2 = 0;
				if (num2 != 0)
				{
					goto IL_008d;
				}
				goto IL_00ae;
			}
		}
		goto IL_00bc;
		IL_00bc:
		num4 = 0;
		if (num4 != 0)
		{
			goto IL_00c4;
		}
		goto IL_0134;
		IL_00c4:
		AlgorithmIdentifier p = fgstu[num4];
		bool flag = false;
		num3 = 0;
		if (num3 != 0)
		{
			goto IL_00db;
		}
		goto IL_0105;
		IL_00db:
		if (dzoif[num3].iblrs(p) && 0 == 0)
		{
			flag = true;
			if (flag)
			{
				goto IL_0114;
			}
		}
		num3++;
		goto IL_0105;
		IL_0114:
		if (!flag || 1 == 0)
		{
			fgstu.RemoveAt(num4);
			num4--;
		}
		num4++;
		goto IL_0134;
	}

	internal void ixwfs(rhegb p0)
	{
		if (p0 == null || 1 == 0)
		{
			p0 = new rhegb(this, new ObjectIdentifier("1.2.840.113549.1.7.2"));
		}
		int num;
		if (dzoif != null && 0 == 0)
		{
			num = 0;
			if (num != 0)
			{
				goto IL_0034;
			}
			goto IL_004a;
		}
		goto IL_0060;
		IL_0034:
		dzoif[num].lndip(null);
		num++;
		goto IL_004a;
		IL_0060:
		xiloh = new zjcch(0);
		dzoif = new SignerInfoCollection(this);
		fgstu = new zmdyp();
		hecty = new ArrayList();
		grukv = new CertificateRevocationListCollection();
		ritvm = new CertificateCollection();
		aaewo = p0;
		return;
		IL_004a:
		if (num >= dzoif.Count)
		{
			goto IL_0060;
		}
		goto IL_0034;
	}

	public SignedData Clone()
	{
		SignedData signedData = new SignedData();
		IEnumerator<Certificate> enumerator = ritvm.GetEnumerator();
		try
		{
			while (enumerator.MoveNext() ? true : false)
			{
				Certificate current = enumerator.Current;
				signedData.ritvm.Add(current);
			}
		}
		finally
		{
			if (enumerator != null && 0 == 0)
			{
				enumerator.Dispose();
			}
		}
		signedData.ypnxc = ypnxc;
		signedData.grukv = grukv.xizgs();
		signedData.qddfu = qddfu.inqqo();
		if (jdxxk != null && 0 == 0)
		{
			aipxl aipxl = signedData.qddfu.lywza();
			if (aipxl != null && 0 == 0)
			{
				signedData.jdxxk = new ContentInfo(jdxxk.ContentType, aipxl);
			}
			else
			{
				signedData.jdxxk = new ContentInfo(jdxxk.ContentType, jdxxk.jphgq());
			}
		}
		IEnumerator<SignerInfo> enumerator2 = dzoif.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext() ? true : false)
			{
				SignerInfo current2 = enumerator2.Current;
				signedData.dzoif.lquvo.Add(current2.haaka(signedData));
			}
		}
		finally
		{
			if (enumerator2 != null && 0 == 0)
			{
				enumerator2.Dispose();
			}
		}
		signedData.jzroi = jzroi;
		signedData.rbxbo = rbxbo;
		signedData.wnihb = wnihb;
		IEnumerator<AlgorithmIdentifier> enumerator3 = fgstu.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext() ? true : false)
			{
				AlgorithmIdentifier current3 = enumerator3.Current;
				signedData.fgstu.Add(current3);
			}
		}
		finally
		{
			if (enumerator3 != null && 0 == 0)
			{
				enumerator3.Dispose();
			}
		}
		IEnumerator enumerator4 = hecty.GetEnumerator();
		try
		{
			while (enumerator4.MoveNext() ? true : false)
			{
				byte[] value = (byte[])enumerator4.Current;
				signedData.hecty.Add(value);
			}
			return signedData;
		}
		finally
		{
			if (enumerator4 is IDisposable disposable && 0 == 0)
			{
				disposable.Dispose();
			}
		}
	}

	public SignedData()
		: this(null, detached: false)
	{
	}

	public SignedData(ContentInfo contentInfo)
		: this(contentInfo, detached: false)
	{
	}

	public SignedData(ContentInfo contentInfo, bool detached)
	{
		ypnxc = CertificateIncludeOption.WholeChain;
		rbxbo = Rebex.Security.Cryptography.Pkcs.CertificateFinder.Default;
		ixwfs(null);
		jdxxk = contentInfo;
		qddfu = new lxxka(jdxxk, detached);
		jzroi = detached;
	}
}
