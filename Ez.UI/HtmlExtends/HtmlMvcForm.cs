using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web;

namespace Ez.UI.HtmlExtends
{
    public class HtmlMvcForm : MvcForm
    {
        private bool _disposed;
        private readonly ViewContext _viewContext;

        public HtmlMvcForm(ViewContext viewContext)
            : base(viewContext)
        {
            this._viewContext = viewContext;
        }

        protected override void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                this._disposed = true;
                EndForm();
            }
        }

        public void EndForm()
        {
            this._viewContext.Writer.Write("</fieldset>");
            this._viewContext.Writer.Write("</form>");
            this._viewContext.OutputClientValidation();
            this._viewContext.FormContext = null;
        }
    }
}
