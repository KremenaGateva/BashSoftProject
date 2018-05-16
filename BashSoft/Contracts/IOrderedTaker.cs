namespace BashSoft.Contracts
{
    using System.Collections.Generic;

    public interface IOrderedTaker
    {
        void OrderAndTake(string courseName, string comparison, int? studentsToTake = null);
    }
}
