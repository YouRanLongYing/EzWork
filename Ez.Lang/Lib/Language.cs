using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UBIQ.Resource.Framework
{
    public partial class FrmLanguage
    {
        public static UBIQ.Resource.Framework.LanguageAssemblyProvider AssemblyProvider { set; get; }
        private static global::UBIQ.Resource.Framework.CResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public FrmLanguage()
        {
        }

        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::UBIQ.Resource.Framework.CResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::UBIQ.Resource.Framework.CResourceManager temp = new global::UBIQ.Resource.Framework.CResourceManager("UBIQ.Resource.Framework.FrmLanguage", typeof(FrmLanguage).Assembly);
                    if (FrmLanguage.AssemblyProvider != null && !string.IsNullOrEmpty(FrmLanguage.AssemblyProvider.AssemblyName) && FrmLanguage.AssemblyProvider.AssemblyType != null)
                    {
                        temp.CustomResourceManager = new global::System.Resources.ResourceManager(FrmLanguage.AssemblyProvider.AssemblyName, FrmLanguage.AssemblyProvider.AssemblyType.Assembly);
                    }

                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }
    }
}
