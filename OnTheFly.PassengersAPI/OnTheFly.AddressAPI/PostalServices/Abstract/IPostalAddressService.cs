﻿namespace OnTheFly.AddressAPI.PostalServices.Abstract
{
    public interface IPostalAddressService
    {
        Task<IAddressResult?> Fetch(string zipcode);
    }
}
