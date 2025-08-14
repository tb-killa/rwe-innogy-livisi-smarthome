using System;
using System.Collections;
using System.Collections.Generic;
using onrkn;

namespace Rebex;

public static class Licensing
{
	private class ubjiw : ICollection<string>, IEnumerable<string>, IEnumerable
	{
		private readonly List<string> yutvz;

		public object biojb => yutvz;

		public int Count
		{
			get
			{
				lock (yutvz)
				{
					return yutvz.Count;
				}
			}
		}

		public bool IsReadOnly => false;

		public ubjiw()
		{
			yutvz = new List<string>();
		}

		public void Add(string item)
		{
			lock (yutvz)
			{
				fwwdw fwwdw = ongpx.plgbf(item, item);
				if (fwwdw != null && 0 == 0)
				{
					throw new InvalidOperationException(fwwdw.Message);
				}
				yutvz.Add(item);
			}
		}

		public void Clear()
		{
			lock (yutvz)
			{
				using (List<string>.Enumerator enumerator = yutvz.GetEnumerator())
				{
					while (enumerator.MoveNext() ? true : false)
					{
						string current = enumerator.Current;
						ongpx.foqfm(current);
					}
				}
				yutvz.Clear();
			}
		}

		public bool Contains(string item)
		{
			lock (yutvz)
			{
				return yutvz.Contains(item);
			}
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			lock (yutvz)
			{
				yutvz.CopyTo(array, arrayIndex);
			}
		}

		public IEnumerator<string> GetEnumerator()
		{
			lock (yutvz)
			{
				return new List<string>(yutvz).GetEnumerator();
			}
		}

		public bool Remove(string item)
		{
			lock (yutvz)
			{
				ongpx.foqfm(item);
				return yutvz.Remove(item);
			}
		}

		private IEnumerator erxxz()
		{
			return GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in erxxz
			return this.erxxz();
		}
	}

	private static string lnohm;

	private static readonly ubjiw phlxe = new ubjiw();

	public static string Key
	{
		get
		{
			lock (phlxe.biojb)
			{
				return lnohm;
			}
		}
		set
		{
			lock (phlxe.biojb)
			{
				if (string.Equals(lnohm, value, StringComparison.Ordinal))
				{
					return;
				}
				lnohm = value;
				if (value != null && 0 == 0)
				{
					value = value.Trim('=').Trim();
					if (value.Length == 0 || 1 == 0)
					{
						value = null;
					}
				}
				if (value != null && 0 == 0)
				{
					ongpx.plgbf("", value);
				}
				else
				{
					ongpx.foqfm("");
				}
			}
		}
	}

	public static ICollection<string> Keys => phlxe;
}
