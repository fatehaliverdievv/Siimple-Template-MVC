namespace Siimple.ViewModels
{
    public class PaginateVM<T>
    {
        public int MaxPage { get; set; }
        public int CurrentPage { get; set; }
        public ICollection<T> Items { get; set;}
    }
}
