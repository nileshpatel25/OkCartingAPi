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
    [RoutePrefix("api/vehicleMaster")]
    public class VehicleMasterController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public VehicleMasterController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("addvehiclemaster")]
        public async Task<ResponseStatus> addvehiclemaster(vehiclemaster vehicleRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (vehicleRequest.id == "0")
                {
                    var vehiclename = appDbContex.vehiclemasters.Where(a => a.vehiclename == vehicleRequest.vehiclename && a.deleted == false).FirstOrDefault();
                    if (vehiclename == null)
                    {
                        var guId = Guid.NewGuid();
                        vehiclemaster vehiclemaster = new vehiclemaster
                        {
                            id = guId.ToString(),                           
                            vehiclename = vehicleRequest.vehiclename,                         
                            createAt = DateTime.UtcNow,
                            deleted = false,
                            approved=false
                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.vehiclemasters.Add(vehiclemaster);
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
                    var name = appDbContex.vehiclemasters.Where(a => a.vehiclename == vehicleRequest.vehiclename && a.deleted == false && a.id != vehicleRequest.id).SingleOrDefault();
                    if (name == null)
                    {
                        var vehicle = appDbContex.vehiclemasters.Where(a => a.id == vehicleRequest.id).SingleOrDefault();
                        if (vehicle != null)
                        {
                            vehicle.vehiclename = vehicleRequest.vehiclename;                       
                            vehicle.createAt = DateTime.UtcNow;                           
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
        [Route("approvedvehicle")]
        public async Task<ResponseStatus> approvedvehicle(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var vehicle = appDbContex.vehiclemasters.Where(a => a.id == id).SingleOrDefault();
                if (vehicle != null)
                {
                    vehicle.approved = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "vehicle approved Successfully!";
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


        [HttpPost]
        [Route("deletevehicle")]
        public async Task<ResponseStatus> deletevehicle(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var vehicle = appDbContex.vehiclemasters.Where(a => a.id == id).SingleOrDefault();
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
        [HttpGet]
        [Route("allvehiclelist")]
        public async Task<ResponseStatus> getAllvehiclelist()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();              
                var vehiclelist = appDbContex.vehiclemasters.Where(a => a.deleted == false && a.approved == true).OrderByDescending(a => a.createAt).ToList();
                status.lstItems = vehiclelist;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        [Route("allvehiclelistwithoutapprove")]
        public async Task<ResponseStatus> getAllvehiclelistwithoutapprove()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var vehiclelist = appDbContex.vehiclemasters.Where(a => a.deleted == false && a.approved == false).OrderByDescending(a => a.createAt).ToList();
                status.lstItems = vehiclelist;
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
