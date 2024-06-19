using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using OnTheFly.AddressAPI.Data;
using OnTheFly.AddressApiServices.AddressApiServices;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GetPassengerService
    {
        private readonly IAddressApiService _api;

        public GetPassengerService(IAddressApiService api)
        {
            _api = api;
        }

        public async Task<Address> GetAddress(string zipCode, string number)
        {
            Address? address = await _api.GetAddress(new AddressDTO { ZipCode = zipCode, Number = number });

            if (address == null)
                throw new Exception("CEP inválido.");

            return address;
        }
    }
}
