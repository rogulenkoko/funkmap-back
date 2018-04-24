# Bandmap

Bandmap is an evolving service for fast and convenient communication between musicians and music lovers around the world.

The repository contains the back-end service of [Bandmap (demo)](https://bandmap.azurewebsites.net). Demo user: demo/demo.

Service provides REST API for client applications.

(documentation is available on https://bandmap-api.azurewebsites.net/swagger/ui/index)

## Technology Stack

### Database 
1. MongoDB (primary db)
2. Redis (cache, message queue)
3. Azure Blob Storage (binary data such as photos)

### Domain
1. .NET, C#, Owin
2. Autofac (DI Container)
3. SignalR (push-notifications)
4. NLog
5. MSTest
6. Swagger

## Installation

#### 1. .NET / VS 
First of all you have to install .NET 4.5.2 and Visual Studio. Open Funkmap solution and build the solution to restore Nuget packages.
#### 2. MongoDB 
Install MongoDB, if you are going to use local MongoDB. If you use local [MongoDB](https://www.mongodb.com/download-center#community) instance keep the configuration file (App.config/Web.config) as it is. For usage of remote MongoDB instance (cluster) you should specify it in configuration file:

```xml
 <connectionStrings>
    <add name="FunkmapMongoConnection" connectionString="<some MongoDB connection string>" />
    <add name="FunkmapMessengerMongoConnection" connectionString="<some MongoDB connection string>" />
    <add name="FunkmapAuthMongoConnection" connectionString="<some MongoDB connection string>" />
    <add name="FunkmapNotificationsMongoConnection" connectionString="<some MongoDB connection string>" />
    <add name="FunkmapFeedbackMongoConnection" connectionString="<some MongoDB connection string>" />
  </connectionStrings>
```

You can use the same instance (cluster) for all modules.

#### 3. File storage

You can use Azure Blob Storage or MongoDB's GridFS as a file storage.

Azure Blob Storage:

Settings in <appSettings> section for local Azure Blob Storage:

```xml
 <add key="file-storage" value="Azure"></add>
 <add key="azure-storage" value="UseDevelopmentStorage=true" />
```

Also for local usage you should install and run [Azure Blob Storage Emulator](https://docs.microsoft.com/ru-ru/azure/storage/common/storage-use-emulator).

Change "azure-storage" for real Azure Blob Storage usage.

GridFS:
Settings in <appSettings> section:
    
```xml
 <add key="file-storage" value="GridFs"></add>
```

#### 4. Message Queue

You can use RedisMQ or InMemory implementation as a message queue:

RedisMQ:

```xml
  <add key="message-queue-type" value="Redis"/>
  <add key="redis-primary" value="<Redis connection string>" />
```

In memory:

```xml
  <add key="message-queue-type" value="Memory"/>
```

#### 5. Cache storage

You can use RedisMQ or InMemory implementation as a message queue:

RedisMQ:

```xml
  <add key="cache-storage-type" value="Redis"/>
  <add key="redis-primary" value="<Redis connection string>" />
```

In memory:

```xml
 <add key="cache-storage-type" value="Memory"/>
```

#### 6. Logging

Disable logging: 

```xml
  <add key="logging-type" value="Empty" />
```

File logging: 

```xml
  <add key="logging-type" value="File" />
```

Email logging: 

```xml
  <add key="logging-type" value="Email" />
```

for email logging you should set right credantials in NLogEmail.config

## Links

* [Back-end, REST API documentaion](https://bandmap-api.azurewebsites.net/swagger/ui/index)

* [Bandmap (demo)](https://bandmap.azurewebsites.net)
