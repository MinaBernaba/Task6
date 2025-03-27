using AutoMapper;

namespace PostsProject.Application.Mapping.UserMapper
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {
            RegisterUser();
        }
    }
}
