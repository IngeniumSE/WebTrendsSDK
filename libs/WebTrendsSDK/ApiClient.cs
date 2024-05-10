// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Linq;

using WebTrendsSDK.Api;

namespace WebTrendsSDK;

/// <summary>
/// Defines the contract for an API client.
/// </summary>
public interface IApiClient
{
	/// <summary>
	/// Gets the settings for this API client.
	/// </summary>
	WebTrendsSettings Settings { get; }

	/// <summary>
	/// Sends the specified request to the WebTrends API.
	/// </summary>
	/// <param name="request">The WebTrends request.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The WebTrends response.</returns>
	Task<WebTrendsResponse> SendAsync(
		WebTrendsRequest request,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Sends the specified request to the WebTrends API.
	/// </summary>
	/// <param name="request">The WebTrends request.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The WebTrends response.</returns>
	Task<WebTrendsResponse> SendAsync<TData>(
		WebTrendsRequest<TData> request,
		CancellationToken cancellationToken = default)
		where TData : notnull;

	/// <summary>
	/// Sends the specified request to the WebTrends API and returns the response data.
	/// </summary>
	/// <param name="request">The WebTrends request.</param>
	/// <param name="responseFactory">The response data factory.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The WebTrends response.</returns>
	Task<WebTrendsResponse<TData>> FetchSingleAsync<TData, TBody>(
		WebTrendsRequest request,
		CancellationToken cancellationToken = default)
		where TData : OtsResponse<TBody>
		where TBody : Body;

	/// <summary>
	/// Sends the specified request to the WebTrends API and returns the response data.
	/// </summary>
	/// <param name="request">The WebTrends request.</param>
	/// <param name="responseFactory">The response data factory.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The WebTrends response.</returns>
	Task<WebTrendsResponse<TResponseData[]>> FetchManyAsync<TRequestData, TResponseData, TResponseBody>(
		WebTrendsRequest<TRequestData> request,
		CancellationToken cancellationToken = default)
		where TRequestData : notnull
		where TResponseData : OtsResponse<TResponseBody>
		where TResponseBody : Body;
}

/// <summary>
/// Provides a base implementation of an API client.
/// </summary>
public abstract class ApiClient : IApiClient
{
	readonly HttpClient _http;
	readonly WebTrendsSettings _settings;
	readonly JsonSerializerOptions _serializerOptions = JsonUtility.CreateSerializerOptions();
	readonly JsonSerializerOptions _deserializerOptions = JsonUtility.CreateDeserializerOptions();
	readonly Uri _baseUrl;

	protected ApiClient(HttpClient http, WebTrendsSettings settings)
	{
		_http = Ensure.IsNotNull(http, nameof(http));
		_settings = Ensure.IsNotNull(settings, nameof(settings));
		_settings.Validate();

		_baseUrl = new Uri(settings.OtsBaseUrl);
	}

	public WebTrendsSettings Settings => _settings;

	#region Send and Fetch

	#region Send
	public async Task<WebTrendsResponse> SendAsync(
		WebTrendsRequest request,
		CancellationToken cancellationToken = default)
	{
		Ensure.IsNotNull(request, nameof(request));
		using var httpReq = CreateHttpRequest(request);
		HttpResponseMessage? httpResp = null;

		string? requestContent = await CaptureRequestContent(httpReq).ConfigureAwait(false);
		try
		{
			httpResp = await _http.SendAsync(httpReq, cancellationToken)
				.ConfigureAwait(false);

			var transformedResponse = await TransformResponse(
				httpReq.Method,
				httpReq.RequestUri,
				httpResp)
				.ConfigureAwait(false);

			transformedResponse.RequestContent = requestContent;
			transformedResponse.ResponseContent = await CaptureResponseContent(httpResp).ConfigureAwait(false);

			return transformedResponse;
		}
		catch (Exception ex)
		{
			return await GetWebTrendsErrorResponse(httpReq, requestContent, httpResp, ex).ConfigureAwait(false);
		}
		finally
		{
			httpReq?.Dispose();
		}
	}

