namespace Bot.Services.String
{
    public interface IStringProcessor
    {
        string this[string key] { get; set; }
    }
}
