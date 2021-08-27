using CartingManagmentApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using System.Data.Entity.SqlServer;

namespace CartingManagmentApi.Controllers
{
    [System.Web.Http.RoutePrefix("api/expense")]
    public class ExpenseController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }

        public ExpenseController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("addexpensetype")]
        public async Task<ResponseStatus> addexpensetype(expensetype expenseRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (expenseRequest.id == "0")
                {
                    var guId = Guid.NewGuid();
                    expensetype expensetype = new expensetype
                    {
                        id = guId.ToString(),
                        userid = expenseRequest.userid,
                            name = expenseRequest.name,
                        remark = expenseRequest.remark,
                        deleted = false
                    };
                    appDbContex.expensetypes.Add(expensetype);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Expense Type save successfully!";
                    return status;
                }
                else
                {
                    var expensetype = appDbContex.expensetypes.Where(x => x.id == expenseRequest.id).SingleOrDefault();
                    if (expensetype != null)
                    {
                        expensetype.userid = expenseRequest.userid;
                        expensetype.name = expenseRequest.name;
                        expensetype.remark = expenseRequest.remark;
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Expesne Type Updated Successfully!";
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

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("deleteexpensetype")]
        public async Task<ResponseStatus> deleteexpensetype(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var expensetype = appDbContex.expensetypes.Where(x => x.id == id).SingleOrDefault();
                if (expensetype != null)
                {
                    expensetype.deleted = true;
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "expense type deleted Successfully!";
                    return status;
                }
                status.status = false;
                status.message = "expense type not Exists!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("getexpensetypebyid")]
        public async Task<ResponseStatus> getexpensetypebyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                status.lstItems = appDbContex.expensetypes.Where(a => a.deleted == false && a.id == id).ToList();
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("addexpensedetails")]
        public async Task<ResponseStatus> addexpensetype(expensedetails expensedetailsRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (expensedetailsRequest.id == "0")
                {
                    var guId = Guid.NewGuid();
                    expensedetails expensedetails = new expensedetails
                    {
                        id = guId.ToString(),
                        userid = expensedetailsRequest.userid,
                        expensetypeid = expensedetailsRequest.expensetypeid,
                        amount = expensedetailsRequest.amount,
                        chequeno = expensedetailsRequest.chequeno,
                        remark = expensedetailsRequest.remark,
                        expensedate = expensedetailsRequest.expensedate,
                        deleted = false
                    };
                    appDbContex.expensedetails.Add(expensedetails);
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "Expense Details save successfully!";
                    return status;
                }
                else
                {
                    var expensedetails = appDbContex.expensedetails.Where(x => x.id == expensedetailsRequest.id).SingleOrDefault();
                    if (expensedetails != null)
                    {
                        expensedetails.userid = expensedetailsRequest.userid;
                        expensedetails.amount = expensedetailsRequest.amount;
                        expensedetails.chequeno = expensedetailsRequest.chequeno;
                        expensedetails.remark = expensedetailsRequest.remark;
                        expensedetails.expensedate = expensedetailsRequest.expensedate;
                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Expesne Detail Updated Successfully!";
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


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("deleteexpensedetails")]
        public async Task<ResponseStatus> deleteexpensedetails(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var expensedetails = appDbContex.expensedetails.Where(x => x.id == id).SingleOrDefault();
                if (expensedetails != null)
                {
                    expensedetails.deleted = true;
                    await appDbContex.SaveChangesAsync();
                    status.status = true;
                    status.message = "expense type deleted Successfully!";
                    return status;
                }
                status.status = false;
                status.message = "expense type not Exists!";
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("getexpensedetailsbyid")]
        public async Task<ResponseStatus> getexpensedetailsbyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                status.lstItems = appDbContex.expensedetails.Where(a => a.deleted == false && a.id == id).ToList();
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("allexpensetypelistbyuserid")]
        public async Task<ResponseStatus> getallexpensetypelistbyuserid(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.expensetypes.Where(a => a.deleted == false && a.userid == userid).ToList();
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("allexpensedetaillistbyuseridandtypeid")]
        public async Task<ResponseStatus> getallexpensedetaillistbyuseridandtypeid(string userid, string typeid, int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                if (!string.IsNullOrEmpty(typeid))
                {


                
                int count = appDbContex.expensedetails.Where(a => a.deleted == false && a.userid == userid && a.expensetypeid == typeid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;

                    status.lstItems = from c in appDbContex.expensedetails.Where(a => a.deleted == false && a.userid == userid && a.expensetypeid == typeid).OrderByDescending(a => a.expensedate).Skip(skip).Take(pageSize)
                    select new
                                      {
                        c.amount,
                        c.chequeno,
                        c.deleted,
                        c.expensedate,
                        dateofexpense = SqlFunctions.DateName("day", c.expensedate).Trim() + "/" + SqlFunctions.StringConvert((double)c.expensedate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.expensedate),

                        c.expensetypeid,
                        expensename=appDbContex.expensetypes.Where(a=>a.id==c.expensetypeid).FirstOrDefault().name,
                        c.id,
                        c.remark,
                        c.userid
                                      };

                                      
            }
                else
                {
                    int count = appDbContex.expensedetails.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                    int skip = (pageNo - 1) * pageSize;

                    status.lstItems = from c in appDbContex.expensedetails.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a => a.expensedate).Skip(skip).Take(pageSize)
                                      select new
                                      {
                                          c.amount,
                                          c.chequeno,
                                          c.deleted,
                                          c.expensedate,
                                          dateofexpense = SqlFunctions.DateName("day", c.expensedate).Trim() + "/" + SqlFunctions.StringConvert((double)c.expensedate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.expensedate),

                                          c.expensetypeid,
                                          expensename = appDbContex.expensetypes.Where(a => a.id == c.expensetypeid).FirstOrDefault().name,
                                          c.id,
                                          c.remark,
                                          c.userid
                                      };

                }
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("allexpensedetaillistbyuserid")]
        public async Task<ResponseStatus> getallexpensedetaillistbyuserid(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = appDbContex.expensedetails.Where(a => a.deleted == false && a.userid == userid).ToList();
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
