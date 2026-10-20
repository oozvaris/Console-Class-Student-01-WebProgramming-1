using DAL.Data;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace Console_Class_Student_01_ProgrammingBasic2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .Build();

            var studentRepository = new StudentRepository(configuration);
            var studentService = new StudentService(studentRepository);

           
            // Display menu
            bool exit = false;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("Menu:");
                    Console.WriteLine("1. Display all courses");
                    Console.WriteLine("2. Register a new course");
                    Console.WriteLine("3. Search course by ID");
                    Console.WriteLine("4. Search course by Name");

                    Console.WriteLine("5. Display all students");
                    Console.WriteLine("6. Register a new student");
                    Console.WriteLine("7. Search Student by ID");
                    Console.WriteLine("8. Search Student by Name");


                    Console.WriteLine("10. Exit");
                    Console.Write("Enter your choice: ");
                    int choice = Convert.ToInt32(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            // deploy course display logic here                             
                            break;
                        case 2:
                            // deploy course registration logic here
                            break;
                        case 3:
                            // deploy course search by ID logic here
                            break;
                        case 4:
                            // deploy course search by Name logic here
                            break;
                        case 5:
                            studentService.DisplayAllStudentsAsync().Wait();
                            break;
                        case 6:
                            // deploy student registration logic here
                            DAL.Models.Student student = studentService.RegisterStudent();
                            studentService.AddStudentAsync(student).Wait();
                            break;
                        case 7:
                            Console.Write("Enter Student ID to search: ");
                            int studentId = Convert.ToInt32(Console.ReadLine());
                            studentService.DisplayStudentByIdAsync(studentId).Wait();
                            // deploy student search by ID logic here
                            break;
                        case 8:
                            Console.Write("Enter Student Name to search: ");
                            string studentName = Console.ReadLine();
                            studentService.DisplayStudentByNameAsync(studentName).Wait();
                            // deploy student search by Name logic here
                            break;
                        case 10:
                            exit = true;
                            Console.WriteLine("Exiting the program. Goodbye!");
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }

                }                
                catch (FormatException ex)
                {
                    Console.WriteLine("Invalid input format. Please enter a valid number.");
                    Console.WriteLine("Please try again.");
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    Console.WriteLine("Please try again.");
                    continue;
                }
                finally
                {
                    // Console.WriteLine("Console Application run successufully");

                }
            }

            Console.ReadLine();
        }
    
        public static bool isValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
