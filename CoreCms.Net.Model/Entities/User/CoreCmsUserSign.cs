/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2025/2/15 22:42:52
 *        Description: 暂无
 ***********************************************************************/

using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreCms.Net.Model.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CoreCmsUserSign
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CoreCmsUserSign()
        {
        }
		
        /// <summary>
        /// 签到记录
        /// </summary>
        [Display(Name = "签到记录")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 Id  { get; set; }
        
		
        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 UserId  { get; set; }
        
		
        /// <summary>
        /// 签到日期
        /// </summary>
        [Display(Name = "签到日期")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.DateTime SignDate  { get; set; }
        
		
        /// <summary>
        /// 本次签到获得的积分
        /// </summary>
        [Display(Name = "本次签到获得的积分")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 SignPoint  { get; set; }
        
		
        /// <summary>
        /// 连续签到天数
        /// </summary>
        [Display(Name = "连续签到天数")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 ContinuityDays  { get; set; }
        
		
        /// <summary>
        /// 记录创建时间
        /// </summary>
        [Display(Name = "记录创建时间")]
		
        
        
        
        
        public System.DateTime? CreateTime  { get; set; }
        
		
    }
}
