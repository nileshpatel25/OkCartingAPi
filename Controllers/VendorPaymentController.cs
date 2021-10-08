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
    [RoutePrefix("api/vendorpayment")]
    public class VendorPaymentController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public VendorPaymentController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addvendorpayment")]
        public async Task<ResponseStatus> addvendorpayment(vendorpayment paymentRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (paymentRequest.id == "0")
                {
                    //var vehiclename = appDbContex.vehicles.Where(a => a.name == vehicleRequest.name && a.deleted == false).FirstOrDefault();
                    //if (cityname == null)
                    //{
                    var guId = Guid.NewGuid();
                    vendorpayment vendorpayment = new vendorpayment
                    {
                        id = guId.ToString(),
                        userid = paymentRequest.userid,
                        vendorid = paymentRequest.vendorid,
                        amount = paymentRequest.amount,
                        chequeno = paymentRequest.chequeno,
                        purchaseid = paymentRequest.purchaseid,
                        paymentby = paymentRequest.paymentby,
                        paymentdate = paymentRequest.paymentdate,
                        remark = paymentRequest.remark,
                        createAt = DateTime.UtcNow,
                        paymenttype = paymentRequest.paymenttype,

                        deleted = false
                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.Vendorpayments.Add(vendorpayment);

                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "payment details save successfully!";
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
                    var customerpayment = appDbContex.Vendorpayments.Where(a => a.id == paymentRequest.id).SingleOrDefault();
                    if (customerpayment != null)
                    {
                        customerpayment.userid = paymentRequest.userid;
                        customerpayment.vendorid = paymentRequest.vendorid;
                        customerpayment.amount = paymentRequest.amount;
                        customerpayment.chequeno = paymentRequest.chequeno;
                        customerpayment.purchaseid = paymentRequest.purchaseid;
                        customerpayment.paymentby = paymentRequest.paymentby;
                        customerpayment.paymentdate = paymentRequest.paymentdate;
                        customerpayment.remark = paymentRequest.remark;
                        customerpayment.createAt = DateTime.UtcNow;
                        customerpayment.paymenttype = paymentRequest.paymenttype;
                        // city.updateAt = DateTime.Now;
                        //  memoryCache.Remove("citylist");
                        // appDbContex.Update(city);
                        await appDbContex.SaveChangesAsync();

                        status.status = true;
                        status.message = "payment details Updated Successfully!";
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
        [Route("deletepaymentdetails")]
        public async Task<ResponseStatus> deletepaymentdetails(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var payment = appDbContex.Vendorpayments.Where(a => a.id == id).SingleOrDefault();
                if (payment != null)
                {
                    payment.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "payment details deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "payment details not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        [HttpPost]
        [Route("allvendorpaymentdetailsbyuserid")]
        public async Task<ResponseStatus> getallcustometpaymentdetailsbyuserid(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.Vendorpayments.Where(a => a.deleted == false && a.userid == userid)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.amount,
                                      c.chequeno,
                                      c.purchaseid,
                                      c.paymentby,
                                      dateofpayment=c.paymentdate,
                                      paymentdate = SqlFunctions.DateName("day", c.paymentdate).Trim() + "/" + SqlFunctions.StringConvert((double)c.paymentdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.paymentdate),
                                      c.paymenttype,
                                      c.remark,
                                      c.vendorid,
                                      vendorname = appDbContex.Petrolpumps.Where(a => a.id == c.vendorid).FirstOrDefault().name,
                                      ownername = appDbContex.Petrolpumps.Where(a => a.id == c.vendorid).FirstOrDefault().ownername,
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
        [Route("vendorpaymentdetailsbyvendorid")]
        public async Task<ResponseStatus> getallcustometpaymentdetailsbycustid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.Vendorpayments.Where(a => a.deleted == false && a.vendorid == id)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.amount,
                                      c.chequeno,
                                      c.purchaseid,
                                      PurchaseDetails = appDbContex.fuelmasters.Where(a => a.id == c.purchaseid).ToList(),
                                      c.paymentby,
                                      dateofpayment = c.paymentdate,
                                      paymentdate = SqlFunctions.DateName("day", c.paymentdate).Trim() + "/" + SqlFunctions.StringConvert((double)c.paymentdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.paymentdate),
                                      c.paymenttype,
                                      c.remark,
                                      c.vendorid,
                                      vendorname = appDbContex.Petrolpumps.Where(a => a.id == c.vendorid).FirstOrDefault().name,
                                      ownername = appDbContex.Petrolpumps.Where(a => a.id == c.vendorid).FirstOrDefault().ownername,
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
