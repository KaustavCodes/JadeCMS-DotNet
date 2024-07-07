namespace JadedCmsCore.Interfaces.Core;

public interface IContentHookManager
{
    void RegisterContent(string hookName, Func<string> contentProvider);

    void RegisterContent(string hookName, Func<Task<string>> contentProvider);

    IEnumerable<Func<string>> GetContentProviders(string hookName);
}

public class ContentHookManager : IContentHookManager
{
    private readonly Dictionary<string, List<Func<string>>> _hooks = new();

    public void RegisterContent(string hookName, Func<string> contentProvider)
    {
        if (!_hooks.ContainsKey(hookName))
        {
            _hooks[hookName] = new List<Func<string>>();
        }
        _hooks[hookName].Add(contentProvider);
    }

    public void RegisterContent(string hookName, Func<Task<string>> contentProvider)
    {
        if (!_hooks.ContainsKey(hookName))
        {
            _hooks[hookName] = new List<Func<string>>();
        }
        _hooks[hookName].Add(() => contentProvider().GetAwaiter().GetResult());
    }

    public IEnumerable<Func<string>> GetContentProviders(string hookName)
    {
        if (_hooks.ContainsKey(hookName))
        {
            return _hooks[hookName];
        }
        return Enumerable.Empty<Func<string>>();
    }
}