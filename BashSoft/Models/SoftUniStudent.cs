namespace BashSoft.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BashSoft.Contracts;
    using BashSoft.Exceptions;

    public class SoftUniStudent : IStudent
    {
        private string userName;
        private Dictionary<string, ICourse> enrolledCourses;
        private Dictionary<string, double> gradesByCourseName;

        public SoftUniStudent(string userName)
        {
            this.UserName = userName;
            this.enrolledCourses = new Dictionary<string, ICourse>();
            this.gradesByCourseName = new Dictionary<string, double>();
        }

        public string UserName
        {
            get
            {
                return this.userName;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidStringException();
                }

                this.userName = value;
            }
        }

        public IReadOnlyDictionary<string, ICourse> EnrolledCourses
        {
            get
            {
                return this.enrolledCourses;
            }
        }

        public IReadOnlyDictionary<string, double> GradesByCourseName
        {
            get
            {
                return this.gradesByCourseName;
            }
        }

        public int CompareTo(IStudent other)
        {
            return this.UserName.CompareTo(other.UserName);
        }

        public void EnrollInCourse(ICourse course)
        {
            if (this.enrolledCourses.ContainsKey(course.Name))
            {
                throw new DuplicateEntryInStructureException(this.UserName, course.Name);
            }
            this.enrolledCourses.Add(course.Name, course);
        }

        public void SetGradesOnCourse(string courseName, params int[] scores)
        {
            if (!this.enrolledCourses.ContainsKey(courseName))
            {
                throw new CourseNotFoundException();
            }

            if (scores.Length > SoftUniCourse.NumberOfTasksOnExam)
            {
                throw new ArgumentException(ExceptionMessages.InvalidNumberOfScores);
            }

            this.gradesByCourseName.Add(courseName, CalculateGrade(scores));
        }

        private double CalculateGrade(int[] scores)
        {
            double percentigeOfSolvedExam = 
                scores.Sum() / (double)(SoftUniCourse.NumberOfTasksOnExam * SoftUniCourse.MaxScoreOnExamTask);
            double grade = percentigeOfSolvedExam * 4 + 2;
            return grade;
        }

        public override string ToString()
        {
            return this.UserName;
        }
    }
}
