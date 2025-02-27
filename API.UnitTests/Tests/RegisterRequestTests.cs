using API.DTOs;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;
namespace API.Tests
{
    public class RegisterRequestTests
    {
        // Prueba: Validar que las propiedades estén correctamente validadas cuando son válidas
        [Fact]
        public void RegisterRequest_ShouldBeValid_WhenValidProperties()
        {
            var registerRequest = new RegisterRequest
            {
                Username = "ValidUser",  // Nombre de usuario válido
                Password = "Valid1"      // Contraseña válida dentro de los requisitos
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(registerRequest, null, null);
            bool isValid = Validator.TryValidateObject(registerRequest, validationContext, validationResults, true);
            // Mostrar errores de validación si la prueba falla
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    Console.WriteLine($"Error: {validationResult.ErrorMessage}");
                }
            }
            Assert.True(isValid);  // Debe ser válido
            Assert.Empty(validationResults);  // No debe haber errores de validación
        }
        [Fact]
        public void RegisterRequest_ShouldNotBeValid_WhenUsernameIsNullOrEmpty()
        {
            var invalidRegisterRequest = new RegisterRequest
            {
                Username = string.Empty,  // Username vacío
                Password = "ValidPass"
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(invalidRegisterRequest, null, null);
            bool isValid = Validator.TryValidateObject(invalidRegisterRequest, validationContext, validationResults, true);
            Assert.False(isValid);  // Debe ser inválido
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Username"));  // Error en la propiedad Username
        }
        [Fact]
        public void RegisterRequest_ShouldNotBeValid_WhenPasswordIsTooShort()
        {
            var invalidRegisterRequest = new RegisterRequest
            {
                Username = "ValidUser",
                Password = "Pwd"  // Contraseña con menos de 4 caracteres
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(invalidRegisterRequest, null, null);
            bool isValid = Validator.TryValidateObject(invalidRegisterRequest, validationContext, validationResults, true);
            Assert.False(isValid);  // Debe ser inválido
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Password"));  // Error en la propiedad Password
        }
        // Prueba: Validar que la propiedad Password no sea válida cuando tiene más de 8 caracteres
        [Fact]
        public void RegisterRequest_ShouldNotBeValid_WhenPasswordIsTooLong()
        {
            var invalidRegisterRequest = new RegisterRequest
            {
                Username = "ValidUser",
                Password = "ThisIsTooLongPassword"  // Contraseña con más de 8 caracteres
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(invalidRegisterRequest, null, null);
            bool isValid = Validator.TryValidateObject(invalidRegisterRequest, validationContext, validationResults, true);
            Assert.False(isValid);  // Debe ser inválido
            Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Password"));  // Error en la propiedad Password
        }
        [Fact]
        public void RegisterRequest_ShouldBeValid_WhenPasswordIsBetween4And8Characters()
        {
            var validRegisterRequest = new RegisterRequest
            {
                Username = "ValidUser",
                Password = "ValidPsw"  // Contraseña válida entre 4 y 8 caracteres
            };
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(validRegisterRequest, null, null);
            bool isValid = Validator.TryValidateObject(validRegisterRequest, validationContext, validationResults, true);
            Assert.True(isValid);  // Debe ser válido
            Assert.Empty(validationResults);  // No debe haber errores de validación
        }
    }
}