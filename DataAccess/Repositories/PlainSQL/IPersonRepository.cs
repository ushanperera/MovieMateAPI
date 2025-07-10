using DataAccess.Models;

namespace DataAccess.Repositories;

public interface IPersonRepository
{
    Task<IEnumerable<PersonModel>> GetPeopleAsync();
    Task<PersonModel> GetPeopleByIdAsync(int id);
    Task<PersonModel> CreatePersonAsync(PersonModel person);
    Task UpdatePersonAsync(PersonModel person);
    Task DeletePersonAsync(int id);
}
