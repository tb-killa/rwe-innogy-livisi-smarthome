using Rebex.Security.Cryptography.Pkcs;

namespace onrkn;

internal class pddyr
{
	private sxlwf hkpic;

	private rwolq effdi;

	private lnabj lufzx;

	public pddyr(sxlwf issuerAndSerialNumber)
	{
		hkpic = issuerAndSerialNumber;
		lufzx = new rwknq(hkpic, 0, rmkkr.osptv);
	}

	public pddyr(rwolq subjectKeyIdentifier)
	{
		effdi = subjectKeyIdentifier;
		lufzx = new rwknq(effdi, 2, rmkkr.zkxoz);
	}

	public SubjectIdentifier pjlww()
	{
		if (hkpic != null && 0 == 0)
		{
			return new SubjectIdentifier(hkpic);
		}
		if (effdi != null && 0 == 0)
		{
			return new SubjectIdentifier(effdi);
		}
		return null;
	}

	public pddyr(int position)
	{
		switch (position)
		{
		case 65536:
			hkpic = new sxlwf();
			lufzx = new rwknq(hkpic, 0, rmkkr.osptv);
			break;
		case 65538:
			effdi = new rwolq();
			lufzx = new rwknq(effdi, 2, rmkkr.zkxoz);
			break;
		default:
			lufzx = rillp.yeukt;
			break;
		}
	}

	public lnabj scqbh()
	{
		return lufzx;
	}
}
