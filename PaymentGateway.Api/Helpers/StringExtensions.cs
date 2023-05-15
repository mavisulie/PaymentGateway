namespace PaymentGateway.Api.Helpers;
/// <summary>
/// Helper class for strings.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Masks card numbers
    /// </summary>
    public static string Mask(this string cardNumber) => 
        string.Concat(cardNumber.Substring(0, 3), new String('X', 13));
}