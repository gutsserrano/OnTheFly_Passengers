using Models;
using Newtonsoft.Json;
using System.Text;

namespace Services
{
    public class CreatePassengerService
    {
        public async Task<Address> GetAddress(AddressDTO addressDTO)
        {
            using (HttpClient client = new HttpClient())
            {
                string addressApiUrl = $"https://localhost:7217/api/addresses/zipcode/{addressDTO.ZipCode}/number/{addressDTO.Number}";
                HttpResponseMessage response = await client.GetAsync(addressApiUrl);

                StringContent content = new (JsonConvert.SerializeObject(addressDTO), Encoding.UTF8, "application/json");

                if ((int)response.StatusCode == 404) response = await client.PostAsync("https://localhost:7217/api/addresses", content);
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Address viacepAddress = JsonConvert.DeserializeObject<Address>(jsonResponse);

                if (viacepAddress.ZipCode == null) { throw new Exception("CEP inválido."); }

                var address = new Address
                {
                    ZipCode = addressDTO.ZipCode,
                    Number = addressDTO.Number,
                    Street = viacepAddress.Street,
                    Complement = addressDTO.Complement,
                    City = viacepAddress.City,
                    State = viacepAddress.State
                };

                return address;
            }
        }

        public async Task<Passenger> CreatePassenger(PassengerDTO passengerDTO, Address address)
        {
            var passenger = new Passenger
            {
                Cpf = passengerDTO.Cpf,
                Name = passengerDTO.Name,
                Gender = passengerDTO.Gender,
                Phone = passengerDTO.Phone,
                DtBirth = passengerDTO.DtBirth,
                DtRegister = DateTime.Now,
                Restricted = false,
                Address = address
            };

            return passenger;
        }
    }
}
