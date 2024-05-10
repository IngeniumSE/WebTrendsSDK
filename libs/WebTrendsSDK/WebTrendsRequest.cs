// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

namespace WebTrendsSDK;

/// <summary>
/// Represents a request to a WebTrends API resource.
/// </summary>
/// <param name="method">The HTTP method.</param>
/// <param name="resource">The relative resource.</param>
/// <param name="query">The query string.</param>
/// <param name="userAgent">The user agent.</param>
public class WebTrendsRequest(
	HttpMethod method,
	PathString resource,
	QueryString? query = null,
	string? userAgent = null)
{
	/// <summary>
	/// Gets the HTTP method for the request.
	/// </summary>
	public HttpMethod Method => method;

	/// <summary>
	/// Gets the relative resource for the request.
	/// </summary>
	public PathString Resource => resource;

	/// <summary>
	/// Gets the query string.
	/// </summary>
	public QueryString? Query => query;

	/// <summary>
	/// Gets the user agent.
	/// </summary>
	public string? UserAgent => userAgent;
}

/// <summary>
/// Represents a request to a WebTrends API resource.
/// </summary>
/// <param name="method">The HTTP method.</param>
/// <param name="resource">The relative resource.</param>
/// <param name="data">The data.</param>
/// <param name="query">The query string.</param>
/// <param name="userAgent">The user agent.</param>
/// <typeparam name="TData">The data type.</typeparam>
public class WebTrendsRequest<TData>(
	HttpMethod method,
	PathString resource,
	TData data,
	QueryString? query = null,
	string? userAgent = null) : WebTrendsRequest(method, resource, query, userAgent)
	where TData : notnull
{
	/// <summary>
	/// Gets the model for the request.
	/// </summary>
	public TData Data => data;
}
