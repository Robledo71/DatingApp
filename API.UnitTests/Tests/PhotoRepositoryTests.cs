using API.DataEntities;
using Xunit;
public class PhotoTest
{
    [Fact]
    public void Photo_Should_Associate_With_AppUser()
    {
        // Arrange
        var appUser = new AppUser
        {
            Id = 1,
            UserName = "TestUser",
            KnownAs = "Tester",
            Gender = "Male",
            City = "TestCity",
            Country = "TestCountry",
            BirthDay = new DateOnly(1990, 1, 1)
        };
        var photo = new Photo
        {
            Id = 1,
            Url = "https://example.com/photo.jpg",
            IsMain = true,
            AppUser = appUser,
            AppUserId = appUser.Id
        };
        // Act & Assert
        Assert.Equal(appUser.Id, photo.AppUserId);
        Assert.Equal("https://example.com/photo.jpg", photo.Url);
        Assert.True(photo.IsMain);
    }
}