using Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure {
    public class UrlHelper : IUrlHelper {
        private readonly IHttpContextAccessor _accessor;

        public UrlHelper(IHttpContextAccessor accessor) {
            _accessor = accessor;
        }
        public string GetCurrentUrl() {
        
            return $"{ _accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host.Value}/";
        }
        public string GetCurrentUrl(string url) {

            return $"{ GetCurrentUrl()}/{url}";
        }
    }
}
