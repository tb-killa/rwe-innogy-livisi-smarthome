using System.Runtime.InteropServices;

namespace System.Reflection;

[ComVisible(true)]
[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
public sealed class AssemblyFileVersionAttribute : Attribute
{
	public string Version { get; set; }

	public AssemblyFileVersionAttribute(string version)
	{
		Version = version;
	}
}
