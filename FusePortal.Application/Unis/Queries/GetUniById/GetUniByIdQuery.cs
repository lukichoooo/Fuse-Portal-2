using MediatR;

namespace FusePortal.Application.Unis.Queries.GetUniById
{
    public sealed record GetUniByIdQuery(Guid Id) : IRequest<UniDto>;
}
