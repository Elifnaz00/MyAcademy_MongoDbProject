using AutoMapper;
using Travel.Web.DTOs.ReservationDtos;
using Travel.Web.Entities;

namespace Travel.Web.Mappings
{
    public class ReservationMapping : Profile
    {
        public ReservationMapping() {

            CreateMap<ResultReservationDto, Reservation>();
            CreateMap<Reservation, ResultReservationDto >();
            CreateMap<CreateReservationDto, Reservation >();
            CreateMap<Reservation, RecentBookingsDto>();
        
        }
    }
}
