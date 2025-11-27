using AutoMapper;
using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.MappingProfiles
{
    public class EmpleadoProfile : Profile
    {
        public EmpleadoProfile()
        {
            CreateMap<Empleado, EmpleadoResponseDTO>();

            CreateMap<EmpleadoRequestDTO, Empleado>().ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId));
        }
    }
}