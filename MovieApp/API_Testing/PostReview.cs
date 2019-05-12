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
    public class API_Tests
    {
        private async Task<ReviewTask_RA> GetReview(string id)
        {
            var reviewObj = new ReviewTask_RA();
            reviewObj.Id = Guid.Parse(id);
            reviewObj.Name = "TestUser";
            reviewObj.Score = 0;
            reviewObj.MovieId = "09324892-da72-453b-8edb-79408cd13c20";
            reviewObj.UserId = "3acb1a8e-2a70-4654-a92c-ebbfbb15e2f4";
            return await Task.Run(() => reviewObj);
        }

        private IDataProvider dataProvider;

        [TestMethod]
        public async Task AddReviewAsync(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

            using (var httpTest = new HttpTest())
            {
                string id = "406d0f63-807e-4048-a726-3e649ed5a2d4";
                await dataProvider.Add(await GetReview(id));
                var result = dataProvider.GetReview(id);
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(Task<List<ReviewTask_RA>>));
                Assert.AreEqual(GetReview(id), result);
                await dataProvider.DeleteReservation(id);
            }
        }
    }
}
