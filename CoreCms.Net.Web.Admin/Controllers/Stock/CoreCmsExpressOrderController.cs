/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2025/3/9 22:53:35
 *        Description: 暂无
 ***********************************************************************/


using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.Entities.Expression;
using CoreCms.Net.Model.FromBody;
using CoreCms.Net.Model.ViewModels.UI;
using CoreCms.Net.Filter;
using CoreCms.Net.Loging;
using CoreCms.Net.IServices;
using CoreCms.Net.Utility.Helper;
using CoreCms.Net.Utility.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NPOI.HSSF.UserModel;
using SqlSugar;
using CoreCms.Net.Web.Admin.Infrastructure;

namespace CoreCms.Net.Web.Admin.Controllers
{
    /// <summary>
    /// 快递配送表
    ///</summary>
    [Description("快递配送表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsExpressOrderController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsExpressOrderServices _CoreCmsExpressOrderServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsExpressOrderController(IWebHostEnvironment webHostEnvironment
            ,ICoreCmsExpressOrderServices CoreCmsExpressOrderServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _CoreCmsExpressOrderServices = CoreCmsExpressOrderServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsExpressOrder/GetPageList
         /// <summary>
        /// 获取列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("获取列表")]
        public async Task<AdminUiCallBack> GetPageList()
        {
            var jm = new AdminUiCallBack();
            var pageCurrent = Request.Form["page"].FirstOrDefault().ObjectToInt(1);
            var pageSize = Request.Form["limit"].FirstOrDefault().ObjectToInt(30);
            var where = PredicateBuilder.True<CoreCmsExpressOrder>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsExpressOrder, object>> orderEx = orderField switch
            {
                "id" => p => p.id,"expno" => p => p.expno,"expcom" => p => p.expcom,"storeid" => p => p.storeid,"storename" => p => p.storename,"explist" => p => p.explist,"recname" => p => p.recname,"rectel" => p => p.rectel,"recaddr" => p => p.recaddr,"sendtime" => p => p.sendtime,"totalpay" => p => p.totalpay,"discount" => p => p.discount,"pointuse" => p => p.pointuse,"serverid" => p => p.serverid,"serverticker" => p => p.serverticker,"note" => p => p.note,"createtime" => p => p.createtime,
                _ => p => p.id
            };

            //设置排序方式
            var orderDirection = Request.Form["orderDirection"].FirstOrDefault();
            var orderBy = orderDirection switch
            {
                "asc" => OrderByType.Asc,
                "desc" => OrderByType.Desc,
                _ => OrderByType.Desc
            };
            //查询筛选
			
			//主键编号 int
			var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
			//快递单号 varchar
			var expno = Request.Form["expno"].FirstOrDefault();
            if (!string.IsNullOrEmpty(expno))
            {
                where = where.And(p => p.expno.Contains(expno));
            }
			//快递公司 varchar
			var expcom = Request.Form["expcom"].FirstOrDefault();
            if (!string.IsNullOrEmpty(expcom))
            {
                where = where.And(p => p.expcom.Contains(expcom));
            }
			//门店编号 varchar
			var storeid = Request.Form["storeid"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storeid))
            {
                where = where.And(p => p.storeid.Contains(storeid));
            }
			//门店名称 varchar
			var storename = Request.Form["storename"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storename))
            {
                where = where.And(p => p.storename.Contains(storename));
            }
			//代拿列表 varchar
			var explist = Request.Form["explist"].FirstOrDefault();
            if (!string.IsNullOrEmpty(explist))
            {
                where = where.And(p => p.explist.Contains(explist));
            }
			//收件人 varchar
			var recname = Request.Form["recname"].FirstOrDefault();
            if (!string.IsNullOrEmpty(recname))
            {
                where = where.And(p => p.recname.Contains(recname));
            }
			//联系电话 varchar
			var rectel = Request.Form["rectel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(rectel))
            {
                where = where.And(p => p.rectel.Contains(rectel));
            }
			//收货地址 varchar
			var recaddr = Request.Form["recaddr"].FirstOrDefault();
            if (!string.IsNullOrEmpty(recaddr))
            {
                where = where.And(p => p.recaddr.Contains(recaddr));
            }
			//派件时间 datetime
			var sendtime = Request.Form["sendtime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sendtime))
            {
                where = where.And(p => p.sendtime.Contains(sendtime));
            }
			//总金额 int
			var totalpay = Request.Form["totalpay"].FirstOrDefault().ObjectToInt(0);
            if (totalpay > 0)
            {
                where = where.And(p => p.totalpay == totalpay);
            }
			//优惠金额 int
			var discount = Request.Form["discount"].FirstOrDefault().ObjectToInt(0);
            if (discount > 0)
            {
                where = where.And(p => p.discount == discount);
            }
			//使用积分 int
			var pointuse = Request.Form["pointuse"].FirstOrDefault().ObjectToInt(0);
            if (pointuse > 0)
            {
                where = where.And(p => p.pointuse == pointuse);
            }
			//使用服务 int
			var serverid = Request.Form["serverid"].FirstOrDefault().ObjectToInt(0);
            if (serverid > 0)
            {
                where = where.And(p => p.serverid == serverid);
            }
			//服务码 varchar
			var serverticker = Request.Form["serverticker"].FirstOrDefault();
            if (!string.IsNullOrEmpty(serverticker))
            {
                where = where.And(p => p.serverticker.Contains(serverticker));
            }
			//备注 varchar
			var note = Request.Form["note"].FirstOrDefault();
            if (!string.IsNullOrEmpty(note))
            {
                where = where.And(p => p.note.Contains(note));
            }
			//创建时间 datetime
			var createtime = Request.Form["createtime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createtime))
            {
                if (createtime.Contains("到"))
                {
                    var dts = createtime.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.createtime > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.createtime < dtEnd);
                }
                else
                {
                    var dt = createtime.ObjectToDate();
                    where = where.And(p => p.createtime > dt);
                }
            }
            //获取数据
            var list = await _CoreCmsExpressOrderServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsExpressOrder/GetIndex
        /// <summary>
        /// 首页数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("首页数据")]
        public AdminUiCallBack GetIndex()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }
        #endregion

        #region 创建数据============================================================
        // POST: Api/CoreCmsExpressOrder/GetCreate
        /// <summary>
        /// 创建数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("创建数据")]
        public AdminUiCallBack GetCreate()
        {
            //返回数据
            var jm = new AdminUiCallBack { code = 0 };
            return jm;
        }
        #endregion

        #region 创建提交============================================================
        // POST: Api/CoreCmsExpressOrder/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody]CoreCmsExpressOrder entity)
        {
            var jm = await _CoreCmsExpressOrderServices.InsertAsync(entity);
            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsExpressOrder/GetEdit
        /// <summary>
        /// 编辑数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑数据")]
        public async Task<AdminUiCallBack> GetEdit([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsExpressOrderServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            jm.data = model;

            return jm;
        }
        #endregion

        #region 编辑提交============================================================
        // POST: Api/CoreCmsExpressOrder/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody]CoreCmsExpressOrder entity)
        {
            var jm = await _CoreCmsExpressOrderServices.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsExpressOrder/DoDelete/10
        /// <summary>
        /// 单选删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("单选删除")]
        public async Task<AdminUiCallBack> DoDelete([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsExpressOrderServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
				return jm;
            }
            jm = await _CoreCmsExpressOrderServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 批量删除============================================================
        // POST: Api/CoreCmsExpressOrder/DoBatchDelete/10,11,20
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody]FMArrayIntIds entity)
        {
            var jm = await _CoreCmsExpressOrderServices.DeleteByIdsAsync(entity.id);
            return jm;
        }

        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsExpressOrder/GetDetails/10
        /// <summary>
        /// 预览数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("预览数据")]
        public async Task<AdminUiCallBack> GetDetails([FromBody]FMIntId entity)
        {
            var jm = new AdminUiCallBack();

            var model = await _CoreCmsExpressOrderServices.QueryByIdAsync(entity.id, false);
            if (model == null)
            {
                jm.msg = "不存在此信息";
                return jm;
            }
            jm.code = 0;
            jm.data = model;

            return jm;
        }
        #endregion

        #region 选择导出============================================================
        // POST: Api/CoreCmsExpressOrder/SelectExportExcel/10
        /// <summary>
        /// 选择导出
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("选择导出")]
        public async Task<AdminUiCallBack> SelectExportExcel([FromBody]FMArrayIntIds entity)
        {
            var jm = new AdminUiCallBack();

            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsExpressOrderServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("主键编号");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("快递单号");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("快递公司");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("门店编号");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("门店名称");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("代拿列表");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("收件人");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("联系电话");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("收货地址");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("派件时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("总金额");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("优惠金额");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);

            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("使用积分");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);

            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("使用服务");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);

            var cell14 = headerRow.CreateCell(14);
            cell14.SetCellValue("服务码");
            cell14.CellStyle = headerStyle;
            mySheet.SetColumnWidth(14, 10 * 256);

            var cell15 = headerRow.CreateCell(15);
            cell15.SetCellValue("备注");
            cell15.CellStyle = headerStyle;
            mySheet.SetColumnWidth(15, 10 * 256);

            var cell16 = headerRow.CreateCell(16);
            cell16.SetCellValue("创建时间");
            cell16.CellStyle = headerStyle;
            mySheet.SetColumnWidth(16, 10 * 256);

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);

                    var rowTemp0 = rowTemp.CreateCell(0);
                        rowTemp0.SetCellValue(listModel[i].id.ToString());
                        rowTemp0.CellStyle = commonCellStyle;

                    var rowTemp1 = rowTemp.CreateCell(1);
                        rowTemp1.SetCellValue(listModel[i].expno.ToString());
                        rowTemp1.CellStyle = commonCellStyle;

                    var rowTemp2 = rowTemp.CreateCell(2);
                        rowTemp2.SetCellValue(listModel[i].expcom.ToString());
                        rowTemp2.CellStyle = commonCellStyle;

                    var rowTemp3 = rowTemp.CreateCell(3);
                        rowTemp3.SetCellValue(listModel[i].storeid.ToString());
                        rowTemp3.CellStyle = commonCellStyle;

                    var rowTemp4 = rowTemp.CreateCell(4);
                        rowTemp4.SetCellValue(listModel[i].storename.ToString());
                        rowTemp4.CellStyle = commonCellStyle;

                    var rowTemp5 = rowTemp.CreateCell(5);
                        rowTemp5.SetCellValue(listModel[i].explist.ToString());
                        rowTemp5.CellStyle = commonCellStyle;

                    var rowTemp6 = rowTemp.CreateCell(6);
                        rowTemp6.SetCellValue(listModel[i].recname.ToString());
                        rowTemp6.CellStyle = commonCellStyle;

                    var rowTemp7 = rowTemp.CreateCell(7);
                        rowTemp7.SetCellValue(listModel[i].rectel.ToString());
                        rowTemp7.CellStyle = commonCellStyle;

                    var rowTemp8 = rowTemp.CreateCell(8);
                        rowTemp8.SetCellValue(listModel[i].recaddr.ToString());
                        rowTemp8.CellStyle = commonCellStyle;

                    var rowTemp9 = rowTemp.CreateCell(9);
                        rowTemp9.SetCellValue(listModel[i].sendtime.ToString());
                        rowTemp9.CellStyle = commonCellStyle;

                    var rowTemp10 = rowTemp.CreateCell(10);
                        rowTemp10.SetCellValue(listModel[i].totalpay.ToString());
                        rowTemp10.CellStyle = commonCellStyle;

                    var rowTemp11 = rowTemp.CreateCell(11);
                        rowTemp11.SetCellValue(listModel[i].discount.ToString());
                        rowTemp11.CellStyle = commonCellStyle;

                    var rowTemp12 = rowTemp.CreateCell(12);
                        rowTemp12.SetCellValue(listModel[i].pointuse.ToString());
                        rowTemp12.CellStyle = commonCellStyle;

                    var rowTemp13 = rowTemp.CreateCell(13);
                        rowTemp13.SetCellValue(listModel[i].serverid.ToString());
                        rowTemp13.CellStyle = commonCellStyle;

                    var rowTemp14 = rowTemp.CreateCell(14);
                        rowTemp14.SetCellValue(listModel[i].serverticker.ToString());
                        rowTemp14.CellStyle = commonCellStyle;

                    var rowTemp15 = rowTemp.CreateCell(15);
                        rowTemp15.SetCellValue(listModel[i].note.ToString());
                        rowTemp15.CellStyle = commonCellStyle;

                    var rowTemp16 = rowTemp.CreateCell(16);
                        rowTemp16.SetCellValue(listModel[i].createtime.ToString());
                        rowTemp16.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsExpressOrder导出(选择结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

        #region 查询导出============================================================
        // POST: Api/CoreCmsExpressOrder/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsExpressOrder>();
                //查询筛选
			
			//主键编号 int
			var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
			//快递单号 varchar
			var expno = Request.Form["expno"].FirstOrDefault();
            if (!string.IsNullOrEmpty(expno))
            {
                where = where.And(p => p.expno.Contains(expno));
            }
			//快递公司 varchar
			var expcom = Request.Form["expcom"].FirstOrDefault();
            if (!string.IsNullOrEmpty(expcom))
            {
                where = where.And(p => p.expcom.Contains(expcom));
            }
			//门店编号 varchar
			var storeid = Request.Form["storeid"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storeid))
            {
                where = where.And(p => p.storeid.Contains(storeid));
            }
			//门店名称 varchar
			var storename = Request.Form["storename"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storename))
            {
                where = where.And(p => p.storename.Contains(storename));
            }
			//代拿列表 varchar
			var explist = Request.Form["explist"].FirstOrDefault();
            if (!string.IsNullOrEmpty(explist))
            {
                where = where.And(p => p.explist.Contains(explist));
            }
			//收件人 varchar
			var recname = Request.Form["recname"].FirstOrDefault();
            if (!string.IsNullOrEmpty(recname))
            {
                where = where.And(p => p.recname.Contains(recname));
            }
			//联系电话 varchar
			var rectel = Request.Form["rectel"].FirstOrDefault();
            if (!string.IsNullOrEmpty(rectel))
            {
                where = where.And(p => p.rectel.Contains(rectel));
            }
			//收货地址 varchar
			var recaddr = Request.Form["recaddr"].FirstOrDefault();
            if (!string.IsNullOrEmpty(recaddr))
            {
                where = where.And(p => p.recaddr.Contains(recaddr));
            }
			//派件时间 datetime
			var sendtime = Request.Form["sendtime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(sendtime))
            {
                var dt = sendtime.ObjectToDate();
                where = where.And(p => p.recaddr.Contains(recaddr));
            }
			//总金额 int
			var totalpay = Request.Form["totalpay"].FirstOrDefault().ObjectToInt(0);
            if (totalpay > 0)
            {
                where = where.And(p => p.totalpay == totalpay);
            }
			//优惠金额 int
			var discount = Request.Form["discount"].FirstOrDefault().ObjectToInt(0);
            if (discount > 0)
            {
                where = where.And(p => p.discount == discount);
            }
			//使用积分 int
			var pointuse = Request.Form["pointuse"].FirstOrDefault().ObjectToInt(0);
            if (pointuse > 0)
            {
                where = where.And(p => p.pointuse == pointuse);
            }
			//使用服务 int
			var serverid = Request.Form["serverid"].FirstOrDefault().ObjectToInt(0);
            if (serverid > 0)
            {
                where = where.And(p => p.serverid == serverid);
            }
			//服务码 varchar
			var serverticker = Request.Form["serverticker"].FirstOrDefault();
            if (!string.IsNullOrEmpty(serverticker))
            {
                where = where.And(p => p.serverticker.Contains(serverticker));
            }
			//备注 varchar
			var note = Request.Form["note"].FirstOrDefault();
            if (!string.IsNullOrEmpty(note))
            {
                where = where.And(p => p.note.Contains(note));
            }
			//创建时间 datetime
			var createtime = Request.Form["createtime"].FirstOrDefault();
            if (!string.IsNullOrEmpty(createtime))
            {
                var dt = createtime.ObjectToDate();
                where = where.And(p => p.createtime > dt);
            }
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsExpressOrderServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
                var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);
            
            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("主键编号");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);
			
            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("快递单号");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);
			
            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("快递公司");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);
			
            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("门店编号");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);
			
            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("门店名称");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);
			
            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("代拿列表");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);
			
            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("收件人");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);
			
            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("联系电话");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);
			
            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("收货地址");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);
			
            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("派件时间");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);
			
            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("总金额");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);
			
            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("优惠金额");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);
			
            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("使用积分");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);
			
            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("使用服务");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);
			
            var cell14 = headerRow.CreateCell(14);
            cell14.SetCellValue("服务码");
            cell14.CellStyle = headerStyle;
            mySheet.SetColumnWidth(14, 10 * 256);
			
            var cell15 = headerRow.CreateCell(15);
            cell15.SetCellValue("备注");
            cell15.CellStyle = headerStyle;
            mySheet.SetColumnWidth(15, 10 * 256);
			
            var cell16 = headerRow.CreateCell(16);
            cell16.SetCellValue("创建时间");
            cell16.CellStyle = headerStyle;
            mySheet.SetColumnWidth(16, 10 * 256);
			

            headerRow.Height = 30 * 20;
            var commonCellStyle = ExcelHelper.GetCommonStyle(book);

            //将数据逐步写入sheet1各个行
            for (var i = 0; i < listModel.Count; i++)
            {
                var rowTemp = mySheet.CreateRow(i + 1);


            var rowTemp0 = rowTemp.CreateCell(0);
            rowTemp0.SetCellValue(listModel[i].id.ToString());
            rowTemp0.CellStyle = commonCellStyle;



            var rowTemp1 = rowTemp.CreateCell(1);
            rowTemp1.SetCellValue(listModel[i].expno.ToString());
            rowTemp1.CellStyle = commonCellStyle;



            var rowTemp2 = rowTemp.CreateCell(2);
            rowTemp2.SetCellValue(listModel[i].expcom.ToString());
            rowTemp2.CellStyle = commonCellStyle;



            var rowTemp3 = rowTemp.CreateCell(3);
            rowTemp3.SetCellValue(listModel[i].storeid.ToString());
            rowTemp3.CellStyle = commonCellStyle;



            var rowTemp4 = rowTemp.CreateCell(4);
            rowTemp4.SetCellValue(listModel[i].storename.ToString());
            rowTemp4.CellStyle = commonCellStyle;



            var rowTemp5 = rowTemp.CreateCell(5);
            rowTemp5.SetCellValue(listModel[i].explist.ToString());
            rowTemp5.CellStyle = commonCellStyle;



            var rowTemp6 = rowTemp.CreateCell(6);
            rowTemp6.SetCellValue(listModel[i].recname.ToString());
            rowTemp6.CellStyle = commonCellStyle;



            var rowTemp7 = rowTemp.CreateCell(7);
            rowTemp7.SetCellValue(listModel[i].rectel.ToString());
            rowTemp7.CellStyle = commonCellStyle;



            var rowTemp8 = rowTemp.CreateCell(8);
            rowTemp8.SetCellValue(listModel[i].recaddr.ToString());
            rowTemp8.CellStyle = commonCellStyle;



            var rowTemp9 = rowTemp.CreateCell(9);
            rowTemp9.SetCellValue(listModel[i].sendtime.ToString());
            rowTemp9.CellStyle = commonCellStyle;



            var rowTemp10 = rowTemp.CreateCell(10);
            rowTemp10.SetCellValue(listModel[i].totalpay.ToString());
            rowTemp10.CellStyle = commonCellStyle;



            var rowTemp11 = rowTemp.CreateCell(11);
            rowTemp11.SetCellValue(listModel[i].discount.ToString());
            rowTemp11.CellStyle = commonCellStyle;



            var rowTemp12 = rowTemp.CreateCell(12);
            rowTemp12.SetCellValue(listModel[i].pointuse.ToString());
            rowTemp12.CellStyle = commonCellStyle;



            var rowTemp13 = rowTemp.CreateCell(13);
            rowTemp13.SetCellValue(listModel[i].serverid.ToString());
            rowTemp13.CellStyle = commonCellStyle;



            var rowTemp14 = rowTemp.CreateCell(14);
            rowTemp14.SetCellValue(listModel[i].serverticker.ToString());
            rowTemp14.CellStyle = commonCellStyle;



            var rowTemp15 = rowTemp.CreateCell(15);
            rowTemp15.SetCellValue(listModel[i].note.ToString());
            rowTemp15.CellStyle = commonCellStyle;



            var rowTemp16 = rowTemp.CreateCell(16);
            rowTemp16.SetCellValue(listModel[i].createtime.ToString());
            rowTemp16.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsExpressOrder导出(查询结果).xls";
            string filePath = webRootPath + tpath;
            DirectoryInfo di = new DirectoryInfo(filePath);
            if (!di.Exists)
            {
                di.Create();
            }
            FileStream fileHssf = new FileStream(filePath + fileName, FileMode.Create);
            book.Write(fileHssf);
            fileHssf.Close();

            jm.code = 0;
            jm.msg = GlobalConstVars.ExcelExportSuccess;
            jm.data = tpath + fileName;

            return jm;
        }
        #endregion

        

    }
}
