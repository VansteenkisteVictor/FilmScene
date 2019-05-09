using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MovieApp_v2.Repositories
{
    public class Movies_HTTP : IMovies_HTTP
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public IEnumerable<ReviewTask_RA> MovieReviews { get; private set; }
        public Login login { get; set; }

        public Movies_HTTP(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            Console.Write(_clientFactory);
        }


        public async Task<IEnumerable<ReviewTask_RA>> OnGetReview(string id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44305/api/ReviewTask/{id}");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    MovieReviews = await response.Content
                        .ReadAsAsync<IEnumerable<ReviewTask_RA>>();
                }
                return MovieReviews;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<ReviewTask_RA>> OnGetReviewUser(string id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44305/api/ReviewTask/userreview/{id}");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    MovieReviews = await response.Content
                        .ReadAsAsync<IEnumerable<ReviewTask_RA>>();
                }
                return MovieReviews;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task PostReview(ReviewTask_RA review)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync("https://localhost:44305/api/ReviewTask", new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json") );
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task Register(Login login)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync("https://localhost:44305/api/login", new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task Delete(string id)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.DeleteAsync($"https://localhost:44305/api/ReviewTask/delete/{id}");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task Update(ReviewTask_RA review)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PutAsync("https://localhost:44305/api/ReviewTask/change", new StringContent(JsonConvert.SerializeObject(review), Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Login> Login(string Username)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44305/api/login/{Username}");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    login = await response.Content
                        .ReadAsAsync<Login>();
                }
                return login;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


    }
}
