using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Entities;

namespace Seguridad.Core.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Usuario, UsuarioDto>();
        }
    }
}
