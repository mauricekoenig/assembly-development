using System;

public static class Guard
{
    public static void NotNull(
        object value)
    {
        if (value == null)
            throw new ArgumentNullException();
    }

    public static void NotNull(
        object value,
        string paramName)
    {
        if (value == null)
            throw new ArgumentNullException(paramName);
    }

    public static void NotNullOrWhiteSpace(
        string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Value cannot be null or whitespace.");
    }

    public static void NotNullOrWhiteSpace(
        string value,
        string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Value cannot be null or whitespace.",
                paramName);
    }
}