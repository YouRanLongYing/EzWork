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
namespace Ez.Controllers
{
    public class TestController : DefaultController<IDepartmentBiz>
    {
        public ActionResult Index()
        {
            //FW_Department entity = new FW_Department
            //{
            //    dep_name = "顶级部门",
            //    dep_desp = "部门描述",
            //    remark = "备注",
            //    dep_status = Enums.DepartmentStatus.Normal.GetHashCode()
            //};
            //BizResult<FW_Department> result = this.DefaultBiz.AddDepartment(entity);

            //entity = result.Data;

            //entity.dep_name = entity.dep_name + "已修改";

            //result = this.DefaultBiz.EditDepartment(entity);

            //result = this.DefaultBiz.GetDepartment(result.Data.id);


            //FW_Department childentity = new FW_Department
            //{
            //    dep_name = "二级部门",
            //    dep_desp = "部门描述",
            //    remark = "备注",
            //    dep_parentid = result.Data.id,
            //    dep_status = Enums.DepartmentStatus.Normal.GetHashCode()
            //};

            //result = this.DefaultBiz.AddDepartment(childentity);

            //result = this.DefaultBiz.GetParentDepartment(result.Data.id);

            //BizResult<Dtos.Library.PageDto<FW_Department>> pageresult = this.DefaultBiz.GetDepartmentAll(new Dtos.Library.PageDto<FW_Department>()
            //{
            //    PageIndex = 1,
            //    PageSize = 2
            //});

            return View();
        }

        #region 测试上传功能
        public ActionResult UploadTest()
        {
            FormDto dto = new FormDto
            {
                Name = "kongjing"
            };
            SetFormDto(dto);
            return View(dto);
        }
        #endregion

        #region 测试表单功能
        /// <summary>
        /// 模拟从其他表中获取到的可用于选择的列表数据并将列表数据装配到表单模型
        /// </summary>
        /// <param name="dto">表单模型</param>
        /// <returns>表单模型</returns>
        private FormDto SetFormDto(FormDto dto)
        {
            ///配置UI上用于选择性别的属性
            dto.Agent_Sex = new PropAgent(p => dto.Sex);
            dto.Agent_Sex.Add(new PropAgentItem("男", "0", selected: true));
            dto.Agent_Sex.Add(new PropAgentItem("女", "1"));
            //配置UI上用于选择爱好的属性
            dto.Agent_Hob = new PropAgent(p => dto.Hob);
            dto.Agent_Hob.Add(new PropAgentItem("游泳", "0"));
            dto.Agent_Hob.Add(new PropAgentItem("听歌", "1"));
            dto.Agent_Hob.Add(new PropAgentItem("书法", "2", selected: true));

            //配置UI上用于选择工作年限的属性
            dto.Agent_Workyear = new PropAgent(p => dto.Workyear);
            dto.Agent_Workyear.Add(new PropAgentItem("一年", "1"));
            dto.Agent_Workyear.Add(new PropAgentItem("五年", "5", selected: true));
            dto.Agent_Workyear.Add(new PropAgentItem("十年", "10"));

            return dto;
        }
        /// <summary>
        /// 从数据库中获取的表单模型
        /// </summary>
        /// <returns></returns>
        private FormDto GetDataFromDB()
        {
            return new FormDto
            {
                Name = "endfasle@163.com",
                Sex = 0,
                IsWork = true,
                Brithday = DateTime.Now
            };
        }

        public ActionResult FormTest()
        {
            FormDto _obj = GetDataFromDB();

            _obj = SetFormDto(_obj);


            //模拟存储的性别信息
            _obj.Agent_Sex.SetSelected(this.Cache["DB_Sex"].ToSafeString("0"));

            //模拟存储的爱好信息信息
            if (this.Cache["DB_Hob"] != null)
            {
                IList<string> selecteds = this.Cache["DB_Hob"] as IList<string>;
                _obj.Agent_Hob.SetSelected(selecteds.ToArray());
            }

            //模拟存储的工作年限信息
            _obj.Agent_Workyear.SetSelected(this.Cache["DB_Workyear"].ToSafeString("10"));

            this.Cache["FormDto"] = _obj;
            return View(_obj);
        }

        [HttpPost]
        [AsyncAction]
        [ValidateInput(false)]
        public JsResult FormTestAsync(FormDto dto)
        {
            this.Cache["DB_Sex"] = dto.Sex;
            if (dto.Hob != null)
            {
                this.Cache["DB_Hob"] = dto.Hob;
            }
            this.Cache["DB_Workyear"] = dto.Workyear;
            return new JsResult(true, null, "数据已处理成功！");
        }
        #endregion
    }
}
