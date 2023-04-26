using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using AzHttpAPI.Models;
using AzHttpAPI.Services;

namespace AzHttpAPI
{
    public class APIEmployee
    {
        IServices<Employee, int> empServ = new EmployeeServices();
        ResponseObject<Employee> empResponse = new ResponseObject<Employee>();

        //Employee Methods

        [FunctionName("GETEmp")]
        public async Task<IActionResult> GETEmp(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Employees")] HttpRequest req,
            ILogger log)
        {

            empResponse = await empServ.GetAsync();
            return new OkObjectResult(empResponse);
        }

        [FunctionName("GETSingleEmp")]
        public async Task<IActionResult> GETSingleEmp(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Employees/{id:int}")] HttpRequest req, int id,
            ILogger log)
        {
            empResponse = await empServ.GetAsync(id);
            return new OkObjectResult(empResponse);
        }

        [FunctionName("POSTEmp")]
        public async Task<IActionResult> POSTEmp(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "Employees")] HttpRequest req,
            ILogger log)
        {
            var bodyData = new StreamReader(req.Body).ReadToEnd();
            var emp = JsonSerializer.Deserialize<Employee>(bodyData);
            empResponse = await empServ.CreateAsync(emp);

            return new OkObjectResult(empResponse);
        }

        [FunctionName("PUTEmp")]
        public async Task<IActionResult> PUTEmp(
           [HttpTrigger(AuthorizationLevel.Function, "put", Route = "Employees/{id:int}")] HttpRequest req, int id,
           ILogger log)
        {
            try
            {
                //  var queryData = req.Query["id"];
                var bodyData = new StreamReader(req.Body).ReadToEnd();
                var emp = JsonSerializer.Deserialize<Employee>(bodyData);
                empResponse = await empServ.UpdateAsync(id, emp);

                return new OkObjectResult(empResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return null;
        }

        [FunctionName("DELETEEmp")]
        public async Task<IActionResult> DELETEEmp(
          [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "Employees/{id:int}")] HttpRequest req, int id,
          ILogger log)
        {
            empResponse = await empServ.DeleteAsync(id);
            return new OkObjectResult(empResponse);
        }
    }
}
