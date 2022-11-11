using ExampleProject.DataAccess.Data;
using ExampleProject.Models;
using ExampleProject.Web.Repositories.Interfaces;

namespace ExampleProject.Web.Repositories
{
    public class EmployeeRepository : IEmloyeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        public List<Employee> GetCompanyEmployees(int compId)
        {
            return _context.Employees.Where(e => e.CompanyId == compId).ToList();
        }

        public Employee Find(int id)
        {
            return _context.Employees.FirstOrDefault(t => t.Id == id);
        }

        public void Create(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            var entity = _context.Employees.First(t => t.Id == id);
            _context.Employees.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            _context.Employees.Update(employee);
            _context.SaveChanges();
        }
    }
}
