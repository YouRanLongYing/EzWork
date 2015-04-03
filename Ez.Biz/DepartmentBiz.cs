using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using Ez.Dtos.Entities;
using Ez.Dtos.Library;
using Ez.DBContract;
using Ez.Cache;
using Ez.Core;
using Ez.Lang;
using Ez.Dtos;
using Ez.Dtos.Entities.Nexus;
namespace Ez.Biz
{
    /// <summary>
    /// 部门模块数据管理业务
    /// </summary>
    public class DepartmentBiz : DefaultBiz, IDepartmentBiz
    {
        /// <summary>
        /// 获取父级路径
        /// </summary>
        /// <param name="parentid">父级id</param>
        /// <param name="pparentPath">父级的父级路径</param>
        /// <returns>当前父级路径</returns>
        private string GetParentPathString(int parentid, string pparentPath)
        {
            return string.Format("{0}>{1}", pparentPath, parentid);
        }
        /// <summary>
        /// 获取部门信息
        /// created by kongjing
        /// </summary>
        /// <param name="dto">分页模型</param>
        /// <returns>分页结果</returns>
        public BizResult<PageDto<FW_Department>> GetDepartmentAll(PageDto<FW_Department> dto)
        {
            if (dto != null)
            {
                int records = 0;
                dto.Results = this.ProDb.QueryPaging<FW_Department>(new QuerySql
                {
                    SelectColumns = "id,dep_name,dep_desp,dep_parentid,dep_status,create_time,parent_path,remark",
                    FromTableNames = "FW_Department",
                    SequenceColumnName = "id",
                    IsDesc = dto.IsDesc,
                    PageIndex = dto.PageIndex,
                    PageSize = dto.PageSize,
                    OrderBy = dto.OrderBy,
                    EqualCondition = new Dictionary<string, string> { { "dep_status",Enums.DepartmentStatus.Normal.GetHashCode().ToString() } },
                }, out records);
                dto.Records = records;
                return new BizResult<PageDto<FW_Department>>(true, dto);
            }
            else
            {
                return new BizResult<PageDto<FW_Department>>(false, dto);
            }
        }
        /// <summary>
        /// 删除一个部门
        ///  created by kongjing
        /// </summary>
        /// <param name="depid">部门编号</param>
        /// <returns>是否成功</returns>
        public BizResult DeleteDepartment(int depid)
        {
            if (this.ProDb.GetSingle("select count(1) from FW_Department where dep_parentid = @depid", new DbParam("@depid", depid)).ToSafeInt() == 0)
            {
                int effect = this.ProDb.ExecuteSql("delete FW_Department where id=@id", new DbParam("@id", depid));
                return new BizResult(effect > 0);
            }
            else
            { 
               return new BizResult(false,null,string.Format(EzLanguage.SYS_Exp_DataHadRef,EzLanguage.COM_Btn_Delete));
            }
        }
        /// <summary>
        /// 添加一个部门
        ///  created by kongjing
        /// </summary>
        /// <param name="entity">部门信息</param>
        /// <returns>是否成功</returns>
        public BizResult<FW_Department> AddDepartment(FW_Department entity)
        {
            bool processok = false;
            if (entity != null)
            {
                entity.id = this.ProDb.GetSequenceVal("FW_Department");
                if (entity.dep_parentid > 0)
                {
                    IList<object> pids = this.ProDb.GetDataArray("select id,parent_path from FW_Department where id =@pid order by dep_parentid asc", new DbParam("@pid", entity.dep_parentid));
                    if (pids.Count == 1)
                    {
                        object[] array = pids[0] as object[];
                        if (array != null)
                        {
                            entity.parent_path = GetParentPathString(array[0].ToSafeInt(), array[1].ToSafeString());
                        }
                    }
                }
                else
                {
                    entity.dep_parentid = 0;
                    entity.parent_path = string.Empty;
                }
                //新增
                StringBuilder insertSql = new StringBuilder("insert into FW_Department(id,dep_name,dep_desp,dep_parentid,dep_status,create_time,parent_path,remark) ");
                insertSql.Append("values ");
                insertSql.Append("(@id,@dep_name,@dep_desp,@dep_parentid,@dep_status,@create_time,@parent_path,@remark)");
                entity.create_time = DateTime.Now;
                processok = this.ProDb.ExecuteSql(insertSql.ToString(),
                    new DbParam("@id", entity.id),
                    new DbParam("@dep_name", entity.dep_name),
                    new DbParam("@dep_desp", entity.dep_desp),
                    new DbParam("@dep_parentid", entity.dep_parentid),
                    new DbParam("@dep_status", entity.dep_status),
                    new DbParam("@create_time", entity.create_time),
                    new DbParam("@parent_path", entity.parent_path),
                    new DbParam("@remark", entity.remark)) > 0;
            }
            return new BizResult<FW_Department>(entity != null && processok, entity);
        }
        /// <summary>
        /// 编辑部门信息
        ///  created by kongjing
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns>修改后的部门信息</returns>
        public BizResult<FW_Department> EditDepartment(FW_Department entity)
        {
            bool processok = false;
            if (entity != null)
            {
                StringBuilder updateSql = new StringBuilder("update FW_Department set ");
                updateSql.Append("dep_name=@dep_name,");
                updateSql.Append("dep_desp=@dep_desp,");
                updateSql.Append("dep_parentid=@dep_parentid,");
                updateSql.Append("dep_status=@dep_status,");
                updateSql.Append("parent_path=@parent_path, ");
                updateSql.Append("remark=@remark ");
                updateSql.Append("where id=@id");
                processok = this.ProDb.ExecuteSql(updateSql.ToString(),
                    new DbParam("@id", entity.id),
                    new DbParam("@dep_name", entity.dep_name),
                    new DbParam("@dep_desp", entity.dep_desp),
                    new DbParam("@dep_parentid", entity.dep_parentid),
                    new DbParam("@dep_status", entity.dep_status),
                    new DbParam("@parent_path", entity.parent_path),
                    new DbParam("@remark", entity.remark))>0;
                RemoveCachePool(CacheKeys.FRM_DEP_INFORMATION, entity.id);
            }
            return new BizResult<FW_Department>(entity != null && processok,entity);
        }
        /// <summary>
        /// 编辑部门信息
        ///  created by kongjing
        /// </summary>
        /// <param name="depid">部门编号</param>
        /// <returns>修改后的部门信息</returns>
        public BizResult<FW_Department> GetDepartment(int depid)
        {
           FW_Department entity  = CachePool<FW_Department>(()=>{
               return this.ProDb.GetEntity<FW_Department>("select id,dep_name,dep_desp,dep_parentid,dep_status,create_time,parent_path,remark from FW_Department where id=@id", new DbParam("@id", depid));
           }, CacheKeys.FRM_DEP_INFORMATION,depid);
           return new BizResult<FW_Department>(entity != null && entity.id > 0, entity);
        }
        /// <summary>
        /// 获取上级部门
        ///  created by kongjing
        /// </summary>
        /// <param name="cdepid">当前部门编号</param>
        /// <returns></returns>
        public BizResult<FW_Department> GetParentDepartment(int cdepid)
        {
            int parentid = this.ProDb.GetSingle("select dep_parentid from FW_Department where id=@cdepid", new DbParam("@cdepid", cdepid)).ToSafeInt();
            return this.GetDepartment(parentid);
        }
        /// <summary>
        /// 获取所有下级部门
        ///  created by kongjing
        /// </summary>
        /// <param name="depid">当前部门编号</param>
        /// <returns></returns>
        public BizResult<IList<FW_Department>> GetChildrenDepartment(int depid)
        {
            StringBuilder selSql = new StringBuilder("select id,dep_name,dep_desp,dep_parentid,dep_status,create_time,parent_path,remark from FW_Department ");
            selSql.Append("where dep_parentid like '%>@depid%'");
            IList<FW_Department> entities = this.ProDb.GetEntities<FW_Department>(selSql.ToString(), new DbParam("@depid", depid));
            return new BizResult<IList<FW_Department>>(entities != null && entities.Count > 0, entities);
        }
        /// <summary>
        /// 获取用户所在部门
        ///  created by kongjing
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <returns>可能会在多部门所以返回的是列表</returns>
        public BizResult<IList<FW_Department>> GetUserDepartment(int userid)
        {
            StringBuilder selSql = new StringBuilder("select id,dep_name,dep_desp,dep_parentid,dep_status,create_time,parent_path,remark from FW_Department A inner join FW_U_Ref_Department B on A.id=B.dep_id ");
            selSql.Append("where B.login_id =@userid");
            IList<FW_Department> entities = this.ProDb.GetEntities<FW_Department>(selSql.ToString(), new DbParam("@userid", userid));
            return new BizResult<IList<FW_Department>>(entities != null && entities.Count > 0, entities);
        }
        /// <summary>
        /// 设置用户到部门的关系
        /// </summary>
        /// <param name="ref_entity">关系信息</param>
        /// <returns></returns>
        public BizResult SetDepartmentUser(FW_U_Ref_Department ref_entity)
        {
            if (this.ProDb.GetSingle("select count(1) from FW_Department where id =@depid", new DbParam("@depid", ref_entity.dep_id)).ToSafeInt() > 0)
            {
                if (this.ProDb.GetSingle("select count(1) from FW_U_Ref_Department where login_id =@userid and dep_id =@depid",
                    new DbParam("@userid", ref_entity.login_id),
                    new DbParam("@depid", ref_entity.dep_id)).ToSafeInt() > 0)
                {
                    ref_entity.create_time = DateTime.Now;
                    int effect = this.ProDb.ExecuteSql("insert into FW_U_Ref_Department(login_id,dep_id,create_time,creater) values(@userid,@depid,@time,@creater)",
                         new DbParam("@userid", ref_entity.login_id), new DbParam("@depid", ref_entity.dep_id), new DbParam("@time", ref_entity.create_time), new DbParam("@creater",ref_entity.creater));
                    return new BizResult(effect > 0);
                }
                else
                {
                    return new BizResult(EzLanguage.SYS_Exp_DataExits, false);
                }
            }
            else
            {
                return new BizResult(EzLanguage.SYS_Exp_DepInfoNotExits,false);
            }
        }
    }
}
