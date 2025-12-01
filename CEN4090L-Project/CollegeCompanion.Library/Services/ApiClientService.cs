using CollegeCompanion.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using CEN4090L_Project.Models;
using System.Net.Http.Json;

namespace CollegeCompanion.Library.Services
{
    public class ApiClientService
    {
        private readonly HttpClient _httpClient;
        

        public ApiClientService(ApiClientOptions apiClientOptions)
        {
            _httpClient = new HttpClient();

            // Check if the property is null and throw a meaningful exception
            if (string.IsNullOrWhiteSpace(apiClientOptions.ApiBaseAdress))
            {
                throw new ArgumentException("ApiBaseAdress cannot be null or empty.", nameof(apiClientOptions.ApiBaseAdress));
            }
            
            _httpClient.BaseAddress = new System.Uri(apiClientOptions.ApiBaseAdress);
        }

        // Get all users
        public async Task<List<User>?> GetUsers()
        {
            return await _httpClient.GetFromJsonAsync<List<User>?>("/api/User");
        }

        // Get user by ID
        public async Task<User?> GetUserById(int id)
        {
            return await _httpClient.GetFromJsonAsync<User?>($"/api/User/{id}");
        }

        // Create new user
        public async Task CreateUser(User user)
        {
            //var response = await _httpClient.PostAsJsonAsync("/api/User", user);
            //response.EnsureSuccessStatusCode();
            //return await response.Content.ReadFromJsonAsync<User?>();

            await _httpClient.PostAsJsonAsync("/api/User", user);
        }

        // Update existing user
        public async Task UpdateUser(User user)
        {
            await _httpClient.PutAsJsonAsync($"/api/User", user);
        }

        // Delete user by ID
        public async Task DeleteUser(int id)
        {
            await _httpClient.DeleteAsync($"/api/User/{id}");
        }
    }
}