	public async Task<WebTrendsResponse> SendAsync<TRequest>(
		WebTrendsRequest<TRequest> request,
		CancellationToken cancellationToken = default)
		where TRequest : notnull
	{
		Ensure.IsNotNull(request, nameof(request));
		using var httpReq = CreateHttpRequest(request);
		HttpResponseMessage? httpResp = null;

		string? requestContent = await CaptureRequestContent(httpReq).ConfigureAwait(false);
		try
		{
			httpResp = await _http.SendAsync(httpReq, cancellationToken).ConfigureAwait(false);

			var transformedResponse = await TransformResponse(
				httpReq.Method,
				httpReq.RequestUri,
				httpResp)
				.ConfigureAwait(false);

			transformedResponse.RequestContent = requestContent;
			transformedResponse.ResponseContent = await CaptureResponseContent(httpResp).ConfigureAwait(false);

			return transformedResponse;
		}
		catch (Exception ex)
		{
			return await GetWebTrendsErrorResponse(httpReq, requestContent, httpResp, ex).ConfigureAwait(false);
		}
		finally
		{
			httpReq?.Dispose();
		}
	}
	#endregion

	#region Fetch
	public async Task<WebTrendsResponse<TResponseData>> FetchSingleAsync<TResponseData, TResponseBody>(
		WebTrendsRequest request,
		CancellationToken cancellationToken = default)
		where TResponseData : OtsResponse<TResponseBody>
		where TResponseBody : Body
	{
		Ensure.IsNotNull(request, nameof(request));
		using var httpReq = CreateHttpRequest(request);
		HttpResponseMessage? httpResp = null;

		string? requestContent = await CaptureRequestContent(httpReq).ConfigureAwait(false);
		try
		{
			httpResp = await _http.SendAsync(httpReq, cancellationToken)
				.ConfigureAwait(false);

			var transformedResponse = await TransformSingleResponse<TResponseData, TResponseBody>(
				httpReq.Method,
				httpReq.RequestUri,
				httpResp,
				cancellationToken)
				.ConfigureAwait(false);

			transformedResponse.RequestContent = requestContent;
			transformedResponse.ResponseContent = await CaptureResponseContent(httpResp).ConfigureAwait(false);

			return transformedResponse;
		}
		catch (Exception ex)
		{
			return await GetWebTrendsErrorResponse<TResponseData>(httpReq, requestContent, httpResp, ex).ConfigureAwait(false);
		}
		finally
		{
			httpReq?.Dispose();
		}
	}

	public async Task<WebTrendsResponse<TResponseData>> FetchSingleAsync<TRequestData, TResponseData, TResponseBody>(
		WebTrendsRequest<TRequestData> request,
		CancellationToken cancellationToken = default)
		where TRequestData : notnull
		where TResponseData : OtsResponse<TResponseBody>
		where TResponseBody : Body
	{
		Ensure.IsNotNull(request, nameof(request));
		using var httpReq = CreateHttpRequest<TRequestData>(request);
		HttpResponseMessage? httpResp = null;

		string? requestContent = await CaptureRequestContent(httpReq).ConfigureAwait(false);
		try
		{
			httpResp = await _http.SendAsync(httpReq, cancellationToken)
				.ConfigureAwait(false);

			var transformedResponse = await TransformSingleResponse<TResponseData, TResponseBody>(
				httpReq.Method,
				httpReq.RequestUri,
				httpResp,
				cancellationToken)
				.ConfigureAwait(false);

			transformedResponse.RequestContent = requestContent;
			transformedResponse.ResponseContent = await CaptureResponseContent(httpResp).ConfigureAwait(false);

			return transformedResponse;
		}
		catch (Exception ex)
		{
			return await GetWebTrendsErrorResponse<TResponseData>(httpReq, requestContent, httpResp, ex).ConfigureAwait(false);
		}
		finally
		{
			httpReq?.Dispose();
		}
	}

	public async Task<WebTrendsResponse<TResponseData[]>> FetchManyAsync<TResponseData, TResponseBody>(
		WebTrendsRequest request,
		CancellationToken cancellationToken = default)
		where TResponseData : OtsResponse<TResponseBody>
		where TResponseBody : Body
	{
		Ensure.IsNotNull(request, nameof(request));
		using var httpReq = CreateHttpRequest(request);
		HttpResponseMessage? httpResp = null;

		string? requestContent = await CaptureRequestContent(httpReq).ConfigureAwait(false);
		try
		{
			httpResp = await _http.SendAsync(httpReq, cancellationToken)
				.ConfigureAwait(false);

			var transformedResponse = await TransformManyResponse<TResponseData, TResponseBody>(
				httpReq.Method,
				httpReq.RequestUri,
				httpResp,
				cancellationToken)
				.ConfigureAwait(false);

			transformedResponse.RequestContent = requestContent;
			transformedResponse.ResponseContent = await CaptureResponseContent(httpResp).ConfigureAwait(false);

			return transformedResponse;
		}
		catch (Exception ex)
		{
			return await GetWebTrendsErrorResponse<TResponseData[]>(httpReq, requestContent, httpResp, ex).ConfigureAwait(false);
		}
		finally
		{
			httpReq?.Dispose();
		}
	}

