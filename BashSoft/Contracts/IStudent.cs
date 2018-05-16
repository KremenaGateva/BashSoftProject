namespace BashSoft.Contracts
{
    using System;
    using System.Collections.Generic;

    public interface IStudent : IComparable<IStudent>
    {
        string UserName { get; }

        IReadOnlyDictionary<string, ICourse> EnrolledCourses { get; }

        IReadOnlyDictionary<string, double> GradesByCourseName { get; }

        void EnrollInCourse(ICourse course);

        void SetGradesOnCourse(string courseName, params int[] scores);
    }
}