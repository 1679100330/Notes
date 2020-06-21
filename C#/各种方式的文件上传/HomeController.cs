using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public void Upload()
        {
            HttpPostedFileBase file = null;
            HttpFileCollectionBase hfcb = Request.Files;
            if (hfcb.Count > 0)
            {
                file = hfcb.Get(hfcb.GetKey(0));
            }
            if (file != null) {
                var filePath = Path.Combine(Request.MapPath("~/App_Data"), Path.GetFileName(file.FileName));
                file.SaveAs(filePath);
            }
        }

    }
}
