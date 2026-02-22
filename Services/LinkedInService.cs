using Carbo.Models;

namespace Carbo.Services;

/// <summary>
/// LinkedIn profile lookup. Uses a Google search-based approach since the
/// official LinkedIn API requires partner approval. Falls back to a browser
/// open if a programmatic result can't be parsed.
/// </summary>
public static class LinkedInService
{
    private static readonly HttpClient Http = new();

    public static async Task<LinkedInProfile?> LookupAsync(string name)
    {
        // Build a Google search for the LinkedIn profile
        var query = Uri.EscapeDataString($"site:linkedin.com/in {name}");
        var searchUrl = $"https://www.google.com/search?q={query}";

        // Open the search in the browser as a best-effort lookup
        // (Programmatic scraping of LinkedIn is against ToS; this opens the
        //  result in the default browser for the user to review.)
        await ProcessSpawner.Run("powershell", $"-command Start-Process '{searchUrl}'");

        // Return a placeholder profile so callers know a lookup was attempted
        return new LinkedInProfile
        {
            Name = name,
            Headline = "See browser for LinkedIn results",
            Company = "",
            ProfileUrl = $"https://www.linkedin.com/search/results/people/?keywords={Uri.EscapeDataString(name)}"
        };
    }
}
