using danh_gia_csharp.ConnectDB;
using danh_gia_csharp.model;
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
namespace danh_gia_csharp.service.StudentService {
    public class ClassService : IClassService {

      
        private readonly MongoDbContext<Class> mongoDbContext;
        private readonly MongoDbContext<Student> mongoDbContextStudent;

        // Inject MongoDbContext vào constructor
        public ClassService(MongoDbContext<Class> mongoDbContext, MongoDbContext<Student> mongoDbContextStudent)
        {
            this.mongoDbContext = mongoDbContext;
            this.mongoDbContextStudent = mongoDbContextStudent;
        }
        public async Task<List<Class>> getAll()
        {
            //var collection = mongoDbContext.GetCollection<Class>();
            //await collection.DeleteManyAsync(Builders<Class>.Filter.Empty);
            var classes = await mongoDbContext.DbContext.ToListAsync();
            foreach (var classObj in classes)
            {
                var classGuid = classObj.Guid;

                var students = await mongoDbContextStudent.DbContext
                    .Where(s => s.ClassId == classGuid.ToString())
                    .ToListAsync();

                classObj.Students = students;
            }
            return classes;
        }

        public async Task<Class> insertClass(Class newClass)
        {
        
            try
            {
                newClass.Guid = Guid.NewGuid();
                await mongoDbContext.DbContext.Insert(newClass); 
                return newClass;
            }
            catch (Exception ex)
            {
                
                throw new InvalidOperationException("Error inserting class into database.", ex);
            }
        }


    }
}
