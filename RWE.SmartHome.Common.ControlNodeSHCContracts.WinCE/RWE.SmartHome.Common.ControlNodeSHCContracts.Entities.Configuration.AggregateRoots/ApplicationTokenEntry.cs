using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums;

namespace RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.AggregateRoots;

public class ApplicationTokenEntry
{
	[XmlAttribute]
	public string Name { get; set; }

	[XmlAttribute]
	public string AppId { get; set; }

	[XmlAttribute]
	public string ApplicationUrl { get; set; }

	[XmlAttribute]
	public string SHCPackageChecksum { get; set; }

	[XmlAttribute]
	public string ShcPackageFilename { get; set; }

	[XmlAttribute]
	public string SlPackageChecksum { get; set; }

	[XmlAttribute]
	public string Version { get; set; }

	public List<ApplicationParameter> Parameters { get; set; }

	[XmlAttribute]
	public bool IsService { get; set; }

	[XmlAttribute]
	public bool IsEnabledByUser { get; set; }

	[XmlAttribute]
	public bool IsEnabledByWebshop { get; set; }

	public DateTime? ExpirationDate { get; set; }

	[XmlAttribute]
	public bool UpdateAvailable { get; set; }

	[XmlIgnore]
	public bool IsEnabled => IsEnabledByUser && IsEnabledByWebshop;

	[XmlAttribute]
	public string FullyQualifiedTypeName { get; set; }

	[XmlAttribute]
	public bool ActiveOnShc { get; set; }

	[XmlAttribute]
	public string AppManifest { get; set; }

	public ApplicationKind ApplicationKind { get; set; }

	public ApplicationTokenEntry Clone()
	{
		ApplicationTokenEntry applicationTokenEntry = new ApplicationTokenEntry();
		applicationTokenEntry.ActiveOnShc = ActiveOnShc;
		applicationTokenEntry.AppId = AppId;
		applicationTokenEntry.ApplicationUrl = ApplicationUrl;
		applicationTokenEntry.AppManifest = AppManifest;
		applicationTokenEntry.ApplicationKind = ApplicationKind;
		applicationTokenEntry.IsEnabledByUser = IsEnabledByUser;
		applicationTokenEntry.IsEnabledByWebshop = IsEnabledByWebshop;
		applicationTokenEntry.IsService = IsService;
		applicationTokenEntry.FullyQualifiedTypeName = FullyQualifiedTypeName;
		applicationTokenEntry.ExpirationDate = ExpirationDate;
		applicationTokenEntry.Name = Name;
		applicationTokenEntry.SHCPackageChecksum = SHCPackageChecksum;
		applicationTokenEntry.ShcPackageFilename = ShcPackageFilename;
		applicationTokenEntry.SlPackageChecksum = SlPackageChecksum;
		applicationTokenEntry.UpdateAvailable = UpdateAvailable;
		applicationTokenEntry.Version = Version;
		applicationTokenEntry.Parameters = new List<ApplicationParameter>();
		ApplicationTokenEntry applicationTokenEntry2 = applicationTokenEntry;
		if (Parameters != null)
		{
			foreach (ApplicationParameter parameter in Parameters)
			{
				applicationTokenEntry2.Parameters.Add(parameter.Clone());
			}
		}
		return applicationTokenEntry2;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder(Name);
		stringBuilder.Append(ActiveOnShc);
		stringBuilder.Append(AppId);
		stringBuilder.Append(ApplicationUrl);
		stringBuilder.Append(AppManifest);
		stringBuilder.Append(ApplicationKind);
		stringBuilder.Append(IsEnabledByUser);
		stringBuilder.Append(IsEnabledByWebshop);
		stringBuilder.Append(IsService);
		stringBuilder.Append(FullyQualifiedTypeName);
		stringBuilder.Append(ExpirationDate);
		stringBuilder.Append(SHCPackageChecksum);
		stringBuilder.Append(ShcPackageFilename);
		stringBuilder.Append(SlPackageChecksum);
		stringBuilder.Append(UpdateAvailable);
		stringBuilder.Append(Version);
		if (Parameters != null)
		{
			foreach (ApplicationParameter parameter in Parameters)
			{
				stringBuilder.Append(parameter.Key);
				stringBuilder.Append(parameter.Value);
			}
		}
		return stringBuilder.ToString();
	}
}
