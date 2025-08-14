using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using Rebex;
using Rebex.Security.Cryptography;

namespace onrkn;

internal abstract class nyxyz
{
	private const string rsbxw = "\\Program Files\\Rebex";

	private const string rvwok = "SOFTWARE\\Rebex";

	private static readonly byte[] ayyoi = new byte[1] { 32 };

	private static readonly byte[] fzhaw = new byte[1] { 10 };

	private readonly object zdfcv = new object();

	private readonly string qyisl;

	private readonly bool migkt;

	protected abstract string qpwep { get; }

	protected abstract string eqrzr { get; }

	protected abstract string lzkgn { get; }

	private string aypyz => Path.Combine(qyisl, qpwep);

	protected nyxyz(string cachePathRoot)
	{
		if (cachePathRoot == null || 1 == 0)
		{
			migkt = true;
			qyisl = "\\Program Files\\Rebex";
		}
		else
		{
			qyisl = cachePathRoot;
		}
		if (!Directory.Exists(aypyz) || 1 == 0)
		{
			Directory.CreateDirectory(aypyz);
		}
	}

	private string mvrmb(string p0)
	{
		return Path.Combine(aypyz, p0);
	}

	private void fymic(Stream p0, string p1, awngk p2)
	{
		string text = mvrmb(p1);
		try
		{
			Stream stream = new FileStream(text, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
			try
			{
				try
				{
					p0.alskc(stream);
				}
				finally
				{
					stream.Flush();
				}
			}
			finally
			{
				if (stream != null && 0 == 0)
				{
					((IDisposable)stream).Dispose();
				}
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			p2.byfnx(LogLevel.Error, eqrzr, "Unable to save {2} to '{0}': {1}", text, ex, lzkgn);
		}
		catch (IOException ex2)
		{
			p2.byfnx(LogLevel.Error, eqrzr, "Unable to save {2} to '{0}': {1}", text, ex2, lzkgn);
		}
	}

	private Stream qixkb(string p0, awngk p1)
	{
		string text = mvrmb(p0);
		try
		{
			return File.Open(text, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
		catch (UnauthorizedAccessException ex)
		{
			p1.byfnx(LogLevel.Error, eqrzr, "Unable to load {2} from '{0}': {1}", text, ex, lzkgn);
			return null;
		}
		catch (IOException ex2)
		{
			if (ex2 is DirectoryNotFoundException || ex2 is FileNotFoundException)
			{
				p1.byfnx(LogLevel.Error, eqrzr, "Unable to load {3} from '{0}' ({1}): {2}", text, ((object)ex2).GetType().FullName, ex2.Message, lzkgn);
			}
			else
			{
				p1.byfnx(LogLevel.Error, eqrzr, "Unable to load {2} from '{0}': {1}", text, ex2, lzkgn);
			}
			return null;
		}
	}

	public void vrgug(string p0, Stream p1, awngk p2)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("url");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "url");
		}
		if (p1 == null || 1 == 0)
		{
			throw new ArgumentNullException("data");
		}
		lock (zdfcv)
		{
			string text = ((migkt ? true : false) ? boimv(p0, p2, p2: true) : drhdd(p0, p2, p2: true));
			if (!string.IsNullOrEmpty(text))
			{
				fymic(p1, text, p2);
			}
		}
	}

	public void raovz(string p0, awngk p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("url");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "url");
		}
		lock (zdfcv)
		{
			string text = ((migkt ? true : false) ? boimv(p0, p1, p2: false, p3: true) : drhdd(p0, p1, p2: false, p3: true));
			if (string.IsNullOrEmpty(text) && 0 == 0)
			{
				return;
			}
			string text2 = mvrmb(text);
			try
			{
				File.Delete(text2);
			}
			catch (UnauthorizedAccessException ex)
			{
				p1.byfnx(LogLevel.Error, eqrzr, "Unable to delete {2} '{0}': {1}", text2, ex, lzkgn);
			}
			catch (IOException ex2)
			{
				p1.byfnx(LogLevel.Error, eqrzr, "Unable to delete {2} '{0}': {1}", text2, ex2, lzkgn);
			}
		}
	}

	public Stream xwedh(string p0, awngk p1)
	{
		if (p0 == null || 1 == 0)
		{
			throw new ArgumentNullException("url");
		}
		if (p0.Length == 0 || 1 == 0)
		{
			throw new ArgumentException("String cannot have zero length.", "url");
		}
		lock (zdfcv)
		{
			string text = ((migkt ? true : false) ? boimv(p0, p1, p2: false) : drhdd(p0, p1, p2: false));
			if (string.IsNullOrEmpty(text) && 0 == 0)
			{
				return null;
			}
			return qixkb(text, p1);
		}
	}

	private string nraxe()
	{
		return Guid.NewGuid().ToString().ToLower(CultureInfo.InvariantCulture);
	}

	private string boimv(string p0, awngk p1, bool p2, bool p3 = false)
	{
		string text = Path.Combine("SOFTWARE\\Rebex", qpwep);
		if (p2 && 0 == 0)
		{
			RegistryKey registryKey = Registry.CurrentUser.CreateSubKey(text);
			try
			{
				if (registryKey == null || 1 == 0)
				{
					p1.byfnx(LogLevel.Error, eqrzr, "Unable to create/open registry key '{0}' for {2} '{1}'.", text, p0, lzkgn);
					return null;
				}
				string text2 = registryKey.GetValue(p0) as string;
				if (string.IsNullOrEmpty(text2) && 0 == 0)
				{
					text2 = nraxe();
					registryKey.SetValue(p0, text2, RegistryValueKind.String);
				}
				return text2;
			}
			finally
			{
				if (registryKey != null && 0 == 0)
				{
					((IDisposable)registryKey).Dispose();
				}
			}
		}
		RegistryKey registryKey2 = Registry.CurrentUser.OpenSubKey(text, p3);
		try
		{
			if (registryKey2 == null || 1 == 0)
			{
				return null;
			}
			string result = registryKey2.GetValue(p0) as string;
			if (p3 && 0 == 0)
			{
				registryKey2.DeleteValue(p0, throwOnMissingValue: false);
			}
			return result;
		}
		finally
		{
			if (registryKey2 != null && 0 == 0)
			{
				((IDisposable)registryKey2).Dispose();
			}
		}
	}

	private string drhdd(string p0, awngk p1, bool p2, bool p3 = false)
	{
		string p4 = brgjd.wlvqq(HashingAlgorithm.ComputeHash(HashingAlgorithmId.SHA1, EncodingTools.UTF8.GetBytes(p0)));
		string text = mvrmb(p4);
		string result = null;
		try
		{
			if (!File.Exists(text) || 1 == 0)
			{
				if (!p2 || 1 == 0)
				{
					return null;
				}
			}
			else
			{
				List<string> list = new List<string>();
				FileAccess p5 = ((!p3 || 1 == 0) ? FileAccess.Read : FileAccess.ReadWrite);
				FileShare p6 = ((!p3 || 1 == 0) ? FileShare.Read : FileShare.None);
				FileStream fileStream = vtdxm.vswch(text, FileMode.Open, p5, p6);
				try
				{
					StreamReader streamReader = new StreamReader(new npohs(fileStream, leaveOpen: true), EncodingTools.UTF8);
					try
					{
						for (string text2 = streamReader.ReadLine(); text2 != null; text2 = streamReader.ReadLine())
						{
							if (!brgjd.qwnqu(text2) || 1 == 0)
							{
								int num = text2.LastIndexOf(' ');
								if (num > 0 && text2.Substring(0, num) == p0 && 0 == 0)
								{
									result = text2.Substring(num + 1).TrimEnd('\r');
									if (!p3 || 1 == 0)
									{
										return result;
									}
								}
								else if (p3 && 0 == 0)
								{
									list.Add(text2.TrimEnd('\r'));
								}
							}
						}
					}
					finally
					{
						if (streamReader != null && 0 == 0)
						{
							((IDisposable)streamReader).Dispose();
						}
					}
					byte[] array;
					int num2;
					int num3;
					if (p3 && 0 == 0 && list.Count > 0)
					{
						fileStream.Position = 0L;
						bool flag = false;
						long length = fileStream.Length;
						try
						{
							fileStream.SetLength(0L);
						}
						catch (NotSupportedException)
						{
							flag = true;
						}
						using (List<string>.Enumerator enumerator = list.GetEnumerator())
						{
							while (enumerator.MoveNext() ? true : false)
							{
								string current = enumerator.Current;
								fileStream.xnred(EncodingTools.UTF8.GetBytes(current));
								fileStream.xnred(fzhaw);
							}
						}
						if (flag && 0 == 0)
						{
							num2 = (int)(length - fileStream.Position);
							if (num2 > 0)
							{
								num2 = Math.Min(num2, fzhaw.Length);
								array = new byte[num2];
								num2 -= 2;
								num3 = 0;
								if (num3 != 0)
								{
									goto IL_0248;
								}
								goto IL_0255;
							}
						}
					}
					goto end_IL_0084;
					IL_0255:
					if (num3 < num2)
					{
						goto IL_0248;
					}
					fzhaw.CopyTo(array, num2);
					fileStream.xnred(array);
					goto end_IL_0084;
					IL_0248:
					array[num3] = 32;
					num3++;
					goto IL_0255;
					end_IL_0084:;
				}
				finally
				{
					if (fileStream != null && 0 == 0)
					{
						((IDisposable)fileStream).Dispose();
					}
				}
				if (p3 && 0 == 0 && (list.Count == 0 || 1 == 0))
				{
					File.Delete(text);
				}
				if (!p2 || 1 == 0)
				{
					return result;
				}
			}
			FileStream fileStream2 = vtdxm.vswch(text, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
			try
			{
				result = nraxe();
				fileStream2.Seek(0L, SeekOrigin.End);
				fileStream2.xnred(EncodingTools.UTF8.GetBytes(p0));
				fileStream2.xnred(ayyoi);
				fileStream2.xnred(EncodingTools.UTF8.GetBytes(result));
				fileStream2.xnred(fzhaw);
				return result;
			}
			finally
			{
				if (fileStream2 != null && 0 == 0)
				{
					((IDisposable)fileStream2).Dispose();
				}
			}
		}
		catch (UnauthorizedAccessException ex2)
		{
			if (p2 && 0 == 0)
			{
				p1.byfnx(LogLevel.Error, eqrzr, "Unable to create/open file '{0}' for {2} '{1}' due to: {3}", text, p0, lzkgn, ex2);
			}
			return null;
		}
		catch (IOException ex3)
		{
			if (p2 && 0 == 0)
			{
				p1.byfnx(LogLevel.Error, eqrzr, "Unable to create/open file '{0}' for {2} '{1}' due to: {3}", text, p0, lzkgn, ex3);
			}
			return null;
		}
	}
}
