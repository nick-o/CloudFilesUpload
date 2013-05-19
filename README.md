CloudFilesUpload
================


This is a simple example Console App written in .NET using the Rackspace Openstack.net SDK to upload local files to a given Container on a CloudFiles account

Usage
----------

Download via git and change into subdir:
```PoSh
git clone https://github.com/nick-o/CloudFilesUpload.git
cd CloudFilesUpload
```
Compile via PowerShell:
```PoSh
#Get directory of .NET Runtime
$NETdir = [System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()
#Add Runtime dir to PATH environment variable
$env:Path += ";" + $NETdir

#Build using msbuild, default Configuration is Debug so specifying Release Configuration
#using the Rebuild target which runs a clean first
MSBuild.exe .\CloudFilesUpload.sln /t:Rebuild /p:Configuration=Release
```

Ouput directory is bin/Release. Usage per the below:
```PoSh
.\CloudFilesUpload.exe username api_key target_container path_to_file [region (US|UK)]
```

NOTE: Region must be one of US|UK. When omitted, default region (US) will be used
