using DataAccess.DbAccess;
using DataAccess.Models;

namespace DataAccess.Repositories.Data;

public class ShiftData : IShiftData
{
    private readonly ISqlDataAccess _db;

    public ShiftData(ISqlDataAccess db)
    {
        _db = db;
    }

    public Task<IEnumerable<UserModel>> GetShifts() =>
        _db.LoadData<UserModel, dynamic>("GetAllShiftData", new { });


}
