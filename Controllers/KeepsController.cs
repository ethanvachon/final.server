using System.Collections.Generic;
using System.Threading.Tasks;
using CodeWorks.Auth0Provider;
using final.server.Exceptions;
using final.server.Models;
using final.server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final.server.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class KeepsController : ControllerBase
  {
    private readonly KeepsService _ks;

    public KeepsController(KeepsService ks)
    {
      _ks = ks;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Keep>> GetAll()
    {
      try
      {
        return Ok(_ks.GetAll());
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpGet("{id}")]
    public ActionResult<Keep> GetOne(int id)
    {
      try
      {
        return Ok(_ks.GetOne(id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}/addview")]
    public ActionResult<Keep> AddView(int id)
    {
      try
      {
        return Ok(_ks.AddView(id));
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Keep>> Create([FromBody] Keep newKeep)
    {
      try
      {
        newKeep.Keeps = 0;
        newKeep.Views = 0;
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        newKeep.CreatorId = userInfo.Id;
        Keep keep = _ks.Create(newKeep);
        newKeep.Creator = userInfo;
        return Ok(keep);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<Keep>> EditAsync([FromBody] Keep editKeep, int id)
    {
      try
      {
        Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
        editKeep.Id = id;
        editKeep.CreatorId = userInfo.Id;
        editKeep.Creator = userInfo;
        return Ok(_ks.Edit(editKeep, userInfo.Id));
      }
      catch (NotAuthorized e)
      {
        return StatusCode(StatusCodes.Status403Forbidden, e.Message);
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
        return Ok(_ks.Delete(id, userInfo.Id));
      }
      catch (NotAuthorized e)
      {
        return StatusCode(StatusCodes.Status403Forbidden, e.Message);
      }
      catch (System.Exception e)
      {
        return BadRequest(e.Message);
      }
    }
  }
}