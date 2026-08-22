namespace Ufcpp.CodeAnnotationMigrator;

internal static class SourceText
{
    public static int GetLineNumber(string value, int position)
    {
        ArgumentNullException.ThrowIfNull(value);
        ArgumentOutOfRangeException.ThrowIfNegative(position);
        if (position > value.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        var line = 1;
        for (var index = 0; index < position; index++)
        {
            if (value[index] == '\n'
                || value[index] == '\r'
                && (index + 1 >= position || value[index + 1] != '\n'))
            {
                line++;
            }
        }

        return line;
    }
}
