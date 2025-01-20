using danh_gia_csharp.model;

namespace danh_gia_csharp.service {
    public interface IStudentService {

        Task<PageItem<Student>> GetStudentsByClassIdAsync(string? classId, int pageNumber, int pageSize);
        Task<Student> AddStudentAsync(Student student);
        Task<Student> UpdateStudentAsync(Student student );
        Task<Student> findStudent(string studentId);
        Task<bool> DeleteStudentAsync(string studentId);
        //Task<bool> CheckClassExistsAsync(int classId);
        Task<bool> AddStudentIntoClass(string classId , string studentId);
    }
}
