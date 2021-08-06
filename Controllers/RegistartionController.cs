using CartingManagmentApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using CartingManagmentApi.Helper;
using static CartingManagmentApi.ApplicationUserManager;
using System.IO;

namespace CartingManagmentApi.Controllers
{
    [RoutePrefix("api/cartingregistartion")]
    public class RegistartionController : ApiController
    {
      //  SendSMS sendSMS = new SendSMS();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;
        // private readonly IMemoryCache memoryCache;
        sendsms sendSMS = new sendsms();
        public ApplicationDbContext appDbContex { get; }
        public RegistartionController()
        {
            this.appDbContex = new ApplicationDbContext();
            // this.memoryCache = memoryCache;
        }
        public RegistartionController(ApplicationUserManager userManager, ApplicationRoleManager roleManager,
         ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        [HttpGet]
        [Route("getAllUserList")]
        public async Task<ResponseStatus> GetAllUSer()
        {
            try
            {
                ResponseStatus status = new ResponseStatus();

                var alluser = UserManager.Users.OrderByDescending(a => a.joiningdate).ToList();
                status.lstItems = alluser;
                // status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [HttpPost]
        [Route("getAllUser")]
        public async Task<ResponseStatus> GetAllUSer(int pageNo, int pageSize)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                int count = UserManager.Users.ToList().Count();
                int skip = (pageNo - 1) * pageSize;
                var alluser = UserManager.Users.OrderByDescending(a => a.joiningdate).Skip(skip).Take(pageSize).ToList();
                status.lstItems = alluser;
                status.objItem = count;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpPost]
        [Route("getUserinfo")]
        public async Task<ResponseStatus> GetAllUSerinfo(string id)
        {
            try
            {
                ResponseStatus status = new ResponseStatus();
                var alluser = UserManager.Users.Where(a => a.Id == id).ToList();
                status.lstItems = alluser;
                status.status = true;
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }




        [HttpPost]
        [Route("register")]

        public async Task<ResponseStatus> Register(RegisterRequest registerRequest)
        {
            ResponseStatus status = new ResponseStatus();


            //var user = new Applicationuser
            //{
            //    UserName = registerRequest.phonenumber,
            //    PhoneNumber = registerRequest.phonenumber,
            //    Email = registerRequest.email             

            //};
            var user = new ApplicationUser() { UserName = registerRequest.phonenumber, name = registerRequest.name, Email = registerRequest.email, PhoneNumber = registerRequest.phonenumber, source = registerRequest.source ,joiningdate=DateTime.UtcNow};
            var result = await UserManager.CreateAsync(user, registerRequest.password);
            var role = await RoleManager.FindByNameAsync("User");
            if (result.Succeeded)
            {
                LoginStatus loginStatus = new LoginStatus
                {
                    id = Guid.NewGuid().ToString(),
                    userid = user.Id,
                    registrationdate = DateTime.UtcNow,
                    subscriptionenddate = DateTime.UtcNow,
                    subscriptionstartdate = DateTime.UtcNow,
                    deleted = false
                };
                appDbContex.LoginStatuses.Add(loginStatus);
                await appDbContex.SaveChangesAsync();
                // SendEmail sendEmail = new SendEmail();
                // sendEmail.sendemail(registerRequest.Email);
                // await signInManager.SignInAsync(user, isPersistent: false);
                IdentityResult roleresult = null;
                roleresult = await UserManager.AddToRoleAsync(user.Id, role.Name);
                // sendSMS.SendTextSms("Thank you for registartion with us. Green Shop", "+91" + registerRequest.phonenumber);
                status.message = "Registration successfully!";
                status.status = true;
                return status;
            }
            List<string> errors = new List<string>();
            foreach (var error in result.Errors)
            {
                errors.Add(error.ToString());
            }
            status.status = false;
            status.message = errors[0].ToString();
            return status;
            //  return BadRequest(new { message = errors });

        }

        [HttpPost]
        [Route("profileupdate")]
        public async Task<ResponseStatus> profileupdate(profileRequest profileRequest)
        {
            ResponseStatus status = new ResponseStatus();
            try
            {

                var user = UserManager.FindById(profileRequest.id);
                //  var guId = Guid.NewGuid();

                user.Id = profileRequest.id;
                user.name = profileRequest.name;
                user.compnayname = profileRequest.compnayname;
                user.gstin = profileRequest.gstin;
                user.address = profileRequest.address;
                user.address2 = profileRequest.address2;
                user.latitude = profileRequest.latitude;
                user.logitude = profileRequest.logitude;
                user.pushTokenId = profileRequest.pushTokenId;
                user.landmark = profileRequest.landmark;
                user.city = profileRequest.city;
                user.pincode = profileRequest.pincode;
                user.othercontactno = profileRequest.othercontactno;
                user.discription = profileRequest.discription;

                // memoryCache.Remove("citylist");
                //   appDbContex.Addresses.Add(addresses);
                await UserManager.UpdateAsync(user);

                status.status = true;
                status.message = "Profile updated successfully!";

                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("uploadprofilepic")]
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
                ApplicationUser user = appDbContex.Users.Where(a => a.Id == cid).SingleOrDefault();
                if (user != null)
                {

                    user.profilepic = "http://api.okcarting.com/Images/" + filename;
                    await appDbContex.SaveChangesAsync();
                }

                status.status = true;
                status.message = "Profile pic updated successfully";
                // status.filePath = "http://api.clickperfect.me/Images/" + filename;


            }
            catch (Exception ex)
            {
                status.status = false;
                status.message = ex.ToString();
            }
            return status;
        }



        [AllowAnonymous]
        [HttpPost]
        [Route("createrole")]
        public async Task<ResponseStatus> CreateRole(createrolerequest createRoleRequest)
        {
            ResponseStatus status = new ResponseStatus();
            IdentityRole role = new IdentityRole(createRoleRequest.RoleName);
            IdentityResult result = await RoleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                status.status = true;
                status.message = "Role created successfully!";
                return status;

            }
            status.status = false;
            status.message = "role already exists!";
            return status;

        }

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        [HttpPost]
        [Route("sendotp")]
        public async Task<ResponseStatus> sendotp(string mobileno)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                int otpvalue = RandomNumber(100000, 999999);
                sendSMS.SendTextSms("Your OTP Number is " + otpvalue + ", Ok Carting.", "91" + mobileno);
                System.Web.HttpContext.Current.Session["OTP"] = otpvalue.ToString();
                System.Web.HttpContext.Current.Session["MOBILENO"] = mobileno.ToString();
                responseStatus.message = "OTP sent sucsessfully.";
                responseStatus.status = true;
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("verifyotp")]
        public async Task<ResponseStatus> verifyotp(string otpvalue)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                if(System.Web.HttpContext.Current.Session["OTP"]!=null)
                {

               
                if (System.Web.HttpContext.Current.Session["OTP"].ToString() == otpvalue.ToString())
                {
                    responseStatus.status = true;
                    responseStatus.message = "OTP verify successfully";

                }
                else
                {
                    responseStatus.status = false;
                    responseStatus.message = "Invalid OTP";
                }
                }
                else
                {
                    responseStatus.status = false;
                    responseStatus.message = "Invalid OTP";
                }
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        [HttpPost]
        [Route("resetpassword")]
        public async Task<ResponseStatus> resetpassword(string password)
        {
            try
            {
                ResponseStatus responseStatus = new ResponseStatus();
                var user = await UserManager.FindByNameAsync(HttpContext.Current.Session["MOBILENO"].ToString());
                if (user != null)
                {

                    //IdentityResult result = await UserManager.AddPasswordAsync(user.Id, password);

                    //if (!result.Succeeded)
                    //{

                    // }

                    UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(appDbContex);
                    UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);
                    //String hashedNewPassword = UserManager.PasswordHasher.HashPassword(password);
                    //await store.SetPasswordHashAsync(user, hashedNewPassword);
                    //await store.UpdateAsync(user);

                    String hashedNewPassword = UserManager.PasswordHasher.HashPassword(password);
                    ApplicationUser cUser = await store.FindByIdAsync(user.Id);
                    await store.SetPasswordHashAsync(cUser, hashedNewPassword);
                    await store.UpdateAsync(cUser);

                    responseStatus.status = true;
                    responseStatus.message = "Password reset sucessfully";
                }
                return responseStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

    }
}
