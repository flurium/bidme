using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bll.Infrastructure
{
  // TODO REPLACE
  public class User
  { }

  public class EmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : User
  {
    public EmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<DataProtectionTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger)
        : base(dataProtectionProvider, options, logger)
    {
    }
  }

  public class EmailConfirmationProviderOptions : DataProtectionTokenProviderOptions
  { }
}