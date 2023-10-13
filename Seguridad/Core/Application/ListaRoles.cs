using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Seguridad.Core.Dto;

namespace Seguridad.Core.Application
{
    public class ListaRoles
	{
		public class ListaRolesCommand : IRequest<List<RolDto>> {}

        public class ListaRolesHandler : IRequestHandler<ListaRolesCommand, List<RolDto>>
        {
            private readonly RoleManager<IdentityRole> _roleManager;

            public ListaRolesHandler(RoleManager<IdentityRole> roleManager)
            {
                _roleManager = roleManager;
            }

            public Task<List<RolDto>> Handle(ListaRolesCommand request, CancellationToken cancellationToken)
            {
              
                var roles = _roleManager.Roles.ToList();
                List<RolDto> rolDto = new List<RolDto>();

                foreach (var (rol, index) in roles.Select( (value, i) => (value, i) )  )
                {
                    rolDto.Add(new RolDto
                    {
                        Id = roles[index].Id,
                        Name = roles[index].Name
                    });                    
                }
                return Task.FromResult(rolDto);
                // var rolDto _mapper.Map<IdentityRole, RolDto>(roles);
                //return Task.FromResult(roles);
            }
        }



    }
}

