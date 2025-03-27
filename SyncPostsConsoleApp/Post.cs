namespace SyncPostsConsoleApp
{
    public class Post
    {
        public int Id { get; set; }
        public int? ExternalId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsPublic { get; set; }
    }
}