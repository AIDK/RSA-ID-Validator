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
public record IdentityResponse(int? Age, DateOnly? DateOfBirth, bool? IsCitizen, bool? IsFemale, bool IsValid)
{
    public int? Age { get; init; } = Age;
    public DateOnly? DateOfBirth { get; init; } = DateOfBirth;
    public bool? IsCitizen { get; init; } = IsCitizen;
    public bool? IsFemale { get; init; } = IsFemale;
    public bool IsValid { get; init; } = IsValid;
}
