# WebFileExplorer Application
## Stack of technologies:
### Back-end
* ASP.NET 5 MVC & WebApi
* Autofac
* Nancy (for service)

### Front-End
* Angular 1.5
* Semantic UI

## Running application
To run application, build and run **both** solutions from folder WebFileExplorer and WebFileExplorer.Service (run them togher!)
## Overview
### Screenshots
![alt](http://s45.radikal.ru/i110/1609/72/c419351230e3.png)
![alt](http://s020.radikal.ru/i717/1609/0e/1fce4df60dc3.png)
![alt](http://s019.radikal.ru/i613/1609/b9/fe5bfb0d2ba9.png)
## Architecture of project
Arhitecture of project divided on two parts: ASP.NET Single Page Application (**WebFileExplorer**) and REST Service (**WebFileExplorer.Service**). WebFileExplorer is responsible for displaying view and WebApi for angular servies. WebFileExplorer sends messages to WebFileExplorer.Service, which responsible for getting entries of drives, files in directories and counting files.  
![alt](http://s017.radikal.ru/i438/1609/06/c5689218a91a.png)
### WebFileExplorer
For entire lifecycle of page, application generates uniqe guid token which helps service to identify requests. From front-end, application sends requests to WebApi, WebApi controllers sends requests WebFileExplorer.Service, preparing response from service and sending it back to front-end.
### WebFileExplorer.Service
WebFileExplorer.Service is console application, which bootstraps REST Service (using Nancy Framework). In service implemented fast algorythm for counting files in directories and fast implementation for recursive enumerating all nested files and directories, especially I'd want to notice, that enumeration services handles all errors and gives opportunity to discover as much files and directories as possible.
### Caching WebFileExplorer.Service
Service collects results of all its operations and caching it in simple realization in-memmory cache repository. Application by default sents request for getting cached results to prevent a lot of disk IO operations. Of course, user in application can manually update this results and application will send request to service for non-cached result to check for changing filesystem entries. Because usually hard drives information understands as something very permanent, cache is not expiring by any timeouts and will be actual for entire life of service.
### Request pool in WebFileExplorer.Service
One of the most important feature why I have implemented separate service. In Service implemented pool of operation. Because application sents to service identification token, service can understand source of operation. Service knows about all actual pending operations and who requested them. Service gives maximum execution timeout for each operation four seconds (configurable in appsettings.json), after timeout, operation pool requests cancelling operation, if operation was cancelled, user on fron-end will see, that system can't analyze so huge directory (of course system can, if timeout for operation will be increased). If service got request _for the same operation from one source_, it will request to operation pool to kill it, because previvous operation is not neccessery and its result will not be used by anybody.
### Application design
Application was implemented with using Dependency Injection pattern (with Inversion of Control Container Autofac) and code was written with thought for hight unit testability and possibility for isolation from dependencies system under test. Exactly, WebFileExplorer.Service have modulable plugin architecture and can be very easy extended by more independent plugins. You just need to register another new IPlugingBootstrapper and service bootstrapper will run your module. 

## Projects in solution
| Project name                    | Description           
| ------------------------------- |:-------------:|
| WebFileExplorer.RestClient       | Implementation of client for sending requests and receving responses from REST service |
| WebFileExplorer.Domain       | Domain classes for both solutions, which contains structure of messages between service and application      |
| WebFileExplorer | ASP.NET Project with application |
|WebFileExplorer.Service.Core|Contains fundamental architecture classes for service|
|WebFileExplorer.Caching|Implementation in-memmory caching for this service|
|WebFileExplorer.Service.FileSystem|Conains classes and services with manipulation of fil system|
|WebFileExplorer.Service.REST|Porject for REST Service which uses by WebFileExplorer Application|
|WebFileExplorer.Service|Composition root and runner for service|