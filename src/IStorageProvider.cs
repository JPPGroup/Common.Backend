using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.Common.Backend
{
    //TODO: Clean this class up
    /// <summary>
    /// Interface defining platform agnostic interactions with the underlying file system
    /// </summary>
    public interface IStorageProvider
    {
        void CreateFile(string path);
        Task<Stream> OpenFileForRead(string path);
        Task<Stream> OpenFileForWrite(string path, bool createIfMissing);

        Task<Stream> OpenSharedForRead(string filename);
        Task<Stream> OpenSharedForWrite(string filename);

        Task<bool> FileExists(string path);

        Task CopyFile(string origin, string destination);
    }
}
