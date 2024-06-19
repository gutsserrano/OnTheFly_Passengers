using Models;

namespace OnTheFly.AddressApiServices.AddressApiServices
{
    public class MockAddressApi : IAddressApiService
    {
        public async Task<Address?> GetAddress(AddressDTO addressDTO)
        {
            return new Address
            {
                ZipCode = addressDTO.ZipCode,
                Number = addressDTO.Number,
                Complement = addressDTO.Complement,
                Street = "Rua dos Testes",
                City = "Cidade dos Testes",
                State = "JAVA"
            };
        }
    }
}
