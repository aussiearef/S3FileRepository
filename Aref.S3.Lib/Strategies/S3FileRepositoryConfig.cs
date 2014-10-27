using System.Configuration;

namespace Aref.S3.Lib.Strategies
{
    public class S3FileRepositoryConfig : ConfigurationSection
    {
        private const string S3ReadFromAccessKey = "S3.ReadFrom.AccessKey";
        private const string S3ReadFromSecretKey = "S3.ReadFrom.SecretKey";
        private const string S3ReadFromRootBucketName = "S3.ReadFrom.Root.BucketName";
        private const string S3ReadFromRegionName = "S3.ReadFrom.RegionName";
        private const string S3ReadFromRootDir = "S3.ReadFrom.RootDir";

        [ConfigurationProperty(S3ReadFromAccessKey, IsRequired = true)]
        public string AccessKey
        {
            get { return (string) this[S3ReadFromAccessKey]; }
            set { this[S3ReadFromAccessKey] = value; }
        }

        [ConfigurationProperty(S3ReadFromSecretKey, IsRequired = true)]
        public string SecretKey
        {
            get { return (string) this[S3ReadFromSecretKey]; }
            set { this[S3ReadFromSecretKey] = value; }
        }

        [ConfigurationProperty(S3ReadFromRootBucketName, IsRequired = true)]
        public string RootBucketName
        {
            get { return (string) this[S3ReadFromRootBucketName]; }
            set { this[S3ReadFromRootBucketName] = value; }
        }

        [ConfigurationProperty(S3ReadFromRegionName, IsRequired = true)]
        public string RegionName
        {
            get { return (string) this[S3ReadFromRegionName]; }
            set { this[S3ReadFromRegionName] = value; }
        }

        [ConfigurationProperty(S3ReadFromRootDir, IsRequired = true)]
        public string RootDir
        {
            get { return (string) this[S3ReadFromRootDir]; }
            set { this[S3ReadFromRootDir] = value; }
        }
    }
}