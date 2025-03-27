using SyncPostsConsoleApp;
using System.CommandLine;
using System.Net.Http.Json;

class Program
{
    static async Task<int> Main(string[] args)
    {
        // Create a root command with a description.
        var rootCommand = new RootCommand("A command line tool to sync posts from JSONPlaceholder API.");

        // Create the sync command.
        var syncCommand = new Command("sync", "Fetch and sync posts from JSONPlaceholder.");

        // Set the handler for the sync command using SetHandler.
        syncCommand.SetHandler(SyncPostsAsync);

        // Add the sync command to the root command.
        rootCommand.AddCommand(syncCommand);

        // Parse the incoming args and invoke the handler.
        return await rootCommand.InvokeAsync(args);
    }

    static async Task SyncPostsAsync()
    {
        using var context = new ApplicationDbContext();

        // Fetch data from external API
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
        var externalPosts = await response.Content.ReadFromJsonAsync<List<ExternalPost>>();

        // Fetch all existing ExternalIds in one query to prevent duplicates
        var existingExternalIds = context.Posts
            .Where(p => p.ExternalId != null)
            .Select(p => p.ExternalId)
            .ToHashSet();

        // Fetch all existing UserIds in one query for data integrity in data base
        var existingUserIds = context.Users
            .Select(u => u.Id)
            .ToHashSet();

        // Filter out duplicates and posts with invalid UserIds
        var newPosts = externalPosts!
            .Where(ep => !existingExternalIds.Contains(ep.Id)) // Exclude duplicates
            .Where(ep => existingUserIds.Contains(ep.UserId))     // Exclude posts with non-existent UserIds
            .Select(ep => new Post
            {
                Title = ep.Title,
                Body = ep.Body,
                UserId = ep.UserId, // Use the original UserId since it’s valid
                ExternalId = ep.Id,
                IsPublic = true
            })
            .ToList();

        // Add new posts in one batch
        if (newPosts.Count != 0)
        {
            context.Posts.AddRange(newPosts);
            await context.SaveChangesAsync();
            Console.WriteLine($"Inserted {newPosts.Count} new posts.");
        }
        else
            Console.WriteLine("No new posts to insert.");

        Console.WriteLine("Sync completed.");
    }
}
