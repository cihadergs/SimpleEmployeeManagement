using ExampleProject.Models;

namespace ExampleProject.Web.Repositories.Interfaces
{
    public interface IEmloyeeRepository
    {
        IEnumerable<Employee> GetAll();
        List<Employee> GetCompanyEmployees(int compId);
        Employee Find(int id);
        void Create(Employee employee);
        void Remove(int id);
        void Update(Employee employee);
    }
}
