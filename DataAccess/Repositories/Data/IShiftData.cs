using DataAccess.Models;

namespace DataAccess.Repositories.Data;
public interface IShiftData
{
    Task<IEnumerable<UserModel>> GetShifts();
}