public class BlazorQueryOption : IBlazorQueryOption
{
    public bool Refresh { get; set; }
    public int RefreshInterval { get; set; } = 1000 * 60;
    public int CacheDuration { get; set; } = 20;
}

