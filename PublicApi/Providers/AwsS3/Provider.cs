using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicApi.Providers.AwsS3
{
    public static class Provider
    {
        internal static string accessKey = "accessKey";
        internal static string secretaccessKey = "secretAccessKey";
        internal static AmazonS3Config conf = new AmazonS3Config { ServiceURL = "http://api.eric.s3storage.ru", SignatureVersion = "2" };
        public static async Task<string> UploadFromStream(Stream fileBody, string extension, string container, string bucket = "pub")
        {
            using (var client = new AmazonS3Client(accessKey, secretaccessKey, conf))
            {
                var fileGuid = Guid.NewGuid().ToString();
                var keyName = string.Format("{0}/{1}.{2}", container, fileGuid, extension);
                var req = new PutObjectRequest { InputStream = fileBody, BucketName = bucket, Key = keyName, CannedACL = S3CannedACL.PublicRead };
                var res = await client.PutObjectAsync(req);
                if (res.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return keyName;
                else
                    throw new Exception("Upload unsuccessful, error code " + (int)res.HttpStatusCode);
            }
        }

        public static async Task<string> UploadFromMemory(byte[] fileBody, string extension, string container, string bucket = "pub")
        {
            return await UploadFromStream(new MemoryStream(fileBody), extension, container, bucket);
        }

        public static async Task<bool> Exists(string fileName, string container, string bucket = "pub")
        {
            using (var client = new AmazonS3Client(accessKey, secretaccessKey, conf))
            {
                var keyName = String.Format("{0}/{1}", container, fileName);

                try
                {
                    var response = await client.GetObjectMetadataAsync(new GetObjectMetadataRequest()
                    {
                        BucketName = bucket,
                        Key = keyName
                    });

                    return true;
                }

                catch (Exception ex)
                {
                    var aex = (AmazonS3Exception)ex.InnerException;
                    if (aex.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return false;

                    //status wasn't not found, so throw the exception
                    throw;
                }
            }
        }

        public static async Task<MemoryStream> Download(string fileName, string container, string bucket = "pub")
        {
            MemoryStream ms = new MemoryStream();
            using (var client = new AmazonS3Client(accessKey, secretaccessKey, conf))
            {
                var keyName = String.Format("{0}/{1}", container, fileName);

                var request = new GetObjectRequest
                {
                    BucketName = bucket,
                    Key = keyName
                };

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                {
                    response.ResponseStream.CopyTo(ms);
                }
            }
            return ms;
        }

        public static async Task<bool> FileDelete(string path, string bucket = "manage")
        {
            using (var client = new AmazonS3Client(accessKey, secretaccessKey, conf))
            {
                var req = new DeleteObjectRequest { BucketName = bucket, Key = path };
                var res = await client.DeleteObjectAsync(req);
                if (res.HttpStatusCode == System.Net.HttpStatusCode.OK)
                    return true;
                else
                    return false;
            }
        }

        public static bool CopyFolderInsideS3Bucket(string source, string destination, string bucket = "pub")
        {
            using (var client = new AmazonS3Client(accessKey, secretaccessKey, conf))
            {

                var strippedSource = source;
                var strippedDestination = destination;

                // process source
                if (strippedSource.StartsWith("/"))
                    strippedSource = strippedSource.Substring(1);
                if (strippedSource.EndsWith("/"))
                    strippedSource = source.Substring(0, strippedSource.Length - 1);

                var sourceParts = strippedSource.Split('/');
                var sourceBucket = sourceParts[0];

                var sourcePrefix = new StringBuilder();
                for (var i = 1; i < sourceParts.Length; i++)
                {
                    sourcePrefix.Append(sourceParts[i]);
                    sourcePrefix.Append("/");
                }

                // process destination
                if (strippedDestination.StartsWith("/"))
                    strippedDestination = destination.Substring(1);
                if (strippedDestination.EndsWith("/"))
                    strippedDestination = destination.Substring(0, strippedDestination.Length - 1);

                var destinationParts = strippedDestination.Split('/');
                var destinationBucket = destinationParts[0];

                var destinationPrefix = new StringBuilder();
                for (var i = 1; i < destinationParts.Length; i++)
                {
                    destinationPrefix.Append(destinationParts[i]);
                    destinationPrefix.Append("/");
                }

                var contToken = "";

                //////ListObjectsV2Response listObjectsResult = null;

                //////var req = new ListObjectsRequest()
                //////{
                //////   BucketName = sourceBucket,
                //////   Prefix = "couriers//", // sourcePrefix.ToString(),
                //////   Delimiter = "/",
                //////   MaxKeys = 10,
                //////};

                //////ListObjectsResponse res = await client.ListObjectsAsync(req);

                var req = new ListObjectsV2Request()
                {
                    BucketName = sourceBucket,
                    Prefix = sourcePrefix.ToString(),
                    Delimiter = "/",
                    MaxKeys = 1000,
                };

                var listObjectsResult = client.ListObjectsV2Async(req).Result;
                var entries = listObjectsResult.S3Objects;
                var prefixes = listObjectsResult.CommonPrefixes;

                while (listObjectsResult.IsTruncated)
                {
                    req.ContinuationToken = listObjectsResult.NextContinuationToken;
                    listObjectsResult = client.ListObjectsV2Async(req).Result;
                    entries.AddRange(listObjectsResult.S3Objects);
                    prefixes.AddRange(listObjectsResult.CommonPrefixes);
                }

                // copy each file
                foreach (var file in entries)
                {
                    var request = new CopyObjectRequest();
                    request.SourceBucket = bucket;
                    request.SourceKey = file.Key;
                    request.DestinationBucket = destinationBucket;
                    request.DestinationKey = destinationPrefix + file.Key.Substring(sourcePrefix.Length);
                    request.CannedACL = S3CannedACL.PublicRead;
                    var response = client.CopyObjectAsync(request).Result;
                }

                // copy subfolders
                foreach (var folder in prefixes)
                {
                    var actualFolder = folder.Substring(sourcePrefix.Length);
                    actualFolder = actualFolder.Substring(0, actualFolder.Length - 1);
                    var ttt = CopyFolderInsideS3Bucket(strippedSource + "/" + actualFolder, strippedDestination + "/" + actualFolder);
                }

                return true;
            }
        }
    }
}