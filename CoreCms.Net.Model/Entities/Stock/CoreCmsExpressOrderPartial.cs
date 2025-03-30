/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2025/3/9 22:53:35
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 快递配送表
    /// </summary>
    public partial class CoreCmsExpressOrder
    {
        /// <summary>
        ///     订单状态
        /// </summary>
        [Display(Name = "订单状态")]
        [SugarColumn(IsIgnore = true)]
        public string OrderStatusName { get; set; }

        public List<CoreCmsParcelStorage> coreCmsParcelStorageList { get; set; }

    }
}
