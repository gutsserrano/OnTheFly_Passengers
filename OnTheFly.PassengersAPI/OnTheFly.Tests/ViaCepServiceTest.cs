﻿using OnTheFly.AddressAPI.PostalServices;
using OnTheFly.AddressAPI.PostalServices.Abstract;

namespace OnTheFly.Tests
{
    public class ViaCepServiceTest
    {

        [Fact]
        public void GetAddress_CorrectZipcode_ReturnsFullAddress()
        {
            string zipcode = "15990-840";
            var service = new ViaCepService();

            IAddressResult? result = service.Fetch(zipcode).Result;

            Assert.Equal(result.Street, "Rua Cesário Motta");
        }

        [Fact]
        public void GetAddress_IncorrectZipcodeLength_ReturnsNull()
        {
            string zipcode = "0000000000";
            var service = new ViaCepService();

            IAddressResult? result = service.Fetch(zipcode).Result;

            Assert.Null(result);
        }

        [Fact]
        public void GetAddress_IncorrectZipcode_ReturnsNull()
        {
            string zipcode = "00000-000";
            var service = new ViaCepService();

            IAddressResult? result = service.Fetch(zipcode).Result;

            Assert.Null(result);
        }
    }
}
