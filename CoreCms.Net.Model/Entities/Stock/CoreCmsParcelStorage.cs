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
        /// 构造函数
        /// </summary>
        public CoreCmsParcelStorage()
        {
        }
		
        /// <summary>
        /// id
        /// </summary>
        [Display(Name = "id")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 id  { get; set; }
        
		
        /// <summary>
        /// 收件人手机号
        /// </summary>
        [Display(Name = "收件人手机号")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String phone_number  { get; set; }
        
		
        /// <summary>
        /// 所属门店/网点编号
        /// </summary>
        [Display(Name = "所属门店/网点编号")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:10,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String store_id  { get; set; }
        
		
        /// <summary>
        /// 快递单号
        /// </summary>
        [Display(Name = "快递单号")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String tracking_number  { get; set; }
        
		
        /// <summary>
        /// 快递状态（0-待取件 1-已取件 2-异常）
        /// </summary>
        [Display(Name = "快递状态（0-待取件 1-已取件 2-异常）")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 parcel_status  { get; set; }
        
		
        /// <summary>
        /// 存放时间
        /// </summary>
        [Display(Name = "存放时间")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.DateTime storage_time  { get; set; }
        
		
        /// <summary>
        /// 取件时间
        /// </summary>
        [Display(Name = "取件时间")]
		
        
        
        
        
        public System.DateTime? pickup_time  { get; set; }
        
		
        /// <summary>
        /// 快递存放位置
        /// </summary>
        [Display(Name = "快递存放位置")]
		
        
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String storage_location  { get; set; }
        
		
        /// <summary>
        /// 操作员ID
        /// </summary>
        [Display(Name = "操作员ID")]
		
        
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String operator_id  { get; set; }
        
		
        /// <summary>
        /// 包裹尺寸分类（S-小件/M-中件/L-大件）
        /// </summary>
        [Display(Name = "包裹尺寸分类（S-小件/M-中件/L-大件）")]
		
        
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String size_type  { get; set; }
        
		
        /// <summary>
        /// 包裹重量（单位：克）
        /// </summary>
        [Display(Name = "包裹重量（单位：克）")]
		
        
        
        
        
        public System.Int32? weight  { get; set; }
        
		
        /// <summary>
        /// 包裹照片URL
        /// </summary>
        [Display(Name = "包裹照片URL")]
		
        
        
        [StringLength(maximumLength:255,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String photo_url  { get; set; }
        
		
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.DateTime created_time  { get; set; }
        
		
        /// <summary>
        /// 最后更新时间
        /// </summary>
        [Display(Name = "最后更新时间")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.DateTime updated_time  { get; set; }
        
		
    }
}
