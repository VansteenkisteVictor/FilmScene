using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Review_API.Data;

namespace Review_API.Controllers
{
    [Route("api/Reservation")]
    [ApiController]
    public class ReservationController : Controller
    {
        private IDataProvider dataProvider;

        public ReservationController(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<Reservation>> Get(string id)
        {
            var reservations = await this.dataProvider.GetUserReservationsASync(id);
            return reservations;
        }

        [HttpGet]
        public async Task<IEnumerable<Reservation>> GetAll()
        {
            var reservations = await this.dataProvider.GetAllReservationsASync();
            return reservations;
        }

        [HttpPost]
        public async Task Add([FromBody]Reservation reservation)
        {
            await this.dataProvider.AddReservation(reservation);
        }

        [HttpPut]
        public async Task Put([FromBody]Reservation reservation)
        {
            await this.dataProvider.UpdateReservation(reservation);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string Id)
        {
            await this.dataProvider.DeleteReservation(Id);
        }

        [HttpGet("detail/{id}")]
        public async Task<Reservation> GetDetail(string id)
        {
           return await this.dataProvider.GetDetailReservation(id);
        }
    }
}