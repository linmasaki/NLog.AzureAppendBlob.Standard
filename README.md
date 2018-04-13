# NLog.AzureAppendBlob.Standard
An NLog target using Microsoft Azure Storage Append Blobs With .NET Standard2.0, reference of [NLog.AzureAppendBlob](https://github.com/heemskerkerik/NLog.AzureAppendBlob).

## How To Use ##
Install the [NLog.AzureAppendBlob.Standard](https://www.nuget.org/packages/NLog.AzureAppendBlob.Standard/) package from NuGet. If you use NLog 4.x or higher, NLog will automatically load the extension assembly. Otherwise, put the following in your NLog configuration:

    <nlog>
        <extensions>
            <add assembly="NLog.AzureBlobStorage" />
        </extensions>
    </nlog>

### Target configuration ###
The target's type name is ``AzureAppendBlob``.

* **connectionString** - (layout) The connection string for the storage account to use. Consult the Azure Portal to retrieve this.
* **container** - (layout) The name of the blob container where logs will be placed. Will be created when it does not exist.
* **blobName** - (layout) Name of the blob to write to. Will be created when it does not exist.
* **layout** - (layout) Content text to write.

### Sample ###
    <targets async="true">
      <target type="AzureAppendBlob" 
              name="Azure" 
              layout="${longdate} ${level:uppercase=true} - ${message}" 
              connectionString="${appsetting:name=StorageConnectionString}" 
              container="logtest" 
              blobName="${date:format=yyyy-MM-dd}.log" />
    </targets>
    <rules>
    

See the [NLog Wiki](https://github.com/NLog/NLog) for more information about configuring NLog.
