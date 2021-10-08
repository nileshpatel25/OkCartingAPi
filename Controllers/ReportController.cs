using CartingManagmentApi.Models;
using iText.Kernel.Colors;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.UI.WebControls;
namespace CartingManagmentApi.Controllers
{
    [RoutePrefix("api/report")]
    public class ReportController : ApiController
    {
        public ApplicationDbContext appDbContex { get; }
        public ReportController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        [HttpPost]
        [Route("jobworkreportbydate")]
        public async Task<ResponseStatus> jobworkreportwithdate(string userid, DateTime fromdate, DateTime todate)
        {
            try
            {

                ResponseStatus status = new ResponseStatus();
                fromdate = fromdate.AddDays(-1);
                todate = todate.AddDays(1);
                var sell = from c in appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid && a.createAt > fromdate.Date && a.createAt < todate.Date).OrderBy(a => a.createAt)
                           select new
                           {

                               c.id,
                               c.customerid,
                               c.paymenttype,
                               customername = appDbContex.customers.Where(a => a.id == c.customerid).FirstOrDefault().name,
                               c.deleted,
                               c.createAt,
                               c.invoiceno,
                               c.invoicepath,

                               jobworkdate = SqlFunctions.DateName("day", c.createAt).Trim() + "/" + SqlFunctions.StringConvert((double)c.createAt.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.createAt),

                               totalamt = (from jobwork in appDbContex.jobworkdetails
                                           where jobwork.jobworkid == c.id && jobwork.deleted == false
                                           select jobwork).Sum(e => (double?)e.totalamount) ?? 0,

                               jobworkdetail = appDbContex.jobworkdetails.Where(j => j.jobworkid == c.id).Select(j => new
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


                double? totalamount = 0;
                // GENERATE PDF 
                int pdfnumber = RandomNumber(1000, 50000);
                string path = HttpContext.Current.Server.MapPath("~/Invoice/" + pdfnumber + ".pdf");
                string pdfpath = "http://api.okcarting.com/Invoice/" + pdfnumber + ".pdf";
                var userinfo = appDbContex.Users.Where(a => a.Id == userid).FirstOrDefault();
                // var customerinfo = appDbContex.Customers.Where(a => a.id == sellMaster.customerid).FirstOrDefault();
                // Must have write permissions to the path folder
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(path);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);
                iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph(userinfo.compnayname)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFontSize(20);
                iText.Layout.Element.Paragraph newline = new iText.Layout.Element.Paragraph(new Text("\n"));
                iText.Layout.Element.Paragraph dateheader = new iText.Layout.Element.Paragraph("From Date: " + fromdate + " To Date: " + todate)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFontSize(20);

                float[] pointColumnWidths = { 10F, 125F, 125F, 125F, 125F };
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(pointColumnWidths);
                Cell cell11 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("Sr.No."));

                Cell cell12 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("Date"));


                Cell cell14 = new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("Name"));

