using ExampleProject.Models;

namespace ExampleProject.Web.Repositories.Interfaces
{
    public interface ICompanyRepository
    {
        IEnumerable<Company> GetAll();
        Company Find(int id);
        void Create(Company company);
        void Remove(int id);
        void Update(Company company);
    }
}
