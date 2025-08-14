namespace onrkn;

internal class mjsvv : vclqf
{
	protected override void pypmh(byte[] p0, int p1)
	{
		uint p2 = tfugy;
		uint num = xpwzo;
		uint num2 = jvpsp;
		uint num3 = mvysi;
		int num4 = 0;
		if (num4 != 0)
		{
			goto IL_0027;
		}
		goto IL_0044;
		IL_0027:
		nfnsv[num4] = vclqf.bbjej(p0, p1);
		num4++;
		p1 += 4;
		goto IL_0044;
		IL_0044:
		if (num4 >= nfnsv.Length)
		{
			uint[] array = nfnsv;
			p2 = yyppv(p2, num, num2, num3, array[0], 3);
			num3 = yyppv(num3, p2, num, num2, array[1], 7);
			num2 = yyppv(num2, num3, p2, num, array[2], 11);
			num = yyppv(num, num2, num3, p2, array[3], 19);
			p2 = yyppv(p2, num, num2, num3, array[4], 3);
			num3 = yyppv(num3, p2, num, num2, array[5], 7);
			num2 = yyppv(num2, num3, p2, num, array[6], 11);
			num = yyppv(num, num2, num3, p2, array[7], 19);
			p2 = yyppv(p2, num, num2, num3, array[8], 3);
			num3 = yyppv(num3, p2, num, num2, array[9], 7);
			num2 = yyppv(num2, num3, p2, num, array[10], 11);
			num = yyppv(num, num2, num3, p2, array[11], 19);
			p2 = yyppv(p2, num, num2, num3, array[12], 3);
			num3 = yyppv(num3, p2, num, num2, array[13], 7);
			num2 = yyppv(num2, num3, p2, num, array[14], 11);
			num = yyppv(num, num2, num3, p2, array[15], 19);
			p2 = hvxpu(p2, num, num2, num3, array[0], 3);
			num3 = hvxpu(num3, p2, num, num2, array[4], 5);
			num2 = hvxpu(num2, num3, p2, num, array[8], 9);
			num = hvxpu(num, num2, num3, p2, array[12], 13);
			p2 = hvxpu(p2, num, num2, num3, array[1], 3);
			num3 = hvxpu(num3, p2, num, num2, array[5], 5);
			num2 = hvxpu(num2, num3, p2, num, array[9], 9);
			num = hvxpu(num, num2, num3, p2, array[13], 13);
			p2 = hvxpu(p2, num, num2, num3, array[2], 3);
			num3 = hvxpu(num3, p2, num, num2, array[6], 5);
			num2 = hvxpu(num2, num3, p2, num, array[10], 9);
			num = hvxpu(num, num2, num3, p2, array[14], 13);
			p2 = hvxpu(p2, num, num2, num3, array[3], 3);
			num3 = hvxpu(num3, p2, num, num2, array[7], 5);
			num2 = hvxpu(num2, num3, p2, num, array[11], 9);
			num = hvxpu(num, num2, num3, p2, array[15], 13);
			p2 = fbbso(p2, num, num2, num3, array[0], 3);
			num3 = fbbso(num3, p2, num, num2, array[8], 9);
			num2 = fbbso(num2, num3, p2, num, array[4], 11);
			num = fbbso(num, num2, num3, p2, array[12], 15);
			p2 = fbbso(p2, num, num2, num3, array[2], 3);
			num3 = fbbso(num3, p2, num, num2, array[10], 9);
			num2 = fbbso(num2, num3, p2, num, array[6], 11);
			num = fbbso(num, num2, num3, p2, array[14], 15);
			p2 = fbbso(p2, num, num2, num3, array[1], 3);
			num3 = fbbso(num3, p2, num, num2, array[9], 9);
			num2 = fbbso(num2, num3, p2, num, array[5], 11);
			num = fbbso(num, num2, num3, p2, array[13], 15);
			p2 = fbbso(p2, num, num2, num3, array[3], 3);
			num3 = fbbso(num3, p2, num, num2, array[11], 9);
			num2 = fbbso(num2, num3, p2, num, array[7], 11);
			num = fbbso(num, num2, num3, p2, array[15], 15);
			tfugy += p2;
			xpwzo += num;
			jvpsp += num2;
			mvysi += num3;
			return;
		}
		goto IL_0027;
	}

	private static uint yyppv(uint p0, uint p1, uint p2, uint p3, uint p4, int p5)
	{
		return vclqf.prlgj(p0 + ((p1 & p2) | (~p1 & p3)) + p4, p5);
	}

	private static uint hvxpu(uint p0, uint p1, uint p2, uint p3, uint p4, int p5)
	{
		return vclqf.prlgj(p0 + ((p1 & p2) | (p1 & p3) | (p2 & p3)) + p4 + 1518500249, p5);
	}

	private static uint fbbso(uint p0, uint p1, uint p2, uint p3, uint p4, int p5)
	{
		return vclqf.prlgj(p0 + (p1 ^ p2 ^ p3) + p4 + 1859775393, p5);
	}
}
