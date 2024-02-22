using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NoNameBikes.ReCaptchaV2
{
    public class ReCaptchaValidator
    {
        public static ReCaptchaValidationResult IsValid(string captchaResponse)
        {
            if (string.IsNullOrWhiteSpace(captchaResponse))
            {
                return new ReCaptchaValidationResult()
                { Success = false };
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://www.google.com");

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("secret", "6Lc4UuMZAAAAACiP0hYAQ6LYquMbpsL__PjF8i12"));
            values.Add(new KeyValuePair<string, string>("response", captchaResponse));
            FormUrlEncodedContent content = new FormUrlEncodedContent(values);

            HttpResponseMessage response = client.PostAsync("/recaptcha/api/siteverify", content).Result;

            string verificationResponse = response.Content.ReadAsStringAsync().Result;

            var verificationResult = JsonConvert.DeserializeObject<ReCaptchaValidationResult>(verificationResponse);

            return verificationResult;
        }
    }
}
