using DataModels.Location;
using HtmlAgilityPack;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataModels.Authentication
{
    public class User
    {
        [BsonId]
        public string Email { get; set; }
        public bool IsValidEmail()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                    return false;

                var maxEmailSize = 254;
                string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
                var regex = new Regex(pattern, RegexOptions.IgnoreCase);
                
                return regex.IsMatch(Email) && Email.Length <= maxEmailSize;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Password { get; set; }
        public bool IsValidPassword()
        {
            try
            {
                if (string.IsNullOrEmpty(Password))
                    return false;

                var hasNumber = new Regex(@"[0-9]+");
                var hasUpperChar = new Regex(@"[A-Z]+");
                var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");
                var hasMinimum8Chars = new Regex(@".{8,}");
                var maxPassowrdSize = 128;

                var isValidated = hasNumber.IsMatch(Password) &&
                                  hasUpperChar.IsMatch(Password) &&
                                  hasSymbols.IsMatch(Password) &&
                                  hasMinimum8Chars.IsMatch(Password) &&
                                  Password.Length <= maxPassowrdSize;

                return isValidated;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string PhoneNumber { get; set; }
        public bool IsValidPhoneNumber()
        {
            if (string.IsNullOrEmpty(PhoneNumber))
                return false;

            var maxPhoneNumberSize = 20;
            if (PhoneNumber.Length >= 9 &&
                PhoneNumber.Length <= maxPhoneNumberSize)
                return true;
            else
                return false;
        }

        public string ZipCode { get; set; }
        public bool IsValidZipCode()
        {
            try
            {
                // Checks if empty
                if (string.IsNullOrEmpty(ZipCode))
                    return false;

                // Checks if in valid format
                var zipCodeSize = 8;
                var splitedZipCode = ZipCode.Split("-");
                if (ZipCode.Length == zipCodeSize &&
                    int.TryParse(splitedZipCode[0], out int result) &&
                    int.TryParse(splitedZipCode[1], out int result2) &&
                    splitedZipCode[0].Length == 4 &&
                    splitedZipCode[1].Length == 3)
                        return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string Address { get; set; }
        public bool IsValidAddress()
        {
            var maxAddressSize = 94;
            return Address.Length <= maxAddressSize;
        }


        public GeoCoordinates Coordinates { get; set; }
        public Role Role { get; set; }
        public string Salt { get; set; }


        public bool HasValidData()
        {
            var output = IsValidEmail() &&
                         IsValidPassword() && 
                         IsValidPhoneNumber() && 
                         IsValidZipCode() &&
                         IsValidAddress();

            return output;
        }

        public void SetAsClient() => Role = Role.Client;

        public async Task GetCoordinatesFromPostalCode()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Divide the zip code in its two parts
                    var zipCodeFirstHalf = ZipCode.Split("-")[0];
                    var zipCodeSecondHalf = ZipCode.Split("-")[1];

                    // Get the page html from the validation website
                    var postalCodeToCoordinatesWebsiteUri = new Uri($"https://www.codigo-postal.pt/?cp4={zipCodeFirstHalf}&cp3={zipCodeSecondHalf}");
                    string html = await client.DownloadStringTaskAsync(postalCodeToCoordinatesWebsiteUri);

                    //Get the coordinates for the zipcode from the html page
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);

                    var classToGet = "pull-right gps";

                    var rawGpsCoordinates = doc.DocumentNode.SelectNodes("//span[@class='" + classToGet + "']")
                                                            .FirstOrDefault()
                                                            .InnerText
                                                            .Split()
                                                            .FirstOrDefault(x => !string.IsNullOrEmpty(x) && !x.Contains("GPS"))
                                                            .Split(",");

                    // Parse the coordinates to valid values and return the object with the coordinates
                    var gotLatitude = double.TryParse(rawGpsCoordinates[0].Replace('.', ','), out var latitude);
                    var gotLongitude = double.TryParse(rawGpsCoordinates[1].Replace('.', ','), out var longitude);

                    if (gotLatitude && gotLongitude)
                    {
                        Coordinates = new GeoCoordinates
                        {
                            Latitude = latitude,
                            Longitude = longitude
                        };
                    }
                    else
                    {
                        throw new Exception("Unable to get coordinates from postal code.");
                    }
                }

            }
            catch (Exception)
            {
                Coordinates = null;
                throw;
            }
        }

    }
}
