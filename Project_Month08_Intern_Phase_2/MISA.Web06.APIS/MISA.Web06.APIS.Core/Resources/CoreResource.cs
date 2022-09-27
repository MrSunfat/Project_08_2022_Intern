using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Web06.APIS.Core.Resources
{
    public static class CoreResource
    {
        public static string LanguageCode = "VN";

        public static string? GetResoureString(string keyResoure)
        {
            var langCode = CoreResource.LanguageCode;
            var resoureString = $"{langCode}_{keyResoure}";
            return Core.Resources.Content.ResourceManager.GetString(resoureString);
        }

    }
}
