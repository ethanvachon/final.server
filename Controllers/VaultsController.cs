using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using final.server.Models;
using final.server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace final.server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class VaultsController : ControllerBase
  {
    private readonly VaultsService _vs;
    private readonly KeepsService _ks;

    public VaultsController(VaultsService vs, KeepsService ks)
    {
      _vs = vs;
      _ks = ks;
    }

    [HttpGet("{id}/keeps")]
    public async Task<ActionResult<IEnumerable<VaultKeepsViewModel>>> GetKeepsByVaultAsync(int id)
    {
      try
      {
        // Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        Vault vault = _vs.GetOne(id);
        if (vault.IsPrivate == true)
        {
          if (await HttpContext.GetUserInfoAsync<Profile>() != null)
          {
            Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
            if (userInfo.Id == vault.CreatorId)
            {
              return Ok(_ks.GetByVaultId(id));
            }
            return BadRequest("this vault is private");
          }
          return BadRequest("this vault is private");
        }
        return Ok(_ks.GetByVaultId(id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vault>> GetOneAsync(int id)
    {
      try
      {
        if (await HttpContext.GetUserInfoAsync<Profile>() == null)
        {
          return Ok(_vs.GetOne(id));
        }
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        return Ok(_vs.GetOne(id, userInfo.Id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Vault>> Create([FromBody] Vault newVault)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        newVault.CreatorId = userInfo.Id;
        Vault created = _vs.Create(newVault);
        created.Creator = userInfo;
        return Ok(created);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Vault>> Edit(int id, [FromBody] Vault editVault)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        editVault.Id = id;
        editVault.Creator = userInfo;
        return Ok(_vs.Edit(editVault, userInfo.Id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }

    }
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult<string>> Delete(int id)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        return Ok(_vs.Delete(id, userInfo.Id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }

}