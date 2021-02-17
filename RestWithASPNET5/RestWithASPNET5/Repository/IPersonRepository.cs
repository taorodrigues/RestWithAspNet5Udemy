using System.Collections.Generic;
using RestWithASPNET5.Model;

namespace RestWithASPNET5.Repository
{
    public interface IPersonRepository : IRepository<Person>
    {
        Person Disable(long id);
        List<Person> FindByName(string firstName, string secondName);
    }
}
