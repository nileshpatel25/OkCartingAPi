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
    [RoutePrefix("api/fuel")]
    public class FuelMasterController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public FuelMasterController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addfuel")]
        public async Task<ResponseStatus> addfuel(fuelmaster fuelRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {

                if (fuelRequest.id == "0")
                {
                    var guId = Guid.NewGuid();
                    fuelmaster fuelmaster = new fuelmaster
                    {
                        id = guId.ToString(),
                        userid = fuelRequest.userid,
                        petrolpumpid=fuelRequest.petrolpumpid,
                        vehicleid = fuelRequest.vehicleid,
                        driverid = fuelRequest.driverid,
                        rate= fuelRequest.rate,
                        paymenttype=fuelRequest.paymenttype,
                        liter = fuelRequest.liter,
                        totalamount = fuelRequest.totalamount,
                        fueldate = fuelRequest.fueldate,
                        deleted = false,


                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.fuelmasters.Add(fuelmaster);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Fuel details save successfully!";
                    // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                    return status;
                }
                else
                {
                    var fuel = appDbContex.fuelmasters.Where(a => a.id == fuelRequest.id).SingleOrDefault();
                    if (fuel != null)
                    {
                        fuel.userid = fuelRequest.userid;
                        fuel.petrolpumpid = fuelRequest.petrolpumpid;
                        fuel.vehicleid = fuelRequest.vehicleid;
                        fuel.driverid = fuelRequest.driverid;
                        fuel.liter = fuelRequest.liter;
                        fuel.rate = fuelRequest.rate;
                        fuel.paymenttype = fuelRequest.paymenttype;
                        fuel.totalamount = fuelRequest.totalamount;
                        fuel.fueldate = fuelRequest.fueldate;
                        await appDbContex.SaveChangesAsync();

                        status.status = true;
                        status.message = "Fuel details Updated Successfully!";
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
        [Route("uploadreceipt")]
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
                fuelmaster fuelmaster = appDbContex.fuelmasters.Where(a => a.id == cid).SingleOrDefault();
                if (fuelmaster != null)
                {

                    fuelmaster.receipt = "http://api.okcarting.com/Images/" + filename;
                    await appDbContex.SaveChangesAsync();
                }

                status.status = true;
                status.message = "Receipt uploaded successfully";
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
        [Route("deletefuelmaster")]
        public async Task<ResponseStatus> deletefuelmaster(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var fuel = appDbContex.fuelmasters.Where(a => a.id == id).SingleOrDefault();
                if (fuel != null)
                {
                    fuel.deleted = true;
                    // memoryCache.Remove("prodcutlist");
                    //appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "fuel details deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "Fuel details not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpPost]
        [Route("allfuellistbyuserid")]
        public async Task<ResponseStatus> getAllfuelListbyuseris(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.fuelmasters.Where(a => a.deleted == false && a.userid == userid)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.petrolpumpid,
                                      petrolpump = appDbContex.Petrolpumps.Where(j => j.id == c.petrolpumpid).Select(j => new
                                      {
                                         j.id,
                                         j.name,
                                         j.address,
                                         j.ownername,
                                         j.contactno,
                                         j.otherconatcno,
                                         j.gst,
                                         j.deleted

                                      }),
                                      c.vehicleid,
                                      Vehiclename= appDbContex.vehicles.Where(v=>v.id==c.vehicleid).FirstOrDefault().vehiclename + " - " + appDbContex.vehicles.Where(v => v.id == c.vehicleid).FirstOrDefault().vehiclenumber,
                                    c.totalamount,
                                    c.rate,
                                    c.paymenttype,
                                    c.receipt,
                                    c.liter, 
                                     datefuel=c.fueldate,
                                     fueldate = SqlFunctions.DateName("day", c.fueldate).Trim() + "/" + SqlFunctions.StringConvert((double)c.fueldate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.fueldate),
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
        [Route("allfuellistbyid")]
        public async Task<ResponseStatus> getAllfuelListbyId(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.fuelmasters.Where(a => a.deleted == false && a.id == id)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.petrolpumpid,
                                      petrolpump = appDbContex.Petrolpumps.Where(j => j.id == c.petrolpumpid).Select(j => new
                                      {
                                          j.id,
                                          j.name,
                                          j.address,
                                          j.ownername,
                                          j.contactno,
                                          j.otherconatcno,
                                          j.gst,
                                          j.deleted

                                      }),
                                      c.vehicleid,
                                      Vehiclename = appDbContex.vehicles.Where(v => v.id == c.vehicleid).FirstOrDefault().vehiclename + " - " + appDbContex.vehicles.Where(v => v.id == c.vehicleid).FirstOrDefault().vehiclenumber,
                                      c.totalamount,
                                      c.liter,
                                      c.rate,
                                      c.paymenttype,
                                      c.receipt,
                                      datefuel=c.fueldate,
                                      fueldate = SqlFunctions.DateName("day", c.fueldate).Trim() + "/" + SqlFunctions.StringConvert((double)c.fueldate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.fueldate),
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
        [Route("allfuellistbyuseridwithpagging")]
        public async Task<ResponseStatus> getAllfuelListbyuserispagging(int pageNo, int pageSize, string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                int count = appDbContex.fuelmasters.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;

                status.lstItems = from c in appDbContex.fuelmasters.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a => a.fueldate).Skip(skip).Take(pageSize)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.petrolpumpid,
                                      petrolpump = appDbContex.Petrolpumps.Where(j => j.id == c.petrolpumpid).Select(j => new
                                      {
                                          j.id,
                                          j.name,
                                          j.address,
                                          j.ownername,
                                          j.contactno,
                                          j.otherconatcno,
                                          j.gst,
                                          j.deleted

                                      }),
                                      c.vehicleid,
                                      Vehiclename = appDbContex.vehicles.Where(v => v.id == c.vehicleid).FirstOrDefault().vehiclename + " - " + appDbContex.vehicles.Where(v => v.id == c.vehicleid).FirstOrDefault().vehiclenumber,
                                      c.totalamount,
                                      c.liter,
                                      c.rate,
                                      c.paymenttype,
                                      c.receipt,
                                      datefuel =c.fueldate,
                                      fueldate = SqlFunctions.DateName("day", c.fueldate).Trim() + "/" + SqlFunctions.StringConvert((double)c.fueldate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.fueldate),
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
        [Route("addpetrolpump")]
        public async Task<ResponseStatus> addnewpetrolpump(petrolpump petrolpumprequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {

                if (petrolpumprequest.id == "0")
                {
                    var guId = Guid.NewGuid();
                    petrolpump petrolpump = new petrolpump
                    {
                        id = guId.ToString(),
                        userid = petrolpumprequest.userid,
                       name=petrolpumprequest.name,
                       ownername=petrolpumprequest.ownername,
                       address=petrolpumprequest.address,
                       contactno=petrolpumprequest.contactno,
                       otherconatcno=petrolpumprequest.otherconatcno,
                       gst=petrolpumprequest.gst,
                        deleted = false,


                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.Petrolpumps.Add(petrolpump);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Petrol Pump Details save successfully!";
                    // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                    return status;
                }
                else
                {
                    var petrolpump = appDbContex.Petrolpumps.Where(a => a.id == petrolpumprequest.id).SingleOrDefault();
                    if (petrolpump != null)
                    {
                        petrolpump.userid = petrolpumprequest.userid;
                        petrolpump.name = petrolpumprequest.name;
                        petrolpump.ownername = petrolpumprequest.ownername;
                        petrolpump.address = petrolpumprequest.address;
                        petrolpump.contactno = petrolpumprequest.contactno;
                        petrolpump.otherconatcno = petrolpumprequest.otherconatcno;
                        petrolpump.gst = petrolpumprequest.gst;
                        await appDbContex.SaveChangesAsync();

                        status.status = true;
                        status.message = "Petrol Pump Details Updated Successfully!";
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
        [Route("allpetrolpumplistbyuserid")]
        public async Task<ResponseStatus> getAllpetrolpumpListbyuseris(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.Petrolpumps.Where(a => a.deleted == false && a.userid == userid).ToList();
                                 
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpPost]
        [Route("deletepetrolpump")]
        public async Task<ResponseStatus> deletepetrolpump(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var fuel = appDbContex.Petrolpumps.Where(a => a.id == id).SingleOrDefault();
                if (fuel != null)
                {
                    fuel.deleted = true;
                    // memoryCache.Remove("prodcutlist");
                    //appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "petrol pump details deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "petrol pump details not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpPost]
        [Route("petrolpumpdetailsbyid")]
        public async Task<ResponseStatus> getpetrolpumpdetailsbyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems =  appDbContex.Petrolpumps.Where(a => a.deleted == false && a.id == id).SingleOrDefault();
                                 
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
