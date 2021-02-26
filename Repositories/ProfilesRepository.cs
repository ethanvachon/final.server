using System.Data;
using Dapper;
using final.server.Models;

namespace final.server.Repositories
{
  public class ProfilesRepository
  {
    private readonly IDbConnection _db;

    public ProfilesRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Profile GetById(string id)
    {
      string sql = @"SELECT * FROM profiles WHERE id = @id;";
      return _db.QueryFirstOrDefault<Profile>(sql, new { id });
    }

    internal Profile Create(Profile userInfo)
    {
      string sql = @"
      INSERT INTO profiles
      (name, picture, email, id)
      VALUES
      (@Name, @Picture, @Email, @Id)
      ;";
      _db.Execute(sql, userInfo);
      return userInfo;
    }
  }
}