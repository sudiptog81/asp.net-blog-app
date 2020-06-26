using Microsoft.AspNetCore.Hosting;

[assembly:
  HostingStartup(typeof(BlogMvc.Areas.Identity.IdentityHostingStartup))]

namespace BlogMvc.Areas.Identity
{
  public class IdentityHostingStartup : IHostingStartup
  {
    public void Configure(IWebHostBuilder builder)
    {
      builder.ConfigureServices((context, services) => { });
    }
  }
}