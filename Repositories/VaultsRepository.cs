using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using final.server.Models;

namespace final.server.Repositories
{
  public class VaultsRepository
  {
    private readonly IDbConnection _db;

    public VaultsRepository(IDbConnection db)
    {
      _db = db;
    }

    internal Vault Get(int vaultId)
    {
      string sql = @"
      SELECT 
      v.*,
      pr.*
      FROM vaults v
      JOIN profiles pr On v.creatorId = pr.id
      WHERE v.id = @vaultId;";
      return _db.Query<Vault, Profile, Vault>(sql, (vault, profile) => { vault.Creator = profile; return vault; }, new { vaultId }, splitOn: "id").FirstOrDefault();
    }

    internal int Create(Vault newVault)
    {
      string sql = @"
      INSERT INTO vaults
      (name, description, isPrivate, creatorId)
      VALUES
      (@Name, @Description, @IsPrivate, @CreatorId);
      SELECT LAST_INSERT_ID()";
      return _db.ExecuteScalar<int>(sql, newVault);
    }

    internal Vault Edit(Vault editVault)
    {
      string sql = @"
      UPDATE vaults
      SET
      name = @Name,
      description = @Description,
      isPrivate = @IsPrivate
      WHERE id = @id;";
      _db.Execute(sql, editVault);
      return editVault;
    }

    internal void Delete(int id)
    {
      string sql = "DELETE FROM vaults WHERE id = @id LIMIT 1";
      _db.Execute(sql, new { id });
    }

    internal IEnumerable<Vault> GetByProfile(string id)
    {
      string sql = @"
      SELECT 
      v.*,
      pr.*
      FROM vaults v
      JOIN profiles pr On v.creatorId = pr.id
      WHERE v.creatorId = @id;";
      return _db.Query<Vault, Profile, Vault>(sql, (vault, profile) => { vault.Creator = profile; return vault; }, new { id }, splitOn: "id");
    }
  }
}