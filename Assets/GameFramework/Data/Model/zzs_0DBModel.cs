//========================================================
// 描述:
// 创建者:周忠帅
// 联系方式:QQ3192766733
// 创建时间:2022/2/14 17:51:34
//========================================================

using UnityEngine;
namespace GameFramework.Data.Model
{
	using GameFramework.ClassExt;
	using GameFramework.Data.Entity;
	//====================================================
	//该脚本为数据模型(DBModel)脚本，由工具生成，切勿更改
	//该脚本为数据模型(DBModel)脚本，由工具生成，切勿更改
	//该脚本为数据模型(DBModel)脚本，由工具生成，切勿更改
	//====================================================
	
	/// <summary>
	/// zzs_0DBModel类
	/// </summary>
	public partial class zzs_0DBModel : AbstractDBModel<zzs_0DBModel,zzs_0Entity>
	{
	    protected override string FileName => "zzs_0.data";
	    protected override zzs_0Entity MakeData(GameDataTableParser parser)
	    {
	        var entity = new zzs_0Entity();
	        entity.ID = parser.GetFieldValue("ID").ToInt();
	        entity.Name = parser.GetFieldValue("Name");
	        entity.Age = parser.GetFieldValue("Age").ToInt();
	        entity.Other1 = parser.GetFieldValue("Other1");
	        entity.Other2 = parser.GetFieldValue("Other2").ToFloat();
	        entity.Other3 = parser.GetFieldValue("Other3").ToLong();
	        entity.Other4 = parser.GetFieldValue("Other4").ToInt();
	        entity.Other5 = parser.GetFieldValue("Other5");
	        return entity;
	    }
	}
}
