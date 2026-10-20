using DAL.Data.Interfaces;
using DAL.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;
        public StudentRepository(IConfiguration configuration) {

            _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found");


        }

        public async Task<int> CreateAsync(Student student)
        {
            const string sql = """
                INSERT INTO Student (StudentName, StudentSurname, StudentEmail)
                VALUES (@StudentName, @StudentSurname, @StudentEmail);
                SELECT SCOPE_IDENTITY();
                """;

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@StudentName", student.StudentName);
            command.Parameters.AddWithValue("@StudentSurname", student.StudentSurname);
            command.Parameters.AddWithValue("@StudentEmail", student.StudentEmail);


            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();

            return Convert.ToInt32(result);

        }

        public async Task<IReadOnlyList<Student>> GetAllAsync()
        { 
            var students = new List<Student>();
            const string sql = """
                                SELECT StudentID, StudentName, StudentSurname, 
                                    StudentEmail 
                                FROM Student ORDER BY StudentName
                                """;

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var student = new Student
                {
                    StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    StudentSurname = reader.GetString(reader.GetOrdinal("StudentSurname")),
                    StudentEmail = reader.GetString(reader.GetOrdinal("StudentEmail"))
                };
                students.Add(student);
            }

            return students;
        }

        public async Task<Student?> GetByIdAsync(int StudentID)
        {
            var student = new Student();
            //SQL Clouse
            const string sql = """
                                SELECT StudentID, StudentName, StudentSurname, 
                                    StudentEmail 
                                FROM Student
                                WHERE StudentID = @StudentID
                                ORDER BY StudentName 
                                """;

            // SQL Connection
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            // Add Parameter
            command.Parameters.AddWithValue("@StudentID", StudentID);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            //Read Data
            if(await  reader.ReadAsync())
            {
                student = new Student
                {
                    StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    StudentSurname = reader.GetString(reader.GetOrdinal("StudentSurname")),
                    StudentEmail = reader.GetString(reader.GetOrdinal("StudentEmail"))

                };

                return student;

            }

            return null;

        }

        public async Task<Student?> GetByNameAsync(string StudentName)
        {
            var student = new Student();
            //SQL Clouse
            const string sql = """
                                SELECT StudentID, StudentName, StudentSurname, 
                                    StudentEmail 
                                FROM Student
                                WHERE StudentName = @StudentName
                                ORDER BY StudentName 
                                """;

            // SQL Connection
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(sql, connection);

            // Add Parameter
            command.Parameters.AddWithValue("@StudentName", StudentName);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            //Read Data
            if (await reader.ReadAsync())
            {
                student = new Student
                {
                    StudentID = reader.GetInt32(reader.GetOrdinal("StudentID")),
                    StudentName = reader.GetString(reader.GetOrdinal("StudentName")),
                    StudentSurname = reader.GetString(reader.GetOrdinal("StudentSurname")),
                    StudentEmail = reader.GetString(reader.GetOrdinal("StudentEmail"))

                };

                return student;

            }

            return null;

        }

    }
}
