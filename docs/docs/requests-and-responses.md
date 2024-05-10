---
sidebar_position: 4
tags: [Request, Response, API Client, Debugging]
---

# Requests and Responses

While the WebTrendsSDK provides standard operations supported by the WebTrends API, it is possible to craft your own requests if the WebTrendsSDK does not currently support an operation you need.

## Requests

The WebTrendsSDK provides two request types, `WebTrendsRequest` and `WebTrendsRequest<TData>`. These are defined here: https://github.com/IngeniumSE/WebTrendsSDK/blob/main/libs/WebTrendsSDK/WebTrendsRequest.cs

The latter `WebTrendsRequest<TData>` inherits from `WebTrendsRequest` but is tailored towards sending payload data to the WebTrends API.

### Developer-friendly Debugging

The primary consumer of this SDK are developers, so the WebTrendsSDK has been designed to give as much information as possible to developers to aid in understanding any integration issues.

#### Request and Response Content

The `WebTrendsResponse` and `WebTrendsResponse<TData>` type contains useful properties for debugging requests and responses.

| Property | Type | Notes |
|---|---|---|
| `StatusCode` | `HttpStatusCode` | The HTTP status code of the response |
| `RequestMethod` | `HttpMethod` | The HTTP method used |
| `RequestUri` | `Uri` | The requested URI resource |
| `RequestContent` | `string?` | The captured HTTP request content |
| `ResponseContent` | `string?` | The captured HTTP response content |

By default, the `RequestContent` and `ResponseContent` properties will not be populated, these have to be enabled in your app configuration:

```json
{
  "WebTrends": {
    "CaptureRequestContent": true,
    "CaptureResponseContent": true
  }
}
```

Alternatively, you can explicitly enable them through `WebTrendsSettings`:

```
var settings = new WebTrendsSettings
{
    CaptureRequestContent = true,
    CaptureResponseContent = true
};
```