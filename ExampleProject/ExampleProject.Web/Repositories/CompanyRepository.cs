using ExampleProject.DataAccess.Data;
using ExampleProject.Models;
using ExampleProject.Web.Repositories.Interfaces;

namespace ExampleProject.Web.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;

        public CompanyRepository(AppDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Company> GetAll()
        {
            return _context.Companies.ToList();
        }

        public Company Find(int id)
        {
            return _context.Companies.FirstOrDefault(t => t.Id == id);
        }

        public void Create(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
        }

        public void Remove(int id)
        {
            var entity = _context.Companies.First(t => t.Id == id);
            _context.Companies.Remove(entity);
            _context.SaveChanges();
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
            _context.SaveChanges();
        }
    }
}
