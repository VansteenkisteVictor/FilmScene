using Flurl.Http.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Review_API.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace API_Testing
{
    [TestClass]
    public class PostReservation
    {
        private async Task<Reservation> GetReservation(string id)
        {
            var reservationObj = new Reservation();
            reservationObj.Id = Guid.Parse(id);
            reservationObj.Email = "TestUSer";

            return await Task.Run(() => reservationObj);
        }

        private IDataProvider dataProvider;

        [TestMethod]
        public async Task AddReservationAsync(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

            using (var httpTest = new HttpTest())
            {
                string id = "fd6919e7-6bdf-4101-946b-4e42994f7936";
                await dataProvider.AddReservation(await GetReservation(id));
                var result = dataProvider.GetDetailReservation(id);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Task<Reservation>));
                Assert.AreEqual(GetReservation(id), result);
                await dataProvider.Delete(id);
            }
        }
    }
}
