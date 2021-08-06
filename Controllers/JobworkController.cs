using CartingManagmentApi.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using CartingManagmentApi.Helper;

namespace CartingManagmentApi.Controllers
{
    [RoutePrefix("api/jobwork")]
    public class JobworkController : ApiController
    {


        sendsms sendsms = new sendsms();
        public ApplicationDbContext appDbContex { get; }
        public JobworkController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }

        [HttpPost]
        [Route("addjobwork")]
        public async Task<ResponseStatus> addjobwork(jobwork jobworkRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                if (jobworkRequest.id == "0")
                {
                    
                    var guId = Guid.NewGuid();
                    jobwork jobwork = new jobwork
                    {
                        id = guId.ToString(),
                        userid = jobworkRequest.userid,
                        customerid = jobworkRequest.customerid,
                        paymenttype = jobworkRequest.paymenttype,
                        createAt = DateTime.UtcNow,
                        status = "WorkInProgress",
                        deleted = false
                    };
                    // memoryCache.Remove("citylist");
                    appDbContex.jobworks.Add(jobwork);

                    await appDbContex.SaveChangesAsync();
                    double totalamount = 0;
                    for (int i = 0; i < jobworkRequest.jobworkdetails.Count; i++)
                    {
                        var gId = Guid.NewGuid();
                        jobworkdetail jobworkdetail = new jobworkdetail
                        {
                            id = gId.ToString(),
                            jobworkid= guId.ToString(),
                            discrition = jobworkRequest.jobworkdetails[i].discrition,
                            hour = jobworkRequest.jobworkdetails[i].hour,
                            perhourrate = jobworkRequest.jobworkdetails[i].perhourrate,
                            totalamount = jobworkRequest.jobworkdetails[i].totalamount,
                            vehicleid = jobworkRequest.jobworkdetails[i].vehicleid,
                            workdate = jobworkRequest.jobworkdetails[i].workdate,                          
                            deleted = false

                        };
                        //  memoryCache.Remove("orderlist");
                        appDbContex.jobworkdetails.Add(jobworkdetail);
                        await appDbContex.SaveChangesAsync();
                        totalamount += jobworkRequest.jobworkdetails[i].totalamount;


                    }
                    if (jobworkRequest.paymenttype == "Cash")
                    {
                        var pguId = Guid.NewGuid();
                        customerpayment customerpayment = new customerpayment
                        {
                            id = pguId.ToString(),
                            userid = jobworkRequest.userid,
                            customerid = jobworkRequest.customerid,
                            amount = totalamount,
                            //chequeno = paymentRequest.chequeno,
                            jobworkid = guId.ToString(),
                            paymentby = jobworkRequest.paymenttype,
                            paymentdate = DateTime.UtcNow,
                            remark = "",
                            createAt = DateTime.UtcNow,
                            paymenttype = "Received",

                            deleted = false
                        };
                        // memoryCache.Remove("citylist");
                        appDbContex.customerpayments.Add(customerpayment);

                        await appDbContex.SaveChangesAsync();
                    }

                    status.status = true;
                    status.message = "Job work details save successfully!";

                    return status;
                }
                else
                {
                   
                        var jobwork = appDbContex.jobworks.Where(a => a.id == jobworkRequest.id).SingleOrDefault();
                        if (jobwork != null)
                        {
                            jobwork.customerid = jobworkRequest.customerid;
                           
                            jobwork.paymenttype = jobworkRequest.paymenttype;
                            jobwork.createAt = DateTime.UtcNow;
                          
                            await appDbContex.SaveChangesAsync();


                        var jobworkdeta = appDbContex.jobworkdetails.Where(a => a.jobworkid == jobworkRequest.id).ToList();
                        if (jobworkdeta != null)
                        {
                            foreach (var ls in jobworkdeta)
                            {
                                appDbContex.jobworkdetails.Remove(ls);
                                await appDbContex.SaveChangesAsync();
                            }

                        }
                        for (int i = 0; i < jobworkRequest.jobworkdetails.Count; i++)
                        {
                            var gId = Guid.NewGuid();
                            jobworkdetail jobworkdetail = new jobworkdetail
                            {
                                id = gId.ToString(),
                                jobworkid = jobworkRequest.id,
                                discrition = jobworkRequest.jobworkdetails[i].discrition,
                                hour = jobworkRequest.jobworkdetails[i].hour,
                                perhourrate = jobworkRequest.jobworkdetails[i].perhourrate,
                                totalamount = jobworkRequest.jobworkdetails[i].totalamount,
                                vehicleid = jobworkRequest.jobworkdetails[i].vehicleid,
                                workdate = jobworkRequest.jobworkdetails[i].workdate,
                                deleted = false

                            };
                            //  memoryCache.Remove("orderlist");
                            appDbContex.jobworkdetails.Add(jobworkdetail);
                            await appDbContex.SaveChangesAsync();
                        }

                            status.status = true;
                            status.message = "Job work details Updated Successfully!";
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
        [Route("sendpdfinvoice")]
        public async Task<ResponseStatus> sendinvoice(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var jobdetail = appDbContex.jobworks.Where(a => a.id == id).SingleOrDefault();
                if (jobdetail != null)
                {
                    if(!string.IsNullOrEmpty(jobdetail.invoicepath))
                    {
                        if (jobdetail.status == "Completed")
                        {
                            var userinfo = appDbContex.Users.Where(a => a.Id == jobdetail.userid).FirstOrDefault();
                            var customerinfo = appDbContex.customers.Where(a => a.id == jobdetail.customerid).FirstOrDefault();
                            sendsms.SendTextSms("Hi "+customerinfo.name+" Your invoice link : "+jobdetail.invoicepath+". "+ userinfo.compnayname+ "", "91"+customerinfo.mobileno);
                            status.status = true;
                            status.message = "invoice sms sent successfully.";
                        }
                     }
                    else
                    {
                        status.message = "Invoice not found. please generate the invoice first.";
                        status.status = false;
                    }
                }
               return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        [HttpPost]
        [Route("generateinvoice")]
        public async Task<ResponseStatus> generateInvoice(string id,string userid)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var jobdetail = appDbContex.jobworks.Where(a => a.id == id).SingleOrDefault();
                if (jobdetail != null)
                {
                    if (jobdetail.status == "Completed")
                    {
                        string path = HttpContext.Current.Server.MapPath("~/Invoice/" + jobdetail.invoiceno + ".pdf");
                        jobdetail.invoicepath = "http://api.okcarting.com/Invoice/" + jobdetail.invoiceno + ".pdf";
                        var userinfo = appDbContex.Users.Where(a => a.Id == userid).FirstOrDefault();
                        var customerinfo = appDbContex.customers.Where(a => a.id == jobdetail.customerid).FirstOrDefault();
                        // Must have write permissions to the path folder
                        iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(path);
                        iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                        iText.Layout.Document document = new iText.Layout.Document(pdf);
                        iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph(userinfo.compnayname)
                           .SetTextAlignment(TextAlignment.LEFT)
                           .SetFontSize(20);

                        String para1 = "Tutorials Point originated /nfrom the idea that there existsers who respond better to online content and prefer to learn new skills at their own pace from the comforts of their drawing rooms.";

                        String para2 = "" + userinfo.compnayname + " Proprietor";

                        String para3 = "For,";

                        // Creating Paragraphs        
                        iText.Layout.Element.Paragraph paragraph1 = new iText.Layout.Element.Paragraph(para1);
                        iText.Layout.Element.Paragraph paragraph2 = new iText.Layout.Element.Paragraph(para2)
                            .SetTextAlignment(TextAlignment.RIGHT)
                           .SetFontSize(14);
                        iText.Layout.Element.Paragraph paragraph3 = new iText.Layout.Element.Paragraph(para3)
                         .SetTextAlignment(TextAlignment.RIGHT)
                         .SetPaddingRight(50)
                           .SetFontSize(14);
                        iText.Layout.Element.Paragraph newline = new iText.Layout.Element.Paragraph(new Text("\n"));


                        // Table
                        Table headertable = new Table(3, true);
                        Cell cell1 = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new iText.Layout.Element.Paragraph("From,  \n  Name : " + userinfo.name + " \n  Address: " + userinfo.address + " \n MobileNo: " + userinfo.PhoneNumber + ""));
                        cell1.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        Cell cell2 = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new iText.Layout.Element.Paragraph("To, \n " + customerinfo.name + " \n Address: " + customerinfo.address + " \n MobileNo: " + customerinfo.mobileno + ""));
                        cell2.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        Cell cell3 = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new iText.Layout.Element.Paragraph("Date : " + DateTime.UtcNow.ToShortDateString() + " \n Invoice No: " + jobdetail.invoiceno));
                        cell3.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                        headertable.AddCell(cell1);
                        headertable.AddCell(cell2);
                        headertable.AddCell(cell3);

                        float[] pointColumnWidths = { 101F, 101F, 101F, 101F, 101F };
                        Table table = new Table(pointColumnWidths);
                        Cell cell11 = new Cell(1, 1)
                           .SetBackgroundColor(ColorConstants.GRAY)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph("Discription"));

                        Cell cell12 = new Cell(1, 1)
                           .SetBackgroundColor(ColorConstants.GRAY)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph("Date"));


                        Cell cell14 = new Cell(1, 1)
                            .SetBackgroundColor(ColorConstants.GRAY)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph("Hour/Round"));

