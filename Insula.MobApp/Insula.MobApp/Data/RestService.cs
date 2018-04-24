using Insula.MobApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Insula.MobApp.Data
{
    public class RestService
    {
        HttpClient client;

        public RestService()
        {
            client = new HttpClient();
            //client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<User> Authentication(User user)
        {
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{user.Username}:{user.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            try
            {
                var response = await client.GetAsync(Constants.UserUrl);
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    var contentResp = JsonConvert.DeserializeObject<User>(jsonResult);
                    App.User = contentResp;
                    return contentResp;
                }
            }
            catch (Exception) { return null; }
            return null;
        }

        public void Logout()
        {
            App.User = null; 
        }

        public async Task<T> GetResponse<T>(string weburl) where T : class
        {
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{App.User.Username}:{App.User.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            try
            {
                var response = await client.GetAsync(weburl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(jsonResult);
                        return contentResp;
                    }
                    catch (Exception) { return null; }
                }
            }
            catch (Exception) { return null; }
            return null;
        }
        public async Task<T> PostResponse<T>(string weburl, string jsonstring) where T :class
        {
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{App.User.Username}:{App.User.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            string ContentType = "application/json";
            try
            {
                var response = await client.PostAsync(weburl, new StringContent(jsonstring, Encoding.UTF8, ContentType));
                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(jsonResult);
                        return contentResp;
                    }
                    catch (Exception) { return null; }
                }
            }
            catch (Exception) { return null; }
            return null;
        }

        public async Task<T> PutResponse<T>(string weburl, string jsonstring) where T : class
        {
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{App.User.Username}:{App.User.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            string ContentType = "application/json";
            try
            {
                var response = await client.PutAsync(weburl, new StringContent(jsonstring, Encoding.UTF8, ContentType));
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonResult = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var contentResp = JsonConvert.DeserializeObject<T>(jsonResult);
                        return contentResp;
                    }
                    catch (Exception) { return null; }
                }
            }
            catch (Exception) { return null; }
            return null;
        }

        public async Task<bool> DeleteResponse(string weburl)
        {
            var base64String = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{App.User.Username}:{App.User.Password}"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64String);
            try
            {
                var response = await client.DeleteAsync(weburl);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return true;
                }
            }
            catch (Exception) { return false; }
            return false;
        }
    }
}
