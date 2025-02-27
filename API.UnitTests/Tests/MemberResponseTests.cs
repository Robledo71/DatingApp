namespace API.Tests.DTOs;
using API.DTOs;
using Xunit;
using System;
using System.Collections.Generic;
public class MemberResponseTests
{
    [Fact]
    public void MemberResponse_Properties_ShouldBeSetAndRetrievedCorrectly()
    {
        // Arrange
        var photos = new List<PhotoResponse>
        {
            new PhotoResponse { Id = 1, Url = "http://example.com/photo1.jpg", IsMain = true },
            new PhotoResponse { Id = 2, Url = "http://example.com/photo2.jpg", IsMain = false }
        };
        var member = new MemberResponse
        {
            Id = 1,
            UserName = "testuser",
            Age = 25,
            PhotoUrl = "http://example.com/mainphoto.jpg",
            KnownAs = "Tester",
            Created = new DateTime(2020, 1, 1),
            LastActive = new DateTime(2022, 1, 1),
            Gender = "Female",
            Introduction = "This is a test introduction.",
            Interests = "Coding, Reading",
            LookingFor = "Friendship",
            City = "TestCity",
            Country = "TestCountry",
            Photos = photos
        };
        // Act & Assert
        Assert.Equal(1, member.Id);
        Assert.Equal("testuser", member.UserName);
        Assert.Equal(25, member.Age);
        Assert.Equal("http://example.com/mainphoto.jpg", member.PhotoUrl);
        Assert.Equal("Tester", member.KnownAs);
        Assert.Equal(new DateTime(2020, 1, 1), member.Created);
        Assert.Equal(new DateTime(2022, 1, 1), member.LastActive);
        Assert.Equal("Female", member.Gender);
        Assert.Equal("This is a test introduction.", member.Introduction);
        Assert.Equal("Coding, Reading", member.Interests);
        Assert.Equal("Friendship", member.LookingFor);
        Assert.Equal("TestCity", member.City);
        Assert.Equal("TestCountry", member.Country);
        Assert.Equal(photos, member.Photos);
    }
    [Fact]
    public void MemberResponse_DefaultConstructor_ShouldInitializePropertiesToDefaults()
    {
        // Arrange
        var member = new MemberResponse();
        // Act & Assert
        Assert.Equal(0, member.Id);
        Assert.Null(member.UserName);
        Assert.Equal(0, member.Age);
        Assert.Null(member.PhotoUrl);
        Assert.Null(member.KnownAs);
        Assert.Equal(default(DateTime), member.Created);
        Assert.Equal(default(DateTime), member.LastActive);
        Assert.Null(member.Gender);
        Assert.Null(member.Introduction);
        Assert.Null(member.Interests);
        Assert.Null(member.LookingFor);
        Assert.Null(member.City);
        Assert.Null(member.Country);
        Assert.Null(member.Photos);
    }
}