using MediatR;

namespace FusePortal.Application.UseCases.Academic.Unis.Queries.GetUniById
{
    public sealed record GetUniByIdQuery(Guid Id) : IRequest<UniDto>;
}
