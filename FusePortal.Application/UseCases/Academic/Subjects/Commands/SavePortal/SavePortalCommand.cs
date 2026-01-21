using MediatR;

namespace FusePortal.Application.UseCases.Academic.Subjects.Commands.SavePortal
{
    public sealed record SavePortalCommand(string PortalPageText) : IRequest;
}