                        Cell cell15 = new Cell(1, 1)
                            .SetBackgroundColor(ColorConstants.GRAY)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph("Rate"));

                        Cell cell16 = new Cell(1, 1)
                            .SetBackgroundColor(ColorConstants.GRAY)
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph("Amount"));
                        table.AddCell(cell11);
                        table.AddCell(cell12);
                        table.AddCell(cell14);
                        table.AddCell(cell15);
                        table.AddCell(cell16);

                        var jobworkdetails = appDbContex.jobworkdetails.Where(a => a.jobworkid == jobdetail.id).ToList();
                        //Cell cell15 = new Cell(1, 1);
                        //Cell cell22 = new Cell(1, 1);
                        //Cell cell23 = new Cell(1, 1);
                        //Cell cell25 = new Cell(1, 1);
                        //Cell cell26 = new Cell(1, 1);
                        double totalamount = 0;
                        for (int i = 0; i < jobworkdetails.Count; i++)
                        {
                            totalamount += jobworkdetails[i].totalamount;
                            string workdate = jobworkdetails[i].workdate.ToShortDateString();
                            cell11 = new Cell()
                          .SetTextAlignment(TextAlignment.CENTER)
                          .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].discrition));

                            cell12 = new Cell()
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph(workdate));

                            cell14 = new Cell()
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].hour.ToString()));


                            cell15 = new Cell()
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].perhourrate.ToString()));


                            cell16 = new Cell()
                               .SetTextAlignment(TextAlignment.RIGHT)
                               .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].totalamount.ToString()));

                            table.AddCell(cell11);
                            table.AddCell(cell12);
                            table.AddCell(cell14);
                            table.AddCell(cell15);
                            table.AddCell(cell16);
                        }

                        cell11 = new Cell()
                        .SetTextAlignment(TextAlignment.CENTER)
                        .Add(new iText.Layout.Element.Paragraph());

                        cell12 = new Cell()
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph());

                        cell14 = new Cell()
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph());


                        cell15 = new Cell()
                           .SetTextAlignment(TextAlignment.CENTER)
                           .Add(new iText.Layout.Element.Paragraph());


                        cell16 = new Cell()
                            .SetTextAlignment(TextAlignment.RIGHT)
                            .Add(new iText.Layout.Element.Paragraph(totalamount.ToString()));
                        table.AddCell(cell11);
                        table.AddCell(cell12);
                        table.AddCell(cell14);
                        table.AddCell(cell15);
                        table.AddCell(cell16);

                        document.Add(header);
                        document.Add(newline); 
                        document.Add(headertable);
                        document.Add(newline);
                        // Adding paragraphs to document       
                        //document.Add(paragraph1);

                        //document.Add(paragraph2);
                        document.Add(table);
                        document.Add(newline);
                        document.Add(paragraph3);
                        document.Add(paragraph2);
                        document.Close();

                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "Invoice generated successfully!";
                    }
                }
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Route("updatestatus")]
        public async Task<ResponseStatus> updatestatus(string id,string userid)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var jobdetail = appDbContex.jobworks.Where(a => a.id == id).SingleOrDefault();
                if(jobdetail!=null)
                {
                    if(jobdetail.status== "WorkInProgress")
                    {
                        int count = appDbContex.jobworks.Where(a => a.deleted == false && a.userid== userid && a.invoiceno!=0).ToList().Count();
                        jobdetail.status = "Completed";
                       
                            jobdetail.invoiceno = count + 1;
                            string path = HttpContext.Current.Server.MapPath("~/Invoice/" + jobdetail.invoiceno + ".pdf");
                            jobdetail.invoicepath = "http://api.okcarting.com/Invoice/" + jobdetail.invoiceno + ".pdf";
                            var userinfo = appDbContex.Users.Where(a => a.Id == userid).FirstOrDefault();
                            var customerinfo = appDbContex.customers.Where(a => a.id == jobdetail.customerid).FirstOrDefault();
                            // Must have write permissions to the path folder
                            iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(path);
                            iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                            iText.Layout.Document document = new iText.Layout.Document(pdf);
                            iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph(userinfo.compnayname)
                                .SetTextAlignment(TextAlignment.LEFT)
                               .SetFontSize(20);

                            String para1 = "Tutorials Point originated /nfrom the idea that there existsers who respond better to online content and prefer to learn new skills at their own pace from the comforts of their drawing rooms.";

                            String para2 = "" + userinfo.compnayname +" Proprietor";

                            String para3 = "For,";

                            // Creating Paragraphs        
                            iText.Layout.Element.Paragraph paragraph1 = new iText.Layout.Element.Paragraph(para1);
                            iText.Layout.Element.Paragraph paragraph2 = new iText.Layout.Element.Paragraph(para2)
                                .SetTextAlignment(TextAlignment.RIGHT)
                               .SetFontSize(14); 
                            iText.Layout.Element.Paragraph paragraph3 = new iText.Layout.Element.Paragraph(para3)
                             .SetTextAlignment(TextAlignment.RIGHT)
                             .SetPaddingRight(50)
                               .SetFontSize(14);
                            iText.Layout.Element.Paragraph newline = new iText.Layout.Element.Paragraph(new Text("\n"));


                            // Table
                            Table headertable = new Table(3, true);
                            Cell cell1 = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new iText.Layout.Element.Paragraph("From,  \n  Name : "+userinfo.name+" \n  Address: "+userinfo.address+" \n MobileNo: "+userinfo.PhoneNumber+""));
                            cell1.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            Cell cell2 = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new iText.Layout.Element.Paragraph("To, \n "+customerinfo.name+" \n Address: "+ customerinfo.address+" \n MobileNo: "+customerinfo.mobileno+""));
                            cell2.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            Cell cell3 = new Cell(1, 1).SetTextAlignment(TextAlignment.LEFT).Add(new iText.Layout.Element.Paragraph("Date : " + DateTime.UtcNow.ToShortDateString() + " \n Invoice No: " + jobdetail.invoiceno));
                            cell3.SetBorder(iText.Layout.Borders.Border.NO_BORDER);
                            headertable.AddCell(cell1);
                            headertable.AddCell(cell2);
                            headertable.AddCell(cell3);

                            float[] pointColumnWidths = { 101F, 101F, 101F, 101F, 101F };
                            Table table = new Table(pointColumnWidths);
                            Cell cell11 = new Cell(1, 1)
                               .SetBackgroundColor(ColorConstants.GRAY)
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph("Discription"));

                            Cell cell12 = new Cell(1, 1)
                               .SetBackgroundColor(ColorConstants.GRAY)
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph("Date"));

                           
                            Cell cell14 = new Cell(1, 1)
                                .SetBackgroundColor(ColorConstants.GRAY)
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph("Hour/Round"));

                            Cell cell15 = new Cell(1, 1)
                                .SetBackgroundColor(ColorConstants.GRAY)
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph("Rate"));

                            Cell cell16 = new Cell(1, 1)
                                .SetBackgroundColor(ColorConstants.GRAY)
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph("Amount"));
                            table.AddCell(cell11);
                            table.AddCell(cell12);
                            table.AddCell(cell14);
                            table.AddCell(cell15);
                            table.AddCell(cell16);

                            var jobworkdetails = appDbContex.jobworkdetails.Where(a => a.jobworkid == jobdetail.id).ToList();
                            //Cell cell15 = new Cell(1, 1);
                            //Cell cell22 = new Cell(1, 1);
                            //Cell cell23 = new Cell(1, 1);
                            //Cell cell25 = new Cell(1, 1);
                            //Cell cell26 = new Cell(1, 1);
                            double totalamount = 0;
                            for (int i=0; i<jobworkdetails.Count; i++)
                            {
                                totalamount += jobworkdetails[i].totalamount;
                                string workdate =  jobworkdetails[i].workdate.ToShortDateString();
                                cell11 = new Cell()                             
                              .SetTextAlignment(TextAlignment.CENTER)
                              .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].discrition));

                                cell12 = new Cell()                                   
                                   .SetTextAlignment(TextAlignment.CENTER)
                                   .Add(new iText.Layout.Element.Paragraph(workdate));

                                cell14 = new Cell()                                   
                                   .SetTextAlignment(TextAlignment.CENTER)
                                   .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].hour.ToString()));


                                cell15 = new Cell()                                   
                                   .SetTextAlignment(TextAlignment.CENTER)
                                   .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].perhourrate.ToString()));


                                cell16 = new Cell()                                   
                                   .SetTextAlignment(TextAlignment.RIGHT)
                                   .Add(new iText.Layout.Element.Paragraph(jobworkdetails[i].totalamount.ToString()));
                               
                                table.AddCell(cell11);
                                table.AddCell(cell12);
                                table.AddCell(cell14);
                                table.AddCell(cell15);
                                table.AddCell(cell16);
                            }

                            cell11 = new Cell()
                            .SetTextAlignment(TextAlignment.CENTER)
                            .Add(new iText.Layout.Element.Paragraph());

                            cell12 = new Cell()
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph());

                            cell14 = new Cell()
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph());


                            cell15 = new Cell()
                               .SetTextAlignment(TextAlignment.CENTER)
                               .Add(new iText.Layout.Element.Paragraph());


                            cell16 = new Cell()                                
                                .SetTextAlignment(TextAlignment.RIGHT)                              
                                .Add(new iText.Layout.Element.Paragraph(totalamount.ToString()));
                            table.AddCell(cell11);
                            table.AddCell(cell12);
                            table.AddCell(cell14);
                            table.AddCell(cell15);
                            table.AddCell(cell16);

                            //table.AddCell(cell41);
                            //table.AddCell(cell42);
                            //headertable.AddCell(cell1);
                            //document.Add(headertable);



                            document.Add(header);
                            document.Add(newline);
                            document.Add(headertable);
                            document.Add(newline);
                            // Adding paragraphs to document       
                            //document.Add(paragraph1);

                            //document.Add(paragraph2);
                            document.Add(table);
                            document.Add(newline);
                            document.Add(paragraph3);
                            document.Add(paragraph2);
                            document.Close();
                        }

                        await appDbContex.SaveChangesAsync();
                        status.status = true;
                        status.message = "JobWork Completed Successfully!";
                    }
                    else
                    {
                        status.status = false;
                        status.message = "JobWork Already Completed.";
                    }
               
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        [HttpPost]
        [Route("alljobworkdetails")]
        public async Task<ResponseStatus> getAlljobworklist(int pageNo, int pageSize, string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                int count = appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid).ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var citylst = from c in appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a => a.createAt).Skip(skip).Take(pageSize)
                              select new
                              {

                                  c.id,
                                  c.customerid,
                                  c.paymenttype,
                                  customername = appDbContex.customers.Where(a => a.id == c.customerid).FirstOrDefault().name,
                                  c.deleted,
                                  c.status,
                                  c.createAt,
                                  c.invoiceno,
                                  c.invoicepath,
                                  totalamt = (from jobwork in appDbContex.jobworkdetails
                                              where jobwork.jobworkid == c.id && jobwork.deleted == false  
                                              select jobwork).Sum(e => (double?)e.totalamount) ?? 0,
                                  jobworkdetails = appDbContex.jobworkdetails.Where(j => j.jobworkid == c.id).Select(j=>new
                                  {
                                      j.hour,
                                      j.perhourrate,
                                      j.totalamount,
                                      j.vehicleid,
                                      Vehiclename = appDbContex.vehicles.Where(a => a.id == j.vehicleid).FirstOrDefault().vehiclename,
                                      workdate = SqlFunctions.DateName("day", j.workdate).Trim() + "/" + SqlFunctions.StringConvert((double)j.workdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", j.workdate),
                                      j.discrition

                                  })
                                 
                                  
                                
                                 


                              };
                status.lstItems = citylst;
                status.status = true;
                status.objItem = count;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("alljobworkdetailslistbyuser")]
        public async Task<ResponseStatus> getAlljobworkdetailsList(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid).OrderByDescending(a => a.createAt)
                                  select new
                                  {

                                      c.id,
                                      c.customerid,
                                      c.paymenttype,
                                      customername = appDbContex.customers.Where(a => a.id == c.customerid).FirstOrDefault().name,
                                      c.deleted,
                                      c.status,
                                      c.createAt,
                                      c.invoiceno,
                                      c.invoicepath,
                                      totalamt = (from jobwork in appDbContex.jobworkdetails
                                                  where jobwork.jobworkid == c.id && jobwork.deleted == false
                                                  select jobwork).Sum(e => (double?)e.totalamount) ?? 0,
                                      jobworkdetails = appDbContex.jobworkdetails.Where(j => j.jobworkid == c.id).Select(j => new
                                      {
                                          j.hour,
                                          j.perhourrate,
                                          j.totalamount,
                                          j.vehicleid,
                                          Vehiclename = appDbContex.vehicles.Where(a => a.id == j.vehicleid).FirstOrDefault().vehiclename,
                                          workdate = SqlFunctions.DateName("day", j.workdate).Trim() + "/" + SqlFunctions.StringConvert((double)j.workdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", j.workdate),
                                          j.discrition

                                      })






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
        [Route("alljobworkdetailslistbyid")]
        public async Task<ResponseStatus> getAlljobworkdetailsListbyid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.jobworks.Where(a => a.deleted == false && a.id == id)
                                  select new
                                  {

                                      c.id,
                                      c.customerid,
                                      c.paymenttype,
                                      customername = appDbContex.customers.Where(a => a.id == c.customerid).FirstOrDefault().name,
                                      c.deleted,
                                      c.status,
                                      c.createAt,
                                      c.invoiceno,
                                      c.invoicepath,
                                      totalamt = (from jobwork in appDbContex.jobworkdetails
                                                  where jobwork.jobworkid == c.id && jobwork.deleted == false
                                                  select jobwork).Sum(e => (double?)e.totalamount) ?? 0,
                                      jobworkdetails = appDbContex.jobworkdetails.Where(j => j.jobworkid == c.id).Select(j => new
                                      {
                                          j.hour,
                                          j.perhourrate,
                                          j.totalamount,
                                          j.vehicleid,
                                          Vehiclename = appDbContex.vehicles.Where(a => a.id == j.vehicleid).FirstOrDefault().vehiclename,
                                         // j.workdate,
                                          workdate= SqlFunctions.DateName("day", j.workdate).Trim() + "/" + SqlFunctions.StringConvert((double)j.workdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", j.workdate),
                                          j.discrition

                                      })






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
        [Route("customerjobworkdetailsofpendingpayment")]
        public async Task<ResponseStatus> customerjobworkdetailsofpendingpayment(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from a in appDbContex.jobworks.Where(a => a.deleted == false && a.customerid == id)
                                  select new
                                  {
                                      a.id,
                                      a.customerid,
                                      a.paymenttype,
                                      customername = appDbContex.customers.Where(b => b.id == a.customerid).FirstOrDefault().name,
                                      a.deleted,
                                      a.status,
                                      a.createAt,
                                      a.invoiceno,
                                      a.invoicepath,
                                      jobworkdetails = appDbContex.jobworkdetails.Where(j => j.jobworkid == a.id).Select(j => new
                                      {
                                          j.hour,
                                          j.perhourrate,
                                          j.totalamount,
                                          j.vehicleid,
                                          Vehiclename = appDbContex.vehicles.Where(v => v.id == j.vehicleid).FirstOrDefault().vehiclename,
                                          workdate = SqlFunctions.DateName("day", j.workdate).Trim() + "/" + SqlFunctions.StringConvert((double)j.workdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", j.workdate),
                                          j.discrition

                                      }),
                                      totalamt = (from jobwork in appDbContex.jobworkdetails
                                                  where jobwork.jobworkid == a.id && jobwork.deleted == false
                                                  select jobwork).Sum(e => (double?)e.totalamount) ?? 0,
                                      receivedamt = (from payment in appDbContex.customerpayments
                                                     where payment.jobworkid == a.id && payment.deleted==false
                                                     select payment).Sum(e => (double?)e.amount) ?? 0,
                                      pendingamt = ((from jobwork in appDbContex.jobworkdetails
                                                    where jobwork.jobworkid == a.id && jobwork.deleted == false
                                                    select jobwork).Sum(e => (double?)e.totalamount) ?? 0) -
                                                     ((from payment in appDbContex.customerpayments
                                                      where payment.jobworkid == a.id && payment.deleted == false
                                                      select payment).Sum(e => (double?)e.amount) ?? 0)

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
        [Route("amountdetails")]
        public async Task<ResponseStatus> receivependingamount(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                double totalamount = 0;
                double receivedamt = 0;
                var jobwork = appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid).ToList();
                if(jobwork!=null && jobwork.Count>0)
                {
                    foreach(var ls in jobwork)
                    { 
                    totalamount += (from a in appDbContex.jobworkdetails
                                    where a.jobworkid == ls.id && a.deleted == false
                                    select a).Sum(e => (double?)e.totalamount) ?? 0;
                    }
                }
                receivedamt = (from payment in appDbContex.customerpayments
                                             where payment.userid == userid && payment.deleted == false
                                             select payment).Sum(e => (double?)e.amount) ?? 0;
                status.lstItems = receivedamt;
                status.objItem = totalamount - receivedamt;
                //status.lstItems = from a in appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid)
                //                  select new
                //                  {

                //                      totalamt = (from jobwork in appDbContex.jobworkdetails
                //                                  where jobwork.jobworkid == a.id && jobwork.deleted == false
                //                                  select jobwork).Sum(e => (double?)e.totalamount) ?? 0,
                //                      receivedamt = (from payment in appDbContex.customerpayments
                //                                     where payment.userid == userid && payment.deleted == false
                //                                     select payment).Sum(e => (double?)e.amount) ?? 0,
                //                      pendingamt = ((from jobwork in appDbContex.jobworkdetails
                //                                     where jobwork.jobworkid == a.id && jobwork.deleted == false
                //                                     select jobwork).Sum(e => (double?)e.totalamount) ?? 0) -
                //                                     ((from payment in appDbContex.customerpayments
                //                                       where payment.userid == userid && payment.deleted == false
                //                                       select payment).Sum(e => (double?)e.amount) ?? 0)

                //                  };

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("jobworkdetailsofpendingpaymentbyuserid")]
        public async Task<ResponseStatus> jobworkdetailsofpendingpaymentbyuserid(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from a in appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid)
                                  select new
                                  {
                                      a.id,
                                      a.customerid,
                                      a.paymenttype,
                                      customername = appDbContex.customers.Where(b => b.id == a.customerid).FirstOrDefault().name,
                                      a.deleted,
                                      a.status,
                                      a.createAt,
                                      a.invoiceno,
                                      a.invoicepath,
                                      jobworkdetails = appDbContex.jobworkdetails.Where(j => j.jobworkid == a.id).Select(j => new
                                      {
                                          j.hour,
                                          j.perhourrate,
                                          j.totalamount,
                                          j.vehicleid,
                                          Vehiclename = appDbContex.vehicles.Where(v => v.id == j.vehicleid).FirstOrDefault().vehiclename,
                                          workdate = SqlFunctions.DateName("day", j.workdate).Trim() + "/" + SqlFunctions.StringConvert((double)j.workdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", j.workdate),
                                          j.discrition

                                      }),
                                      totalamt = (from jobwork in appDbContex.jobworkdetails
                                                  where jobwork.jobworkid == a.id && jobwork.deleted == false
                                                  select jobwork).Sum(e => (double?)e.totalamount) ?? 0,
                                      receivedamt = (from payment in appDbContex.customerpayments
                                                     where payment.jobworkid == a.id && payment.deleted == false
                                                     select payment).Sum(e => (double?)e.amount) ?? 0,
                                      pendingamt = ((from jobwork in appDbContex.jobworkdetails
                                                     where jobwork.jobworkid == a.id && jobwork.deleted == false
                                                     select jobwork).Sum(e => (double?)e.totalamount) ?? 0) -
                                                     ((from payment in appDbContex.customerpayments
                                                       where payment.jobworkid == a.id && payment.deleted == false
                                                       select payment).Sum(e => (double?)e.amount) ?? 0)

                                  };

                return status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost]
        [Route("alljobworkdetailslistbycustid")]
        public async Task<ResponseStatus> getAlljobworkdetailsListbycustid(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                status.lstItems = from c in appDbContex.jobworks.Where(a => a.deleted == false && a.customerid == id)
                                  select new
                                  {

                                      c.id,
                                      c.customerid,
                                      c.paymenttype,
                                      customername = appDbContex.customers.Where(a => a.id == c.customerid).FirstOrDefault().name,
                                      c.deleted,
                                      c.status,
                                      c.createAt,
                                      c.invoiceno,
                                      c.invoicepath,
                                      totalamt = (from jobwork in appDbContex.jobworkdetails
                                                 where jobwork.jobworkid == c.id && jobwork.deleted == false
                                                  select jobwork).Sum(e => (double?)e.totalamount) ?? 0,
                                      jobworkdetails = appDbContex.jobworkdetails.Where(j => j.jobworkid == c.id).Select(j => new
                                      {
                                          j.hour,
                                          j.perhourrate,
                                          j.totalamount,
                                          j.vehicleid,
                                          Vehiclename = appDbContex.vehicles.Where(a => a.id == j.vehicleid).FirstOrDefault().vehiclename,
                                          workdate = SqlFunctions.DateName("day", j.workdate).Trim() + "/" + SqlFunctions.StringConvert((double)j.workdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", j.workdate),
                                          j.discrition

                                      })






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
        [Route("deletejobwork")]
        public async Task<ResponseStatus> deletevehicle(string id)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {
                var jobworkdetail = appDbContex.jobworks.Where(a => a.id == id).SingleOrDefault();
                if (jobworkdetail != null)
                {
                    jobworkdetail.deleted = true;
                    // appDbContex.Categories.Update(category);
                    await appDbContex.SaveChangesAsync();

                    status.status = true;
                    status.message = "Job work details deleted Successfully!";
                    return status;

                }

                status.status = false;
                status.message = "Job work details not Exists!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
    }
}
