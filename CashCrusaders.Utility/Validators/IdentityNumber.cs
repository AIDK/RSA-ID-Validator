using System.Text.RegularExpressions;
using CashCrusaders.Utility.Response.Models;
using Microsoft.VisualBasic;

namespace CashCrusaders.Utility;

public static partial class IdentityNumber
{
    public static IdentityResponse Validate(string idNumber)
    {
        var isValid = false;

        if (string.IsNullOrEmpty(idNumber)) return EmptyResponse();

        // to ensure we start off with `valid` information we can trim any
        // empty spaces from the string before we start processing
        idNumber = idNumber.Trim();

        // we do a quick number regex on the id number string just to
        // make sure we're working with a numeric value. then we hand it off
        // to our validation process to validation the individual's ID number
        if (NumberRegex().IsMatch(idNumber)) isValid = IsValidIdentityNumber(idNumber);

        // if the ID number failed the validation process we can return
        // an empty response as our result
        if (!isValid) return EmptyResponse();

        // otherwise we gather some additional information from the ID number
        // and return that as our response
        return DetailsResponse(idNumber);
    }

    /// <summary>
    /// Validates the provided identification number
    /// <list type="bullet">
    /// <item>idNumber: the identification number of the individual</item>
    /// </list>
    /// </summary>
    /// <param name="idNumber">The identification number of the individual</param>
    /// <returns>boolean</returns>
    private static bool IsValidIdentityNumber(string idNumber)
    {
        var isValid = false;

        // we get the last digit in the ID number which is known as the control character
        var controlChar = int.Parse(idNumber[^1].ToString());

        // we don't need the control character for our validation purpose so
        // we can remove it from the ID string
        var id = idNumber.Remove(startIndex: idNumber.Length - 1);

        // part of the validation process is gathering all the values from the ID number
        // for all `odd` and `even` positions.

        // NOTE for `odd` positions we're only getting the values and then returning the sum total.
        var odd = GetOdds(id);

        // NOTE for `even` positions we're getting the values then multiplying those by 2 and then returning
        // the sum of that total.
        var even = GetEvens(id);

        if (odd == 0 || even == 0) return isValid;

        // now that we have our `odd` and `even` results we can continue with the validation process
        var result = odd + even;
        // we have to get the second character value from our sum total (above)
        // as this will form part of our validation comparing it to the control character
        // in the ID number
        var checkSum = int.Parse(result.ToString()[^1..]);

        // compare our checksum agains the checksum control character from 
        // the ID number
        return (10 - checkSum) == controlChar;

    }
    /// <summary>
    /// This method will return all the values extracted from the odd positions of the ID number
    /// and return the sum result
    /// <list type="bullet">
    /// <item>idNumber: the identification number of the individual</item>
    /// </list>
    /// </summary>
    /// <param name="idNumber">The identification number of the individual</param>
    /// <returns>int</returns>
    /// <exception cref="OverflowException"></exception>
    private static int GetOdds(string idNumber)
    {
        List<int> values = [];

        try
        {
            for (int i = 0; i < idNumber.Length; i++)
            {
                // simple way to determine the odd index of the iteration
                // is to make use of modulus operator. then we get the id number
                // value at that index and add it to our list
                if ((i + 1) % 2 == 1) values.Add(int.Parse(idNumber[i].ToString()));
            }

            return values.Count > 0 ? values.Sum() : 0;
        }
        catch (OverflowException)
        {
            // we might run into an overflow exception and in that
            // case we just return 0. this will be handled later in
            // the validation process
            return 0;
        }
    }

    /// <summary>
    /// This method will return all values extracted from the even positions of the ID number
    /// and return the value multiplied by 2.
    /// <list type="bullet">
    /// <item>idNumber: the identification number of the individual</item>
    /// </list>
    /// </summary>
    /// <param name="idNumber">The identification number of the individual</param>
    /// <returns>int</returns>
    /// <exception cref="OverflowException"></exception>
    private static int GetEvens(string idNumber)
    {
        var values = string.Empty;

        try
        {
            for (int i = 0; i < idNumber.Length; i++)
            {
                // simple way to determine the even index of the iteration
                // is to make use of the modulus operator. then we can get the id
                // number value at that index and add it to our string
                if ((i + 1) % 2 == 0) values += idNumber[i].ToString();
            }

            // we take our `even` result and multiply that with 2.
            // then we sum each number from that result and return
            var total = 0;
            var result = (int.Parse(values) * 2).ToString();

            for (int i = 0; i < result.Length; i++)
                total += int.Parse(result[i].ToString());

            return total;
        }
        catch (OverflowException)
        {
            // we might run into an overflow exception and in that
            // case we just return 0. this will be handled later in
            // the validation process
            return 0;
        }
    }

