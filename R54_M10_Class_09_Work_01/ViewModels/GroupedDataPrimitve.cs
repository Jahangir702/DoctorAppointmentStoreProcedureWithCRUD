using R54_M10_Class_09_Work_01.Models;

namespace R54_M10_Class_09_Work_01.ViewModels
{
    public class GroupedDataPrimitve<T>
    {
        public string Key { get; set; } = default!;
        public T Data { get; set; } = default!;
    }
}
