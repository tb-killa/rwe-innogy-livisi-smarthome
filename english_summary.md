# SHC v1 NAND analysis - structured technical notes (English)


## Table of contents
- [Product context (what it is, who made it, and current state)](#product-context-what-it-is-who-made-it-and-current-state)
- [Hardware](#hardware)
- [Software stack](#software-stack)
- [RF protocols and device communication (from decompiled code)](#rf-protocols-and-device-communication-from-decompiled-code)
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

---

## Certificate carving results (dump scan)
We scanned the NAND dump directly for PEM blocks and DER-encoded X.509 sequences:

- **PEM**: one certificate block found and parsed as `SHCLogFileEncryptionCertificate` (issuer `SHMPROD-CA-S`). No PEM private keys were found.
- **DER**: 200 candidate DER sequences were carved; OpenSSL successfully parsed many of them as X.509. They include standard Windows/VeriSign roots and multiple SHM/RWE certificates (`RWE_SmartHome_SHC_Codesigning`, `SHMPROD-CA-S`, `SHMPROD-CA-E`, `SMARTHOME01`), often repeated due to resource embedding. After de-duplication by subject/issuer/serial/dates, **37 unique certificates** remain (168 parsed entries total).
- **PFX/PKCS12**: no valid PKCS12 containers detected.
- **Private keys**: a single DER-encoded RSA private key was detected (1024-bit). It does **not** match any carved certificate modulus and is not a PKCS12 bundle. Likely a standalone/test key or unrelated artifact. Offset in dump: `0xC544BCB`.
- **PKCS#8 (encrypted)**: four PKCS#8 encrypted key blobs were carved. All report OID `1.2.840.113549.1.12.1.3` (pbeWithSHAAnd3-KeyTripleDES-CBC). Without the passphrase, contents remain opaque; they may be false positives or unrelated encrypted blobs.
- **Cert backup paths**: no `CertBackupLocal` / `CertBackupUser` strings or files found in the raw dump; only the paths appear as strings inside `shc_api.dll`.
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

If the encryption certificate is present, AES-128 is used to encrypt log content and the AES key+IV are RSA-encrypted into the XML. The signing cert is the device cert (personal or default) and is used to sign the hashed payload. This is conceptually similar to the KeyVault pattern (AES content + RSA-wrapped AES key).

---

## Log encryption defaults (what the code implies)
The log export path always tries to load the encryption certificate from embedded resources (`SHCLogFileEncryptionCertificate`). If it is present, the log content is encrypted with AES-128 and a signature is attached. If it is missing, the export falls back to plaintext content and still signs the hash. Because the encryption cert is compiled into the resources, encryption is effectively the default behavior on standard builds.\n

This also means the log encryption certificate is **not expected to be in NAND as a separate file**; it is embedded in the application resources. The private key needed to decrypt exported logs would be held by whoever owns the corresponding encryption certificate private key, not by the SHC itself.\n

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
