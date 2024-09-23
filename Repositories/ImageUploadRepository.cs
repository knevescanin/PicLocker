using BlazorApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BlazorApp.Repositories;
public interface IImageUploadRepository
{
    Task UploadImageToDb(ImageFile image, string userId);
    Task UploadImageToOtherUsers(ImageFile image, string userId);
    Task<IEnumerable<ImageFile>> GetImages(string userId);
    Task<IEnumerable<ImageFile>> GetImagesFromOtherUsers(string userId);
    Task<string> GetUsername(ImageFile image);
    Task DeleteImageFromDb(int imageId, string userId);
    Task DeleteImageFromOtherUsers(string imageName);
}

public class ImageUploadRepository : IImageUploadRepository
{
    private readonly IConfiguration _config;

    const string CONN_KEY = "BlazorAppIdentityDbContextConnection";

    public ImageUploadRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task UploadImageToDb(ImageFile image, string userId)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY); 
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "INSERT INTO ImageFile (ImageName, UserId, DateUploaded) VALUES (@ImageName, @UserId, @DateUploaded)";
        await connection.ExecuteAsync(sql, new { ImageName = image.ImageName, UserId = userId, DateUploaded = image.DateUploaded });
    }

     public async Task UploadImageToOtherUsers(ImageFile image, string userId)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY); 
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "INSERT INTO ImageFile_UserAccess (ImageName, UserId, DateUploaded) VALUES (@ImageName, @UserId, @DateUploaded)";
        await connection.ExecuteAsync(sql, new { ImageName = image.ImageName, UserId = userId, DateUploaded = image.DateUploaded });
    }

    public async Task<IEnumerable<ImageFile>> GetImages(string userId)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY);
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "SELECT * FROM ImageFile WHERE UserId = @UserId ORDER BY DateUploaded DESC";
        var images = await connection.QueryAsync<ImageFile>(sql, new { UserId = userId });

        return images;
    }

    public async Task<IEnumerable<ImageFile>> GetImagesFromOtherUsers(string userId)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY);
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "SELECT * FROM ImageFile_UserAccess WHERE UserId = @UserId ORDER BY DateUploaded DESC";
        var images = await connection.QueryAsync<ImageFile>(sql, new { UserId = userId });

        return images;
    }

    public async Task<string> GetUsername(ImageFile image)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY);
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "SELECT u.UserName FROM AspNetUsers u JOIN ImageFile i ON u.Id = i.UserId JOIN ImageFile_UserAccess a ON i.ImageName = a.ImageName WHERE a.ImageName = @ImageName";
        var username = await connection.QueryFirstOrDefaultAsync<string>(sql, new { ImageName = image.ImageName });

        return username;
    }

     public async Task DeleteImageFromDb(int imageId, string userId)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY);
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "DELETE FROM ImageFile WHERE Id = @Id AND UserId = @UserId";
        await connection.ExecuteAsync(sql, new { Id = imageId, UserId = userId });
    }

     public async Task DeleteImageFromOtherUsers(string imageName)
    {
        var connectionString = _config.GetConnectionString(CONN_KEY);
        using IDbConnection connection = new SqlConnection(connectionString);
        string sql = "DELETE FROM ImageFile_UserAccess WHERE ImageName = @ImageName";
        await connection.ExecuteAsync(sql, new { ImageName = imageName });
    }
}