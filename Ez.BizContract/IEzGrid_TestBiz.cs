using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.BizContract;
using Ez.Dtos.Entities;
using Ez.Dtos.Library;
using Ez.Dtos;


namespace Ez.BizContract
{
    public interface IEzGrid_TestBiz : IDefaultBiz
    {
        bool exits(string zone_name);

        BizResult<EzGridTestDto> GetEntity(int zone_id);

        BizResult DeleteData(int zone_id);

        BizResult CreateOrUpdateEntity(EzGridTestDto entity);

        BizResult<PageDto<EzGridTestDto>> Pagnation(PageDto<EzGridTestDto> dto);

        IDictionary<string, string> GetZonesDic();
    }
}
