namespace Posts.Application.Features.Posts.Queries.Response
{
    public class GetPostInfoResponse
    {
        public int PostId { get; set; }
        public string Author { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
    }
}
