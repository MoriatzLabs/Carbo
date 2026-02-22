using Carbo.Models;

namespace Carbo.Services;

/// <summary>
/// LinkedIn profile lookup. Uses a Google search-based approach since the
/// official LinkedIn API requires partner approval. Opens results in the
/// default browser for the user to review.
/// </summary>
public static class LinkedInService
{
    public static async Task<LinkedInProfile?> LookupAsync(string name)
    {
        // Uri.EscapeDataString ensures the name is safe for use in a URL
        var query = Uri.EscapeDataString($"site:linkedin.com/in {name}");
        var searchUrl = $"https://www.google.com/search?q={query}";

        try
        {
            await ProcessSpawner.Run(
                "powershell",
                $"-command Start-Process '{searchUrl}'",
                TimeSpan.FromSeconds(10));
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Could not open browser for LinkedIn lookup: {ex.Message}", ex);
        }

        return new LinkedInProfile
        {
            Name = name,
            Headline = "See browser for LinkedIn results",
            Company = string.Empty,
            ProfileUrl = $"https://www.linkedin.com/search/results/people/?keywords={Uri.EscapeDataString(name)}"
        };
    }
}
