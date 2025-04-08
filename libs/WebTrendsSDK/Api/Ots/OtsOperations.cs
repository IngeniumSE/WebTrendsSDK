// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

namespace WebTrendsSDK.Api;

partial interface IWebTrendsApiClient
{
	/// <summary>
	/// Gets the /draft operations.
	/// </summary>s
	public IOtsOperations Ots { get; }
}

partial class WebTrendsApiClient
{
	Lazy<IOtsOperations>? _ots;
	public IOtsOperations Ots
	{
		get
		{
			return (_ots ??= Defer<IOtsOperations>(c => new OtsOperations(new("/ots/api/rest-1.2"), c))).Value;
		}
	}
}

/// <summary>
/// Providers operations for the /ots/api/rest-1.2 endpoint.
/// </summary>
public partial interface IOtsOperations
{

}

public partial class OtsOperations(PathString path, ApiClient client) : IOtsOperations
{
	readonly PathString _path = path;
	readonly ApiClient _client = client;
}
