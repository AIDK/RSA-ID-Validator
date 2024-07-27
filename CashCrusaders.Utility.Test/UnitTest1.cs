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

    [Test]
    public void IdentityNumberGetDOB()
    {
        var expected_86 = DateOnly.FromDateTime(new DateTime(1986, 08, 06));
        var expected_00 = DateOnly.FromDateTime(new DateTime(2000, 08, 06));

        var id_86 = "860806510608";
        var id_00 = "000806510608";

        Assert.Multiple(() =>
        {
            Assert.That(IdentityNumber.GetDoB(id_86), Is.EqualTo(expected_86));
            Assert.That(IdentityNumber.GetDoB(id_00), Is.EqualTo(expected_00));
        });
    }

    [Test]
    [TestCase("8608065106082", 38)]
    [TestCase("0008065106082", 24)]
    public void IdentityNumberGetAge(string idNumber, int expected)
    {
        var dob = IdentityNumber.GetDoB(idNumber);
        var result = IdentityNumber.GetAge(dob);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("8608065106082", false)]
    [TestCase("8608064106082", true)]
    public void IdentityNumberGetGender(string idNumber, bool expected)
    {
        var result = IdentityNumber.IsFemale(idNumber);

        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    [TestCase("8608065106082", true)]
    [TestCase("8608065106182", false)]
    public void IdentityNumberIsCitizen(string idNumber, bool expected)
    {
        var result = IdentityNumber.IsCitizen(idNumber);

        Assert.That(result, Is.EqualTo(expected));
    }
}