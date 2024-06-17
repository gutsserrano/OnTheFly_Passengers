using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Models
{
    public class Passenger
    {
        [Key]
        public string Cpf { get; set; }
        public string Name { get; set; }
        public char Gender { get; set; }
        public string? Phone { get; set; }
        public DateTime DtBirth { get; set; }
        public DateTime DtRegister { get; set; }
        public bool Restricted { get; set; }
        [NotMapped]
        public Address Address { get; set; }
        [JsonIgnore]
        public string AddressZipCode { get; set; }
        [JsonIgnore]
        public string AddressNumber { get; set; }
    }
}
