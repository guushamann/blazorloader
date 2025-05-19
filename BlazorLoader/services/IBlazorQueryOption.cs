public interface IBlazorQueryOption
{
    bool Refresh { get; set; }
    int RefreshInterval { get; set; }
    int CacheDuration { get; set; }
}

