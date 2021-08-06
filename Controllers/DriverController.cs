using CartingManagmentApi.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CartingManagmentApi.Controllers
{
    [RoutePrefix("api/driver")]
    public class DriverController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public DriverController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("adddriver")]
        public async Task<ResponseStatus> adddriver(driver driverRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (driverRequest.id == "0")
                {
                    //var vehiclename = appDbContex.vehicles.Where(a => a.name == vehicleRequest.name && a.deleted == false).FirstOrDefault();
                    //if (cityname == null)
                    //{
                    var guId = Guid.NewGuid();
                    driver driver = new driver
                    {
                        id = guId.ToString(),
                        userid = driverRequest.userid,
                       name= driverRequest.name,
                       address=driverRequest.address,
                       address2= driverRequest.address2,
                       adharcardno=driverRequest.adharcardno,
                       landmark= driverRequest.landmark,
                       licenseno= driverRequest.licenseno,
                       mobileno= driverRequest.mobileno,
                       othermobileno= driverRequest.othermobileno,
                       licensevalidupto=driverRequest.licensevalidupto,
                       hireon=driverRequest.hireon,
                        perdaysalary= driverRequest.perdaysalary,
                        fulldayhour=driverRequest.fulldayhour,
                        vehicleid = driverRequest.vehicleid,
                        dateofjoining=driverRequest.dateofjoining,
                        createAt = DateTime.UtcNow,
                        active =false,
                        deleted = false
                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.drivers.Add(driver);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.objItem = guId.ToString();
                    status.message = "Driver details save successfully!";
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
                    var driver = appDbContex.drivers.Where(a => a.id == driverRequest.id).SingleOrDefault();
                    if (driver != null)
                    {
                        driver.name = driverRequest.name;
                       driver.address = driverRequest.address;
                        driver.address2 = driverRequest.address2;
                        driver.adharcardno = driverRequest.adharcardno;
                        driver.landmark = driverRequest.landmark;
                        driver.licenseno = driverRequest.licenseno;
                        driver.mobileno = driverRequest.mobileno;
                        driver.othermobileno = driverRequest.othermobileno;
                        driver.hireon = driverRequest.hireon;
                        driver.licensevalidupto = driverRequest.licensevalidupto;
                        driver.perdaysalary = driverRequest.perdaysalary;
                        driver.fulldayhour = driverRequest.fulldayhour;
                        driver.vehicleid = driverRequest.vehicleid;
                        driver.dateofjoining = driverRequest.dateofjoining;
                        driver.createAt = DateTime.UtcNow;
                        // city.updateAt = DateTime.Now;
                        //  memoryCache.Remove("citylist");
                        // appDbContex.Update(city);
                        await appDbContex.SaveChangesAsync();

                        status.status = true;
                        status.objItem = driverRequest.id;
                        status.message = "Driver details Updated Successfully!";
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
        [Route("uploaddriveradharcard")]
        public async Task<ResponseStatus> Upload()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                //var file = Request.Form.Files[0];

                var fileuploadPath = HttpContext.Current.Server.MapPath("~/Images"); ;

                var provider = new MultipartFormDataStreamProvider(fileuploadPath);
                var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                foreach (var header in Request.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                await content.ReadAsMultipartAsync(provider);
                string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();

                var type = provider.FormData["type"];
                var name = provider.FormData["name"];
                string cid = Convert.ToString(provider.FormData["Id"]);
                string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                if (File.Exists(originalFileName))
                {
                    File.Delete(originalFileName);
                }
                File.Move(uploadingFileName, originalFileName);
                driver driver = appDbContex.drivers.Where(a => a.id == cid).SingleOrDefault();
                if (driver != null)
                {

                    driver.adharcardimage = "http://api.okcarting.com/Images/" + filename;
                    await appDbContex.SaveChangesAsync();
                }

                status.status = true;
                status.message = "Adharcard uploaded successfully";
                // status.filePath = "http://api.clickperfect.me/Images/" + filename;


            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.ToString();
            }
            return status;
        }


        [HttpPost]
        [Route("uploaddriverlincense")]
        public async Task<ResponseStatus> uploaddriverlincense()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                //var file = Request.Form.Files[0];

                var fileuploadPath = HttpContext.Current.Server.MapPath("~/Images"); ;

                var provider = new MultipartFormDataStreamProvider(fileuploadPath);
                var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                foreach (var header in Request.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                await content.ReadAsMultipartAsync(provider);
                string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();

                var type = provider.FormData["type"];
                var name = provider.FormData["name"];
                string cid = Convert.ToString(provider.FormData["Id"]);
                string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                if (File.Exists(originalFileName))
                {
                    File.Delete(originalFileName);
                }
                File.Move(uploadingFileName, originalFileName);
                driver driver = appDbContex.drivers.Where(a => a.id == cid).SingleOrDefault();
                if (driver != null)
                {

                    driver.licenseimage = "http://api.okcarting.com/Images/" + filename;
                    await appDbContex.SaveChangesAsync();
                }

                status.status = true;
                status.message = "License image uploaded successfully";
                // status.filePath = "http://api.clickperfect.me/Images/" + filename;


            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.ToString();
            }
            return status;
        }


        [HttpPost]
        [Route("uploaddriverimage")]
        public async Task<ResponseStatus> uploaddriverimage()
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                //var file = Request.Form.Files[0];

                var fileuploadPath = HttpContext.Current.Server.MapPath("~/Images"); ;

                var provider = new MultipartFormDataStreamProvider(fileuploadPath);
                var content = new StreamContent(HttpContext.Current.Request.GetBufferlessInputStream(true));
                foreach (var header in Request.Content.Headers)
                {
                    content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                await content.ReadAsMultipartAsync(provider);
                string uploadingFileName = provider.FileData.Select(x => x.LocalFileName).FirstOrDefault();

                var type = provider.FormData["type"];
                var name = provider.FormData["name"];
                string cid = Convert.ToString(provider.FormData["Id"]);
                string filename = String.Concat(type, name, RandomNumber(1000, 50000) + ".jpg");
                string originalFileName = String.Concat(fileuploadPath, "\\" + (filename).Trim(new Char[] { '"' }));

                if (File.Exists(originalFileName))
                {
                    File.Delete(originalFileName);
                }
                File.Move(uploadingFileName, originalFileName);
                driver driver = appDbContex.drivers.Where(a => a.id == cid).SingleOrDefault();
                if (driver != null)
                {

                    driver.driverimage = "http://api.okcarting.com/Images/" + filename;
                    await appDbContex.SaveChangesAsync();
                }

                status.status = true;
                status.message = "Driver Photo uploaded successfully";
                // status.filePath = "http://api.clickperfect.me/Images/" + filename;


            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.ToString();
            }
            return status;
        }
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }





        [HttpPost]
        [Route("alldriverbyuser")]
        public async Task<ResponseStatus> getAlldriverlist(int pageNo, int pageSize, string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

               
                int count = appDbContex.drivers.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var citylst = from c in  appDbContex.drivers.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a=>a.createAt).Skip(skip).Take(pageSize)
                              select new
                              {

                                  c.id,
                                  c.landmark,
                                  c.licenseno,
                                  c.mobileno,
                                  c.name,
                                  c.othermobileno,
                                  c.hireon,
                                
                                  licensevalidupto = SqlFunctions.DateName("day", c.licensevalidupto).Trim() + "/" + SqlFunctions.StringConvert((double)c.licensevalidupto.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.licensevalidupto),

                                  c.adharcardimage,
                                  c.licenseimage,
                                  c.driverimage,
                                  c.perdaysalary,
                                  c.fulldayhour,
                                  c.userid,
                                  c.vehicleid,
                                  Vehiclename = appDbContex.vehicles.Where(a => a.id == c.vehicleid).FirstOrDefault().vehiclename,
                                  c.address,
                                  c.address2,
                                  c.active,
                                  c.adharcardno,
                                 
                                  dateofjoining = SqlFunctions.DateName("day", c.dateofjoining).Trim() + "/" + SqlFunctions.StringConvert((double)c.dateofjoining.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dateofjoining),
                                  dateofresinging = SqlFunctions.DateName("day", c.dateofresinging).Trim() + "/" + SqlFunctions.StringConvert((double)c.dateofresinging.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dateofresinging),

                                 
                                  c.deleted
                              };
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
        [Route("alldriverlistbyuser")]
        public async Task<ResponseStatus> getAlldriverListbyuseris(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems =from c in  appDbContex.drivers.Where(a => a.deleted == false && a.userid == userid) select new {

                    c.id,
                    c.landmark,
                    c.licenseno,
                    c.mobileno,
                    c.name,
                    c.othermobileno,
                    c.hireon,
                    licensevalidupto = SqlFunctions.DateName("day", c.licensevalidupto).Trim() + "/" + SqlFunctions.StringConvert((double)c.licensevalidupto.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.licensevalidupto),

                    c.adharcardimage,
                    c.licenseimage,
                    c.driverimage,
                    c.perdaysalary,
                    c.fulldayhour,
                    c.userid,
                    c.vehicleid,
                    Vehiclename = appDbContex.vehicles.Where(a => a.id == c.vehicleid).FirstOrDefault().vehiclename,
                    c.address,
                    c.address2,
                    c.active,
                    c.adharcardno,
                    dateofjoining = SqlFunctions.DateName("day", c.dateofjoining).Trim() + "/" + SqlFunctions.StringConvert((double)c.dateofjoining.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dateofjoining),
                    dateofresinging = SqlFunctions.DateName("day", c.dateofresinging).Trim() + "/" + SqlFunctions.StringConvert((double)c.dateofresinging.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dateofresinging),

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
        [Route("driverinfobyid")]
        public async Task<ResponseStatus> getdriverinfobyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.drivers.Where(a => a.deleted == false && a.id == id)
                                  select new
                                  {

                                      c.id,
                                      c.landmark,
                                      c.licenseno,
                                      c.mobileno,
                                      c.name,
                                      c.othermobileno,
                                      c.hireon,
                                      licensevalidupto = SqlFunctions.DateName("day", c.licensevalidupto).Trim() + "/" + SqlFunctions.StringConvert((double)c.licensevalidupto.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.licensevalidupto),

                                      c.adharcardimage,
                                      c.licenseimage,
                                      c.driverimage,
                                      c.perdaysalary,
                                      c.fulldayhour,
                                      c.userid,
                                      c.vehicleid,
                                      Vehiclename = appDbContex.vehicles.Where(a => a.id == c.vehicleid).FirstOrDefault().vehiclename,
                                      c.address,
                                      c.address2,
                                      c.active,
                                      c.adharcardno,
                                      dateofjoining = SqlFunctions.DateName("day", c.dateofjoining).Trim() + "/" + SqlFunctions.StringConvert((double)c.dateofjoining.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dateofjoining),
                                      dateofresinging = SqlFunctions.DateName("day", c.dateofresinging).Trim() + "/" + SqlFunctions.StringConvert((double)c.dateofresinging.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.dateofresinging),

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
        [Route("deletedriver")]
        public async Task<ResponseStatus> deletedriver(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var driver = appDbContex.drivers.Where(a => a.id == id).SingleOrDefault();
                if (driver != null)
                {
                    driver.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "driver deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "driver not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
