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
    [RoutePrefix("api/appversion")]
    public class AppversionController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        // private readonly IMemoryCache memoryCache;
        public AppversionController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("Addappversion")]
        public async Task<ResponseStatus> addCity(Appversion appversionRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {

                var versionname = appDbContex.appversions.Where(a => a.version == appversionRequest.version && a.deleted == false).FirstOrDefault();
                if (versionname == null)
                {
                    var guId = Guid.NewGuid();
                    Appversion appversion = new Appversion
                    {
                        id = guId.ToString(),
                        version = appversionRequest.version,
                        forceUpdate = appversionRequest.forceUpdate,
                        deleted = false,
                        publishdate = DateTime.UtcNow

                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.appversions.Add(appversion);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Save successfully!";
                    // status.lstItems = GetAllCity(cityRequest.stateId).Result.lstItems;
                    return status;
                }
                else
                {
                    status.status = false;
                    status.message = "Already Added!";
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

        [HttpGet]
        [Route("allversionlist")]
        public async Task<ResponseStatus> getallversionlist()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.appversions.Where(a => a.deleted == false).OrderByDescending(a => a.publishdate).ToList().Take(1);
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("deleteappversion")]
        public async Task<ResponseStatus> deleteAppVersion(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var appversion = appDbContex.appversions.Where(a => a.id == id).SingleOrDefault();
                if (appversion != null)
                {
                    appversion.deleted = true;
                    // memoryCache.Remove("prodcutlist");
                    //appDbContex.Update(product);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "AppVersion deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "Appversion Already Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
