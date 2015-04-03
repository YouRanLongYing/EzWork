using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ez.Biz;
using Ez.BizContract;
using Ez.DBContract;
using Ez.Dtos.Entities;
using Ez.Dtos.Library;
using Ez.Core;
using Ez.Dtos;

namespace Ez.Biz
{
    public class EzGrid_TestBiz : DefaultBiz, IEzGrid_TestBiz
    {
        public bool exits(string zone_name)
        {
            return this.ProDb.Exists("select count(1) from pms_zone where zone_name =@zone_name", new DbParam("@zone_name", zone_name));
        }
        public BizResult<EzGridTestDto> GetEntity(int zone_id)
        {
            EzGridTestDto dto = this.ProDb.GetEntity<EzGridTestDto>("select * from pms_zone where zone_id =@zone_id", new DbParam("@zone_id", zone_id));
            return new BizResult<EzGridTestDto>(dto != null, dto);
        }
        public BizResult DeleteData(int zone_id)
        {
            if (this.ProDb.ExecuteSql("delete from pms_zone where zone_id=@zone_id", new DbParam("@zone_id", zone_id)) > 0)
            {
                return new BizResult(true);
            }
            else
            {
                return new BizResult(false);
            }
        }
        public BizResult CreateOrUpdateEntity(EzGridTestDto dto)
        {
            BizResult result = new BizResult();
            EzGrid_Test entity = dto.TranslatorTo<EzGrid_Test, EzGridTestDto>();
            entity.modifier_time = entity.create_time = DateTime.Now;
            result.Success = this.ProDb.OrmCreateOrUpdate(entity, "zone_id");
            return result;
        }
        public BizResult<PageDto<EzGridTestDto>> Pagnation(PageDto<EzGridTestDto> dto)
        {
            int records = 0;
            dto.Results = this.ProDb.QueryPaging<EzGridTestDto>(
            new QuerySql
            {
                SelectColumns = "zone_id, zone_name, dev_time,finish_time",
                FromTableNames = "pms_zone",
                SequenceColumnName = "zone_id",
                IsDesc = dto.IsDesc,
                PageIndex = dto.PageIndex,
                PageSize = dto.PageSize,
                OrderBy = dto.OrderBy,
                LikeCondition = dto.QueryStrings
            }, out records);
            dto.Records = records;
            return new BizResult<PageDto<EzGridTestDto>>(true, dto);
        }

        public IDictionary<string,string> GetZonesDic()
        {
           IDictionary<string, string> dic = new Dictionary<string, string>();
           IList<object> list =  this.ProDb.GetDataArray("select zone_id,zone_name from pms_zone");
           if (list != null && list.Count() > 0)
           {
               foreach (var item in list)
               {
                  object[] arrays =  item as object[];
                  if (arrays == null || arrays.Length != 2) continue;
                  dic.Add(arrays[0].ToSafeString(),arrays[1].ToSafeString());
               }
           }
           return dic;
        }
    }
}
