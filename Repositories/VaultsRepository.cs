using System;
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
      JOIN profiles pr On vault.creatorId = profile.id
      WHERE vault.id = @vaultId;";
      return _db.Query<Vault, Profile, Vault>(sql, (vault, profile) => { vault.Creator = profile; return vault; }, new { vaultId }, splitOn: "id").FirstOrDefault();
    }
  }
}