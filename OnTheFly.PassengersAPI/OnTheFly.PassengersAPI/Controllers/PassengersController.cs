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

        public PassengersController(OnTheFlyPassengersAPIContext context, UpdatePassengerService updatePassengerService)
        {
            _context = context;
            _updatePassengerService = updatePassengerService;
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

            return NoContent();
        }

        // POST: api/Passengers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Passenger>> PostPassenger(Passenger passenger)
        {
          if (_context.Passenger == null)
          {
              return Problem("Entity set 'OnTheFlyPassengersAPIContext.Passenger'  is null.");
          }
            _context.Passenger.Add(passenger);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PassengerExists(passenger.Cpf))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPassenger", new { id = passenger.Cpf }, passenger);
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
