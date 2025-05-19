using System.Text.Json;

public class BlazorQuery<T>
{
    private readonly IMemoryCacheService _cacheService;
    private readonly DataChangeNotifier _notifier;

    public BlazorQuery(IMemoryCacheService cacheService, DataChangeNotifier notifier)
    {
        _cacheService = cacheService;
        _notifier = notifier;
    }

    public BlazorQueryState QueryState { get; set; } = new();
    public T Response { get; set; } = default!;

    public async Task ExecuteAsync(string[] QueryKey, Task<T> task, IBlazorQueryOption? blazorQueryOption = null)
    {
        blazorQueryOption ??= new BlazorQueryOption();
        try
        {
            QueryState.State = RequestState.Loading;
            QueryState.HasError = false;
            QueryState.Error = null;

            Response = await _cacheService.GetOrCreate(string.Join("--", QueryKey), () => task, TimeSpan.FromSeconds(blazorQueryOption.CacheDuration));
            QueryState.State = RequestState.Done;

            // Notify subscribers about the data change
            _notifier.Notify(string.Join("--", QueryKey));
        }
        catch (Exception ex)
        {
            QueryState.HasError = true;
            QueryState.Error = ex;
            QueryState.State = RequestState.Done;
        }
    }

    public void InvalidateQueries(string[] QueryKey)
    {
        _cacheService.Remove(string.Join("--", QueryKey));
        _notifier.Notify(string.Join("--", QueryKey));
    }
}
