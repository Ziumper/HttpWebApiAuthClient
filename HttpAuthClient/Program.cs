using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HttpAuthClient
{
    class Program
    {
        static string host = "http://localhost:58141/";
        static void Main(string[] args)
        {
            string userName = "admin";
            string password = "admin";
            //var registerResult = Register(userName, password);

            //Console.WriteLine("Registration Status Code: {0}", registerResult);

            Dictionary<string,string> responseDictionary = GetToken("admin", "admin");
            foreach (var kvp in responseDictionary)
            {
                Console.WriteLine("{0}: {1}", kvp.Key, kvp.Value);
            }
            Console.Read();
        }


        static string Register(string email, string password)
        {
            var registerModel = new
            {
                Email = email,
                Password = password,
                ConfirmPassword = password
            };
            using (var client = new HttpClient())
            {
                var response =
                    client.PostAsJsonAsync(
                    "http://localhost:58141//api/Account/Register",
                    registerModel).Result;
                return response.StatusCode.ToString();
            }
        }


        static Dictionary<string,string> GetToken(string userName, string password)
        {
            HttpClient client = new HttpClient();
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password" ),
                    new KeyValuePair<string, string>( "username", userName ),
                    new KeyValuePair<string, string> ( "Password", password )
                };
            var content = new FormUrlEncodedContent(pairs);

            // Attempt to get a token from the token endpoint of the Web Api host:
            HttpResponseMessage response =
                client.PostAsync(host + "Token", content).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            // De-Serialize into a dictionary and return:
            Dictionary<string, string> tokenDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            return tokenDictionary;
        }
    }
}
