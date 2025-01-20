namespace danh_gia_csharp.service {
    public class PageItem<T> {

        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
