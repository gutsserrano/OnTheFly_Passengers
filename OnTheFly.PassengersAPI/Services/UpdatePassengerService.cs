using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UpdatePassengerService
    {
        public Passenger UpdatePassenger(Passenger passenger, PassengerUpdateDTO passengerUpdateDTO)
        {
            passenger.Name = passengerUpdateDTO.Name;
            passenger.Gender = passengerUpdateDTO.Gender;
            passenger.Phone = passengerUpdateDTO.Phone;
            passenger.DtBirth = passengerUpdateDTO.DtBirth;
            passenger.Restricted = passengerUpdateDTO.Restricted;

            return passenger;
        }
    }
}
