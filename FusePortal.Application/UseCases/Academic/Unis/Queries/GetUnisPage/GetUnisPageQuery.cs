using MediatR;

namespace FusePortal.Application.UseCases.Academic.Unis.Queries.GetUnisPage
{
    public sealed record GetUnisPageQuery(
            Guid? LastId,
            int PageSize) : IRequest<List<UniDto>>;
}
