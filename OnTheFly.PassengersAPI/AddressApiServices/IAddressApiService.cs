using Models;

namespace OnTheFly.AddressApiServices.AddressApiServices
{
    public interface IAddressApiService
    {
        Task<Address?> GetAddress(AddressDTO addressDTO);
    }
}
