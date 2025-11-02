namespace GitHub.Integration.Api;

public class GitHubClient(HttpClient httpClient)
{
    public async Task<GitHubUser?> GetUser(string apiKey)
    {
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        try
        {
            var response = await httpClient.GetAsync("user");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<GitHubUser>();

            return content;
        }
        catch (Exception ex)
        {
            // Log error...
            return null;
        }
    }

    public async Task<List<GitHubRepository>?> GetRepositories(string apiKey)
    {
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        try
        {
            var response = await httpClient.GetAsync(
                "user/repos?per_page=100&sort=created&direction=desc");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<List<GitHubRepository>>();

            return content;
        }
        catch (Exception ex)
        {
            // Log error...
            return null;
        }
    }
}
