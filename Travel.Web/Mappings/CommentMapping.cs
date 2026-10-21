using AutoMapper;
using Travel.Web.DTOs.CommentDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class CommentMapping : Profile
    {
        public CommentMapping()
        {
            CreateMap<Comment, ResultCommentDto>();
            CreateMap<CreateCommentDto, Comment>();
            
        }
    }
}
