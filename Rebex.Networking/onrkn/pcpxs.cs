using System;
using System.Net;
using System.Net.Sockets;
using Rebex;
using Rebex.Net;
using Rebex.Security.Certificates;

namespace onrkn;

internal interface pcpxs : IDisposable, maowd
{
	Socket iwyxk { get; }

	int awmjj { get; set; }

	EndPoint tqzpe { get; }

	EndPoint mmvhg { get; }

	Rebex.Net.SocketInformation kbsub { get; }

	bool oecnz { get; }

	int rmigt { get; }

	string nqvnm { get; }

	TlsSession ttuom { get; }

	TlsCipher rfidp { get; }

	CertificateChain pcsnf { get; }

	CertificateChain zuacv { get; }

	bool hjgnp { get; }

	TlsConnectionEnd kmhlw { get; }

	TlsCompressionMethod eselh { get; }

	TlsParameters wjrxg { get; set; }

	TlsDebugLevel btazw { get; set; }

	ILogWriter ogfaj { get; set; }

	ISocketFactory kzyui { get; }

	string sgrgy { get; set; }

	event EventHandler<TlsDebugEventArgs> kauiz;

	SocketState zeuxb();

	bool frnhp(int p0, SocketSelectMode p1);

	int dpdcv(byte[] p0);

	int iigng(byte[] p0, SocketFlags p1);

	int fdatf(byte[] p0, int p1, SocketFlags p2);

	int cwstz(byte[] p0, int p1, int p2);

	int lebxx(ArraySegment<byte> p0);

	int qekdr(byte[] p0);

	int knqbq(byte[] p0, SocketFlags p1);

	int xkqrs(byte[] p0, int p1, SocketFlags p2);

	int rdein(byte[] p0, int p1, int p2);

	int fqpqe(ArraySegment<byte> p0);

	void kalfy();

	void evkkv();

	void zxphg();

	void brfcm(EndPoint p0);

	void ydjrx(string p0, int p1);

	IAsyncResult pyczw(EndPoint p0, AsyncCallback p1, object p2);

	IAsyncResult otfps(string p0, int p1, AsyncCallback p2, object p3);

	void ptzgj(IAsyncResult p0);

	int bdjkg(byte[] p0, int p1, int p2, SocketFlags p3);

	IAsyncResult duzhm(byte[] p0, int p1, int p2, SocketFlags p3, AsyncCallback p4, object p5);

	int ksyaj(IAsyncResult p0);

	int leuiu(byte[] p0, int p1, int p2, SocketFlags p3);

	IAsyncResult wktun(byte[] p0, int p1, int p2, SocketFlags p3, AsyncCallback p4, object p5);

	int puqjz(IAsyncResult p0);

	IAsyncResult tbjai(AsyncCallback p0, object p1);

	void wmtjp(IAsyncResult p0);

	object axzix(object p0);

	void ppzyr(SocketShutdown p0);

	void nptsw();
}
