using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models
{
    public class PassengerUpdateDTO
    {
        public string Cpf { get; set; }
        public string Name { get; set; }
        public char Gender { get; set; }
        public string? Phone { get; set; }
        public DateTime DtBirth { get; set; }
        public bool Restricted { get; set; }
    }
}
