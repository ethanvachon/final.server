using System;
using final.server.Models;
using final.server.Repositories;

namespace final.server.Services
{
  public class VaultKeepsService
  {
    private readonly VaultKeepsRepository _repo;
    private readonly VaultsRepository _vrepo;

    public VaultKeepsService(VaultKeepsRepository repo, VaultsRepository vrepo)
    {
      _repo = repo;
      _vrepo = vrepo;
    }

    internal VaultKeep Create(VaultKeep newVaultKeep, string userId)
    {
      Vault vault = _vrepo.Get(newVaultKeep.VaultId);
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