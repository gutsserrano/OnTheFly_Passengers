using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using OnTheFly.AddressAPI.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class GetPassengerService
    {
        public async Task<Address> GetAddress(string zipCode, string number)
        {
            using (HttpClient client = new HttpClient())
            {
                string addressApiUrl = $"https://localhost:7217/api/addresses/zipcode/{zipCode}/number/{number}";
                HttpResponseMessage response = await client.GetAsync(addressApiUrl);
                
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Address viacepAddress = JsonConvert.DeserializeObject<Address>(jsonResponse);

                if (viacepAddress.ZipCode == null) { throw new Exception("CEP inválido."); }

                return viacepAddress;
            }
        }
        
    }
}
