using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Cuenta_Movimiento.Controllers;
using Cuenta_Movimiento.Models.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Cuenta_Movimiento.Tests
{
    [TestFixture]
    public class MovimientosControllerTests
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
            _client = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _client.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task GetMovimientos_ReturnsOkResponse()
        {
            var response = await _client.GetAsync("/Movimientos");

            response.EnsureSuccessStatusCode();
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var responseContent = await response.Content.ReadAsStringAsync();
            var movimientos = JsonConvert.DeserializeObject<List<MovimientoModel>>(responseContent);
            Assert.NotNull(movimientos);
            Assert.IsInstanceOf<List<MovimientoModel>>(movimientos);
        }
    }
}

