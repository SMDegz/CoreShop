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
        /// 构造函数
        /// </summary>
        public CoreCmsExpressOrder()
        {
        }
		
        /// <summary>
        /// 主键编号
        /// </summary>
        [Display(Name = "主键编号")]
		
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 id  { get; set; }
        
		
        /// <summary>
        /// 快递单号
        /// </summary>
        [Display(Name = "快递单号")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String expno  { get; set; }
        
		
        /// <summary>
        /// 快递公司
        /// </summary>
        [Display(Name = "快递公司")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String expcom  { get; set; }
        
		
        /// <summary>
        /// 门店编号
        /// </summary>
        [Display(Name = "门店编号")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String storeid  { get; set; }
        
		
        /// <summary>
        /// 门店名称
        /// </summary>
        [Display(Name = "门店名称")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String storename  { get; set; }
        
		
        /// <summary>
        /// 代拿列表
        /// </summary>
        [Display(Name = "代拿列表")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String explist  { get; set; }
        
		
        /// <summary>
        /// 收件人
        /// </summary>
        [Display(Name = "收件人")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String recname  { get; set; }
        
		
        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:20,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String rectel  { get; set; }
        
		
        /// <summary>
        /// 收货地址
        /// </summary>
        [Display(Name = "收货地址")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        [StringLength(maximumLength:200,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String recaddr  { get; set; }
        
		
        /// <summary>
        /// 派件时间
        /// </summary>
        [Display(Name = "派件时间")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.DateTime sendtime  { get; set; }
        
		
        /// <summary>
        /// 总金额
        /// </summary>
        [Display(Name = "总金额")]
		
        [Required(ErrorMessage = "请输入{0}")]
        
        
        
        public System.Int32 totalpay  { get; set; }
        
		
        /// <summary>
        /// 优惠金额
        /// </summary>
        [Display(Name = "优惠金额")]
		
        
        
        
        
        public System.Int32? discount  { get; set; }
        
		
        /// <summary>
        /// 使用积分
        /// </summary>
        [Display(Name = "使用积分")]
		
        
        
        
        
        public System.Int32? pointuse  { get; set; }
        
		
        /// <summary>
        /// 使用服务
        /// </summary>
        [Display(Name = "使用服务")]
		
        
        
        
        
        public System.Int32? serverid  { get; set; }
        
		
        /// <summary>
        /// 服务码
        /// </summary>
        [Display(Name = "服务码")]
		
        
        
        [StringLength(maximumLength:50,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String serverticker  { get; set; }
        
		
        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
		
        
        
        [StringLength(maximumLength:100,ErrorMessage = "{0}不能超过{1}字")]
        
        public System.String note  { get; set; }
        
		
        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
		
        
        
        
        
        public System.DateTime? createtime  { get; set; }
        
		
    }
}
