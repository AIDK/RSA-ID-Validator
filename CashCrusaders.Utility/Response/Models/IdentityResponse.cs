namespace CashCrusaders.Utility.Response.Models;

/// <summary>
/// This class will return more detailed information relevant to the identification number
/// <list type="bullet">
/// <item>Age: will return the age that has been calculated from the ID number</item>
/// <item>DateOfBirth: will return the date of birth calculated from the ID number</item>
/// <item>IsCitizen: will return whether the person is a South African citizen</item>
/// <item>IsFemale: will return whether the person is a Female</item>
/// <item>IsValid: will return if the ID number is valid or not</item>
/// </list>
/// </summary>
public class IdentityResponse
{
    public int? Age { get; set; } = 0;
    public DateOnly? DateOfBirth { get; init; } = null;
    public bool? IsCitizen { get; init; } = null;
    public bool? IsFemale { get; init; } = null;
    public bool IsValid { get; init; } = false;
}
