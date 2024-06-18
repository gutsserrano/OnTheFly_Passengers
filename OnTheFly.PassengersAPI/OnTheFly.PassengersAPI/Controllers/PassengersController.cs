using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using OnTheFly.PassengersAPI.Data;
using Services;

namespace OnTheFly.PassengersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassengersController : ControllerBase
    {
        private readonly OnTheFlyPassengersAPIContext _context;
        private readonly UpdatePassengerService _updatePassengerService;
        private readonly CreatePassengerService _createPassengerService;
        private readonly GetPassengerService _getPassengerService;
        private readonly DeletePassengerService _deletePassengerService;

        public PassengersController(OnTheFlyPassengersAPIContext context, UpdatePassengerService updatePassengerService, CreatePassengerService createPassengerService, GetPassengerService getPassengerService, DeletePassengerService deletePassengerService)
        {
            _context = context;
            _updatePassengerService = updatePassengerService;
            _createPassengerService = createPassengerService;
            _getPassengerService = getPassengerService;
            _deletePassengerService = deletePassengerService;
        }

        // GET: api/Passengers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Passenger>>> GetPassenger()
        {
            /*if (_context.Passenger == null)
            {
                return NotFound();
            }
              return await _context.Passenger.ToListAsync();*/

            return new List<Passenger>
            {
                new Passenger
                {
                    Cpf = "12345678901",
                    Name = "John Doe",
                    DtBirth = new DateTime(1990, 1, 1),
                    DtRegister = DateTime.Now,
                    Address = new Address
                    {
                        Street = "Main St",
                        Number = "123",
                        City = "Springfield",
                        State = "IL"
                    },
                    AddressNumber = "123",
                    AddressZipCode = "12345"
                }
            };
        }

        // GET: api/Passengers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Passenger>> GetPassenger(string id)
        {
            if (_context.Passenger == null)
            {
                return NotFound();
            }
            var passenger = await _context.Passenger.FindAsync(id);

            if (passenger == null)
            {
                return NotFound();
            }

            return passenger;
        }

        // PUT: api/Passengers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{cpf}")]
        public async Task<IActionResult> PutPassenger(string cpf, PassengerUpdateDTO passengerUpdateDTO)
        {
            if (cpf != passengerUpdateDTO.Cpf)
            {
                return BadRequest();
            }

            Passenger passenger = GetPassenger(passengerUpdateDTO.Cpf).Result.Value;

            _context.Entry(_updatePassengerService.UpdatePassenger(passenger, passengerUpdateDTO)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PassengerExists(cpf))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(passenger);
        }

        [HttpPost]
        public async Task<ActionResult<Passenger>> PostPassenger(PassengerDTO passengerDTO)
        {
            Passenger passenger = null;

            try
            {
                Address address = await _createPassengerService.GetAddress(passengerDTO.AddressDTO);
                passenger = await _createPassengerService.CreatePassenger(passengerDTO, address);

                _context.Passenger.Add(passenger);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(passenger);
        }

        // DELETE: api/Passengers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePassenger(string id)
        {
            if (_context.Passenger == null)
            {
                return NotFound();
            }
            var passenger = await _context.Passenger.FindAsync(id);
            if (passenger == null)
            {
                return NotFound();
            }

            _context.Passenger.Remove(passenger);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PassengerExists(string id)
        {
            return (_context.Passenger?.Any(e => e.Cpf == id)).GetValueOrDefault();
        }
    }
}
