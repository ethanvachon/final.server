using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using final.server.Models;

namespace final.server.Repositories
{
  public class KeepsRepository
  {
    private readonly IDbConnection _db;

    public KeepsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal IEnumerable<Keep> GetAll()
    {
      string sql = @"
        SELECT
        k.*,
        pr.*
        FROM keeps k
        JOIN profiles pr ON k.creatorId = pr.id;";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) => { keep.Creator = profile; return keep; }, splitOn: "id");
    }

    internal Keep GetOne(int id)
    {
      string sql = @"
      SELECT
      k.*,
      pr.*
      FROM keeps k
      JOIN profiles pr ON k.creatorId = pr.Id
      WHERE k.Id = @id;";
      return _db.Query<Keep, Profile, Keep>(sql, (keep, profile) => { keep.Creator = profile; return keep; }, new { id }, splitOn: "id").FirstOrDefault();
    }

    internal int Create(Keep newKeep)
    {
      string sql = @"
      INSERT INTO keeps
      (creatorId, name, description, img, views, keeps)
      VALUES
      (@CreatorId, @Name, @Description, @Img, @Views, @Keeps);
      SELECT LAST_INSERT_ID();";
      return _db.ExecuteScalar<int>(sql, newKeep);
    }

    internal Keep Edit(Keep editKeep)
    {
      string sql = @"
      UPDATE keeps
      SET
      name = @Name,
      description = @Description,
      img = @Img
      WHERE id = @id;";
      _db.Execute(sql, editKeep);
      return editKeep;
    }

    internal void Delete(int keepId)
    {
      string sql = "DELETE FROM keeps WHERE id = @keepId LIMIT 1";
      _db.Execute(sql, new { keepId });
    }
  }
}