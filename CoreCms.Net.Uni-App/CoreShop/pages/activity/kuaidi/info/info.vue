﻿<template>
    <view class="pageBox">
        <u-navbar title="限时秒杀"></u-navbar>
        <view class="tab-box coreshop-flex coreshop-align-center">
            <view class="tab-item" @tap="onTab(tab.id,tab.status)" :class="{ 'tab-active': tabCurrent === tab.id }" v-for="tab in tabList" :key="tab.id">
                <text class="tab-title">{{ tab.title }}</text>
                <text v-show="tabCurrent === tab.id" class="tab-triangle"></text>
            </view>
        </view>
        <view class="content-box">
            <scroll-view scroll-y="true" enable-back-to-top @scrolltolower="loadMore" class="scroll-box">
                <view class="goods-item" v-for="item in goodsList" :key="item.id" v-if="goodsList.length>0">
                    <view class="activity-goods-box u-flex u-row-between">
                        <view class="img-box">
                            <slot name="tag"></slot>
                            <image class="img" :src="item.image" mode="aspectFill"></image>
                        </view>
                        <view class="goods-right u-flex u-row-between coreshop-flex-direction">
                            <view class="title u-line-1">{{ item.name }}</view>
                            <view class="tip u-line-1">{{ item.brief }}</view>
                            <view class="slod-end">
                                <view class="coreshop-flex coreshop-align-center">
                                    <u-line-progress :striped="true" :percent="getPercent(item.buyPromotionCount, item.stock)" :striped-active="true" :show-percent="true" class="cu-progress"></u-line-progress>
                                    <view class="progress-text">已抢{{ getProgress(item.buyPromotionCount, item.stock) }}</view>
                                </view>
                            </view>
                            <view class=" price-box">
                                <view class="coreshop-flex coreshop-align-center">
                                    <view class="current">￥{{ item.price }}</view>
                                    <view class="original">￥{{ item.mktprice }}</view>
                                </view>
                            </view>
                            <u-button type="error" class="buy-btn" size="mini" v-if="tabCurrent=='ing'" @click="goSeckillDetail(item.id, item.groupId)">{{ btnType[tabCurrent].name }}</u-button>
                            <u-button type="error" class="buy-btn" size="mini" v-else>{{ btnType[tabCurrent].name }}</u-button>
                        </view>
                    </view>

                </view>
                <!-- 无数据时默认显示 -->
                <view class="coreshop-emptybox" v-else>
                    <u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'" icon-size="300" text="暂无秒杀信息" mode="list"></u-empty>
                </view>
                <!-- 加载更多 -->
                <u-loadmore :status="loadStatus" :icon-type="iconType" :load-text="loadText" margin-top="20" margin-bottom="20" />
            </scroll-view>
        </view>
        <!-- 登录提示 -->
        <coreshop-login-modal></coreshop-login-modal>
    </view>
</template>

<script>
    import { commonUse, goods } from '@/common/mixins/mixinsHelper.js';
    export default {
        mixins: [commonUse, goods],
        data() {
            return {
                loadStatus: 'loadmore',
                iconType: 'flower',
                loadText: {
                    loadmore: '轻轻上拉',
                    loading: '努力加载中',
                    nomore: '实在没有了'
                },
                loading: false,
                page: 1,
                limit: 10,
                lastPage: 1,
                status: 1,
                tabCurrent: 'ing',
                goodsList: [],
                btnType: {
                    ing: {
                        name: '立即抢购',
                        color: 'btn-ing'
                    },
                    nostart: {
                        name: '尚未开始',
                        color: 'btn-nostart'
                    },
                    ended: {
                        name: '已结束',
                        color: 'btn-end',
                    },
                },
                tabList: [
                    {
                        id: 'ing',
                        title: '进行中',
                        status: 1

                    },
                    {
                        id: 'nostart',
                        title: '即将开始',
                        status: 0
                    },
                    {
                        id: 'ended',
                        title: '已结束',
                        status: 2
                    },
                ]
            };
        },
        onLoad() {
            setTimeout(() => {
                this.loading = true;
            }, 500);
            this.getGoodsList();
        },
        //onReachBottom() {
        //    if (this.loadStatus === 'loadmore') {
        //        this.getGoodsList()
        //    }
        //},
        methods: {
            onTab(id, status) {
                this.tabCurrent = id;
                this.status = status;
                this.goodsList = [];
                this.page = 1;
                this.loadStatus = 'loading';
                this.$u.debounce(this.getGoodsList, 500);
            },
            // 百分比
            getProgress(sales, stock) {
                let unit = '';
                if (stock + sales > 0) {
                    let num = (sales / (sales + stock)) * 100;
                    unit = num.toFixed(2) + '%';
                } else {
                    unit = '0%';
                }
                return unit;
            },
            // 进度数
            getPercent(sales, stock) {
                let unit = 0;
                if (stock + sales > 0) {
                    let num = (sales / (sales + stock)) * 100;
                } else {
                    unit = 30;
                }
                return unit;
            },
            // 加载更多
            loadMore() {
                if (this.page < this.lastPage) {
                    this.page += 1;
                    this.getGoodsList();
                }
            },
            // 秒杀列表
            getGoodsList() {
                let _this = this;
                let data = {
                    page: this.page,
                    limit: this.limit,
                    type: 4, //秒杀
                    status: this.status
                }
                this.loadStatus = 'loading';
                this.$u.api.getGroup(data).then(res => {
                    if (res.status) {
                        if (res.data) {
                            let _goodsList = res.data.goods;
                            _this.goodsList = [..._this.goodsList, ..._goodsList]
                        }
                        _this.lastPage = res.data.totalPages;
                        if (_this.page < res.data.totalPages) {
                            _this.page++
                            _this.loadStatus = 'loadmore'
                        } else {
                            _this.loadStatus = 'nomore'
                        }
                    } else {
                        _this.$u.toast(res.msg)
                    }
                });

            }
        }
    }
</script>

<style lang="scss" scoped>
    @import "info.scss";
</style>
