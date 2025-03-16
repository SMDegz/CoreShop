<template>
	<view class="u-wrap">
		<u-toast ref="uToast" /><u-no-network></u-no-network>
		<u-navbar :is-back="false" :background="background">
			<view class="slot-wrap">
				<u-search :show-action="true" shape="round" v-model="searchKey" action-text="搜索" placeholder="请输入搜索内容"
					@custom="goSearch" @search="goSearch" :action-style="actionStyle"></u-search>
			</view>
		</u-navbar>

		<view class="u-padding-20" v-if="CateStyle==1">
			<u-row gutter="16">
				<u-col span="12" v-for="(item,index) in tabbar" :key="index" @click="goClass(item.id)">
					<u-image width="100%" height="300rpx" :src="item.imageUrl">
						<u-loading slot="loading"></u-loading>
					</u-image>
					<view class="u-text-center u-padding-top-30 u-padding-bottom-30">
						{{item.name}}
					</view>
				</u-col>
			</u-row>
		</view>
		<view class="u-padding-top-20 u-padding-bottom-20" v-if="CateStyle==2">
			<u-row gutter="16">
				<u-col span="4" v-for="(item,index) in tabbar" :key="index" @click="goClass(item.id)">
					<u-image width="100%" height="210rpx" :src="item.imageUrl">
						<u-loading slot="loading"></u-loading>
					</u-image>
					<view class="u-text-center u-padding-top-30 u-padding-bottom-30">
						{{item.name}}
					</view>
				</u-col>
			</u-row>
		</view>
		<view class="u-menu-wrap" v-if="CateStyle==3">
			<scroll-view scroll-y scroll-with-animation class="u-tab-view menu-scroll-view" :scroll-top="scrollTop">
				<view v-for="(item,index) in tabbar" :key="index" class="u-tab-item"
					:class="[current==index ? 'u-tab-item-active' : '']" :data-current="index"
					@tap.stop="swichMenu(item,index)">
					<text class="u-line-1">{{item.name}}</text>
				</view>
			</scroll-view>
			<block v-for="(item,index) in tabbar" :key="index">
				<scroll-view scroll-y class="right-box" v-if="current==index">
					<view class="page-view">
						<view class="class-item">
							<view class="item-title">
								<text>{{item.name}}</text>
							</view>
							<view class="item-container">
								<view class="thumb-box" v-for="(item1, index1) in item.child" :key="index1"
									@click="goClass(item1.id)">
									<u-image width="120rpx" height="120rpx" :src="item1.imageUrl">
										<u-loading slot="loading"></u-loading>
									</u-image>
									<view class="item-menu-name">{{item1.name}}</view>
								</view>
							</view>
						</view>
					</view>
				</scroll-view>
			</block>
		</view>


		<view class="u-menu-wrap" v-if="CateStyle==4">
			<scroll-view scroll-y scroll-with-animation class="u-tab-view menu-scroll-view" :scroll-top="scrollTop">
				<view v-for="(item,index) in tabbar" :key="index" class="u-tab-item"
					:class="[current==index ? 'u-tab-item-active' : '']" :data-current="index"
					@tap.stop="swichMenu(item,index)">
					<text class="u-line-1">{{item.name}}</text>
				</view>
			</scroll-view>
			<block v-for="(item,index) in tabbar" :key="index">
				<scroll-view scroll-y class="right-box" v-if="current==index">
					<view class="page-view">
						<view class="class-item">
							<view class="item-title">
								<text>{{item.name}}</text>
							</view>
							<view class="item-container">
								<view class="thumb-box" v-for="(item1, index1) in item.child" :key="index1"
									@click="goGoodsByClass(item1.id,index1)">
									<u-image width="120rpx" height="120rpx" :src="item1.imageUrl">
										<u-loading slot="loading"></u-loading>
									</u-image>
									<view class="item-menu-name">{{item1.name}}</view>
								</view>
							</view>
						</view>
						<!-- 商品列表 -->
						<view>
							<scroll-view scroll-y="true" :scroll-into-view="toView" class="scroll-Y"
								@scrolltolower="lower" enable-back-to-top="true" lower-threshold="45">
								<!-- 列表图片 -->
								<view v-if="currentList === 1">
									<view v-if="goodsList.length > 0">
										<view class="img-list-item" v-for="(item_good, index_good) in goodsList"
											:key="index_good" >
											<view class="good_box">
												<u-row gutter="5" justify="space-between">
													<u-col span="4">
														<!-- 警告：微信小程序中需要hx2.8.11版本才支持在template中结合其他组件，比如下方的lazy-load组件 -->
														<u-lazy-load threshold="-150" border-radius="10"
															:image="item_good.image"
															:index="item_good.id" @click="goGoodsDetail(item_good.id)"></u-lazy-load>
														<view class="good-tag-recommend2" v-if="item_good.isRecommend">
															推荐
														</view>
														<view class="good-tag-hot" v-if="item_good.isHot">
															热门
														</view>
													</u-col>
													<u-col span="8">
														<view class="contentBody">
															<view @click="goGoodsDetail(item_good.id)">
																<view
																	class="good_title-xl u-line-2  u-padding-left-10 u-padding-right-10">
																	{{item_good.name}}
																</view>
																<view class="good-price u-padding-10">
																	{{item_good.price}}元
																	<!-- <span class="u-font-xs  coreshop-text-through u-margin-left-15 coreshop-text-gray">{{item.mktprice}}元</span> -->
																</view>
																<view class="good-des u-padding-10"
																	v-if="item_good.commentsCount > 0">
																	{{ item_good.commentsCount }}条评论
																</view>
																<view class="good-des u-padding-10"
																	v-else-if="item_good.commentsCount <= 0">
																	暂无评论
																</view>
															</view>
															<view>
																<u-icon name="shopping-cart" color="#2979ff" size="40" class="btnCart"
															@click.stop="clickHandle(item_good.id)" v-if="item_good.stock > 0"></u-icon>
																<u-icon name="shopping-cart" color="#cfcfcf" size="40" class="btnCart" v-else></u-icon>
															</view>
														</view>
													</u-col>
												</u-row>
											</view>
										</view>
										<u-loadmore :status="loadStatus" :icon-type="loadIconType" :load-text="loadText"
											margin-top="20" margin-bottom="20" />
									</view>
									<view class="coreshop-emptybox" v-else>
										<u-empty :src="$globalConstVars.apiFilesUrl+'/static/images/empty/data.png'"
											icon-size="300" text="当前列表为空" mode="list"></u-empty>
									</view>
								</view>
							</scroll-view>
						</view>
					</view>
				</scroll-view>
			</block>
		</view>

		<!-- 登录提示 -->
		<coreshop-login-modal></coreshop-login-modal>
	</view>
