namespace KULESH.Blazor.Services
{
    public interface ITeamService<T> where T : class
    {
        event Action ListChanged;

        // List of objects
        IEnumerable<T> Teams { get; }

        // Number of actual page
        int CurrentPage { get; }

        // Total anount of pages
        int TotalPages { get; }

        // Getting the list of objects
        Task GetTeams(int pageNo = 1, int pageSize = 3);
    }
}
