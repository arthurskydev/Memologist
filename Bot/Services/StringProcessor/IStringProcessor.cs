namespace Bot.Services.StringProcService
{
    public interface IStringProcessor
    {
        string this[string key] { get; set; }
    }
}
