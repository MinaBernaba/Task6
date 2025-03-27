using AutoMapper;

namespace Posts.Application.Mapper.PostsMapper
{
    public partial class PostMapper : Profile
    {
        public PostMapper()
        {
            CreatePostMapper();
            UpdatePostMapper();
        }
    }
}
