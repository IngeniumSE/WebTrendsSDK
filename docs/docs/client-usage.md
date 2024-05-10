---
sidebar_position: 3
tags: [HTTP Client, API Client, Dependency Injection]
---

# API Client Usage

There are a number of ways to create an API client for accessing the WebTrends API. The approach you take will depend on how your app works.

:::tip[Examples]

Examples on this page are using the C# default constructors feature for brevity. The approaches listed here will work with typical constructors.

:::

## Using Dependency Injection (DI)

You can use DI to inject a client into your app code. 

### Using the default client

The default client is pre-configured to use the configured `WebTrendsSettings`. The client will be disposed when the owning scope is disposed.

```csharp
public class MyService(IWebTrendsApiClient client)
{
  public async Task DoSomething()
  {
    // Do something with client.
  }
}
```

### Using the API client factory

It is possible to inject the API client factory and settings instead. You need to manage the lifetime of the client itself.

```csharp
public class MyService(IWebTrendsApiClientFactory clientFactory, WebTrendsSettings settings)
{
  public async Task DoSomething()
  {
    using var client = clientFactory.CreateApiClient(settings);

    // Do something with client.
  }
}
```

If you are not using the pre-configured `WebTrendsSettings`, you can provided your own:

```csharp
public class MyService(IWebTrendsApiClientFactory clientFactory)
{
  public async Task DoSomething()
  {
    var settings = new WebTrendsSettings();
    using var client = clientFactory.CreateApiClient(settings);

    // Do something with client.
  }
}
```

### Manually creating a client

You can manually create a client yourself, but you are responsible for managing its lifetime. The following are some examples.

#### Creating an API client manually

```csharp
var httpClient = new HttpClient();
var settings = new WebTrendsSettings();

var apiClient = new WebTrendsApiClient(httpClient, settings);
```

#### Creating an API client factory manually

```csharp
var httpClientFactory = new WebTrendsHttpClientFactory();
var apiClientFactory = new WebTrendsApiClientFactory(httpClientFactory);
var settings = new WebTrendsSettings();

var apiClient = apiClientFactory.CreateApiClient(settings);
```

### Managing the HTTP Client

If you need to control how the `HttpClient` is created, you can implement your own implementaton of `IWebTrendsHttpClientFactory`:

```csharp
public class MyWebTrendsHttpClientFactory : IWebTrendsHttpClientFactory
{
  public HttpClient CreateClient(string name)
  {
    return new HttpClient();
  }
}
```

If you are using the standard approach to dependency injection, you can register your own implementation, which will be used instead of the default implementation:

```csharp
services.AddWebTrends();
services.AddScoped<IWebTrendsHttpClientFactory, MyWebTrendsHttpClientFactory>();
```