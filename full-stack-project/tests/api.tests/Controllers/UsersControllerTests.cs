using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using BusinessLogic.Services;
using Moq;
using BusinessLogic.Contracts;
using EFCore.EFContext;
using EFCore.UOF;
using Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace api.tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<ILogger<UnitOfWorkService>> _mockLogger;
        public UsersControllerTests()
        {
            _mockLogger = new Mock<ILogger<UnitOfWorkService>>();
        }
        [Fact]
        public void Test_Create_UniqueUserBy_Email_PhoneNo()
        {
            //Use memory DB
            //var options = new DbContextOptionsBuilder<EfDataContext>()
            //.UseInMemoryDatabase(databaseName: "StaffCirle")
            //.Options;

            //use main DB
            string connectionString = "Data Source=172.24.240.1,1433;Initial Catalog = StaffCirle;user id = sa; password = Password1;";
            var options = new DbContextOptionsBuilder<EfDataContext>()
            .UseSqlServer(connectionString)
            .Options;

            var context = new EfDataContext(options);

            var unitOfWorkService = new UnitOfWorkService(context, _mockLogger.Object);

            Users user = new Users
            {
                FullName = "Lawal Opeyemi",
                EmailAddress = "lawal.hammid4@gmail.com",
                PhoneNumber = "+547835958229",
            };

            string Password = "*PeterOla123";

            var usersService = new UsersService(context, unitOfWorkService);

            var result = usersService.AddUser(user, Password).Result;
            // Assert
            Assert.True(true == result.IsSuccessful);
        }
    }
}