	public async Task<WebTrendsResponse<TResponseData[]>> FetchManyAsync<TRequestData, TResponseData, TResponseBody>(
		WebTrendsRequest<TRequestData> request,
		CancellationToken cancellationToken = default)
		where TRequestData : notnull
		where TResponseData : OtsResponse<TResponseBody>
		where TResponseBody : Body
	{
		Ensure.IsNotNull(request, nameof(request));
		using var httpReq = CreateHttpRequest(request);
		HttpResponseMessage? httpResp = null;

		string? requestContent = await CaptureRequestContent(httpReq).ConfigureAwait(false);
		try
		{
			httpResp = await _http.SendAsync(httpReq, cancellationToken)
				.ConfigureAwait(false);

			var transformedResponse = await TransformManyResponse<TResponseData, TResponseBody>(
				httpReq.Method,
				httpReq.RequestUri,
				httpResp,
				cancellationToken)
				.ConfigureAwait(false);

			transformedResponse.RequestContent = requestContent;
			transformedResponse.ResponseContent = await CaptureResponseContent(httpResp).ConfigureAwait(false);

			return transformedResponse;
		}
		catch (Exception ex)
		{
			return await GetWebTrendsErrorResponse<TResponseData[]>(httpReq, requestContent, httpResp, ex).ConfigureAwait(false);
		}
		finally
		{
			httpReq?.Dispose();
		}
	}
	#endregion

	#endregion

	#region Preprocessing
	protected internal HttpRequestMessage CreateHttpRequest(
		WebTrendsRequest request)
	{
		string pathAndQuery = request.Resource.ToUriComponent();
		var query = request.Query;

		if (query.HasValue)
		{
			pathAndQuery += query.Value.ToUriComponent();
		}
		var uri = new Uri(_baseUrl, CombineRelativePaths(_baseUrl.PathAndQuery, pathAndQuery));

		var message = new HttpRequestMessage(request.Method, uri);

		if (request.UserAgent is { Length: >0 })
		{
			message.Headers.TryAddWithoutValidation("User-Agent", request.UserAgent);
		}

		return message;
	}

	protected internal HttpRequestMessage CreateHttpRequest<TRequest>(
		WebTrendsRequest<TRequest> request)
		where TRequest : notnull
	{
		var message = CreateHttpRequest((WebTrendsRequest)request);

		message.Content = JsonContent.Create(
			inputValue: request.Data, options: _serializerOptions);

		return message;
	}
	#endregion

	#region Postprocessing
	protected internal async Task<WebTrendsResponse> TransformResponse(
		HttpMethod method,
		Uri uri,
		HttpResponseMessage response,
		CancellationToken cancellationToken = default)
	{
		if (response.IsSuccessStatusCode)
		{
			return new WebTrendsResponse(
				method,
				uri,
				response.IsSuccessStatusCode,
				response.StatusCode
			);
		}
		else
		{
			Error? error = await GetError(response).ConfigureAwait(false);

			return new WebTrendsResponse(
				method,
				uri,
				response.IsSuccessStatusCode,
				response.StatusCode,
				error: error
			);
		}
	}

	protected internal async Task<WebTrendsResponse<TData>> TransformSingleResponse<TData, TBody>(
		HttpMethod method,
		Uri uri,
		HttpResponseMessage response,
		CancellationToken cancellationToken = default)
		where TData : OtsResponse<TBody>
		where TBody : Body
	{
		if (response.IsSuccessStatusCode)
		{
			TData? data = null;
			Error? error = null;
			if (response.Content is not null)
			{
				data = await response.Content.ReadFromJsonAsync<TData>(_deserializerOptions)
					.ConfigureAwait(false);

				error = data?.Body?.Error;
			}

			return new WebTrendsResponse<TData>(
				method,
				uri,
				error is null,
				response.StatusCode,
				data: data,
				error: error
			);
		}
		else
		{
			Error? error = await GetError(response).ConfigureAwait(false);

			return new WebTrendsResponse<TData>(
				method,
				uri,
				false,
				response.StatusCode,
				error: error
			);
		}
	}

