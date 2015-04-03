using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using Ez.Dtos.Entities;
using Ez.DBContract;
using Ez.Dtos.Library;
using Ez.Cache;

namespace Ez.Biz
{
    /// <summary>
    /// 角色管理业务
    /// </summary>
    public class RoleBiz:DefaultBiz,IRoleBiz
    {
        /// <summary>
        /// 获取指定用户的权限
        /// </summary>
        /// <param name="login_id">登录用户编号</param>
        /// <param name="roleid">角色编号</param>
        /// <param name="webproj">是否为web项目，默认是</param>
        /// <returns></returns>
        public BizResult<IList<FW_U_Rights>> GetRights(int login_id,int roleid,bool webproj =true)
        {
            IList<FW_U_Rights> list = CachePool<IList<FW_U_Rights>>(() =>
            {
                StringBuilder sql = new StringBuilder();
                if (roleid.Equals(1))
                {
                    sql.Append(" select * from(");
                    sql.AppendFormat(" select * from FW_U_Rights where is_menu=1 and is_web_project = {0}", webproj ? 1 : 0);
                    sql.Append(" union");
                    sql.AppendFormat(" select * from FW_U_Rights where is_menu=0 and is_shortcut=1 and is_web_project = {0}", webproj ? 1 : 0);
                    sql.Append(" ) tb order by sort asc");
                    return this.ProDb.GetEntities<FW_U_Rights>(sql.ToString(),new DbParam("@proid", this.DbMaster.Proid));
                }
                else
                {
                    sql.Append(" select * from(");
                    sql.Append(" select distinct A.* from FW_U_Rights A ");
                    sql.Append(" inner join FW_R_Ref_Rights B on A.id = B.right_id");
                    sql.Append(" inner join FW_U_Ref_Roles C on C.roleid = B.role_id");
                    sql.Append(" inner join FW_U_Roles D on C.roleid = D.id");
                    sql.AppendFormat(" where A.un_ctrl!=0 and A.is_menu=1 and C.roleid=@roleid and C.[login_id]=@uid and A.is_web_project = {0} and (A.pro_id=0 or A.pro_id=@proid)", webproj ? 1 : 0);
                    sql.Append(" union");
                    sql.AppendFormat(" select * from FW_U_Rights where un_ctrl=0 and is_menu=1 and is_web_project = {0}", webproj ? 1 : 0);
                    sql.Append(" union");
                    sql.AppendFormat(" select * from FW_U_Rights where un_ctrl=0 and is_menu=0 and is_shortcut=1 and is_web_project = {0}", webproj ? 1 : 0);
                    sql.Append(" ) tb order by sort asc");
                    return this.ProDb.GetEntities<FW_U_Rights>(sql.ToString(),
                    new DbParam("@roleid", roleid),
                    new DbParam("@proid", this.DbMaster.Proid),
                    new DbParam("@uid", login_id));
                }
            }, CacheKeys.FRM_USER_ROLE_INFO, login_id, this.DbMaster.Proid, roleid);
            return new BizResult<IList<FW_U_Rights>>(list != null && list.Count > 0, list);
        }
        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <param name="dto">传输模型</param>
        /// <returns>分页结果</returns>
        public BizResult<PageDto<FW_U_Roles>> GetRoleList(PageDto<FW_U_Roles> dto)
        {
            int records = 0;
            dto.Results = this.ProDb.QueryPaging<FW_U_Roles>(
            new QuerySql
            {
                SelectColumns = "id, role_name, role_name_rs_key,[state],info,creater,create_time",
                FromTableNames = "FW_U_Roles",
                SequenceColumnName = "id",
                IsDesc = dto.IsDesc,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize,
                OrderBy = dto.OrderBy
            }, out records);
            dto.Records = records;

            return new BizResult<PageDto<FW_U_Roles>>(true, dto);
        }
    }
}
