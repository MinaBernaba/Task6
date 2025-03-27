using Posts.Application.Features.Posts.Queries.Response;
using Posts.Data.Entities;

namespace Posts.Application.ServiceInterfaces
{
    public interface IPostService
    {
        Task<IEnumerable<GetAllApprovedPostsResponse>> GetAllApprovedPostsAsync();
        Task<IEnumerable<GetAllPostsResponse>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int postId);
        Task<GetPostInfoResponse> GetPostWithDetailsByIdAsync(int postId);
        Task<bool> IsPostExistByIdAsync(int postId);
        Task<bool> CreatePostAsync(Post post);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> DeletePostAsync(Post post);
    }
}
