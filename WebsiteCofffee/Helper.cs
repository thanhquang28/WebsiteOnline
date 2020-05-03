using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebsiteCofffee
{
    //get data from session
    public static class Helpers
    {
        public static string GetNameUserSession(this HtmlHelper helper)
        {
            return (string)System.Web.HttpContext.Current.Session["NameOfUser"];
        }

        public static string GetAvaSession(this HtmlHelper helper)
        {
            var dirContainImg = (string)System.Web.HttpContext.Current.Session["DirectionAva"];
            var forSrc = "/" + dirContainImg;
            return forSrc;
        }

        public static string GetIdSession(this HtmlHelper helper)
        {
            return "/Admin/Users/Details/" + System.Web.HttpContext.Current.Session["UserId"].ToString(); ;
        }
        public static string GetImgUser(this HtmlHelper helper)
        {
            var dirImgUser = (string)System.Web.HttpContext.Current.Session["DirImgUser"];
            var realSrc = "/" + dirImgUser;
            return realSrc;
        }

        public static MvcHtmlString Image(this HtmlHelper html,byte[] image, string classname, object htmlAttribute = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("class", classname);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttribute));

            var imageString = image != null ? Convert.ToBase64String(image) : "";
            var img = string.Format("data:image/jpg;base64,{0}", imageString);
            builder.MergeAttribute("src", img);

            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }
    }
}