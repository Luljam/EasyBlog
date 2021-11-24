using EasyBlog.Web.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBlog.Web.Core
{
    public interface IConfigurationFactory
    {
        EasyBlogModulesConfigurationElementCollection GetModules();
    }

    public class ConfigurationFactory : IConfigurationFactory
    {
        public EasyBlogModulesConfigurationElementCollection GetModules()
        {
            EasyBlogConfigurationSection config = ConfigurationManager.GetSection("easyBlog")
               as EasyBlogConfigurationSection;

            if (config != null)
            {
                return config.Modules;
            }
            else
            {
                return null;
            }
        }
    }

}
