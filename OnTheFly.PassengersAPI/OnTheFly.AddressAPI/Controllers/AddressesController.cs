using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.AddressAPI.Data;
using OnTheFly.AddressAPI.PostalServices.Abstract;

namespace OnTheFly.AddressAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : ControllerBase
    {
        private readonly OnTheFlyAddressAPIContext _context;
        private readonly IPostalAddressService _service;

        public AddressesController(OnTheFlyAddressAPIContext context, IPostalAddressService service)
        {
            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> Get()
        {
            return Ok(await _context.Address.ToListAsync());
        }

        [HttpGet("zipcode/{zipcode}/number/{number}")]
        public async Task<ActionResult<Address>> Get(string zipcode, string number)
        {
            var address = await _context.Address.Where(
                address => address.ZipCode.Replace("-", "") == zipcode.Replace("-", "") && 
                address.Number == number
            ).FirstOrDefaultAsync();

            if (address == null)
                return NotFound();

            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<Address>> Post(AddressDTO addressDTO)
        {
            bool addressExists = await AddressExistsAsync(addressDTO);

            if (addressExists)
                return Conflict("Endereço já cadastrado");

            IAddressResult? result = await _service.Fetch(addressDTO.ZipCode);

            if (result == null)
                return BadRequest();

            var address = new Address
            {
                Complement = addressDTO.Complement,
                Number = addressDTO.Number,
                ZipCode = result.Zipcode,
                State = result.State,
                Street = result.Street,
                City = result.City
            };

            _context.Address.Add(address);
            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<bool> AddressExistsAsync(AddressDTO addressDTO)
        {
            bool addressExists = await _context.Address
                .AnyAsync(address => address.ZipCode.Replace("-", "") == addressDTO.ZipCode.Replace("-", "") && address.Number == addressDTO.Number);

            return addressExists;
        }
    }
}
