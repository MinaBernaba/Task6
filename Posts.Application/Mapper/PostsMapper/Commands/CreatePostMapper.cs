using Posts.Application.Features.Posts.Commands.Models;
using Posts.Data.Entities;

namespace Posts.Application.Mapper.PostsMapper
{
    public partial class PostMapper
    {
        public void CreatePostMapper() => CreateMap<CreatePostCommand, Post>();
    }
}
