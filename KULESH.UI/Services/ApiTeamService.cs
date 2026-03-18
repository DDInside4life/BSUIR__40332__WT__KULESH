using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KULESH.Domain.Entities;
using KULESH.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace KULESH.UI.Services
{
    public class ApiTeamService : ITeamService
    {
        private readonly HttpClient _http;

        public ApiTeamService(HttpClient http)
        {
            _http = http;
        }

        public async Task<ResponseData<FootballTeam>> CreateTeamAsync(FootballTeam product, IFormFile? formFile)
        {
            try
            {
                // 1. Отправляем объект в JSON
                var response = await _http.PostAsJsonAsync("api/FootballTeams", product);
                if (!response.IsSuccessStatusCode)
                {
                    return ResponseData<FootballTeam>.Error($"Ошибка при создании: {response.StatusCode}");
                }

                // 2. Читаем созданный объект (с ID)
                var createdTeam = await response.Content.ReadFromJsonAsync<FootballTeam>();
                if (createdTeam == null)
                {
                    return ResponseData<FootballTeam>.Error("Не удалось прочитать ответ API");
                }

                // 3. Если есть файл, отправляем его
                if (formFile != null && formFile.Length > 0)
                {
                    // Создаем multipart-контент
                    using var content = new MultipartFormDataContent();
                    using var streamContent = new StreamContent(formFile.OpenReadStream());
                    content.Add(streamContent, "image", formFile.FileName); // имя поля "image", как в примере

                    // Отправляем POST на тот же ресурс с ID (предполагаем, что API принимает изображение по ID)
                    var imageResponse = await _http.PostAsync($"api/FootballTeams/{createdTeam.Id}", content);
                    if (!imageResponse.IsSuccessStatusCode)
                    {
                        // Можно вернуть ошибку, но объект уже создан. По заданию, вероятно, нужно сообщить об ошибке.
                        return ResponseData<FootballTeam>.Error($"Объект создан, но не удалось загрузить изображение: {imageResponse.StatusCode}");
                    }
                }

                return ResponseData<FootballTeam>.OK(createdTeam);
            }
            catch (Exception ex)
            {
                return ResponseData<FootballTeam>.Error(ex.Message);
            }
        }

        public async Task DeleteTeamAsync(int id)
        {
            var resp = await _http.DeleteAsync($"api/FootballTeams/{id}");
            resp.EnsureSuccessStatusCode();
        }

        public async Task<ResponseData<FootballTeam>> GetTeamByIdAsync(int id)
        {
            try
            {
                var resp = await _http.GetAsync($"api/FootballTeams/{id}");
                if (resp.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return ResponseData<FootballTeam>.Error("Not found");
                }

                resp.EnsureSuccessStatusCode();
                var team = await resp.Content.ReadFromJsonAsync<FootballTeam>();
                return ResponseData<FootballTeam>.OK(team!);
            }
            catch (Exception ex)
            {
                return ResponseData<FootballTeam>.Error(ex.Message);
            }
        }

        public async Task<ResponseData<List<FootballTeam>>> GetTeamListAsync(string? category)
        {
            try
            {
                string url = "api/FootballTeams";
                if (!string.IsNullOrWhiteSpace(category))
                {
                    url += "?category=" + Uri.EscapeDataString(category);
                }

                var resp = await _http.GetAsync(url);
                if (!resp.IsSuccessStatusCode)
                {
                    return ResponseData<List<FootballTeam>>.Error($"API error: {(int)resp.StatusCode} {resp.ReasonPhrase}");
                }

                // API returns ResponseData<List<FootballTeam>> already
                var data = await resp.Content.ReadFromJsonAsync<ResponseData<List<FootballTeam>>>();
                if (data == null)
                    return ResponseData<List<FootballTeam>>.Error("Invalid response from API");

                return data;
            }
            catch (Exception ex)
            {
                return ResponseData<List<FootballTeam>>.Error(ex.Message);
            }
        }

        public async Task UpdateTeamAsync(int id, FootballTeam product, IFormFile? formFile)
        {
            var resp = await _http.PutAsJsonAsync($"api/FootballTeams/{id}", product);
            resp.EnsureSuccessStatusCode();
        }
    }
}
