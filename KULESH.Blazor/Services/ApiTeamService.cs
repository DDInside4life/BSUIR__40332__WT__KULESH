using System.Net.Http.Json;
using KULESH.Domain.Entities;
using KULESH.Domain.Models;

namespace KULESH.Blazor.Services
{
    public class ApiTeamService(HttpClient http) : ITeamService<FootballTeam>
    {
        private List<FootballTeam> _teams = new();
        private int _currentPage = 1;
        private int _totalPages = 1;

        public event Action? ListChanged;

        public IEnumerable<FootballTeam> Teams => _teams;
        public int CurrentPage => _currentPage;
        public int TotalPages => _totalPages;

        public async Task GetTeams(int pageNo = 1, int pageSize = 3)
        {
            try
            {
                var response = await http.GetAsync("api/FootballTeams");
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<ResponseData<List<FootballTeam>>>();
                    if (responseData?.Success == true && responseData.Data != null)
                    {
                        var allTeams = responseData.Data;
                        _totalPages = (int)Math.Ceiling(allTeams.Count / (double)pageSize);
                        _currentPage = pageNo;
                        _teams = allTeams
                            .Skip((pageNo - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
                    }
                    else
                    {
                        _teams = new List<FootballTeam>();
                        _currentPage = 1;
                        _totalPages = 0;
                    }
                }
                else
                {
                    _teams = new List<FootballTeam>();
                    _currentPage = 1;
                    _totalPages = 0;
                }
            }
            catch
            {
                _teams = new List<FootballTeam>();
                _currentPage = 1;
                _totalPages = 0;
            }
            finally
            {
                ListChanged?.Invoke();
            }
        }

        // Для совместимости с интерфейсом (если требуется)
        public async Task<ResponseData<List<FootballTeam>>> GetTeamListAsync(string? category)
        {
            await GetTeams();
            return ResponseData<List<FootballTeam>>.OK(_teams);
        }

        public Task<ResponseData<FootballTeam>> GetTeamByIdAsync(int id) => throw new NotImplementedException();
        public Task<ResponseData<FootballTeam>> CreateTeamAsync(FootballTeam product, IFormFile? formFile) => throw new NotImplementedException();
        public Task UpdateTeamAsync(int id, FootballTeam product, IFormFile? formFile) => throw new NotImplementedException();
        public Task DeleteTeamAsync(int id) => throw new NotImplementedException();
    }
}