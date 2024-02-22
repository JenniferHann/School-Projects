using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NoNameBikes.Models;
using System.Net;
using Newtonsoft.Json.Linq;
using NoNameBikes.ReCaptchaV2;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Extensions;

namespace NoNameBikes.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public CustomerController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        // GET: Customer
        public async Task<IActionResult> Index()
        {
            return View(await db.Customer.ToListAsync());
        }

        // GET: Customer/Details/5
        public IActionResult Details()
        {
            String cookie = "";
            if (HttpContext.Request.Cookies.ContainsKey("CurrentLoggedId"))
                cookie = HttpContext.Request.Cookies["CurrentLoggedId"];

            int id = Int32.Parse(cookie);

            IEnumerable<Customer> customer = from tmpCustomer in db.Customer
                                             where tmpCustomer.CustomerId == id
                                             select tmpCustomer;
            return View(customer.First());
        }

        // GET: Customer/LogIn
        public IActionResult LogIn()
        {
            return View();
        }

        // POST: Customer/LogIn
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("EmailAddress,Password")] Customer customer)
        {
            string captchaResponse = Request.Form["g-Recaptcha-Response"];
            ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);
            if (!result.Success)
            {
                ViewData["captchaFailure"] = true;
                return View();
            }

            IEnumerable<Customer> validEmail = (from tmpCustomer in db.Customer
                                               where tmpCustomer.EmailAddress == customer.EmailAddress
                                               select tmpCustomer);

            if(validEmail.Count() == 0)
            {
                ViewData["popup"] = true;
                return View();
            }
            Customer found = validEmail.First();
            byte[] salt = Convert.FromBase64String(found.PasswordSalt);

            string saltAndPwd = string.Concat(customer.Password, salt);
            byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

            HashAlgorithm algo = new SHA256Managed();
            byte[] hashed = algo.ComputeHash(allBytes);

            if (Convert.ToBase64String(hashed) != found.PasswordHash)
            {
                ViewData["popup"] = true;
                return View();
            }

            DateTime now = DateTime.Now;
            LogIn login = new LogIn();
            login.CustomerId = found.CustomerId;
            login.TimeStamp = now;

            db.LogIn.Add(login);
            await db.SaveChangesAsync();

            HttpContext.Response.Cookies.Append("LoggedIn", "true");
            HttpContext.Response.Cookies.Append("CurrentLoggedId", found.CustomerId.ToString());
            return RedirectToAction("Index", "Bikes");
        }

        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Append("LoggedIn", "false");
            HttpContext.Response.Cookies.Append("CurrentLoggedId", "-1");
            return RedirectToAction("Index", "Bikes");
        }

        // GET: Customer/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,FirstName,MiddleName,LastName,Suffix,CompanyName,EmailAddress,Phone,Password,ConfirmPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                string captchaResponse = Request.Form["g-Recaptcha-Response"];
                ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);
                if (!result.Success)
                {
                    ViewData["captchaFailure"] = true;
                    return View();
                }
                    
                IEnumerable<Customer> usedEmail = (from tmpCustomer in db.Customer
                                                   where tmpCustomer.EmailAddress == customer.EmailAddress
                                                   select tmpCustomer);

                if(usedEmail.Count() != 0)
                {
                    ViewData["emailUsed"] = true;
                    return View();
                }

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] salt = new byte[4];
                rng.GetBytes(salt);

                string saltAndPwd = string.Concat(customer.Password, salt);
                byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

                HashAlgorithm algo = new SHA256Managed();
                byte[] hashed = algo.ComputeHash(allBytes);

                customer.PasswordHash = Convert.ToBase64String(hashed);
                customer.PasswordSalt = Convert.ToBase64String(salt);
                customer.ModifiedDate = DateTime.Now;

                if(sendEmail(customer, "Account Created", "Your account has been created. Have fun!"))
                {
                    db.Add(customer);
                    await db.SaveChangesAsync(); 
                    return View("/Views/Customer/RegistrationSuccess.cshtml", customer);
                }                 
            }
            ViewData["InvalidInfo"] = true;
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        public async Task<IActionResult> PasswordReset(string email)
        { 
            IEnumerable<Customer> validEmail = (from tmpCustomer in db.Customer
                                                where tmpCustomer.EmailAddress == email
                                                select tmpCustomer);

            if (validEmail.Count() == 0)
            {
                return View("/Views/Customer/Failure.cshtml");
            }
            Customer found = validEmail.First();

            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Customer/NewPassword/" + resetCode;
            var link = Request.GetEncodedUrl().Replace(Request.GetEncodedPathAndQuery(), verifyUrl);

            string topic = "Password Reset";
            string msg = "<a href=\""+link+"\">reset password</a>";
            bool sent = sendEmail(found, topic, msg, true);

            PasswordRecoveryToken PRT = new PasswordRecoveryToken();
            PRT.CreationDate = DateTime.Now;
            PRT.Token = resetCode;

            db.PasswordRecoveryToken.Add(PRT);
            await db.SaveChangesAsync();

            if (sent)
            {
                ViewData["email"] = email;
                return View("/Views/Customer/ResetEmailSent.cshtml");
            }      
 
            return View("/Views/Customer/Failure.cshtml");
        }

        public async Task<IActionResult> SaveNewPassword(string email, string pwd)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[4];
            rng.GetBytes(salt);

            string saltAndPwd = string.Concat(pwd, salt);
            byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

            HashAlgorithm algo = new SHA256Managed();
            byte[] hashed = algo.ComputeHash(allBytes);

            string pwdHash = Convert.ToBase64String(hashed);
            string pwdSalt = Convert.ToBase64String(salt);

            var customerToUpdate = db.Customer.FirstOrDefault(c => c.EmailAddress == email);

            if(customerToUpdate != null)
            {
                customerToUpdate.PasswordHash = pwdHash;
                customerToUpdate.PasswordSalt = pwdSalt;
            }

            db.Customer.Update(customerToUpdate);

            await db.SaveChangesAsync();

            return View("/Views/Customer/NewPasswordSaved.cshtml", customerToUpdate);
        }

        public async Task<IActionResult> NewPassword()
        {
            string link = Request.GetEncodedUrl();
            string[] values = link.Split("/");

            IEnumerable<PasswordRecoveryToken> tokens = (from t in db.PasswordRecoveryToken
                                                         where t.Token == values.Last()
                                                         select t);

            PasswordRecoveryToken selected = tokens.First();

            if (selected.Expired == true)
                return View("/Views/Customer/InvalidLink.cshtml");

            selected.Expired = true;
            db.PasswordRecoveryToken.Update(selected);
            await db.SaveChangesAsync();

            return View();
        }

        private bool sendEmail(Customer customer, string topic, string msg, bool html=false)
        {
            bool success = false;

            try
            {
                //Send the email
                var messageObj = new MimeMessage();
                messageObj.From.Add(new MailboxAddress("NoNameBikes", "nonamebikes2020@gmail.com"));
                messageObj.To.Add(new MailboxAddress(customer.FirstName+" "+customer.LastName, customer.EmailAddress));
                messageObj.To.Add(new MailboxAddress("NoName Bikes", "nonamebikes2020@gmail.com"));

                messageObj.Subject = topic;
                if(html)
                {
                    messageObj.Body = new TextPart("html")
                    {
                        Text = msg
                    };
                }
                else
                {
                    messageObj.Body = new TextPart("Plain")
                    {
                        Text = msg
                    };
                }
                
                using var client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("nonamebikes2020@gmail.com", "JacCs2020");
                client.Send(messageObj);
                client.Disconnect(true);
                success = true;

            }
            catch (Exception e)
            {
                success = false;
            }

            return success;
        }

        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await db.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,NameStyle,Title,FirstName,MiddleName,LastName,Suffix,CompanyName,SalesPerson,EmailAddress,Phone,PasswordHash,PasswordSalt,Rowguid,ModifiedDate")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(customer);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await db.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await db.Customer.FindAsync(id);
            db.Customer.Remove(customer);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return db.Customer.Any(e => e.CustomerId == id);
        }

        // GET: Customer/Details/5
        public IActionResult DetailsFR()
        {
            String cookie = "";
            if (HttpContext.Request.Cookies.ContainsKey("CurrentLoggedId"))
                cookie = HttpContext.Request.Cookies["CurrentLoggedId"];

            int id = Int32.Parse(cookie);

            IEnumerable<Customer> customer = from tmpCustomer in db.Customer
                                             where tmpCustomer.CustomerId == id
                                             select tmpCustomer;
            return View(customer.First());
        }

        // GET: Customer/LogIn
        public IActionResult LogInFR()
        {
            return View();
        }

        // POST: Customer/LogIn
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInFR([Bind("EmailAddress,Password")] Customer customer)
        {
            string captchaResponse = Request.Form["g-Recaptcha-Response"];
            ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);
            if (!result.Success)
            {
                ViewData["captchaFailure"] = true;
                return View();
            }

            IEnumerable<Customer> validEmail = (from tmpCustomer in db.Customer
                                                where tmpCustomer.EmailAddress == customer.EmailAddress
                                                select tmpCustomer);

            if (validEmail.Count() == 0)
            {
                ViewData["popup"] = true;
                return View();
            }
            Customer found = validEmail.First();
            byte[] salt = Convert.FromBase64String(found.PasswordSalt);

            string saltAndPwd = string.Concat(customer.Password, salt);
            byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

            HashAlgorithm algo = new SHA256Managed();
            byte[] hashed = algo.ComputeHash(allBytes);

            if (Convert.ToBase64String(hashed) != found.PasswordHash)
            {
                ViewData["popup"] = true;
                return View();
            }

            DateTime now = DateTime.Now;
            LogIn login = new LogIn();
            login.CustomerId = found.CustomerId;
            login.TimeStamp = now;

            db.LogIn.Add(login);
            await db.SaveChangesAsync();

            HttpContext.Response.Cookies.Append("LoggedIn", "true");
            HttpContext.Response.Cookies.Append("CurrentLoggedId", found.CustomerId.ToString());
            return RedirectToAction("IndexFR", "Bikes");
        }
        public IActionResult LogOutFR()
        {
            HttpContext.Response.Cookies.Append("LoggedIn", "false");
            HttpContext.Response.Cookies.Append("CurrentLoggedId", "-1");
            return RedirectToAction("IndexFR", "Bikes");
        }

        // GET: Customer/Create
        public IActionResult CreateFR()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFR([Bind("Title,FirstName,MiddleName,LastName,Suffix,CompanyName,EmailAddress,Phone,Password,ConfirmPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                string captchaResponse = Request.Form["g-Recaptcha-Response"];
                ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);
                if (!result.Success)
                {
                    ViewData["captchaFailure"] = true;
                    return View();
                }

                IEnumerable<Customer> usedEmail = (from tmpCustomer in db.Customer
                                                   where tmpCustomer.EmailAddress == customer.EmailAddress
                                                   select tmpCustomer);

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] salt = new byte[4];
                rng.GetBytes(salt);

                string saltAndPwd = string.Concat(customer.Password, salt);
                byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

                HashAlgorithm algo = new SHA256Managed();
                byte[] hashed = algo.ComputeHash(allBytes);

                customer.PasswordHash = Convert.ToBase64String(hashed);
                customer.PasswordSalt = Convert.ToBase64String(salt);
                customer.ModifiedDate = DateTime.Now;

                if (sendEmail(customer, "Account Created", "Your account has been created. Have fun!"))
                {
                    db.Add(customer);
                    await db.SaveChangesAsync();
                    return View("/Views/Customer/RegistrationSuccessFR.cshtml", customer);
                }
            }
            ViewData["InvalidInfo"] = true;
            return View();
        }

        public IActionResult ForgotPasswordFR()
        {
            return View();
        }

        public async Task<IActionResult> PasswordResetFR(string email)
        {
            IEnumerable<Customer> validEmail = (from tmpCustomer in db.Customer
                                                where tmpCustomer.EmailAddress == email
                                                select tmpCustomer);

            if (validEmail.Count() == 0)
            {
                return View("/Views/Customer/FailureFR.cshtml");
            }
            Customer found = validEmail.First();

            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Customer/NewPasswordFR/" + resetCode;
            var link = Request.GetEncodedUrl().Replace(Request.GetEncodedPathAndQuery(), verifyUrl);

            string topic = "Mot de Pass réinitialisation";
            string msg = "<a href=\"" + link + "\">réinitialiser le mot de passe</a>";
            bool sent = sendEmail(found, topic, msg, true);

            PasswordRecoveryToken PRT = new PasswordRecoveryToken();
            PRT.CreationDate = DateTime.Now;
            PRT.Token = resetCode;

            db.PasswordRecoveryToken.Add(PRT);
            await db.SaveChangesAsync();

            if (sent)
            {
                ViewData["email"] = email;
                return View("/Views/Customer/ResetEmailSentFR.cshtml");
            }

            return View("/Views/Customer/FailureFR.cshtml");
        }

        public async Task<IActionResult> SaveNewPasswordFR(string email, string pwd)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[4];
            rng.GetBytes(salt);

            string saltAndPwd = string.Concat(pwd, salt);
            byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

            HashAlgorithm algo = new SHA256Managed();
            byte[] hashed = algo.ComputeHash(allBytes);

            string pwdHash = Convert.ToBase64String(hashed);
            string pwdSalt = Convert.ToBase64String(salt);

            var customerToUpdate = db.Customer.FirstOrDefault(c => c.EmailAddress == email);

            if (customerToUpdate != null)
            {
                customerToUpdate.PasswordHash = pwdHash;
                customerToUpdate.PasswordSalt = pwdSalt;
            }

            db.Customer.Update(customerToUpdate);

            await db.SaveChangesAsync();

            return View("/Views/Customer/NewPasswordSavedFR.cshtml", customerToUpdate);
        }

        public async Task<IActionResult> NewPasswordFR()
        {
            string link = Request.GetEncodedUrl();
            string[] values = link.Split("/");

            IEnumerable<PasswordRecoveryToken> tokens = (from t in db.PasswordRecoveryToken
                                                         where t.Token == values.Last()
                                                         select t);

            PasswordRecoveryToken selected = tokens.First();

            if (selected.Expired == true)
                return View("/Views/Customer/InvalidLinkFR.cshtml");

            selected.Expired = true;
            db.PasswordRecoveryToken.Update(selected);
            await db.SaveChangesAsync();

            return View();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// FRENCH VERSION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /*public async Task<IActionResult> IndexFR()
        {
            return View(await db.Customer.ToListAsync());
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> DetailsFR()
        {
            String cookie = "";
            if (HttpContext.Request.Cookies.ContainsKey("CurrentLoggedId"))
                cookie = HttpContext.Request.Cookies["CurrentLoggedId"];

            int id = Int32.Parse(cookie);

            IEnumerable<Customer> customer = from tmpCustomer in db.Customer
                                             where tmpCustomer.CustomerId == id
                                             select tmpCustomer;
            return View(customer.First());
        }

        // GET: Customer/LogIn
        public IActionResult LogInFR()
        {
            return View();
        }

        // POST: Customer/LogIn
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogInFR([Bind("EmailAddress,Password")] Customer customer)
        {
            string captchaResponse = Request.Form["g-Recaptcha-Response"];
            ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);
            if (!result.Success)
            {
                ViewData["captchaFailure"] = true;
                return View();
            }

            IEnumerable<Customer> validEmail = (from tmpCustomer in db.Customer
                                                where tmpCustomer.EmailAddress == customer.EmailAddress
                                                select tmpCustomer);

            if (validEmail.Count() == 0)
            {
                ViewData["popup"] = true;
                return View();
            }
            Customer found = validEmail.First();
            byte[] salt = Convert.FromBase64String(found.PasswordSalt);

            string saltAndPwd = string.Concat(customer.Password, salt);
            byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

            HashAlgorithm algo = new SHA256Managed();
            byte[] hashed = algo.ComputeHash(allBytes);

            if (Convert.ToBase64String(hashed) != found.PasswordHash)
            {
                ViewData["popup"] = true;
                return View();
            }

            HttpContext.Response.Cookies.Append("LoggedIn", "true");
            HttpContext.Response.Cookies.Append("CurrentLoggedId", found.CustomerId.ToString());
            return RedirectToAction("Index", "Bikes");
        }
        public IActionResult LogOutFR()
        {
            HttpContext.Response.Cookies.Append("LoggedIn", "false");
            HttpContext.Response.Cookies.Append("CurrentLoggedId", "-1");
            return RedirectToAction("Index", "Bikes");
        }

        // GET: Customer/Create
        public IActionResult CreateFR()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFR([Bind("Title,FirstName,MiddleName,LastName,Suffix,CompanyName,EmailAddress,Phone,Password,ConfirmPassword")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                string captchaResponse = Request.Form["g-Recaptcha-Response"];
                ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);
                if (!result.Success)
                {
                    ViewData["captchaFailure"] = true;
                    return View();
                }

                IEnumerable<Customer> usedEmail = (from tmpCustomer in db.Customer
                                                   where tmpCustomer.EmailAddress == customer.EmailAddress
                                                   select tmpCustomer);

                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] salt = new byte[4];
                rng.GetBytes(salt);

                string saltAndPwd = string.Concat(customer.Password, salt);
                byte[] allBytes = Encoding.UTF8.GetBytes(saltAndPwd);

                HashAlgorithm algo = new SHA256Managed();
                byte[] hashed = algo.ComputeHash(allBytes);

                customer.PasswordHash = Convert.ToBase64String(hashed);
                customer.PasswordSalt = Convert.ToBase64String(salt);
                customer.ModifiedDate = DateTime.Now;

                db.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Bikes");
            }
            ViewData["InvalidInfo"] = true;
            return View();
        }*/

        // GET: Customer/Edit/5
        public async Task<IActionResult> EditFR(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await db.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFR(int id, [Bind("CustomerId,NameStyle,Title,FirstName,MiddleName,LastName,Suffix,CompanyName,SalesPerson,EmailAddress,Phone,PasswordHash,PasswordSalt,Rowguid,ModifiedDate")] Customer customer)
        {
            if (id != customer.CustomerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(customer);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customer/Delete/5
        public async Task<IActionResult> DeleteFR(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await db.Customer
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedFR(int id)
        {
            var customer = await db.Customer.FindAsync(id);
            db.Customer.Remove(customer);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExistsFR(int id)
        {
            return db.Customer.Any(e => e.CustomerId == id);
        }
    }
}
