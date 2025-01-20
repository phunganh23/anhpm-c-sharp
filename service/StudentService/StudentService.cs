using danh_gia_csharp.ConnectDB;
using danh_gia_csharp.model;
using Microsoft.AspNetCore.Mvc.RazorPages;
using danh_gia_csharp.service;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using cloud.core;
using cloud.core.mongodb;
using MongoDB.Bson;
using MongoDB.Driver.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Microsoft.EntityFrameworkCore;
namespace danh_gia_csharp.service.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly MongoDbContext<Student> mongoDbContext;
        private readonly MongoDbContext<Class> mongoDbContextClass;

        public StudentService(MongoDbContext<Class> mongoDbContextClass, MongoDbContext<Student> mongoDbContext)
        {
            this.mongoDbContext = mongoDbContext;
            this.mongoDbContextClass = mongoDbContextClass;
        }


        public async Task<PageItem<Student>> GetStudentsByClassIdAsync(string? classId, int pageNumber, int pageSize)
        {
            try
            {
                var query = mongoDbContext.DbContext.AsQueryable();


                if (classId != null)
                {
                    query = query.Where(s => s.ClassId == classId);
                }


                var pagedStudents = query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();


                var sortedStudents = pagedStudents
                    .OrderBy(s => s.LastName, StringComparer.Create(new System.Globalization.CultureInfo("vi-VN"), false))
                    .ToList();


                var totalRecords = query.Count();

                return new PageItem<Student>
                {
                    TotalRecords = totalRecords,
                    Items = sortedStudents,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi lấy danh sách học sinh: {ex.Message}");
                throw;
            }
        }



        public async Task<Student> AddStudentAsync(Student student)
        {
            var classId = student.ClassId;

            if (classId != null && classId != "")
            {
                if (!await CheckClassExistsAsync(classId))
                {
                    Console.WriteLine($"không tồn tại lớp học này");
                    throw new InvalidOperationException($"Lớp học với ID {classId} không tồn tại.");

                }
            }

            try
            {

                student.Guid = Guid.NewGuid();



                await mongoDbContext.DbContext.Insert(student);
                return student;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm học sinh: {ex.Message}");
                throw;
            }
        }

        //    private int GenerateUniqueCode()
        //    {
        //        var random = new Random();
        //        int newCode;

        //        do
        //        {
        //            newCode = random.Next(100000, 999999); // Tạo mã số ngẫu nhiên 6 chữ số
        //        }
        //        while (mongoDbContext.DbContext.AsQueryable().Any(s => s.code == newCode));

        //        return newCode;
        //    }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            try
            {
                if (student.ClassId != null && student.ClassId != "")
                {
                    var classExists = await CheckClassExistsAsync(student.ClassId);
                    if (!classExists)
                    {
                        throw new Exception("Lớp học không tồn tại.");
                    }
                }


                var existingStudent = await mongoDbContext.DbContext
                    .Where(s => s.Guid == student.Guid)
                    .FirstOrDefaultAsync();

                if (existingStudent == null)
                {
                    throw new Exception("Học sinh không tồn tại.");
                }


                existingStudent.FirstName = student.FirstName;
                existingStudent.LastName = student.LastName;
                existingStudent.DayOfBirth = student.DayOfBirth;
                existingStudent.Gender = student.Gender;
                existingStudent.Avatar = student.Avatar;
                existingStudent.ClassId = student.ClassId;


                await mongoDbContext.DbContext.Update(existingStudent);

                return existingStudent;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi cập nhật học sinh: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteStudentAsync(string studentId)
        {
            try
            {
                Student student = await findStudent(studentId);
                if (student != null)
                {
                    await mongoDbContext.DbContext.Delete(student.Id);
                }
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa học sinh: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CheckClassExistsAsync(string classId)
        {



            if (Guid.TryParse(classId, out Guid parsedClassId))
            {
                var classExists = await mongoDbContextClass.DbContext
                    .Where(c => c.Guid == parsedClassId)
                    .FirstOrDefaultAsync();

                if (classExists != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }


        }

        public async Task<Student> findStudent(string studentId)
        {

            if (Guid.TryParse(studentId, out Guid parsedGuid))
            {
                var student = await mongoDbContext.DbContext
                    .Where(c => c.Guid == parsedGuid)
                    .FirstOrDefaultAsync();

                if (student != null)
                {
                    return student;
                }
                return null;
            }
            else
            {
                return null;
            }
        }


        public async Task<bool> AddStudentIntoClass(string classId, string studentId)
        {
            try
            {

                Student exitstStudent = null;
                if (Guid.TryParse(studentId, out Guid parsedGuidStudent))
                {
                    exitstStudent = await mongoDbContext.DbContext
                        .Where(c => c.Guid == parsedGuidStudent)
                        .FirstOrDefaultAsync();

                    if (exitstStudent == null)
                    {
                        return false;
                    }


                    if (Guid.TryParse(classId, out Guid parsedGuidClass))
                    {
                        var exitstClass = await mongoDbContextClass.DbContext
                            .Where(c => c.Guid == parsedGuidClass)
                            .FirstOrDefaultAsync();

                        if (exitstClass == null)
                        {
                            return false;
                        }


                        exitstStudent.ClassId = classId;


                        await UpdateStudentAsync(exitstStudent);

                        return true;
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm học sinh vào lớp: {ex.Message}");
                return false;
            }
            return false;
        }



    }
}
