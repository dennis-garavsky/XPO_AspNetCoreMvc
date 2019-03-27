using System;
using System.Collections.Generic;
using System.Linq;
using DevExtremeAspNetCoreApp1.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using DevExpress.Xpo;
using System.Threading.Tasks;
using DevExtremeAspNetCoreApp1.Helpers;

namespace DevExtremeAspNetCoreApp1.Controllers {

    [Route("api/[controller]")]
    public class CustomerController : Controller {

        UnitOfWork unitOfWork;

        public CustomerController(UnitOfWork uow) {
            unitOfWork = uow;
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions) {
            var query = unitOfWork.Query<Customer>().OrderBy(t => t.FirstName).ThenBy(t => t.LastName);
            return DataSourceLoader.Load(query, loadOptions);
        }

        [HttpPost]
        public async Task<IActionResult> Add(string values) {
            var newCustomer = JsonPopulateObjectHelper.PopulateObject<Customer>(values, unitOfWork);
            await unitOfWork.CommitChangesAsync();
            return Ok(newCustomer);
        }

        [HttpPut]
        public async Task<IActionResult> Update(int key, string values) {
            var customer = await unitOfWork.GetObjectByKeyAsync<Customer>(key);
            if(customer == null) {
                return NotFound();
            }
            JsonPopulateObjectHelper.PopulateObject(values, unitOfWork, customer);
            await unitOfWork.CommitChangesAsync();
            return Ok(customer);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int key) {
            var customer = await unitOfWork.GetObjectByKeyAsync<Customer>(key);
            if(customer == null) {
                return NotFound();
            }
            customer.Delete();
            await unitOfWork.CommitTransactionAsync();
            return Ok();
        }
    }
}