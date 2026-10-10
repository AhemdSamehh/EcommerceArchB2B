using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private ISender _sender = null!;

        public ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>()!;
    }
}
