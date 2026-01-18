using Facet;
using FusePortal.Application.UseCases.Academic.Unis;
using FusePortal.Domain.Entities.Identity.UserAggregate;

namespace FusePortal.Application.UseCases.Identity.Users
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
            ]),
    ]
    public partial record UserDetailsDto;


    [Facet(typeof(User),
            Include =
            [
                nameof(User.Id),
                nameof(User.Name),
                nameof(User.Address),
                nameof(User.Universities),
            ],
            NestedFacets =
            [
                typeof(UniDto),
            ],
            NullableProperties = false
            ),
    ]
    public partial record UserWithUniDto;


    [Facet(typeof(User),
            Include =
            [
                nameof(User.Id),
                nameof(User.Name),
                nameof(User.Address),
                nameof(User.Email),
                nameof(User.PasswordHash),
            ]),
    ]
    public partial record UserCredentialsDto;


}
