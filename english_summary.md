# SHC v1 NAND analysis - structured technical notes (English)








## Table of contents
- [Product context (what it is, who made it, and current state)](#product-context-what-it-is-who-made-it-and-current-state)
- [Hardware](#hardware)
- [Software stack](#software-stack)
- [RF protocols and device communication (from decompiled code)](#rf-protocols-and-device-communication-from-decompiled-code)
- [Protocol summary: BidCoS vs CoSIP (from code)](#protocol-summary-bidcos-vs-cosip-from-code)
- [Protocol usage by device type (from code)](#protocol-usage-by-device-type-from-code)
- [BidCoS vs SIPcos side-by-side (technical)](#bidcos-vs-sipcos-side-by-side-technical)
- [Authorship hints (from code metadata)](#authorship-hints-from-code-metadata)
- [BidCoS key usage and validation (v1)](#bidcos-key-usage-and-validation-v1)
- [External context on RF protocols and vendor (public sources)](#external-context-on-rf-protocols-and-vendor-public-sources)
- [NAND acquisition scope and integrity](#nand-acquisition-scope-and-integrity)
- [ROM/XIP extraction](#romxip-extraction)
- [Persistent registry (EKIM)](#persistent-registry-ekim)
- [ROM/XIP module metadata](#romxip-module-metadata)
- [KeyVault XML (full content)](#keyvault-xml-full-content)
- [Certificates (OpenSSL output)](#certificates-openssl-output)
- [Certificate carving results (dump scan)](#certificate-carving-results-dump-scan)
- [Default user PFX (Cluj-SMARTHOME-SHCDefault00001.pfx)](#default-user-pfx-cluj-smarthome-shcdefault00001pfx)
- [Registry hive certificate stores (hvtool dump)](#registry-hive-certificate-stores-hvtool-dump)
- [CE certificate store carving (raw NAND)](#ce-certificate-store-carving-raw-nand)
- [Certificate validity impact (Not After)](#certificate-validity-impact-not-after)
- [Certificate expiry impact (what would break)](#certificate-expiry-impact-what-would-break)
- [Certificate expiry impact on device key crypto](#certificate-expiry-impact-on-device-key-crypto)
- [KeyVault crypto flow (software view)](#keyvault-crypto-flow-software-view)
- [Device keys (software view)](#device-keys-software-view)
- [DeviceKey structure and crypto path (CSV)](#devicekey-structure-and-crypto-path-csv)
- [DeviceKey encode/decode path (from code)](#devicekey-encodedecode-path-from-code)
- [DeviceKey CSV production (how rows are generated)](#devicekey-csv-production-how-rows-are-generated)
- [SGTIN generation and serial mapping (code and examples)](#sgtin-generation-and-serial-mapping-code-and-examples)
- [DeviceKey lifecycle (readable flow)](#devicekey-lifecycle-readable-flow)
- [KeyVault to DeviceKey chain (readable flow)](#keyvault-to-devicekey-chain-readable-flow)
- [Log export crypto flow (readable)](#log-export-crypto-flow-readable)
- [DeviceKey CSV structure (fields and format)](#devicekey-csv-structure-fields-and-format)
- [Base64 ciphertext analysis (sanitized)](#base64-ciphertext-analysis-sanitized)
- [Why ECB was used (and what that implies)](#why-ecb-was-used-and-what-that-implies)
- [AES construction details (what the code does)](#aes-construction-details-what-the-code-does)
- [Device key validation behavior](#device-key-validation-behavior)
- [Log export and encryption (USB)](#log-export-and-encryption-usb)
- [Log encryption defaults (what the code implies)](#log-encryption-defaults-what-the-code-implies)
- [Log decryption key availability (dump view)](#log-decryption-key-availability-dump-view)
- [Log encryption and the master key (clarification)](#log-encryption-and-the-master-key-clarification)
- [Logging endpoints and storage (local)](#logging-endpoints-and-storage-local)
- [settings.config and boot.config (USB update package + dump correlation)](#settingsconfig-and-bootconfig-usb-update-package-dump-correlation)
- [Device key export (USB CSV exists)](#device-key-export-usb-csv-exists)
- [Software APIs and internal services (overview)](#software-apis-and-internal-services-overview)
- [Backend/API endpoints and payloads (v1)](#backendapi-endpoints-and-payloads-v1)
- [DNS status of backend domains](#dns-status-of-backend-domains)
- [shc_api.dll (native flash/registry bridge)](#shcapidll-native-flashregistry-bridge)
- [Internal services and classes (selected map)](#internal-services-and-classes-selected-map)
- [Internal API map (high-level)](#internal-api-map-high-level)
- [Software resources (what is embedded)](#software-resources-what-is-embedded)
- [DnsService.exe (native Bonjour/mDNS service)](#dnsserviceexe-native-bonjourmdns-service)
- [Important storage paths (observed)](#important-storage-paths-observed)
- [Startup flow (high-level)](#startup-flow-high-level)
- [Update pipeline (software view)](#update-pipeline-software-view)
- [Default update behavior (community documentation, Classic SHC v1 only)](#default-update-behavior-community-documentation-classic-shc-v1-only)
- [Firmware update validation (managed code)](#firmware-update-validation-managed-code)
- [Application update handling (USB ZIP path, managed code)](#application-update-handling-usb-zip-path-managed-code)
- [Local operation without cloud (technical summary)](#local-operation-without-cloud-technical-summary)
- [Native validation (unknown scope)](#native-validation-unknown-scope)
- [Software security observations (managed layer)](#software-security-observations-managed-layer)

## Product context (what it is, who made it, and current state)
The SHC v1 (Classic SmartHome Controller) is the central unit from the RWE/innogy/LIVISI SmartHome product line. It is a Windows Embedded CE 6.0 appliance designed for local device control, with optional cloud services historically used for updates and remote access. After March 1, 2024, cloud-centric operation is no longer the primary path; community guidance focuses on local operation and USB-based updates for legacy devices.

In practice, this means the controller can still be used locally, but update delivery and remote access depend on local tooling and community-provided integrations rather than vendor cloud services.

## Hardware
- SoC: AT91SAM9G20
- TPM: AT97SC3204 (TPM CSP present in registry)
- NAND: K9F2G08U0C
- OS: Windows Embedded CE 6.0

Why this matters: the TPM implies that private keys can be hardware-bound and non-exportable. That is consistent with what we see in the dump (public material only).

---

## Software stack
- Boot/ROM: WinCE XIP/ROM image
- Managed layer: .NET CF assemblies
- Native layer: `shc_api.dll` and platform-specific DLLs

The managed layer handles KeyVault parsing, device key decryption, and update orchestration. If stronger validation exists (e.g., image signature checks), it is likely in native code or enforced by the backend.

---

## RF protocols and device communication (from decompiled code)
The decompiled solution includes several protocol stacks and a SerialAPI layer that routes RF frames to higher-level handlers. Key indicators:

- **BidCos/SerialAPI**: `SerialAPI` classes (`BIDCOSHeader`, `BidCosHandler2`, `BIDCOSNode`) indicate the BidCos protocol family, commonly associated with eQ-3 hardware. This supports the hypothesis that eQ-3 is the original RF stack/vendor base for part of the device ecosystem.
- **SIPcos**: A full SIPcos handler layer (`SIPcosHeader`, `SIPcosHandler`, `SIPcosFrameType`) suggests a specific frame format with command handlers and security modes.
- **wMBus**: A dedicated wMBus protocol adapter module exists, implying support for wireless M-Bus devices.
- **Lemonbeat**: A full Lemonbeat domain model and protocol adapter are present, covering actions, status reports, and state machines.

These stacks are orchestrated through the device communication modules and the protocol multiplexer. This indicates a multi-protocol RF environment where different device families coexist behind a unified control plane.

Board pinout note (community CSV):

- We pulled `RWE_Zentrale_Pinbelegung.csv` and found **ST400** listed as `PB5 (RXD0)` and `PB4 (TXD0)`, which maps to the main CPU UART0 pins.
- The same file lists **SPI0** on **ST101** (`PA0/PA1/PA2/PA3` for MISO/MOSI/SPCK/NPCS0).
- The CSV does **not** mention TRX868/CC1101 or explicit GDO0/GDO2 lines, so their exact routing is not confirmed by this file alone.

AT91 pin mapping (from the same CSV):

- **ST400 / UART0**: `PB5` = `RXD0`, `PB4` = `TXD0`
- **ST101 / SPI0**: `PA0` = `SPI0_MISO`, `PA1` = `SPI0_MOSI`, `PA2` = `SPI0_SPCK`, `PA3` = `SPI0_NPCS0`

Community hardware note (unverified):

- A forum post suggests **ST400** is the serial interface to the AVR, **PRG1** is the ISP programming header, and the **TRX868/CC1101** RF module is wired to the AVR via SPI plus two GPIOs (GDO0/GDO2).
- This aligns with the AVR firmware observation that SPI-style pins are used and additional GPIOs are toggled, but it is **not confirmed** without a schematic or board trace.

---

## Protocol summary: BidCoS vs CoSIP (from code)
The decompiled code clearly separates **BidCoS** and **CoSIP/SIPcos** as two different RF protocol stacks. BidCoS is the HomeMatic family, while CoSIP is a separate stack built on the CORESTACK header format with routing and MAC security.

### BidCoS (HomeMatic family)
Evidence and structure:
- Dedicated BidCoS header parser with a 9-byte header (frame counter, header bits, frame type, 3-byte sender, 3-byte receiver).
- BidCoS is used for specific eQ-3 device types (smoke detectors and siren in the codebase).
- BidCoS messages can be sent via the SIPcos transport, but the BidCoS frame format remains distinct.

Code fragments:

```csharp
// BidCoS header parsing (9 bytes)
private byte m_frameCounter;
private BIDCOSHeaderBitField m_headerBits;
private BIDCOSFrameType m_frameType;
private byte[] m_sender;
private byte[] m_receiver;

public bool Parse(ref List<byte> message)
{
    if (message.Count >= 9)
    {
        m_frameCounter = message[0];
        m_headerBits = (BIDCOSHeaderBitField)message[1];
        m_frameType = (BIDCOSFrameType)message[2];
        m_sender = new byte[3] { message[3], message[4], message[5] };
        m_receiver = new byte[3] { message[6], message[7], message[8] };
        message.RemoveRange(0, 9);
        return true;
    }
    return false;
}
```

```csharp
// BidCoS device types in sysinfo frame mapping
case 66:  deviceType = BIDCOSDeviceType.Eq3BasicSmokeDetector; break;
case 170: deviceType = BIDCOSDeviceType.Eq3EncryptedSmokeDetector; break;
case 249: deviceType = BIDCOSDeviceType.Eq3EncryptedSiren; break;
```

Where it appears:
- `SerialAPI/BIDCOSHeader.cs`
- `SerialAPI/BidCoSFrames/BIDCOSSysinfoFrame.cs`
- `SerialAPI/BidCosLayer/*`

### CoSIP / SIPcos
Evidence and structure:
- SIPcos uses a CORESTACK header with routing, MAC security, and frame types.
- SIPcos adds its own 2-byte header (frame type + flags + sequence number) after CORESTACK.
- The frame type space includes configuration, status, routing, firmware update, switch commands, and time information.

Code fragments:

```csharp
// CORESTACK header parse (routing + MAC security)
m_frameType = (CorestackFrameType)((data[0] & 0x1C) >> 2);
m_macSecurity = (data[0] & 0x20) == 32;
...
data.CopyTo(2, m_macDestination, 0, 3);
data.CopyTo(5, m_macSource, 0, 3);
...
if (m_macSecurity)
{
    data.CopyTo(8, m_sequence_number, 0, 4);
    data.CopyTo(12, m_mic, 0, 4);
}
```

```csharp
// SIPcos header (2 bytes after CORESTACK header)
m_sipcosFrameType = (SIPcosFrameType)(data[0] & 0x3F);
m_stayAwake = (data[0] & 0x40) == 64;
m_bidi = (data[0] & 0x80) == 128;
m_sequenceNumber = data[1];
```

```csharp
// SIPcos frame type map
public enum SIPcosFrameType : byte
{
    NETWORK_MANAGEMENT_FRAME = 0,
    ROUTE_MANAGEMENT = 1,
    FIRMWARE_UPDATE = 15,
    ANSWER = 16,
    CONFIGURATION = 17,
    STATUSINFO = 18,
    TIMESLOT_CC = 19,
    DIRECT_EXECUTION = 20,
    TIME_INFORMATION = 21,
    UNCONDITIONAL_SWITCH_COMMAND = 22,
    CONDITIONAL_SWITCH_COMMAND = 23,
    LEVEL_COMMAND = 24,
    VIRTUAL_BIDCOS_COMMAND = 119
}
```

Where it appears:
- `SerialAPI/CORESTACKHeader.cs`
- `SerialAPI/SIPcosHeader.cs`
- `SerialAPI/SIPcosFrameType.cs`
- `SipcosCommandHandler/*`

### Protocol selection signal
SIPcos device info frames explicitly declare which protocol a device uses.

```csharp
public enum DeviceInfoProtocolType : byte
{
    SIPcos,
    BIDcos
}
```

This is used to decide how inclusion and security handling proceed for each device.

---

### HomeMatic AES challenge-response equivalence (BidCoS)
The HomeMatic AES challenge-response algorithm described in public research matches the implementation here almost 1:1. The code builds a session key by XORing a padded 6-byte challenge with the AES key, then uses a two-step AES operation with an XOR step against the m-frame tail. This is used for BidCoS encrypted devices (e.g., WSD2 and Siren).

Code fragment:

```csharp
// session key from challenge (padded to 16 bytes) XOR AES key
sessionKey = XORArray(PadArray(challengeBytes, 16), aesKey);

// AES(payload) = Enc( Enc(random6||m[0..9]) XOR m[10..25] )
byte[] b = originalMessage.Take(10).ToArray();
byte[] input = AppendArray(random6Bytes, b);
byte[] a = Encrypt(input);
byte[] b2 = originalMessage.Skip(10).Take(16).ToArray();
byte[] input2 = XORArray(a, b2);
byte[] array = Encrypt(input2);
```

Where it is used:
- `SerialAPI/AesChallengeResponse.cs`
- `SerialAPI.BidCosLayer.DevicesSupport.Wsd2/ReceiveFrameHandlerAnswer.cs`
- `SerialAPI.BidCosLayer.DevicesSupport.Sir/ReceiveFrameHandlerAnswer.cs`

Reference (external): https://git.zerfleddert.de/hmcfgusb/AES/

---

## Protocol usage by device type (from code)
Based on the protocol-specific stacks and device mappings in this repo:

### BidCoS-only devices (explicit in BidCoS sysinfo/device adapters)
- **Eq3BasicSmokeDetector** (WSD)
- **Eq3EncryptedSmokeDetector** (WSD2)
- **Eq3EncryptedSiren** (SIR)

Evidence:
- `SerialAPI/BidCoSFrames/BIDCOSSysinfoFrame.cs`
- `SerialAPI/BidCosLayer/*`

### CoSIP/SIPcos devices (handled by SIPcos stacks/adapters)
These types are listed in the built-in device mapping and are handled through the SIPcos stack and protocol adapters:
- `RST`, `RST2` (radiator thermostats)
- `WRT` (room thermostat)
- `FSC8` (floor heating control)
- `PSS`, `PSSO` (pluggable switches)
- `WSC2` (wall controller)
- `BRC8` (basic remote)
- `WMD`, `WMDO` (motion detectors)
- `WDS` (door/window sensor)
- `PSD` (pluggable dimmer)
- `PSR` (router)
- `ISS2`, `ISD2`, `ISC2`, `ISR2` (in-wall devices)
- `RVA`, `ChargingStation`, `PresenceDevice` (other mapped types)

Evidence:
- `RWE.SmartHome.SHC.DeviceManagerInterfaces/PhysicalDeviceFactory.cs`
- `RWE.SmartHome.Common.ControlNodeSHCContracts.WinCE/RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums/BuiltinPhysicalDeviceType.cs`
- `RWE.SmartHome.SHC.SipCosProtocolAdapter/*`

Note: the protocol selection is driven by `DeviceInfoProtocolType` (SIPcos vs BIDcos) in device info frames.

---

## BidCoS vs SIPcos side-by-side (technical)
Readable comparison extracted from code:

| Aspect | BidCoS | SIPcos / CoSIP |
| --- | --- | --- |
| Header layout | 9-byte BidCoS header: frame counter, header bits, frame type, 3-byte sender, 3-byte receiver | CORESTACK header (routing + MAC security) + 2-byte SIPcos header (frame type + flags + sequence number) |
| Addressing | 3-byte sender/receiver only | MAC + IP fields, hop limit, and routed address extensions |
| Routing | None in header | Explicit routing modes: FIRST_ROUTED, IN_PATH, LAST_ROUTED |
| Security in header | No MAC security fields in BidCoS header | MAC security flag + MIC + sequence counter in CORESTACK |
| Challenge/response AES | Used for encrypted BidCoS devices (WSD2/Siren) | Not used in SIPcos flow |
| Frame types | BidCoS-specific types (separate enums) | SIPcos frame types: NETWORK_MANAGEMENT, ROUTE_MANAGEMENT, FIRMWARE_UPDATE, STATUSINFO, SWITCH, etc. |
| Stack location | `SerialAPI.BidCosLayer/*`, `SerialAPI.BidCoSFrames/*` | `SerialAPI/SIPcos*`, `SipcosCommandHandler/*` |

Naming note: the codebase uses **CoSIP** and **SIPcos/SIPcos** interchangeably for the same protocol stack. High-level types use `ProtocolIdentifier.Cosip`, while the low-level stack uses `SIPcos*` classes.

Code anchors:
- BidCoS header: `SerialAPI/BIDCOSHeader.cs`
- CORESTACK header: `SerialAPI/CORESTACKHeader.cs`
- SIPcos header/types: `SerialAPI/SIPcosHeader.cs`, `SerialAPI/SIPcosFrameType.cs`
- AES challenge/response: `SerialAPI/AesChallengeResponse.cs`

---

## Authorship hints (from code metadata)
There is no explicit "CoSIP authored by X" statement in the code. The strongest attribution signals are assembly metadata and namespaces:

- Many core SHC assemblies list `AssemblyCompany("Innogy SE")`, including SIPcos protocol adapter modules.
  - Example: `RWE.SmartHome.SHC.SipCosProtocolAdapter/Properties/AssemblyInfo.cs`
  - Example: `RWE.SmartHome.SHC.SipCos.TechnicalConfiguration/Properties/AssemblyInfo.cs`
- Some shared contracts/SDK assemblies list `AssemblyCompany("RWE")`.
  - Example: `RWE.SmartHome.Common.ControlNodeSHCContracts.WinCE/Properties/AssemblyInfo.cs`
- eQ-3 appears only as **device type naming**, not as protocol authorship.
  - Example: `RWE.SmartHome.SHC.DeviceManagerInterfaces/DeviceTypesEq3.cs`
  - Example: `SerialAPI/BidCoSFrames/BIDCOSDeviceType.cs`

Based on this repo alone, CoSIP/SIPcos appears to be part of the RWE/Innogy SmartHome software stack, while eQ-3 is referenced for specific device types (primarily in BidCoS paths).

---

## BidCoS key usage and validation (v1)
Key points from the v1 BidCoS stack:

- **SGTIN is the lookup key**, not the crypto key. It is used to locate the device key, but it does not derive it.
- **Serial number is display-only** (derived from SGTIN). It does not affect key material.
- **Device keys are stored encrypted** in `\\NandFlash\\DevicesKeysStorage.csv` and only decrypted after the KeyVault master key is unwrapped.
- **No plaintext key persistence** for BidCoS nodes. `DefaultKey` is `[XmlIgnore]` and never serialized.
- **Wsd2LocalKey is random per hub**, not a device key.
- **Validation is minimal** in the CSV path (Base64 check only).

BidCoS device types present in the v1 codebase (from `BIDCOSSysinfoFrame`):

| Device type | Sysinfo type code | Adapter | Key usage |
| --- | --- | --- | --- |
| Eq3BasicSmokeDetector (WSD1) | `0x42` (66) | `WsdAdapter` | No device key retrieval; default key logic is bypassed |
| Eq3EncryptedSmokeDetector (WSD2) | `0xAA` (170) | `Wsd2Adapter` | DeviceKey used for inclusion, then switch to local key |
| Eq3EncryptedSiren | `0xF9` (249) | `SirAdapter` | DeviceKey used for inclusion, then switch to local key |

```csharp
// BIDCOSSysinfoFrame: device type mapping from sysinfo
switch (m_deviceTypeNumber[1])
{
    case 66:
        deviceType = BIDCOSDeviceType.Eq3BasicSmokeDetector;
        itemReference = 91419;
        break;
    case 170:
        deviceType = BIDCOSDeviceType.Eq3EncryptedSmokeDetector;
        itemReference = 91419;
        break;
    case 249:
        deviceType = BIDCOSDeviceType.Eq3EncryptedSiren;
        itemReference = 97510;
        break;
    default:
        deviceType = BIDCOSDeviceType.Unknown;
        break;
}
```

Code fragments (decompiled):

```csharp
// DeviceKeyRepository: decrypt per-device key
byte[] key = DecryptDeviceKey(Convert.FromBase64String(array[2]));
storedDevice.Sgtin = Convert.FromBase64String(array[0]);
storedDevice.SerialNumber = array[1];
storedDevice.Key = key;
```

```csharp
// BidCoS WSD2: retrieve and apply device key
bidCosHandler.bidcosKeyRetriever.GetDeviceKey(
    SGTIN96.Create(base.Node.Sgtin),
    key => { base.Node.DefaultKey = key; },
    null, 5000
);
```

```csharp
// BIDCOSNode: keys are not persisted
[XmlIgnore]
public byte[] DefaultKey { get; set; }
```

```csharp
// BIDCOSNodeCollection: local WSD2 key is random
Wsd2LocalKey = new byte[16];
new Random().NextBytes(Wsd2LocalKey);
```

Operational implications:
- Without the KeyVault private key, the encrypted CSV cannot be decrypted, so BidCoS device keys stay unavailable.
- There is **no serial/SGTIN -> DeviceKey derivation** in the v1 codebase.
- No default user/passwords exist for BidCoS keys in code; keys are fetched from CSV or backend.
- **WSD1 does not require a DeviceKey**. The CSV entry can be missing and inclusion still works.

Default credentials or keys:
- **No default user/passwords** are used for BidCoS devices in this stack.
- **No static/default DeviceKey** is embedded; keys are retrieved via CSV or backend.

### Backend key exchange (DeviceKey retrieval)
If a key is missing locally, the SHC uses the KeyExchange service to fetch it by SGTIN:

```csharp
// KeyExchangeServiceClient: GetDeviceKey flow
public KeyExchangeResult GetDeviceKey(byte[] sgtin, out byte[] deviceKey)
{
    GetDeviceKeyRequest request = new GetDeviceKeyRequest(sgtin);
    GetDeviceKeyResponse deviceKey2 = GetDeviceKey(request);
    deviceKey = deviceKey2.deviceKey;
    return deviceKey2.GetDeviceKeyResult;
}
```

Operationally, this means the **backend is the authoritative source** for DeviceKeys when the local CSV does not contain them.

### WSD1 (basic smoke detector)
WSD1 devices use the standard BidCoS path without per-device key retrieval:

```csharp
public override bool EnsureCurrentNodeDefaultKey()
{
    return true;
}
```

Inclusion uses ConfigBegin/ConfigData/ConfigEnd and then group registration. Keys are not requested from CSV for WSD1.

### WSD2 (encrypted smoke detector)
WSD2 devices request a DeviceKey and then switch from default key to a local key:

```csharp
public override bool EnsureCurrentNodeDefaultKey()
{
    bidCosHandler.bidcosKeyRetriever.GetDeviceKey(
        SGTIN96.Create(base.Node.Sgtin),
        key => { base.Node.DefaultKey = key; },
        null, 5000
    );
    return base.Node.DefaultKey != null;
}
```

```csharp
public byte[] CurrentKey()
{
    if (base.Included)
    {
        return base.Node.UseDefaultKey ? base.Node.DefaultKey : Wsd2LocalKey;
    }
    return base.Node.DefaultKey;
}
```

During inclusion, the device starts with the **default key**, then `ConfigureNodeKey` installs the **local Wsd2LocalKey** and `UseDefaultKey` flips to `false`.

### Siren (encrypted siren)
The siren adapter mirrors WSD2 logic but uses the local key as a per-hub "private" key:

```csharp
public override bool EnsureCurrentNodeDefaultKey()
{
    bidCosHandler.bidcosKeyRetriever.GetDeviceKey(SGTIN96.Create(base.Node.Sgtin), UpdateDefaultKey, null, 5000);
    return base.Node.DefaultKey != null;
}
```

```csharp
public byte[] CurrentKey()
{
    if (!base.Included || base.Node.UseDefaultKey)
    {
        return base.Node.DefaultKey;
    }
    return GetPrivateRandomKey();
}

private byte[] GetPrivateRandomKey()
{
    return bidCosHandler.NodesManager.Wsd2LocalKey;
}
```

The siren uses the **device key** during inclusion, then switches to the **local key** for ongoing encrypted traffic.

---

## External context on RF protocols and vendor (public sources)
Public documentation and community sources commonly associate **BidCos** with the **HomeMatic** ecosystem and the vendor **eQ-3**. The decompiled code’s BidCos/SerialAPI classes align with that association, suggesting that a portion of the RF stack is derived from or compatible with eQ-3/HomeMatic devices.

Patent ownership is **not confirmed** from the sources reviewed in this pass, so no definitive statement is made here. If needed, this should be verified against official vendor documentation or patent databases.

---

## NAND acquisition scope and integrity
We analyzed a single 256 MB raw NAND image. A basic sanity pass showed a large amount of erased data (0xFF), which is normal for unused NAND, but it does not guarantee full correctness. OOB/ECC data and bad-block remapping are not present in a raw dump, and no second capture was available for validation.

---

## ROM/XIP extraction
A WinCE ROM/XIP extraction at the expected offset yielded:
- `nk.exe` and a full DLL set
- `boot.hv`, `default.hv`, and `user.hv` registry hives

These hives decode cleanly and show system certificate store configuration and TPM CSP registration. The registry references ROM P7B bundles and does not include private key containers.

---

## Persistent registry (EKIM)
EKIM blocks were found but do not decode into full registry hives. Observations:
- EKIM headers are constant per group.
- Payloads are ~95% zero.
- Non-zero fields appear at fixed offsets and repeat across blocks.
- The variable fields map into ROM/XIP regions rather than registry cell structures.

Conclusion: the EKIM data in this dump looks like sparse metadata or placeholders, not complete persistent registry content.

---

## ROM/XIP module metadata
A dense `.dll` name cluster appears around `0x00d00000-0x00d40000`. Nearby strings include crypto/cert APIs (e.g., `PFXImportCertStore`). Attempts to reconstruct a simple module table (pointer arrays, offset/size pairs, 16-bit indices, hash tables) did not yield a clean structure, suggesting packed metadata rather than a flat directory.

---

## KeyVault XML (full content)
We scanned the raw dump for XML-like patterns and filtered by `KeyVault`, `MasterKey`, and `EncryptionKey`. One fragment contained a complete KeyVault structure. Full raw content:

```xml
<KeyVault>
<EncryptionKey>
68C5449D5F135EA4935E6260E0DD99935EDF993A0498500F2DD0BFA6597C7DAF99C9437D28C66FA5D5DE9AA6DF1126389DE66640BD9611BA758E1AE4D3511A39EDB8B2C486A58690F1829E352899F8F08745D06BCD908C99FEEAA97441BBA98BE74593D595D53C6A55FF5F3EDA30E36758A2F2AE807CC3B0625F9132EE4A24EA6684E58DC4426F53197B78653E318D8EBF8E0BCADAF2150ADA93B2619EE45D485708F8CB1A8F6E0BBA726B00A6E14077B183C7DF9AC390D131203C86824C59A99A2EAA6DC15325DFE8A5F2F30182C25DB6E62D4C51E9793DBFF8F5BF2947C37A5E20A4537017971D8FE678581B048C51884272FAE98D40EA1A2166FE608CCCC5
</EncryptionKey>
<SigningCertContent>
MIIFyjCCBLKgAwIBAgIKO2UqggAAABJm9jANBgkqhkiG9w0BAQUFADBcMRUwEwYKCZImiZPyLGQBGRYFbG9jYWwxEzARBgoJkiaJk/IsZAEZFgNyd2UxFzAVBgoJkiaJk/IsZAEZFgdzaG1wcm9kMRUwEwYDVQQDEwxTSE1QUk9ELUNBLUUwHhcNMTYxMTE2MTkzNDI4WhcNMzYxMTExMTkzNDI4WjAWMRQwEgYDVQQDEwtTTUFSVEhPTUUwMTCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALd3NgCDR58N7ti+fI/rim691J8PCy9U2TJK/5HBKjd+HpFxyfTvx4az40R1iHpTqcgS5wPOePTTC+kbB/HfquklXTVpnyFigRBshWG8M7KY/7FJki3F/+Eu1E2px9UVAClYICOMPV9AZcGWNPglXk5fLArnTL045wynoWigw6VZZJqL50dun04a9wXqh76CL1hQz7FcsK0CbNCf/OCLWGJ5pFZxjL+5okj5Ew1Ha1ExEtL3yYX0HbKKM1UZb/O2q+upy+lrlFVytguUnSqZssSkgchaTets00FMjm/USWUOJQ40DlIgU3q7RYhyxz29Bf8hxELPXaADHZjaYpALaU8CAwEAAaOCAtIwggLOMIIBfQYDVR0RBIIBdDCCAXCCC1NNQVJUSE9NRTAxghFzbWFydGhvbWUwMS5sb2NhbIILU01BUlRIT01FMDKCEXNtYXJ0aG9tZTAyLmxvY2FsggtTTUFSVEhPTUUwM4IRc21hcnRob21lMDMubG9jYWyCC1NNQVJUSE9NRTA0ghFzbWFydGhvbWUwNC5sb2NhbIILU01BUlRIT01FMDWCEXNtYXJ0aG9tZTA1LmxvY2FsggtTTUFSVEhPTUUwNoIRc21hcnRob21lMDYubG9jYWyCC1NNQVJUSE9NRTA3ghFzbWFydGhvbWUwNy5sb2NhbIILU01BUlRIT01FMDiCEXNtYXJ0aG9tZTA4LmxvY2FsggtTTUFSVEhPTUUwOYIRc21hcnRob21lMDkubG9jYWyCC1NNQVJUSE9NRTEwghFzbWFydGhvbWUxMC5sb2NhbKAuBgorBgEEAYI3FAIDoCAMHjkxNDEwMTAzNzQ1MkBzaG1wcm9kLnJ3ZS5sb2NhbDAdBgNVHQ4EFgQUdsbMgvNA4NVPs87AKb6EuG0Is3owHwYDVR0jBBgwFoAUkWgfGrfzNfnMlK1O/T/b2NjrSSIwDgYDVR0PAQH/BAQDAgWgMD0GCSsGAQQBgjcVBwQwMC4GJisGAQQBgjcVCIS/ow3q/H6FyYsmhK+JCYSlwiaBIoGAzwiB+pQNAgFkAgEIMDMGA1UdJQQsMCoGCCsGAQUFBwMBBggrBgEFBQcDBAYKKwYBBAGCNwoDBAYIKwYBBQUHAwIwQQYJKwYBBAGCNxUKBDQwMjAKBggrBgEFBQcDATAKBggrBgEFBQcDBDAMBgorBgEEAYI3CgMEMAoGCCsGAQUFBwMCMEQGCSqGSIb3DQEJDwQ3MDUwDgYIKoZIhvcNAwICAgCAMA4GCCqGSIb3DQMEAgIAgDAHBgUrDgMCBzAKBggqhkiG9w0DBzANBgkqhkiG9w0BAQUFAAOCAQEAQooZQylAREeHclxtfcjiMnY9FpBuDcN8y9uR/85VqGuDMOrSd8IRn0ZmjnULhT+/q5pO8Op9ZGfRMUazijtImCoUgoW8QfDKQ7ftQNUUterOpIFmCFcgd64GIJTMj0LSxwD0N8XefcTLlyZXA9UY5JkNH4sgl6DUwrbslvgHWIQS9Tl6c1ROeSoZYiOuIkf4bUm7YuWyy9wQHABkw25JO7e84uU3ZFqcEss/YNLha3YSifm7l6QU1rDCmgaY5pKRmaOJnD6o0/uF4I2/ZvnCITZwVa3xVw0yIeIW3oUHjndb1D8XvqlnKY/pEf/ELFzmEp2fHPFqknhFe3+u3WINXg==
</SigningCertContent>
<MasterKey>
5D4D20254EB4505152A3FE40F844D6D46D2EC58B5CA06785214479E80B68BE27E890C2D9A206B260F3F55492626AD184073E173219AFE9184ED4F37CF659712CEB264C51BFB3A51C2D2414ABED8ACDFE
</MasterKey>
</KeyVault>
```

---

## Certificates (OpenSSL output)
We decoded `SigningCertContent` to PEM and inspected it with OpenSSL. Verbatim output:

```text
Certificate:
    Data:
        Version: 3 (0x2)
        Serial Number:
            3b:65:2a:82:00:00:00:12:66:f6
        Signature Algorithm: sha1WithRSAEncryption
        Issuer: DC = local, DC = rwe, DC = shmprod, CN = SHMPROD-CA-E
        Validity
            Not Before: Nov 16 19:34:28 2016 GMT
            Not After : Nov 11 19:34:28 2036 GMT
        Subject: CN = SMARTHOME01
        Subject Public Key Info:
            Public Key Algorithm: rsaEncryption
                Public-Key: (2048 bit)
                Modulus:
                    00:b7:77:36:00:83:47:9f:0d:ee:d8:be:7c:8f:eb:
                    8a:6e:bd:d4:9f:0f:0b:2f:54:d9:32:4a:ff:91:c1:
                    2a:37:7e:1e:91:71:c9:f4:ef:c7:86:b3:e3:44:75:
                    88:7a:53:a9:c8:12:e7:03:ce:78:f4:d3:0b:e9:1b:
                    07:f1:df:aa:e9:25:5d:35:69:9f:21:62:81:10:6c:
                    85:61:bc:33:b2:98:ff:b1:49:92:2d:c5:ff:e1:2e:
                    d4:4d:a9:c7:d5:15:00:29:58:20:23:8c:3d:5f:40:
                    65:c1:96:34:f8:25:5e:4e:5f:2c:0a:e7:4c:bd:38:
                    e7:0c:a7:a1:68:a0:c3:a5:59:64:9a:8b:e7:47:6e:
                    9f:4e:1a:f7:05:ea:87:be:82:2f:58:50:cf:b1:5c:
                    b0:ad:02:6c:d0:9f:fc:e0:8b:58:62:79:a4:56:71:
                    8c:bf:b9:a2:48:f9:13:0d:47:6b:51:31:12:d2:f7:
                    c9:85:f4:1d:b2:8a:33:55:19:6f:f3:b6:ab:eb:a9:
                    cb:e9:6b:94:55:72:b6:0b:94:9d:2a:99:b2:c4:a4:
                    81:c8:5a:4d:eb:6c:d3:41:4c:8e:6f:d4:49:65:0e:
                    25:0e:34:0e:52:20:53:7a:bb:45:88:72:c7:3d:bd:
                    05:ff:21:c4:42:cf:5d:a0:03:1d:98:da:62:90:0b:
                    69:4f
                Exponent: 65537 (0x10001)
        X509v3 extensions:
            X509v3 Subject Alternative Name: 
                DNS:SMARTHOME01, DNS:smarthome01.local, DNS:SMARTHOME02, DNS:smarthome02.local, DNS:SMARTHOME03, DNS:smarthome03.local, DNS:SMARTHOME04, DNS:smarthome04.local, DNS:SMARTHOME05, DNS:smarthome05.local, DNS:SMARTHOME06, DNS:smarthome06.local, DNS:SMARTHOME07, DNS:smarthome07.local, DNS:SMARTHOME08, DNS:smarthome08.local, DNS:SMARTHOME09, DNS:smarthome09.local, DNS:SMARTHOME10, DNS:smarthome10.local, othername: UPN::914101037452@shmprod.rwe.local
            X509v3 Subject Key Identifier: 
                76:C6:CC:82:F3:40:E0:D5:4F:B3:CE:C0:29:BE:84:B8:6D:08:B3:7A
            X509v3 Authority Key Identifier: 
                91:68:1F:1A:B7:F3:35:F9:CC:94:AD:4E:FD:3F:DB:D8:D8:EB:49:22
            X509v3 Key Usage: critical
                Digital Signature, Key Encipherment
            1.3.6.1.4.1.311.21.7: 
                0..&+.....7.....
..~...&.......&.".......
..d...
            X509v3 Extended Key Usage: 
                TLS Web Server Authentication, E-mail Protection, Microsoft Encrypted File System, TLS Web Client Authentication
            1.3.6.1.4.1.311.21.10: 
                020
..+.......0
..+.......0..
+.....7
..0
..+.......
            S/MIME Capabilities: 
                050...*.H..
......0...*.H..
......0...+....0
..*.H..
..
    Signature Algorithm: sha1WithRSAEncryption
    Signature Value:
        42:8a:19:43:29:40:44:47:87:72:5c:6d:7d:c8:e2:32:76:3d:
        16:90:6e:0d:c3:7c:cb:db:91:ff:ce:55:a8:6b:83:30:ea:d2:
        77:c2:11:9f:46:66:8e:75:0b:85:3f:bf:ab:9a:4e:f0:ea:7d:
        64:67:d1:31:46:b3:8a:3b:48:98:2a:14:82:85:bc:41:f0:ca:
        43:b7:ed:40:d5:14:b5:ea:ce:a4:81:66:08:57:20:77:ae:06:
        20:94:cc:8f:42:d2:c7:00:f4:37:c5:de:7d:c4:cb:97:26:57:
        03:d5:18:e4:99:0d:1f:8b:20:97:a0:d4:c2:b6:ec:96:f8:07:
        58:84:12:f5:39:7a:73:54:4e:79:2a:19:62:23:ae:22:47:f8:
        6d:49:bb:62:e5:b2:cb:dc:10:1c:00:64:c3:6e:49:3b:b7:bc:
        e2:e5:37:64:5a:9c:12:cb:3f:60:d2:e1:6b:76:12:89:f9:bb:
        97:a4:14:d6:b0:c2:9a:06:98:e6:92:91:99:a3:89:9c:3e:a8:
        d3:fb:85:e0:8d:bf:66:f9:c2:21:36:70:55:ad:f1:57:0d:32:
        21:e2:16:de:85:07:8e:77:5b:d4:3f:17:be:a9:67:29:8f:e9:
        11:ff:c4:2c:5c:e6:12:9d:9f:1c:f1:6a:92:78:45:7b:7f:ae:
        dd:62:0d:5e
```

We also extracted the embedded log encryption certificate resource (`SHCLogFileEncryptionCertificate`) and inspected it:

```text
Certificate:
    Data:
        Version: 3 (0x2)
        Serial Number:
            1c:d5:8c:45:00:00:00:00:00:11
        Signature Algorithm: sha1WithRSAEncryption
        Issuer: DC = local, DC = rwe, DC = shmprod, CN = SHMPROD-CA-S
        Validity
            Not Before: Oct  6 08:33:32 2010 GMT
            Not After : Oct  6 08:43:32 2040 GMT
        Subject: CN = SHCLogFileEncryptionCertificate
        Subject Public Key Info:
            Public Key Algorithm: rsaEncryption
                Public-Key: (2048 bit)
                Modulus:
                    00:f5:0a:fb:0d:bb:a3:07:01:b0:1c:18:dd:f1:bb:
                    4f:4b:8e:22:83:c2:c6:3a:d9:cf:a6:9d:6c:ed:d3:
                    ab:81:09:87:d7:62:ef:08:f8:ac:f1:a3:d6:b8:28:
                    ad:cd:8b:d3:f3:fe:70:af:aa:0a:20:3a:78:d6:47:
                    3e:ff:7a:d9:5c:2b:c6:a3:4f:2a:e8:ee:a6:64:74:
                    c7:7e:93:88:aa:b0:66:47:b1:96:0f:04:b9:29:c3:
                    73:e8:49:b4:84:98:52:de:ab:5c:12:22:02:b6:a9:
                    69:97:1e:96:ca:00:24:b9:01:24:7f:53:21:5a:13:
                    67:b2:2d:82:ff:63:b3:b4:22:23:15:70:b3:39:db:
                    a2:c6:81:d3:c8:8c:f3:6c:f1:ef:96:8a:18:08:61:
                    21:25:6f:dd:b3:39:2e:25:75:e7:fe:fc:bb:80:8e:
                    7f:94:5b:74:7d:d5:da:e7:ea:2b:a8:b8:84:44:d3:
                    05:a1:64:dd:7d:05:c8:6f:0a:49:9a:9e:f9:d0:5b:
                    f3:66:57:66:9b:eb:a7:66:26:5b:75:b6:a4:95:fd:
                    f1:f3:fa:4a:b9:9d:0e:88:fb:87:0b:11:24:63:06:
                    29:49:55:9d:d5:0a:7b:3e:00:18:d9:9c:62:03:c4:
                    6f:66:10:4b:ad:4c:b4:e6:19:31:76:65:f5:a3:e8:
                    97:43
                Exponent: 65537 (0x10001)
        X509v3 extensions:
            X509v3 Key Usage: critical
                Digital Signature, Non Repudiation, Key Encipherment, Data Encipherment
            S/MIME Capabilities: 
                050...*.H..
......0...*.H..
......0...+....0
..*.H..
..
            X509v3 Subject Key Identifier: 
                24:11:6E:90:1C:2E:12:5F:17:B0:BA:00:B3:74:C9:C9:01:72:CE:C4
            X509v3 Authority Key Identifier: 
                70:F0:EC:F6:4F:D2:30:C9:04:2D:B5:B6:00:6F:D2:27:23:45:4C:61
            X509v3 Basic Constraints: critical
                CA:FALSE
    Signature Algorithm: sha1WithRSAEncryption
    Signature Value:
        49:3e:92:49:87:0d:0f:27:98:c9:6a:65:f8:b7:7d:b9:67:cc:
        ba:0b:72:fe:00:65:cb:38:d7:dd:75:80:17:55:c6:57:f4:9d:
        09:02:21:8b:ae:95:d9:55:0c:cf:64:12:e9:6d:cc:64:a4:36:
        74:41:a4:9d:78:f7:a4:32:10:a6:ba:16:04:8e:24:94:c2:b0:
        ef:38:6e:46:82:ef:15:c8:9f:a1:6c:4d:08:b8:bc:b6:41:cb:
        d0:3d:e9:2d:e9:80:3b:98:89:ba:96:7f:66:c3:9c:bd:d0:ea:
        5b:dd:af:02:6a:ba:07:5b:fc:ea:a5:c8:26:e0:36:da:48:be:
        65:5a:95:d5:d8:bf:7b:3b:f1:7a:06:68:88:60:f0:1d:bc:98:
        29:55:dc:25:7c:1f:e1:b3:64:8c:a3:48:48:d4:3f:0a:89:a7:
        ff:19:2b:5c:fa:f6:b2:27:d6:8b:db:d0:56:4a:6a:20:8a:84:
        f6:ab:c1:72:a1:d2:dd:08:38:30:49:d5:8b:73:56:b4:01:33:
        ce:38:53:84:82:03:17:a4:ec:54:aa:dc:d4:2d:f8:92:a3:8b:
        13:75:44:46:ce:c4:f6:40:bf:b5:76:fd:20:cb:14:c1:21:0c:
        a9:0e:c7:32:d6:ef:31:bd:63:94:8f:dc:43:b2:18:e3:4c:53:
        2a:bc:54:7e
```

---

## Certificate carving results (dump scan)
We scanned the NAND dump directly for PEM blocks and DER-encoded X.509 sequences:

- **PEM**: one certificate block found and parsed as `SHCLogFileEncryptionCertificate` (issuer `SHMPROD-CA-S`). No PEM private keys were found.
- **DER**: 200 candidate DER sequences were carved; OpenSSL successfully parsed many of them as X.509. They include standard Windows/VeriSign roots and multiple SHM/RWE certificates (`RWE_SmartHome_SHC_Codesigning`, `SHMPROD-CA-S`, `SHMPROD-CA-E`, `SMARTHOME01`), often repeated due to resource embedding. After de-duplication by subject/issuer/serial/dates, **37 unique certificates** remain (168 parsed entries total).
- **PFX/PKCS12**: no valid PKCS12 containers detected.
- **Private keys**: a single DER-encoded RSA private key was detected (1024-bit). It does **not** match any carved certificate modulus and is not a PKCS12 bundle. Likely a standalone/test key or unrelated artifact. Offset in dump: `0xC544BCB`.
- **PKCS#8 (encrypted)**: four PKCS#8 encrypted key blobs were carved. All report OID `1.2.840.113549.1.12.1.3` (pbeWithSHAAnd3-KeyTripleDES-CBC). Without the passphrase, contents remain opaque; they may be false positives or unrelated encrypted blobs.
- **Cert backup paths**: UTF-16LE strings for `\NandFlash\CertBackupLocal` / `\NandFlash\CertBackupUser` appear in OS message strings, but no backing files or PFX blobs were found in the raw dump.
- **P7B bundles**: extracted `shc_root_certs.p7b`, `shc_ca_certs.p7b`, `shc_codesign_certs.p7b`, and `sysroots.p7b` to PEM. No private key markers were present. Example subjects include `RWE_SmartHome_SHC_Codesigning`, `SHMPROD-CA-S`, and `SHMPROD-CA-E`.
- **PKCS#8 passphrases**: tested a small set of common/default passphrases (empty, `password`, `123456`, `shc`, `smarthome`, `livisi`, `rwe`, `innogy`, `osboxes.org`, etc.) and none unlocked the PKCS#8 blobs.
- **Next step (offline)**: run a larger wordlist with hashcat/John against the PKCS#8 blobs; requires a proper PBE hash extraction and a curated wordlist.

This supports the earlier conclusion: public cert material is embedded in ROM/resources, while private keys are likely in the TPM or protected store and not present in the raw NAND dump.

---

## Default user PFX (Cluj-SMARTHOME-SHCDefault00001.pfx)
We obtained the PFX password during analysis and extracted the contents with OpenSSL.

- **Password**: `Test1234!`
- **Keybag**: `pbeWithSHA1And3-KeyTripleDES-CBC`, iteration `2000`
- **Cert bag**: `pbeWithSHA1And40BitRC2-CBC`, iteration `2000`
- **Contents**: 1x private key (PKCS#8 RSA 2048) + 1x certificate

OpenSSL (summary):

```text
MAC: sha1, Iteration 2000
Shrouded Keybag: pbeWithSHA1And3-KeyTripleDES-CBC, Iteration 2000
PKCS7 Encrypted data: pbeWithSHA1And40BitRC2-CBC, Iteration 2000
subject=DC = local, DC = smarthome, OU = SmartHome, OU = Client, OU = SHCDefaultCertificates, CN = SHCDefaultUser00001
issuer=DC = local, DC = smarthome, CN = smarthome-SMARTHOME-DC-CA
serial=4F000063EB06687F47D850AFFC0002000063EB
notBefore=Feb  8 14:34:46 2019 GMT
notAfter=Jan  9 09:00:41 2020 GMT
friendlyName: le-SmartHomeDefaultUserCertificate-0437ff67-d089-40e8-bc06-fc0ac89ffe57
CSP: Microsoft Strong Cryptographic Provider
```

How this is used in code:

- `CertificateManager` searches the **MY** store for a default client certificate based on a DN search string and can import a PFX if configured via `DefaultCertificateFile`/`DefaultCertificatePassword`.
- The hard-coded search string is `local.rwe.shmprod.Smarthome.Client.SHCDefaultCertificates` (this PFX DN is `local.smarthome...`, so it is likely a factory/default cert or environment-specific).
- This default user cert is for client identity and local services; it is **not** the KeyVault signing cert used to unwrap the device master key.


Code fragment (default cert import + search):

```csharp
private string defaultCertificateSearchString =
    "local.rwe.shmprod.Smarthome.Client.SHCDefaultCertificates";

private void ImportDefaultCertificateFromFile()
{
    if (string.IsNullOrEmpty(properties.DefaultCertificateFile) ||
        string.IsNullOrEmpty(properties.DefaultCertificatePassword))
        return;

    Certificate certificate = Certificate.CreateFromPfxFile(
        properties.DefaultCertificateFile,
        properties.DefaultCertificatePassword);

    if (!SHCWrapper.Crypto.Certificates.ImportCertificate(
            properties.DefaultCertificateFile, "MY",
            properties.DefaultCertificatePassword))
    {
        Console.WriteLine("[CertificateManager] Import Default Certificate from file failed");
        return;
    }
    BackupRegistry();
}

private void SearchDefaultCertificateInMyStore()
{
    foreach (Certificate certificate in new CertificateStore("My").EnumCertificates())
    {
        if (CheckDistinguishedName(certificate, defaultCertificateSearchString))
            defaultCertificateThumbprint = certificate.GetCertHashString();
    }
}
```

Additional PFX files with passwords obtained during analysis (deep OpenSSL details):

**1) SHCDefault-Essen-SHMPROD.pfx**
- Password: `Test1234!`
- Subject: `CN=SHCDefaultUserTest, OU=SHCDefaultCertificates, OU=Client, OU=Smarthome, DC=local, DC=rwe, DC=shmprod`
- Issuer: `CN=SHMPROD-CA-S, DC=local, DC=rwe, DC=shmprod`
- Validity: `2010-07-07` to `2040-07-07`
- EKU: `TLS Web Client Authentication`, `E-mail Protection`, `Microsoft Encrypted File System`
- Key Usage: `Digital Signature`, `Key Encipherment`
- SAN: `UPN=SHCDefaultUserTest@shmprod.rwe.local`
- Basic Constraints: `CA:FALSE`

**2) default_cert_testwe0.pfx**
- Password: `pass`
- Subject: `CN=SHCAvatarDefUsr00001, O=Livisi, L=Dortmund, ST=NW, C=DE`
- Issuer: `CN=CA, OU=accounting, O=Internet Widgits Pty Ltd, L=city, ST=Some-State, C=AU`
- Validity: `2019-03-27` to `2029-03-24`
- EKU: `TLS Web Server Authentication`, `TLS Web Client Authentication`, `Code Signing`, `E-mail Protection`
- Key Usage: `Digital Signature`, `Non Repudiation`, `Key Encipherment`
- SAN: `UPN=SHCAvatarDefUsr00001`
- Basic Constraints: `CA:FALSE`

Usage inference:
- These certs match the **default client certificate** flow in `CertificateManager` (imported into `MY` store for identity).
- They do **not** match the KeyVault signing cert used for device key unwrap.
- SAN fields are UPN-only, so there are **no DNS or wildcard** entries in these two certs.

---
---

## Registry hive certificate stores (hvtool dump)
We converted `boot.hv`, `default.hv`, and `user.hv` to `.reg` using `hvtool` and searched for certificate stores and key containers:

- `HKLM\\Comm\\Security\\SystemCertificates\\CodeSign` → `InitFile="\\windows\\shc_codesign_certs.p7b"`
- `HKLM\\Comm\\Security\\SystemCertificates\\CA` → `InitFile="\\windows\\shc_ca_certs.p7b"`
- `HKCU\\Comm\\Security\\SystemCertificates\\Root` → `InitFile="\\windows\\shc_root_certs.p7b"`
- TPM CSP is registered as `SHC Trusted Platform Module Cryptographic Service Provider`.

No `CertBackupLocal` / `CertBackupUser` keys or private-key container references appeared in these hives, reinforcing that private keys are not stored as exportable blobs in the NAND image.

## CE certificate store carving (raw NAND)
We did a raw NAND scan for certificate-store artifacts and ASN.1 blobs:

- UTF-16LE hits for `\NandFlash\CertBackupUser` / `\NandFlash\CertBackupLocal` appear only inside OS log/diagnostic strings (backup/restore messages). No actual backup files or PFX blobs were located in the dump.
- Extracted 421 ASN.1 DER candidates; 48 parse as valid X.509 certs. The parsed list is in `analysis/certs_from_dump/parsed_certs.txt`.
- Found one PKCS#1 RSA private key at offset `0x0C544BCB` (607 bytes, 1024-bit). OpenSSL parses it as a private key, but its modulus does **not** match any extracted certs. Nearby strings reference `support@rebex.net`, suggesting a test/sample key rather than a device identity key.
- `support@rebex.net` appears to be a Rebex software vendor contact (Rebex provides .NET networking/security libraries). This strengthens the interpretation that the key/cert pair is a bundled test/sample artifact, not a device-unique key.
- No PEM blocks and no PKCS#12 containers were detected in the raw dump.

---

## Certificate validity impact (Not After)
The `Not After : Nov 11 19:34:28 2036 GMT` field is the expiration of the **SMARTHOME01 identity certificate**. It does not automatically shut down the device. It does mean that **TLS authentication using this certificate will fail after that date**, unless the certificate is rotated or replaced. So local functionality can continue, but any workflow that depends on this certificate for backend or mutual TLS will be impacted after expiry.

---

## Certificate expiry impact (what would break)
If the SHC continues to use the SMARTHOME01 cert past its expiry date, likely impacts include:

- Backend or cloud connections that require client certificates (mTLS) will fail.
- Update checks/downloads that depend on authenticated TLS sessions will fail.
- Any local service endpoints that rely on this certificate for TLS identity may warn or reject connections, depending on client validation behavior.

Local device control that does not depend on TLS client auth should continue to function, but anything that uses this identity for authentication will be impacted.

---

## Certificate expiry impact on device key crypto
The device key encryption/decryption path does **not** depend on the X.509 validity period. Device keys are encrypted with a master key derived from the KeyVault, and that path uses the **private key** to unwrap the KeyVault’s AES material. Certificate expiration affects TLS identity and authentication, not the AES/RSA key material itself. So an expired cert can break communications, but it does not automatically invalidate existing encrypted device keys.

---

## KeyVault crypto flow (software view)
### What the KeyVault means (step-by-step)
From the decompiled `DeviceMasterKeyRepository`, the encryption chain is explicit:

1) `EncryptionKey` is a hex string representing RSA-encrypted bytes. The decrypted payload is `AES key || AES IV`.
2) RSA decryption uses the **private** key corresponding to `SigningCertContent`.
3) The AES key and IV decrypt `MasterKey` (AES-128 in CBC mode).
4) The first 32 bytes of the resulting plaintext master key are used as the AES-256 key for device key decryption.

If the RSA private key is missing, the chain stops immediately at step 1. This is exactly what we observe in the NAND.

### How the XML is generated (from code)
The KeyVault is written by the SHC software itself. The relevant flow is:

```csharp
// 1) Create AES-128 key and IV
RijndaelManaged aes = new RijndaelManaged();
aes.KeySize = 128;

// 2) Encrypt the master export key with AES
ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);
// ... write encrypted bytes into <MasterKey>

// 3) Encrypt AES key+IV using RSA public key
RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PublicKey;
byte[] blob = aes.Key || aes.IV;
byte[] encrypted = rsa.Encrypt(blob, false);
// ... write as hex into <EncryptionKey>

// 4) Embed public certificate as base64
xmlWriter.WriteString(Convert.ToBase64String(cert.GetRawCertData()));
```

### Where the XML is used (decrypt path)
On load, the process is reversed:

```csharp
// Read XML
KeyVault xml = Deserialize(local.xml);

// 1) Load certificate and private key
Certificate cert = FindCertInStore();
RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;

// 2) Decrypt AES key + IV
byte[] aesBlob = rsa.Decrypt(hex(EncryptionKey), false);

// 3) Decrypt MasterKey
byte[] master = AesDecrypt(hex(MasterKey), key, iv);
```

If `cert.PrivateKey` is null, the flow fails. That is the hard stop we see in the dump-only scenario.

---

## Device keys (software view)
The CSV device key store contains encrypted key material, not raw keys. The decompiled logic shows the exact flow:

```csharp
byte[] master = GetMasterKey();
byte[] key = master.Take(32).ToArray();
byte[] deviceKey = AesEcbDecrypt(csvValue, key);
```

AES-256 in ECB mode with PKCS7 padding is used. Without the master key, the CSV is opaque.

---

## DeviceKey structure and crypto path (CSV)
The device key storage is a CSV table that includes identifiers (e.g., serial/SGTIN) and an **encrypted key blob**. The blob is not plaintext; it is base64-encoded ciphertext. The key flow is:

1) Load and decrypt the master key from KeyVault.\n
2) Use the first 32 bytes of that master key as the AES-256 key.\n
3) Base64-decode the CSV field.\n
4) AES-256 ECB + PKCS7 decrypt to obtain the device key bytes.\n

Simplified logic from the decompiled code:

```csharp
byte[] master = GetMasterKey();               // KeyVault decrypt
byte[] aesKey = master.Take(32).ToArray();    // 256-bit
byte[] cipher = Convert.FromBase64String(csvValue);
byte[] deviceKey = AesEcbPkcs7Decrypt(cipher, aesKey);
```

If the master key cannot be decrypted (e.g., missing private key), the CSV data remains opaque and device keys cannot be derived from identifiers alone.

---

## DeviceKey encode/decode path (from code)
The decompiled `DeviceKeyRepository` contains both the **decode** and **encode** paths. This is the important part for how the CSV is produced and later consumed:

```csharp
// decode: CSV -> plaintext key
byte[] cipher = Convert.FromBase64String(csvKey);
byte[] key = DecryptDeviceKey(cipher); // AES-256 ECB, PKCS7

// encode: plaintext key -> CSV
byte[] cipher = EncryptDeviceKey(deviceKey);
string csvKey = Convert.ToBase64String(cipher);
```

The AES key is derived from the master key:

```csharp
byte[] master = deviceMasterKeyRepository.GetMasterKeyFromFile();
Array.Resize(ref master, 32); // use first 32 bytes
```

So the CSV is **not** a source of truth for keys by itself; it is a storage format for encrypted keys.

---

## DeviceKey CSV production (how rows are generated)
When the system stores keys, it builds each CSV row explicitly:

```csharp
string sgtinB64 = Convert.ToBase64String(deviceKey.SGTIN);
string serial = SerialForDisplay.FromSgtin(deviceKey.SGTIN);
byte[] enc = EncryptDeviceKey(deviceKey.Key);
string keyB64 = Convert.ToBase64String(enc);
string line = $"{sgtinB64},{serial},{keyB64}";
```

This is important because it shows the SGTIN is stored as base64, the serial is derived from the SGTIN for display, and the key is **always** encrypted before being written.

---

## SGTIN generation and serial mapping (code and examples)
The SGTIN is represented as a 96-bit structure in `SGTIN96`. It carries `FilterValue`, `Partition`, `CompanyPrefix`, `ItemReference`, and `SerialNumber`. The packing into the 12-byte SGTIN is done in `GetSerialData()`:

```csharp
list.Add(header); // 0x30
list.Add((byte)(FilterValue << 5));
list[1] |= (byte)(Partition << 2);
int n = itemReferenceBitCount[Partition];
ulong tmp = (CompanyPrefix << n) | ItemReference;
// tmp packed into bytes 1..7, then append SerialNumber (bytes 7..11)
```

The display serial is derived from SGTIN in `SerialForDisplay.FromSgtin`:

```csharp
if (sgtin.CompanyPrefix == 4051495 && (sgtin.ItemReference == 91419 || sgtin.ItemReference == 97510))
    return FromBidCosDevice(sgtin);
return ((ulong)sgtin.ItemReference * 10000000 + sgtin.SerialNumber)
    .ToString().PadLeft(12, '0');
```

BidCos serial formatting uses bitfields in `FromBidCosDevice`:

```csharp
string digits = (sgtin.SerialNumber & 0xFFFFFF).ToString().PadLeft(7, '0');
ulong vendor = (sgtin.SerialNumber & 0xE0000000) >> 29; // 0=RW,1=WE,2=EQ
char lead = (char)(((sgtin.SerialNumber & 0x1F000000) >> 24) + 65); // 'A'..'Z'
return $"{lead}{prefix}{digits}";
```

Example (standard numeric serial):
- If `ItemReference=12345` and `SerialNumber=6789012`, the display serial is `123456789012`.
- Reverse direction (serial -> SGTIN) is **possible only if** you also know `CompanyPrefix`, `Partition`, and `FilterValue`. The serial alone does not carry those.

```csharp
// serial -> SGTIN96 (non-BidCos)
string serial = "123456789012";
uint itemRef = uint.Parse(serial.Substring(0, 5));
ulong sn = ulong.Parse(serial.Substring(5, 7));
var sgtin = new SGTIN96 {
    CompanyPrefix = 4051495, // must be known
    Partition = 5,           // must match prefix length
    FilterValue = 0,
    ItemReference = itemRef,
    SerialNumber = sn
};
byte[] sgtinBytes = sgtin.GetSerialData();
```

For BidCos-style serials (`ARW0123456` etc.), the reverse mapping reconstructs `SerialNumber` from the letter + vendor code + digits using the same bitfield layout. There is no managed helper for this in code; it is inferred from `FromBidCosDevice`.

---

## DeviceKey lifecycle (readable flow)

```
DeviceKey (plaintext)
    -> EncryptDeviceKey (AES-256 ECB, PKCS7)
    -> Base64
    -> CSV row (SGTIN, SerialNo, Key)

CSV row
    -> Base64 decode
    -> DecryptDeviceKey
    -> DeviceKey (plaintext)
```

---

## KeyVault to DeviceKey chain (readable flow)

```
KeyVault (local.xml)
    -> RSA decrypt EncryptionKey (needs device private key)
    -> AES-128 key + IV
    -> AES-128 CBC decrypt MasterKey
    -> MasterKey bytes
    -> take first 32 bytes
    -> AES-256 ECB decrypt device keys
```

---

## Log export crypto flow (readable)

```
Log export (USB)
    -> load signing cert (personal/default)
    -> load encryption cert (embedded resource)
    -> AES-128 encrypt log content (if encryption cert exists)
    -> RSA encrypt AES key + IV into XML
    -> hash content
    -> sign hash with device private key
```

XML layout produced by the exporter:

```
<Upload>
  <Logfile>
    <Info>...</Info>
    <Content>...</Content>
  </Logfile>
  <Signature>...</Signature>
</Upload>
```

---

## DeviceKey CSV structure (fields and format)
The CSV uses a simple, fixed header and per-device rows:

```
SGTIN,SerialNo,Key
```

Field meanings:
- `SGTIN`: device identifier (string)
- `SerialNo`: device serial number (numeric string)
- `Key`: base64-encoded ciphertext of the device key

Observed properties of the `Key` field:
- The base64 decodes to a fixed-length binary blob.
- The ciphertext often ends with a repeated base64 suffix, consistent with PKCS7 padding behavior for AES-ECB (identical padding block across entries).

This aligns with the decompiled AES-256 ECB + PKCS7 path shown above.

Example row format (synthetic):

```
TESTSGTIN00000001,123456789012,<base64-ciphertext>
```

---

## Base64 ciphertext analysis (sanitized)
We did not include real ciphertext in this write-up, but the following observations are based on direct analysis of the CSV data:

- **Base64 length is consistent** across rows, implying a fixed-size ciphertext output from AES (multiples of 16 bytes).
- **Repeated base64 suffix** appears on many rows. This is typical for AES-ECB with PKCS7 padding: if the last plaintext block is padding-only and identical across entries, the final ciphertext block becomes identical as well.
- **ECB mode fingerprints**: because ECB encrypts identical plaintext blocks into identical ciphertext blocks, it produces visible repetition when the input has predictable structure or padding.
- **No plaintext leakage**: while ECB repetition patterns are visible, they do not reveal the key. They only indicate that the same block value appears at the same position across rows.

These traits match the decompiled AES-256 ECB + PKCS7 logic and explain why the CSV “Key” field is not directly usable without the master key.

---

## Why ECB was used (and what that implies)
The decompiled code uses AES-256 in ECB mode for device key storage. ECB is simple and fast, and it avoids the need to store an IV per record. That likely made storage and implementation straightforward on CE/CF. The downside is the classic ECB weakness: repeated plaintext blocks produce repeated ciphertext blocks, which leaks structural patterns (but not the key itself).

In this dataset, the most visible artifact is the repeated final block caused by PKCS7 padding. It does not let us decrypt anything, but it is a recognizable fingerprint of the chosen mode.

---

## AES construction details (what the code does)
The managed code uses standard .NET `RijndaelManaged` and `CryptoStream` implementations. There is no custom block cipher logic; the custom pieces are around **how keys are assembled and where they are stored**.

Key points from the code paths:

- **KeyVault encryption** uses AES-128-CBC with PKCS7 padding. AES key and IV are generated by `RijndaelManaged`, then encrypted using RSA.
- **Device key decryption** uses AES-256-ECB with PKCS7 padding. The AES key is the first 32 bytes of the decrypted master key.
- **No custom AES** is implemented; all block cipher operations are standard library calls.

Example pattern for AES use:

```csharp
RijndaelManaged aes = new RijndaelManaged();
aes.Mode = CipherMode.CBC;
aes.Padding = PaddingMode.PKCS7;
aes.Key = key;
aes.IV = iv;
ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
```

The important part is not the AES primitive itself, but the key derivation and storage chain that supplies `key` and `iv`.

---

## Device key validation behavior
When loading the CSV, the code performs a strict base64 validation check on each key field. Non-base64 values are skipped with a log message:

```csharp
private bool IsDeviceKeyValidBase64(string encryptedKey)
{
    try { Convert.FromBase64String(encryptedKey); return true; }
    catch (FormatException) { return false; }
}
```

This means malformed or non-base64 entries are ignored, but it does **not** verify cryptographic integrity (no MAC or signature on the CSV entries).

---

## Log export and encryption (USB)
The system includes a USB log export path. Log export writes an XML “Upload” file that contains:

- **Info** section (serial number, signing cert details, encryption cert details)
- **Content** section (either plaintext or AES-encrypted log lines)
- **Signature** over the hashed log contents

The flow is standard-library crypto with device certificates:

```csharp
// LogExporter.ExportLog
GetSigningCertificate();        // from local store (personal or default)
GetEncryptionCertificate();     // from embedded PEM resource
CreateAes128();                 // AES-128 for log encryption
WriteLogfile();                 // writes Info + Content
WriteSignature();               // RSA signature over hashed content
```

Resource usage (actual code paths):

```csharp
// LogExporter.GetEncryptionCertificate
encryptionCertificate =
    Certificate.CreateFromPemFile(Resources.SHCLogFileEncryptionCertificate);

// LogExporter.WriteEncryptionKey
RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)encryptionCertificate.PublicKey;
byte[] keyiv = new byte[aes128.Key.Length + aes128.IV.Length];
Array.Copy(aes128.Key, keyiv, aes128.Key.Length);
Array.Copy(aes128.IV, 0, keyiv, aes128.Key.Length, aes128.IV.Length);
byte[] wrapped = rsa.Encrypt(keyiv, fOAEP: false);
WriteBinHex(wrapped); // stored in <EncryptionKey>
```

We exported the embedded certificate to `extracted_resources/SHCLogFileEncryptionCertificate.pem` (1364 bytes) for analysis.

If the encryption certificate is present, AES-128 is used to encrypt log content and the AES key+IV are RSA-encrypted into the XML. The signing cert is the device cert (personal or default) and is used to sign the hashed payload. This is conceptually similar to the KeyVault pattern (AES content + RSA-wrapped AES key).

---

## Log encryption defaults (what the code implies)
The log export path always tries to load the encryption certificate from embedded resources (`SHCLogFileEncryptionCertificate`). If it is present, the log content is encrypted with AES-128 and a signature is attached. If it is missing, the export falls back to plaintext content and still signs the hash. Because the encryption cert is compiled into the resources, encryption is effectively the default behavior on standard builds.\n

This also means the log encryption certificate is **not expected to be in NAND as a separate file**; it is embedded in the application resources. The private key needed to decrypt exported logs would be held by whoever owns the corresponding encryption certificate private key, not by the SHC itself.\n

**Decryption feasibility (with current material)**

Because the AES key+IV are RSA-wrapped using the **public** log encryption certificate, decryption requires the **private key** for `SHCLogFileEncryptionCertificate`. That private key is not present in the NAND dump or embedded resources we extracted. With the current artifacts, encrypted log exports can be parsed and verified structurally, but they cannot be decrypted.

---

## Log decryption key availability (dump view)
In the dump, we do **not** see a private key that matches the log encryption certificate. That is consistent with the design: the encryption certificate is embedded (public key only), while the corresponding private key is expected to be held outside the SHC. This allows the device to export encrypted logs without being able to decrypt them itself.

---

## Log encryption and the master key (clarification)
Log encryption does **not** use the KeyVault master key. It uses a separate embedded log‑encryption certificate and a fresh AES‑128 key per export. Therefore, the absence of the master key in the dump does not prevent log **encryption**. It does prevent log **decryption** unless the external private key for the log‑encryption cert is available. If the embedded encryption cert were absent, the code falls back to plaintext log export.

---

## Logging endpoints and storage (local)
The managed stack uses a file-based logger and restores legacy logs from `\\NandFlash\\logStore` at startup. USB log export operates on these local log files and writes `shc.log` to the mounted USB volume.

Serial logging can be **toggled via USB folders**:
- If the USB root contains `EnableSerialLog`, the device starts `ConsoleLogging` and sets `FilePersistence.EnableSerialLogging = true`.
- If the USB root contains `DisableSerialLog`, it stops serial logging and clears the flag.

These folder triggers are checked in `UsbStickLogExport` on USB insertion, and the folder is deleted after processing.

Code excerpt (from `UsbStickLogExport`):

```csharp
if (Directory.Exists("Hard Disk\\EnableSerialLog") && !FilePersistence.EnableSerialLogging)
{
    StartSerialLogging();
    Log.Information(Module.Logging, "\"EnableSerialLog\" was found on USB. Serial logging enabled!");
    Directory.Delete("Hard Disk\\EnableSerialLog");
    FilePersistence.EnableSerialLogging = true;
}
if (Directory.Exists("Hard Disk\\DisableSerialLog"))
{
    Log.Information(Module.Logging, "\"DisableSerialLog\" was found on USB. Stopping serial logging...");
    StopSerialLogging();
    Directory.Delete("Hard Disk\\DisableSerialLog");
    FilePersistence.EnableSerialLogging = false;
}
```

Related implementation (what serial logging does):

```csharp
private void StartSerialLogging()
{
    if (consoleLogger == null)
    {
        containerAccess.Register("ConsoleLogging", (Func<Container, IService>)delegate(Container c)
        {
            ConsoleLogging consoleLogging = new ConsoleLogging(c);
            c.Resolve<ITaskManager>().Register(consoleLogging);
            return consoleLogging;
        }).InitializedBy(delegate(Container c, IService v)
        {
            v.Initialize();
        }).ReusedWithin(ReuseScope.Container);
        consoleLogger = (ConsoleLogging)containerAccess.ResolveNamed<IService>("ConsoleLogging");
    }
    consoleLogger.Start();
}

private void StopSerialLogging()
{
    if (consoleLogger != null)
    {
        consoleLogger.Stop();
    }
}
```

---

## settings.config and boot.config (USB update package + dump correlation)
We recovered **full** versions of both files from the Classic USB update package at `shc.zip`:

- `settings.config` (complete, includes `<Signature>`).
- `boot.config` (complete, module list + log levels).

In the NAND dump, the `settings.config` content appears as an ASCII XML fragment (`xml_ascii_009.xml`, offset `76357635`) and the `boot.config` content appears only as a **partial** fragment (`xml_ascii_005.xml`, offset `59502595`). The dump fragment matches the beginning of the USB `boot.config`, so the dump likely stores it **fragmented** or interleaved in NAND.

What is inside `settings.config` (selected highlights):

- **Backend endpoints** (TLS URLs for device management, config, updates, key exchange, initialization, messaging, notifications, storage).
- **Device cert enrollment**: `CertificateSubjectName=SMARTHOME01`, `CertificateTemplateName=SHCMultipurposeCertificate`, `CertificateUpnSuffix=shmprod.rwe.local`.
- **Backend shutdown cutoff**: `StopBackendRequestsDate=3/1/2024`.
- **Relay connection**: `RelayServerUrl=wss://gateway.services-smarthome.de/API/1.0/shcconnection/connect`, `UseCertificateAuthentication=true`.
- **WebServiceHost credentials**: `ClientId=clientId`, `ClientSecret=clientPass` (placeholders/defaults, plaintext).
- **Update windows and minimum free memory thresholds** for OS/app updates.

What is inside `boot.config` (full file from USB update):

- Module list + log levels; modules include `Core`, `Logging`, `BackendCommunication`, `SerialCommunication`, `DeviceManager`, `ApplicationsHost`, `RuleEngine`, `SipCosProtocolAdapter`, `wMBusProtocolAdapter`, `LemonbeatProtocolAdapter`, `StartupLogic`, etc.
- Log levels are plain text values (`Error`, `Warning`, `Information`, `Debug`) per module.

Signature / integrity behavior (from decompiled code):

- `settings.config` embeds a `<Signature>` hex blob. The `ConfigSignature` helper computes SHA1 over `<Sections>` and verifies with **any** cert in the `CodeSign` store. The hard-coded thumbprint constant exists but is unused.\n
- **No call sites** were found for `ConfigSignature` in the decompiled managed code; `ConfigurationManager` loads `settings.config` without validating the signature.\n
- `SettingsFileHelper.ShouldRegisterBackendRequests()` reads `settings.config` directly and falls back to a hard-coded cutoff date on errors; it does not validate the signature.\n
- `boot.config` is loaded directly by `ModuleLoader` and **no signature or hash check** is performed in managed code.

Encryption status:

- Both config files are **plaintext XML**. The `<Signature>` element is an integrity marker, not encryption.

Code fragments from the core layer:

```csharp
// settings.config
XElement x = XElement.Load(directoryName + "\\settings.config");
```

```csharp
// boot.config
XElement x = XElement.Load(directoryName + "\\boot.config");
```

In practice, `settings.config` governs operational parameters (update windows, endpoints, limits), while `boot.config` drives module startup order and logging behavior. The USB update package files appear to be **generic defaults**, not per-device secrets.

### Load procedure and security (decompiled behavior)
The managed code loads both files directly from the application directory. There is no encryption; any protection is limited to the optional signature embedded in `settings.config`.

**settings.config load path (ConfigurationManager):**

```csharp
private static string DefaultConfigurationPath => Path.Combine(ConfigurationDirectory, "settings.config");

private void Load()
{
    configurationSections.Clear();
    LoadConfigurationFile(DefaultConfigurationPath, userConfiguration: false);
}

private void LoadConfigurationFile(string path, bool userConfiguration)
{
    if (!File.Exists(path))
    {
        HaltFatally("No configuration file.");
        return;
    }
    using XmlReader xmlReader = XmlReader.Create(path);
    xmlReader.MoveToContent();
    while (!xmlReader.IsStartElement("Sections") && xmlReader.Read()) {}
    xmlReader.Read();
    while (xmlReader.IsStartElement("Section"))
    {
        string attribute = xmlReader.GetAttribute("Name");
        ConfigurationSection cs = this[attribute];
        ReadConfigurationSection(cs, xmlReader, userConfiguration);
        xmlReader.Skip();
    }
}
```

**settings.config usage example (SettingsFileHelper):**

```csharp
XElement xElement = XElement.Load(directoryName + "\\settings.config");
XElement xElement2 = item.Descendants()
    .FirstOrDefault(d => d.Attribute("Key").Value == "StopBackendRequestsDate");
string value = xElement2.Attribute("Value").Value;
```

**boot.config load path (ModuleLoader):**

```csharp
XElement xElement = XElement.Load(directoryName + "\\boot.config");
foreach (XElement item in xElement.Elements().Where(e => e.Name == "module"))
{
    XAttribute name = item.Attribute("name");
    XAttribute assembly = item.Attribute("assembly");
    XAttribute klass = item.Attribute("class");
    XAttribute logLevel = item.Attribute("logLevel");
    // load module from assembly and set log level
}
```

**Optional signature verification (not called):**

```csharp
internal bool CheckSignature()
{
    doc.LoadXml(File.ReadAllText(fileName));
    string nodeXml = GetNodeXml("Sections");
    byte[] signature = StringToByteArray(GetNodeXml("Signature"));
    return IsSignatureValid(Encoding.UTF8.GetBytes(nodeXml), signature);
}

private bool IsSignatureValid(byte[] encryptedData, byte[] signature)
{
    CertificateStore store = new CertificateStore(StoreLocation.LocalMachine, "CodeSign");
    foreach (Certificate cert in store.EnumCertificates())
    {
        RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PublicKey;
        byte[] hash = new SHA1Managed().ComputeHash(encryptedData);
        if (rsa.VerifyHash(hash, CryptoConfig.MapNameToOID("SHA1"), signature))
        {
            return true;
        }
    }
    return false;
}
```

**Security implication summary:**

- `settings.config` integrity is **not enforced** by default in managed code (signature verifier exists but is unused).
- `boot.config` has **no signature** and is loaded directly.
- Both files are **plaintext**, so any confidentiality relies on filesystem access control, not cryptography.

### Dump integrity check (what this means for the NAND)
We compared the USB versions against the dump strings:

- `settings.config` from the dump **matches** the USB file (the only difference is a BOM/encoding marker in the dump string). This suggests the dump captures a full copy.
- `boot.config` in the dump is **truncated**: the extracted XML ends mid-line and is missing the final lines (`WebServerHost`, closing `</boot>`). This looks like a **string‑carve limitation or fragmentation**, not necessarily corruption of the NAND.

Conclusion: the dump appears **consistent**, but some files are **not stored contiguously** in NAND or are embedded inside larger packed resources. For such data, simple string carving can miss tail bytes; deeper extraction (ROM/NK parsing or resource unpacking) is required to rule out missing critical data.

Additional deep extraction (WSL/Ubuntu):

- We extracted WinCE ROM segments at offsets `0x80000` and `0x360000` using `winceextractor.py`. No `settings.config` / `boot.config` files were present in those ROM extractions, and no `<Settings>`/`<boot>` XML strings were found there.
- This points to the configs living **outside** the ROM image (likely in the app/update resources), which explains why they appear as strings in NAND rather than clean files in ROM.

---

---

## Device key export (USB CSV exists)
Correction: the managed code **does** implement USB export/import of device keys as CSV.

The `DeviceKeyExporter` subscribes to USB drive events and copies `\\NandFlash\\DevicesKeysStorage.csv` to `Hard Disk\\devices\\DevicesKeysStorage.csv` when a USB stick is attached. If a CSV already exists on the USB stick, it is **imported first** (merge new keys) and then replaced by the device's current CSV.

Key code path (from `DeviceKeyExporter`):

```csharp
eventManager.GetEvent<USBDriveNotificationEvent>()
    .Subscribe(ExportDeviceKeyCsv, null, ThreadOption.PublisherThread, null);

private void ExportDeviceKeyCsv(USBDriveNotificationEventArgs args)
{
    if (!args.Attached) return;
    if (!File.Exists("\\NandFlash\\DevicesKeysStorage.csv"))
        Log.Information(Module.BusinessLogic, "The Devices Keys storage CSV does not exist");
    ExportFile();
}

private void ExportFile()
{
    if (File.Exists(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv")))
        ImportKeysFromCsv();
    CreateDirectoryOnUsb();
    if (File.Exists(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv")))
        File.Delete(Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv"));
    File.Copy("\\NandFlash\\DevicesKeysStorage.csv",
              Path.Combine("Hard Disk\\devices\\", "DevicesKeysStorage.csv"));
    FilePersistence.DevicesKeysExported = true;
}
```

This is distinct from log export (which uses encrypted XML). The device key CSV export is a simple file copy to USB with a merge step.

---

## Software APIs and internal services (overview)
The managed stack uses a mix of standard .NET CF APIs and vendor libraries:

- **Crypto**: `RijndaelManaged`, `RSACryptoServiceProvider`, `CryptoStream`, `HashAlgorithm` from .NET CF.
- **Certificates**: `Org.Mentalis.Security.Certificates` for store access; device certs located by thumbprint.
- **Networking**: WCF bindings, Rebex TLS stack for HTTPS.
- **Persistence**: file-based storage on NAND (e.g., `/NandFlash` paths).
- **Native bridge**: `shc_api.dll` for raw flash updates and registry backup hooks.

These are the core APIs the decompiled code relies on for update handling, certificate management, and encryption/decryption flows.

---

## Backend/API endpoints and payloads (v1)
The backend communication stack uses **WCF SOAP** clients generated by `NetCFSvcUtil`. It is **not** a REST/JSON API. Each client has a default `EndpointAddress` set to `https://localhost/Service`, which suggests the real backend URLs are injected at runtime (config or provisioning). The only concrete host hard-coded in code is the SMS service:

- `https://sh70a0100.shmtest.rwe.local/PublicFacingServicesShc/SmsServices/SendSmsService.svc`

In practice, the SHC calls SOAP services defined by their action URIs (namespaces under `http://rwe.com/SmartHome/...`). Below is the action map extracted from the service clients.

**Service action map (from the decompiled clients)**

- **KeyExchangeService** (`SmartHome.SHC.BackendCommunication.KeyExchangeScope`)
  - `.../IKeyExchangeService/EncryptNetworkKey`
  - `.../IKeyExchangeService/GetDeviceKey`
  - `.../IKeyExchangeService/GetMasterKey`
  - `.../IKeyExchangeService/GetDevicesKeys`
- **ConfigurationService** (`...ConfigurationScope`)
  - `.../IConfigurationService/GetShcSyncRecord`
  - `.../IConfigurationService/ConfirmShcSyncRecord`
  - `.../IConfigurationService/SetManagedSHCConfiguration`
  - `.../IConfigurationService/AddManagedSHCConfiguration`
  - `.../IConfigurationService/SetUnmanagedSHCConfiguration`
  - `.../IConfigurationService/DeleteManagedSHCConfiguration`
  - `.../IConfigurationService/DeleteUnmanagedSHCConfiguration`
  - `.../IConfigurationService/GetManagedSHCConfiguration`
  - `.../IConfigurationService/GetUnmanagedSHCConfiguration`
  - `.../IConfigurationService/GetRestorePointShcConfiguration`
- **DeviceManagementService** (`...DeviceManagementScope`)
  - `.../IDeviceManagementService/UploadLogFile`
  - `.../IDeviceManagementService/UploadSystemInfo`
- **SoftwareUpdateService** (`...SoftwareUpdateScope`)
  - `.../ISoftwareUpdateService/CheckForSoftwareUpdate`
  - `.../ISoftwareUpdateService/ShcSoftwareUpdated`
- **DeviceUpdateService** (`...DeviceUpdateScope`)
  - `.../IDeviceUpdateService/CheckForDeviceUpdate`
- **ShcInitializationService** (`...ShcIntializationScope`)
  - `.../IShcInitializationService/SubmitCertificateRequest`
  - `.../IShcInitializationService/RetrieveInitializationData`
  - `.../IShcInitializationService/ConfirmShcOwnership`
  - `.../IShcInitializationService/ShcResetByOwner`
  - `.../IShcInitializationService/SubmitOwnershipRequest`
  - `.../IShcInitializationService/RetrieveOwnershipData`
- **ShcMessagingService** (`...ShcMessagingScope`)
  - `.../IShcMessagingService/SendSmokeDetectionNotification`
  - `.../IShcMessagingService/SendSmokeDetectionNotification14`
  - `.../IShcMessagingService/SendNotificationEmail`
  - `.../IShcMessagingService/SendEmail`
  - `.../IShcMessagingService/SendSystemEmail`
  - `.../IShcMessagingService/GetEmailRemainingQuota`
- **NotificationService** (`...NotificationScope`)
  - `.../INotificationService/SendNotifications`
  - `.../INotificationService/SendSystemNotifications`
- **PublicStorage / DataTracking** (`...PublicStorageScope`)
  - `.../IDataTrackingService/StoreData`
  - `.../IDataTrackingService/StoreListData`
- **PublicStorage / DAL** (`...PublicStorageScope`)
  - `.../IDalStorageService/StoreDeviceActivityLog`
  - `.../IDalStorageService/PurgeDeviceActivityLog`
- **SmsService** (`...SmsScope`)
  - `.../ISmsService/SendSystemSms`
  - `.../ISmsService/SendSms`
  - `.../ISmsService/GetSmsRemainingQuota`
- **ApplicationTokenService** (`...ApplicationTokenScope`)
  - `.../IApplicationTokenService/GetApplicationToken`
  - `.../IApplicationTokenService/GetApplicationTokenHash`

**Configured endpoints (from USB Update SHC Classic config)**

The update package includes `USB Update SHC Classic\\shc\\settings.config`, which provides concrete service URLs used at runtime. These appear to override the `https://localhost/Service` defaults in the generated clients.

- `DeviceManagementServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/DeviceManagementService.svc`
- `ConfigurationServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ConfigurationService.svc`
- `SoftwareUpdateServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/SoftwareUpdateService.svc`
- `KeyExchangeServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/KeyExchangeService.svc`
- `ShcInitializationServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ShcInitializationService.svc`
- `ShcMessagingServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ShcMessagingService.svc`
- `ApplicationTokenServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ApplicationManagement/ApplicationTokenService.svc`
- `DeviceUpdateServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/DeviceUpdateService.svc`
- `NotificationServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/Notifications/NotificationService.svc`
- `DataTrackingClientUrl` -> `https://storage.services-smarthome.de/PublicStorageServices/DataTrackingService.svc`
- `SmsServiceUrl` -> `https://shcc.services-smarthome.de/PublicFacingServicesSHC/SmsServices/SendSmsService.svc`
- `RelayServerUrl` -> `wss://gateway.services-smarthome.de/API/1.0/shcconnection/connect`

Other backend-related settings in the same file:

- `CertificateUpnSuffix` -> `shmprod.rwe.local` (UPN suffix used in certificate enrollment)
- `StopBackendRequestsDate` -> `3/1/2024` (cutoff for backend requests)
- `WebServiceHost` has `ClientId` and `ClientSecret` set to `clientId` / `clientPass` (likely placeholders)

The same endpoint block was also recovered from the NAND dump via carved `settings.config` fragments, so these URLs are present on-device and not only in the USB update package.

`USB Update SHC Classic\\shc\\boot.config` does not list any URLs; it only defines module startup order and log levels.

**Endpoint matrix (URL -> service role / action family)**

| URL (settings.config) | Service role | Action family |
| --- | --- | --- |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/DeviceManagementService.svc` | DeviceManagementService | UploadLogFile, UploadSystemInfo |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ConfigurationService.svc` | ConfigurationService | Get/Set/Delete Managed/Unmanaged config, sync record |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/SoftwareUpdateService.svc` | SoftwareUpdateService | CheckForSoftwareUpdate, ShcSoftwareUpdated |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/KeyExchangeService.svc` | KeyExchangeService | GetDeviceKey, GetMasterKey, GetDevicesKeys, EncryptNetworkKey |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ShcInitializationService.svc` | ShcInitializationService | SubmitCertificateRequest, RetrieveInitializationData, ownership flows |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ShcMessagingService.svc` | ShcMessagingService | Send email notifications, quotas |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ApplicationManagement/ApplicationTokenService.svc` | ApplicationTokenService | GetApplicationToken, GetApplicationTokenHash |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/DeviceUpdateService.svc` | DeviceUpdateService | CheckForDeviceUpdate |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/Notifications/NotificationService.svc` | NotificationService | SendNotifications, SendSystemNotifications |
| `https://storage.services-smarthome.de/PublicStorageServices/DataTrackingService.svc` | DataTrackingService | StoreData, StoreListData |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/SmsServices/SendSmsService.svc` | SmsService | SendSms, SendSystemSms, GetSmsRemainingQuota |
| `wss://gateway.services-smarthome.de/API/1.0/shcconnection/connect` | RelayServer | WSS relay connection bootstrap |

**Reachability/deprecation note (config-only)**

- `StopBackendRequestsDate` is set to `3/1/2024`, which implies the software will stop sending backend requests after that date even if URLs remain configured.
- There are no per-endpoint disable flags in `settings.config`, so the only explicit global cutoff we see is `StopBackendRequestsDate`.
- Treat endpoint reachability as "unknown/likely decommissioned" after the cutoff; the config alone cannot confirm live availability.

**Security note (DNS spoofing and TLS)**

This report does not assess or recommend interception techniques. From the managed code we do not see an explicit "accept all certificates" or TLS bypass. The stack relies on WCF + the TLS library and the device certificate store, so standard certificate validation is expected unless bypassed elsewhere (native code or TLS library behavior not visible here).

**Config tampering caveat**

`settings.config` contains a `<Signature>` element. If the application verifies this signature on load, altering `StopBackendRequestsDate` or service URLs will likely be rejected. The decompiled managed code does not show the signature verification path, so the enforcement point remains unknown from this layer alone.

---

## DNS status of backend domains
We rechecked public DNS resolution for the backend hostnames referenced in `settings.config`:

- `services-smarthome.de` and `gateway.services-smarthome.de` return **SERVFAIL** from public resolvers.
  - `nslookup services-smarthome.de 1.1.1.1` -> "Server failed"
  - `nslookup services-smarthome.de 8.8.8.8` -> "Server failed"
  - Same result for `gateway.services-smarthome.de`
- With SERVFAIL, no A/AAAA/MX/TXT/SRV records can be obtained from public resolvers.
- NS/SOA/MX queries also fail with SERVFAIL via public resolvers, so the current authoritative chain cannot be confirmed from this host.

Interpretation:

- The domain is registered and appears delegated (per earlier WHOIS notes), but public DNS resolution currently fails.
- This can be caused by DNSSEC/zone misconfiguration or by intentional restriction (authoritative servers refusing public queries).
- Without authoritative answers, we cannot confirm current service availability or decommissioning from DNS alone.

WHOIS/RDAP snapshot (DENIC RDAP):

- Status: `active`
- Last changed: `2020-12-14T10:20:11+01:00`

Reported DNS configuration (unverified from current resolvers):

- Previous checks (reported) indicated Azure DNS nameservers:
  - `ns1-02.azure-dns.com`
  - `ns2-02.azure-dns.net`
  - `ns3-02.azure-dns.org`
  - `ns4-02.azure-dns.info`
- Because public DNS queries currently SERVFAIL, these NS values could not be revalidated on this machine.

**Payload shapes (examples from the generated request/response classes)**

Key exchange (SGTIN to DeviceKey):

```csharp
[XmlRoot("GetDeviceKey", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetDeviceKeyRequest
{
    [XmlElement(DataType="base64Binary")]
    public byte[] sgtin;
}

[XmlRoot("GetDeviceKeyResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class GetDeviceKeyResponse
{
    public KeyExchangeResult GetDeviceKeyResult;
    [XmlElement(DataType="base64Binary")]
    public byte[] deviceKey;
}
```

Certificate enrollment:

```csharp
[XmlRoot("SubmitCertificateRequest", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SubmitCertificateRequestRequest
{
    public string shcSerial;
    public string pin;
    public string certificateRequest;
}

[XmlRoot("SubmitCertificateRequestResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class SubmitCertificateRequestResponse
{
    public InitializationErrorCode SubmitCertificateRequestResult;
    public string sessionToken;
}
```

Initialization polling result (cloud -> SHC):

```csharp
[XmlRoot("RetrieveInitializationDataResponse", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class RetrieveInitializationDataResponse
{
    public InitializationErrorCode RetrieveInitializationDataResult;
    public string issuedCertificate;
    public ShcSyncRecord shcSyncRecord;
    public bool furtherPollingRequired;
    public int pollAfterSeconds;
}

public class ShcSyncRecord
{
    public ShcRole[] Roles;
    public ShcUser[] Users; // includes PasswordHash in each ShcUser
}
```

Software update metadata (cloud -> SHC):

```csharp
public class UpdateInfo
{
    public UpdateCategory Category;
    public string DownloadLocation;
    public string DownloadUser;
    public string DownloadPassword;
    public UpdateType Type;
    public DateTime UpdateDeadline;
    public string Version;
}
```

Log upload (SHC -> cloud):

```csharp
[XmlRoot("UploadLogFile", Namespace = "http://rwe.com/SmartHome/2010/11/08/PublicFacingServices")]
public class UploadLogFileRequest
{
    public string shcSerial;
    [XmlElement(DataType="base64Binary")]
    public byte[] content;
    public int currentPackage;
    public int nextPackage;
    public string correlationId;
}
```

Device update lookup (SHC -> cloud):

```csharp
public class DeviceDescriptor
{
    public string AddInVersion;
    public string CurrentFirmwareVersion;
    public string HardwareVersion;
    public short Manufacturer;
    public int ProductId;
}

public class DeviceUpdateInfo
{
    public string ImageChecksum;
    public string ImageUrl;
    public string ReleaseNotesLocation;
    public DeviceUpdateType UpdateType;
    public string VersionNumber;
}
```

**WebSocket channel**

`WebSocketSecureClient` performs an HTTPS handshake, reads a `Location:` header, and converts it to `wss://` before establishing the WebSocket connection. This implies a redirect-based WSS bootstrap rather than a hard-coded WSS endpoint in the binary.

---

## shc_api.dll (native flash/registry bridge)
We extracted a **full native WinCE ARM** `shc_api.dll` from the dump. The two copies in the workspace are byte‑identical (SHA‑256 match), so the ROM extraction is consistent.

What we can confirm from static analysis:

- **Native ARM PE** (not .NET), image base `0x40840000`, entrypoint `0x40848204`.
- **Export table present** (16 functions, base ordinal 1). The export directory was located manually and parsed.
- **Strings indicate flash + registry ops**:
  - `WriteRawPartition`, `EraseRawPartition`
  - `BackupRegistry`, `RestoreRegistry`
  - `Cannot read/write bootloader settings`
  - `Cannot access NAND Flash driver`
  - `\NandFlash\CertBackupLocal`, `\NandFlash\CertBackupUser`
  - `readEK` (likely TPM EK read)

Exported API surface (from the parsed export table):

```
01 Cert_ImportCertificate
02 Cert_ImportCertificateWithPrivateKey
03 DhcpRenew
04 EraseRawPartition
05 GetHWVersion
06 GetSGTIN
07 IsFactoryReset
08 RasConnect
09 RasDisconnect
10 RasIsConnected
11 Reset
12 StartSNTPService
13 StopSNTPService
14 WriteRawPartition
15 BackupRegistry
16 RestoreRegistry
```

Managed usage mapping (from `SHCWrapper` and core code):

- `GetSGTIN` → SHC serial/SGTIN generation (device identity).
- `GetHWVersion` → hardware version detection.
- `StartSNTPService` / `StopSNTPService` → NTP control.\n
- `DhcpRenew` → renew IP on adapter `EMACB1` (default).
- `RasConnect` / `RasDisconnect` / `RasIsConnected` → Lemonbeat RAS connectivity (USB dongle).
- `Reset` / `IsFactoryReset` → reset handling / factory reset detection.
- `EraseRawPartition` / `WriteRawPartition` → firmware update raw flash write.\n
- `BackupRegistry` / `RestoreRegistry` → certificate store persistence to `\NandFlash\CertBackupLocal` / `\NandFlash\CertBackupUser`.

Note: `LoadPublicKeyFromCertFile` is declared in `PrivateWrapper` but **not** exported by this DLL, suggesting version mismatch or a stubbed API.

Disassembly excerpt (entrypoint stub):

```asm
0x40848204: str lr, [sp, #-4]!
0x40848208: cmp r1, #1
0x4084820c: bleq #0x408483d4
0x40848210: mov r0, #1
0x40848214: pop {lr}
0x40848218: bx lr
```

This aligns with the managed update path calling `WriteRawPartition` through a native bridge. Deeper reversing will require a full disassembly workflow (e.g., Ghidra/IDA) to map exported entrypoints and parameters.

---

## Internal services and classes (selected map)
These are the key managed classes that define the security and update behavior:

- `DeviceMasterKeyRepository` - KeyVault read/write and master key unwrap\n
- `DeviceKeyRepository` - CSV encode/decode and AES-ECB encryption\n
- `CertificateManager` - certificate store selection and thumbprints\n
- `SoftwareUpdateProcessor` - update orchestration and staging\n
- `FirmwareImage` - NK header validation\n
- `LogExporter` / `UsbStickLogExport` - log export, encryption, and signatures\n
- `StartupLogic` - initialization order, registration, and update triggers\n

This list is the best starting point for deeper code archaeology.

---
## Internal API map (high-level)
Based on the decompiled namespaces and services, the internal API surface splits into:

- **Backend communication**: WCF service clients for software updates, device updates, and key exchange.\n
- **Local communication**: local control plane services and device adapters (serial, protocol multiplexers).\n
- **Crypto and certificates**: certificate store access, KeyVault handling, log export signing/encryption.\n
- **Update/firmware**: managed update orchestration, native raw flash write via `shc_api.dll`.\n
- **Logging/persistence**: file-based logging, USB log export, and data persistence before updates.\n

This provides a map of where to look when extending analysis by function area.

---

## Software resources (what is embedded)
The decompiled assemblies include embedded resources that carry important trust and crypto material:\n

- CA and root bundles (P7B) used for trust stores.\n
- `SHCLogFileEncryptionCertificate` (PEM) used for log export encryption.\n
- Application defaults and configuration strings.\n

This is why some certificates do not appear as standalone files in NAND; they live inside the managed binaries.

Resource files (resx) overview (decompiled):

- **Core trust store bundle**: `RWE.SmartHome.SHC.Core.Properties.Resources.resx` contains 10 binary blobs (`System.Byte[]`) with CA/root material, including `DigiCertGlobalRootCA`, `DigiCertGlobalRootG2`, `MicrosoftRSARootCA2017`, `MicrosoftEVECCRootCA2017`, `DTRUSTRootClass3CA22009`, `SHCRootCA_VerisignNew`, `SHCSubCA_VerisignNew`, plus `shc_ca_certs` and `shc_root_certs`.
- **Log encryption cert**: `RWE.SmartHome.SHC.BusinessLogic.Properties.Resources.resx` contains a single binary blob `SHCLogFileEncryptionCertificate` (used by `LogExporter`).
- **Serial resource**: `RWE.SmartHome.SHC.BusinessLogic.Resources.Resources.resx` contains a single binary blob named `Serial` (used by the business logic layer).
- **Logging pattern**: `RWE.SmartHome.SHC.BusinessLogic.Properties.Resource.resx` contains `LogEntryPattern` (string pattern for log entries).
- **DataAccess schema strings**: `RWE.SmartHome.SHC.DataAccess.Properties.Resources.resx` contains 41 string entries, including schema/table names like `LogicalDevicesXml`, `DeviceActivityLogs`, and `ApplicationsSettings`.
- **Auth and error strings**: multiple `*.ErrorStrings.resx` and `ErrorResources.resx` files contain localized or constant error messages and labels.
- **Dependency resources**: the Microsoft.Practices Mobile ContainerModel resource file contains a few string error messages.

Overall, the embedded resources are mostly strings plus a small set of binary blobs for trust and log encryption. This explains why several certs are present in the binaries even when not found as standalone files in NAND.

**Coprocessor firmware blob (Serial)**

The `Serial` resource (32,768 bytes) is used as the AVR coprocessor firmware image during updates. The updater validates a CRC over the blob and then flashes it.

```csharp
// CoprocessorUpdater.CheckCoprocImageIntegrity
byte[] serial = Resources.Serial;
foreach (byte val in serial) crc.CRC16_update(val);
string text = crc.CRC16_High.ToString(\"X2\") + crc.CRC16_Low.ToString(\"X2\");
if (text != configuration.TargetCoprocessorChecksum)
    throw new CoprocessorUpdateException(...);

// CoprocessorUpdater.FlashCoprocessor
using MemoryStream data_to_write = new MemoryStream(Resources.Serial);
isFlashingSuccessful = AVRFirmwareManager.UpdateAVR(data_to_write);
```

We exported the blob to `extracted_resources/Serial.bin` for further inspection.

**Serial.bin deep analysis (AVR image)**

Binary characteristics:

- Size: 32,768 bytes (0x8000), typical of AVR flash images.
- Entropy: ~5.45 bits/byte (not compressed/encrypted).
- Padding: large 0xFF region from 0x51A6 (10,842 bytes) and 0x00 tail from 0x7C00 (1,024 bytes).
- Code region: non-0xFF/0x00 data from ~0x0100 to ~0x506C.

Vector table (assuming 4-byte slots, 26 entries at 0x0000-0x0067):

```
v00 @0x0000: JMP 0x4DD4
v01 @0x0004: JMP 0x0842
v02 @0x0008: JMP 0x0850
v03 @0x000C: RETI
v04 @0x0010: RETI
v05 @0x0014: RETI
v06 @0x0018: JMP 0x0B36
v07 @0x001C: RETI
v08 @0x0020: RETI
v09 @0x0024: RETI
v10 @0x0028: RETI
v11 @0x002C: RETI
v12 @0x0030: RETI
v13 @0x0034: RETI
v14 @0x0038: JMP 0x0B08
v15 @0x003C: RETI
v16 @0x0040: RETI
v17 @0x0044: RETI
v18 @0x0048: JMP 0x0CF0
v19 @0x004C: RETI
v20 @0x0050: JMP 0x0CE4
v21 @0x0054: RETI
v22 @0x0058: RETI
v23 @0x005C: RETI
v24 @0x0060: RETI
v25 @0x0064: RETI
```

Disassembly samples (avr-objdump, -b binary -m avr):

```
00000000: 0c 94 ea 26  jmp  0x4dd4
00000004: 0c 94 21 04  jmp  0x0842
00000008: 0c 94 28 04  jmp  0x0850
0000000c: 18 95        reti
...
00004dd4: 0f e9        ldi  r16, 0x9F
00004dd6: 0d bf        out  0x3d, r16
00004de0: 0e 94 01 28  call 0x5002
00004df4: 0c 94 e7 26  jmp  0x4dce
```

Likely MCU family:

- The 0x8000 image size and 26-vector table are consistent with an AVR ATmega328-class device (avr5 family). This is a best-fit identification based on vector count and flash size; the exact MCU is not confirmed in the dump.
- I/O register usage aligns with ATmega328p UART0 mapping (I/O 0x20-0x22 for UCSR0A/B/C, 0x24 for UBRR0L, 0x26 for UDR0) and stack pointer setup via I/O 0x3D/0x3E (SPL/SPH).
- Field note: the board uses a **16 MHz external crystal**, which matches the low fuse setting `0xFF` (external full-swing oscillator, CKDIV8 disabled).

```asm
4e04: out 0x21, r20    ; UCSR0B
4e06: out 0x22, r21    ; UCSR0C
4e28: out 0x20, r16    ; UCSR0A
a90:  out 0x24, r17    ; UBRR0L
a9a:  out 0x26, r16    ; UDR0
```

Bootloader hints:

- No `spm` instruction found in the binary; this suggests the blob is an application image only, not a self-programming bootloader.
- The SHC v2 file `serial_v0.e.bin` is byte-identical to `Serial.bin` (same SHA-256).

**I/O usage and pin hints (AVR I/O space)**

Observed low I/O address usage (from disassembly):

- 0x03/0x04/0x05 (PINB/DDRB/PORTB) with bit ops on PORTB:
  - `sbi 0x05,2`, `cbi 0x05,2` (PB2)
  - `cbi 0x05,3` (PB3)
  - `sbi 0x05,5` (PB5)
  - `sbic 0x03,4` (PB4 input)
- 0x07/0x08 (DDRC/PORTC) touched, suggesting PORTC lines are configured.
- 0x09/0x0B (PIND/PORTD) touched, indicating PORTD use.
- 0x20-0x27 used for UART0 registers (UCSR0A/B/C, UBRR0L, UDR0).
- 0x3D/0x3E and 0x3F used (SPL/SPH, SREG).

If the MCU is ATmega328p, PB2/PB3/PB4/PB5 correspond to the SPI pin group (SS/MOSI/MISO/SCK). The pattern of toggling PB2/PB3/PB5 and reading PB4 is consistent with SPI-style signaling, but the exact peripheral wiring still depends on the board.

**Coprocessor flashing protocol (from decompiled code)**

The SHC does **not** use a UART bootloader. It programs the AVR coprocessor via **SPI ISP** using classic AVR ISP commands. The code streams raw binary data and writes flash in 64-word (128-byte) pages.

Key steps and commands (from `AVRFirmwareManager`):

```csharp
// 1) Programming enable
// 0xAC 0x53 0x00 0x00
array[0] = 0xAC; array[1] = 0x53; array[2] = 0x00; array[3] = 0x00;

// 2) Chip erase
// 0xAC 0x80 0x00 0x00
array[0] = 0xAC; array[1] = 0x80; array[2] = 0x00; array[3] = 0x00;

// 3) Poll ready
// 0xF0 0x00 0x00 0x00
array[0] = 0xF0; array[1] = 0x00; array[2] = 0x00; array[3] = 0x00;
```

Flash write loop (raw binary, 2 bytes at a time):

```csharp
short addr = 0;
short pageWords = 64; // 128 bytes

// Load program memory (low/high byte)
// low:  0x40 0x00 addr data
// high: 0x48 0x00 addr data
SendDataByte(data, (byte)(addr & 0x3F), bDataHigh: false);
SendDataByte(data, (byte)(addr & 0x3F), bDataHigh: true);
addr++;

// When addr % 64 == 0, write page:
// 0x4C high_addr low_addr 0x00
WritePage((short)(addr - pageWords));
```

Optional fuse/lock programming (present but not used in update flow):

```csharp
// 0xAC 0xE0 = lock bits
// 0xAC 0xA0 = low fuse
// 0xAC 0xA8 = high fuse
// 0xAC 0xA4 = extended fuse
WriteLockFuseBits(LockType.LOCK, lockBits);
WriteLockFuseBits(LockType.LOW, fuseLow);
WriteLockFuseBits(LockType.HIGH, fuseHigh);
WriteLockFuseBits(LockType.EXTENDED, fuseExt);
```

Conclusion: the coprocessor is programmed with **SPI ISP**, not a serial bootloader. The `Serial.bin` blob is a raw AVR application image and is flashed directly.

---

## DnsService.exe (native Bonjour/mDNS service)
The USB update package contains a native WinCE ARM binary `DnsService.exe` (`USB Update SHC Classic\\shc\\DnsService.exe`). Static analysis and strings strongly match Apple/Bonjour mDNSResponder behavior (DNS‑SD and mDNS).

Evidence and metadata:

- **Native WinCE ARM**: Machine `0x1c2`, Subsystem `9` (Windows CE GUI), **not .NET**.
- **Network‑centric imports**: `WS2.dll` (socket, WSA*), `iphlpapi.dll` (adapter/IP tables), `COREDLL.dll`.
- **Bonjour/mDNS strings**: `_dns-sd`, `_mdns`, `mDNS_RegisterService`, `DNSServiceBrowse`, `DNSServiceResolve`, and Bonjour registry paths like `SOFTWARE\\Apple Computer, Inc.\\Bonjour\\DynDNS\\...`.
- **PDB path**: `r:\\SmartHome\\Release\\V1.2\\Main\\Source\\RWE.SmartHome.SHC\\App\\Native\\Bonjour\\bin\\Release\\DnsService.pdb`.

Disassembly excerpt (ARM entrypoint wrapper):

```asm
0x00034114: push {r4, r5, lr}
0x00034118: mov r4, r2
0x0003411c: mov r5, r0
0x00034120: bl #0x342d4
0x00034124: mov r1, r4
0x00034128: mov r0, r5
0x0003412c: bl #0x34028
0x00034130: pop {r4, r5, lr}
0x00034134: bx lr
```

Trigger/startup behavior:

- The managed codebase does **not** reference `DnsService.exe` directly.\n
- On WinCE, such services are usually started by **registry service entries** (e.g., `HKLM\\Services\\...`) or a native boot launcher. We did not find an explicit `DnsService` string in the extracted registry hives, so it likely resides in the NK/ROM registry or is started by another native component.

Analysis artifacts:

- `full_raw_2112_artifacts\\dnsservice_metadata.txt`
- `full_raw_2112_artifacts\\dnsservice_imports.txt`
- `full_raw_2112_artifacts\\dnsservice_strings.txt`
- `full_raw_2112_artifacts\\dnsservice_disasm_blocks.txt`

---

## Important storage paths (observed)
- `\\NandFlash\\DevicesKeysStorage.csv` - encrypted device keys
- `\\NandFlash\\local.xml` - KeyVault (master key wrapper)
- `\\NandFlash\\logStore` - legacy logs
- `\\NandFlash\\update.bin` - firmware update image
- `\\NandFlash\\app_update.bin` - application update staging
- `\\NandFlash\\shc.zip` - application update package (final)

These paths are referenced directly in the managed code and match the update and persistence logic.

---

## Startup flow (high-level)
The startup sequence (from `StartupLogic`) includes:

- Time synchronization and network checks.
- Initial registration and certificate discovery.
- Mandatory update check (before full initialization).
- Master key retrieval and persistence (KeyVault creation on first run).
- Restoration of persisted data and logs.

This order matters: the system attempts to ensure valid time and network state before registration and update checks, and it ensures the master key exists before device key operations.\n

---

## Update pipeline (software view)
The .NET update flow is clear and deterministic:

1) Query backend for update availability using client certificate authentication (WCF over HTTPS).
2) Download update to `/NandFlash/temp.bin` using backend-provided credentials.
3) Move to `/NandFlash/update.bin` (firmware) or `/NandFlash/app_update.bin` (application).
4) Firmware update calls `WriteRawPartition` through `shc_api.dll` after a basic NK header/length check.
5) Application update renames to `/NandFlash/shc.zip` and reboots.

In the managed layer, there is no cryptographic signature verification of the image. Any signature enforcement would be in native code or in the backend distribution path.

---

## Default update behavior (community documentation, Classic SHC v1 only)
Community guidance describes two primary update paths for the Classic SHC v1:

1) Online update via the cloud service. The SHC checks periodically (roughly daily) and can also check on restart. Updates are presented in the app when available. Multiple sequential updates may be required if the device is far behind.

2) USB update as a fallback if online updates fail. The Classic SHC v1 uses two separate artifacts:
   - **OS update**: a raw WinCE image (typically `nk_signed.bin`) placed at the USB root.
   - **Application update**: a ZIP package (`shc.zip`) placed at the USB root.

The USB process is driven at boot: power off, insert FAT32-formatted USB stick, power on, and wait for the display prompt indicating USB removal. The device then completes the update process after the stick is removed.

This aligns with the managed code behavior: the firmware update path handles a raw image file (`update.bin`) and the app update path handles a ZIP (`shc.zip`). The .NET layer itself performs only basic structural checks for firmware images, not cryptographic signature verification. Any stronger validation must occur in native code or on the server side.

---

## Firmware update validation (managed code)
The managed layer validates the raw OS image structurally. It checks the NK header signature (`B00FF\\n`), record sizes, and overall length consistency. There is no cryptographic signature verification in this layer.

Code excerpt (simplified from `FirmwareImage.CheckFirmwareImage`):

```csharp
private static bool CheckNKSignatureHeader(byte[] buffer)
{
    if (buffer[0] == 66 && buffer[1] == 48 && buffer[2] == 48 &&
        buffer[3] == 48 && buffer[4] == 70 && buffer[5] == 70)
    {
        return buffer[6] == 10; // 'B00FF\n'
    }
    return false;
}
```

After validation, the update is written via native code:

```csharp
if (WinCEFirmwareManager.WriteRawPartition("/NandFlash/update.bin"))
{
    // mark update state, delete file, reboot
}
```

If stronger integrity checks exist, they would have to be in native code (`shc_api.dll`) or enforced by the backend before delivery.

---

## Application update handling (USB ZIP path, managed code)
Application updates are handled as a ZIP file. The managed layer moves the downloaded file to `/NandFlash/shc.zip` and reboots. There is no managed-layer checksum or signature verification for the ZIP.

Code excerpt (simplified from `SoftwareUpdateProcessor.UpdateApplication`):

```csharp
private void UpdateApplication(bool downloadOnly)
{
    File.Delete("/NandFlash/shc.zip");
    File.Delete("/NandFlash/update.bin");
    File.Move("/NandFlash/app_update.bin", "/NandFlash/shc.zip");
    if (!downloadOnly)
    {
        UpdatePerformedHandling.SetUpdatePerformedState(
            UpdatePerformedStatus.Controlled | UpdatePerformedStatus.ApplicationOnly,
            flush: true);
        PersistLog();
        ResetPlatform(); // reboot
    }
}
```

This matches the USB guidance: the ZIP is placed at the root of the USB stick, the system copies it to NAND at boot, and the reboot triggers the app update apply step.

---

## Local operation without cloud (technical summary)
Local operation is possible because the SHC runs a full local stack and exposes local services. With cloud services no longer the primary path, practical local use relies on:\n
- Local access to the SHC’s UI and local services.\n
- Community-provided local bindings/integrations that talk to the SHC directly.\n
- USB updates for OS/app where necessary.\n

This does not alter the KeyVault encryption model: private keys and master keys remain protected by the TPM, and local operation does not bypass that boundary.

---

## Native validation (unknown scope)
The managed layer calls into `shc_api.dll` for raw partition writes (`WriteRawPartition`, `EraseRawPartition`) and registry backup/restore. These are ordinal exports in the native DLL, which suggests the managed layer is trusting a native implementation for any deeper checks.

From the current native analysis:
- We can enumerate exports and see strings related to NAND/bootloader access and cert backup paths.
- We do **not** yet see any explicit signature- or hash-verification strings tied to NK validation inside the DLL.

That means signature enforcement is still **possible**, but it is not confirmed in the native layer we extracted. If it exists, it may live in:
- Bootloader code (outside `shc_api.dll`), or
- Another native component invoked during update apply, not the managed staging step.

Conclusion: managed code only performs structural checks; native enforcement remains unverified and requires deeper disassembly to confirm or rule out.

---

## Software security observations (managed layer)
These are observations about the managed code paths and are not a statement about the full system security posture:

- Firmware validation in .NET checks only an NK header signature and size consistency. There is no managed-layer cryptographic signature verification.
- The download path supports both binary and base64-wrapped payloads and accepts backend-supplied credentials, which increases reliance on backend trust.
- TLS settings in the WCF stack emphasize compatibility (`TlsCipherSuite.Fast`, SHA1 in some places), which is outdated by current standards.

Any stronger enforcement may exist in native code or on the server side.
