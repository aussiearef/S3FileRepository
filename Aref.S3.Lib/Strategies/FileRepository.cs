/*
 *      S3 FILE REPOSITORY
 *      WRITTEN BY AREF KARIMI OCTOBER 2014
 *      WWW.ASPGUY.WORDPRESS.COM
 *      
 */

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Aref.S3.Lib.Interfaces;

namespace Aref.S3.Lib.Strategies
{
    public class FileRepository : IFileRepository
    {
        private readonly IAmazonS3 _client;
        private readonly S3FileRepositoryConfig _config;
        private readonly TransferUtility _transferAgent;
        private string _workingDir = "/";

        public FileRepository()
        {
            _config = ConfigurationManager.GetSection("AspGuy/S3Repository") as S3FileRepositoryConfig;

            if (_config == null)
            {
                throw new ConfigurationErrorsException(
                    "A configuration section for S3FileRepositoryConfig is expected but it is missing in the configuration file.");
            }

            bool aConfigValueIsMissing = string.IsNullOrEmpty(_config.AccessKey) ||
                                         string.IsNullOrEmpty(_config.SecretKey) ||
                                         string.IsNullOrEmpty(_config.RegionName)
                                         || string.IsNullOrEmpty(_config.RootBucketName);

            if (aConfigValueIsMissing)
                throw new ConfigurationErrorsException(
                    "A configuration attribute of S3FileRepositoryConfig class is missing. Please check the configuration file.");


            _client = AWSClientFactory.CreateAmazonS3Client(_config.AccessKey, _config.SecretKey,
                RegionEndpoint.GetBySystemName(_config.RegionName));
            _transferAgent = new TransferUtility(_client);
            _workingDir = _config.RootDir;
        }


        public void Download(string fileName, string targetPath)
        {
            string key;
            if (fileName.StartsWith(_workingDir, true, CultureInfo.InvariantCulture))
            {
                key = fileName;
                targetPath = string.Format("{0}\\{1}", targetPath, Path.GetFileName(fileName));
            }
            else
            {
                string justFileName =
                    fileName
                        .Split(new[] {"/"}, StringSplitOptions.RemoveEmptyEntries)
                        .LastOrDefault() ?? "";
                targetPath = Path.Combine(targetPath, justFileName);
                key = string.Format("{0}/{1}", _workingDir, fileName);
            }

            _transferAgent.Download(targetPath, _config.RootBucketName, key);
        }


        public void ChangeDir(string relativePath)
        {
            _workingDir = relativePath.StartsWith("/")
                ? string.Format("{0}{1}", _config.RootDir, relativePath)
                : _workingDir.EndsWith("/")
                    ? string.Format("{0}{1}", _workingDir, relativePath)
                    : string.Format("{0}/{1}", _workingDir, relativePath);
        }

        public IEnumerable<string> GetFileNames(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                pattern = "*.*";

            string path = Path.GetDirectoryName(pattern).Replace(@"\", @"/");
            path = string.Format("{0}/{1}", _workingDir, path);

            var request = new ListObjectsRequest
            {
                BucketName = _config.RootBucketName,
                Delimiter = "",
                Prefix = path
            };

            ListObjectsResponse response = _client.ListObjects(request);
            return
                response.S3Objects.Select(x => x.Key)
                    .Concat(response.CommonPrefixes)
                    .Where(x => !x.EndsWith("/"))
                    .ToList();
        }

        public IEnumerable<string> GetSubdirNames(string startRelativeFolder = "")
        {
            string prefix = string.Format("{0}/{1}", _workingDir, startRelativeFolder);
            var request = new ListObjectsRequest
            {
                BucketName = _config.RootBucketName,
                Delimiter = "/",
                Prefix = prefix
            };

            ListObjectsResponse response = _client.ListObjects(request);
            IEnumerable<string> folders = response.CommonPrefixes.Where(s => s.EndsWith("/"));
            IEnumerable<string> result = folders.Select(s =>
            {
                if (s.StartsWith(prefix))
                    s = s.Remove(0, prefix.Length);

                string[] parts = s.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
                return parts.Length > 0 ? parts[0] : "";
            });

            return result;
        }

        public void AddFile(string localFilePath)
        {
            _transferAgent.Upload(localFilePath, _config.RootBucketName,
                string.Format("{0}/{1}", _workingDir, Path.GetFileName(localFilePath)));
        }

        public bool FileExists(string relativeFileName)
        {
            string key = string.Format("{0}{1}", _workingDir, relativeFileName);
            GetObjectResponse s3Obj = _client.GetObject(_config.RootBucketName, key);
            return s3Obj != null;
        }

        public void DeleteFile(string relativeFileName)
        {
            string rootKey = string.Format("{0}/{1}", _workingDir, relativeFileName);
            List<string> allObjects = _client.ListObjects(new ListObjectsRequest
            {
                BucketName = _config.RootBucketName,
                Prefix = rootKey
            }).CommonPrefixes;

            allObjects.AsParallel().ForAll(x => _client.DeleteObject(_config.RootBucketName, x));
        }
    }
}