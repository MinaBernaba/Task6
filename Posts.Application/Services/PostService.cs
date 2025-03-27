using Microsoft.EntityFrameworkCore;
using Posts.Application.Features.Posts.Queries.Response;
using Posts.Application.ServiceInterfaces;
using Posts.Data.Entities;
using PostsProject.Infrastructure.Interfaces;

namespace Posts.Application.Services
{
    public class PostService(PostsProject.Infrastructure.Interfaces.IPostService _postRepository) : ServiceInterfaces.IPostService
    {
        public async Task<IEnumerable<GetAllApprovedPostsResponse>> GetAllApprovedPostsAsync()
        => await _postRepository.GetAllNoTracking()
                .Where(p => p.IsPublic)
                .Select(p => new GetAllApprovedPostsResponse
                {
                    PostId = p.Id,
                    Author = p.User.UserName!,
                    Body = p.Body,
                    Title = p.Title
                }).ToListAsync();
        public async Task<IEnumerable<GetAllPostsResponse>> GetAllPostsAsync()
        => await _postRepository.GetAllNoTracking()
                .Select(p => new GetAllPostsResponse
                {
                    PostId = p.Id,
                    Author = p.User.UserName!,
                    Body = p.Body,
                    Title = p.Title,
                    IsPublic = p.IsPublic
                }).ToListAsync();

        public async Task<bool> IsPostExistByIdAsync(int postId) => await _postRepository.IsExistAsync(p => p.Id == postId);
        public async Task<Post> GetPostByIdAsync(int postId) => await _postRepository.GetByIdAsync(postId);
        public async Task<GetPostInfoResponse> GetPostWithDetailsByIdAsync(int postId) =>
            await _postRepository.GetAllNoTracking()
                .Where(p => p.Id == postId)
                .Select(p => new GetPostInfoResponse
                {
                    PostId = p.Id,
                    Author = p.User.UserName!,
                    Body = p.Body,
                    Title = p.Title
                }).FirstAsync();
        public async Task<bool> CreatePostAsync(Post post) => await _postRepository.AddAsync(post);
        public async Task<bool> UpdatePostAsync(Post post) => await _postRepository.UpdateAsync(post);
        public async Task<bool> DeletePostAsync(Post post) => await _postRepository.DeleteAsync(post);
    }
}
