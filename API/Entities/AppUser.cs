using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int id {get; set;}
        public string userName { get; set; }
       public byte[] passwordHash { get; set; }
        public byte[] passwordSalt { get; set; }
        public DateTime dateofBirth { get; set; }
        public string knownAs { get; set; }
        public DateTime created { get; set; } = DateTime.Now;
        public DateTime lastActive { get; set; } = DateTime.Now;
        public string gender { get; set; }
        public string introduction { get; set; }
        public string lookingFor { get; set; }
        public string interests { get; set; }
        public string city  { get; set; }
        public string country { get; set; }
        public ICollection<Photo> photos { get; set; }
        // public int GetAge(){
        //     return DateofBirth.CalculateAge();
        // }
    }
}