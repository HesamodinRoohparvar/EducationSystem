namespace EducationSystem.Application.Common.Models
{
    public record PagingRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
