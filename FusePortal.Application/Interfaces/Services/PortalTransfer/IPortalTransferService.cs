namespace FusePortal.Application.Interfaces.Services.PortalTransfer
{
    public interface IPortalTransferService
    {
        Task<List<SubjectLLMDto>> SavePortalAsync(string portalPageText);
    }
}
