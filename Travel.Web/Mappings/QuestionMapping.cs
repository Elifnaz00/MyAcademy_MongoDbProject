using AutoMapper;
using Travel.Web.DTOs.QıuestionDto;
using Travel.Web.DTOs.QıuestionDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class QuestionMapping : Profile
    {
        public QuestionMapping() {
             CreateMap<Question, ResultQuestionDto>();
             CreateMap<CreateQuestionDto, Question>();
             CreateMap<Question, ResultQuestionDto>();
        }
    }
}
