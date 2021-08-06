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
    [RoutePrefix("api/vehicle")]
    public class VehicleController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public VehicleController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addvehicle")]
        public async Task<ResponseStatus> addCity(vehicle vehicleRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (vehicleRequest.id == "0")
                {
                    var vehiclename = appDbContex.vehicles.Where(a => a.vehiclenumber == vehicleRequest.vehiclenumber && a.userid==vehicleRequest.userid && a.deleted == false).FirstOrDefault();
                    if (vehiclename == null)
                    {
                        var guId = Guid.NewGuid();
                        vehicle vehicle = new vehicle   
                        {
                            id = guId.ToString(),
                            userid=vehicleRequest.userid,
                            vehiclename = vehicleRequest.vehiclename,
                            vehiclenumber=vehicleRequest.vehiclenumber,
                            perhourrate=vehicleRequest.perhourrate,
                            createAt=DateTime.UtcNow,
                            deleted = false
                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.vehicles.Add(vehicle);
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Vehicle details save successfully!";
                        // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                        return status;
                    }
                    else
                    {
                        status.status = false;
                        status.message = "Vehicle details Already Added!";
                    }
                }
                else
                {
                    var name = appDbContex.vehicles.Where(a => a.vehiclenumber == vehicleRequest.vehiclenumber && a.userid == vehicleRequest.userid && a.deleted == false && a.id != vehicleRequest.id).FirstOrDefault();
                    if (name == null)
                    {
                        var vehicle = appDbContex.vehicles.Where(a => a.id == vehicleRequest.id).SingleOrDefault();
                        if (vehicle != null)
                        {
                            vehicle.vehiclename = vehicleRequest.vehiclename;
                            vehicle.vehiclenumber = vehicleRequest.vehiclenumber;
                            vehicle.perhourrate = vehicleRequest.perhourrate;
                            vehicle.createAt = DateTime.UtcNow;
                            // city.updateAt = DateTime.Now;
                            //  memoryCache.Remove("citylist");
                            // appDbContex.Update(city);
                            await appDbContex.SaveChangesAsync();

                            status.status = true;
                            status.message = "Vehicle details Updated Successfully!";
                            return status;

                        }
                    }
                    status.status = false;
                    status.message = "Vehicle details Already Exists!";

                    return status;
                }
              // }

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
        [Route("allvehiclebyuser")]
        public async Task<ResponseStatus> getAllvehiclelist(int pageNo, int pageSize, string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

               
                int count = appDbContex.vehicles.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var citylst = appDbContex.vehicles.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a=>a.createAt).Skip(skip).Take(pageSize).ToList();
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
        [Route("allvehiclelistbyuser")]
        public async Task<ResponseStatus> getAllCityList(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                
                status.lstItems = appDbContex.vehicles.Where(a => a.deleted == false && a.userid== userid).ToList();
               
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("deletevehicle")]
        public async Task<ResponseStatus> deletevehicle(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var vehicle = appDbContex.vehicles.Where(a => a.id == id).SingleOrDefault();
                if (vehicle != null)
                {
                    vehicle.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "vehicle deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "vehicle not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }




    }
}
