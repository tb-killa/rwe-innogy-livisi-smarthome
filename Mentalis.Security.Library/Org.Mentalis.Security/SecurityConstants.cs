namespace Org.Mentalis.Security;

internal sealed class SecurityConstants
{
	public const int ALG_CLASS_DATA_ENCRYPT = 24576;

	public const int ALG_CLASS_HASH = 32768;

	public const int ALG_CLASS_KEY_EXCHANGE = 40960;

	public const int ALG_SID_3DES = 3;

	public const int ALG_SID_AES = 17;

	public const int ALG_SID_AES_128 = 14;

	public const int ALG_SID_AES_192 = 15;

	public const int ALG_SID_AES_256 = 16;

	public const int ALG_SID_CYLINK_MEK = 12;

	public const int ALG_SID_DES = 1;

	public const int ALG_SID_DH_EPHEM = 2;

	public const int ALG_SID_DH_SANDF = 1;

	public const int ALG_SID_MD2 = 1;

	public const int ALG_SID_MD4 = 2;

	public const int ALG_SID_MD5 = 3;

	public const int ALG_SID_RC2 = 2;

	public const int ALG_SID_RC4 = 1;

	public const int ALG_SID_RSA_ANY = 0;

	public const int ALG_SID_SHA1 = 4;

	public const int ALG_SID_SSL3SHAMD5 = 8;

	public const int ALG_TYPE_ANY = 0;

	public const int ALG_TYPE_BLOCK = 1536;

	public const int ALG_TYPE_DH = 2560;

	public const int ALG_TYPE_RSA = 1024;

	public const int ALG_TYPE_STREAM = 2048;

	public const int ASC_REQ_ALLOCATE_MEMORY = 256;

	public const int ASC_REQ_CONFIDENTIALITY = 16;

	public const int ASC_REQ_EXTENDED_ERROR = 32768;

	public const int ASC_REQ_REPLAY_DETECT = 4;

	public const int ASC_REQ_SEQUENCE_DETECT = 8;

	public const int ASC_REQ_STREAM = 65536;

	public const int AT_KEYEXCHANGE = 1;

	public const int AUTHTYPE_CLIENT = 1;

	public const int AUTHTYPE_SERVER = 2;

	public const int CALG_3DES = 26115;

	public const int CALG_AES = 26129;

	public const int CALG_AES_128 = 26126;

	public const int CALG_AES_192 = 26127;

	public const int CALG_AES_256 = 26128;

	public const int CALG_CYLINK_MEK = 26124;

	public const int CALG_DES = 26113;

	public const int CALG_DH_EPHEM = 43522;

	public const int CALG_DH_SF = 43521;

	public const int CALG_MD2 = 32769;

	public const int CALG_MD4 = 32770;

	public const int CALG_MD5 = 32771;

	public const int CALG_RC2 = 26114;

	public const int CALG_RC4 = 26625;

	public const int CALG_RSA_KEYX = 41984;

	public const int CALG_SHA1 = 32772;

	public const int CALG_SSL3_SHAMD5 = 32776;

	public const int CERT_CHAIN_CACHE_END_CERT = 1;

	public const int CERT_CHAIN_CACHE_ONLY_URL_RETRIEVAL = 4;

	public const int CERT_CHAIN_DISABLE_AUTH_ROOT_AUTO_UPDATE = 256;

	public const int CERT_CHAIN_DISABLE_PASS1_QUALITY_FILTERING = 64;

	public const int CERT_CHAIN_FIND_BY_ISSUER = 1;

	public const int CERT_CHAIN_POLICY_ALLOW_TESTROOT_FLAG = 32768;

	public const int CERT_CHAIN_POLICY_ALLOW_UNKNOWN_CA_FLAG = 16;

	public const int CERT_CHAIN_POLICY_IGNORE_ALL_NOT_TIME_VALID_FLAGS = 7;

	public const int CERT_CHAIN_POLICY_IGNORE_ALL_REV_UNKNOWN_FLAGS = 3840;

	public const int CERT_CHAIN_POLICY_IGNORE_CA_REV_UNKNOWN_FLAG = 1024;

	public const int CERT_CHAIN_POLICY_IGNORE_CTL_NOT_TIME_VALID_FLAG = 2;

	public const int CERT_CHAIN_POLICY_IGNORE_CTL_SIGNER_REV_UNKNOWN_FLAG = 512;

	public const int CERT_CHAIN_POLICY_IGNORE_END_REV_UNKNOWN_FLAG = 256;

	public const int CERT_CHAIN_POLICY_IGNORE_INVALID_BASIC_CONSTRAINTS_FLAG = 8;

