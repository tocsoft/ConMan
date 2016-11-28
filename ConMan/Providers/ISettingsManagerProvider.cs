namespace ConMan.Providers
{
    public interface ISettingsManagerProvider
    {
        string GetValue(string path);
    }
}