namespace FusePortal.Api.Settings
{
    public sealed record ControllerSettings
    {
        public int DefaultPageSize { get; init; }
        public int SmallPageSize { get; init; }
        public int BigPageSize { get; init; }
    }
}
