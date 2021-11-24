using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EasyBlog.Web.Core
{
    public class LocalStrings : ILocatorStrings
    {
        public string SiteTitle {
            get { return "EasyBlog (DI Enable)";  } 
        }
        public string PostsTitle
        {
            get { return "Easy Blog - Home"; }
        }

        public string PostTitle
        {
            get { return "Blog Post"; }
        }

        public string NewPostTitle
        {
            get { return "New Blog Post"; }
        }
    }
    public interface ILocatorStrings
    {
        string SiteTitle { get; }
        string PostsTitle { get; }
        string PostTitle { get; }
        string NewPostTitle { get; }

    }
}