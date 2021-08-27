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
    [RoutePrefix("api/driverattendance")]
    public class DriverAttendanceController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public DriverAttendanceController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addattendance")]
        public async Task<ResponseStatus> addattendance(driverattendance driverRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (driverRequest.id == "0")
                {
                    var drivarename = appDbContex.driverattendances.Where(a => a.dtattencanceDate == driverRequest.dtattencanceDate && a.deleted == false).FirstOrDefault();
                    if (drivarename == null)
                    {
                        var guId = Guid.NewGuid();
                        driverattendance driverattendance = new driverattendance
                        {
                            id = guId.ToString(),
                            userid = driverRequest.userid,
                            driverid = driverRequest.driverid,
                            dtattencanceDate = driverRequest.dtattencanceDate,
                            status = driverRequest.status,
                            hour=driverRequest.hour,
                            deleted = false
                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.driverattendances.Add(driverattendance);

                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Driver Attendance save successfully!";
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;

                    }
                    else
                    {
                        status.status = false;
                        status.message = "Driver Attendance Already Added for this date!";
                    }
                    return status;
                }
                else
                {
                    var drivarename = appDbContex.driverattendances.Where(a => a.dtattencanceDate == driverRequest.dtattencanceDate && a.deleted == false && a.id != driverRequest.id).FirstOrDefault();

                    //var name = appDbContex.Cities.Where(a => a.name == cityRequest.name && a.deleted == false && a.Id != cityRequest.Id).SingleOrDefault();
                    if (drivarename == null)
                    {
                        var driver = appDbContex.driverattendances.Where(a => a.id == driverRequest.id).SingleOrDefault();
                        if (driver != null)
                        {
                            driver.userid = driverRequest.userid;
                            driver.driverid = driverRequest.driverid;
                            driver.dtattencanceDate = driverRequest.dtattencanceDate;
                            driver.status = driverRequest.status;
                            driver.hour = driverRequest.hour;
                            // city.updateAt = DateTime.Now;
                            //  memoryCache.Remove("citylist");
                            // appDbContex.Update(city);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.message = "Driver Attendance Updated Successfully!";
                            //return status;

                        }
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Driver Attendance Already Added for this date!";
                    }
                }
                //status.status = false;
                //status.message = "Vehicle details Already Exists!";

                return status;
                //}
                //}

            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.Message;
                throw ex;
            }
           
        }


        [HttpPost]
        [Route("deleteattendance")]
        public async Task<ResponseStatus> deleteattendance(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var driver = appDbContex.driverattendances.Where(a => a.id == id).SingleOrDefault();
                if (driver != null)
                {
                    driver.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "driver attendance details deleted Successfully!";
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
        [Route("alldriverattendancedetailsbyuserid")]
        public async Task<ResponseStatus> getalldriverattendancedetailsbyuserid(int pageNo, int pageSize, string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                int count = appDbContex.driverattendances.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;

                status.lstItems = from c in appDbContex.driverattendances.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a => a.dtattencanceDate).Skip(skip).Take(pageSize)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                                                   
                                   
                                      status = c.status == 0 ? "FullDay" :
                                                c.status == 1 ? "HalfDay" : "Absence",
                                                attendacedate=c.dtattencanceDate,
                                      dtattencanceDate = SqlFunctions.DateName("day", c.dtattencanceDate).Trim() + "/" + SqlFunctions.StringConvert((double)c.dtattencanceDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dtattencanceDate),
                                     c.hour,
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
        [Route("driverattanancebyid")]
        public async Task<ResponseStatus> attandnceinfobyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.driverattendances.Where(a => a.deleted == false && a.id == id)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      status = c.status == 0 ? "FullDay" :
                                                c.status == 1 ? "HalfDay" : "Absence",
                                      attendacedate = c.dtattencanceDate,
                                      dtattencanceDate = SqlFunctions.DateName("day", c.dtattencanceDate).Trim() + "/" + SqlFunctions.StringConvert((double)c.dtattencanceDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dtattencanceDate),
                                      c.hour,
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
        [Route("alldriverattendancedetailsbydriverid")]
        public async Task<ResponseStatus> getalldriverattendancedetailsbyuseriddriverid(string userid,string driverid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.driverattendances.Where(a => a.deleted == false && a.userid == userid && a.driverid==driverid)
                                  select new
                                  {

                                      c.id,
                                      c.userid,                                     
                                      status = c.status == 0 ? "FullDay" :
                                                c.status == 1 ? "HalfDay" : "Absence",
                                      attendacedate = c.dtattencanceDate,
                                      dtattencanceDate = SqlFunctions.DateName("day", c.dtattencanceDate).Trim() + "/" + SqlFunctions.StringConvert((double)c.dtattencanceDate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dtattencanceDate),
                                      c.hour,
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
        [Route("driverattendancebydriverid")]
        public async Task<ResponseStatus> getdriverattendancebydriverid(string driverid, int attendacemonth, int attendaceyear)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                //DateTime dtattendacemonth = attendacemonth;
                //DateTime dtattendaceyear = attendaceyear;
                var hireon = appDbContex.drivers.Where(emp => emp.id == driverid).SingleOrDefault().hireon;
                Double perdaysalary = appDbContex.drivers.Where(emp => emp.id == driverid).SingleOrDefault().perdaysalary;

                //Day Wages
                if (hireon == "Day Wages")
                {
                    Double empfulldaycount = appDbContex.driverattendances
                    .Where(emp => emp.driverid == driverid && emp.dtattencanceDate.Month == attendacemonth && emp.dtattencanceDate.Year == attendaceyear && emp.status == 0)
                    .Count();
                    Double emphalfdaycount = appDbContex.driverattendances
                   .Where(emp => emp.driverid == driverid && emp.dtattencanceDate.Month == attendacemonth && emp.dtattencanceDate.Year == attendaceyear && emp.status == 1)
                   .Count();
                    Double empabsancecount = appDbContex.driverattendances
                   .Where(emp => emp.driverid == driverid && emp.dtattencanceDate.Month == attendacemonth && emp.dtattencanceDate.Year == attendaceyear && emp.status == 2)
                   .Count();
                    Double dbamount = (perdaysalary * empfulldaycount) + ((perdaysalary * emphalfdaycount) / 2);
                    status.status = true;
                    status.objItem = dbamount;
                    //return status;

                }
                else
                {

                }

                // status.status = true;

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}
