using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using NoNameBikes.Models;
using NoNameBikes.ReCaptchaV2;


namespace NoNameBikes.Controllers
{
    public class ContactUsController : Controller
    {
        private readonly AdventureWorksLT2017Context _context;

        public ContactUsController(AdventureWorksLT2017Context context)
        {
            _context = context;
        }

        // GET: ContactUs
        public async Task<IActionResult> Index()
        {
            return View(await _context.Contact.ToListAsync());
        }

        // GET: ContactUs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: ContactUs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,PostalCode,Email,Topic,PhoneNumber,Comment")] Contact contact)
        {
            string captchaResponse = Request.Form["g-Recaptcha-Response"];

            ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);

            if (!result.Success)
            {
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                contact.TimeStamp = DateTime.Now;

                if (!SendEmail(contact))
                {
                    return View("/Views/ContactUs/Failure.cshtml");
                }

                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success), contact);
            }
            return View(contact);
        }

        public IActionResult Success(Contact c)
        {
            return View(c);
        }

        private bool SendEmail(Contact contact)
        {
            bool success = false;

            try
            {
                //send the email
                var messageObj = new MimeMessage();
                messageObj.From.Add(new MailboxAddress("Contact Us From", "nonamebikes2020@gmail.com"));
                messageObj.To.Add(new MailboxAddress("NoName Bikes", "nonamebikes2020@gmail.com"));
                messageObj.To.Add(new MailboxAddress(contact.FirstName + " " + contact.LastName, contact.Email));
                messageObj.ReplyTo.Add(new MailboxAddress(contact.FirstName + " " + contact.LastName, contact.Email));

                messageObj.Subject = contact.Topic;
                messageObj.Body = new TextPart("plain")
                {
                    Text = contact.Comment
                };

                using var client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("nonamebikes2020@gmail.com", "JacCs2020");
                client.Send(messageObj);
                client.Disconnect(true);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }

            return success;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// FRENCH VERSION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> IndexFR()
        {
            return View(await _context.Contact.ToListAsync());
        }

        // GET: ContactUs/Details/5
        public async Task<IActionResult> DetailsFR(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: ContactUs/Create
        public IActionResult CreateFR()
        {
            return View();
        }

        // POST: ContactUs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFR([Bind("FirstName,LastName,PostalCode,Email,Topic,PhoneNumber,Comment")] Contact contact)
        {
            string captchaResponse = Request.Form["g-Recaptcha-Response"];

            ReCaptchaValidationResult result = ReCaptchaValidator.IsValid(captchaResponse);

            if (!result.Success)
            {
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                contact.TimeStamp = DateTime.Now;

                if (!SendEmail(contact))
                {
                    return View("/Views/ContactUs/Failure.cshtml");
                }

                _context.Add(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Success), contact);
            }
            return View(contact);
        }

        public IActionResult SuccessFR(Contact c)
        {
            return View(c);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // GET: ContactUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        // POST: ContactUs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,TimeStamp,FirstName,LastName,PostalCode,Email,Topic,PhoneNumber,Comment")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            return View(contact);
        }

        // GET: ContactUs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contact
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: ContactUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contact.FindAsync(id);
            _context.Contact.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contact.Any(e => e.Id == id);
        }
    }
}
