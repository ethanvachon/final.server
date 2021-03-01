using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using final.server.Models;
using final.server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final.server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  [Authorize]
  public class AccountController : ControllerBase
  {
    private readonly ProfilesService _ps;
    private readonly VaultsService _vs;

    public AccountController(ProfilesService ps, VaultsService vs)
    {
      _ps = ps;
      _vs = vs;
    }

    [HttpGet]
    public async Task<ActionResult<Profile>> GetAccount()
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        return Ok(_ps.GetOrCreateProfile(userInfo));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("vaults")]
    public async Task<ActionResult<IEnumerable<Vault>>> GetAccountVaults()
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        return Ok(_vs.GetByAccount(userInfo.Id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}