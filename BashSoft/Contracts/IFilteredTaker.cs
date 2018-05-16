namespace BashSoft.Contracts
{
    using System.Collections.Generic;

    public interface IFilteredTaker
    {
        void FilterAndTake(string courseName, string givenFilter, int? studentsToTake = null);
    }
}
