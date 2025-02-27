namespace API.Tests.Controllers;
using Xunit;
using Moq;
using API.Controllers;
using API.Data;
using API.DTOs;
using AutoMapper;
using API.DataEntities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
public class UsersControllerTests
{
    private readonly Mock<IUserRepository> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UsersController _controller;
    public UsersControllerTests()
    {
        _mockRepo = new Mock<IUserRepository>();
        _mockMapper = new Mock<IMapper>();
        _controller = new UsersController(_mockRepo.Object, _mockMapper.Object);
    }
    [Fact]
    public async Task GetAllAsync_ReturnsOkWithMembers()
    {
        // Arrange
        var members = new List<MemberResponse>
        {
            new MemberResponse { UserName = "arenita" },
            new MemberResponse { UserName = "perlita" }
        };
        _mockRepo.Setup(repo => repo.GetMembersAsync()).ReturnsAsync(members);
        // Act
        var result = await _controller.GetAllAsync();
        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedMembers = Assert.IsType<List<MemberResponse>>(okResult.Value);
        Assert.Equal(2, returnedMembers.Count);
    }
    [Fact]
    public async Task GetByUsernameAsync_ReturnsMemberWhenFound()
    {
        // Arrange
        var member = new MemberResponse { UserName = "user1" };
        _mockRepo.Setup(repo => repo.GetMemberAsync("user1")).ReturnsAsync(member);
        // Act
        var result = await _controller.GetByUsernameAsync("user1");
        // Assert
        var returnedMember = Assert.IsType<MemberResponse>(result.Value);
        Assert.Equal("user1", returnedMember.UserName);
    }
    [Fact]
    public async Task GetByUsernameAsync_ReturnsNotFoundWhenMemberDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetMemberAsync("user1")).ReturnsAsync((MemberResponse)null);
        // Act
        var result = await _controller.GetByUsernameAsync("user1");
        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
[Fact]
    public async Task UpdateUser_ReturnsBadRequestWhenUsernameNotInToken()
    {
        // Arrange
        var request = new MemberUpdateRequest { /* Agregar propiedades necesarias */ };
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity()) // Usuario sin claims
            }
        };
        // Act
        var result = await _controller.UpdateUser(request);
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("No username found in token", badRequestResult.Value);
    }
    [Fact]
    public async Task UpdateUser_ReturnsBadRequestWhenUserNotFound()
    {
        // Arrange
        var request = new MemberUpdateRequest { /* Propiedades necesarias */ };
        var username = "user1";
        // Configurar el contexto del controlador para usar el nombre de usuario en el token
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.NameIdentifier, username) // Aseguramos que el token tiene el nombre de usuario
                }))
            }
        };
        // Configurar el mock para devolver null (ya que el usuario no se encuentra)
        _mockRepo.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync((AppUser)null);  // Cambiado a AppUser
        // Act
        var result = await _controller.UpdateUser(request);
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Could not find user", badRequestResult.Value);
    }
    [Fact]
    public async Task UpdateUser_ReturnsBadRequestWhenUpdateFails()
    {
        // Arrange
        var request = new MemberUpdateRequest 
        {
            // Configurar propiedades necesarias para la prueba
            Introduction = "Updated Introduction",
            Interests = "Updated Interests"
        };
        var username = "user1";
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, username)
                }))
            }
        };
        var user = new AppUser
        {
            UserName = username,
            KnownAs = "Tester",
            Gender = "Unknown",
            City = "TestCity",
            Country = "TestCountry"
        };
        _mockRepo.Setup(repo => repo.GetByUsernameAsync(username)).ReturnsAsync(user);
        _mockRepo.Setup(repo => repo.SaveAllAsync()).ReturnsAsync(false); // Simula un fallo al guardar
        // Act
        var result = await _controller.UpdateUser(request);
        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("Update user failed!", badRequestResult.Value);
    }
}