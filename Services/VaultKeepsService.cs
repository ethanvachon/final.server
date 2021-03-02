using System;
using final.server.Models;
using final.server.Repositories;

namespace final.server.Services
{
  public class VaultKeepsService
  {
    private readonly VaultKeepsRepository _repo;
    private readonly VaultsRepository _vrepo;

    private readonly KeepsRepository _krepo;

    public VaultKeepsService(VaultKeepsRepository repo, VaultsRepository vrepo, KeepsRepository krepo)
    {
      _repo = repo;
      _vrepo = vrepo;
      _krepo = krepo;
    }

    internal VaultKeep Create(VaultKeep newVaultKeep, string userId)
    {
      Vault vault = _vrepo.Get(newVaultKeep.VaultId);
      Keep keep = _krepo.GetOne(newVaultKeep.KeepId);
      if (keep == null)
      {
        throw new Exception("invalid keep id");
      }
      if (vault == null)
      {
        throw new Exception("invalid vault id");
      }
      if (vault.CreatorId != userId)
      {
        throw new Exception("you are not the owner of this vault");
      }
      int id = _repo.Create(newVaultKeep);
      newVaultKeep.Id = id;
      keep.Keeps = keep.Keeps + 1;
      _krepo.AddKeep(keep);
      return newVaultKeep;
    }
    internal void delete(int id, string userId)
    {
      VaultKeep toDelete = _repo.GetById(id);
      if (toDelete == null)
      {
        throw new Exception("invalid id");
      }
      if (toDelete.CreatorId != userId)
      {
        throw new Exception("cannot delete if you are not the creator");
      }
      _repo.Delete(id);
    }

  }
}