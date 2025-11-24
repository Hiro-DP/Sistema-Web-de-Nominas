using AutoMapper;
using Sistema_Web_de_Nominas.Dto;
using Sistema_Web_de_Nominas.Models;

namespace Sistema_Web_de_Nominas.MappingProfiles
{
    public class NominaProfile: Profile
    {
        public NominaProfile() 
        {
            CreateMap<Nomina, NominaResponseDTO>();

            CreateMap<NominaRequestDTO, Nomina>()
               
                .ForMember(dest => dest.CodigoId, opt => opt.Ignore())
                .ForMember(dest => dest.Empleado, opt => opt.Ignore())
                .ForMember(dest => dest.MontoDeHorasExtras, opt => opt.Ignore())
                .ForMember(dest => dest.Devengado, opt => opt.Ignore())
                .ForMember(dest => dest.INSSLaboral, opt => opt.Ignore())
                .ForMember(dest => dest.MontoDeducciones, opt => opt.Ignore())
                .ForMember(dest => dest.SalarioNeto, opt => opt.Ignore());
        }
    }
}
