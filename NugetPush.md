1. Switch to Release
2. Select project --> right button --> Pack
3. Project properties --> Package --> Package version --> ++
4. Send packages to server

	cd C:\PrivProjects\CqrsDemo\CqrsDemo\bin\Release  
	dotnet nuget push .\tBlabs.Cqrs.Core.1.3.0.nupkg --api-key KEY --source https://api.nuget.org/v3/index.json

	C:\PrivProjects\CqrsDemo\Middlewares\bin\Release  
	dotnet nuget push .\tBlabs.Cqrs.Middleware.1.2.0.nupkg --api-key KEY --source https://api.nuget.org/v3/index.json

5. Go to https://www.nuget.org/account/Packages and wait for package update