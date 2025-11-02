using System.Text.Json.Serialization;

namespace GitHub.Integration.Api;

public sealed class GitHubRepository
{
    [JsonPropertyName("id")]
    public  long Id { get; init; }

    [JsonPropertyName("node_id")]
    public  string NodeId { get; init; }

    [JsonPropertyName("name")]
    public  string Name { get; init; }

    [JsonPropertyName("full_name")]
    public  string FullName { get; init; }

    [JsonPropertyName("private")]
    public  bool Private { get; init; }

    [JsonPropertyName("owner")]
    public  GitHubUser Owner { get; init; }

    [JsonPropertyName("html_url")]
    public  string HtmlUrl { get; init; }

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("fork")]
    public  bool Fork { get; init; }

    [JsonPropertyName("url")]
    public  string Url { get; init; }

    [JsonPropertyName("created_at")]
    public  DateTime CreatedAt { get; init; }

    [JsonPropertyName("updated_at")]
    public  DateTime UpdatedAt { get; init; }

    [JsonPropertyName("pushed_at")]
    public  DateTime PushedAt { get; init; }

    [JsonPropertyName("homepage")]
    public string? Homepage { get; init; }

    [JsonPropertyName("size")]
    public  int Size { get; init; }

    [JsonPropertyName("stargazers_count")]
    public  int StargazersCount { get; init; }

    [JsonPropertyName("watchers_count")]
    public  int WatchersCount { get; init; }

    [JsonPropertyName("language")]
    public string? Language { get; init; }

    [JsonPropertyName("has_issues")]
    public  bool HasIssues { get; init; }

    [JsonPropertyName("has_projects")]
    public  bool HasProjects { get; init; }

    [JsonPropertyName("has_downloads")]
    public  bool HasDownloads { get; init; }

    [JsonPropertyName("has_wiki")]
    public  bool HasWiki { get; init; }

    [JsonPropertyName("has_pages")]
    public  bool HasPages { get; init; }

    [JsonPropertyName("has_discussions")]
    public  bool HasDiscussions { get; init; }

    [JsonPropertyName("forks_count")]
    public  int ForksCount { get; init; }

    [JsonPropertyName("archived")]
    public  bool Archived { get; init; }

    [JsonPropertyName("disabled")]
    public  bool Disabled { get; init; }

    [JsonPropertyName("open_issues_count")]
    public  int OpenIssuesCount { get; init; }

    [JsonPropertyName("license")]
    public object? License { get; init; }

    [JsonPropertyName("allow_forking")]
    public  bool AllowForking { get; init; }

    [JsonPropertyName("is_template")]
    public  bool IsTemplate { get; init; }

    [JsonPropertyName("web_commit_signoff_")]
    public  bool WebCommitSignoff { get; init; }

    [JsonPropertyName("topics")]
    public string[]? Topics { get; init; }

    [JsonPropertyName("visibility")]
    public  string Visibility { get; init; }

    [JsonPropertyName("forks")]
    public  int Forks { get; init; }

    [JsonPropertyName("open_issues")]
    public  int OpenIssues { get; init; }

    [JsonPropertyName("watchers")]
    public  int Watchers { get; init; }

    [JsonPropertyName("default_branch")]
    public  string DefaultBranch { get; init; }

    [JsonPropertyName("permissions")]
    public object? Permissions { get; init; }
}
