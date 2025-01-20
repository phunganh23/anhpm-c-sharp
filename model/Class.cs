using cloud.core.mongodb;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel;
namespace danh_gia_csharp.model
{

    public class Class : AbstractEntityObjectIdTracking
    {

        [SwaggerIgnore] 
        public Guid Guid { get; set; }


        [Required]
        [MaxLength(150)]
        [DefaultValue("")]
        public string Name { get; set; }

        [SwaggerIgnore]
        public ICollection<Student> Students { get; set; } = new List<Student>();
    }

}
