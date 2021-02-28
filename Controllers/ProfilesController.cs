using System.Collections.Generic;
using final.server.Models;
using final.server.Services;
using Microsoft.AspNetCore.Mvc;

namespace final.server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProfilesController : ControllerBase
  {
    private readonly ProfilesService _ps;
    private readonly VaultsService _vs;

    private readonly KeepsService _ks;

    public ProfilesController(ProfilesService ps, VaultsService vs, KeepsService ks)
    {
      _ps = ps;
      _vs = vs;
      _ks = ks;
    }

    [HttpGet("{id}")]
    public ActionResult<ProfilesController> Get(string id)
    {
      try
      {
        Profile profile = _ps.GetProfileById(id);
        return Ok(profile);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}/keeps")]
    public ActionResult<IEnumerable<Keep>> GetKeeps(string id)
    {
      try
      {
        return Ok(_ks.GetByProfile(id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}/vaults")]
    public ActionResult<IEnumerable<Vault>> GetVaults(string id)
    {
      try
      {
        return Ok(_vs.GetByProfile(id));
      }
      catch (System.Exception e)
      {
        return BadRequest(id);
      }
    }
  }
}