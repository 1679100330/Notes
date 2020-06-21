#ASP.NET Core MVC

##过滤器
> IAuthorizationFilter

	public class GlobalAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {            
			//获取方式一
            var area = context.RouteData.DataTokens["area"];
            var action = context.RouteData.Values["action"];            
            var controller = context.RouteData.Values["controller"];
			
			//获取方式二
			ActionDescriptor ad = context.ActionDescriptor;
            //var action = ad.RouteValues["action"];   
            //var controller = ad.RouteValues["controller"];   
            
			//跳过加上[AllowAnonymous]注解的Action
			if (context.Filters.Any(e => (e as AllowAnonymousFilter) != null))
            {
                return;
            }else{
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }                
        }
    }

> ExceptionFilterAttribute

	public class GlobalExceptionAttribute:ExceptionFilterAttribute{
        public override void OnException(ExceptionContext context) {
            
        }
    }

> 注册全局过滤器

	//写法一
	services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        .AddMvcOptions(options => {
        options.Filters.Add<GlobalAuthorizationFilter>();
        options.Filters.Add<GlobalExceptionAttribute>();
    }); 

	//写法二
	services.AddScoped<GlobalAuthorizationFilter>();
    services.AddScoped<GlobalExceptionAttribute>();                           
    services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
        .AddMvcOptions(options => {
        options.Filters.AddService(typeof(GlobalAuthorizationFilter));
        options.Filters.AddService(typeof(GlobalExceptionAttribute));
    }); 

> 扩展IsAjaxRequest

	public static class AjaxRequestExtensions
	{
		public static bool IsAjaxRequest(this HttpRequest request)
		{
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			return request.Headers["X-Requested-With"] == "XMLHttpRequest" || (request.Headers != null && request.Headers["X-Requested-With"] == "XMLHttpRequest");
		}
	}

	var isAjax = context.HttpContext.Request.IsAjaxRequest();
