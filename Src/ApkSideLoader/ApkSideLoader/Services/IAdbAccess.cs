using System;
using System.Collections.Generic;
using System.Text;

namespace ApkSideLoader.Services
{
  public interface IAdbAccess
  {
    string CallAdb(string arg);
    string LoadFile();
  }
}
