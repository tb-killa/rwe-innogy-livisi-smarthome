# SHC v1 NAND Analyse - strukturierte technische Notizen (Deutsch)








## Inhaltsverzeichnis
- [Produktkontext (was es ist, Hersteller, aktueller Status)](#produktkontext-was-es-ist-hersteller-aktueller-status)
- [Hardware](#hardware)
- [Software-Stack](#software-stack)
- [RF-Protokolle und Geraetekommunikation (aus Code)](#rf-protokolle-und-geraetekommunikation-aus-code)
- [Protokoll-UEbersicht: BidCoS vs CoSIP (aus Code)](#protokoll-uebersicht-bidcos-vs-cosip-aus-code)
- [Protokollnutzung nach Geraetetyp (aus Code)](#protokollnutzung-nach-geraetetyp-aus-code)
- [BidCoS vs SIPcos Gegenueberstellung (technisch)](#bidcos-vs-sipcos-gegenueberstellung-technisch)
- [Autorschaft-Hinweise (Code-Metadaten)](#autorschaft-hinweise-code-metadaten)
- [BidCoS Key-Verwendung und Validierung (v1)](#bidcos-key-verwendung-und-validierung-v1)
- [Externer Kontext zu RF-Protokollen und Vendor (oeffentliche Quellen)](#externer-kontext-zu-rf-protokollen-und-vendor-oeffentliche-quellen)
- [NAND-Akquisition und Integritaet](#nand-akquisition-und-integritaet)
- [ROM/XIP Extraktion](#romxip-extraktion)
- [Persistente Registry (EKIM)](#persistente-registry-ekim)
- [ROM/XIP Modul-Metadaten](#romxip-modul-metadaten)
- [KeyVault XML (voller Inhalt)](#keyvault-xml-voller-inhalt)
- [Zertifikate (OpenSSL Output)](#zertifikate-openssl-output)
- [Zertifikats-Carving (Dump-Scan)](#zertifikats-carving-dump-scan)
- [Default-User PFX (Cluj-SMARTHOME-SHCDefault00001.pfx)](#default-user-pfx-cluj-smarthome-shcdefault00001pfx)
- [Registry-Hives und Cert-Stores (hvtool Dump)](#registry-hives-und-cert-stores-hvtool-dump)
- [CE-Zertifikatsspeicher-Carving (Raw NAND)](#ce-zertifikatsspeicher-carving-raw-nand)
- [Zertifikatsgueltigkeit (Not After)](#zertifikatsgueltigkeit-not-after)
- [Ablaufdatum - moegliche Auswirkungen](#ablaufdatum-moegliche-auswirkungen)
- [Ablaufdatum und DeviceKey-Krypto](#ablaufdatum-und-devicekey-krypto)
- [KeyVault crypto flow (software view)](#keyvault-crypto-flow-software-view)
- [Device Keys (software view)](#device-keys-software-view)
- [DeviceKey Encode/Decode Pfad (aus Code)](#devicekey-encodedecode-pfad-aus-code)
- [DeviceKey CSV Erzeugung (wie Zeilen entstehen)](#devicekey-csv-erzeugung-wie-zeilen-entstehen)
- [SGTIN Erzeugung und Serial-Mapping (Code und Beispiele)](#sgtin-erzeugung-und-serial-mapping-code-und-beispiele)
- [DeviceKey Lebenszyklus (lesbarer Ablauf)](#devicekey-lebenszyklus-lesbarer-ablauf)
- [KeyVault zu DeviceKey Kette (lesbarer Ablauf)](#keyvault-zu-devicekey-kette-lesbarer-ablauf)
- [Log-Export Krypto-Flow (lesbar)](#log-export-krypto-flow-lesbar)
- [DeviceKey Struktur und Krypto-Pfad (CSV)](#devicekey-struktur-und-krypto-pfad-csv)
- [DeviceKey CSV Struktur (Felder und Format)](#devicekey-csv-struktur-felder-und-format)
- [Base64 Ciphertext Analyse (sanitisiert)](#base64-ciphertext-analyse-sanitisiert)
- [AES Konstruktion (wie der Code arbeitet)](#aes-konstruktion-wie-der-code-arbeitet)
- [DeviceKey Validierung](#devicekey-validierung)
- [Log-Export und Verschluesselung (USB)](#log-export-und-verschluesselung-usb)
- [Log-Verschluesselung (Standardverhalten)](#log-verschluesselung-standardverhalten)
- [Log-Verschluesselung und MasterKey (Klarstellung)](#log-verschluesselung-und-masterkey-klarstellung)
- [Log-Decrypt-Key Verfuegbarkeit (Dump-View)](#log-decrypt-key-verfuegbarkeit-dump-view)
- [Logging-Endpunkte und lokale Speicherung](#logging-endpunkte-und-lokale-speicherung)
- [settings.config und boot.config (USB-Update + Dump-Korrelation)](#settingsconfig-und-bootconfig-usb-update-dump-korrelation)
- [DeviceKey Export (USB CSV vorhanden)](#devicekey-export-usb-csv-vorhanden)
- [Software APIs und interne Services (Ueberblick)](#software-apis-und-interne-services-ueberblick)
- [Backend/API-Endpunkte und Payloads (v1)](#backendapi-endpunkte-und-payloads-v1)
- [DNS-Status der Backend-Domains](#dns-status-der-backend-domains)
- [shc_api.dll (native Flash/Registry Bridge)](#shcapidll-native-flashregistry-bridge)
- [Interne API-Landkarte (High-Level)](#interne-api-landkarte-high-level)
- [Interne Klassen (ausgewaehlte Kernbausteine)](#interne-klassen-ausgewaehlte-kernbausteine)
- [Software-Resources (eingebettete Inhalte)](#software-resources-eingebettete-inhalte)
- [DnsService.exe (native Bonjour/mDNS Komponente)](#dnsserviceexe-native-bonjourmdns-komponente)
- [Warum ECB verwendet wurde (und was das bedeutet)](#warum-ecb-verwendet-wurde-und-was-das-bedeutet)
- [Update Pipeline (software view)](#update-pipeline-software-view)
- [Standard-Updateverhalten (Community-Dokumentation, Classic SHC v1)](#standard-updateverhalten-community-dokumentation-classic-shc-v1)
- [Firmware-Validierung (Managed Code)](#firmware-validierung-managed-code)
- [Application-Update (USB ZIP, Managed Code)](#application-update-usb-zip-managed-code)
- [Lokaler Betrieb ohne Cloud (technische Zusammenfassung)](#lokaler-betrieb-ohne-cloud-technische-zusammenfassung)
- [Native Validierung (unbekannter Umfang)](#native-validierung-unbekannter-umfang)
- [Software-Security Beobachtungen (Managed Layer)](#software-security-beobachtungen-managed-layer)
- [Wichtige Speicherpfade (beobachtet)](#wichtige-speicherpfade-beobachtet)
- [Startup Ablauf (High-Level)](#startup-ablauf-high-level)

## Produktkontext (was es ist, Hersteller, aktueller Status)
Die SHC v1 (Classic SmartHome Controller) ist die Zentraleinheit der RWE/innogy/LIVISI SmartHome Produktlinie. Es handelt sich um ein Windows Embedded CE 6.0 Geraet, das lokal arbeitet, aber historisch Cloud-Dienste fuer Updates und Remote-Zugriff genutzt hat. Nach dem 1. Maerz 2024 ist der Cloud-Betrieb nicht mehr der primäre Weg; Community-Dokumentation fokussiert deshalb auf lokalen Betrieb und USB-Updates fuer Legacy-Geraete.

Praktisch bedeutet das: Die Zentrale kann weiterhin lokal genutzt werden, aber Updates und Remote-Zugriff laufen ueber lokale Werkzeuge und Community-Integrationen statt ueber die Hersteller-Cloud.

## Hardware
- SoC: AT91SAM9G20
- TPM: AT97SC3204 (TPM CSP in der Registry registriert)
- NAND: K9F2G08U0C
- OS: Windows Embedded CE 6.0

Warum das wichtig ist: Ein TPM deutet darauf hin, dass Private Keys hardwaregebunden und nicht exportierbar sind. Das passt zu den Dump-Funden (nur oeffentliches Material).

---

## Software-Stack
- Boot/ROM: WinCE XIP/ROM Image
- Managed Layer: .NET CF Assemblies
- Native Layer: `shc_api.dll` und Plattform-DLLs

Der Managed Layer steuert KeyVault, Device-Key Entschluesselung und Update-Flows. Starke Validierung (Signaturen) ist eher in nativen Komponenten oder serverseitig zu erwarten.

---

## RF-Protokolle und Geraetekommunikation (aus Code)
Im dekompilierten Code sind mehrere Protokoll-Stacks und eine SerialAPI-Schicht sichtbar. Wichtige Hinweise:

- **BidCos/SerialAPI**: Klassen wie `BIDCOSHeader`, `BidCosHandler2`, `BIDCOSNode` deuten klar auf BidCos. Das wird haeufig mit eQ-3 Hardware assoziiert und stuetzt die Vermutung eines eQ-3-Stacks als Ursprung fuer Teile der RF-Kommunikation.
- **SIPcos**: Vollstaendige SIPcos-Handler (`SIPcosHeader`, `SIPcosHandler`, `SIPcosFrameType`) mit Command-Handlern und Security-Modi.
- **wMBus**: Ein eigener wMBus-Protokolladapter ist vorhanden.
- **Lemonbeat**: Lemonbeat-DomainModel und Protokolladapter mit Actions/Status/StateMachines.

Das zeigt eine Multi-Protokoll-Architektur, bei der unterschiedliche RF-Geraetefamilien ueber eine gemeinsame Kontrollschicht angebunden werden.

Board-Pinout Hinweis (Community CSV):

- `RWE_Zentrale_Pinbelegung.csv` listet **ST400** als `PB5 (RXD0)` und `PB4 (TXD0)`; das sind UART0 Pins des Main CPU.
- Die gleiche Datei listet **SPI0** an **ST101** (`PA0/PA1/PA2/PA3` fuer MISO/MOSI/SPCK/NPCS0).
  - In der CSV stehen **keine** TRX868/CC1101 oder GDO0/GDO2 Hinweise, die exakte Leitung ist damit dort nicht belegt.
  
---

## Protokoll-UEbersicht: BidCoS vs CoSIP (aus Code)
Im dekompilierten Code sind **BidCoS** und **CoSIP/SIPcos** klar getrennte RF-Stacks. BidCoS entspricht der HomeMatic-Familie, CoSIP basiert auf dem CORESTACK-Header mit Routing und MAC-Security.

### BidCoS (HomeMatic-Familie)
Belege und Struktur:
- Eigener BidCoS-Header mit 9-Byte-Header (Frame Counter, Header Bits, Frame Type, 3-Byte Sender, 3-Byte Receiver).
- BidCoS wird fuer bestimmte eQ-3 Geraetetypen genutzt (Rauchmelder und Sirene in diesem Code).
- BidCoS-Nachrichten koennen ueber SIPcos transportiert werden, bleiben aber ein eigenes Frame-Format.

Codefragmente:

```csharp
// BidCoS-Header Parsing (9 Bytes)
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
// BidCoS-Device-Typen im Sysinfo-Frame
case 66:  deviceType = BIDCOSDeviceType.Eq3BasicSmokeDetector; break;
case 170: deviceType = BIDCOSDeviceType.Eq3EncryptedSmokeDetector; break;
case 249: deviceType = BIDCOSDeviceType.Eq3EncryptedSiren; break;
```

Fundstellen:
- `SerialAPI/BIDCOSHeader.cs`
- `SerialAPI/BidCoSFrames/BIDCOSSysinfoFrame.cs`
- `SerialAPI/BidCosLayer/*`

### CoSIP / SIPcos
Belege und Struktur:
- SIPcos nutzt CORESTACK-Header mit Routing, MAC-Security und Frame Types.
- SIPcos hat danach einen eigenen 2-Byte-Header (Frame Type + Flags + Sequence Number).
- Frame-Type-Menge umfasst Konfiguration, Status, Routing, Firmware-Update, Schalten und Zeitinformationen.

Codefragmente:

```csharp
// CORESTACK-Header Parsing (Routing + MAC-Security)
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
// SIPcos-Header (2 Bytes nach CORESTACK)
m_sipcosFrameType = (SIPcosFrameType)(data[0] & 0x3F);
m_stayAwake = (data[0] & 0x40) == 64;
m_bidi = (data[0] & 0x80) == 128;
m_sequenceNumber = data[1];
```

```csharp
// SIPcos Frame-Type Map
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

Fundstellen:
- `SerialAPI/CORESTACKHeader.cs`
- `SerialAPI/SIPcosHeader.cs`
- `SerialAPI/SIPcosFrameType.cs`
- `SipcosCommandHandler/*`

### Protokoll-Auswahl im Device-Info
SIPcos Device-Info-Frames deklarieren explizit, welches Protokoll ein Device nutzt.

```csharp
public enum DeviceInfoProtocolType : byte
{
    SIPcos,
    BIDcos
}
```

Das wird genutzt, um Inklusion und Security pro Device zu steuern.

---

### HomeMatic AES Challenge-Response Aequivalenz (BidCoS)
Der HomeMatic AES Challenge-Response Ablauf aus der oeffentlichen Analyse passt hier nahezu 1:1. Der Code bildet einen Session-Key per XOR (6-Byte Challenge, auf 16 Bytes gepadded) mit dem AES-Key und verwendet danach ein zweistufiges AES-Verfahren mit einem XOR gegen den m-Frame-Teil. Das wird fuer BidCoS-Geraete mit Verschluesselung genutzt (z.B. WSD2 und Sirene).

Codefragment:

```csharp
// Session-Key aus Challenge (auf 16 Bytes gepadded) XOR AES-Key
sessionKey = XORArray(PadArray(challengeBytes, 16), aesKey);

// AES(payload) = Enc( Enc(random6||m[0..9]) XOR m[10..25] )
byte[] b = originalMessage.Take(10).ToArray();
byte[] input = AppendArray(random6Bytes, b);
byte[] a = Encrypt(input);
byte[] b2 = originalMessage.Skip(10).Take(16).ToArray();
byte[] input2 = XORArray(a, b2);
byte[] array = Encrypt(input2);
```

Fundstellen:
- `SerialAPI/AesChallengeResponse.cs`
- `SerialAPI.BidCosLayer.DevicesSupport.Wsd2/ReceiveFrameHandlerAnswer.cs`
- `SerialAPI.BidCosLayer.DevicesSupport.Sir/ReceiveFrameHandlerAnswer.cs`

Referenz (extern): https://git.zerfleddert.de/hmcfgusb/AES/

---

## Protokollnutzung nach Geraetetyp (aus Code)
Basierend auf den Protokoll-Stacks und den Device-Mappings in diesem Repo:

### BidCoS-only Geraete (explizit in BidCoS Sysinfo/Adapter)
- **Eq3BasicSmokeDetector** (WSD)
- **Eq3EncryptedSmokeDetector** (WSD2)
- **Eq3EncryptedSiren** (SIR)

Fundstellen:
- `SerialAPI/BidCoSFrames/BIDCOSSysinfoFrame.cs`
- `SerialAPI/BidCosLayer/*`

### CoSIP/SIPcos Geraete (ueber SIPcos-Stacks/Adapter)
Diese Typen sind im Builtin-Mapping gelistet und werden ueber SIPcos behandelt:
- `RST`, `RST2` (Radiator Thermostat)
- `WRT` (Room Thermostat)
- `FSC8` (Floor Heating Control)
- `PSS`, `PSSO` (Pluggable Switch)
- `WSC2` (Wall Controller)
- `BRC8` (Basic Remote)
- `WMD`, `WMDO` (Motion Detector)
- `WDS` (Door/Window Sensor)
- `PSD` (Pluggable Dimmer)
- `PSR` (Router)
- `ISS2`, `ISD2`, `ISC2`, `ISR2` (In-wall Geraete)
- `RVA`, `ChargingStation`, `PresenceDevice` (weitere gemappte Typen)

Fundstellen:
- `RWE.SmartHome.SHC.DeviceManagerInterfaces/PhysicalDeviceFactory.cs`
- `RWE.SmartHome.Common.ControlNodeSHCContracts.WinCE/RWE.SmartHome.Common.ControlNodeSHCContracts.Entities.Configuration.Enums/BuiltinPhysicalDeviceType.cs`
- `RWE.SmartHome.SHC.SipCosProtocolAdapter/*`

Hinweis: Die Protokollauswahl kommt aus `DeviceInfoProtocolType` (SIPcos vs BIDcos) in Device-Info-Frames.

---

## BidCoS vs SIPcos Gegenueberstellung (technisch)
Lesbare Gegenueberstellung aus dem Code:

| Aspekt | BidCoS | SIPcos / CoSIP |
| --- | --- | --- |
| Header-Layout | 9-Byte BidCoS-Header: Frame Counter, Header Bits, Frame Type, 3-Byte Sender, 3-Byte Empfaenger | CORESTACK-Header (Routing + MAC-Security) + 2-Byte SIPcos-Header (Frame Type + Flags + Sequence Number) |
| Addressierung | Nur 3-Byte Sender/Empfaenger | MAC + IP Felder, Hop Limit, geroutete Address-Extensions |
| Routing | Kein Routing im Header | Explizite Routing-Modi: FIRST_ROUTED, IN_PATH, LAST_ROUTED |
| Security im Header | Keine MAC-Security Felder im BidCoS-Header | MAC-Security Flag + MIC + Sequence Counter im CORESTACK |
| AES Challenge/Response | Fuer verschluesselte BidCoS-Geraete (WSD2/Sirene) | Nicht im SIPcos Flow genutzt |
| Frame Types | BidCoS-spezifische Typen (separate Enums) | SIPcos Frame Types: NETWORK_MANAGEMENT, ROUTE_MANAGEMENT, FIRMWARE_UPDATE, STATUSINFO, SWITCH, usw. |
| Stack-Pfade | `SerialAPI.BidCosLayer/*`, `SerialAPI.BidCoSFrames/*` | `SerialAPI/SIPcos*`, `SipcosCommandHandler/*` |

Namenshinweis: Im Code werden **CoSIP** und **SIPcos/SIPcos** fuer denselben Protokoll-Stack verwendet. High-Level nutzt `ProtocolIdentifier.Cosip`, der Low-Level-Stack heisst `SIPcos*`.

Code-Referenzen:
- BidCoS Header: `SerialAPI/BIDCOSHeader.cs`
- CORESTACK Header: `SerialAPI/CORESTACKHeader.cs`
- SIPcos Header/Types: `SerialAPI/SIPcosHeader.cs`, `SerialAPI/SIPcosFrameType.cs`
- AES Challenge/Response: `SerialAPI/AesChallengeResponse.cs`

---

## Autorschaft-Hinweise (Code-Metadaten)
Es gibt keinen expliziten "CoSIP authored by X" Hinweis im Code. Die staerksten Zuordnungen kommen aus Assembly-Metadaten und Namespaces:

- Viele zentrale SHC-Assemblies fuehren `AssemblyCompany("Innogy SE")`, inkl. der SIPcos-Protocol-Adapter Module.
  - Beispiel: `RWE.SmartHome.SHC.SipCosProtocolAdapter/Properties/AssemblyInfo.cs`
  - Beispiel: `RWE.SmartHome.SHC.SipCos.TechnicalConfiguration/Properties/AssemblyInfo.cs`
- Einige Shared-Contracts/SDK Assemblies fuehren `AssemblyCompany("RWE")`.
  - Beispiel: `RWE.SmartHome.Common.ControlNodeSHCContracts.WinCE/Properties/AssemblyInfo.cs`
- eQ-3 taucht nur als **Device-Typ-Name** auf, nicht als Protokoll-Autor.
  - Beispiel: `RWE.SmartHome.SHC.DeviceManagerInterfaces/DeviceTypesEq3.cs`
  - Beispiel: `SerialAPI/BidCoSFrames/BIDCOSDeviceType.cs`

Aus dem Repo allein sieht CoSIP/SIPcos wie Teil des RWE/Innogy SmartHome-Stacks aus, waehrend eQ-3 nur fuer bestimmte Device-Typen (hauptsaechlich im BidCoS-Pfad) referenziert wird.

AT91 Pin-Mapping (aus der gleichen CSV):

- **ST400 / UART0**: `PB5` = `RXD0`, `PB4` = `TXD0`
- **ST101 / SPI0**: `PA0` = `SPI0_MISO`, `PA1` = `SPI0_MOSI`, `PA2` = `SPI0_SPCK`, `PA3` = `SPI0_NPCS0`

Community-Hardware-Hinweis (unverifiziert):

- Ein Forenbeitrag beschreibt **ST400** als serielle Schnittstelle zum AVR, **PRG1** als ISP-Programmierheader, und das **TRX868/CC1101** Funkmodul per SPI plus zwei GPIOs (GDO0/GDO2) am AVR.
- Das passt grob zu den Firmware-Hinweisen (SPI-Style I/O und weitere GPIO-Nutzung), ist aber **nicht bestaetigt** ohne Schaltplan oder Board-Trace.

---

## BidCoS Key-Verwendung und Validierung (v1)
Kernpunkte aus dem v1 BidCoS-Stack:

- **SGTIN ist der Lookup-Key**, nicht der Kryptoschluessel. Er dient nur zur Zuordnung des DeviceKeys.
- **Seriennummer ist Anzeige-Logik** (aus SGTIN abgeleitet) und beeinflusst keine Schluessel.
- **Device Keys werden verschluesselt** in `\\NandFlash\\DevicesKeysStorage.csv` gespeichert und erst nach KeyVault-Unwrap entschluesselt.
- **Keine Klartext-Key-Persistenz** fuer BidCoS-Knoten. `DefaultKey` ist `[XmlIgnore]` und wird nicht serialisiert.
- **Wsd2LocalKey ist pro Hub zufaellig**, kein DeviceKey.
- **Validierung ist minimal** (nur Base64-Check im CSV-Pfad).

BidCoS Device-Typen im v1 Code (aus `BIDCOSSysinfoFrame`):

| Device-Typ | Sysinfo-Type-Code | Adapter | Key-Nutzung |
| --- | --- | --- | --- |
| Eq3BasicSmokeDetector (WSD1) | `0x42` (66) | `WsdAdapter` | Keine DeviceKey-Abfrage; Default-Key Logik wird umgangen |
| Eq3EncryptedSmokeDetector (WSD2) | `0xAA` (170) | `Wsd2Adapter` | DeviceKey fuer Inclusion, danach Wechsel auf lokalen Key |
| Eq3EncryptedSiren | `0xF9` (249) | `SirAdapter` | DeviceKey fuer Inclusion, danach Wechsel auf lokalen Key |

```csharp
// BIDCOSSysinfoFrame: DeviceType Mapping aus Sysinfo
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

Codefragmente (dekompiliert):

```csharp
// DeviceKeyRepository: DeviceKey entschluesseln
byte[] key = DecryptDeviceKey(Convert.FromBase64String(array[2]));
storedDevice.Sgtin = Convert.FromBase64String(array[0]);
storedDevice.SerialNumber = array[1];
storedDevice.Key = key;
```

```csharp
// BidCoS WSD2: DeviceKey holen und setzen
bidCosHandler.bidcosKeyRetriever.GetDeviceKey(
    SGTIN96.Create(base.Node.Sgtin),
    key => { base.Node.DefaultKey = key; },
    null, 5000
);
```

```csharp
// BIDCOSNode: Keys werden nicht persistiert
[XmlIgnore]
public byte[] DefaultKey { get; set; }
```

```csharp
// BIDCOSNodeCollection: lokaler WSD2-Key ist zufaellig
Wsd2LocalKey = new byte[16];
new Random().NextBytes(Wsd2LocalKey);
```

Praktische Folgen:
- Ohne den KeyVault-Private-Key kann das verschluesselte CSV nicht entschluesselt werden.
- Es gibt **keine Serial/SGTIN -> DeviceKey Ableitung** im v1 Code.
- Keine Default-User/Passwoerter fuer BidCoS Keys im Code; Keys kommen aus CSV oder Backend.
- **WSD1 braucht keinen DeviceKey**. Ein fehlender CSV-Eintrag verhindert die Inclusion nicht.

Default-Credentials oder Keys:
- **Keine Default-User/Passwoerter** fuer BidCoS Geraete im Code.
- **Kein statischer DeviceKey** ist eingebettet; Keys kommen aus CSV oder Backend.

### Backend Key-Exchange (DeviceKey Abruf)
Wenn lokal kein Key vorhanden ist, ruft die SHC den Key vom Backend anhand der SGTIN ab:

```csharp
// KeyExchangeServiceClient: GetDeviceKey Ablauf
public KeyExchangeResult GetDeviceKey(byte[] sgtin, out byte[] deviceKey)
{
    GetDeviceKeyRequest request = new GetDeviceKeyRequest(sgtin);
    GetDeviceKeyResponse deviceKey2 = GetDeviceKey(request);
    deviceKey = deviceKey2.deviceKey;
    return deviceKey2.GetDeviceKeyResult;
}
```

Das bedeutet: Das **Backend ist die autoritative Quelle** fuer DeviceKeys, wenn das lokale CSV sie nicht enthaelt.

### WSD1 (Basic Smoke Detector)
WSD1 nutzt den normalen BidCoS-Pfad ohne DeviceKey-Retrieval:

```csharp
public override bool EnsureCurrentNodeDefaultKey()
{
    return true;
}
```

Die Inclusion laeuft ueber ConfigBegin/ConfigData/ConfigEnd und Group-Registration. Keine Key-Anfrage aus CSV.

### WSD2 (Encrypted Smoke Detector)
WSD2 holt den DeviceKey und schaltet danach auf den lokalen Hub-Key:

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

Waehrend der Inclusion wird der **DeviceKey** genutzt, danach setzt `ConfigureNodeKey` den **lokalen Key** und `UseDefaultKey` wird `false`.

### Sirene (Encrypted Siren)
Die Siren-Logik entspricht WSD2, aber der lokale Key ist der "private" Hub-Key:

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

Die Sirene nutzt den **DeviceKey** fuer Inclusion und wechselt danach auf den **lokalen Key** fuer laufenden Traffic.

---

## Externer Kontext zu RF-Protokollen und Vendor (oeffentliche Quellen)
Oeffentliche Doku und Community-Quellen ordnen **BidCos** haeufig dem **HomeMatic** Oekosystem und dem Hersteller **eQ-3** zu. Die BidCos/SerialAPI Klassen im dekompilierten Code passen zu dieser Zuordnung, was eine technische Verwandtschaft nahelegt.

Patentinhaberschaft ist **nicht verifiziert**. Fuer belastbare Aussagen muessen offizielle Herstellerdokumente oder Patentdatenbanken herangezogen werden.

---

## NAND-Akquisition und Integritaet
Wir analysierten ein einzelnes 256 MB Raw NAND Image. Ein einfacher Check zeigt viele 0xFF Bereiche (normal fuer geloeschten NAND), ist aber keine 100 Prozent Garantie. OOB/ECC Daten fehlen, ein zweiter Dump zum Vergleich existiert nicht.

---

## ROM/XIP Extraktion
Ein WinCE ROM/XIP Extract am erwarteten Offset lieferte:
- `nk.exe` und eine komplette DLL Sammlung
- `boot.hv`, `default.hv`, `user.hv`

Die Hives dekodieren sauber und verweisen auf ROM P7B Zertifikats-Bundles. Der TPM CSP ist registriert, Private Key Container tauchen nicht auf.

---

## Persistente Registry (EKIM)
EKIM Bloecke sind vorhanden, aber nicht als komplette Hives decodierbar. Beobachtungen:
- EKIM Header sind pro Gruppe konstant.
- Payloads sind ~95 Prozent Null.
- Nicht-Null Bytes sind fix positioniert und wiederholen sich.
- Variable Felder zeigen in ROM/XIP Regionen statt in Registry Cells.

Fazit: Das sieht nach Metadaten/Platzhaltern aus, nicht nach echter persistenter Registry.

---

## ROM/XIP Modul-Metadaten
Wir fanden dichte `.dll` Namenscluster im Bereich `0x00d00000-0x00d40000`. Der Kontext enthaelt Funktionsnamen aus Crypto/Cert APIs (z. B. `PFXImportCertStore`). Scans nach Pointer-Arrays, Offset/Size Paaren, 16-bit Indices und Hash-Tabellen ergaben keine klare Modul-Tabelle. Das wirkt wie gepackte Metadaten, nicht wie eine einfache Directory-Struktur.

---

## KeyVault XML (voller Inhalt)
Wir scannten den Dump nach XML Strukturen und filterten auf `KeyVault`, `MasterKey` und `EncryptionKey`. Ein Fragment enthielt ein vollstaendiges KeyVault Element. Vollstaendiger Rohinhalt:

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

## Zertifikate (OpenSSL Output)
Wir haben `SigningCertContent` zu PEM decodiert und mit OpenSSL untersucht. Ausgabe (verbatim):

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

Zusätzlich haben wir das eingebettete Log-Encryption Zertifikat (`SHCLogFileEncryptionCertificate`) extrahiert und mit OpenSSL geprueft:

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

## Zertifikats-Carving (Dump-Scan)
Wir haben den NAND-Dump direkt auf PEM-Blocks und DER-encodierte X.509 Sequenzen gescannt:

- **PEM**: ein Zertifikatsblock gefunden und als `SHCLogFileEncryptionCertificate` geparst (Issuer `SHMPROD-CA-S`). Keine PEM Private Keys gefunden.
- **DER**: 200 DER-Kandidaten gecarvt; OpenSSL konnte viele als X.509 parsen. Enthalten sind Windows/VeriSign Roots und mehrere SHM/RWE Zertifikate (`RWE_SmartHome_SHC_Codesigning`, `SHMPROD-CA-S`, `SHMPROD-CA-E`, `SMARTHOME01`), oft mehrfach durch Resource-Embedding. Nach Dedupe (Subject/Issuer/Serial/Datum) bleiben **37 eindeutige Zertifikate** (168 geparste Eintraege).
- **PFX/PKCS12**: keine gueltigen PKCS12 Container gefunden.
- **Private Keys**: ein einzelner DER-codierter RSA Private Key wurde gefunden (1024-bit). Er matcht **keinen** der gecarvten Zertifikats-Moduli und ist kein PKCS12 Bundle. Vermutlich ein Standalone/Test-Key oder fremdes Artefakt. Offset im Dump: `0xC544BCB`.
- **PKCS#8 (verschluesselt)**: vier PKCS#8 encrypted Key-Blobs gecarvt. Alle zeigen OID `1.2.840.113549.1.12.1.3` (pbeWithSHAAnd3-KeyTripleDES-CBC). Ohne Passphrase sind Inhalte nicht auslesbar; moeglich sind False-Positives oder fremde/irrelevante Blobs.
- **Cert-Backup-Pfade**: UTF-16LE-Strings fuer `\NandFlash\CertBackupLocal` / `\NandFlash\CertBackupUser` tauchen in OS-Message-Strings auf, aber es wurden keine Backup-Dateien oder PFX-Blobs im Rohdump gefunden.
- **P7B Bundles**: `shc_root_certs.p7b`, `shc_ca_certs.p7b`, `shc_codesign_certs.p7b` und `sysroots.p7b` zu PEM extrahiert. Keine Private-Key-Marker gefunden. Beispiel-Subjects: `RWE_SmartHome_SHC_Codesigning`, `SHMPROD-CA-S`, `SHMPROD-CA-E`.
- **PKCS#8 Passphrases**: kleine Liste an Standard-Passwoertern getestet (leer, `password`, `123456`, `shc`, `smarthome`, `livisi`, `rwe`, `innogy`, `osboxes.org` usw.), keiner entsperrte die PKCS#8 Blobs.
- **Naechster Schritt (offline)**: groessere Wordlist mit Hashcat/John gegen die PKCS#8 Blobs laufen lassen; benoetigt PBE-Hash-Extraktion und eine saubere Wordlist.

Das stuetzt die bisherige Annahme: Oeffentliche Zertifikate liegen in ROM/Resources, private Keys liegen vermutlich im TPM oder geschuetzten Store und sind nicht als Klartext im NAND-Dump vorhanden.

---

## Default-User PFX (Cluj-SMARTHOME-SHCDefault00001.pfx)
Wir haben das PFX-Passwort waehrend der Analyse ermittelt und den Inhalt mit OpenSSL extrahiert.

- **Passwort**: `Test1234!`
- **Keybag**: `pbeWithSHA1And3-KeyTripleDES-CBC`, Iteration `2000`
- **Cert bag**: `pbeWithSHA1And40BitRC2-CBC`, Iteration `2000`
- **Inhalt**: 1x Private Key (PKCS#8 RSA 2048) + 1x Zertifikat

OpenSSL (Summary):

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

Verwendung im Code:

- `CertificateManager` sucht im **MY** Store nach einem Default-Client-Zertifikat (DN Search String) und kann ein PFX importieren, falls `DefaultCertificateFile`/`DefaultCertificatePassword` konfiguriert sind.
- Der harte Search-String ist `local.rwe.shmprod.Smarthome.Client.SHCDefaultCertificates` (das PFX hier ist `local.smarthome...`, also vermutlich Factory/Umgebungs-spezifisch).
- Dieses Default-User-Zertifikat dient der Client-Identitaet und lokalen Services; es ist **nicht** das KeyVault-Signing-Cert fuer den MasterKey-Unwrap.


Code-Fragment (Default-Cert Import + Suche):

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

Zusaetzliche PFX-Dateien mit waehrend der Analyse ermittelten Passwoertern (OpenSSL-Details):

**1) SHCDefault-Essen-SHMPROD.pfx**
- Passwort: `Test1234!`
- Subject: `CN=SHCDefaultUserTest, OU=SHCDefaultCertificates, OU=Client, OU=Smarthome, DC=local, DC=rwe, DC=shmprod`
- Issuer: `CN=SHMPROD-CA-S, DC=local, DC=rwe, DC=shmprod`
- Gueltigkeit: `2010-07-07` bis `2040-07-07`
- EKU: `TLS Web Client Authentication`, `E-mail Protection`, `Microsoft Encrypted File System`
- Key Usage: `Digital Signature`, `Key Encipherment`
- SAN: `UPN=SHCDefaultUserTest@shmprod.rwe.local`
- Basic Constraints: `CA:FALSE`

**2) default_cert_testwe0.pfx**
- Passwort: `pass`
- Subject: `CN=SHCAvatarDefUsr00001, O=Livisi, L=Dortmund, ST=NW, C=DE`
- Issuer: `CN=CA, OU=accounting, O=Internet Widgits Pty Ltd, L=city, ST=Some-State, C=AU`
- Gueltigkeit: `2019-03-27` bis `2029-03-24`
- EKU: `TLS Web Server Authentication`, `TLS Web Client Authentication`, `Code Signing`, `E-mail Protection`
- Key Usage: `Digital Signature`, `Non Repudiation`, `Key Encipherment`
- SAN: `UPN=SHCAvatarDefUsr00001`
- Basic Constraints: `CA:FALSE`

Usage-Einschaetzung:
- Diese Zertifikate passen zum **Default-Client-Zertifikat** Pfad in `CertificateManager` (Import in den `MY` Store).
- Sie sind **nicht** das KeyVault-Signing-Cert fuer den DeviceKey-Unwrap.
- Die SAN-Felder enthalten nur UPNs, **keine DNS/Wildcard** Eintraege.

---
---

## Registry-Hives und Cert-Stores (hvtool Dump)
Wir haben `boot.hv`, `default.hv` und `user.hv` mit `hvtool` in `.reg` konvertiert und nach Cert-Stores/Key-Containern gesucht:

- `HKLM\\Comm\\Security\\SystemCertificates\\CodeSign` → `InitFile="\\windows\\shc_codesign_certs.p7b"`
- `HKLM\\Comm\\Security\\SystemCertificates\\CA` → `InitFile="\\windows\\shc_ca_certs.p7b"`
- `HKCU\\Comm\\Security\\SystemCertificates\\Root` → `InitFile="\\windows\\shc_root_certs.p7b"`
- TPM CSP ist registriert als `SHC Trusted Platform Module Cryptographic Service Provider`.

Keine `CertBackupLocal` / `CertBackupUser` Keys und keine Private-Key-Container-Referenzen in den Hives. Das spricht dafuer, dass Private Keys nicht als exportierbare Blobs im NAND liegen.

## CE-Zertifikatsspeicher-Carving (Raw NAND)
Wir haben den Raw-NAND gezielt auf Cert-Store-Artefakte und ASN.1-Blobs untersucht:

- UTF-16LE-Treffer fuer `\NandFlash\CertBackupUser` / `\NandFlash\CertBackupLocal` stammen nur aus OS-Log/Diagnose-Strings (Backup/Restore Meldungen). Es wurden keine echten Backup-Dateien oder PFX-Blobs gefunden.
- 421 ASN.1-DER Kandidaten extrahiert; 48 davon sind gueltige X.509 Zertifikate. Die geparste Liste liegt in `analysis/certs_from_dump/parsed_certs.txt`.
- Ein PKCS#1 RSA Private Key gefunden bei Offset `0x0C544BCB` (607 Byte, 1024-bit). OpenSSL parst ihn als Private Key, aber sein Modulus passt zu **keinem** der extrahierten Zertifikate. In der Umgebung tauchen Strings zu `support@rebex.net` auf, was eher auf einen Test/Sample-Key hindeutet als auf einen Device-Identity-Key.
- `support@rebex.net` ist eine Rebex Kontaktadresse (Rebex bietet .NET Networking/Security Libraries). Das staerkt die Einordnung als Test/Sample-Artefakt und nicht als device-spezifischer Schluessel.
- Keine PEM-Bloecke und keine PKCS#12 Container im Raw-Dump gefunden.

---

## Zertifikatsgueltigkeit (Not After)
Das Feld `Not After : Nov 11 19:34:28 2036 GMT` ist das Ablaufdatum des **SMARTHOME01 Identitaetszertifikats**. Das bedeutet nicht, dass das Geraet automatisch abschaltet. Es bedeutet aber, dass **TLS-Authentisierung mit diesem Zertifikat ab diesem Datum fehlschlaegt**, sofern das Zertifikat nicht erneuert wird. Lokale Funktionen koennen weiter laufen, aber alle Pfade, die dieses Zertifikat fuer Backend oder mTLS benoetigen, sind ab dem Ablauf betroffen.

---

## Ablaufdatum - moegliche Auswirkungen
Wenn die SHC nach dem Ablaufdatum weiterhin dieses Zertifikat nutzt, sind voraussichtlich folgende Bereiche betroffen:

- Backend/Cloud Verbindungen mit Client-Zertifikaten (mTLS) schlagen fehl.
- Update-Checks und Downloads, die auf authentifizierte TLS-Sessions angewiesen sind, schlagen fehl.
- Lokale TLS-Endpunkte, die dieses Zertifikat zur Identitaet nutzen, koennen Warnungen oder Ablehnungen erhalten (je nach Client-Validierung).

Lokale Funktionen ohne TLS-Client-Auth sollten weiter laufen, aber alle Auth-Pfade ueber dieses Zertifikat sind betroffen.

---

## Ablaufdatum und DeviceKey-Krypto
Die DeviceKey-Krypto haengt **nicht** vom Ablaufdatum des X.509 Zertifikats ab. Device Keys werden mit dem MasterKey entschluesselt, und dieser wird ueber die KeyVault-Kette (RSA -> AES) abgeleitet. Das Ablaufdatum beeinflusst TLS-Identitaet und Authentisierung, nicht das Schluesselmaterial selbst. Ein abgelaufenes Zertifikat kann also Kommunikation brechen, aber es macht vorhandene Device Keys nicht ungueltig.

---

## KeyVault crypto flow (software view)
### Wie der KeyVault funktioniert (step-by-step)
Aus `DeviceMasterKeyRepository`:

1) `EncryptionKey` ist ein hex RSA Ciphertext. Decrypt ergibt `AES key || AES IV`.
2) RSA Decrypt nutzt den **Private Key** zum `SigningCertContent` Zertifikat.
3) AES-128 CBC decrypt von `MasterKey`.
4) Die ersten 32 Bytes des MasterKey sind der AES-256 Key fuer die Device Keys.

Wenn der Private Key fehlt, stoppt die Kette sofort.

### Wie das XML erzeugt wird (aus Code)

```csharp
RijndaelManaged aes = new RijndaelManaged();
aes.KeySize = 128;
ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);
RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PublicKey;
byte[] blob = aes.Key || aes.IV;
byte[] encrypted = rsa.Encrypt(blob, false);
xmlWriter.WriteString(Convert.ToBase64String(cert.GetRawCertData()));
```

### Decrypt Pfad

```csharp
KeyVault xml = Deserialize(local.xml);
Certificate cert = FindCertInStore();
RSACryptoServiceProvider rsa = (RSACryptoServiceProvider)cert.PrivateKey;
byte[] aesBlob = rsa.Decrypt(hex(EncryptionKey), false);
byte[] master = AesDecrypt(hex(MasterKey), key, iv);
```

---

## Device Keys (software view)

```csharp
byte[] master = GetMasterKey();
byte[] key = master.Take(32).ToArray();
byte[] deviceKey = AesEcbDecrypt(csvValue, key);
```

AES-256 ECB mit PKCS7 Padding.

---

## DeviceKey Encode/Decode Pfad (aus Code)
Im `DeviceKeyRepository` sind beide Wege sichtbar:

```csharp
// decode: CSV -> Klartext-Key
byte[] cipher = Convert.FromBase64String(csvKey);
byte[] key = DecryptDeviceKey(cipher); // AES-256 ECB, PKCS7

// encode: Klartext-Key -> CSV
byte[] cipher = EncryptDeviceKey(deviceKey);
string csvKey = Convert.ToBase64String(cipher);
```

Der AES-Schluessel kommt aus dem MasterKey:

```csharp
byte[] master = deviceMasterKeyRepository.GetMasterKeyFromFile();
Array.Resize(ref master, 32); // erste 32 Bytes
```

---

## DeviceKey CSV Erzeugung (wie Zeilen entstehen)
Beim Speichern erzeugt der Code die CSV-Zeile explizit:

```csharp
string sgtinB64 = Convert.ToBase64String(deviceKey.SGTIN);
string serial = SerialForDisplay.FromSgtin(deviceKey.SGTIN);
byte[] enc = EncryptDeviceKey(deviceKey.Key);
string keyB64 = Convert.ToBase64String(enc);
string line = $"{sgtinB64},{serial},{keyB64}";
```

Damit ist klar: SGTIN wird base64 gespeichert, die Seriennummer wird aus der SGTIN abgeleitet, und der Key wird immer verschluesselt bevor er in die CSV geschrieben wird.\n

---

## SGTIN Erzeugung und Serial-Mapping (Code und Beispiele)
Die SGTIN liegt als 96-bit Struktur in `SGTIN96` vor und enthaelt `FilterValue`, `Partition`, `CompanyPrefix`, `ItemReference` und `SerialNumber`. Das Packing in die 12 Byte passiert in `GetSerialData()`:

```csharp
list.Add(header); // 0x30
list.Add((byte)(FilterValue << 5));
list[1] |= (byte)(Partition << 2);
int n = itemReferenceBitCount[Partition];
ulong tmp = (CompanyPrefix << n) | ItemReference;
// tmp in Bytes 1..7 packen, danach SerialNumber (Bytes 7..11)
```

Die Anzeige-Seriennummer wird aus der SGTIN abgeleitet (`SerialForDisplay.FromSgtin`):

```csharp
if (sgtin.CompanyPrefix == 4051495 && (sgtin.ItemReference == 91419 || sgtin.ItemReference == 97510))
    return FromBidCosDevice(sgtin);
return ((ulong)sgtin.ItemReference * 10000000 + sgtin.SerialNumber)
    .ToString().PadLeft(12, '0');
```

BidCos-Format nutzt Bitfelder (`FromBidCosDevice`):

```csharp
string digits = (sgtin.SerialNumber & 0xFFFFFF).ToString().PadLeft(7, '0');
ulong vendor = (sgtin.SerialNumber & 0xE0000000) >> 29; // 0=RW,1=WE,2=EQ
char lead = (char)(((sgtin.SerialNumber & 0x1F000000) >> 24) + 65); // 'A'..'Z'
return $"{lead}{prefix}{digits}";
```

Beispiel (Standard-Seriennummer):
- Wenn `ItemReference=12345` und `SerialNumber=6789012`, ergibt sich die Anzeige-Seriennummer `123456789012`.
- Rueckwaerts (Serial -> SGTIN) geht **nur**, wenn `CompanyPrefix`, `Partition` und `FilterValue` bekannt sind. Die Seriennummer allein enthaelt diese Felder nicht.

```csharp
// serial -> SGTIN96 (non-BidCos)
string serial = "123456789012";
uint itemRef = uint.Parse(serial.Substring(0, 5));
ulong sn = ulong.Parse(serial.Substring(5, 7));
var sgtin = new SGTIN96 {
    CompanyPrefix = 4051495, // muss bekannt sein
    Partition = 5,           // muss zur Prefix-Laenge passen
    FilterValue = 0,
    ItemReference = itemRef,
    SerialNumber = sn
};
byte[] sgtinBytes = sgtin.GetSerialData();
```

Fuer BidCos-Seriennummern (`ARW0123456` etc.) muss `SerialNumber` aus Buchstabe + Vendor-Code + Ziffern per Bitfeld-Layout rekonstruiert werden. Eine fertige Reverse-Funktion gibt es im Managed Code nicht; sie laesst sich aus `FromBidCosDevice` ableiten.

---

## DeviceKey Lebenszyklus (lesbarer Ablauf)

```
DeviceKey (Klartext)
    -> EncryptDeviceKey (AES-256 ECB, PKCS7)
    -> Base64
    -> CSV-Zeile (SGTIN, SerialNo, Key)

CSV-Zeile
    -> Base64 decode
    -> DecryptDeviceKey
    -> DeviceKey (Klartext)
```

---

## KeyVault zu DeviceKey Kette (lesbarer Ablauf)

```
KeyVault (local.xml)
    -> RSA decrypt EncryptionKey (braucht Device Private Key)
    -> AES-128 Key + IV
    -> AES-128 CBC decrypt MasterKey
    -> MasterKey Bytes
    -> erste 32 Bytes verwenden
    -> AES-256 ECB decrypt Device Keys
```

---

## Log-Export Krypto-Flow (lesbar)

```
Log-Export (USB)
    -> Signing-Cert laden (personal/default)
    -> Encryption-Cert laden (embedded resource)
    -> AES-128 Log-Content verschluesseln (wenn Cert vorhanden)
    -> RSA verschluesselt AES Key + IV in XML
    -> Content hash erzeugen
    -> Hash mit Device Private Key signieren
```

XML Struktur des Exports:

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

## DeviceKey Struktur und Krypto-Pfad (CSV)
Die Device Keys liegen in einer CSV-Tabelle mit Identifiern (z. B. Serial/SGTIN) und einem **verschluesselten Key-Blob**. Der Blob ist kein Klartext, sondern base64-codierter Ciphertext. Der Ablauf:

1) MasterKey aus dem KeyVault entschluesseln.\n
2) Die ersten 32 Bytes als AES-256 Key verwenden.\n
3) CSV-Feld per Base64 dekodieren.\n
4) AES-256 ECB + PKCS7 entschluesseln.\n

Vereinfachte Logik aus dem dekompilierten Code:

```csharp
byte[] master = GetMasterKey();               // KeyVault decrypt
byte[] aesKey = master.Take(32).ToArray();    // 256-bit
byte[] cipher = Convert.FromBase64String(csvValue);
byte[] deviceKey = AesEcbPkcs7Decrypt(cipher, aesKey);
```

Wenn der MasterKey nicht entschluesselt werden kann (z. B. fehlender Private Key), bleibt die CSV undurchsichtig und die Device Keys lassen sich nicht aus Identifiern ableiten.

---

## DeviceKey CSV Struktur (Felder und Format)
Die CSV hat einen festen Header und pro Geraet eine Zeile:

```
SGTIN,SerialNo,Key
```

Feldbedeutung:
- `SGTIN`: Geraete-Identifikator (String)
- `SerialNo`: Seriennummer (numerischer String)
- `Key`: Base64-encodierter Ciphertext des Device Keys

Beobachtungen zum `Key` Feld:
- Base64 dekodiert zu einem festen Binary-Blob.
- Am Ende wiederholt sich haeufig ein Base64-Suffix, passend zu PKCS7 Padding bei AES-ECB (identischer Padding-Block).

Das passt zur AES-256 ECB + PKCS7 Logik aus dem dekompilierten Code.

Beispielzeile (synthetisch):

```
TESTSGTIN00000001,123456789012,<base64-ciphertext>
```

---

## Base64 Ciphertext Analyse (sanitisiert)
Wir enthalten hier keine echten Ciphertexts. Die folgenden Beobachtungen basieren auf direkter Analyse der CSV Daten:

- **Base64 Laenge ist konsistent** ueber die Zeilen, passend zu AES Blockgroesse (Vielfache von 16 Bytes).
- **Wiederholtes Base64-Suffix** tritt oft auf. Das ist typisch fuer AES-ECB mit PKCS7 Padding: ein identischer Padding-Block fuehrt zu identischem letztem Ciphertext-Block.
- **ECB-Fingerprint**: identische Klartext-Blocks ergeben identische Ciphertext-Blocks, was Wiederholungsmuster sichtbar macht.
- **Keine Schluesselleakage**: diese Muster verraten nicht den Key, sie zeigen nur gleiche Blockwerte an festen Positionen.

Das erklaert, warum der CSV `Key` ohne MasterKey nicht verwendbar ist.

---

## AES Konstruktion (wie der Code arbeitet)
Die Kryptografie nutzt Standardbibliotheken. Es gibt keine eigene Blockcipher-Implementierung; der eigene Teil betrifft die Schluesselkette und das Packaging.

Wichtige Punkte aus den Codepfaden:

- **KeyVault** nutzt AES-128-CBC mit PKCS7. AES Key + IV kommen aus `RijndaelManaged` und werden per RSA verschluesselt.
- **Device Keys** nutzen AES-256-ECB mit PKCS7. Der AES Key sind die ersten 32 Bytes des MasterKey.\n
- **Kein Custom AES**: die eigentliche Blockcipher-Logik kommt aus .NET (`RijndaelManaged`, `CryptoStream`).\n

Beispielmuster fuer AES Nutzung:

```csharp
RijndaelManaged aes = new RijndaelManaged();
aes.Mode = CipherMode.CBC;
aes.Padding = PaddingMode.PKCS7;
aes.Key = key;
aes.IV = iv;
ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
```

---

## DeviceKey Validierung
Beim CSV Laden wird Base64 strikt geprueft. Ungueltige Eintraege werden ignoriert:

```csharp
private bool IsDeviceKeyValidBase64(string encryptedKey)
{
    try { Convert.FromBase64String(encryptedKey); return true; }
    catch (FormatException) { return false; }
}
```

Das prueft das Format, aber nicht die kryptografische Integritaet (keine MAC/Signatur pro Eintrag).

---

## Log-Export und Verschluesselung (USB)
Die SHC kann Logs auf USB exportieren. Der Export erzeugt eine XML-Datei mit:

- **Info** (Seriennummer, Signing-Cert, Encryption-Cert)
- **Content** (Logdaten, optional AES-verschluesselt)
- **Signature** (RSA-Signatur ueber den Hash des Log-Contents)

Die Kette (aus dem dekompilierten Code):\n

```csharp
GetSigningCertificate();        // lokales Geraetezertifikat
GetEncryptionCertificate();     // PEM Ressource
CreateAes128();                 // AES-128 fuer Log-Content
WriteLogfile();                 // Info + Content
WriteSignature();               // RSA Signatur des Hashes
```

Ressourcen-Pfad (konkreter Code):

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
WriteBinHex(wrapped); // wird in <EncryptionKey> abgelegt
```

Wir haben das eingebettete Zertifikat nach `extracted_resources/SHCLogFileEncryptionCertificate.pem` exportiert (1364 Bytes).

Wenn ein Encryption-Cert vorhanden ist, wird der Log-Content AES-128 verschluesselt und AES Key + IV werden per RSA in der XML verpackt. Die Signatur nutzt das Geraetezertifikat (personal oder default). Das ist konzeptionell sehr aehnlich zur KeyVault-Struktur.\n

---

## Log-Verschluesselung (Standardverhalten)
Der Log-Export versucht immer, das Encryption-Zertifikat aus den eingebetteten Ressourcen zu laden (`SHCLogFileEncryptionCertificate`). Ist es vorhanden, wird der Log-Content AES-128 verschluesselt und signiert. Fehlt es, faellt der Export auf Klartext-Content zurueck und signiert trotzdem den Hash. Da das Zertifikat in den Ressourcen kompiliert ist, ist Verschluesselung im Standard-Build die Regel.\n

Das bedeutet auch: Das Log-Encryption-Zertifikat liegt **nicht** als Datei im NAND, sondern ist in der Assembly eingebettet. Der private Schluessel zum Entschluesseln der Logs liegt nicht im SHC, sondern bei der Stelle, die dieses Zertifikat verwaltet.\n

**Entschluesselbarkeit (mit aktuellem Material)**

Der AES Key+IV werden per RSA mit dem **Public Key** des Log-Encryption-Zertifikats gewrappt. Fuer die Entschluesselung braucht man den **Private Key** von `SHCLogFileEncryptionCertificate`. Dieser Private Key ist im NAND Dump oder den Ressourcen nicht vorhanden. Mit den aktuellen Artefakten lassen sich die Log-Exports nur strukturell pruefen, aber nicht entschluesseln.

---

## Log-Verschluesselung und MasterKey (Klarstellung)
Die Log-Export-Verschluesselung nutzt **nicht** den DeviceMasterKey. Der Log-Content wird per AES-128 verschluesselt, der AES Key + IV werden mit dem **Log-Encryption-Zertifikat** (Public Key) verpackt. Der MasterKey spielt hier keine Rolle; ohne den passenden privaten Schluessel des Log-Encryption-Zertifikats ist eine Entschluesselung der exportierten Logs nicht moeglich.\n

---

## Log-Decrypt-Key Verfuegbarkeit (Dump-View)
Im Dump sehen wir **keinen** privaten Schluessel, der zum Log-Encryption-Zertifikat passt. Das passt zum Design: Das Zertifikat ist eingebettet (Public Key), der private Schluessel wird extern gehalten. So kann die SHC Logs verschluesselt exportieren, ohne sie selbst wieder entschluesseln zu koennen.

---

## Logging-Endpunkte und lokale Speicherung
Der Managed Stack nutzt einen Datei-Logger und stellt Legacy-Logs aus `\\NandFlash\\logStore` beim Start wieder her. Der USB-Log-Export schreibt `shc.log` auf das gemountete USB-Laufwerk und basiert auf den lokalen Logdateien.\n

Serielles Logging kann **ueber USB-Ordner** ein- oder ausgeschaltet werden:
- Ordner `EnableSerialLog` im USB-Root startet `ConsoleLogging` und setzt `FilePersistence.EnableSerialLogging = true`.\n
- Ordner `DisableSerialLog` stoppt das Logging und setzt die Flag zurueck.\n

Die Ordner werden bei USB-Einstecken geprueft und danach geloescht.\n

Code-Auszug (aus `UsbStickLogExport`):

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

Zugehoerige Implementierung (was serielles Logging macht):

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

## settings.config und boot.config (USB-Update + Dump-Korrelation)
Die **vollen** Dateien liegen im Classic USB-Update-Paket unter `shc.zip`:

- `settings.config` (vollstaendig, inkl. `<Signature>`).
- `boot.config` (vollstaendig, Modul-Liste + Log-Level).

Im NAND-Dump taucht `settings.config` als ASCII-XML Fragment auf (`xml_ascii_009.xml`, Offset `76357635`). `boot.config` ist im Dump nur **fragmentiert** vorhanden (`xml_ascii_005.xml`, Offset `59502595`). Das Fragment matcht den Anfang der USB-`boot.config`, daher ist die Datei im Dump vermutlich **zerschnitten** oder mit anderen Daten vermischt.

Was wir in `settings.config` sehen (Auswahl):

- **Backend-Endpunkte** (TLS-URLs fuer Device Management, Configuration, Updates, Key Exchange, Initialization, Messaging, Notifications, Storage).
- **Zertifikats-Enrollment**: `CertificateSubjectName=SMARTHOME01`, `CertificateTemplateName=SHCMultipurposeCertificate`, `CertificateUpnSuffix=shmprod.rwe.local`.
- **Backend-Abschaltzeitpunkt**: `StopBackendRequestsDate=3/1/2024`.
- **Relay-Verbindung**: `RelayServerUrl=wss://gateway.services-smarthome.de/API/1.0/shcconnection/connect`, `UseCertificateAuthentication=true`.
- **WebServiceHost-Creds**: `ClientId=clientId`, `ClientSecret=clientPass` (Placeholder/Default, Klartext).
- **Update-Fenster und Memory-Schwellen** fuer OS- und App-Updates.

Was wir in `boot.config` sehen (volle Datei aus dem USB-Update):

- Modul-Liste + Log-Level; u. a. `Core`, `Logging`, `BackendCommunication`, `SerialCommunication`, `DeviceManager`, `ApplicationsHost`, `RuleEngine`, `SipCosProtocolAdapter`, `wMBusProtocolAdapter`, `LemonbeatProtocolAdapter`, `StartupLogic`.\n
- Log-Levels sind Klartext (`Error`, `Warning`, `Information`, `Debug`) pro Modul.

Signatur / Integritaet (aus dem dekompilierten Code):

- `settings.config` enthaelt eine `<Signature>` in Hex. `ConfigSignature` berechnet SHA1 ueber `<Sections>` und verifiziert gegen **beliebige** Zertifikate im `CodeSign` Store. Die hart kodierte Thumbprint-Konstante wird nicht genutzt.\n
- **Keine Call-Sites** fuer `ConfigSignature` im dekompilierten Managed Code; `ConfigurationManager` laedt `settings.config` ohne Signaturpruefung.\n
- `SettingsFileHelper.ShouldRegisterBackendRequests()` liest `settings.config` direkt und faellt bei Fehlern auf ein hart kodiertes Cutoff-Datum zurueck; eine Signaturpruefung findet dort nicht statt.\n
- `boot.config` wird direkt von `ModuleLoader` geladen; **keine** Signatur- oder Hash-Pruefung im Managed Code.

Verschluesselung:

- Beide Konfigs sind **Klartext-XML**. Die `<Signature>` ist Integritaet, keine Verschluesselung.

Code-Fragmente aus dem Core:

```csharp
// settings.config
XElement x = XElement.Load(directoryName + "\\settings.config");
```

```csharp
// boot.config
XElement x = XElement.Load(directoryName + "\\boot.config");
```

In der Praxis steuert `settings.config` die Betriebsparameter (Update-Fenster, Endpoints, Limits), waehrend `boot.config` das Modul-Startup und Logging-Verhalten bestimmt. Die USB-Update-Dateien wirken wie **generische Defaults**, nicht wie geraetespezifische Geheimnisse.

### Ladepfad und Sicherheit (dekompilierter Ablauf)
Die Managed-Schicht laedt beide Dateien direkt aus dem App-Verzeichnis. Es gibt keine Verschluesselung; Schutz erfolgt nur optional ueber die `settings.config`-Signatur.

**settings.config Ladepfad (ConfigurationManager):**

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

**settings.config Nutzung (SettingsFileHelper):**

```csharp
XElement xElement = XElement.Load(directoryName + "\\settings.config");
XElement xElement2 = item.Descendants()
    .FirstOrDefault(d => d.Attribute("Key").Value == "StopBackendRequestsDate");
string value = xElement2.Attribute("Value").Value;
```

**boot.config Ladepfad (ModuleLoader):**

```csharp
XElement xElement = XElement.Load(directoryName + "\\boot.config");
foreach (XElement item in xElement.Elements().Where(e => e.Name == "module"))
{
    XAttribute name = item.Attribute("name");
    XAttribute assembly = item.Attribute("assembly");
    XAttribute klass = item.Attribute("class");
    XAttribute logLevel = item.Attribute("logLevel");
    // Module laden und Log-Level setzen
}
```

**Optionale Signaturpruefung (nicht verwendet):**

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

**Sicherheits-Implikationen:**

- `settings.config` Integritaet wird **nicht** erzwungen (Signatur-Code existiert, wird aber nicht aufgerufen).
- `boot.config` hat **keine Signatur** und wird direkt geladen.
- Beide Dateien sind **Klartext**; Vertraulichkeit haengt nur von Dateirechten ab.

### Dump-Integritaet (Bedeutung fuer die NAND-Analyse)
Vergleich USB vs. Dump:

- `settings.config` aus dem Dump **entspricht** der USB-Datei (Unterschied nur im BOM/Encoding der Dump-Stringdarstellung). Das spricht fuer eine **vollstaendige** Kopie.
- `boot.config` im Dump ist **abgeschnitten**: die XML endet mitten in einer Zeile und es fehlen die letzten Zeilen (`WebServerHost`, schliessendes `</boot>`). Das wirkt wie **String-Carve/Fragmentierung**, nicht zwingend wie NAND-Korruption.

Fazit: Der Dump wirkt **konsistent**, aber bestimmte Daten sind **nicht zusammenhaengend** gespeichert oder in groessere Ressourcen eingebettet. Fuer solche Inhalte reichen einfache String-Suchen nicht; hier ist tiefere ROM/NK-Extraktion oder Resource-Unpacking erforderlich, um Luecken auszuschliessen.

Zusaetzliche Tiefenanalyse (WSL/Ubuntu):

- WinCE ROM Segmente bei `0x80000` und `0x360000` wurden mit `winceextractor.py` extrahiert. In diesen ROM-Extraktionen fanden sich **keine** `settings.config` / `boot.config` Dateien und auch keine `<Settings>`/`<boot>` XML-Strings.\n
- Das spricht dafuer, dass die Configs **ausserhalb** des ROMs liegen (vermutlich in App/Update-Ressourcen) und im NAND nur als Strings/Fragmente auftauchen.

---

## DeviceKey Export (USB CSV vorhanden)
Korrektur: Der Managed Code **implementiert** den USB-Export/Import der Device Keys als CSV.

Der `DeviceKeyExporter` subscribed auf USB-Events und kopiert `\\NandFlash\\DevicesKeysStorage.csv` nach `Hard Disk\\devices\\DevicesKeysStorage.csv`, sobald ein USB-Stick erkannt wird. Existiert auf dem Stick bereits eine CSV, wird sie **zuerst importiert** (Merge neuer Keys) und danach durch die aktuelle CSV des SHC ersetzt.

Codepfad (aus `DeviceKeyExporter`):

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

Das ist getrennt vom Log-Export (der eine verschluesselte XML erzeugt). Der DeviceKey-CSV Export ist ein einfacher Dateikopiervorgang mit Merge-Schritt.

---

## Software APIs und interne Services (Ueberblick)
Der Managed Stack nutzt Standard-.NET CF APIs und Vendor-Libs:

- **Krypto**: `RijndaelManaged`, `RSACryptoServiceProvider`, `CryptoStream`, Hash-APIs.
- **Zertifikate**: `Org.Mentalis.Security.Certificates` fuer Store-Zugriff und Thumbprint-Suche.
- **Netzwerk**: WCF + Rebex TLS Stack fuer HTTPS.\n
- **Persistenz**: File-basiert auf NAND (`/NandFlash`).\n
- **Native Bridge**: `shc_api.dll` fuer Raw-Flash Updates und Registry-Backup Hooks.\n

---

## Backend/API-Endpunkte und Payloads (v1)
Der Backend-Stack nutzt **WCF SOAP** Clients (generiert via `NetCFSvcUtil`) und ist **kein** REST/JSON API. Jeder Client hat eine Default-`EndpointAddress` auf `https://localhost/Service`, was darauf hindeutet, dass die echten URLs zur Laufzeit aus Konfiguration/Provisionierung stammen. Der einzige hart codierte Host im Code ist der SMS-Service:

- `https://sh70a0100.shmtest.rwe.local/PublicFacingServicesShc/SmsServices/SendSmsService.svc`

In der Praxis referenziert der SHC SOAP Services ueber Action-URIs (Namespaces unter `http://rwe.com/SmartHome/...`). Die folgende Action-Liste stammt direkt aus den dekompilierten Service-Clients.

**Service Action Map (aus den Clients)**

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

**Konfigurierte Endpunkte (aus dem USB Update SHC Classic Config)**

Das Update-Paket enthaelt `USB Update SHC Classic\\shc\\settings.config` mit konkreten Service-URLs fuer den Runtime-Betrieb. Diese Werte ersetzen offenbar die `https://localhost/Service` Defaults der generierten Clients.

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

Weitere backend-relevante Settings in der gleichen Datei:

- `CertificateUpnSuffix` -> `shmprod.rwe.local` (UPN-Suffix fuer Zertifikats-Enrollment)
- `StopBackendRequestsDate` -> `3/1/2024` (Stopp-Datum fuer Backend-Requests)
- `WebServiceHost` hat `ClientId` und `ClientSecret` auf `clientId` / `clientPass` (wahrscheinlich Platzhalter)

Der gleiche Endpoint-Block wurde auch aus dem NAND Dump via `settings.config` Fragment-Carving rekonstruiert, die URLs sind also on-device vorhanden und nicht nur im USB Update Paket.

`USB Update SHC Classic\\shc\\boot.config` enthaelt keine URLs; es definiert nur die Modul-Startreihenfolge und Log-Level.

**Endpoint-Matrix (URL -> Service-Rolle / Action-Familie)**

| URL (settings.config) | Service-Rolle | Action-Familie |
| --- | --- | --- |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/DeviceManagementService.svc` | DeviceManagementService | UploadLogFile, UploadSystemInfo |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ConfigurationService.svc` | ConfigurationService | Get/Set/Delete Managed/Unmanaged config, sync record |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/SoftwareUpdateService.svc` | SoftwareUpdateService | CheckForSoftwareUpdate, ShcSoftwareUpdated |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/KeyExchangeService.svc` | KeyExchangeService | GetDeviceKey, GetMasterKey, GetDevicesKeys, EncryptNetworkKey |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ShcInitializationService.svc` | ShcInitializationService | SubmitCertificateRequest, RetrieveInitializationData, Ownership-Flows |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ShcMessagingService.svc` | ShcMessagingService | Send email notifications, quotas |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/ApplicationManagement/ApplicationTokenService.svc` | ApplicationTokenService | GetApplicationToken, GetApplicationTokenHash |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/DeviceUpdateService.svc` | DeviceUpdateService | CheckForDeviceUpdate |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/Notifications/NotificationService.svc` | NotificationService | SendNotifications, SendSystemNotifications |
| `https://storage.services-smarthome.de/PublicStorageServices/DataTrackingService.svc` | DataTrackingService | StoreData, StoreListData |
| `https://shcc.services-smarthome.de/PublicFacingServicesSHC/SmsServices/SendSmsService.svc` | SmsService | SendSms, SendSystemSms, GetSmsRemainingQuota |
| `wss://gateway.services-smarthome.de/API/1.0/shcconnection/connect` | RelayServer | WSS Relay Connection Bootstrap |

**Erreichbarkeit/Deaktivierung (nur aus Config ableitbar)**

- `StopBackendRequestsDate` ist auf `3/1/2024` gesetzt, d.h. die Software beendet Backend-Requests nach diesem Datum, selbst wenn URLs konfiguriert sind.
- Es gibt keine per-Endpoint Disable-Flags in `settings.config`; der einzige explizite globale Cutoff ist `StopBackendRequestsDate`.
- Damit ist die Reachability nach dem Cutoff "unbekannt/ueberwiegend deaktiviert"; die Config selbst beweist keine Live-Verfuegbarkeit.

**Security-Hinweis (DNS Spoofing und TLS)**

Dieser Bericht bewertet oder empfiehlt keine Interception-Techniken. Im managed Code sehen wir keinen expliziten "accept all certificates" oder TLS-Bypass. Der Stack setzt auf WCF + TLS-Library und den Zertifikatsspeicher des Geraets, daher ist Standard-Zertifikatsvalidierung zu erwarten, sofern sie nicht in nativen Komponenten umgangen wird.

**Config-Tampering Hinweis**

`settings.config` enthaelt ein `<Signature>` Element. Wenn die Anwendung diese Signatur beim Laden prueft, werden Aenderungen an `StopBackendRequestsDate` oder Service-URLs vermutlich verworfen. Im dekompilierten Managed Layer ist der konkrete Pruefpfad nicht sichtbar; die Enforcement-Stelle bleibt aus dieser Schicht heraus unklar.

---

## DNS-Status der Backend-Domains
Wir haben die oeffentliche DNS-Aufloesung fuer die in `settings.config` referenzierten Backend-Hostnamen erneut geprueft:

- `services-smarthome.de` und `gateway.services-smarthome.de` liefern **SERVFAIL** von oeffentlichen Resolvern.
  - `nslookup services-smarthome.de 1.1.1.1` -> "Server failed"
  - `nslookup services-smarthome.de 8.8.8.8` -> "Server failed"
  - Gleiches Ergebnis fuer `gateway.services-smarthome.de`
- Mit SERVFAIL lassen sich keine A/AAAA/MX/TXT/SRV Records aus oeffentlichen Resolvern ziehen.
- NS/SOA/MX Queries schlagen ebenfalls mit SERVFAIL fehl, d. h. die aktuelle autoritative Kette ist von diesem Host nicht verifizierbar.

Interpretation:

- Die Domain ist registriert und erscheint delegiert (laut frueheren WHOIS-Notizen), aber die oeffentliche DNS-Aufloesung funktioniert aktuell nicht.
- Moegliche Ursachen sind DNSSEC/Zone-Fehler oder bewusst eingeschraenkte Antworten (Authoritatives verweigern oeffentliche Queries).
- Ohne autoritative Antworten ist die aktuelle Erreichbarkeit der Services per DNS nicht belegbar.

WHOIS/RDAP Snapshot (DENIC RDAP):

- Status: `active`
- Last changed: `2020-12-14T10:20:11+01:00`

Gemeldete DNS-Konfiguration (aus frueheren Checks, aktuell nicht verifizierbar):

- Als DNS wurden zuvor Azure Nameserver genannt:
  - `ns1-02.azure-dns.com`
  - `ns2-02.azure-dns.net`
  - `ns3-02.azure-dns.org`
  - `ns4-02.azure-dns.info`
- Wegen der aktuellen SERVFAILs konnten diese NS-Werte auf dieser Maschine nicht erneut bestaetigt werden.

**Payload-Strukturen (Beispiele aus den Request/Response Klassen)**

Key Exchange (SGTIN zu DeviceKey):

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

Zertifikats-Enrollment:

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

Initialization Polling (Cloud -> SHC):

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
    public ShcUser[] Users; // enthaelt PasswordHash in jedem ShcUser
}
```

Software-Update Metadaten (Cloud -> SHC):

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

Log Upload (SHC -> Cloud):

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

Device Update Lookup (SHC -> Cloud):

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

**WebSocket Kanal**

`WebSocketSecureClient` startet mit einem HTTPS Handshake, liest den `Location:` Header und konvertiert ihn nach `wss://`. Das deutet auf ein Redirect-basiertes WSS-Bootstrap hin, ohne hart codierte WSS-URL im Binary.

---

## shc_api.dll (native Flash/Registry Bridge)
Wir konnten eine **vollstaendige native WinCE ARM** `shc_api.dll` aus dem Dump extrahieren. Die beiden gefundenen Kopien sind byte-identisch (SHA-256 gleich), was 
fuer konsistente ROM-Extraktion spricht.

Static-Analyse Highlights:

- **Native ARM PE** (kein .NET), ImageBase `0x40840000`, Entrypoint `0x40848204`.
- **Ordinal-Exports vorhanden** (manuell aus der Export-Tabelle gelesen). Liste (1-16):
  - `Cert_ImportCertificate`
  - `Cert_ImportCertificateWithPrivateKey`
  - `DhcpRenew`
  - `EraseRawPartition`
  - `GetHWVersion`
  - `GetSGTIN`
  - `IsFactoryReset`
  - `RasConnect`
  - `RasDisconnect`
  - `RasIsConnected`
  - `Reset`
  - `StartSNTPService`
  - `StopSNTPService`
  - `WriteRawPartition`
  - `BackupRegistry`
  - `RestoreRegistry`
- **Strings deuten auf Flash + Registry Operationen**:
  - `WriteRawPartition`, `EraseRawPartition`
  - `BackupRegistry`, `RestoreRegistry`
  - `Cannot read/write bootloader settings`
  - `Cannot access NAND Flash driver`
  - `\NandFlash\CertBackupLocal`, `\NandFlash\CertBackupUser`
  - `readEK` (wahrscheinlich TPM EK read)

Disassembly-Auszug (Entrypoint Stub):

```asm
0x40848204: str lr, [sp, #-4]!
0x40848208: cmp r1, #1
0x4084820c: bleq #0x408483d4
0x40848210: mov r0, #1
0x40848214: pop {lr}
0x40848218: bx lr
```

Das passt zur Managed Update-Logik, die `WriteRawPartition` via native Bridge nutzt. Fuer tieferes Reverse-Engineering braucht es einen kompletten Disassembly-Workflow 
(Ghidra/IDA), um Parameter und interne Checks sauber zu mappen.

Managed-Nutzung (aus dem dekompilierten Code):

- **Device/Identity**: `GetSGTIN`, `GetHWVersion` fuer Seriennummer/HW-Revision.
- **Netzwerk**: `DhcpRenew`, `RasConnect`, `RasDisconnect`, `RasIsConnected`.
- **Zeit**: `StartSNTPService`, `StopSNTPService`.
- **Update/Recovery**: `WriteRawPartition`, `EraseRawPartition`, `BackupRegistry`, `RestoreRegistry`, `Reset`, `IsFactoryReset`.

---


## Interne API-Landkarte (High-Level)
Aus den Namespaces und Services ergibt sich folgende Aufteilung:\n

- **Backend-Kommunikation**: WCF Clients fuer Software-Updates, Device-Updates und Key-Exchange.\n
- **Lokale Kommunikation**: lokale Control-Plane Services und Protokoll-Adapter.\n
- **Krypto/Zertifikate**: Certificate Store, KeyVault Handling, Log Export Signierung/Verschluesselung.\n
- **Update/Firmware**: Managed Update Flow, nativer Raw-Write via `shc_api.dll`.\n
- **Logging/Persistenz**: File-Logging, USB-Log-Export, Persistenz vor Updates.\n

Das hilft, neue Funde funktional einzuordnen.\n

---

## Interne Klassen (ausgewaehlte Kernbausteine)
Diese Klassen definieren wesentliche Sicherheits- und Updatepfade:

- `DeviceMasterKeyRepository` - KeyVault Lesen/Schreiben und MasterKey Unwrap\n
- `DeviceKeyRepository` - CSV Encode/Decode und AES-ECB\n
- `CertificateManager` - Cert Store Auswahl, Thumbprints\n
- `SoftwareUpdateProcessor` - Update Orchestrierung\n
- `FirmwareImage` - NK Header Pruefung\n
- `LogExporter` / `UsbStickLogExport` - Log Export, Encrypt, Sign\n
- `StartupLogic` - Initialisierung, Registration, Update Trigger\n

---

## Software-Resources (eingebettete Inhalte)
Die Assemblies enthalten wichtige Ressourcen:\n

- CA/Root-Bundles (P7B) fuer Trust Stores.\n
- `SHCLogFileEncryptionCertificate` (PEM) fuer Log-Verschluesselung.\n
- Defaults und Konfigurationswerte.\n

Darum tauchen einige Zertifikate nicht als separate Dateien im NAND auf.\n

Ressourcen-Dateien (resx) Ueberblick:

- **Core Trust Store Bundle**: `RWE.SmartHome.SHC.Core.Properties.Resources.resx` enthaelt 10 Binary-Blobs (`System.Byte[]`) mit CA/Root-Material, u. a. `DigiCertGlobalRootCA`, `DigiCertGlobalRootG2`, `MicrosoftRSARootCA2017`, `MicrosoftEVECCRootCA2017`, `DTRUSTRootClass3CA22009`, `SHCRootCA_VerisignNew`, `SHCSubCA_VerisignNew`, plus `shc_ca_certs` und `shc_root_certs`.
- **Log-Encryption Zertifikat**: `RWE.SmartHome.SHC.BusinessLogic.Properties.Resources.resx` enthaelt ein einzelnes Binary-Blob `SHCLogFileEncryptionCertificate` (wird im `LogExporter` genutzt).
- **Serial Resource**: `RWE.SmartHome.SHC.BusinessLogic.Resources.Resources.resx` enthaelt ein Binary-Blob namens `Serial` (wird im Business-Logic Layer genutzt).
- **Logging Pattern**: `RWE.SmartHome.SHC.BusinessLogic.Properties.Resource.resx` enthaelt `LogEntryPattern` (String-Pattern fuer Logeintraege).
- **DataAccess Schema Strings**: `RWE.SmartHome.SHC.DataAccess.Properties.Resources.resx` enthaelt 41 String-Keys, z. B. `LogicalDevicesXml`, `DeviceActivityLogs`, `ApplicationsSettings`.
- **Auth- und Error-Strings**: mehrere `*.ErrorStrings.resx` und `ErrorResources.resx` enthalten Fehlermeldungen und Labels.
- **Abhaengigkeiten**: die Microsoft.Practices Mobile ContainerModel Ressourcen enthalten wenige String-Fehlertexte.

In Summe sind die Ressourcen ueberwiegend Strings plus einige Binary-Blobs fuer Trust und Log-Encryption. Das erklaert, warum bestimmte Zertifikate in den Binaries auftauchen, aber nicht als eigene Dateien im NAND.

**Coprocessor Firmware Blob (Serial)**

Die Ressource `Serial` (32.768 Bytes) ist das AVR-Coprocessor-Firmware-Image. Der Updater berechnet eine CRC ueber das Blob und flasht es anschliessend.

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

Wir haben das Blob nach `extracted_resources/Serial.bin` exportiert.

**Serial.bin Detailanalyse (AVR Image)**

Binary Eigenschaften:

- Groesse: 32.768 Bytes (0x8000), typisch fuer AVR Flash Images.
- Entropie: ~5.45 bits/byte (nicht komprimiert/verschluesselt).
- Padding: grosser 0xFF Bereich ab 0x51A6 (10.842 Bytes) und 0x00 Tail ab 0x7C00 (1.024 Bytes).
- Codebereich: nicht-0xFF/0x00 Daten von ca. 0x0100 bis ca. 0x506C.

Vector Table (4-Byte Slots, 26 Eintraege bei 0x0000-0x0067):

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

Disassembly Beispiele (avr-objdump, -b binary -m avr):

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

Wahrscheinliche MCU Familie:

- 0x8000 Imagegroesse und 26 Vectoren passen gut zu einem AVR ATmega328-Cluster (avr5 Familie). Das ist eine best-fit Annahme basierend auf Vector Count und Flash Groesse; die exakte MCU ist im Dump nicht sicher belegt.
- I/O Register Usage passt zur ATmega328p UART0 Map (I/O 0x20-0x22 fuer UCSR0A/B/C, 0x24 fuer UBRR0L, 0x26 fuer UDR0) sowie Stackpointer Setup via I/O 0x3D/0x3E (SPL/SPH).
- Feld-Hinweis: das Board nutzt einen **16 MHz External Crystal**, was zu Low Fuse `0xFF` passt (Full-Swing Oscillator, CKDIV8 deaktiviert).

```asm
4e04: out 0x21, r20    ; UCSR0B
4e06: out 0x22, r21    ; UCSR0C
4e28: out 0x20, r16    ; UCSR0A
a90:  out 0x24, r17    ; UBRR0L
a9a:  out 0x26, r16    ; UDR0
```

Bootloader Hinweise:

- Kein `spm` Instruction im Binary gefunden; das spricht fuer ein App-Image ohne Self-Programming Bootloader.
- Die SHC v2 Datei `serial_v0.e.bin` ist byte-identisch zu `Serial.bin` (SHA-256 gleich).

**I/O Nutzung und Pin-Hinweise (AVR I/O Space)**

Beobachtete Low-I/O Adressen (aus der Disassembly):

- 0x03/0x04/0x05 (PINB/DDRB/PORTB) mit Bit-Operationen auf PORTB:
  - `sbi 0x05,2`, `cbi 0x05,2` (PB2)
  - `cbi 0x05,3` (PB3)
  - `sbi 0x05,5` (PB5)
  - `sbic 0x03,4` (PB4 Input)
- 0x07/0x08 (DDRC/PORTC) wird genutzt, PORTC Lines werden konfiguriert.
- 0x09/0x0B (PIND/PORTD) wird genutzt, PORTD ist aktiv.
- 0x20-0x27 genutzt fuer UART0 Register (UCSR0A/B/C, UBRR0L, UDR0).
- 0x3D/0x3E und 0x3F genutzt (SPL/SPH, SREG).

Wenn die MCU eine ATmega328p ist, entsprechen PB2/PB3/PB4/PB5 der SPI Pin-Gruppe (SS/MOSI/MISO/SCK). Das Pattern aus PB2/PB3/PB5 toggles und PB4 read passt zu SPI-Signalen, die exakte Verdrahtung ist jedoch board-abhaengig.

**Coprocessor Flash-Protokoll (aus dekompiliertem Code)**

Der SHC nutzt **kein** UART Bootloader-Update. Er programmiert den AVR Coprocessor per **SPI ISP** mit klassischen AVR-ISP Commands. Die Firmware wird als raw Binary gestreamt und in 64-Word (128-Byte) Pages geschrieben.

Kernschritte und Commands (aus `AVRFirmwareManager`):

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

Flash-Loop (raw Binary, 2 Bytes pro Wort):

```csharp
short addr = 0;
short pageWords = 64; // 128 bytes

// Load program memory (low/high byte)
// low:  0x40 0x00 addr data
// high: 0x48 0x00 addr data
SendDataByte(data, (byte)(addr & 0x3F), bDataHigh: false);
SendDataByte(data, (byte)(addr & 0x3F), bDataHigh: true);
addr++;

// Wenn addr % 64 == 0, Page write:
// 0x4C high_addr low_addr 0x00
WritePage((short)(addr - pageWords));
```

Optionale Fuse/Lock Programmierung (im Code vorhanden, im Update-Flow nicht genutzt):

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

Fazit: der Coprocessor wird per **SPI ISP** programmiert, kein Serial Bootloader. Das `Serial.bin` ist ein raw AVR Application Image und wird direkt geflasht.

---

## DnsService.exe (native Bonjour/mDNS Komponente)
Im USB-Updatepaket liegt ein natives WinCE ARM Binary `DnsService.exe` (`USB Update SHC Classic\\shc\\DnsService.exe`). Static-Analyse und Strings deuten klar auf Apple/Bonjour mDNSResponder (DNS‑SD + mDNS).

Evidenz und Metadaten:

- **Native WinCE ARM**: Machine `0x1c2`, Subsystem `9` (Windows CE GUI), **kein .NET**.
- **Netzwerk-Imports**: `WS2.dll` (Sockets, WSA*), `iphlpapi.dll` (Adapter/IP Tabellen), `COREDLL.dll`.
- **Bonjour/mDNS Strings**: `_dns-sd`, `_mdns`, `mDNS_RegisterService`, `DNSServiceBrowse`, `DNSServiceResolve`, dazu Bonjour-Registry-Pfade `SOFTWARE\\Apple Computer, Inc.\\Bonjour\\DynDNS\\...`.
- **PDB Pfad**: `r:\\SmartHome\\Release\\V1.2\\Main\\Source\\RWE.SmartHome.SHC\\App\\Native\\Bonjour\\bin\\Release\\DnsService.pdb`.

Disassembly-Auszug (ARM Entry-Wrapper):

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

Trigger/Startverhalten:

- Im Managed Code gibt es **keine** direkte Referenz auf `DnsService.exe`.\n
- Unter WinCE werden solche Services typischerweise ueber **Registry Services** (`HKLM\\Services\\...`) oder einen nativen Boot-Launcher gestartet. In den extrahierten Hives fanden wir keinen expliziten `DnsService`‑Eintrag, daher liegt der Trigger vermutlich im NK/ROM Registry oder in einem anderen nativen Modul.

Artefakte der Analyse:

- `full_raw_2112_artifacts\\dnsservice_metadata.txt`
- `full_raw_2112_artifacts\\dnsservice_imports.txt`
- `full_raw_2112_artifacts\\dnsservice_strings.txt`
- `full_raw_2112_artifacts\\dnsservice_disasm_blocks.txt`

---

## Warum ECB verwendet wurde (und was das bedeutet)
Der dekompilierte Code nutzt AES-256 im ECB Modus fuer die Device Keys. ECB ist einfach, schnell und benoetigt kein IV pro Eintrag, was die Speicherung vereinfacht. Der Nachteil ist die klassische ECB Schwachstelle: gleiche Klartext-Blocks erzeugen gleiche Ciphertext-Blocks, wodurch Strukturmuster sichtbar werden (aber nicht der Key selbst).

In diesem Datensatz faellt vor allem der identische letzte Block durch PKCS7 Padding auf. Das ist ein Fingerprint des Modus, liefert aber keinen direkten Klartext.

---

## Update Pipeline (software view)

1) Backend Update Check mit Client Zertifikat Auth.
2) Download nach `/NandFlash/temp.bin` mit Backend Credentials.
3) Umbenennen in `/NandFlash/update.bin` oder `/NandFlash/app_update.bin`.
4) Firmware Update ruft `WriteRawPartition` via `shc_api.dll` nach einfacher NK Header Pruefung.
5) App Update renamte zu `/NandFlash/shc.zip` und rebootet.

---

## Standard-Updateverhalten (Community-Dokumentation, Classic SHC v1)
Die Community beschreibt zwei Hauptwege fuer die Classic SHC v1:

1) Online-Update ueber den Cloud-Dienst. Die SHC prueft periodisch (ungefaehr taeglich) und auch nach einem Neustart. Updates werden in der App angezeigt, wenn sie verfuegbar sind. Wenn das System lange nicht aktualisiert wurde, koennen mehrere Updates nacheinander notwendig sein.

2) USB-Update als Fallback, wenn Online-Updates scheitern. Bei der Classic SHC v1 sind OS-Image und App-Paket getrennt:
   - **OS-Update**: Rohes WinCE Image (typisch `nk_signed.bin`) im Root des USB-Sticks.
   - **App-Update**: ZIP Paket (`shc.zip`) im Root des USB-Sticks.

Der Ablauf erfolgt beim Boot: Geraet ausschalten, FAT32-USB-Stick einstecken, einschalten, auf die Anzeige zum Entfernen des Sticks warten und danach das Update abschliessen lassen.

Das passt zur Managed-Logik: Firmware-Updates verarbeiten ein Roh-Image (`update.bin`), App-Updates eine ZIP (`shc.zip`). Der .NET Layer macht dabei nur grundlegende Strukturchecks, keine kryptografische Signaturpruefung. Strengere Checks muessen im nativen Code oder serverseitig liegen.

---

## Firmware-Validierung (Managed Code)
Der Managed Layer validiert das OS-Image nur strukturell. Geprueft werden NK-Header (`B00FF\\n`), Record-Laengen und die Gesamtkonsistenz. Eine kryptografische Signaturpruefung gibt es im .NET Layer nicht.

Code-Auszug (vereinfacht aus `FirmwareImage.CheckFirmwareImage`):

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

Nach der Pruefung wird direkt per nativer Funktion geschrieben:

```csharp
if (WinCEFirmwareManager.WriteRawPartition("/NandFlash/update.bin"))
{
    // Update-State setzen, Datei loeschen, reboot
}
```

Wenn es strengere Checks gibt, liegen sie im nativen Code (`shc_api.dll`) oder serverseitig.\n

---

## Application-Update (USB ZIP, Managed Code)
App-Updates laufen als ZIP. Der Managed Layer verschiebt nach `/NandFlash/shc.zip` und rebootet. Eine Signatur- oder Hashpruefung gibt es im Managed Layer nicht.

Code-Auszug (vereinfacht aus `SoftwareUpdateProcessor.UpdateApplication`):

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

Das passt zum USB-Weg: ZIP liegt im Root, wird ins NAND kopiert, Reboot triggert Apply.\n

---

## Lokaler Betrieb ohne Cloud (technische Zusammenfassung)
Lokaler Betrieb ist moeglich, da die SHC einen vollwertigen lokalen Stack hat. Nach dem Cloud-Ende ist die Praxis:\n
- Lokaler Zugriff auf UI und lokale Services.\n
- Community-Integrationen/Binds fuer lokale Kommunikation.\n
- USB-Updates fuer OS/App bei Bedarf.\n

Das aendert nichts am KeyVault-Modell: Private Keys und Master Keys bleiben TPM-geschuetzt.\n

---

## Native Validierung (unbekannter Umfang)
Der Managed Layer ruft `shc_api.dll` fuer Raw-Partition-Writes (`WriteRawPartition`, `EraseRawPartition`) und Registry-Backup/Restore auf. Diese Funktionen sind als Ordinal-Exports vorhanden, was bedeutet, dass der Managed Layer auf die native Implementierung fuer tiefere Checks vertraut.

Aus der aktuellen nativen Analyse:
- Exports sind sichtbar, ebenso Strings fuer NAND/Bootloader-Zugriff und Cert-Backup-Pfade.
- **Keine** klaren Signatur-/Hash-Check-Strings fuer NK-Validierung im DLL-Stringmaterial gefunden.

Das heisst: Signaturpruefung ist weiterhin **moeglich**, aber bisher nicht belegt. Falls sie existiert, liegt sie eher:
- Im Bootloader (ausserhalb `shc_api.dll`), oder
- In einem anderen nativen Modul, das beim Apply-Schritt aufgerufen wird.

Fazit: Managed Code macht nur Struktur-Checks; native Enforcement ist unbestimmt und braucht tieferes Disassembly.

---

## Software-Security Beobachtungen (Managed Layer)

- Firmware-Check im .NET Layer prueft nur NK Header und Laengen.
- Downloadpfad akzeptiert Base64- und Binary-Streams und vertraut auf Backend-Auth.
- TLS Settings setzen auf kompatible Ciphers und SHA1.

Strengere Checks koennen im nativen Code oder serverseitig liegen.

---

## Wichtige Speicherpfade (beobachtet)
- `\\NandFlash\\DevicesKeysStorage.csv` - verschluesselte Device Keys
- `\\NandFlash\\local.xml` - KeyVault (MasterKey Wrapper)
- `\\NandFlash\\logStore` - Legacy Logs
- `\\NandFlash\\update.bin` - Firmware Update Image
- `\\NandFlash\\app_update.bin` - App Update Staging
- `\\NandFlash\\shc.zip` - App Update Paket (final)

---

## Startup Ablauf (High-Level)
Die Startup-Sequenz (aus `StartupLogic`) enthaelt:

- Zeit-Synchronisation und Netzwerk-Checks.
- Initial Registration und Zertifikats-Discovery.
- Pflicht-Update-Check (vor der vollen Initialisierung).
- MasterKey Retrieval und Persistenz (KeyVault Creation beim ersten Lauf).
- Wiederherstellung von Persistenz und Logs.
