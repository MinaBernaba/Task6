using PostsProject.Data.Identity;

namespace Posts.Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public int? ExternalId { get; set; } // Nullable for user-created posts; set for synced posts from external APIs
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsPublic { get; set; } = false;
        public virtual User User { get; set; } = null!;
    }
}