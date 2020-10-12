"# banking_project" 

How To Run:

Requirements: 

  -dotnet (https://docs.microsoft.com/pl-pl/dotnet/core/install/windows?tabs=netcore31)
  
  -.NET Core 3,1 SDK or newer (can install by NuGet Packages)
  
  -heavily sugested visual 2019
  
  
 After cloning repo in command console go to web and api and run in each folder command:
 
  -dotnet build
  
 it will build project and install dependencies.
 
 After building servieces run:
 
  -dotnet run -> in web
  
  -dotnet run --urls=http://localhost:5001/ in api
  
 connect to http://localhost:5000/
 
 and yes it's fucking hello world!
 
 
 api won't work if you don't have mongodb database (currently unnecessary) but it has leftover code.
 you can check it out to introduce yourself to how it works
