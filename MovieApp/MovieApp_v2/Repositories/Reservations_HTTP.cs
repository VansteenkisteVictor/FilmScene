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
    public class Reservations_HTTP : IReservations_HTTP
    {
        private readonly IHttpClientFactory _clientFactory;

        public IEnumerable<Reservation> Reservations { get; private set; }
        public Reservation ReservationDetail { get; set; }


        public Reservations_HTTP(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Reservation>> GetReservationUser(string id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44305/api/Reservation/{id}");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Reservations = await response.Content
                        .ReadAsAsync<IEnumerable<Reservation>>();
                }
                return Reservations;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<IEnumerable<Reservation>> GetReservationAll()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44305/api/Reservation");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Reservations = await response.Content
                        .ReadAsAsync<IEnumerable<Reservation>>();
                }
                return Reservations;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Reservation> GetReservationDetail(string id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://localhost:44305/api/Reservation/detail/{id}");
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    ReservationDetail = await response.Content
                        .ReadAsAsync<Reservation>();
                }
                return ReservationDetail;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task PostReservation(Reservation reservation)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PostAsync("https://localhost:44305/api/Reservation", new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task UpdateReservations(Reservation reservation)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var response = await client.PutAsync("https://localhost:44305/api/Reservation", new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json"));
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
                var response = await client.DeleteAsync($"https://localhost:44305/api/Reservation/{id}");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
