namespace EngineeringThesisAPI.Models.PaginationModel
{
    public class PaginationModel<T>
    {
        public int PageIndex { get; }
        public int TotalPages { get; }
        public List<T> Items { get; }

        public PaginationModel(List<T> items, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(items.Count / (double)pageSize);

            Items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static PaginationModel<T> Create(List<T> items, int pageIndex, int pageSize)
        {
            return new PaginationModel<T>(items, pageIndex, pageSize);
        }

    }
}
