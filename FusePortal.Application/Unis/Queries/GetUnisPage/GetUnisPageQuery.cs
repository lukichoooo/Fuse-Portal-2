using MediatR;

namespace FusePortal.Application.Unis.Queries.GetUnisPage
{
    public sealed record GetUnisPageQuery(
            Guid? LastId,
            int PageSize) : IRequest<List<UniDto>>;
}
