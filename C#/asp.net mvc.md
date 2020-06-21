#asp.net mvc

##App_start中BundleConfig的介绍使用
	它用来将js和css进行压缩（多个文件可以打包成一个文件），并且可以区分调试和非调试，在调试时不进行压缩。在Web.config中，当compilation编译的debug属性设为true时，表示项目处于调试模式，这时Bundle是不会将文件进行打包压缩的，最终部署运行时，将debug设为false就可以看到js和css被打包和压缩了。

	public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            //重新注册忽略规则
            ResetIgnorePatterns(bundles.IgnoreList);

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Res/Service/jquery/jquery-3.1.1.min.js",
                        "~/Res/Service/bootstrap/bootstrap.min.js",
                        "~/Res/Service/highcharts/highcharts.js"));
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Res/Service/bootstrap/bootstrap.min.css"));
        }

        private static void ResetIgnorePatterns(IgnoreList ignoreList)
        {
            ignoreList.Clear();
            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }

##过滤器
	Filter 类型			实现的接口				描述	
	Authorization		IAuthorizationFilter	最先运行的Filter，被用作请求权限校验	
	Action				IActionFilter			在Action运行的前、后运行	
	Exception			IExceptionFilter		当异常发生的时候运行

###1.注册全局过滤器
	public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeFilterAttribute());
            filters.Add(new AccessHandleFilterAttribute());
        }
    }

###2.实现AuthorizeAttribute
	权限过滤
	执行顺序：OnAuthorization -->AuthorizeCore -->HandleUnauthorizedRequest

	public class AuthorizeFilterAttribute: AuthorizeAttribute
    {       
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            string controllerName = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string actionName = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //string controllerName = filterContext.RouteData.Values["controller"].ToString();
            //string actionName = filterContext.RouteData.Values["action"].ToString();
            UrlHelper urlHelper = new UrlHelper(filterContext.RequestContext);
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectResult(
                        urlHelper.Action("NoAuth", "Home"));
            }
            else
            {
                filterContext.Result = new RedirectResult(
                        urlHelper.Action("Login", "Home"));
            }
        }
    }
###3.实现ActionFilterAttribute	
	public class AccessHandleFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //Model验证
            var viewData = filterContext.Controller.ViewData;
            if (!viewData.ModelState.IsValid)
            {
                var errorMsg = "";
                foreach (var key in viewData.ModelState.Keys)
                {
                    var modelState = viewData.ModelState[key];
                    if (modelState.Errors.Count > 0)
                    {
                        errorMsg = modelState.Errors[0].ErrorMessage;
                        break;
                    }
                }
                if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.Result = new ContentResult
                    {
                        Content = JsonConvert.SerializeObject(
                            new MsgResult() { Status = StatusEnum.FAILURE, Obj = errorMsg })
                    };
                }
                else
                {
                    filterContext.Result = new ContentResult() { Content = errorMsg };
                }
            }
            base.OnActionExecuting(filterContext);                       
        }        
       
    }
###4.实现HandleErrorAttribute
	public class GlobalHandleErrorAttribute : HandleErrorAttribute
    {
        private ILog logger = LogManager.GetLogger(typeof(GlobalHandleErrorAttribute));
        public override void OnException(ExceptionContext filterContext)
        {
            //记录异常
            logger.Error("系统异常：", filterContext.Exception);
            base.OnException(filterContext);                        
        }
    }
###5.AllowAnonymous
	表示一个特性，该特性用于标记在授权期间要跳过 AuthorizeAttribute 的控制器和操作。

##Model验证的扩展
###1.常用的验证特性
	[Required(ErrorMessage = "代码不能为空")]
	[RegularExpression(@"^[A-Z0-9\-]{1,10}$", ErrorMessage = "代码必须由1~10个大写字母或数字或横杠(-)组成")]	
	[Range(0, 10, ErrorMessage = "床位数的区间0~10")]

###2.验证多个属性的关系
	实体类的属性加上[Relation(CriteriaEnum.DORMITORYAREACODE, "FactoryAreacode")]

	public class RelationAttribute : ValidationAttribute
    {
        public string factoryAreacode { set; get; }       

        public RelationAttribute(CriteriaEnum criteria, string factoryAreacode = null)
		{
            this.factoryAreacode = factoryAreacode;            
		}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var factoryAreacodeProperty = validationContext.ObjectType.GetProperty(factoryAreacode);
			var factoryAreacodePropertyValue = factoryAreacodeProperty.GetValue(validationContext.ObjectInstance, null);                    
			if (count==0)
			{                        
				return new ValidationResult("厂区代码不可用");
			}
            return null;
        }
    }

###3.验证字符串字节的长度
	实体类的属性加上[StringByteLength(30, ErrorMessage = "名称的字符串字节长度不能超过30（中文字符占3个）")]

	public class StringByteLengthAttribute : ValidationAttribute
    {

        public StringByteLengthAttribute(int length)
        {
            this.MaximumLength = length;
        }

        public int MaximumLength { get; set; }

        public int MinimumLength { get; set; }

        public override bool IsValid(object value)
        {
            int length = Encoding.UTF8.GetByteCount(value + "");
            if (length >= MinimumLength && length <= MaximumLength) {
                return true;
            }
            else
            {
                return false;
            }            
        }

    }
##视图
	@Html.ActionLink("退出", "Logout", "Home")
	@Url.Action("Default", "Home")

	@*加載菜單*@    
    @if (@Session["User"] != null)
    {
        for (int i = 0; i < ((DormitoryMS.Models.SysUser)@Session["User"]).Menus.Count; i++)
        {
            DormitoryMS.Models.SysMenu menu = ((DormitoryMS.Models.SysUser)@Session["User"]).Menus[@i];
            if (menu.Pid == "#" && menu.IsMenu == "1")
            {
                <div title="@menu.Name">
                    @for (int j = 0; j < ((DormitoryMS.Models.SysUser)@Session["User"]).Menus.Count; j++)
                    {
                        DormitoryMS.Models.SysMenu childMenu = ((DormitoryMS.Models.SysUser)@Session["User"]).Menus[@j];
                        if (childMenu.Pid == menu.Id && childMenu.IsMenu=="1")
                        {
                            <a class="menu" href="javascript:;" menuid="@childMenu.Id" url="@Url.Action(childMenu.Action, childMenu.Controller)" name="@childMenu.Name">@childMenu.Name</a>                                    
                        }
                    }
                </div>
            }
        }
    } 
