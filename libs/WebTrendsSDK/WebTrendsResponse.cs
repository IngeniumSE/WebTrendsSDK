// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using System.Diagnostics;
using System.Net;
using System.Text;

using WebTrendsSDK.Api;

namespace WebTrendsSDK;

/// <summary>
/// Represents a WebTrends response.
/// </summary>
/// <param name="method">The HTTP method requested.</param>
/// <param name="uri">The URI requested.</param>
/// <param name="isSuccess">States whether the status code is a success HTTP status code.</param>
/// <param name="statusCode">The HTTP status code returned.</param>
/// <param name="meta">The paging metadata for the request, if available.</param>
/// <param name="rateLimiting">The rate limiting metadata for the request, if available.</param>
/// <param name="links">The set of links provided by the response.</param>
/// <param name="error">The API error, if available.</param>
[DebuggerDisplay("{ToDebuggerString(),nq}")]
public class WebTrendsResponse(
	HttpMethod method,
	Uri uri,
	bool isSuccess,
	HttpStatusCode statusCode,
	Error? error = default)
{
	/// <summary>
	/// Gets whether the status code represents a success HTTP status code.
	/// </summary>
	public bool IsSuccess => isSuccess;

	/// <summary>
	/// Gets the error.
	/// </summary>
	public Error? Error => error;

	/// <summary>
	/// Gets the HTTP status code of the response.
	/// </summary>
	public HttpStatusCode StatusCode => statusCode;

	/// <summary>
	/// Gets or sets the request HTTP method.
	/// </summary>
	public HttpMethod RequestMethod => method;

	/// <summary>
	/// Gets or sets the request URI.
	/// </summary>
	public Uri RequestUri => uri;

	/// <summary>
	/// Gets or sets the request content, when logging is enabled.
	/// </summary>
	public string? RequestContent { get; set; }

	/// <summary>
	/// Gets or sets the response content, when logging is enabled.
	/// </summary>
	public string? ResponseContent { get; set; }

	/// <summary>
	/// Provides a string representation for debugging.
	/// </summary>
	/// <returns></returns>
	public virtual string ToDebuggerString()
	{
		var builder = new StringBuilder();
		builder.Append($"{StatusCode}: {RequestMethod} {RequestUri.PathAndQuery}");
		if (Error is not null)
		{
			builder.Append($" - {Error.ErrorMessage}");
		}

		return builder.ToString();
	}
}

/// <summary>
/// Represents a WebTrends response with payload data.
/// </summary>
/// <param name="method">The HTTP method requested.</param>
/// <param name="uri">The URI requested.</param>
/// <param name="isSuccess">States whether the status code is a success HTTP status code.</param>
/// <param name="statusCode">The HTTP status code.</param>
/// <param name="data">The API response data, if available.</param>
/// <param name="meta">The paging metadata for the request, if available.</param>
/// <param name="rateLimiting">The rate limiting metadata for the request, if available.</param>
/// <param name="links">The set of links provided by the response.</param>
/// <param name="error">The API error, if available.</param>
/// <typeparam name="TData">The data type.</typeparam>
public class WebTrendsResponse<TData>(
	HttpMethod method,
	Uri uri,
	bool isSuccess,
	HttpStatusCode statusCode,
	TData? data = default,
	Error? error = default) : WebTrendsResponse(method, uri, isSuccess, statusCode, error)
	where TData : class
{
	/// <summary>
	/// Gets the response data.
	/// </summary>
	public TData? Data => data;

	/// <summary>
	/// Gets whether the response has data.
	/// </summary>
	public bool HasData => data is not null;

	public override string ToDebuggerString()
	{
		var builder = new StringBuilder();
		builder.Append($"{StatusCode}");
		if (HasData)
		{
			builder.Append($" ({Data!.GetType().Name})");
		}
		builder.Append($": {RequestMethod} {RequestUri.PathAndQuery}");
		if (Error is not null)
		{
			builder.Append($" - {Error.ErrorMessage}");
		}

		return builder.ToString();
	}
}
