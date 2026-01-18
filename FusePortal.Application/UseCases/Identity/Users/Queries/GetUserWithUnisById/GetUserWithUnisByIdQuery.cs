using MediatR;

namespace FusePortal.Application.UseCases.Identity.Users.Queries.GetUserWithUnisById
{
    public sealed record GetUserWithUnisByIdQuery(Guid Id) : IRequest<UserWithUniDto>;
}