	protected internal async Task<WebTrendsResponse<TData[]>> TransformManyResponse<TData, TBody>(
		HttpMethod method,
		Uri uri,
		HttpResponseMessage response,
		CancellationToken cancellationToken = default)
		where TData : OtsResponse<TBody>
		where TBody : Body
	{
		if (response.IsSuccessStatusCode)
		{
			TData[]? data = null;
			Error? error = null;
			if (response.Content is not null)
			{
				data = await response.Content.ReadFromJsonAsync<TData[]>(_serializerOptions)
					.ConfigureAwait(false);

				if (data is {  Length: >0 })
				{
					error = data[0]?.Body.Error;
				}
			}

			return new WebTrendsResponse<TData[]>(
				method,
				uri,
				error is null,
				response.StatusCode,
				data: data ?? Array.Empty<TData>(),
				error: error
			);
		}
		else
		{
			Error? error = await GetError(response).ConfigureAwait(false);

			return new WebTrendsResponse<TData[]>(
				method,
				uri,
				false,
				response.StatusCode,
				error: error
			);
		}
	}

	async Task<string?> CaptureRequestContent(HttpRequestMessage httpReq)
	{
		if (_settings.CaptureRequestContent && httpReq.Content is not null)
		{
			var request = await httpReq.Content.ReadAsStringAsync()
				.ConfigureAwait(false);

			return request;
		}

		return null;
	}

	async Task<string?> CaptureResponseContent(HttpResponseMessage httpResp)
	{
		if (_settings.CaptureResponseContent && httpResp.Content is not null)
		{
			var request = await httpResp.Content.ReadAsStringAsync()
				.ConfigureAwait(false);

			return request;
		}

		return null;
	}

	async Task<Error?> GetError(HttpResponseMessage response)
	{
		if (response.Content is null)
		{
			return null;
		}

		var content = await response.Content.ReadFromJsonAsync<OtsResponse>()
			.ConfigureAwait(false);

		return content?.Body?.Error;
	}

	string? GetHeader(string name, HttpHeaders headers)
		=> headers.TryGetValues(name, out var values)
		? values.First()
		: null;
	#endregion

	protected internal Lazy<TOperations> Defer<TOperations>(Func<ApiClient, TOperations> factory)
		=> new Lazy<TOperations>(() => factory(this));

	protected internal Uri Root(string resource)
		=> new Uri(resource, UriKind.Relative);

	string CombineRelativePaths(string parent, string child)
	{
		if (parent is { Length: > 0 } && child is { Length: > 0 })
		{
			return $"{parent.TrimEnd('/')}/{child.TrimStart('/')}";
		}
		else
		{
			return parent + child;
		}
	}

	async Task<WebTrendsResponse> GetWebTrendsErrorResponse(
		HttpRequestMessage httpReq,
		string? requestContent,
		HttpResponseMessage? httpResp,
		Exception exception)
	{
		var response = new WebTrendsResponse(
			httpReq.Method,
			httpReq.RequestUri,
			false,
			(HttpStatusCode)0,
			error: new Error
			{
				ErrorMessage = exception.Message,
				Exception = exception
			});

		response.RequestContent = requestContent;

		if (httpResp?.Content is not null)
		{
			response.ResponseContent = await httpResp.Content.ReadAsStringAsync()
				.ConfigureAwait(false); ;
		}

		return response;
	}

	async Task<WebTrendsResponse<TData>> GetWebTrendsErrorResponse<TData>(
		HttpRequestMessage httpReq,
		string? requestContent,
		HttpResponseMessage? httpResp,
		Exception exception)
		where TData : class
	{
		var response = new WebTrendsResponse<TData>(
			httpReq.Method,
			httpReq.RequestUri,
			false,
			(HttpStatusCode)0,
			error: new Error
			{
				ErrorMessage = exception.Message,
				Exception = exception
			});

		response.RequestContent = requestContent;

		if (httpResp?.Content is not null)
		{
			response.ResponseContent = await httpResp.Content.ReadAsStringAsync()
				.ConfigureAwait(false); ;
		}

		return response;
	}
}