	public const int CERT_CHAIN_POLICY_IGNORE_INVALID_NAME_FLAG = 64;

	public const int CERT_CHAIN_POLICY_IGNORE_INVALID_POLICY_FLAG = 128;

	public const int CERT_CHAIN_POLICY_IGNORE_NOT_TIME_NESTED_FLAG = 4;

	public const int CERT_CHAIN_POLICY_IGNORE_NOT_TIME_VALID_FLAG = 1;

	public const int CERT_CHAIN_POLICY_IGNORE_ROOT_REV_UNKNOWN_FLAG = 2048;

	public const int CERT_CHAIN_POLICY_IGNORE_WRONG_USAGE_FLAG = 32;

	public const int CERT_CHAIN_POLICY_SSL = 4;

	public const int CERT_CHAIN_POLICY_TRUST_TESTROOT_FLAG = 16384;

	public const int CERT_CHAIN_RETURN_LOWER_QUALITY_CONTEXTS = 128;

	public const int CERT_CHAIN_REVOCATION_CHECK_CACHE_ONLY = int.MinValue;

	public const int CERT_CHAIN_REVOCATION_CHECK_CHAIN = 536870912;

	public const int CERT_CHAIN_REVOCATION_CHECK_CHAIN_EXCLUDE_ROOT = 1073741824;

	public const int CERT_CHAIN_REVOCATION_CHECK_END_CERT = 268435456;

	public const int CERT_COMPARE_ANY = 0;

	public const int CERT_COMPARE_ENHKEY_USAGE = 10;

	public const int CERT_COMPARE_KEY_IDENTIFIER = 15;

	public const int CERT_COMPARE_MD5_HASH = 4;

	public const int CERT_COMPARE_NAME = 2;

	public const int CERT_COMPARE_NAME_STR_A = 7;

	public const int CERT_COMPARE_NAME_STR_W = 8;

	public const int CERT_COMPARE_SHA1_HASH = 1;

	public const int CERT_COMPARE_SHIFT = 16;

	public const int CERT_DATA_ENCIPHERMENT_KEY_USAGE = 16;

	public const int CERT_DIGITAL_SIGNATURE_KEY_USAGE = 128;

	public const int CERT_E_CHAINING = -2146762486;

	public const int CERT_E_CN_NO_MATCH = -2146762481;

	public const int CERT_E_EXPIRED = -2146762495;

	public const int CERT_E_PURPOSE = -2146762490;

	public const int CERT_E_REVOCATION_FAILURE = -2146762482;

	public const int CERT_E_REVOKED = -2146762484;

	public const int CERT_E_ROLE = -2146762493;

	public const int CERT_E_UNTRUSTEDROOT = -2146762487;

	public const int CERT_E_UNTRUSTEDTESTROOT = -2146762483;

	public const int CERT_E_VALIDITYPERIODNESTING = -2146762494;

	public const int CERT_E_WRONG_USAGE = -2146762480;

	public const int CERT_FIND_ANY = 0;

	public const int CERT_FIND_CTL_USAGE = 655360;

	public const int CERT_FIND_ENHKEY_USAGE = 655360;

	public const int CERT_FIND_HASH = 65536;

	public const int CERT_FIND_KEY_IDENTIFIER = 983040;

	public const int CERT_FIND_MD5_HASH = 262144;

	public const int CERT_FIND_SHA1_HASH = 65536;

	public const int CERT_FIND_SUBJECT_NAME = 131079;

	public const int CERT_FIND_SUBJECT_STR_A = 458759;

	public const int CERT_FIND_SUBJECT_STR_W = 524295;

	public const int CERT_FRIENDLY_NAME_PROP_ID = 11;

	public const int CERT_HASH_PROP_ID = 3;

	public const int CERT_INFO_SUBJECT_FLAG = 7;

	public const int CERT_KEY_AGREEMENT_KEY_USAGE = 8;

	public const int CERT_KEY_CERT_SIGN_KEY_USAGE = 4;

	public const int CERT_KEY_ENCIPHERMENT_KEY_USAGE = 32;

	public const int CERT_KEY_IDENTIFIER_PROP_ID = 20;

	public const int CERT_KEY_PROV_HANDLE_PROP_ID = 1;

	public const int CERT_KEY_PROV_INFO_PROP_ID = 2;

	public const int CERT_MD5_HASH_PROP_ID = 4;

	public const int CERT_NAME_DISABLE_IE4_UTF8_FLAG = 65536;

	public const int CERT_NAME_FRIENDLY_DISPLAY_TYPE = 5;

