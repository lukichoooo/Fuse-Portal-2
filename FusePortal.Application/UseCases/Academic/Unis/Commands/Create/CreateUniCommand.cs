using FusePortal.Domain.Common.ValueObjects.Address;
using MediatR;

namespace FusePortal.Application.UseCases.Academic.Unis.Commands.Create
{
    public sealed record CreateUniCommand(
        string Name,
        Address Address
            ) : IRequest;
}
