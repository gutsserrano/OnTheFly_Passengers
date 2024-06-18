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
            if (!ValidateCpf(passengerDTO.Cpf)) throw new Exception("CPF inválido.");

            var passenger = new Passenger
            {
                Cpf = passengerDTO.Cpf,
                Name = passengerDTO.Name,
                Gender = passengerDTO.Gender,
                Phone = passengerDTO.Phone,
                DtBirth = passengerDTO.DtBirth,
                DtRegister = DateTime.Now,
                Restricted = false,
                Address = address,
                AddressZipCode = address.ZipCode,
                AddressNumber = address.Number
            };

            return passenger;
        }

        public static bool ValidateCpf(string cpf)
        {
            if (cpf.Contains(".")) cpf = cpf.Replace(".", "");
            if (cpf.Contains("-")) cpf = cpf.Replace("-", "");

            if (cpf.Length != 11) return false;

            bool valid = false;
            for (int i = 0; i < cpf.Length - 1 && !valid; i++)
            {
                int n1 = int.Parse(cpf.Substring(i, 1));
                int n2 = int.Parse(cpf.Substring(i + 1, 1));

                if (n1 != n2) valid = true;
            }

            return valid && ValidateFirstDigit(cpf) && ValidateSecondDigit(cpf);
        }

        private static bool ValidateFirstDigit(string cpf)
        {
            int result = 0;
            for (int i = 0, mult = 10; i < 9; i++, mult--) result += int.Parse(cpf.Substring(i, 1)) * mult;

            int rest = (result * 10) % 11;
            if (rest == 10) rest = 0;

            int FirstDigit = int.Parse(cpf.Substring(9, 1));

            if (rest == FirstDigit) return true;

            return false;
        }

        private static bool ValidateSecondDigit(string cpf)
        {
            int result = 0;
            for (int i = 0, mult = 11; i < 10; i++, mult--) result += int.Parse(cpf.Substring(i, 1)) * mult;

            int rest = (result * 10) % 11;

            int SecondDigit = int.Parse(cpf.Substring(10, 1));

            if (rest == SecondDigit) return true;

            return false;
        }
    }
}