	public const int CERT_NAME_ISSUER_FLAG = 1;

	public const int CERT_NAME_SIMPLE_DISPLAY_TYPE = 4;

	public const int CERT_NON_REPUDIATION_KEY_USAGE = 64;

	public const int CERT_OFFLINE_CRL_SIGN_KEY_USAGE = 2;

	public const int CERT_PVK_FILE_PROP_ID = 12;

	public const int CERT_RDN_ENCODED_BLOB = 1;

	public const int CERT_RDN_UNICODE_STRING = 12;

	public const int CERT_SHA1_HASH_PROP_ID = 3;

	public const int CERT_STORE_ADD_NEW = 1;

	public const int CERT_STORE_PROV_COLLECTION = 11;

	public const int CERT_STORE_PROV_FILENAME_A = 7;

	public const int CERT_STORE_PROV_MEMORY = 2;

	public const int CERT_STORE_PROV_PKCS7 = 5;

	public const int CERT_STORE_PROV_SERIALIZED = 6;

	public const int CERT_STORE_PROV_SYSTEM_A = 9;

	public const int CERT_STORE_SAVE_AS_PKCS7 = 2;

	public const int CERT_STORE_SAVE_AS_STORE = 1;

	public const int CERT_STORE_SAVE_TO_MEMORY = 2;

	public const int CERT_SYSTEM_STORE_CURRENT_SERVICE = 262144;

	public const int CERT_SYSTEM_STORE_CURRENT_SERVICE_ID = 4;

	public const int CERT_SYSTEM_STORE_CURRENT_USER = 65536;

	public const int CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY = 458752;

	public const int CERT_SYSTEM_STORE_CURRENT_USER_GROUP_POLICY_ID = 7;

	public const int CERT_SYSTEM_STORE_CURRENT_USER_ID = 1;

	public const int CERT_SYSTEM_STORE_LOCAL_MACHINE = 131072;

	public const int CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE = 589824;

	public const int CERT_SYSTEM_STORE_LOCAL_MACHINE_ENTERPRISE_ID = 9;

	public const int CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY = 524288;

	public const int CERT_SYSTEM_STORE_LOCAL_MACHINE_GROUP_POLICY_ID = 8;

	public const int CERT_SYSTEM_STORE_LOCAL_MACHINE_ID = 2;

	public const int CERT_SYSTEM_STORE_LOCATION_SHIFT = 16;

	public const int CERT_SYSTEM_STORE_SERVICES = 327680;

	public const int CERT_SYSTEM_STORE_SERVICES_ID = 5;

	public const int CERT_SYSTEM_STORE_USERS = 393216;

	public const int CERT_SYSTEM_STORE_USERS_ID = 6;

	public const int CERT_X500_NAME_STR = 3;

	public const int CMSG_CERT_PARAM = 12;

	public const int CP_ACP = 0;

	public const int CRYPT_ACQUIRE_COMPARE_KEY_FLAG = 4;

	public const int CRYPT_ACQUIRE_SILENT_FLAG = 64;

	public const int CRYPT_E_EXISTS = -2146885627;

	public const int CRYPT_E_REVOCATION_OFFLINE = -2146885613;

	public const int CRYPT_E_REVOKED = -2146885616;

	public const int CRYPT_EXPORTABLE = 1;

	public const int CRYPT_FIND_SILENT_KEYSET_FLAG = 64;

	public const int CRYPT_FIRST = 1;

	public const int CRYPT_MACHINE_KEYSET = 32;

	public const int CRYPT_NEWKEYSET = 8;

	public const int CRYPT_PREGEN = 64;

	public const int CRYPT_SILENT = 64;

	public const int CRYPT_USER_KEYSET = 4096;

	public const int CRYPT_VERIFYCONTEXT = -268435456;

	public const int CRYPTPROTECT_LOCAL_MACHINE = 4;

	public const int CRYPTPROTECT_UI_FORBIDDEN = 1;

	public const int CRYPTPROTECT_VERIFY_PROTECTION = 64;

	public const int ERROR_MORE_DATA = 234;

	public const int EXPORT_PRIVATE_KEYS = 4;

	public const int HP_HASHVAL = 2;

	public const int ISC_REQ_ALLOCATE_MEMORY = 256;

	public const int ISC_REQ_CONFIDENTIALITY = 16;

	public const int ISC_REQ_EXTENDED_ERROR = 16384;

	public const int ISC_REQ_MANUAL_CRED_VALIDATION = 524288;

	public const int ISC_REQ_MUTUAL_AUTH = 2;

	public const int ISC_REQ_REPLAY_DETECT = 4;

