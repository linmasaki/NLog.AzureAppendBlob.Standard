using System;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;


namespace NLog.AzureAppendBlob.Standard
{
    [Target("AzureAppendBlob")]
    public sealed class AzureAppendBlobTarget : TargetWithLayout
    {
        [RequiredParameter]
        public Layout ConnectionString { get; set; }

        [RequiredParameter]
        public Layout Container { get; set; }

        [RequiredParameter]
        public Layout BlobName { get; set; }

        private CloudBlobClient _client;
        private CloudBlobContainer _container;
        private CloudAppendBlob _blob;

        //public AzureAppendBlobTarget()
        //{
        //    _client = null;
        //}

        protected override void InitializeTarget()
		{
			base.InitializeTarget();
			_client = null;
		}        

        protected override void Write(LogEventInfo logEvent)
        {
            _client = CloudStorageAccount.Parse(ConnectionString.Render(logEvent)).CreateCloudBlobClient();

            if (_client == null)
            {
                return;
            }

            string containerName = Container.Render(logEvent);
            string blobName = BlobName.Render(logEvent);

            if (_container == null || _container.Name != containerName)
            {
                _container = _client.GetContainerReference(containerName);
                _container.CreateIfNotExistsAsync().Wait();
                _blob = null;
            }

            if (_blob == null || _blob.Name != blobName)
            {
                _blob = _container.GetAppendBlobReference(blobName);

                if (!_blob.ExistsAsync().Result)
                {
                    try
                    {
                        _blob.Properties.ContentType = "text/plain";
                        _blob.CreateOrReplaceAsync().Wait();
                    }
                    catch (StorageException ex) when (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.Conflict)
                    {
                        // to be expected
                    }
                }
            }

            _blob.AppendTextAsync(Layout.Render(logEvent) + "\r\n").Wait();
        }

    }
}