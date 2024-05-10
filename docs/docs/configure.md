---
sidebar_position: 2
title: Configure
description: Details how to configure the WebTrendsSDK library within your project
tags: [Getting started, Configuration]
---

# Configure

The WebTrendsSDK targets .NET Standard 2.0 (`netstandard2.0`) which means you can install the SDK on any project that supports .NET Standard 2.0. You can view the [supported frameworks and platforms](https://www.nuget.org/packages/WebTrendsSDK#supportedframeworks-body-tab) on NuGet.org.

## .NET Core and .NET 5+

Apps written for the newer .NET platform can be configured to use the WebTrendsSDK through app configuration and service registration. 

### Configuring WebTrends settings
To configure the WebTrendsSDK, add the following to your `appsettings.json` file:

```json
{
  "WebTrends": {
    "AccountId": "{account_id}",
    "KeyToken": "{token}",
    "WebsiteUrl": "{website_url}
  }
}
```

The app configuration is mapped to the `WebTrendsSettings` type, which supports the following properties:

| Property | Type | Default Value | Notes |
| --- | --- | --- | --- |
| `AccountId` | `string` | `null` | Your account ID |
| `KeyToken` | `string` | `null` | Your authentication token |
| `OtsBaseUrl` | `string` | https://ots.webtrends-optimize.com | The base URL for the WebTrends OTS REST API |
| `CaptureRequestContent` | `bool` | `false` | If enabled, each request will capture the request content |
| `CaptureResponseContent` | `bool` | `false` | If enabled, each request will capture the response content |

### Service registration
To add WebTrendsSDK services to your Dependency Injection container, you need to add a call to `AddWebTrends()` to your `IServiceCollection`. There are a few _flavours_ of a .NET app, so you'll need to decide which one you are using

#### Using `WebApplication.CreateBuilder()`
This is the typical approach to modern ASP.NET Core apps, typically using Minimal APIs.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebTrends();
```

#### Using a Startup class
This is an approach used by ASP.NET Core web apps.

```csharp
public class Startup
{
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddWebTrends();
  }
}
```

#### Using `HostBuilder`
The `HostBuilder` approach targets non-ASP.NET Core scenarios, such as Windows services, or Function apps (Azure Functions, AWS Lambda, etc.) that require host lifetime management.

```csharp
var builder = new HostBuilder()
  .ConfigureServices(services => {
    servies.AddWebTrends();
  });
```

:::warning[Note on Lifetime]

The `AddWebTrends()` extension method registers a per-scope lifetime for the default API client. This is typical for ASP.NET Core apps where the lifetime scope is per-request. For non-ASP.NET Core hosts, you may need to manage the lifetime through a lifetime scope, otherwise you may up with a singleton instance of the client.

:::

## .NET Framework

### Vanilla .NET Framework

If you are targeting .NET Framework but will be managing the lifetime of the SDK services yourself, you can easily create an instance of a client and settings. See [Client Usage](/docs/client-usage) for more information.

### Integration through an Inversion of Control (IoC) container

If you are targeting .NET Framework, you may or may not be using a IoC container, such as Autofac, or StructureMap, etc. It is possible to wire up the WebTrendsSDK using the same strategy as the default for ASP.NET Core, however as there are numerous IOC containers available, we haven't documented them here. Below is an outline of the recommended service lifetime for the services provided by the SDK:

| Service | Implementation | Recommended Lifetime | Notes |
| --- | --- | --- | --- |
| `WebTrendsSettings` | `WebTrendsSettings` | `Singleton` | Pre-configure this instance. |
| `IWebTrendsHttpClientFactory` | `WebTrendsHttpClientFactory` | `Scoped` | This is for customising a `HttpClient` instance. |
| `IIsuuApiClientFactory` | `WebTrendsApiClientFactory` | `Scoped` | |
| `IIsuuApiClient` | `WebTrendsApiClient` | `Scoped` | This is the default instance when injected directly. |