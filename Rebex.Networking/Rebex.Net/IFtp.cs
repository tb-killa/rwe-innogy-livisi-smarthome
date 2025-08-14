using System;
using System.IO;
using System.Text;
using Rebex.IO;

namespace Rebex.Net;

public interface IFtp : IDisposable
{
	IFtpSettings Settings { get; }

	ILogWriter LogWriter { get; set; }

	string UserName { get; }

	string ServerName { get; }

	int ServerPort { get; }

	Encoding Encoding { get; set; }

	bool IsConnected { get; }

	bool IsAuthenticated { get; }

	int MaxDownloadSpeed { get; set; }

	int MaxUploadSpeed { get; set; }

	int Timeout { get; set; }

	TransferType TransferType { get; set; }

	event EventHandler<ProblemDetectedEventArgs> ProblemDetected;

	event EventHandler<TraversingEventArgs> Traversing;

	event EventHandler<TransferProgressChangedEventArgs> TransferProgressChanged;

	event EventHandler<DeleteProgressChangedEventArgs> DeleteProgressChanged;

	event EventHandler<ListItemReceivedEventArgs> ListItemReceived;

	void CheckConnectionState();

	void KeepAlive();

	void Login(string userName, string password);

	void Abort();

	void Disconnect();

	string CreateDirectory(string remotePath);

	void ChangeDirectory(string remotePath);

	void RemoveDirectory(string remotePath);

	void Rename(string fromPath, string toPath);

	void SetFileDateTime(string remotePath, DateTime newDateTime);

	void DeleteFile(string remotePath);

	void Delete(string remotePath, TraversalMode traversalMode);

	void Delete(FileSet set);

	bool DirectoryExists(string remotePath);

	bool FileExists(string remotePath);

	string GetCurrentDirectory();

	DateTime GetFileDateTime(string remotePath);

	long GetFileLength(string remotePath);

	FileSystemItem GetInfo(string remotePath, bool failIfNotFound);

	Stream GetDownloadStream(string remotePath);

	Stream GetDownloadStream(string remotePath, SeekOrigin origin, long offset);

	Stream GetUploadStream(string remotePath);

	Stream GetUploadStream(string remotePath, SeekOrigin origin, long offset);

	long GetFile(string remotePath, Stream outputStream);

	long GetFile(string remotePath, string localPath);

	long GetFile(string remotePath, Stream outputStream, long remoteOffset);

	long GetFile(string remotePath, string localPath, long remoteOffset, long localOffset);

	long PutFile(Stream sourceStream, string remotePath);

	long PutFile(string localPath, string remotePath);

	long PutFile(Stream sourceStream, string remotePath, long remoteOffset, long length);

	long PutFile(string localPath, string remotePath, long localOffset, long remoteOffset, long length);

	void Download(string remotePath, string localDirectoryPath);

	void Download(string remotePath, string localDirectoryPath, TraversalMode traversalMode);

	void Download(string remotePath, string localDirectoryPath, TraversalMode traversalMode, TransferMethod transferMethod, ActionOnExistingFiles existingFileMode);

	void Download(FileSet set, string localDirectoryPath);

	void Download(FileSet set, string localDirectoryPath, TransferMethod transferMethod, ActionOnExistingFiles existingFileMode);

	void Upload(string localPath, string remoteDirectoryPath);

	void Upload(string localPath, string remoteDirectoryPath, TraversalMode traversalMode);

	void Upload(string localPath, string remoteDirectoryPath, TraversalMode traversalMode, TransferMethod transferMethod, ActionOnExistingFiles existingFileMode);

	void Upload(FileSet set, string remoteDirectoryPath);

	void Upload(FileSet set, string remoteDirectoryPath, TransferMethod transferMethod, ActionOnExistingFiles existingFileMode);

	FileSystemItemCollection GetList();

	FileSystemItemCollection GetList(string arguments);

	FileSystemItemCollection GetItems(string remotePath);

	FileSystemItemCollection GetItems(string remotePath, TraversalMode traversalMode);

	FileSystemItemCollection GetItems(FileSet set);

	string[] GetNameList();

	string[] GetNameList(string arguments);

	string[] GetRawList();

	string[] GetRawList(string arguments);

	Checksum GetChecksum(string remotePath, ChecksumAlgorithm algorithm);

	Checksum GetChecksum(string remotePath, ChecksumAlgorithm algorithm, long offset, long count);
}
