using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using cloud.core.mongodb;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

namespace danh_gia_csharp.model {
    public class Student : AbstractEntityObjectIdTracking {
        
        
        [SwaggerIgnore]
        public Guid Guid { get; set; }


        [DefaultValue("")]
        public String? ClassId { get; set; }

     

        [Required]
        [DefaultValue("")]
        [MaxLength(150)]
        [RegularExpression(@"^[a-zA-Z0-9\sÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂƠưăợƠƯƠỪƠ]*$",
            ErrorMessage = "FirstName contains wrong characters..")]
        public string FirstName { get; set; } 

        [Required]
        [DefaultValue("")]
        [MaxLength(150)]
        [RegularExpression(@"^[a-zA-Z0-9\sÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂƠưăợƠƯƠỪƠ]*$",
            ErrorMessage = "LastName contains wrong characters..")]
        public string LastName { get; set; } 

      
        [Range(1, 2)]
        [DefaultValue(1)]
        public short Gender { get; set; } = 1; 

        [DataType(DataType.Date)]
        [Column(TypeName = "datetime")]
        [DefaultValue("")]
        public DateTime? DayOfBirth { get; set; }

        [MaxLength(500)]
        [Url]
        [DefaultValue("")]
        [SwaggerIgnore]
        public string? Avatar { get; set; } 
    }
}
