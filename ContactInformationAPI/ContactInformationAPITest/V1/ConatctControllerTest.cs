using ContactInformationAPI.Controllers;
using ContactInformationAPI.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Moq;
using ContactInformationAPI.Modal;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Castle.Core.Resource;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

namespace ContactInformationAPITest.V1
{
    [TestClass]
    public class ConatctControllerTest
    {
        private readonly Mock<IContactDB> iContactDB;
        private readonly Mock<ILogger<ConatctController>> _logger;
        private readonly Mock<IHostingEnvironment>
            _hostingEnvironment;

        List<ConatctModal> conatctModals = new List<ConatctModal>();
        ConatctController conatctController;
        public ConatctControllerTest()
        {
            iContactDB = new Mock<IContactDB>();
            _logger = new Mock<ILogger<ConatctController>>();
            _hostingEnvironment = new Mock<IHostingEnvironment>();
            _hostingEnvironment.Setup(m => m.ContentRootPath).
                Returns("D:\\ContactInformation\\API\\ContactInformationAPI\\ContactInformationAPI\\ContactInformationAPI");
            for (int i = 0; i <= 20; i++)
            {
                conatctModals.Add(new ConatctModal()
                {
                    Id = i,
                    FirstName = "Fname" + i,
                    LastName = "Lname" + i,
                    Email = i + "@gmail.com"
                });
            }

            conatctController = new ConatctController(iContactDB.Object,
                    _logger.Object, _hostingEnvironment.Object);
        }

        /// <summary>
        /// it is test case fecatch data if data is  avilable
        /// </summary>
        [Test]
        public void GetContact()
        {
            //Arrange 
                iContactDB.Setup(x => x.GetConatctAsync(It.IsAny<string>())).Returns(Task.FromResult(conatctModals));

                //Act 
                var result = (OkObjectResult)conatctController.GetContact().Result;
               var apiResonse  = (APIResponse)result.Value;
                //Assert
                Assert.IsNotNull(apiResonse.Result);
        }
        /// <summary>
        /// it is test case fecatch data if data is not avilable
        /// </summary>

        [Test]
        public void GetContact_WithOutData()
        {
            //Arrange 
            iContactDB.Setup(x => x.GetConatctAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<ConatctModal>()));

            //Act 
            var result = (NotFoundObjectResult)conatctController.GetContact().Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.AreEqual(null,apiResonse.Result);
        }

        /// <summary>
        /// it is test case fecatch data if data is  avilable
        /// </summary>
        [Test]
        public void GetContactById()
        {
            //Arrange 
            iContactDB.Setup(x => x.GetContactByIdAsync(1,It.IsAny<string>())).Returns(Task.FromResult(new ConatctModal()
            {
                Id = 1,
                FirstName = "Fname" + 1,
                LastName = "Lname" + 1,
                Email = 1 + "@gmail.com"
            }));

            //Act 
            var result = (OkObjectResult)conatctController.GetContactById(1).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.IsNotNull(apiResonse.Result);
        }

        /// <summary>
        /// it is test case fecatch data if data is not avilable
        /// </summary>
        [Test]
        public void GetContactById_withoutData()
        {
            //Arrange 
            iContactDB.Setup(x => x.GetContactByIdAsync(1, It.IsAny<string>())).Returns(Task.FromResult(new ConatctModal() ));

            //Act 
            var result = (NotFoundObjectResult)conatctController.GetContactById(1).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.AreEqual(null, apiResonse.Result);
        }


        /// <summary>
        /// it is test case new record add
        /// </summary>
        [Test]
        public void AddContact()
        {
            //Arrange 
            iContactDB.Setup(x => x.AddConatctAsync(It.IsAny<ConatctModalVM>(), It.IsAny<string>())).Returns(Task.FromResult(1));

            //Act 
            var result = (OkObjectResult)conatctController.AddContact(It.IsAny<ConatctModalVM>()).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.IsNotNull(apiResonse.StatusCode =="201");
        }


        /// <summary>
        /// it is test case if record is avibale in db and try to add new record
        /// </summary>
        [Test]
        public void AddContact_DublicateRecord()
        {
            //Arrange 
            iContactDB.Setup(x => x.AddConatctAsync(It.IsAny<ConatctModalVM>(), It.IsAny<string>())).Returns(Task.FromResult(-1));

            //Act 
            var result = (OkObjectResult)conatctController.AddContact(It.IsAny<ConatctModalVM>()).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.IsNotNull(apiResonse.StatusCode == "409");
        }


        /// <summary>
        /// it is test case if record is  avilable
        /// </summary>
        [Test]
        public void UpdateContact()
        {
            //Arrange 
            iContactDB.Setup(x => x.UpdateConatctAsync(It.IsAny<ConatctModalVM>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult("200"));

            //Act 
            var result = (OkObjectResult)conatctController.UpdateContact(new ConatctModal()
            {
                Id = 1,
                FirstName = "Fname" + 1,
                LastName = "Lname" + 1,
                Email = 1 + "@gmail.com"
            }).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.IsNotNull(apiResonse.StatusCode == "201");
        }

        /// <summary>
        /// it is test case if record is not avilable
        /// </summary>
        [Test]
        public void UpdateContact_NotRecordFound()
        {
            //Arrange 
            iContactDB.Setup(x => x.UpdateConatctAsync(It.IsAny<ConatctModalVM>(), It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult("404"));

            //Act 
            var result = (NotFoundObjectResult)conatctController.UpdateContact(new ConatctModal()
            {
                Id = 1,
                FirstName = "Fname" + 1,
                LastName = "Lname" + 1,
                Email = 1 + "@gmail.com"
            }).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.IsNotNull(apiResonse.StatusCode == "404");
        }

        /// <summary>
        /// It test case for delete record.
        /// </summary>
        [Test]
        public void DeleteContact()
        {
            //Arrange 
            iContactDB.Setup(x => x.DeleteConatctAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(true));

            //Act 
            var result = (OkObjectResult)conatctController.DeleteContact(1).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.IsNotNull(apiResonse.StatusCode == "200");
        }

        /// <summary>
        /// It test case for delete record if record is not found
        /// </summary>
        [Test]
        public void DeleteContact_RecordNotFound()
        {
            //Arrange 
            iContactDB.Setup(x => x.DeleteConatctAsync(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));

            //Act 
            var result = (NotFoundObjectResult)conatctController.DeleteContact(1).Result;
            var apiResonse = (APIResponse)result.Value;
            //Assert
            Assert.IsNotNull(apiResonse.StatusCode == "404");
        }
    }
}
