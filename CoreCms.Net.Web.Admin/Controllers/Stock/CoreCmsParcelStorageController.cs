/***********************************************************************
 *            Project: CoreCms
 *        ProjectName: 核心内容管理系统                                
 *                Web: https://www.corecms.net                      
 *             Author: 大灰灰                                          
 *              Email: jianweie@163.com                                
 *         CreateTime: 2025/3/8 21:34:33
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
    /// 快递信息表
    ///</summary>
    [Description("快递信息表")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    [RequiredErrorForAdmin]
    [Authorize]
    public class CoreCmsParcelStorageController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICoreCmsParcelStorageServices _CoreCmsParcelStorageServices;

        /// <summary>
        /// 构造函数
        ///</summary>
        public CoreCmsParcelStorageController(IWebHostEnvironment webHostEnvironment
            ,ICoreCmsParcelStorageServices CoreCmsParcelStorageServices
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _CoreCmsParcelStorageServices = CoreCmsParcelStorageServices;
        }

        #region 获取列表============================================================
        // POST: Api/CoreCmsParcelStorage/GetPageList
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
            var where = PredicateBuilder.True<CoreCmsParcelStorage>();
            //获取排序字段
            var orderField = Request.Form["orderField"].FirstOrDefault();

            Expression<Func<CoreCmsParcelStorage, object>> orderEx = orderField switch
            {
                "id" => p => p.id,"phone_number" => p => p.phone_number,"store_id" => p => p.store_id,"tracking_number" => p => p.tracking_number,"parcel_status" => p => p.parcel_status,"storage_time" => p => p.storage_time,"pickup_time" => p => p.pickup_time,"storage_location" => p => p.storage_location,"operator_id" => p => p.operator_id,"size_type" => p => p.size_type,"weight" => p => p.weight,"photo_url" => p => p.photo_url,"created_time" => p => p.created_time,"updated_time" => p => p.updated_time,
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
			
			//id int
			var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
			//收件人手机号 varchar
			var phone_number = Request.Form["phone_number"].FirstOrDefault();
            if (!string.IsNullOrEmpty(phone_number))
            {
                where = where.And(p => p.phone_number.Contains(phone_number));
            }
			//所属门店/网点编号 varchar
			var store_id = Request.Form["store_id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(store_id))
            {
                where = where.And(p => p.store_id.Contains(store_id));
            }
			//快递单号 varchar
			var tracking_number = Request.Form["tracking_number"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tracking_number))
            {
                where = where.And(p => p.tracking_number.Contains(tracking_number));
            }
			//快递状态（0-待取件 1-已取件 2-异常） int
			var parcel_status = Request.Form["parcel_status"].FirstOrDefault().ObjectToInt(0);
            if (parcel_status > 0)
            {
                where = where.And(p => p.parcel_status == parcel_status);
            }
			//存放时间 datetime
			var storage_time = Request.Form["storage_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storage_time))
            {
                if (storage_time.Contains("到"))
                {
                    var dts = storage_time.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.storage_time > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.storage_time < dtEnd);
                }
                else
                {
                    var dt = storage_time.ObjectToDate();
                    where = where.And(p => p.storage_time > dt);
                }
            }
			//取件时间 datetime
			var pickup_time = Request.Form["pickup_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(pickup_time))
            {
                if (pickup_time.Contains("到"))
                {
                    var dts = pickup_time.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.pickup_time > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.pickup_time < dtEnd);
                }
                else
                {
                    var dt = pickup_time.ObjectToDate();
                    where = where.And(p => p.pickup_time > dt);
                }
            }
			//快递存放位置 varchar
			var storage_location = Request.Form["storage_location"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storage_location))
            {
                where = where.And(p => p.storage_location.Contains(storage_location));
            }
			//操作员ID varchar
			var operator_id = Request.Form["operator_id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(operator_id))
            {
                where = where.And(p => p.operator_id.Contains(operator_id));
            }
			//包裹尺寸分类（S-小件/M-中件/L-大件） varchar
			var size_type = Request.Form["size_type"].FirstOrDefault();
            if (!string.IsNullOrEmpty(size_type))
            {
                where = where.And(p => p.size_type.Contains(size_type));
            }
			//包裹重量（单位：克） int
			var weight = Request.Form["weight"].FirstOrDefault().ObjectToInt(0);
            if (weight > 0)
            {
                where = where.And(p => p.weight == weight);
            }
			//包裹照片URL varchar
			var photo_url = Request.Form["photo_url"].FirstOrDefault();
            if (!string.IsNullOrEmpty(photo_url))
            {
                where = where.And(p => p.photo_url.Contains(photo_url));
            }
			//创建时间 datetime
			var created_time = Request.Form["created_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(created_time))
            {
                if (created_time.Contains("到"))
                {
                    var dts = created_time.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.created_time > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.created_time < dtEnd);
                }
                else
                {
                    var dt = created_time.ObjectToDate();
                    where = where.And(p => p.created_time > dt);
                }
            }
			//最后更新时间 datetime
			var updated_time = Request.Form["updated_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updated_time))
            {
                if (updated_time.Contains("到"))
                {
                    var dts = updated_time.Split("到");
                    var dtStart = dts[0].Trim().ObjectToDate();
                    where = where.And(p => p.updated_time > dtStart);
                    var dtEnd = dts[1].Trim().ObjectToDate();
                    where = where.And(p => p.updated_time < dtEnd);
                }
                else
                {
                    var dt = updated_time.ObjectToDate();
                    where = where.And(p => p.updated_time > dt);
                }
            }
            //获取数据
            var list = await _CoreCmsParcelStorageServices.QueryPageAsync(where, orderEx, orderBy, pageCurrent, pageSize, true);
            //返回数据
            jm.data = list;
            jm.code = 0;
            jm.count = list.TotalCount;
            jm.msg = "数据调用成功!";
            return jm;
        }
        #endregion

        #region 首页数据============================================================
        // POST: Api/CoreCmsParcelStorage/GetIndex
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
        // POST: Api/CoreCmsParcelStorage/GetCreate
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
        // POST: Api/CoreCmsParcelStorage/DoCreate
        /// <summary>
        /// 创建提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("创建提交")]
        public async Task<AdminUiCallBack> DoCreate([FromBody]CoreCmsParcelStorage entity)
        {
            var jm = await _CoreCmsParcelStorageServices.InsertAsync(entity);
            return jm;
        }
        #endregion

        #region 编辑数据============================================================
        // POST: Api/CoreCmsParcelStorage/GetEdit
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

            var model = await _CoreCmsParcelStorageServices.QueryByIdAsync(entity.id, false);
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
        // POST: Api/CoreCmsParcelStorage/Edit
        /// <summary>
        /// 编辑提交
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("编辑提交")]
        public async Task<AdminUiCallBack> DoEdit([FromBody]CoreCmsParcelStorage entity)
        {
            var jm = await _CoreCmsParcelStorageServices.UpdateAsync(entity);
            return jm;
        }
        #endregion

        #region 删除数据============================================================
        // POST: Api/CoreCmsParcelStorage/DoDelete/10
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

            var model = await _CoreCmsParcelStorageServices.ExistsAsync(p => p.id == entity.id, true);
            if (!model)
            {
                jm.msg = GlobalConstVars.DataisNo;
				return jm;
            }
            jm = await _CoreCmsParcelStorageServices.DeleteByIdAsync(entity.id);

            return jm;
        }
        #endregion

        #region 批量删除============================================================
        // POST: Api/CoreCmsParcelStorage/DoBatchDelete/10,11,20
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("批量删除")]
        public async Task<AdminUiCallBack> DoBatchDelete([FromBody]FMArrayIntIds entity)
        {
            var jm = await _CoreCmsParcelStorageServices.DeleteByIdsAsync(entity.id);
            return jm;
        }

        #endregion

        #region 预览数据============================================================
        // POST: Api/CoreCmsParcelStorage/GetDetails/10
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

            var model = await _CoreCmsParcelStorageServices.QueryByIdAsync(entity.id, false);
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
        // POST: Api/CoreCmsParcelStorage/SelectExportExcel/10
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
            var listModel = await _CoreCmsParcelStorageServices.QueryListByClauseAsync(p => entity.id.Contains(p.id), p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
            var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);

            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("id");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);

            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("收件人手机号");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);

            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("所属门店/网点编号");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);

            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("快递单号");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);

            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("快递状态（0-待取件 1-已取件 2-异常）");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);

            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("存放时间");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);

            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("取件时间");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);

            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("快递存放位置");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);

            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("操作员ID");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);

            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("包裹尺寸分类（S-小件/M-中件/L-大件）");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);

            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("包裹重量（单位：克）");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);

            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("包裹照片URL");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);

            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("创建时间");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);

            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("最后更新时间");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);

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
                        rowTemp1.SetCellValue(listModel[i].phone_number.ToString());
                        rowTemp1.CellStyle = commonCellStyle;

                    var rowTemp2 = rowTemp.CreateCell(2);
                        rowTemp2.SetCellValue(listModel[i].store_id.ToString());
                        rowTemp2.CellStyle = commonCellStyle;

                    var rowTemp3 = rowTemp.CreateCell(3);
                        rowTemp3.SetCellValue(listModel[i].tracking_number.ToString());
                        rowTemp3.CellStyle = commonCellStyle;

                    var rowTemp4 = rowTemp.CreateCell(4);
                        rowTemp4.SetCellValue(listModel[i].parcel_status.ToString());
                        rowTemp4.CellStyle = commonCellStyle;

                    var rowTemp5 = rowTemp.CreateCell(5);
                        rowTemp5.SetCellValue(listModel[i].storage_time.ToString());
                        rowTemp5.CellStyle = commonCellStyle;

                    var rowTemp6 = rowTemp.CreateCell(6);
                        rowTemp6.SetCellValue(listModel[i].pickup_time.ToString());
                        rowTemp6.CellStyle = commonCellStyle;

                    var rowTemp7 = rowTemp.CreateCell(7);
                        rowTemp7.SetCellValue(listModel[i].storage_location.ToString());
                        rowTemp7.CellStyle = commonCellStyle;

                    var rowTemp8 = rowTemp.CreateCell(8);
                        rowTemp8.SetCellValue(listModel[i].operator_id.ToString());
                        rowTemp8.CellStyle = commonCellStyle;

                    var rowTemp9 = rowTemp.CreateCell(9);
                        rowTemp9.SetCellValue(listModel[i].size_type.ToString());
                        rowTemp9.CellStyle = commonCellStyle;

                    var rowTemp10 = rowTemp.CreateCell(10);
                        rowTemp10.SetCellValue(listModel[i].weight.ToString());
                        rowTemp10.CellStyle = commonCellStyle;

                    var rowTemp11 = rowTemp.CreateCell(11);
                        rowTemp11.SetCellValue(listModel[i].photo_url.ToString());
                        rowTemp11.CellStyle = commonCellStyle;

                    var rowTemp12 = rowTemp.CreateCell(12);
                        rowTemp12.SetCellValue(listModel[i].created_time.ToString());
                        rowTemp12.CellStyle = commonCellStyle;

                    var rowTemp13 = rowTemp.CreateCell(13);
                        rowTemp13.SetCellValue(listModel[i].updated_time.ToString());
                        rowTemp13.CellStyle = commonCellStyle;

            }
            // 导出excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsParcelStorage导出(选择结果).xls";
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
        // POST: Api/CoreCmsParcelStorage/QueryExportExcel/10
        /// <summary>
        /// 查询导出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("查询导出")]
        public async Task<AdminUiCallBack> QueryExportExcel()
        {
            var jm = new AdminUiCallBack();

            var where = PredicateBuilder.True<CoreCmsParcelStorage>();
                //查询筛选
			
			//id int
			var id = Request.Form["id"].FirstOrDefault().ObjectToInt(0);
            if (id > 0)
            {
                where = where.And(p => p.id == id);
            }
			//收件人手机号 varchar
			var phone_number = Request.Form["phone_number"].FirstOrDefault();
            if (!string.IsNullOrEmpty(phone_number))
            {
                where = where.And(p => p.phone_number.Contains(phone_number));
            }
			//所属门店/网点编号 varchar
			var store_id = Request.Form["store_id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(store_id))
            {
                where = where.And(p => p.store_id.Contains(store_id));
            }
			//快递单号 varchar
			var tracking_number = Request.Form["tracking_number"].FirstOrDefault();
            if (!string.IsNullOrEmpty(tracking_number))
            {
                where = where.And(p => p.tracking_number.Contains(tracking_number));
            }
			//快递状态（0-待取件 1-已取件 2-异常） int
			var parcel_status = Request.Form["parcel_status"].FirstOrDefault().ObjectToInt(0);
            if (parcel_status > 0)
            {
                where = where.And(p => p.parcel_status == parcel_status);
            }
			//存放时间 datetime
			var storage_time = Request.Form["storage_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storage_time))
            {
                var dt = storage_time.ObjectToDate();
                where = where.And(p => p.storage_time > dt);
            }
			//取件时间 datetime
			var pickup_time = Request.Form["pickup_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(pickup_time))
            {
                var dt = pickup_time.ObjectToDate();
                where = where.And(p => p.pickup_time > dt);
            }
			//快递存放位置 varchar
			var storage_location = Request.Form["storage_location"].FirstOrDefault();
            if (!string.IsNullOrEmpty(storage_location))
            {
                where = where.And(p => p.storage_location.Contains(storage_location));
            }
			//操作员ID varchar
			var operator_id = Request.Form["operator_id"].FirstOrDefault();
            if (!string.IsNullOrEmpty(operator_id))
            {
                where = where.And(p => p.operator_id.Contains(operator_id));
            }
			//包裹尺寸分类（S-小件/M-中件/L-大件） varchar
			var size_type = Request.Form["size_type"].FirstOrDefault();
            if (!string.IsNullOrEmpty(size_type))
            {
                where = where.And(p => p.size_type.Contains(size_type));
            }
			//包裹重量（单位：克） int
			var weight = Request.Form["weight"].FirstOrDefault().ObjectToInt(0);
            if (weight > 0)
            {
                where = where.And(p => p.weight == weight);
            }
			//包裹照片URL varchar
			var photo_url = Request.Form["photo_url"].FirstOrDefault();
            if (!string.IsNullOrEmpty(photo_url))
            {
                where = where.And(p => p.photo_url.Contains(photo_url));
            }
			//创建时间 datetime
			var created_time = Request.Form["created_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(created_time))
            {
                var dt = created_time.ObjectToDate();
                where = where.And(p => p.created_time > dt);
            }
			//最后更新时间 datetime
			var updated_time = Request.Form["updated_time"].FirstOrDefault();
            if (!string.IsNullOrEmpty(updated_time))
            {
                var dt = updated_time.ObjectToDate();
                where = where.And(p => p.updated_time > dt);
            }
            //获取数据
            //创建Excel文件的对象
            var book = new HSSFWorkbook();
            //添加一个sheet
            var mySheet = book.CreateSheet("Sheet1");
            //获取list数据
            var listModel = await _CoreCmsParcelStorageServices.QueryListByClauseAsync(where, p => p.id, OrderByType.Asc, true);
            //给sheet1添加第一行的头部标题
                var headerRow = mySheet.CreateRow(0);
            var headerStyle = ExcelHelper.GetHeaderStyle(book);
            
            var cell0 = headerRow.CreateCell(0);
            cell0.SetCellValue("id");
            cell0.CellStyle = headerStyle;
            mySheet.SetColumnWidth(0, 10 * 256);
			
            var cell1 = headerRow.CreateCell(1);
            cell1.SetCellValue("收件人手机号");
            cell1.CellStyle = headerStyle;
            mySheet.SetColumnWidth(1, 10 * 256);
			
            var cell2 = headerRow.CreateCell(2);
            cell2.SetCellValue("所属门店/网点编号");
            cell2.CellStyle = headerStyle;
            mySheet.SetColumnWidth(2, 10 * 256);
			
            var cell3 = headerRow.CreateCell(3);
            cell3.SetCellValue("快递单号");
            cell3.CellStyle = headerStyle;
            mySheet.SetColumnWidth(3, 10 * 256);
			
            var cell4 = headerRow.CreateCell(4);
            cell4.SetCellValue("快递状态（0-待取件 1-已取件 2-异常）");
            cell4.CellStyle = headerStyle;
            mySheet.SetColumnWidth(4, 10 * 256);
			
            var cell5 = headerRow.CreateCell(5);
            cell5.SetCellValue("存放时间");
            cell5.CellStyle = headerStyle;
            mySheet.SetColumnWidth(5, 10 * 256);
			
            var cell6 = headerRow.CreateCell(6);
            cell6.SetCellValue("取件时间");
            cell6.CellStyle = headerStyle;
            mySheet.SetColumnWidth(6, 10 * 256);
			
            var cell7 = headerRow.CreateCell(7);
            cell7.SetCellValue("快递存放位置");
            cell7.CellStyle = headerStyle;
            mySheet.SetColumnWidth(7, 10 * 256);
			
            var cell8 = headerRow.CreateCell(8);
            cell8.SetCellValue("操作员ID");
            cell8.CellStyle = headerStyle;
            mySheet.SetColumnWidth(8, 10 * 256);
			
            var cell9 = headerRow.CreateCell(9);
            cell9.SetCellValue("包裹尺寸分类（S-小件/M-中件/L-大件）");
            cell9.CellStyle = headerStyle;
            mySheet.SetColumnWidth(9, 10 * 256);
			
            var cell10 = headerRow.CreateCell(10);
            cell10.SetCellValue("包裹重量（单位：克）");
            cell10.CellStyle = headerStyle;
            mySheet.SetColumnWidth(10, 10 * 256);
			
            var cell11 = headerRow.CreateCell(11);
            cell11.SetCellValue("包裹照片URL");
            cell11.CellStyle = headerStyle;
            mySheet.SetColumnWidth(11, 10 * 256);
			
            var cell12 = headerRow.CreateCell(12);
            cell12.SetCellValue("创建时间");
            cell12.CellStyle = headerStyle;
            mySheet.SetColumnWidth(12, 10 * 256);
			
            var cell13 = headerRow.CreateCell(13);
            cell13.SetCellValue("最后更新时间");
            cell13.CellStyle = headerStyle;
            mySheet.SetColumnWidth(13, 10 * 256);
			

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
            rowTemp1.SetCellValue(listModel[i].phone_number.ToString());
            rowTemp1.CellStyle = commonCellStyle;



            var rowTemp2 = rowTemp.CreateCell(2);
            rowTemp2.SetCellValue(listModel[i].store_id.ToString());
            rowTemp2.CellStyle = commonCellStyle;



            var rowTemp3 = rowTemp.CreateCell(3);
            rowTemp3.SetCellValue(listModel[i].tracking_number.ToString());
            rowTemp3.CellStyle = commonCellStyle;



            var rowTemp4 = rowTemp.CreateCell(4);
            rowTemp4.SetCellValue(listModel[i].parcel_status.ToString());
            rowTemp4.CellStyle = commonCellStyle;



            var rowTemp5 = rowTemp.CreateCell(5);
            rowTemp5.SetCellValue(listModel[i].storage_time.ToString());
            rowTemp5.CellStyle = commonCellStyle;



            var rowTemp6 = rowTemp.CreateCell(6);
            rowTemp6.SetCellValue(listModel[i].pickup_time.ToString());
            rowTemp6.CellStyle = commonCellStyle;



            var rowTemp7 = rowTemp.CreateCell(7);
            rowTemp7.SetCellValue(listModel[i].storage_location.ToString());
            rowTemp7.CellStyle = commonCellStyle;



            var rowTemp8 = rowTemp.CreateCell(8);
            rowTemp8.SetCellValue(listModel[i].operator_id.ToString());
            rowTemp8.CellStyle = commonCellStyle;



            var rowTemp9 = rowTemp.CreateCell(9);
            rowTemp9.SetCellValue(listModel[i].size_type.ToString());
            rowTemp9.CellStyle = commonCellStyle;



            var rowTemp10 = rowTemp.CreateCell(10);
            rowTemp10.SetCellValue(listModel[i].weight.ToString());
            rowTemp10.CellStyle = commonCellStyle;



            var rowTemp11 = rowTemp.CreateCell(11);
            rowTemp11.SetCellValue(listModel[i].photo_url.ToString());
            rowTemp11.CellStyle = commonCellStyle;



            var rowTemp12 = rowTemp.CreateCell(12);
            rowTemp12.SetCellValue(listModel[i].created_time.ToString());
            rowTemp12.CellStyle = commonCellStyle;



            var rowTemp13 = rowTemp.CreateCell(13);
            rowTemp13.SetCellValue(listModel[i].updated_time.ToString());
            rowTemp13.CellStyle = commonCellStyle;


            }
            // 写入到excel
            string webRootPath = _webHostEnvironment.WebRootPath;
            string tpath = "/files/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + "-CoreCmsParcelStorage导出(查询结果).xls";
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
