using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExampleProject.DataAccess.Enums
{ 
    public enum UserRole
    {
        Operator=1,
        Chef=2,
        Supervisor=3,
        Manager=4
    }


    public enum CompanyType
    {
        [Display(Name = "Admin")]
        Admin = 1,
        [Display(Name = "Producer")]
        Producer = 2,
        [Display(Name = "Supplier")]
        Supplier = 3
    }
}
