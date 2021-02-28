using System;
using System.Collections;
using System.Collections.Generic;
using final.server.Exceptions;
using final.server.Models;
using final.server.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace final.server.Services
{
  public class KeepsService
  {
    private readonly KeepsRepository _repo;

    public KeepsService(KeepsRepository repo)
    {
      _repo = repo;
    }

    internal IEnumerable<Keep> GetAll()
    {
      IEnumerable<Keep> keeps = _repo.GetAll();
      return keeps;
    }

    internal Keep GetOne(int id)
    {
      Keep keep = _repo.GetOne(id);
      if (keep == null)
      {
        throw new Exception("invalid id");
      }
      return keep;
    }

    internal Keep Create(Keep newKeep)
    {
      newKeep.Id = _repo.Create(newKeep);
      return newKeep;
    }

    internal Keep Edit(Keep editKeep, string userId)
    {
      Keep preEdit = _repo.GetOne(editKeep.Id);
      if (preEdit == null)
      {
        throw new Exception("invalid id");
      }
      if (preEdit.CreatorId != userId)
      {
        throw new NotAuthorized("cannot edit keep if you aren't the creator");
      }
      return _repo.Edit(editKeep);
    }

    internal object GetByProfile(string id)
    {
      return _repo.GetByProfile(id);
    }

    internal IEnumerable<VaultKeepsViewModel> GetByVaultId(int id)
    {
      return _repo.GetByVault(id);
    }

    internal string Delete(int keepId, string accountId)
    {
      Keep preDelete = _repo.GetOne(keepId);
      if (preDelete == null)
      {
        throw new Exception("invalid id");
      }
      if (preDelete.CreatorId != accountId)
      {
        throw new NotAuthorized("cannot delete if you are not the creator");
      }
      _repo.Delete(keepId);
      return "deleted";
    }
  }
}