	public const int ISC_REQ_SEQUENCE_DETECT = 8;

	public const int ISC_REQ_STREAM = 32768;

	public const int ISC_RET_EXTENDED_ERROR = 16384;

	public const string KEY_CONTAINER = "{48959A69-B181-4cdd-B135-7565701307C5}";

	public const int KP_ALGID = 7;

	public const int KP_BLOCKLEN = 8;

	public const int KP_G = 12;

	public const int KP_IV = 1;

	public const int KP_KEYLEN = 9;

	public const int KP_MODE = 4;

	public const int KP_MODE_BITS = 5;

	public const int KP_P = 11;

	public const int KP_PADDING = 3;

	public const int KP_X = 14;

	public const int LMEM_FIXED = 0;

	public const int LMEM_ZEROINIT = 64;

	public const int NTE_BAD_KEYSET = -2146893802;

	public const int NTE_EXISTS = -2146893809;

	public const int PKCS_7_ASN_ENCODING = 65536;

	public const int PKCS5_PADDING = 1;

	public const int PLAINTEXTKEYBLOB = 8;

	public const int PP_ENUMALGS_EX = 22;

	public const int PRIVATEKEYBLOB = 7;

	public const int PROV_DSS_DH = 13;

	public const int PROV_RSA_AES = 24;

	public const int PROV_RSA_FULL = 1;

	public const int PUBLICKEYBLOB = 6;

	public const int RANDOM_PADDING = 2;

	public const int RSA_CSP_PUBLICKEYBLOB = 19;

	public const int SCH_CRED_AUTO_CRED_VALIDATION = 32;

	public const int SCH_CRED_MANUAL_CRED_VALIDATION = 8;

	public const int SCH_CRED_NO_DEFAULT_CREDS = 16;

	public const int SCH_CRED_NO_SERVERNAME_CHECK = 4;

	public const int SCH_CRED_USE_DEFAULT_CREDS = 64;

	public const int SCHANNEL_CRED_VERSION = 4;

	public const int SCHANNEL_RENEGOTIATE = 0;

	public const int SCHANNEL_SHUTDOWN = 1;

	public const int SEC_E_ILLEGAL_MESSAGE = -2146893018;

	public const int SEC_E_INCOMPLETE_MESSAGE = -2146893032;

	public const int SEC_E_INVALID_TOKEN = -2146893048;

	public const int SEC_E_NO_CREDENTIALS = -2146893042;

	public const int SEC_E_OK = 0;

	public const int SEC_I_CONTEXT_EXPIRED = 590615;

	public const int SEC_I_CONTINUE_NEEDED = 590610;

	public const int SEC_I_INCOMPLETE_CREDENTIALS = 590624;

	public const int SEC_I_RENEGOTIATE = 590625;

	public const int SECBUFFER_DATA = 1;

	public const int SECBUFFER_EMPTY = 0;

	public const int SECBUFFER_EXTRA = 5;

	public const int SECBUFFER_STREAM_HEADER = 7;

	public const int SECBUFFER_STREAM_TRAILER = 6;

	public const int SECBUFFER_TOKEN = 2;

	public const int SECBUFFER_VERSION = 0;

	public const int SECPKG_ATTR_ISSUER_LIST_EX = 89;

	public const int SECPKG_ATTR_LOCAL_CERT_CONTEXT = 84;

	public const int SECPKG_ATTR_REMOTE_CERT_CONTEXT = 83;

	public const int SECPKG_ATTR_STREAM_SIZES = 4;

	public const int SECPKG_CRED_INBOUND = 1;

	public const int SECPKG_CRED_OUTBOUND = 2;

	public const int SECURITY_NATIVE_DREP = 16;

	public const int SIMPLEBLOB = 1;

	public const string szOID_COMMON_NAME = "2.5.4.3";

	public const string szOID_ORGANIZATION_NAME = "2.5.4.10";

	public const string szOID_RSA_unstructName = "1.2.840.113549.1.9.2";

	public const int TRUST_E_BASIC_CONSTRAINTS = -2146869223;

	public const int TRUST_E_CERT_SIGNATURE = -2146869244;

	public const int X509_ANY_STRING = 6;

	public const int X509_ASN_ENCODING = 1;

	public const int X509_NAME = 7;

	public const int X509_NAME_VALUE = 6;

	public const int X509_UNICODE_ANY_STRING = 24;

	public const int X509_UNICODE_NAME = 20;

	public const int X509_UNICODE_NAME_VALUE = 24;

	public const int ZERO_PADDING = 3;

	private SecurityConstants()
	{
	}
}
