## Bureaucratize

Sourcecode for my engineer degree diploma. 

### Technologies
- CNTK (CNN) + Accord.NET
- ASP.NET Core 
- Akka.NET

### How to build
- Get Visual Studio 2017 - https://www.visualstudio.com/downloads/
- Get .NET Framework 4.6.2 - https://www.microsoft.com/net/download/dotnet-framework-runtime/net462
- Get .NET Core 2.0 SDK (or later, ie. 2.1) - https://www.microsoft.com/net/download/windows
- Add local nuget source to point at **./static/local-nuget** folder in Visual Studio (Tools -> Options -> NuGet Package Manager -> Package Sources)
- Run chosen solution:
  * **./src/Bureaucratize/Bureaucratize.sln** for main WebApp and AkkaHost
  * **./src/Bureaucratize.FileStorage/Bureaucratize.FileStorage.sln** for FileStorage service
  * **./src/Bureaucratize.MachineLearning/Bureaucratize.MachineLearning.sln** for machine learning ConsoleApp (requires GPU for NVIDIA CUDA acceleration!)
  
### Known issues
- Sometimes WebApp (**Bureaucratize.Web**) or AkkaHost (**Bureaucratize.ImageProcessing.Host**) will fail to build due to mismatch of libraries between .NET Framework and .NET Core when you build **entire solution**; to fix this, REBUILD only chosen application project (sometimes you might need to REBUILD and then - immediately after - BUILD chosen application .csproj);
- if ImageProcessing.Host doesn't start first, Bureaucratize.Web won't connect properly (ever). You need to restart AppPool where Bureaucratize.Web is installed so that it will connect properly to ActorSystem for the first time; if you're running it via Visual Studio (via IIS Express), just stop the web app and run it again;

### Project has been curated under PJATK

![](http://www.pja.edu.pl/templates/pjwstk/images/logo-lg-md.png)

**Website:** http://www.pja.edu.pl
