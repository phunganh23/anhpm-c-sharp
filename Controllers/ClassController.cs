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
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> InsertClass([FromBody] Class newClass)
        {
            if (newClass == null)
            {
                return BadRequest("Class data is null.");
            }

            try
            {
                var insertedClass = await _classService.insertClass(newClass);
                return Ok(insertedClass); 
            }
            catch (System.Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllClasses()
        {
            try
            {
                
                var classes = await _classService.getAll();

               
                return Ok(classes);
            }
            catch (Exception ex)
            {
             
                return BadRequest($"Error: {ex.Message}");
            }
        }



    }
}