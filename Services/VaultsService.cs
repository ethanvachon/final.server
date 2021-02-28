using System;
using final.server.Models;
using final.server.Repositories;

namespace final.server.Services
{
  public class VaultsService
  {
    private readonly VaultsRepository _repo;

    public VaultsService(VaultsRepository repo)
    {
      _repo = repo;
    }

    internal Vault GetOne(int vaultId)
    {

      Vault vault = _repo.Get(vaultId);
      if (vault == null)
      {
        throw new Exception("invalid id");
      }
      if (vault.IsPrivate)
      {
        throw new Exception("not authorized");
      }
      return vault;
    }
    internal Vault GetOne(int vaultId, string userId)
    {

      Vault vault = _repo.Get(vaultId);
      if (vault == null)
      {
        throw new Exception("invalid id");
      }
      if (vault.IsPrivate)
      {
        if (vault.CreatorId == userId)
        {
          return vault;
        }
        throw new Exception("not authorized");
      }
      return vault;
    }

    internal Vault Create(Vault newVault)
    {
      newVault.Id = _repo.Create(newVault);
      return newVault;
    }

    internal Vault Edit(Vault editVault, string id)
    {
      Vault vault = _repo.Get(editVault.Id);
      if (vault == null)
      {
        throw new Exception("invalid id");
      }
      if (vault.CreatorId != id)
      {
        throw new Exception("cannot edit Vault if you aren't the creator");
      }
      return _repo.Edit(editVault);
    }

    internal object GetByProfile(string id)
    {
      throw new NotImplementedException();
    }

    internal string Delete(int id, string accountId)
    {
      Vault preDelete = _repo.Get(id);
      if (preDelete == null)
      {
        throw new Exception("invalid id");
      }
      if (preDelete.CreatorId != accountId)
      {
        throw new Exception("cannot delete if you are not the creator");
      }
      _repo.Delete(id);
      return "deleted";
    }
  }
}