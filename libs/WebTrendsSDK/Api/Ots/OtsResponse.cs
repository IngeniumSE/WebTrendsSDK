// This work is licensed under the terms of the MIT license.
// For a copy, see <https://opensource.org/licenses/MIT>.

using System.Text.Json.Serialization;

namespace WebTrendsSDK.Api;

public class OtsResponse : OtsResponse<Body>
{
}

public class OtsResponse<TBody> where TBody: Body
{
	[JsonPropertyName("body")]
	public TBody Body { get; set; } = default!;

	[JsonPropertyName("metadata")]
	public string? Metadata { get; set; }

	[JsonPropertyName("params")]
	public Parameters? Parameters { get; set; }
}

public class Body
{
	[JsonPropertyName("cookies")]
	public Dictionary<string, Cookie>? Cookies { get; set; }

	[JsonPropertyName("message")]
	public Error? Error { get; set; }
}

public class Error
{
	[JsonPropertyName("errorCode")]
	public int ErrorCode { get; set; }

	[JsonPropertyName("errorMessage")]
	public string ErrorMessage { get; set; } = default!;

	[JsonIgnore]
	public Exception? Exception { get; set; }
}

public class Cookie
{
	[JsonPropertyName("timeout")]
	public long Timeout { get; set; }

	[JsonPropertyName("type")]
	public CookieType Type { get; set; }

	[JsonPropertyName("value")]
	public string Value { get; set; } = default!;
}

public enum CookieType
{
	Persisted,
	Session
}

public class Parameters
{
	[JsonPropertyName("_wt_sessionID")]
	public string? SessionId { get; set; }

	[JsonPropertyName("cookieDomain")]
	public string? CookieDomain { get; set; }

	[JsonPropertyName("guid")]
	public string? Guid { get; set; }

	[JsonPropertyName("r_experimentID")]
	public long ExperimentId { get; set; }

	[JsonPropertyName("r_paused")]
	public string? Paused { get; set; }

	[JsonPropertyName("r_runID")]
	public long RunId { get; set; }

	[JsonPropertyName("r_runState")]
	public string? RunState { get; set; }

	[JsonPropertyName("r_testID")]
	public long TestId { get; set; }

	[JsonPropertyName("r_type")]
	public string? Type { get; set; }

	[JsonPropertyName("systemUID")]
	public string? SystemUid { get; set; }

	[JsonPropertyName("testAlias")]
	public string? TestAlias { get; set; }

	[JsonPropertyName("trackingGuid")]
	public string? TrackingGuid { get; set; }
}
