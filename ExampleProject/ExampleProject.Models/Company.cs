using ExampleProject.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExampleProject.Models
{
    public class Company
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Company Name")]
        public string Name { get; set; }

        [DisplayName("Company Type")]
        public CompanyType CompanyType { get; set; }

        [DisplayName("Is Active?")]
        public bool IsActive { get; set; }

        public List<AppUser> AppUsers { get; set; }
        public List<Employee> Employees { get; set; }
    }
}