    /// <summary>
    /// This method will return the detailed results of the ID validation process
    /// <list type="bullet">
    /// <item>idNumber: the identification number of the individual</item>
    /// </list>
    /// </summary>
    /// <param name="idNumber">The identification number of the individual</param>
    /// <returns>IdentityResponse</returns>
    private static IdentityResponse DetailsResponse(string idNumber)
    {
        var dob = GetDoB(idNumber);

        return new IdentityResponse(Age: GetAge(dob), DateOfBirth: dob, IsCitizen: IsCitizen(idNumber), IsFemale: IsFemale(idNumber), IsValid: true);
    }

    /// <summary>
    /// Calculates the individual's date of birth from their ID number
    /// <list type="bullet">
    /// <item>idNumber: the id number of the individual</item>
    /// </list>
    /// </summary>
    /// <param name="idNumber">The id number of the individual</param>
    /// <returns>date</returns>
    public static DateOnly? GetDoB(string idNumber)
    {
        if (string.IsNullOrEmpty(idNumber)) return null;

        var dob = new
        {
            // 860806
            Year = idNumber[..2],               // 86
            Month = idNumber.Substring(2, 2),   // 08
            Day = idNumber.Substring(4, 2),     // 06
        };

        // we take the last two digits of the current year
        // which in 2024 will be the number 24
        var lastTwoDigits = DateOnly.FromDateTime(DateTime.Now).Year.ToString().Substring(2, 2);

        // we evaluate whether the person was born within the last `24` years. We're using
        // 24 here because we are in the year 2024 (at the time of writing this code) however 
        // this will be handled dynamically.
        // NOTE if the person was born in the last `24` years we return their DOB year as 20**
        // otherwise they were born before the year 2000 and we return 19**.
        // IMPORTANT the DOB format returned is YYYY/MM/dd
        if (Enumerable.Range(0, int.Parse(lastTwoDigits)).Contains(int.Parse(dob.Year)))
            return DateOnly.Parse($"20{dob.Year}/{dob.Month}/{dob.Day}");

        return DateOnly.Parse($"19{dob.Year}/{dob.Month}/{dob.Day}");
    }

    /// <summary>
    /// Calculates the individual's age from their date of birth
    /// <list type="bullet">
    /// <item>dob: the date of birth of the individual</item>
    /// </list>
    /// </summary>
    /// <param name="dob"></param>
    /// <returns>nullable int</returns>
    public static int? GetAge(DateOnly? dob)
    {
        if (dob is null) return null;
        return DateOnly.FromDateTime(DateTime.Now).Year - dob.Value.Year;
    }

    /// <summary>
    /// Determines the individual's gender from their ID number
    /// <list type="bullet">
    /// <item>idNumber: the ID number of the individual</item>
    /// </list>
    /// Gender is determined by the 4 digit number starting at the 6th position: 860806 [5106] 082.
    /// </summary>
    /// <param name="idNumber">The identification number of the individual</param>
    /// <returns>boolean</returns>
    public static bool IsFemale(string idNumber)
    {
        // in order to determine the individual's gender
        // we have to look at the number from index 6 to 9.
        // any number below 5000 is Female and anything over 5000 is Male

        // using the id number provided we get the first digit and evaluate
        // whether the person is Male or Female by checking whether the digit
        // is above or below 4***
        var value = int.Parse(idNumber.Substring(6, 1));

        return value >= 0 && value <= 4;
    }

    /// <summary>
    /// Determines whether the individual is an South African citizen or not
    /// <list type="bullet">
    /// <item>idNumber: the identification number of the individual</item>
    /// </list>
    /// SA citizenship is determined by the 10th digit in the ID number: 8608065106 [0] 82
    /// </summary>
    /// <param name="idNumber">The ID number of the individual</param>
    /// <returns>boolean</returns>
    public static bool IsCitizen(string idNumber) => idNumber.Substring(10, 1) == "0";

    /// <summary>
    /// This method will return an empty response
    /// </summary>
    /// <returns>Invalid IdentityResponse</returns>
    private static IdentityResponse EmptyResponse() => new(Age: null, DateOfBirth: null, IsCitizen: null, IsFemale: null, IsValid: false);

    [GeneratedRegex(@"^\d+$")]
    private static partial Regex NumberRegex();
}
