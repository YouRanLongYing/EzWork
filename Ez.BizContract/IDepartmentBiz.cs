using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Dtos.Library;
using Ez.Dtos.Entities;
using Ez.Dtos.Entities.Nexus;

namespace Ez.BizContract
{
    /// <summary>
    /// 部门业务模块协议
    /// </summary>
    public interface IDepartmentBiz
    {
        /// <summary>
        /// 获取部门信息
        /// created by kongjing
        /// </summary>
        /// <param name="dto">分页模型</param>
        /// <returns>分页结果</returns>
        BizResult<PageDto<FW_Department>> GetDepartmentAll(PageDto<FW_Department> dto);
        /// <summary>
        /// 删除一个部门
        ///  created by kongjing
        /// </summary>
        /// <param name="depid">部门编号</param>
        /// <returns>是否成功</returns>
        BizResult DeleteDepartment(int depid);
        /// <summary>
        /// 添加一个部门
        ///  created by kongjing
        /// </summary>
        /// <param name="entity">部门信息</param>
        /// <returns>部门信息</returns>
        BizResult<FW_Department> AddDepartment(FW_Department entity);
        /// <summary>
        /// 编辑部门信息
        ///  created by kongjing
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <returns>修改后的部门信息</returns>
        BizResult<FW_Department> EditDepartment(FW_Department entity);
        /// <summary>
        /// 编辑部门信息
        ///  created by kongjing
        /// </summary>
        /// <param name="depid">部门编号</param>
        /// <returns>修改后的部门信息</returns>
        BizResult<FW_Department> GetDepartment(int depid);
        /// <summary>
        /// 获取所有上级部门
        ///  created by kongjing
        /// </summary>
        /// <param name="depid">当前部门编号</param>
        /// <returns></returns>
        BizResult<FW_Department> GetParentDepartment(int depid);
        /// <summary>
        /// 获取所有下级部门
        ///  created by kongjing
        /// </summary>
        /// <param name="depid">当前部门编号</param>
        /// <returns></returns>
        BizResult<IList<FW_Department>> GetChildrenDepartment(int depid);
        /// <summary>
        /// 获取用户所在部门
        ///  created by kongjing
        /// </summary>
        /// <param name="userid">用户编号</param>
        /// <returns>可能会在多部门所以返回的是列表</returns>
        BizResult<IList<FW_Department>> GetUserDepartment(int userid);

        /// <summary>
        /// 设置用户到部门的关系
        /// </summary>
        /// <param name="ref_entity">关系</param>
        /// <returns></returns>
        BizResult SetDepartmentUser(FW_U_Ref_Department ref_entity);
    }
}
