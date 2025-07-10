
namespace DataAccess.Models;
internal class ShiftDataModel
{
    public int Id { get; set; }
    public int ShiftId { get; set; }
    public DateTime ShiftDate { get; set; }
    public int RateId { get; set; }
    public float Rate { get; set; }
    public DateTime WorkStartTime { get; set; }
    public DateTime WorkEndTime { get; set; }
    public DateTime MealBreakStartTime { get; set; }
    public DateTime MealBreakEndTime { get; set; }
    public string TotalWorkedDuration { get; set; }
    public string TotalMealBreakDuration { get; set; }
    public string MealBreakGapDuration { get; set; }
    public string MealBreakPenaltyDuration { get; set; }
    public string NetWorkDuration { get; set; }
    public string ExtraHoursWorkedDuration { get; set; }
    public bool IsPublicHoliday { get; set; }
    public bool IsWeekendShift { get; set; }

    public int HourlyPay { get; set; }
    public int ExtraHourPay { get; set; }
    public int MealBreakPenaltyPay { get; set; }
    public int PublicHolidayPenaltyPay { get; set; }
    public int WeekEndPenaltyPay { get; set; }
    public int TotalDayPay { get; set; }
}
