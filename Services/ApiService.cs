using System.Text;
using System.Text.Json;
using UsuariosRestApp.Models;

namespace UsuariosRestApp.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://jsonplaceholder.typicode.com/users";

    public ApiService()
    {
        _httpClient = new HttpClient();
    }

    // GET: Obtener lista de usuarios
    public async Task<List<Usuario>> GetUsuariosAsync()
    {
        var response = await _httpClient.GetAsync(BaseUrl);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json);
        return usuarios ?? new List<Usuario>();
    }

    // GET: Obtener un usuario por ID
    public async Task<Usuario?> GetUsuarioByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Usuario>(json);
    }

    // POST: Crear un nuevo usuario
    public async Task<Usuario?> CreateUsuarioAsync(Usuario nuevoUsuario)
    {
        var json = JsonSerializer.Serialize(nuevoUsuario);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(BaseUrl, content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Usuario>(responseJson);
    }

    // DELETE: Eliminar usuario por ID
    public async Task<bool> DeleteUsuarioAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        response.EnsureSuccessStatusCode();
        return response.IsSuccessStatusCode;
    }
}