</template>
<script>
	import { mapMutations, mapActions, mapState } from 'vuex';
	import {
		goods
	} from '@/common/mixins/mixinsHelper.js';
	export default {
		mixins: [goods],
		data() {
			return {
				background: {
					backgroundColor: '#e54d42',
				},
				actionStyle: {
					color: '#ffffff',
				},
				tabbar: [],
				scrollTop: 0, //tab标题的滚动条位置
				current: 0, // 预设当前项的值
				currentChild: 0, // 预设当前项的子值
				menuHeight: 0, // 左边菜单的高度
				menuItemHeight: 0, // 左边菜单item的高度,
				beans: [],
				advert: {},
				isChild: false,
				searchKey: '',

				title: '列表',
				currentList: 1,
				id: '',
				showView: false,
				goodsList: [],

				scrollTop: 0,

				loadStatus: 'loadmore',
				loadIconType: 'flower',
				loadText: {
					loadmore: '轻轻上拉',
					loading: '努力加载中',
					nomore: '实在没有了'
				},

				toView: '',
				searchData: {
					where: {},
					limit: 10,
					page: 1,
					order: {
						key: 'sort',
						sort: 'asc'
					}
				},
				searchKey: '请输入关键字搜索', //关键词
				alllist: true,
				allgrid: false,
				screents: true,
				screentc: false,
				
				buyNum:1,
				submitStatus: false,
				cartNums: 0, // 购物车数量
				type: 1, // 1加入购物车 2购买
				goodsInfo: { album: [] }, // 商品详情
				product: {}, // 货品详情
				isfav: false, // 商品是否收藏
				minBuyNum: 1, // 最小可购买数量

				currentData: [{
						label: '表格',
						value: 0,
					},
					{
						label: '列表',
						value: 1,
					}
				],
			}
		},
		computed: {
			...mapState({
			    hasLogin: state => state.hasLogin,
			    userInfo: state => state.userInfo,
			}),
			hasLogin: {
			    get() {
			        return this.$store.state.hasLogin;
			    },
			    set(val) {
			        this.$store.commit('hasLogin', val);
			    }
			},
			userInfo: {
			    get() {
			        return this.$store.state.userInfo;
			    },
			    set(val) {
			        this.$store.commit('userInfo', val);
			    }
			},
			CateStyle() {
				return this.$store.state.config.cateStyle ? this.$store.state.config.cateStyle : 3;
			}
		},
		onShow() {
		    this.submitStatus = false;
		},
		onLoad() {
			this.categories();
			this.getBanner();


		},
		onPageScroll(e) {
			this.scrollTop = e.scrollTop;
		},
		methods: {
			getGoodsPro: function(catId) {
				var where = {};
				if (catId) {
				    where.catId = catId;
					where.hot=true;
				}
				this.searchData.where = where;
				this.setSearchData(this.searchData, true);
				this.getGoods();
			},
			getGoods: function() {
				var _this = this;

				_this.$u.api.goodsList(_this.conditions()).then(res => {
					console.log(res)
					if (res.status) {
						if (res.data.className != '') {
							_this.title = res.data.className;
						} else {
							if (res.data.where && res.data.where.searchName && res.data.where.searchName !=
								'') {
								_this.title = "商品搜索";
							}
						}
						_this.goodsList = _this.goodsList.concat(res.data.list);
						if(res.data.list == null  || res.data.list.length <=0){
							_this.$refs.uToast.show({ title: "火速上架中...", type: 'success', back: false });
						}


						//console.log(_this.searchData);
						if (res.data.totalPages > _this.searchData.page) {
							_this.loadStatus = 'loadmore';
							_this.searchData.page++;
						} else {
							// 数据已加载完毕
							_this.loadStatus = 'nomore';
						}

					}
				});
			},
			// 统一返回筛选条件 查询条件 分页
			conditions() {
				let data = this.searchData;
				var newData = this.$u.deepClone(data);
				if (data.where) {
					newData.where = JSON.stringify(data.where);
				}
				//把排序换成字符串
				if (data.order) {
					var sort = data.order.key + ' ' + data.order.sort;
					if (data.order.key != 'sort') {
						sort = sort + ',sort asc'; //如果不是综合排序，增加上第二个排序优先级排序
					}
					newData.order = sort;
				} else {
					newData.order = 'sort asc';
				}
				return newData;
			},
			goGoodsByClass:function(catId,index) {
				this.currentChild = index
				this.getGoodsPro(catId)
			},
			//上拉加载
			lower: function() {
				var _this = this;
				_this.toView = 'loading';

				if (!_this.loadingComplete) {
					_this.setSearchData({
						page: _this.searchData.page + 1
					});
					_this.getGoods();
				}
			},
			listgrid: function() {
				let _this = this;
				if (_this.alllist) {
					_this.allgrid = true;
					_this.listgrid = true;
					_this.alllist = false;
				} else {
					_this.allgrid = false;
					_this.listgrid = false;
					_this.alllist = true;
				}
			},
			listGrid() {
				if (this.currentList == 0) {
					this.currentList = 1;
				} else {
					this.currentList = 0;
				}
			},
			//设置查询条件
			setSearchData: function(searchData, clear = false) {
				// 深度克隆
				this.searchData = this.$u.deepClone(searchData);

				if (clear) {
					this.goodsList = [];
				}
			},
			onChangeShowState: function() {
				var _this = this;
				_this.showView = !_this.showView;
			},
			categories() {
				var _this = this;
				_this.$u.api.categories().then(res => {
					console.log(res)
					if (res.status) {
						_this.tabbar = res.data;
						if(res.data !=null && res.data.length > 0 && res.data[0].child.length > 0){
							_this.goGoodsByClass(res.data[_this.current].child[_this.currentChild].id,_this.currentChild)
						}
					}
				});
			},
			getImg() {
				return Math.floor(Math.random() * 35);
			},
			// 点击左边的栏目切换
			async swichMenu(item,index) {
				console.log(item)
				if (index == this.current) return;
				this.current = index;
				// 如果为0，意味着尚未初始化
				if (this.menuHeight == 0 || this.menuItemHeight == 0) {
					await this.getElRect('menu-scroll-view', 'menuHeight');
					await this.getElRect('u-tab-item', 'menuItemHeight');
				}
				// 将菜单菜单活动item垂直居中
				this.scrollTop = index * this.menuItemHeight + this.menuItemHeight / 2 - this.menuHeight / 2;
				if(this.tabbar !=null && this.tabbar.length > 0 && this.tabbar[0].child.length > 0){
					var classId =this.tabbar[index].child[0].id;
					this.goGoodsByClass(classId,0)
				}
				
			},
			// 获取一个目标元素的高度
			getElRect(elClass, dataVal) {
				new Promise((resolve, reject) => {
					const query = uni.createSelectorQuery().in(this);
					query.select('.' + elClass).fields({
						size: true
					}, res => {
						// 如果节点尚未生成，res值为null，循环调用执行
						if (!res) {
							setTimeout(() => {
								this.getElRect(elClass);
							}, 10);
							return;
						}
						this[dataVal] = res.height;
					}).exec();
				})
			},
			goClass(catId) {
				uni.navigateTo({
					url: '/pages/category/list/list?id=' + catId
				});
			},
			getBanner() {
				this.$u.api.advert({
					codes: 'tpl1_class_banner1'
				}).then(res => {
					this.advert = res.data;
				});
			},
			// 广告点击查看详情
			showSliderInfo(type, val) {
				if (type == 1) {
					if (val.indexOf('http') != -1) {
						// #ifdef H5
						window.location.href = val
						// #endif
					} else {
						// #ifdef H5 || APP-PLUS || APP-PLUS-NVUE || MP
						if (val == '/pages/index/default/default' || val == '/pages/category/index/index' || val ==
							'/pages/index/cart/cart' || val == '/pages/index/member/member') {
							this.$u.route({
								type: 'switchTab',
								url: val
							});
							return;
						} else if (val.indexOf('/pages/coupon/coupon') > -1) {
							var id = val.replace('/pages/coupon/coupon?id=', "");
							this.receiveCoupon(id)
						} else {
							this.$u.route(val);
							return;
						}
						// #endif
					}
				} else if (type == 2) {
					// 商品详情
					this.goGoodsDetail(val);
				} else if (type == 3) {
					// 文章详情
					this.$u.route('/pages/article/details/details?id=' + val + '&idType=1');
				} else if (type == 4) {
					// 文章列表
					this.$u.route('/pages/article/list/list?cid=' + val);
				}
			},
			goSearch() {
				if (this.searchKey != '') {
					this.$u.route('/pages/category/list/list?key=' + this.searchKey);
				} else {
					this.$refs.uToast.show({
						title: '请输入查询关键字',
						type: 'warning',
					})
				}
			},
			
			
			// 加入购物车
			addToCart(goodId) {
			    let data = {
			        productId: goodId,
			        nums: this.buyNum
			    }
			    this.$u.api.addCart(data).then(res => {
					console.log("addcart")
					console.log(res)
			        this.submitStatus = false;
			        if (res.status) {
			            this.getCartNums(); // 获取购物车数量
			            this.$refs.uToast.show({ title: res.msg, type: 'success', back: false });
			        } else {
			            this.$u.toast(res.msg);
			        }
			    });
			},
			// 获取购物车数量
			getCartNums() {
			    let userToken = this.$db.get("userToken");
			    if (userToken && userToken != '') {
			        // 获取购物车数量
			        this.$u.api.getCartNum().then(res => {
						console.log("getCartNums")
						console.log(res)
			            if (res.status) {
			                this.cartNums = res.data;
			            }
			        })
			    }
			},
			clickHandle(goodId) {
			    if (!this.hasLogin) {
			        this.$store.commit('showLoginTip', true);
			        return false;
			    }
			    this.submitStatus = true;
				this.getGoodsDetail(goodId);
				console.log(this.product)
			   
			},
			// 获取商品详情
			getGoodsDetail(goodsId) {
			    let _this = this;
			    let data = {
			        id: parseInt(goodsId)
			    }
			    // 如果用户已经登录 要传用户token
			    let userToken = this.$db.get("userToken");
			    if (userToken) {
			        this.$u.api.goodsDetailByToken(data).then(res => {
						console.log("getGoodsDetail2")
						console.log(res)
			            if (res.status == true) {
			                let info = res.data;
			                let products = res.data.product;
			                _this.goodsInfo = info;
			                _this.isfav = res.data.isFav;
			                _this.product = _this.spesClassHandle(products);
			
			                _this.buyNum = _this.product.stock >= _this.minBuyNum ? _this.minBuyNum : 0;
			                // 判断如果登录用户添加商品浏览足迹
			                // if (userToken) {
			                //     _this.goodsBrowsing();
			                // }
							_this.addToCart(_this.product.id);
			            } else {
			                _this.$refs.uToast.show({ title: res.msg, type: 'error', back: true })
			            }
			        })
			    } else {
			        this.$u.api.goodsDetail(data).then(res => {
			            if (res.status == true) {
			                let info = res.data;
			                let products = res.data.product;
			                _this.goodsInfo = info;
			                _this.isfav = res.data.isFav;
			                _this.product = _this.spesClassHandle(products);
			
			                _this.buyNum = _this.product.stock >= _this.minBuyNum ? _this.minBuyNum : 0;
			                // 判断如果登录用户添加商品浏览足迹
			                // if (userToken) {
			                //     _this.goodsBrowsing();
			                // }
			            } else {
			                _this.$refs.uToast.show({ title: res.msg, type: 'error', back: true })
			            }
			        })
			    }
			
			},
			// 多规格样式统一处理
			spesClassHandle(products) {
			    // 判断是否是多规格 (是否有默认规格)
			    if (products.hasOwnProperty('defaultSpecificationDescription')) {
			        let spes = products.defaultSpecificationDescription;
			        for (let key in spes) {
			            for (let i in spes[key]) {
			                if (spes[key][i].hasOwnProperty('isDefault') && spes[key][i].isDefault === true) {
			                    this.$set(spes[key][i], 'cla', 'selected');
			                } else if (spes[key][i].hasOwnProperty('productId') && spes[key][i].productId) {
			                    this.$set(spes[key][i], 'cla', 'not-selected');
			                } else {
			                    this.$set(spes[key][i], 'cla', 'none');
			                }
			            }
			        }
			        spes = JSON.stringify(spes)
			        products.defaultSpecificationDescription = spes;
			    }
			    return products;
			},
		}
	}
</script>
<style lang="scss" scoped>
	@import "index.scss";
</style>