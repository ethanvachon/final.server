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

    internal object GetOne(int vaultId, string userId)
    {

      Vault vault = _repo.Get(vaultId);
      if (vault == null)
      {
        throw new Exception("invalid id");
      }
      if (vault.CreatorId != userId)
      {
        if (vault.IsPrivate)
        {
          throw new Exception("not authorized");
        }
        return vault;
      }
      return vault;
    }

    internal Vault Create(Vault newVault)
    {
      throw new NotImplementedException();
    }

    internal object Edit(Vault editVault, string id)
    {
      throw new NotImplementedException();
    }

    internal object Delete(int id1, string id2)
    {
      throw new NotImplementedException();
    }
  }
}