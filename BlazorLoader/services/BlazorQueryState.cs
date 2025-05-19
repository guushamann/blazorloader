public enum RequestState
{
    NotStarted,
    Loading,
    Done
}

public class BlazorQueryState
{
    public RequestState State { get; set; } = RequestState.NotStarted;
    public bool HasError { get; set; }
    public Exception? Error { get; set; }

}
