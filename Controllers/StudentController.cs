using Microsoft.AspNetCore.Mvc;
using danh_gia_csharp.model;
using danh_gia_csharp.service.StudentService;
using System.Threading.Tasks;
using danh_gia_csharp.service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using cloud.core;
using cloud.core.mongodb;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using danh_gia_csharp.middleware;
namespace danh_gia_csharp.api
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }


        [HttpGet("students-by-class")]
        public async Task<IActionResult> GetStudentsByClassId([FromQuery] string? classId, [FromQuery] int pageNumber, [FromQuery] int pageSize = 45)
        {
            if (pageSize <= 0 || pageSize == null)
            {
                return BadRequest("Page size must be already and greater than 0.");
            }
            if (pageNumber <= 0 || pageNumber == null)
            {
                return BadRequest("pageNumber size must be already and greater than 0.");
            }

            var pageItem = await _studentService.GetStudentsByClassIdAsync(classId, pageNumber, pageSize);
            return Ok(pageItem);
        }




        /// chay tren post man

        [HttpPost("upload-avatar/{studentId}")]
        public async Task<IActionResult> AddStudent(String studentId, [FromForm] IFormFile avatarFile)
        {
            Student student = await _studentService.findStudent(studentId);

            if (student == null)
                return BadRequest("Student data is not exist.");


            if (avatarFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                var fileExtension = Path.GetExtension(avatarFile.FileName).ToLower();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Only jpg, jpeg, and png images are allowed.");
                }


                var avatarPath = await UploadAvatarAsync(avatarFile);
                if (!string.IsNullOrEmpty(avatarPath))
                {
                    student.Avatar = avatarPath;
                }
            }
            else
            {
                return BadRequest("file avatar is not exist.");
            }

            try
            {

                var addedStudent = await _studentService.UpdateStudentAsync(student);
                return Ok(addedStudent);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        public async Task<string> UploadAvatarAsync(IFormFile avatarFile)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");


            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            var fileExtension = Path.GetExtension(avatarFile.FileName);
            var newFileName = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(uploads, newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            return "/avatars/" + newFileName;
        }









        [HttpPost("add")]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {

            if (student == null)
                return BadRequest("Student data is null.");


            try
            {

                var addedStudent = await _studentService.AddStudentAsync(student);
                return Ok(addedStudent);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

       


        [HttpPut("update/{guid}")]
        public async Task<IActionResult> UpdateStudent([FromRoute] string guid, [FromBody] Student student)
        {
            if (!Guid.TryParse(guid, out Guid parsedGuid))
            {
                return BadRequest("Invalid Guid format.");
            }
            var existingStudent = await _studentService.findStudent(guid);
            if (existingStudent == null)
            {
                return BadRequest("Student data is not existed.");
            }
            student.Guid = parsedGuid;


            try
            {
                var updatedStudent = await _studentService.UpdateStudentAsync(student);
                return Ok(updatedStudent);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



        [HttpDelete("delete/{studentId}")]
        public async Task<IActionResult> DeleteStudent(string studentId)
        {
            if (string.IsNullOrEmpty(studentId))
                return BadRequest("StudentId is required.");

            try
            {
                var result = await _studentService.DeleteStudentAsync(studentId);
                if (result)
                    return Ok("deleted");
                else
                    return NotFound("Student not found.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



        [HttpPost("add-to-class/{classId}/{studentId}")]
        public async Task<IActionResult> AddStudentToClass(string classId, string studentId)
        {
            var result = await _studentService.AddStudentIntoClass(classId, studentId);
            if (result)
                return Ok("Student added to class successfully.");
            else
                return BadRequest("Error adding student to class.");
        }


        //}
        //}
    }
}

