using PostsProject.Infrastructure.Context;
using PostsProject.Infrastructure.Interfaces;
using Posts.Data.Entities;

namespace PostsProject.Infrastructure.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostService
    {
        public PostRepository(ApplicationDbContext context) : base(context) { }
    }
}
