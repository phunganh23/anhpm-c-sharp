using danh_gia_csharp.model;

namespace danh_gia_csharp.service {
    public interface IClassService {

        Task<Class> insertClass(Class newClass);
        Task<List<Class>> getAll();

    }
}
