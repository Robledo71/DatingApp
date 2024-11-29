using API.Data;
using API.DataEntities;
using API.DTOs;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
public class UserRepositoryTests
{
    private readonly DataContext _context;
    private readonly UserRepository _repository;
    private readonly IMapper _mapper;
    public UserRepositoryTests()
    {
        // Configurar AutoMapper
        var mappingConfig = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<AppUser, MemberResponse>();
            cfg.CreateMap<Photo, PhotoResponse>();  // Add this line to map Photo to PhotoResponse
        });
        _mapper = mappingConfig.CreateMapper();
        // Configurar DbContext en memoria
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        _context = new DataContext(options);
        // Sembrar datos iniciales
        SeedData();
        // Crear instancia del repositorio
        _repository = new UserRepository(_context, _mapper);
    }
    private void SeedData()
    {
        // Limpiar la base de datos
        _context.Users.RemoveRange(_context.Users);
        _context.SaveChanges();
        // Sembrar nuevos datos
        var users = new List<AppUser>
        {
            new AppUser { Id = 1, UserName = "user1", Gender = "Male", KnownAs = "John", City = "City1", Country = "Country1" },
            new AppUser { Id = 2, UserName = "user2", Gender = "Female", KnownAs = "Jane", City = "City2", Country = "Country2" }
        };
        _context.Users.AddRange(users);
        _context.SaveChanges();
    }
    [Fact]
    public async Task GetByIdAsync_ShouldReturnUserById()
    {
        // Act
        var result = await _repository.GetByIdAsync(1);
        // Assert
        Assert.NotNull(result);
        Assert.Equal("user1", result.UserName);
    }
    [Fact]
    public async Task GetByUsernameAsync_ShouldReturnUserByUsername()
    {
        // Act
        var result = await _repository.GetByUsernameAsync("user1");
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }
    [Fact]
    public async Task GetMemberAsync_ShouldReturnMemberResponseByUsername()
    {
        // Act
        var result = await _repository.GetMemberAsync("user1");
        // Assert
        Assert.NotNull(result);
        Assert.Equal("John", result.KnownAs);
    }
    [Fact]
    public async Task GetMembersAsync_ShouldReturnAllMembers()
    {
        // Act
        var result = await _repository.GetMembersAsync();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    [Fact]
    public async Task SaveAllAsync_ShouldSaveChanges()
    {
        // Act
        _context.Users.Add(new AppUser { Id = 3, UserName = "user3", Gender = "Male", KnownAs = "Jake", City = "City3", Country = "Country3" });
        var result = await _repository.SaveAllAsync();
        // Assert
        Assert.True(result);
    }
    [Fact]
    public void Update_ShouldModifyUserState()
    {
        // Arrange
        var user = _context.Users.First();
        user.KnownAs = "UpdatedName";
        // Act
        _repository.Update(user);
        var state = _context.Entry(user).State;
        // Assert
        Assert.Equal(EntityState.Modified, state);
    }
}