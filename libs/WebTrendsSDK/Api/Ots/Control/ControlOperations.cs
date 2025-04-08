// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

namespace WebTrendsSDK.Api;

partial interface IOtsOperations
{
	/// <summary>
	/// Gets the /ots/api/rest-1.2/control operations.
	/// </summary>
	IControlOperations Control { get; }
}

partial class OtsOperations
{
	Lazy<IControlOperations> _control;
	public IControlOperations Control
		=> (_control ??= _client.Defer<IControlOperations>(
					c => new ControlOperations(_path + "/control", c))).Value;
}

public interface IControlOperations
{
	/// <summary>
	/// Gets a project by alias
	/// HTTP POST /ots/api/rest-1.2/control/{accountId}-{alias}
	/// </summary>
	/// <param name="projectAlias">The project alias.</param>
	/// <param name="websiteUrl">The website URL.</param>
	/// <param name="userAgent">The user agent.</param>
	/// <param name="state">The state identity, which controls which tests apply. Default is <see cref="State.Normal"/></param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The WebTrends response.</returns>
	Task<WebTrendsResponse<Project>> GetProjectAsync(
		string projectAlias,
		string? websiteUrl = null,
		string? userAgent = null,
		State? state = null,
		CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets all projects for the account.
	/// </summary>s
	/// <param name="websiteUrl">The website URL.</param>
	/// <param name="userAgent">The user agent.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>The WebTrends response.</returns>
	Task<WebTrendsResponse<Project[]>> GetProjectsAsync(
		string? websiteUrl = null,
		string? userAgent = null,
		State? state = State.Normal,
		CancellationToken cancellationToken = default);
}

public class ControlOperations(PathString path, ApiClient client) : IControlOperations
{
	readonly PathString _path = path;
	readonly ApiClient _client = client;

	public async Task<WebTrendsResponse<Project>> GetProjectAsync(
		string projectAlias,
		string? websiteUrl = null,
		string? userAgent = null,
		State? state = null,
		CancellationToken cancellationToken = default)
	{
		Ensure.IsNotNullOrEmpty(projectAlias, nameof(projectAlias));

		var operationPath = _path + $"/{_client.Settings.AccountId}-{projectAlias}";

		var request = new WebTrendsRequest<OtsRequest>(
			HttpMethod.Post,
			operationPath,
			CreateOtsRequest(websiteUrl, state),
			BuildQuery(_client.Settings),
			userAgent);

		return await _client.FetchSingleAsync<OtsRequest, Project, ProjectBody>(request, cancellationToken).ConfigureAwait(false);
	}

	public async Task<WebTrendsResponse<Project[]>> GetProjectsAsync(
		string? websiteUrl = null,
		string? userAgent = null,
		State? state = null,
		CancellationToken cancellationToken = default)
	{
		var operationPath = _path + $"/{_client.Settings.AccountId}";

		var request = new WebTrendsRequest<OtsRequest>(
			HttpMethod.Post,
			operationPath,
			CreateOtsRequest(websiteUrl, state),
			BuildQuery(_client.Settings),
			userAgent);

		return await _client.FetchManyAsync<OtsRequest, Project, ProjectBody>(request, cancellationToken).ConfigureAwait(false);
	}

	OtsRequest CreateOtsRequest(string? websiteUrl, State? state)
		=> new OtsRequest(
				websiteUrl is { Length: > 0 } ? websiteUrl : _client.Settings.WebsiteUrl,
				state.GetValueOrDefault(_client.Settings.State).ToString().ToLower());

	QueryString BuildQuery(WebTrendsSettings settings)
		=> new QueryStringBuilder()
				.AddParameter("debug", settings.Debug)
				.AddParameter("_wt.encrypted", settings.Encrypted)
				.AddParameter("_wt.track", settings.Track)
				.AddParameter("keyToken", settings.KeyToken)
				.Build();
}
