namespace ConsoleApp7
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public List<Course> Courses { get; set; }

        public Student(int studentId, string name, int age)
        {
            StudentId = studentId;
            Name = name;
            Age = age;
            Courses = new List<Course>();
        }

        public bool Enroll(Course course)
        {
            if (Courses.Any(c => c.CourseId == course.CourseId))
                return false;

            Courses.Add(course);
            return true;
        }

        public string PrintDetails()
        {
            string result = $"ID: {StudentId}\nName: {Name}\nAge: {Age}\nCourses: ";

            if (Courses.Count == 0)
                result += "No courses enrolled";
            else
                result += string.Join(", ", Courses.Select(c => c.Title));

            return result;
        }
    }

    class Instructor
    {
        public int InstructorId { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }

        public Instructor(int instructorId, string name, string specialization)
        {
            InstructorId = instructorId;
            Name = name;
            Specialization = specialization;
        }

        public string PrintDetails()
        {
            return $"ID: {InstructorId}\nName: {Name}\nSpecialization: {Specialization}";
        }
    }

    class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public Instructor Instructor { get; set; }

        public Course(int courseId, string title, Instructor instructor)
        {
            CourseId = courseId;
            Title = title;
            Instructor = instructor;
        }

        public string PrintDetails()
        {
            return $"ID: {CourseId}\nTitle: {Title}\nInstructor: {Instructor.Name}";
        }
    }

    class StudentManager
    {
        public List<Student> Students { get; set; } = new List<Student>();
        public List<Course> Courses { get; set; } = new List<Course>();
        public List<Instructor> Instructors { get; set; } = new List<Instructor>();

        public bool AddStudent(Student student)
        {
            if (FindStudent(student.StudentId) != null)
                return false;

            Students.Add(student);
            return true;
        }

        public bool AddCourse(Course course)
        {
            if (FindCourse(course.CourseId) != null)
                return false;

            Courses.Add(course);
            return true;
        }

        public bool AddInstructor(Instructor instructor)
        {
            if (FindInstructor(instructor.InstructorId) != null)
                return false;

            Instructors.Add(instructor);
            return true;
        }

        public Student? FindStudent(int studentId)
        {
            return Students.FirstOrDefault(s => s.StudentId == studentId);
        }

        public Course? FindCourse(int courseId)
        {
            return Courses.FirstOrDefault(c => c.CourseId == courseId);
        }

        public Instructor? FindInstructor(int instructorId)
        {
            return Instructors.FirstOrDefault(i => i.InstructorId == instructorId);
        }

        public bool UpdateStudentInformation(int studentId, string name, int age)
        {
            Student? student = FindStudent(studentId);

            if (student == null)
                return false;

            student.Name = name;
            student.Age = age;

            return true;
        }

        public bool DeleteStudent(int studentId)
        {
            Student? student = FindStudent(studentId);

            if (student == null)
                return false;

            Students.Remove(student);
            return true;
        }

        public bool EnrollStudentInCourse(int studentId, int courseId)
        {
            Student? student = FindStudent(studentId);
            Course? course = FindCourse(courseId);

            if (student == null || course == null)
                return false;

            return student.Enroll(course);
        }
    }

    class Program
    {
        static StudentManager manager = new StudentManager();

        static void Main()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n===== Student Management System =====");
                Console.WriteLine("1. Add a new Student");
                Console.WriteLine("2. View all Students");
                Console.WriteLine("3. Search for a Student by ID");
                Console.WriteLine("4. Update Student Information");
                Console.WriteLine("5. Delete a Student");
                Console.WriteLine("6. Add a new Course");
                Console.WriteLine("7. Add a new Instructor");
                Console.WriteLine("8. Enroll a Student in a Course");
                Console.WriteLine("9. View all Courses");
                Console.WriteLine("10. View all Instructors");
                Console.WriteLine("0. Exit");
                Console.Write("Choose an option: ");

                int choice = ReadInt();

                switch (choice)
                {
                    case 1:
                        AddStudent();
                        break;

                    case 2:
                        ViewAllStudents();
                        break;

                    case 3:
                        SearchStudent();
                        break;

                    case 4:
                        UpdateStudent();
                        break;

                    case 5:
                        DeleteStudent();
                        break;

                    case 6:
                        AddCourse();
                        break;

                    case 7:
                        AddInstructor();
                        break;

                    case 8:
                        EnrollStudent();
                        break;

                    case 9:
                        ViewAllCourses();
                        break;

                    case 10:
                        ViewAllInstructors();
                        break;

                    case 0:
                        running = false;
                        Console.WriteLine("Goodbye!");
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
        }

        static int ReadInt()
        {
            int value;

            while (!int.TryParse(Console.ReadLine(), out value))
            {
                Console.Write("Enter a valid number: ");
            }

            return value;
        }

        static string ReadText(string message)
        {
            string? value;

            do
            {
                Console.Write(message);
                value = Console.ReadLine();
            }
            while (string.IsNullOrWhiteSpace(value));

            return value.Trim();
        }

        static void AddStudent()
        {
            Console.WriteLine("\n--- Add Student ---");

            Console.Write("Student ID: ");
            int id = ReadInt();

            string name = ReadText("Student Name: ");

            Console.Write("Student Age: ");
            int age = ReadInt();

            if (age <= 0)
            {
                Console.WriteLine("Age must be greater than zero.");
                return;
            }

            Student student = new Student(id, name, age);

            if (manager.AddStudent(student))
                Console.WriteLine("Student added successfully.");
            else
                Console.WriteLine("Student ID already exists.");
        }

        static void ViewAllStudents()
        {
            Console.WriteLine("\n--- All Students ---");

            if (manager.Students.Count == 0)
            {
                Console.WriteLine("No students found.");
                return;
            }

            foreach (Student student in manager.Students)
            {
                Console.WriteLine(student.PrintDetails());
                Console.WriteLine("--------------------");
            }
        }

        static void SearchStudent()
        {
            Console.Write("Enter Student ID: ");
            int id = ReadInt();

            Student? student = manager.FindStudent(id);

            if (student != null)
                Console.WriteLine(student.PrintDetails());
            else
                Console.WriteLine("Student not found.");
        }

        static void UpdateStudent()
        {
            Console.Write("Enter Student ID: ");
            int id = ReadInt();

            Student? student = manager.FindStudent(id);

            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            string name = ReadText("New Name: ");

            Console.Write("New Age: ");
            int age = ReadInt();

            if (age <= 0)
            {
                Console.WriteLine("Age must be greater than zero.");
                return;
            }

            manager.UpdateStudentInformation(id, name, age);
            Console.WriteLine("Student updated successfully.");
        }

        static void DeleteStudent()
        {
            Console.Write("Enter Student ID: ");
            int id = ReadInt();

            if (manager.DeleteStudent(id))
                Console.WriteLine("Student deleted successfully.");
            else
                Console.WriteLine("Student not found.");
        }

        static void AddInstructor()
        {
            Console.WriteLine("\n--- Add Instructor ---");

            Console.Write("Instructor ID: ");
            int id = ReadInt();

            string name = ReadText("Instructor Name: ");
            string specialization = ReadText("Specialization: ");

            Instructor instructor = new Instructor(id, name, specialization);

            if (manager.AddInstructor(instructor))
                Console.WriteLine("Instructor added successfully.");
            else
                Console.WriteLine("Instructor ID already exists.");
        }

        static void AddCourse()
        {
            Console.WriteLine("\n--- Add Course ---");

            if (manager.Instructors.Count == 0)
            {
                Console.WriteLine("Add an instructor first.");
                return;
            }

            Console.Write("Course ID: ");
            int id = ReadInt();

            string title = ReadText("Course Title: ");

            ViewAllInstructors();

            Console.Write("Enter Instructor ID: ");
            int instructorId = ReadInt();

            Instructor? instructor = manager.FindInstructor(instructorId);

            if (instructor == null)
            {
                Console.WriteLine("Instructor not found.");
                return;
            }

            Course course = new Course(id, title, instructor);

            if (manager.AddCourse(course))
                Console.WriteLine("Course added successfully.");
            else
                Console.WriteLine("Course ID already exists.");
        }

        static void EnrollStudent()
        {
            Console.Write("Enter Student ID: ");
            int studentId = ReadInt();

            Console.Write("Enter Course ID: ");
            int courseId = ReadInt();

            if (manager.EnrollStudentInCourse(studentId, courseId))
                Console.WriteLine("Student enrolled successfully.");
            else
                Console.WriteLine("Student or course not found, or student already enrolled.");
        }

        static void ViewAllCourses()
        {
            Console.WriteLine("\n--- All Courses ---");

            if (manager.Courses.Count == 0)
            {
                Console.WriteLine("No courses found.");
                return;
            }

            foreach (Course course in manager.Courses)
            {
                Console.WriteLine(course.PrintDetails());
                Console.WriteLine("--------------------");
            }
        }

        static void ViewAllInstructors()
        {
            Console.WriteLine("\n--- All Instructors ---");

            if (manager.Instructors.Count == 0)
            {
                Console.WriteLine("No instructors found.");
                return;
            }

            foreach (Instructor instructor in manager.Instructors)
            {
                Console.WriteLine(instructor.PrintDetails());
                Console.WriteLine("--------------------");
            }
        }
    }
}