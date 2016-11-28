using ConMan.Providers;

namespace ConMan
{
    public interface ISettingsManager
    {
        string GetSetting(string path);
        void RegisterProvider(ISettingsManagerProvider provider);
    }
}