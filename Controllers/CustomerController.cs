using CartingManagmentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
namespace CartingManagmentApi.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public CustomerController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addcustomer")]
        public async Task<ResponseStatus> addcustomer(customer customerRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (customerRequest.id == "0")
                {
                    //var vehiclename = appDbContex.vehicles.Where(a => a.name == vehicleRequest.name && a.deleted == false).FirstOrDefault();
                    //if (cityname == null)
                    //{
                    var guId = Guid.NewGuid();
                    customer customer = new customer
                    {
                        id = guId.ToString(),
                        userid = customerRequest.userid,
                        name = customerRequest.name,
                        gstin=customerRequest.gstin,
                        address = customerRequest.address,
                        address2 = customerRequest.address2,
                       // adharcardno = driverRequest.adharcardno,
                        landmark = customerRequest.landmark,
                      //  licenseno = driverRequest.licenseno,
                        mobileno = customerRequest.mobileno,
                        othermobileno = customerRequest.othermobileno,
                        createAt = DateTime.UtcNow,
                        // perdaysalary = driverRequest.perdaysalary,
                        // vehicleid = driverRequest.vehicleid,
                        // dateofjoining = driverRequest.dateofjoining,
                        //  active = false,
                        deleted = false
                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.customers.Add(customer);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Customer details save successfully!";
                    // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                    return status;
                    // }
                    //else
                    //{
                    //    status.status = false;
                    //    status.message = "City Already Added!";
                    //}
                }
                else
                {
                    //var name = appDbContex.Cities.Where(a => a.name == cityRequest.name && a.deleted == false && a.Id != cityRequest.Id).SingleOrDefault();
                    //if (name == null)
                    //{
                    var customer = appDbContex.customers.Where(a => a.id == customerRequest.id).SingleOrDefault();
                    if (customer != null)
                    {
                        customer.name = customerRequest.name;
                        customer.gstin = customerRequest.gstin;
                        customer.address = customerRequest.address;
                        customer.address2 = customerRequest.address2;
                        //customer.adharcardno = driverRequest.adharcardno;
                        customer.landmark = customerRequest.landmark;
                      //  customer.licenseno = driverRequest.licenseno;
                        customer.mobileno = customerRequest.mobileno;
                        customer.othermobileno = customerRequest.othermobileno;
                        customer.createAt = DateTime.UtcNow;
                       // customer.perdaysalary = driverRequest.perdaysalary;
                       // customer.vehicleid = driverRequest.vehicleid;
                        //customer.dateofjoining = driverRequest.dateofjoining;
                        // city.updateAt = DateTime.Now;
                        //  memoryCache.Remove("citylist");
                        // appDbContex.Update(city);
                        await appDbContex.SaveChangesAsync();

                        status.status = true;
                        status.message = "Customer details Updated Successfully!";
                        return status;

                    }
                }
                //status.status = false;
                //status.message = "Vehicle details Already Exists!";

                //return status;
                //}
                //}

            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.Message;
                throw ex;
            }
            return status;
        }

        [HttpPost]
        [Route("allcustomerbyuser")]
        public async Task<ResponseStatus> getAlldriverlist(int pageNo, int pageSize, string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                int count = appDbContex.customers.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var citylst = appDbContex.customers.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a=>a.createAt).Skip(skip).Take(pageSize).ToList();
                status.lstItems = citylst;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("allcustomerslistbyuser")]
        public async Task<ResponseStatus> getAlldriverListbyuseris(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.customers.Where(a => a.deleted == false && a.userid == userid).ToList();
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("customerinfobyid")]
        public async Task<ResponseStatus> customerinfobyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.customers.Where(a => a.deleted == false && a.id == id).ToList();
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deletecustomer")]
        public async Task<ResponseStatus> deletedriver(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var driver = appDbContex.customers.Where(a => a.id == id).SingleOrDefault();
                if (driver != null)
                {
                    driver.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "Customer deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "Customer not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
