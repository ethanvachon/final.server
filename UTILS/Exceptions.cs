using System;

namespace final.server.Exceptions
{
  public class NotAuthorized : Exception
  {
    public NotAuthorized(string message) : base(message)
    {
    }
  }
}