namespace FusePortal.Api.Filters
{
    internal sealed record ApiError(
        string Code,
        string Message
    );

    internal sealed record ApiErrorResponse(
        ApiError Error
    );
}
