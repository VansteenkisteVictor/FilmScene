using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using MovieApp_v2.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class GetMovieSearchTest
    {
        private IomdbAPIMovies _iomdbAPI;
        [TestMethod]
        public async Task GetMovieSearchAsync(IomdbAPIMovies iomdbAPI)
        {
            _iomdbAPI = iomdbAPI;
            var result = await iomdbAPI.GetMovies("test",null,null);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Task<MovieSearch>));
        }

    }
}
