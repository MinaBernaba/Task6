using PostsProject.Application.Features.Authentication.Models;
using PostsProject.Data.Identity;

namespace PostsProject.Application.Mapping.UserMapper
{
    public partial class UserProfile
    {
        public void RegisterUser() => CreateMap<RegisterUserCommand, User>();
    }
}
