using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Review_API.Data;

namespace Review_API.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : Controller
    {
        private IDataProvider dataProvider;

        public LoginController(IDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

        }

        // GET: api/Todo
        [HttpGet("{username}")]
        public async Task<Login> Get(string username)
        {
            var login = await this.dataProvider.GetLogin(username);
            return login;
        }


        [HttpPost]
        public async Task Add([FromBody]Login login)
        {
            await this.dataProvider.AddLogin(login);
        }
    }
}
