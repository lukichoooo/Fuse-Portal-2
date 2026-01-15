using Facet;
using FusePortal.Application.Common;
using FusePortal.Domain.UserAggregate;

namespace FusePortal.Application.Users
{
    [Facet(typeof(User),
            Include =
            [
                nameof(User.Id),
                nameof(User.Name),
            ])
    ]
    public partial record UserDto;


    [Facet(typeof(User),
            Include =
            [
                nameof(User.Id),
                nameof(User.Name),
                nameof(User.Address),
            ],
            NestedFacets = [typeof(AddressDto)]),
    ]
    public partial record UserDetailsDto;


    [Facet(typeof(User),
            Include =
            [
                nameof(User.Id),
                nameof(User.Name),
                nameof(User.Address),
                nameof(User.Email),
                nameof(User.PasswordHash),
            ],
            NestedFacets = [typeof(AddressDto)]),
    ]
    public partial record UserCredentialsDto;

    [Facet(typeof(User),
           Include = [
               nameof(User.Name),
               nameof(User.Address),
               nameof(User.Email),
           ],
           NestedFacets = [typeof(AddressDto)]),
    ]
    public partial class UserCreateDto
    {
        [MapFrom(nameof(User.PasswordHash))]
        public string Password { get; set; } = string.Empty;
    }
    // TODO create registration flow instead
}
