namespace Ufcpp.CodeAnnotationMigrator.Tests;

internal sealed class EnvironmentVariableScope : IDisposable
{
    private readonly IReadOnlyDictionary<string, string?> _originalValues;

    public EnvironmentVariableScope(IReadOnlyDictionary<string, string?> values)
    {
        _originalValues = values.Keys.ToDictionary(
            static key => key,
            Environment.GetEnvironmentVariable,
            StringComparer.Ordinal);
        foreach (var (key, value) in values)
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }

    public void Dispose()
    {
        foreach (var (key, value) in _originalValues)
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }
}
