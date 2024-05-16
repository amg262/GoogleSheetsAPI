using System.Text.Json;

namespace GoogleSheetsAPI.Helpers;

/// <summary>
/// Provides helper methods for working with API data and JSON values.
/// </summary>
public static class ApiHelper
{
    /// <summary>
    /// Converts a JsonElement object to an appropriate .NET object type.
    /// </summary>
    /// <param name="obj">The object to convert, typically a JsonElement.</param>
    /// <returns>
    /// The converted object as a .NET type. If the conversion fails, returns the exception message.
    /// Possible return types are string, float, bool, or null.
    /// </returns>
    /// <remarks>
    /// This method attempts to determine the type of the JSON element and convert it to a corresponding .NET type.
    /// It handles various JSON value kinds such as Number, String, True, False, Null, Undefined, Object, and Array.
    /// If the conversion fails, it catches the exception and returns the exception message.
    /// </remarks>
    public static object GetObjectValue(object? obj)
    {
        try
        {
            if (obj == null) return "NULL";

            var typeOfObject = ((JsonElement)obj).ValueKind;

            return typeOfObject switch
            {
                JsonValueKind.Number => float.Parse(obj.ToString()), // return long.Parse(obj.ToString());
                JsonValueKind.String => obj.ToString(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Undefined => null,
                JsonValueKind.Object => obj.ToString(),
                JsonValueKind.Array => obj.ToString(),
                _ => obj.ToString()
            };
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}