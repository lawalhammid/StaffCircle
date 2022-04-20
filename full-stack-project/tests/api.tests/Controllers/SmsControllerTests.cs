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
    public class SmsControllerTests
    {
        private readonly Mock<ILogger<UnitOfWorkService>> _mockLogger;
        //use main DB. The connection string to your database
        string connectionString = "Data Source=172.24.240.1,1433;Initial Catalog = StaffCirle;user id = sa; password = Password1;";
        
        public SmsControllerTests()
        {
            _mockLogger = new Mock<ILogger<UnitOfWorkService>>();
        }

        [Fact]
        public void Test_Is_ComposeMessage_Save_Successfully()
        {
            //Set dbcontext below ;
            var options = new DbContextOptionsBuilder<EfDataContext>()
            .UseSqlServer(connectionString)
            .Options;

            var context = new EfDataContext(options);

            var unitOfWorkService = new UnitOfWorkService(context, _mockLogger.Object);
            var usersService = new UsersService(context, unitOfWorkService);

            ComposedMessages composedMessages = new ComposedMessages
            {
                Message = "Happy birthday friend",
                UserId = 1
            };

            var messagesService = new MessagesService(context, unitOfWorkService, usersService);

            var result = messagesService.AddNewMessage(composedMessages).Result;
            
            // Assert
            Assert.True(true == result.IsSuccessful);
        }

        //The reason the below is not working as expected is because twilio is not working well yet using UK phone number will test later and update the repo 
        [Fact]
        public void Test_Is_SendSMS_Sent()
        {
            //Set dbcontext below ;
            var options = new DbContextOptionsBuilder<EfDataContext>()
            .UseSqlServer(connectionString)
            .Options;

            var context = new EfDataContext(options);

            var unitOfWorkService = new UnitOfWorkService(context, _mockLogger.Object);
            var usersService = new UsersService(context, unitOfWorkService);
            var messagesService = new MessagesService(context, unitOfWorkService, usersService);

            SendMessages sendMessages = new SendMessages
            {
                 SenderId = 1,
                 MessageId = 0,
                 RecipientPhoneNo = "+447704981776"
            };

            ComposedMessages composedMessages = new ComposedMessages
            {
                Message = "Happy birthday friend",
                MessageNo = "#1-00431",
                UserId = 1
            };

            string NewMessage = "Hey, happy birthday New messa";

            var sendSMSTwiloService = new SendSMSTwiloService(context, unitOfWorkService, usersService, messagesService);

            var result = sendSMSTwiloService.SendMessage(sendMessages,composedMessages, NewMessage).Result;

            // Assert
            Assert.True(true == result.IsSuccessful);
        }

        [Fact]
        public void Test_Get_Message_By_SenderId()
        {
            //Set dbcontext below ;
            var options = new DbContextOptionsBuilder<EfDataContext>()
            .UseSqlServer(connectionString)
            .Options;

            var context = new EfDataContext(options);

            var unitOfWorkService = new UnitOfWorkService(context, _mockLogger.Object);
            var usersService = new UsersService(context, unitOfWorkService);
            var messagesService = new MessagesService(context, unitOfWorkService, usersService);

            int SenderUserId = 1;

            var sendSMSTwiloService = new SendSMSTwiloService(context, unitOfWorkService, usersService, messagesService);

            var result = sendSMSTwiloService.GetAllSendMsgByUserId(SenderUserId).Result;

            // Assert
            Assert.True(result != null && result.Count > 0);
        }

        [Fact]
        public void Test_Get_Message_By_SenderId_And_SendDate()
        {
            //Set dbcontext below ;
            var options = new DbContextOptionsBuilder<EfDataContext>()
            .UseSqlServer(connectionString)
            .Options;

            var context = new EfDataContext(options);

            var unitOfWorkService = new UnitOfWorkService(context, _mockLogger.Object);
            var usersService = new UsersService(context, unitOfWorkService);
            var messagesService = new MessagesService(context, unitOfWorkService, usersService);

            int SenderUserId = 1;
            string SentDate = "2022-04-19";

            var sendSMSTwiloService = new SendSMSTwiloService(context, unitOfWorkService, usersService, messagesService);

            var result = sendSMSTwiloService.GetAllSendMsgByUserIdByDate(SenderUserId, SentDate).Result;

            // Assert
            Assert.True(result != null && result.Count > 0);
        }

    }
}