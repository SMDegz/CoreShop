/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2025/3/8 22:17:33
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 快递信息表
    /// </summary>
    public partial class CoreCmsParcelStorage
    {
        /// <summary>
        ///     快递状态
        /// </summary>
        [Display(Name = "快递状态")]
        [SugarColumn(IsIgnore = true)]
        public string ParcelStatusName { get; set; }

        /// <summary>
        ///     包裹尺寸
        /// </summary>
        [Display(Name = "包裹尺寸")]
        [SugarColumn(IsIgnore = true)]
        public string SizeTypeName { get; set; }

    }
}