                Cell cell15 = new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("Payment Type"));

                Cell cell17 = new Cell(1, 1)
                     .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new iText.Layout.Element.Paragraph("Amount"));
                table.AddCell(cell11);
                table.AddCell(cell12);
                table.AddCell(cell14);
                table.AddCell(cell15);
                table.AddCell(cell17);
                int i = 0;
                foreach (var ls in sell)
                {

                    cell11 = new Cell()
                          .SetTextAlignment(TextAlignment.CENTER)
                          .Add(new iText.Layout.Element.Paragraph((i + 1).ToString()));

                    cell12 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.jobworkdate.ToString()));

                    cell14 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.customername.ToString()));


                    cell15 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.paymenttype.ToString()));




                    cell17 = new Cell()
                      .SetTextAlignment(TextAlignment.RIGHT)
                      .Add(new iText.Layout.Element.Paragraph(ls.totalamt.ToString()));

                    table.AddCell(cell11);
                    table.AddCell(cell12);
                    table.AddCell(cell14);
                    table.AddCell(cell15);

                    table.AddCell(cell17);
                    i++;
                    totalamount += ls.totalamt;
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


                cell17 = new Cell()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new iText.Layout.Element.Paragraph(totalamount.ToString()));
                table.AddCell(cell11);
                table.AddCell(cell12);
                table.AddCell(cell14);
                table.AddCell(cell15);
                table.AddCell(cell17);

                document.Add(header);
                document.Add(newline);
                document.Add(dateheader);
                document.Add(table);
                //document.Add(headertable);
                //document.Add(newline);
                //// Adding paragraphs to document       
                ////document.Add(paragraph1);

                ////document.Add(paragraph2);
                //document.Add(table);
                //document.Add(newline);
                //document.Add(paragraph3);
                //document.Add(paragraph2);
                document.Close();
                //END




                status.invoicepath = pdfpath;
                status.lstItems = sell;
                status.status = true;
                status.objItem = totalamount;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }


        [HttpPost]
        [Route("receivedpaymentreportbydate")]
        public async Task<ResponseStatus> receivedpaymentreportbydatebyuserid(string userid, DateTime fromdate, DateTime todate)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                fromdate = fromdate.AddDays(-1);
                todate = todate.AddDays(1);
                var receivedamount = from c in appDbContex.customerpayments.Where(a => a.deleted == false && a.userid == userid && a.paymentdate > fromdate.Date && a.paymentdate < todate.Date).OrderBy(a => a.paymentdate)
                                     select new
                                     {

                                         c.id,
                                         c.userid,
                                         c.amount,
                                         c.chequeno,
                                         c.jobworkid,
                                         jobworkdetail = appDbContex.jobworkdetails.Where(a => a.jobworkid == c.jobworkid).ToList(),
                                         c.paymentby,
                                         paymentdate = SqlFunctions.DateName("day", c.paymentdate).Trim() + "/" + SqlFunctions.StringConvert((double)c.paymentdate.Month).TrimStart() + "/" + SqlFunctions.DateName("year", c.paymentdate),
                                         c.paymenttype,
                                         c.remark,
                                         c.customerid,
                                         customername = appDbContex.customers.Where(a => a.id == c.customerid).FirstOrDefault().name,
                                         c.deleted


                                     };

                double? totalamount = 0;
                // GENERATE PDF 
                int pdfnumber = RandomNumber(1000, 50000);
                string path = HttpContext.Current.Server.MapPath("~/Invoice/" + pdfnumber + ".pdf");
                string pdfpath = "http://api.okcarting.com/Invoice/" + pdfnumber + ".pdf";
                var userinfo = appDbContex.Users.Where(a => a.Id == userid).FirstOrDefault();
                // var customerinfo = appDbContex.Customers.Where(a => a.id == sellMaster.customerid).FirstOrDefault();
                // Must have write permissions to the path folder
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(path);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);
                iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph(userinfo.compnayname)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFontSize(20);
                iText.Layout.Element.Paragraph newline = new iText.Layout.Element.Paragraph(new Text("\n"));
                iText.Layout.Element.Paragraph dateheader = new iText.Layout.Element.Paragraph("From Date: " + fromdate + " To Date: " + todate)
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFontSize(20);

                float[] pointColumnWidths = { 10F, 125F, 125F, 125F, 125F };
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(pointColumnWidths);
                Cell cell11 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("CustomerName"));

                Cell cell12 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("Payment Date"));


                Cell cell14 = new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("PaymentBy"));

                Cell cell15 = new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("Chequeno"));

                Cell cell17 = new Cell(1, 1)
                     .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new iText.Layout.Element.Paragraph("Amount"));
                table.AddCell(cell11);
                table.AddCell(cell12);
                table.AddCell(cell14);
                table.AddCell(cell15);
                table.AddCell(cell17);
                int i = 0;
                foreach (var ls in receivedamount)
                {

                    cell11 = new Cell()
                          .SetTextAlignment(TextAlignment.CENTER)
                          .Add(new iText.Layout.Element.Paragraph(ls.customername.ToString()));

                    cell12 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.paymentdate.ToString()));

                    cell14 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.paymentby.ToString()));


                    cell15 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.chequeno.ToString()));




                    cell17 = new Cell()
                      .SetTextAlignment(TextAlignment.RIGHT)
                      .Add(new iText.Layout.Element.Paragraph(ls.amount.ToString()));

                    table.AddCell(cell11);
                    table.AddCell(cell12);
                    table.AddCell(cell14);
                    table.AddCell(cell15);

                    table.AddCell(cell17);
                    i++;
                    totalamount += ls.amount;
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


                cell17 = new Cell()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new iText.Layout.Element.Paragraph(totalamount.ToString()));
                table.AddCell(cell11);
                table.AddCell(cell12);
                table.AddCell(cell14);
                table.AddCell(cell15);
                table.AddCell(cell17);

                document.Add(header);
                document.Add(newline);
                document.Add(dateheader);
                document.Add(table);
                //document.Add(headertable);
                //document.Add(newline);
                //// Adding paragraphs to document       
                ////document.Add(paragraph1);

                ////document.Add(paragraph2);
                //document.Add(table);
                //document.Add(newline);
                //document.Add(paragraph3);
                //document.Add(paragraph2);
                document.Close();
                //END
                status.lstItems = receivedamount;
                status.invoicepath = pdfpath;
                status.status = true;
                return status;

            }

            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("customerpendingamountreportbyuserid")]
        public async Task<ResponseStatus> customerjobworkdetailsofpendingpayment(string userid)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();


                var pendingamount = from a in appDbContex.jobworks.Where(a => a.deleted == false && a.userid == userid)
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

                double? totalamount = 0;
                double? totlareceiveamount = 0;
                double? totalpendingamount = 0;
                // GENERATE PDF 
                int pdfnumber = RandomNumber(1000, 50000);
                string path = HttpContext.Current.Server.MapPath("~/Invoice/" + pdfnumber + ".pdf");
                string pdfpath = "http://api.okcarting.com/Invoice/" + pdfnumber + ".pdf";
                var userinfo = appDbContex.Users.Where(a => a.Id == userid).FirstOrDefault();
                // var customerinfo = appDbContex.Customers.Where(a => a.id == sellMaster.customerid).FirstOrDefault();
                // Must have write permissions to the path folder
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(path);
                iText.Kernel.Pdf.PdfDocument pdf = new iText.Kernel.Pdf.PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);
                iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph(userinfo.compnayname)
                   .SetTextAlignment(TextAlignment.LEFT)
                   .SetFontSize(20);
                iText.Layout.Element.Paragraph newline = new iText.Layout.Element.Paragraph(new Text("\n"));
                iText.Layout.Element.Paragraph dateheader = new iText.Layout.Element.Paragraph("")
                        .SetTextAlignment(TextAlignment.LEFT)
                        .SetFontSize(20);

                float[] pointColumnWidths = { 10F, 125F, 125F, 125F, 125F };
                iText.Layout.Element.Table table = new iText.Layout.Element.Table(pointColumnWidths);
                Cell cell11 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("CustomerName"));

                Cell cell12 = new Cell(1, 1)
                   .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("Date"));


                Cell cell14 = new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("TotalAmt"));

                Cell cell15 = new Cell(1, 1)
                    .SetBackgroundColor(ColorConstants.GRAY)
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph("ReceivedAmt"));

                Cell cell17 = new Cell(1, 1)
                     .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new iText.Layout.Element.Paragraph("PendingAmt"));
                table.AddCell(cell11);
                table.AddCell(cell12);
                table.AddCell(cell14);
                table.AddCell(cell15);
                table.AddCell(cell17);
                int i = 0;
                foreach (var ls in pendingamount)
                {
                    var date = "";
                    foreach (var lst in ls.jobworkdetails)
                    {
                        date += lst.workdate.ToString();
                    }

                    cell11 = new Cell()
                          .SetTextAlignment(TextAlignment.CENTER)
                          .Add(new iText.Layout.Element.Paragraph(ls.customername.ToString()));

                    cell12 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(date.ToString()));

                    cell14 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.totalamt.ToString()));


                    cell15 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph(ls.receivedamt.ToString()));




                    cell17 = new Cell()
                      .SetTextAlignment(TextAlignment.RIGHT)
                      .Add(new iText.Layout.Element.Paragraph(ls.pendingamt.ToString()));

                    table.AddCell(cell11);
                    table.AddCell(cell12);
                    table.AddCell(cell14);
                    table.AddCell(cell15);

                    table.AddCell(cell17);
                    i++;
                    totalamount += ls.totalamt;
                    totlareceiveamount += ls.receivedamt;
                    totalpendingamount += ls.pendingamt;
                }
                cell11 = new Cell()
                       .SetTextAlignment(TextAlignment.CENTER)
                       .Add(new iText.Layout.Element.Paragraph());

                cell12 = new Cell()
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph());

                cell14 = new Cell()
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph(totalamount.ToString()));


                cell15 = new Cell()
                   .SetTextAlignment(TextAlignment.CENTER)
                   .Add(new iText.Layout.Element.Paragraph(totlareceiveamount.ToString()));


                cell17 = new Cell()
                    .SetTextAlignment(TextAlignment.RIGHT)
                    .Add(new iText.Layout.Element.Paragraph(totalpendingamount.ToString()));
                table.AddCell(cell11);
                table.AddCell(cell12);
                table.AddCell(cell14);
                table.AddCell(cell15);
                table.AddCell(cell17);

                document.Add(header);
                document.Add(newline);
                document.Add(dateheader);
                document.Add(table);
                //document.Add(headertable);
                //document.Add(newline);
                //// Adding paragraphs to document       
                ////document.Add(paragraph1);

                ////document.Add(paragraph2);
                //document.Add(table);
                //document.Add(newline);
                //document.Add(paragraph3);
                //document.Add(paragraph2);
                document.Close();
                //END
                status.lstItems = pendingamount;
                status.invoicepath = pdfpath;
                status.status = true;
                return status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        [Route("allfuellistbyreportuserid")]
        public async Task<ResponseStatus> getAllfuelListbyuseris(string userid, DateTime fromdate, DateTime todate)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                fromdate = fromdate.AddDays(-1);
                todate = todate.AddDays(1);

                status.lstItems = from c in appDbContex.fuelmasters.Where(a => a.deleted == false && a.userid == userid && a.fueldate > fromdate.Date && a.fueldate < todate.Date).OrderBy(a => a.fueldate)
                                  select new
                                  {

                                      c.id,
                                      c.userid,
                                      c.vehicleid,
                                      Vehiclename = appDbContex.vehicles.Where(v => v.id == c.vehicleid).FirstOrDefault().vehiclename + " - " + appDbContex.vehicles.Where(v => v.id == c.vehicleid).FirstOrDefault().vehiclenumber,
                                      c.totalamount,
                                      c.liter,
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
        [Route("hiringhistorybetweendate")]
        public async Task<ResponseStatus> hiringhistorybetweendates(string userid, DateTime fromdate, DateTime todate)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                fromdate = fromdate.AddDays(-1);
                todate = todate.AddDays(1);
                var vehichlehistory = (from JW in appDbContex.jobworks.Where(a => a.deleted == false)
                                       join JWD in appDbContex.jobworkdetails.Where(b=> b.workdate > fromdate.Date && b.workdate < todate.Date) on JW.id equals JWD.jobworkid 
                                       join C in appDbContex.customers on JW.customerid equals C.id into C from x in C.DefaultIfEmpty()
                                       join V in appDbContex.vehicles on JWD.vehicleid equals V.id into V from y in V.DefaultIfEmpty()
                                       join D in appDbContex.drivers on JWD.driverid equals D.id into D from z in D.DefaultIfEmpty()
                                       orderby JWD.jobworkid
                    select new
                    {
                        JW.userid,
                        JW.customerid,
                        JWD.vehicleid,
                        JWD.hour,
                        JWD.perhourrate,
                        JWD.workdate,
                        JWD.totalamount,
                        x.name,
                        y.vehiclenumber
                        ,drivername = z.name
                    }).ToList();
                status.lstItems = vehichlehistory;
                status.status = true;
                // status.objItem = totalamount; 
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [HttpPost]
        [Route("vehiclehistorybetweendate")]
        public async Task<ResponseStatus> vehiclehistorybetweendates(string userid, DateTime fromdate, DateTime todate)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                fromdate = fromdate.AddDays(-1);
                todate = todate.AddDays(1);              
                var vehichlehistory = (from JWD in appDbContex.jobworkdetails.Where(a => a.deleted == false && a.workdate > fromdate.Date && a.workdate < todate.Date)
                                     
                                       join V in appDbContex.vehicles on JWD.vehicleid equals V.id
                                       //group V.vehiclenumber                                      
                                       select new
                                       {   
                                           JWD.hour,
                                           totalamt = (from jobwork in appDbContex.jobworkdetails
                                                       where jobwork.vehicleid == V.id && jobwork.deleted == false
                                                       select jobwork).Sum(e => (double?)e.hour) ?? 0,
                                           V.vehiclenumber                                           
                                       }).ToList();
                status.lstItems = vehichlehistory;
                status.status = true;
                // status.objItem = totalamount; 
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Route("fueldetailbetweendate")]
        public async Task<ResponseStatus> fueldetailbetweendates(string userid, DateTime fromdate, DateTime todate)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                fromdate = fromdate.AddDays(-1);
                todate = todate.AddDays(1);
                var vehichlehistory = (from FM in appDbContex.fuelmasters.Where(a => a.deleted == false && a.fueldate > fromdate.Date && a.fueldate < todate.Date)
                                       join P in appDbContex.Petrolpumps on FM.petrolpumpid equals P.id into P
                                       from X in P.DefaultIfEmpty()
                                       join D in appDbContex.drivers on FM.driverid equals D.id into D
                                       from z in D.DefaultIfEmpty()                                    
                                       select new
                                       {
                                           FM.fueldate,
                                           X.name,
                                           FM.rate,
                                           FM.liter,
                                           FM.totalamount,
                                           drivername = z.name
                                       }).ToList();
                status.lstItems = vehichlehistory;
                status.status = true;
                // status.objItem = totalamount; 
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Route("expensedetailbetweendate")]
        public async Task<ResponseStatus> expensedetailbetweendates(string userid, DateTime fromdate, DateTime todate)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                fromdate = fromdate.AddDays(-1);
                todate = todate.AddDays(1);
              
                var vehichlehistory = (from ET in appDbContex.expensetypes
                                       join ED in appDbContex.expensedetails.Where(a => a.deleted == false && a.expensedate > fromdate.Date && a.expensedate < todate.Date) on ET.id equals ED.expensetypeid
                                      
                                      
                                       select new
                                       {
                                          ET.name,
                                           ED.amount,
                                           ED.expensedate
                                       }).ToList();
                status.lstItems = vehichlehistory;
                status.status = true;
                // status.objItem = totalamount; 
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}


