/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2021/1/31 21:45:10
 *        Description: 暂无
 ***********************************************************************/

using System.ComponentModel.DataAnnotations;
using SqlSugar;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    ///     服务消费券
    /// </summary>
    public partial class CoreCmsUserServicesTicket
    {
        /// <summary>
        ///     状态说明
        /// </summary>
        [Display(Name = "状态说明")]
        [SugarColumn(IsIgnore = true)]
        public string statusStr { get; set; }

        /// <summary>
        /// 全部数量
        /// </summary>
        [Display(Name = "全部数量")]
        [SugarColumn(IsIgnore = true)]
        public int allUse { get; set; }
        /// <summary>
        /// 已用数量
        /// </summary>
        [Display(Name = "已用数量")]
        [SugarColumn(IsIgnore = true)]
        public int noUse { get; set; }
        /// <summary>
        /// 可用数量
        /// </summary>
        [Display(Name = "可用数量")]
        [SugarColumn(IsIgnore = true)]
        public int yesUse { get; set; }
    }
}