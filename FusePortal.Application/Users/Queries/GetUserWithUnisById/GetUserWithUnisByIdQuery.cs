using MediatR;

namespace FusePortal.Application.Users.Queries.GetUserWithUnisById
{
    public sealed record GetUserWithUnisByIdQuery(Guid Id) : IRequest<UserWithUniDto>;
}
