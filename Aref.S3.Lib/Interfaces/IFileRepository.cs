/*
 * 
 *      S3 FILE REPOSITORY
 *      WRITTEN BY AREF KARIMI OCTOBER 2014
 *      WWW.ASPGUY.WORDPRESS.COM
 *      
 */

using System.Collections.Generic;

namespace Aref.S3.Lib.Interfaces
{
    public interface IFileRepository
    {
        void Download(string fileName, string targetPath);
        void ChangeDir(string relativePath);
        IEnumerable<string> GetFileNames(string pattern);
        IEnumerable<string> GetSubdirNames(string startRelativeFolder = "");
        void AddFile(string localFilePath);
        bool FileExists(string relativeFileName);
        void DeleteFile(string relativeFileName);
    }
}