using CartingManagmentApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CartingManagmentApi.Controllers
{
    [RoutePrefix("api/driveradvancesalary")]
    public class DriveradvancesalaryController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public DriveradvancesalaryController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("adddriveradvancesalary")]
        public async Task<ResponseStatus> adddriveradvancesalary(driveradvancesalary driverRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (driverRequest.id == "0")
                {
                  
                    var guId = Guid.NewGuid();
                    driveradvancesalary driveradvancesalary = new driveradvancesalary
                    {
                        id = guId.ToString(),
                        userid = driverRequest.userid,
                        driverid = driverRequest.driverid,
                      advancesalaryamt=driverRequest.advancesalaryamt,
                      advancesalarydate=driverRequest.advancesalarydate,
                      advancesalarymonth=driverRequest.advancesalarymonth,
                      advancesalaryyear= driverRequest.advancesalaryyear,
                        deleted = false
                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.driveradvancesalaries.Add(driveradvancesalary);

                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Advance Salary save successfully!";
                  
                    return status;
                 
                }
                else
                {
                  
                    var driver = appDbContex.driveradvancesalaries.Where(a => a.id == driverRequest.id).SingleOrDefault();
                    if (driver != null)
                    {
                        driver.userid = driverRequest.userid;
                        driver.driverid = driverRequest.driverid;
                        driver.advancesalaryamt = driverRequest.advancesalaryamt;
                        driver.advancesalarydate = driverRequest.advancesalarydate;
                        driver.advancesalarymonth = driverRequest.advancesalarymonth;
                        driver.advancesalaryyear = driverRequest.advancesalaryyear;
                      
                        await appDbContex.SaveChangesAsync();

                        status.status = true;
                        status.message = "Advance Salary Updated Successfully!";
                        return status;

                    }
                }
              

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
        [Route("deleteadvancesalary")]
        public async Task<ResponseStatus> deleteadvancesalary(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var driver = appDbContex.driveradvancesalaries.Where(a => a.id == id).SingleOrDefault();
                if (driver != null)
                {
                    driver.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "driver advance salary deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "driver details not Exists!";

                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        [HttpPost]
        [Route("alldriveradvancesalarydetailsbyuserid")]
        public async Task<ResponseStatus> getalldriveradvancesalarydetailsbyuserid(int pageNo, int pageSize, string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                int count = appDbContex.driveradvancesalaries.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;

                status.lstItems = from c in appDbContex.driveradvancesalaries.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a => a.advancesalarydate).Skip(skip).Take(pageSize)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.advancesalaryyear,
                                      c.advancesalarymonth,                                     
                                      c.advancesalaryamt,
                                      dateofadvancesalary=c.advancesalarydate,
                                      advancesalarydate = SqlFunctions.DateName("day", c.advancesalarydate).Trim() + "/" + SqlFunctions.StringConvert((double)c.advancesalarydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.advancesalarydate),
                                      c.driverid,
                                      drivername = appDbContex.drivers.Where(a => a.id == c.driverid).FirstOrDefault().name,
                                      c.deleted
                                  };
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("advancesalaryinfobyid")]
        public async Task<ResponseStatus> advancesalaryinfobyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.driveradvancesalaries.Where(a => a.deleted == false && a.id == id)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.advancesalaryyear,
                                      c.advancesalarymonth,
                                      c.advancesalaryamt,
                                      dateofadvancesalary = c.advancesalarydate,
                                      advancesalarydate = SqlFunctions.DateName("day", c.advancesalarydate).Trim() + "/" + SqlFunctions.StringConvert((double)c.advancesalarydate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.advancesalarydate),
                                      c.driverid,
                                      drivername = appDbContex.drivers.Where(a => a.id == c.driverid).FirstOrDefault().name,
                                      c.deleted
                                  };
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
