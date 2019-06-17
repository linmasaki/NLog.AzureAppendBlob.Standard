# NLog.AzureAppendBlob.Standard
[![Version](https://img.shields.io/nuget/vpre/NLog.AzureAppendBlob.Standard.svg)](https://www.nuget.org/packages/NLog.AzureAppendBlob.Standard) 
[![Downloads](https://img.shields.io/nuget/dt/NLog.AzureAppendBlob.Standard.svg)](https://www.nuget.org/packages/NLog.AzureAppendBlob.Standard)  
An NLog target using Microsoft Azure Storage Append Blobs in .Net Core App(1.x ~ 2.x), reference this [NLog.AzureAppendBlob](https://github.com/heemskerkerik/NLog.AzureAppendBlob).

## How To Use ##
Install the [NLog.AzureAppendBlob.Standard](https://www.nuget.org/packages/NLog.AzureAppendBlob.Standard/) package from NuGet. If you use NLog 4.x or higher, it will automatically load the extension assembly. In further, put the following in your NLog configuration(.Net Core must do):

```xml
<nlog>
    <extensions>
        <add assembly="NLog.AzureAppendBlob.Standard" />
    </extensions>
</nlog>
```

If still not work, you can try to Register manually (choose one) in your application start. e.g. main(), app_start().  
```C#
1. Target.Register<NLog.AzureAppendBlob.Standard.AzureAppendBlobTarget>("AzureAppendBlob"); //generic
```
```C#
2. ConfigurationItemFactory.Default.Targets.RegisterDefinition("AzureAppendBlob", typeof(NLog.AzureAppendBlob.Standard.AzureAppendBlobTarget)); //old syntax
```  

### Target configuration ###
The target's type name is ``AzureAppendBlob``.

* **layout** - (layout) Content text to write.
* **connectionString** - (layout) The connection string of the storage account. Consult the Azure Portal to retrieve this.
* **container** - (layout) The name of the blob container where logs will be placed. It will be created automatically when it does not exist.
* **blobName** - (layout) The name of the blob to write to. It will be created automatically when it does not exist(only once, unless you set **forceCheck** to ``true``).
* **forceCheck(Optional)** - (bool) Check if the target blob exists for each write.

### Sample ###

```xml
<targets async="true">
    <target xsi:type="AzureAppendBlob" 
            name="Azure" 
            layout="${longdate} ${uppercase:${level}} - ${message}" 
            connectionString="YourConnectionString" 
            container="YourContainer" 
            blobName="logs/${shortdate}.log" 
            forceCheck= "false" />
</targets>
<rules>
    <logger name="*" minlevel="Trace" writeTo="Azure"/>
</rules>
```

You can see [NLog Wiki](https://github.com/NLog/NLog) for more information about configuring NLog.


## Test App ##
NLog.AzureAppendBlob.Standard.Test is a console program that is preconfigured to use the ``AzureAppendBlob`` target. To test it, you'll have to create an Azure storage account and a blob account.

## Note ##
If you need other NLog extensions(Target or Layout Renderer) that built by me or prefer all in one like me. You can visit [this](https://www.nuget.org/profiles/CoCo).

## Reference ##  
[NLog.AzureAppendBlob](https://github.com/heemskerkerik/NLog.AzureAppendBlob) by [Erik Heemskerk](https://github.com/heemskerkerik)
