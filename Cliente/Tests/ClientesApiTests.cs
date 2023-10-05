using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Cliente_Persona.Tests
{
    public class ClientesApiTests : IDisposable
    {
        private readonly HttpClient _client;

        public ClientesApiTests()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5173/Clientes")
            };
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        [Fact]
        public async Task ObtenerDatos_DebeRetornarStatusCode200()
        {
            var response = await _client.GetAsync("Clientes"); // Reemplaza con la ruta de tu endpoint GET
            Assert.Equals(HttpStatusCode.OK, response.StatusCode);
        }
    }
}