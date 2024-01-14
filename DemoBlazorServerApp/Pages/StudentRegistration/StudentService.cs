// StudentService.cs
using DemoBlazorServerApp;
using DemoBlazorServerApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class StudentService : IStudentService
{
    private readonly DatabaseContext _dbContext;

    public StudentService(DatabaseContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task AddStudentAsync(StudentRegistration student)
    {
        if (student == null)
        {
            throw new ArgumentNullException(nameof(student));
        }

        try
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                // Map specializations and users
                MapSpecialization(student);
                MapUser(student);

                // Set registration date
                student.RegisterDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                // Add student to DbSet
                _dbContext.Students.Add(student);

                // Save changes to the database
                await _dbContext.SaveChangesAsync();

                // Commit the transaction
                await transaction.CommitAsync();
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine($"Error adding student: {ex.Message}");

            // Optionally, rethrow the exception for further handling
            throw;
        }
    }

    public async Task<List<Specialization>> GetSpecializationsAsync()
    {
        // Fetch specializations from DbSet
        return await _dbContext.Specs.ToListAsync();
    }

    public async Task<List<User>> GetUsersAsync()
    {
        // Fetch users from DbSet
        return await _dbContext.Users.ToListAsync();
    }

    private void MapSpecialization(StudentRegistration student)
    {
        // Map specialization details based on SpecId
        var selectedSpecialization = _dbContext.Specs.FirstOrDefault(s => s.SpecId == long.Parse(student.SpecId));
        if (selectedSpecialization != null)
        {
            student.Specialization = selectedSpecialization.Name;
            student.Tax = selectedSpecialization.Tax;
            student.Years = selectedSpecialization.Years;
        }
    }

    private void MapUser(StudentRegistration student)
    {
        // Map user details based on UserId
        var selectedUser = _dbContext.Users.FirstOrDefault(u => u.UserId == long.Parse(student.UserId));
        if (selectedUser != null)
        {
            student.FullName = selectedUser.FullName;
            student.Years = selectedUser.Email;
        }
    }
}

public interface IStudentService
{
}