//ALTER PROC[dbo].[GetDriverHourBetweenDates]
//(
//@userid varchar(50),
//@fromdate DATETIME,
//@TODATE DATETIME
//)
//AS
//BEGIN
//SET NOCOUNT ON;

//SELECT SUM(JWD.hour) as totalhour,V.vehiclenumber FROM jobworkdetails As JWD
//LEFT JOIN vehicles as V ON V.id = JWD.vehicleid
//group by v.vehiclenumber

//END
//USE [CartingManagement]
//GO
///****** Object:  StoredProcedure [dbo].[GetExpensehistoryBetweenDates]    Script Date: 10/5/2021 11:48:44 AM ******/
//SET ANSI_NULLS ON
//GO
//SET QUOTED_IDENTIFIER ON
//GO
//ALTER PROC [dbo].[GetExpensehistoryBetweenDates]
//(
//@userid varchar(50),
//@fromdate DATETIME,
//@TODATE DATETIME
//)
//AS
//BEGIN
//SET NOCOUNT ON;

//SELECT ET.name, ED.amount, ED.expensedate FROM expensetypes As ET
//LEFT JOIN expensedetails as ED ON ET.id = ED.expensetypeid


//END

//USE [CartingManagement]
//GO
///****** Object:  StoredProcedure [dbo].[GetVehiclehistoryBetweenDates]    Script Date: 10/5/2021 11:49:14 AM ******/
//SET ANSI_NULLS ON
//GO
//SET QUOTED_IDENTIFIER ON
//GO
//ALTER PROC[dbo].[GetVehiclehistoryBetweenDates]
//(
//@userid varchar(50),
//@fromdate DATETIME,
//@TODATE DATETIME
//)
//AS
//BEGIN
//SET NOCOUNT ON;

//SELECT SUM(JWD.hour) as totalhour,V.vehiclenumber FROM jobworkdetails As JWD
//LEFT JOIN vehicles as V ON V.id = JWD.vehicleid
//group by v.vehiclenumber

//END