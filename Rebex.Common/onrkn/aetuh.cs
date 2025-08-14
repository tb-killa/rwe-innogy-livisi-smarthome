using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace onrkn;

internal static class aetuh
{
	private sealed class bhhrn<T0> : IEnumerable<T0>, IEnumerable, IEnumerator<T0>, IEnumerator, IDisposable
	{
		private T0 pvsve;

		private int axutc;

		private int wbcna;

		public T0 uhhyd;

		public T0 yqlgh;

		private T0 gybya => pvsve;

		private object wfjgq => pvsve;

		private IEnumerator<T0> ccvki()
		{
			bhhrn<T0> bhhrn;
			if (Thread.CurrentThread.ManagedThreadId == wbcna && axutc == -2)
			{
				axutc = 0;
				bhhrn = this;
			}
			else
			{
				bhhrn = new bhhrn<T0>(0);
			}
			bhhrn.uhhyd = yqlgh;
			return bhhrn;
		}

		IEnumerator<T0> IEnumerable<T0>.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ccvki
			return this.ccvki();
		}

		private IEnumerator eohfw()
		{
			return ccvki();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			//ILSpy generated this explicit interface implementation from .override directive in eohfw
			return this.eohfw();
		}

		private bool fxgud()
		{
			switch (axutc)
			{
			case 0:
				axutc = -1;
				pvsve = uhhyd;
				axutc = 1;
				return true;
			case 1:
				axutc = -1;
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in fxgud
			return this.fxgud();
		}

		private void pmryp()
		{
			throw new NotSupportedException();
		}

		void IEnumerator.Reset()
		{
			//ILSpy generated this explicit interface implementation from .override directive in pmryp
			this.pmryp();
		}

		private void ripdj()
		{
		}

		void IDisposable.Dispose()
		{
			//ILSpy generated this explicit interface implementation from .override directive in ripdj
			this.ripdj();
		}

		public bhhrn(int _003C_003E1__state)
		{
			axutc = _003C_003E1__state;
			wbcna = Thread.CurrentThread.ManagedThreadId;
		}
	}

	public static IEnumerable<TInput> rxpii<TInput>(this TInput p0)
	{
		bhhrn<TInput> bhhrn = new bhhrn<TInput>(-2);
		bhhrn.yqlgh = p0;
		return bhhrn;
	}
}
