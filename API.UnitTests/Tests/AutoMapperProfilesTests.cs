using API.DataEntities;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using Xunit;
namespace API.UnitTests
{
    public class AutoMapperProfilesTests
    {
        private readonly IMapper _mapper;
        public AutoMapperProfilesTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfiles>(); // Asegúrate de que se añade el perfil que contiene la configuración
            });
            _mapper = configuration.CreateMapper();
        }
[Fact]
public void Map_AppUser_To_MemberResponse()
{
    // Arrange
    var appUser = new AppUser
    {
        UserName = "john_doe",           // Inicializar la propiedad requerida
        KnownAs = "John Doe",            // Inicializar la propiedad requerida
        Gender = "Male",                 // Inicializar la propiedad requerida
        City = "New York",               // Inicializar la propiedad requerida
        Country = "USA",                 // Inicializar la propiedad requerida
        BirthDay = new DateOnly(1990, 1, 1), // Usa DateOnly en lugar de DateTime
        Photos = new List<Photo> { new Photo { IsMain = true, Url = "http://example.com/photo.jpg" } }
    };
    // Act
    var memberResponse = _mapper.Map<MemberResponse>(appUser);
    // Assert
    Assert.Equal(34, memberResponse.Age); // Verifica la edad
    Assert.Equal("http://example.com/photo.jpg", memberResponse.PhotoUrl); // Verifica el PhotoUrl
}
[Fact]
public void Map_MemberUpdateRequest_To_AppUser()
{
    // Arrange
    var memberUpdateRequest = new MemberUpdateRequest
    {
        Introduction = "New introduction",
        LookingFor = "Looking for someone",
        Interests = "Hiking, reading",
        City = "New York",
        Country = "USA"
    };
    // Act
    var appUser = _mapper.Map<AppUser>(memberUpdateRequest);
    // Assert
    Assert.Equal("New introduction", appUser.Introduction);
    Assert.Equal("Looking for someone", appUser.LookingFor);
    Assert.Equal("Hiking, reading", appUser.Interests);
    Assert.Equal("New York", appUser.City);
    Assert.Equal("USA", appUser.Country);
}
[Fact]
public void Map_Photo_To_PhotoResponse()
{
    // Arrange
    var photo = new Photo
    {
        Url = "http://example.com/photo.jpg",
        IsMain = true
    };
    // Act
    var photoResponse = _mapper.Map<PhotoResponse>(photo);
    // Assert
    Assert.Equal("http://example.com/photo.jpg", photoResponse.Url); // Verifica la URL
    Assert.True(photoResponse.IsMain); // Verifica que IsMain sea verdadero
}
    }
}