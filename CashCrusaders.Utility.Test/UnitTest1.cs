namespace CashCrusaders.Utility.Test;

public class Tests
{
    [Test]
    [TestCase("8608065106082", true)]               // valid ID number
    [TestCase("8608065173083", false)]              // invalid ID number (invalid checksum value)
    [TestCase(" 8608065106082", true)]              // valid ID number (whitespaces are removed)
    [TestCase("", false)]                           // invalid ID number (empty)
    [TestCase(null, false)]                         // invalid ID number (null)
    [TestCase("86080651060828608065106082", false)] // invalid ID number (overflow exception)
    [TestCase("!@#^$%&*()233", false)]              // invalid ID number (special characters)
    [TestCase("8608--=106082", false)]              // invalid ID number (more special characters)
    [TestCase("080651068", false)]                  // invalid ID number (invalid length)
    public void IdentityNumberValidate(string id, bool expected)
    {
        var result = IdentityNumber.Validate(id);
        Assert.That(result.IsValid, Is.EqualTo(expected));
    }
}