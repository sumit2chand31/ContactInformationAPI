using ContactInformationAPI.Modal;
using ContactInformationAPI.Repository;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ContactInformationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    [Consumes("application/json")]
    public class ConatctController : ControllerBase
    {
        private IContactDB contactService;
        private readonly ILogger<ConatctController> logger;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment
            _hostingEnvironment;
        private readonly string dataPath;

        public ConatctController(IContactDB _contactService,
            ILogger<ConatctController> _logger,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment 
            hostingEnvironment)
        {
            this.contactService = _contactService;
            this.logger = _logger;
            _hostingEnvironment = hostingEnvironment;
            dataPath = _hostingEnvironment.ContentRootPath.ToString()+ @"\Data\ConatctData.json";
        }

        [HttpGet]
        [Route("GetContact")]
        public async Task<IActionResult> GetContact()
        {
            var result = new APIResponse();
            
           var conatact = await this.contactService.GetConatctAsync(dataPath);
            if(conatact.Any())
            {
                result.Status = true;
                result.Result = conatact;
                result.StatusCode ="200";
                return Ok(result);
            }
            else
            {
                result.Status = false;
                result.StatusCode = "404";
                result.Message = "Data is not avilable";
                return NotFound(result);
            }
        }

        [HttpGet]
        [Route("GetContactById/{Id}")]
        public async Task<IActionResult> GetContactById(int Id)
        {
            var result = new APIResponse();
             var conatct =await this.contactService.GetContactByIdAsync(Id, dataPath);
            if(conatct.Email != null)
            {
                result.Status = true;
                result.Message = "Sucess";
                result.StatusCode = "302";
                result.Result = conatct;
                return Ok(result);
            }
            else
            {
                result.Status = false;
                result.Message = "Record not found";
                result.StatusCode = "404";
                return NotFound(result);
            }
        }

        [HttpPost]
        [Route("AddContact")]
        public async Task<IActionResult> AddContact(ConatctModalVM conatctModalVM)
        {
            var result = new APIResponse();
            if (ModelState.IsValid)
            {
               var response= await this.contactService.AddConatctAsync(conatctModalVM, dataPath);
                if(response > 0)
                {
                    result.Status = true;
                    result.StatusCode = "201";
                    result.Message = "Sucess";
                    result.Id = response;
                    return Ok(result);
                }
                else
                {
                    result.Status = false;
                    result.StatusCode = "409";
                    result.Message = "Records is avilable";
                    result.Id = response;
                    return Ok(result);
                }
            
            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                    .Where(y => y.Count > 0)
                    .ToList();

                result.Status = false;
                result.Message = JsonConvert.SerializeObject(errors);
                result.StatusCode = "404";
                return Ok(result);
            }
        }

        [HttpPost]
        [Route("UpdateContact")]
        public async Task<IActionResult> UpdateContact(ConatctModal conatctModalVM)
        {
            var result = new APIResponse();
            if (ModelState.IsValid)
            {
                var modal = new ConatctModalVM()
                {
                    FirstName = conatctModalVM.FirstName,
                    LastName = conatctModalVM.LastName,
                    Email = conatctModalVM.Email
                };
                var response =await this.contactService.UpdateConatctAsync(modal, conatctModalVM.Id, dataPath);
                if(response == "200")
                {
                    result.Status = true;
                    result.Message = "Sucess";
                    result.StatusCode = response.ToString();
                    return Ok(result);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Record is not found";
                    result.StatusCode = response.ToString();
                    return NotFound(result);
                }

            }
            else
            {
                var errors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                result.Status = false;
                result.Message = JsonConvert.SerializeObject(errors);
                result.StatusCode = "400";
                return Ok(result);
            }
        }

        [HttpDelete]
        [Route("DeleteContact/{Id}")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            var result = new APIResponse();
            if(Id > 0)
            {
               var response = await this.contactService.DeleteConatctAsync(Id, dataPath);
                if (response)
                {
                    result.Status = true;
                    result.Message = "Record delete sucessfully";
                    result.StatusCode = "200";
                    return Ok(result);
                }
                else
                {
                    result.Status = false;
                    result.Message = "Record not found";
                    result.StatusCode = "404";
                    return NotFound(result);

                }
            }
            else
            {
                result.Status = false;
                result.Message = "Please Pass Id";
                result.StatusCode = "204";
                return NotFound(result);
            }
        }
    }
}
