using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Ez.Dtos;
using Ez.Controllers.Library;
using Ez.BizContract;
using Ez.Dtos.Entities;
using Ez.UI;
using Ez.UI.HtmlExtend;
using Ez.Core;
using Ez.UI.HtmlExtends;
using Ez.Dtos.Library;
using Ez.Helper;
namespace Ez.Controllers
{
    public class EzGridTestController : DefaultController<IEzGrid_TestBiz>
    {
        public ActionResult ViewTest()
        {

            return View();
        }
        [AsyncAction]
        [HttpGet]
        public JsResult ViewTest_async()
        {
            PageDto<EzGridTestDto> query = new PageDto<EzGridTestDto>();
            if (!string.IsNullOrEmpty(Tools.RequestQuery("zone_name")))
            {
                query.QueryStrings.Add("zone_name", Tools.RequestQuery("zone_name"));
            }
            BizResult<PageDto<EzGridTestDto>> exereturn = this.DefaultBiz.Pagnation(query);
            if (exereturn.Success)
                return exereturn.Data.AsJsResult();
            else
                return new JsResult(false, new { Error = "QueryError" }, "");
        }
        [AsyncAction]
        [HttpPost]
        public JsResult delete(int? zone_id)
        {
            if (zone_id > 0)
            {
                BizResult result = this.DefaultBiz.DeleteData(zone_id.Value);
                if (result.Success)
                {
                    return new JsResult(true, null, "删除成功！");
                }
                else
                {
                    return new JsResult(false, null, "删除失败！");
                }
            }
            else
            {
                return new JsResult(false, null, "参数错误！");
            }
        }
        public ActionResult edit(int? zone_id)
        {
            if (zone_id > 0)
            {
                BizResult<EzGridTestDto> result = this.DefaultBiz.GetEntity(zone_id.Value);
                if (result.Success)
                {
                    return View(result.Data);
                }
            }
            return View(new EzGridTestDto());
        }
        [ValidateInput(false)]
        public JsResult edit_async(EzGridTestDto dto)
        {
            BizResult result = this.DefaultBiz.CreateOrUpdateEntity(dto);
            string actionmame = dto.zone_id > 0 ? "修改" : "添加";
            string resultstring = result.Success ? "成功！" : "失败！";
            return new JsResult(null, result.Success, actionmame + resultstring, "ViewTest", false, false);
        }
    }
}
