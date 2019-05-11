using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MovieApp_v2.Repositories
{
    public class omdbAPIMovies : IomdbAPIMovies
    {
        private readonly IHttpClientFactory _clientFactory;
        public MovieSearch Movies { get; private set; }
        public MovieDetail MovieDetail { get; set; }
        public omdbAPIMovies(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            Console.Write(_clientFactory);
        }
        public async Task<MovieSearch> GetMovies(string search,string type,string year)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"http://www.omdbapi.com/?s={search}&type={type}&y={year}&apikey=9db84c18");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Movies = await response.Content
                        .ReadAsAsync<MovieSearch>();
                }
                return Movies;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<MovieDetail> GetMovieDetail(string id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"http://www.omdbapi.com/?i={id}&apikey=9db84c18");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    MovieDetail = await response.Content
                        .ReadAsAsync<MovieDetail>();
                }
                return MovieDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }




}
