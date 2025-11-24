using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Models;
using AutoMapper;

namespace Sistema_Web_de_Nominas.MappingProfiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Usuario, RespuestaLoginDto>()
                .ForMember(dest => dest.TokenAcceso, opt => opt.Ignore())
                .ForMember(dest => dest.TokenActualizacion, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiraEn, opt => opt.Ignore());

            CreateMap<PeticionRegistroDto, Usuario>()
                .ForMember(dest => dest.Contra, opt => opt.Ignore())
                .ForMember(dest => dest.CorreoConfirmacion, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CorreoConfirmacionToken, opt => opt.Ignore())
                .ForMember(dest => dest.RecargaToken, opt => opt.Ignore())
                .ForMember(dest => dest.RecargaTokenExpirado, opt => opt.Ignore())
                .ForMember(dest => dest.ContraRecargaToken, opt => opt.Ignore())
                .ForMember(dest => dest.ReincioTokenExpirado, opt => opt.Ignore())
                .ForMember(dest => dest.Rol, opt => opt.Ignore());
        }
    }
}
