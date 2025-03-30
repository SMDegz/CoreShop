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
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using CoreCms.Net.Caching.Manual;
using CoreCms.Net.Configuration;
using CoreCms.Net.Model.Entities;
using CoreCms.Net.Model.ViewModels.Basics;
using CoreCms.Net.IRepository;
using CoreCms.Net.IRepository.UnitOfWork;
using CoreCms.Net.Model.ViewModels.UI;
using SqlSugar;

namespace CoreCms.Net.Repository
{
    /// <summary>
    /// 快递信息表 接口实现
    /// </summary>
    public class CoreCmsParcelStorageRepository : BaseRepository<CoreCmsParcelStorage>, ICoreCmsParcelStorageRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoreCmsParcelStorageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

       #region 实现重写增删改查操作==========================================================

        /// <summary>
        /// 重写异步插入方法
        /// </summary>
        /// <param name="entity">实体数据</param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> InsertAsync(CoreCmsParcelStorage entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Insertable(entity).ExecuteReturnIdentityAsync() > 0;
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.CreateSuccess : GlobalConstVars.CreateFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(CoreCmsParcelStorage entity)
        {
            var jm = new AdminUiCallBack();

            var oldModel = await DbClient.Queryable<CoreCmsParcelStorage>().In(entity.id).SingleAsync();
            if (oldModel == null)
            {
            jm.msg = "不存在此信息";
            return jm;
            }
            //事物处理过程开始
        	oldModel.id = entity.id;
            oldModel.phone_number = entity.phone_number;
            oldModel.store_id = entity.store_id;
            oldModel.tracking_number = entity.tracking_number;
            oldModel.parcel_status = entity.parcel_status;
            oldModel.storage_time = entity.storage_time;
            oldModel.pickup_time = entity.pickup_time;
            oldModel.storage_location = entity.storage_location;
            oldModel.operator_id = entity.operator_id;
            oldModel.size_type = entity.size_type;
            oldModel.weight = entity.weight;
            oldModel.photo_url = entity.photo_url;
            oldModel.created_time = entity.created_time;
            oldModel.updated_time = entity.updated_time;
            
            //事物处理过程结束
            var bl = await DbClient.Updateable(oldModel).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> UpdateAsync(List<CoreCmsParcelStorage> entity)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Updateable(entity).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写异步更新方法更新Ids列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ids"></param>
        /// <param name="fieldExpression"></param>
        /// <returns></returns>
        public async Task<AdminUiCallBack>  UpdateFieldByIds<T>(List<int> ids, Expression<Func<T, object>> fieldExpression)
        {
            var jm = new AdminUiCallBack();

            var queryable = DbClient.Queryable<CoreCmsParcelStorage>();
            var queryables = queryable.In(ids).ToList();
            foreach (var item in queryables)
            {
                item.parcel_status = 2;
            }
            var bl = await DbClient.Updateable<CoreCmsParcelStorage>(queryables).ExecuteCommandHasChangeAsync();

            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.EditSuccess : GlobalConstVars.EditFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }



        /// <summary>
        /// 重写删除指定ID的数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdAsync(object id)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsParcelStorage>(id).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        /// <summary>
        /// 重写删除指定ID集合的数据(批量删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public new async Task<AdminUiCallBack> DeleteByIdsAsync(int[] ids)
        {
            var jm = new AdminUiCallBack();

            var bl = await DbClient.Deleteable<CoreCmsParcelStorage>().In(ids).ExecuteCommandHasChangeAsync();
            jm.code = bl ? 0 : 1;
            jm.msg = bl ? GlobalConstVars.DeleteSuccess : GlobalConstVars.DeleteFailure;
            if (bl)
            {
                await UpdateCaChe();
            }

            return jm;
        }

        #endregion

       #region 获取缓存的所有数据==========================================================

        /// <summary>
        /// 获取缓存的所有数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<CoreCmsParcelStorage>> GetCaChe()
        {
            var cache = ManualDataCache.Instance.Get<List<CoreCmsParcelStorage>>(GlobalConstVars.CacheCoreCmsParcelStorage);
            if (cache != null)
            {
                return cache;
            }
            return await UpdateCaChe();
        }

        /// <summary>
        ///     更新cache
        /// </summary>
        public async Task<List<CoreCmsParcelStorage>> UpdateCaChe()
        {
            var list = await DbClient.Queryable<CoreCmsParcelStorage>().With(SqlWith.NoLock).ToListAsync();
            ManualDataCache.Instance.Set(GlobalConstVars.CacheCoreCmsParcelStorage, list);
            return list;
        }

        #endregion


        #region 重写根据条件查询分页数据
        /// <summary>
        ///     重写根据条件查询分页数据
        /// </summary>
        /// <param name="predicate">判断集合</param>
        /// <param name="orderByType">排序方式</param>
        /// <param name="pageIndex">当前页面索引</param>
        /// <param name="pageSize">分布大小</param>
        /// <param name="orderByExpression"></param>
        /// <param name="blUseNoLock">是否使用WITH(NOLOCK)</param>
        /// <returns></returns>
        public new async Task<IPageList<CoreCmsParcelStorage>> QueryPageAsync(Expression<Func<CoreCmsParcelStorage, bool>> predicate,
            Expression<Func<CoreCmsParcelStorage, object>> orderByExpression, OrderByType orderByType, int pageIndex = 1,
            int pageSize = 20, bool blUseNoLock = false)
        {
            RefAsync<int> totalCount = 0;
            List<CoreCmsParcelStorage> page;
            if (blUseNoLock)
            {
                page = await DbClient.Queryable<CoreCmsParcelStorage>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsParcelStorage
                {
                      id = p.id,
                phone_number = p.phone_number,
                store_id = p.store_id,
                pickupcode = p.pickupcode,
                tracking_number = p.tracking_number,
                parcel_status = p.parcel_status,
                storage_time = p.storage_time,
                pickup_time = p.pickup_time,
                storage_location = p.storage_location,
                operator_id = p.operator_id,
                size_type = p.size_type,
                weight = p.weight,
                photo_url = p.photo_url,
                created_time = p.created_time,
                updated_time = p.updated_time,
                
                }).With(SqlWith.NoLock).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            else
            {
                page = await DbClient.Queryable<CoreCmsParcelStorage>()
                .OrderByIF(orderByExpression != null, orderByExpression, orderByType)
                .WhereIF(predicate != null, predicate).Select(p => new CoreCmsParcelStorage
                {
                      id = p.id,
                phone_number = p.phone_number,
                store_id = p.store_id,
                    pickupcode = p.pickupcode,
                    tracking_number = p.tracking_number,
                parcel_status = p.parcel_status,
                storage_time = p.storage_time,
                pickup_time = p.pickup_time,
                storage_location = p.storage_location,
                operator_id = p.operator_id,
                size_type = p.size_type,
                weight = p.weight,
                photo_url = p.photo_url,
                created_time = p.created_time,
                updated_time = p.updated_time,
                
                }).ToPageListAsync(pageIndex, pageSize, totalCount);
            }
            var list = new PageList<CoreCmsParcelStorage>(page, pageIndex, pageSize, totalCount);
            return list;
        }

        #endregion

    }
}
