using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs; 
namespace API.UnitTests.Tests
{
    public class MemberUpdateRequestTests
    {
        [Fact]
        public void MemberUpdateRequest_PropertiesShouldBeNullWhenNotAssigned()
        {
            var memberUpdateRequest = new MemberUpdateRequest();
            Assert.Null(memberUpdateRequest.Introduction);
            Assert.Null(memberUpdateRequest.LookingFor);
            Assert.Null(memberUpdateRequest.Interests);
            Assert.Null(memberUpdateRequest.City);
            Assert.Null(memberUpdateRequest.Country);
        }
        [Fact]
        public void MemberUpdateRequest_PropertiesShouldAcceptStringValues()
        {
            var memberUpdateRequest = new MemberUpdateRequest
            {
                Introduction = "Hello",
                LookingFor = "Friends",
                Interests = "Programming",
                City = "New York",
                Country = "USA"
            };
            Assert.Equal("Hello", memberUpdateRequest.Introduction);
            Assert.Equal("Friends", memberUpdateRequest.LookingFor);
            Assert.Equal("Programming", memberUpdateRequest.Interests);
            Assert.Equal("New York", memberUpdateRequest.City);
            Assert.Equal("USA", memberUpdateRequest.Country);
        }
    }
}