<template>

	<view class="pageBox">
		<u-toast ref="uToast" /><u-no-network></u-no-network>
		<u-navbar title="快递到家"></u-navbar>
		<view class="tab-box coreshop-flex coreshop-align-center">
			<view class="tab-item" @tap="onTab(tab.id,tab.status)" :class="{ 'tab-active': tabCurrent === tab.id }"
				v-for="tab in tabList" :key="tab.id">
				<text class="tab-title">{{ tab.title }}</text>
				<text v-show="tabCurrent === tab.id" class="tab-triangle"></text>
			</view>
		</view>
		<view v-show="tabCurrent == 1" class="u-padding-20">
			<view>
				<!-- 门店信息 -->
				<view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box" v-if="store && store.id"
					@click="goStorelist()">
					<view class="coreshop-list menu  u-padding-30">
						<u-form :model="model" :rules="rules" ref="uForm" :errorType="errorType">
							<u-form-item label="派件时间：" label-width="150" prop="paijianDay">
								<u-input :border="border" type="select" :select-open="actionSheetShow"
									v-model="model.paijianDay" placeholder="请选择派件时间"
									@click="actionSheetShow = true"></u-input>
							</u-form-item>
						</u-form>
					</view>
					<view class="coreshop-bg-white coreshop-card address-view">

						<view class="coreshop-list menu-avatar">
							<view class="store-item coreshop-list-item">
								<!-- <view class="coreshop-bg-grey icon-view">
			                        <u-icon name="map"></u-icon>
			                    </view> -->
								<view class="coreshop-flex coreshop-align-center">
									<view class="img-box">
										<image class="store-img" :src="store.logoImage" mode="aspectFill" lazy-load>
										</image>
									</view>
									<view class="item-left coreshop-flex coreshop-flex-direction coreshop-align-start">
										<text class="store-title">{{ store.storeName }}</text>
										<text class="store-content">{{ store.address }}</text>
									</view>
								</view>
								<view class="action coreshop-text-gray">
									<u-icon name="arrow-right" @click="goStorelist()"></u-icon>
								</view>
							</view>
						</view>
						<view class="address-line" />

						<!-- 积分记录列表 -->
						<view v-if="kuaidiList.length>0">
							<view class="coreshop-log-item u-flex u-row-between" v-for="item in kuaidiList"
								:key="item.id">
								<view class="item-left coreshop-flex coreshop-align-center">
									<!--<image class="log-img" :src="item.buyer.avatar" mode=""></image>-->
									<view class="coreshop-flex coreshop-flex-direction coreshop-align-start">
										<view class="log-name coreshop-text-black">{{ item.tracking_number }}</view>
										<view class="log-notice coreshop-text-grey">{{ item.ParcelStatusName }}</view>
									</view>
								</view>
								<view class="item-right coreshop-flex coreshop-flex-direction coreshop-align-end">
									<view class="log-num coreshop-text-red">
										{{ item.storage_location }}
									</view>
									<view class="log-date coreshop-text-grey">
										{{ $u.timeFormat(item.created_time, 'yyyy.mm.dd') }}
									</view>
								</view>
							</view>
							<!-- 更多 -->
							<u-loadmore :status="status" :icon-type="iconType" :load-text="loadText" margin-top="20"
								margin-bottom="20" />
						</view>
					</view>
				</view>
				<view v-else class='u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box' @click="goStorelist()">
					<view class="coreshop-bg-white coreshop-card address-view">
						<view class='u-padding-20 u-text-center'>暂无门店</view>
						<view class="address-line" />
					</view>
				</view>
				<u-calendar v-model="showCalendar" :mode="calendarMode" :min-date="minDate" :max-date="maxDate"
					@change="calendarChange" safe-area-inset-bottom="true"></u-calendar>
				<u-action-sheet :list="actionSheetList" v-model="actionSheetShow"
					@click="actionSheetCallback"></u-action-sheet>
			</view>
		</view>
		<view v-show="tabCurrent == 2" class="u-padding-20">
			<u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
				<u-form-item label="收货人" label-width="180" prop="name">
					<u-input v-model="form.name" placeholder="请填写收货人姓名" />
				</u-form-item>
				<u-form-item label="手机号" label-width="180" prop="mobile">
					<u-input v-model="form.mobile" placeholder="请填写收货人手机号" />
				</u-form-item>

				<u-form-item label="省市区" label-width="180">
					<u-input :value="pickerValue" @click="showThreePicker" type="select"
						placeholder="请选择省市区区域"></u-input>
					<u-select v-model="show" mode="mutil-column-auto" :list="pickerList" :default-value="pickerIndex"
						@confirm="onConfirm"></u-select>
				</u-form-item>

				<u-form-item label="详细地址" label-width="180" prop="address">
					<u-input v-model="form.address" placeholder="请填写收货详细地址" />
				</u-form-item>

				<u-form-item label="设为默认" label-width="180">
					<u-switch slot="right" v-model="checked"></u-switch>
				</u-form-item>
			</u-form>
		</view>
		<view v-show="tabCurrent == 3" class="u-padding-20">
			<view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
				<view class="coreshop-bg-white coreshop-card pay-view">
					<view class="coreshop-list menu">
						<!-- 客户有服务卷的情况 -->
						<view class="coreshop-list-item" v-if="server!=null && server.redeemCode !='' ">
							<!--<view class="coreshop-list-item arrow">-->
							<view class="content">
								<view class="coreshop-text-black item-view">
									<view class="u-line-1 title" style="font-size: 30;">快递服务卷</view>
									
								</view>
								<view class="coreshop-text-gray u-font-xs flex">
									<text class="u-line-1">
										购买了{{server.allUse}}张！剩{{ server.yesUse }}张，本次卷码：
									</text>
									<text class="coreshop-text-red  u-text-right">
										{{ server.redeemCode }}
									</text>
								</view>
							</view>
							<view class="action" @click="changeServerHandle">
								<view class="coreshop-text-gray fr">
									<switch :class="isUseServer?'checked':''" :checked="isUseServer"></switch>
								</view>
							</view>
						</view>
					</view>
				</view>
			</view>
			<view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
				<view class="coreshop-bg-white coreshop-card pay-view">
					<view class="coreshop-list menu">
						<!-- 商户开启积分 并且用户有积分情况下 -->
						<view class="coreshop-list-item" v-if="isOpenPoint() === 1 && userPointNums > 0 && isUseServer == false">
							<!--<view class="coreshop-list-item arrow">-->
							<view class="content">
								<text class="coreshop-text-black">积分抵扣</text>
								<view class="coreshop-text-gray u-font-xs flex">
									<text class="u-line-1">
										可用{{ canUsePoint}}积分，可抵扣{{ pointMoney}}元，共有{{ userPointNums}}积分
									</text>
								</view>
							</view>
							<view class="action" @click="changePointHandle">
								<view class="coreshop-text-gray fr">
									<switch :class="isUsePoint?'checked':''" :checked="isUsePoint"></switch>
								</view>
							</view>
						</view>
					</view>
				</view>
			</view>
			<!--商品价格计算-->
			<view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
				<view class="coreshop-bg-white coreshop-card coreshop-price-view">

					<view class="coreshop-text-black item-view">
						<view class="u-line-1 title">商品总额</view>
						<text
							class="coreshop-text-red coreshop-text-price u-text-right">{{ cartData.goodsAmount }}</text>
					</view>

					<view class="coreshop-text-black item-view">
						<view class="u-line-1 title">订单优惠</view>
						<text class="u-text-right">- {{ cartData.orderPromotionMoney || '0' }}</text>
					</view>
					<view class="coreshop-text-black item-view">
						<view class="u-line-1 title">优惠券抵扣</view>
						<text class="u-text-right">- {{ cartData.couponPromotionMoney  || '0'}}</text>
					</view>
					<view class="coreshop-text-black item-view">
						<view class="u-line-1 title">积分抵扣</view>
						<text class="u-text-right">- {{ cartData.pointExchangeMoney || '0'}}</text>
					</view>
				</view>
			</view>
			<!--买家留言-->
			<view class="u-margin-top-20 u-margin-bottom-20 coreshop-common-view-box">
				<view class="coreshop-bg-white">
					<view class="u-margin-top-20 u-padding-20">
						<view class="title">买家留言</view>
					</view>
					<view class="u-padding-20">
						<textarea class="memoBox" maxlength="50" @input="memoChange" placeholder="50字以内(选填)"></textarea>
					</view>
				</view>
			</view>
			<!--占位底部距离-->
			<view class="coreshop-tabbar-height" />
			<!--底部操作-->
			<view class="coreshop-bg-white coreshop-footer-fixed coreshop-foot-padding-bottom">
				<view class="u-flex u-flex-nowrap u-row-between  u-padding-20 w100">
					<view class="coreshop-text-black coreshop-text-bold price-view">
						<text class="u-margin-right-20">共 {{ productNums()}} 件商品</text>
						<text>合计<text class="coreshop-text-price coreshop-text-red u-font-lg u-margin-left-20">
								{{ cartData.amount}}</text></text>
					</view>
					<u-button size="medium" type="error" @click="toPay" :disabled='submitStatus'
						:loading='submitStatus'>确认下单</u-button>
				</view>
			</view>
		</view>

		<view class="coreshop-bottomBox">
			<button class="coreshop-btn coreshop-btn-square coreshop-btn-w" @click="upShip()" v-if="tabCurrent != 1"
				:disabled='submitStatus' :loading='submitStatus'>上一步</button>
			<button class="coreshop-btn coreshop-btn-square coreshop-btn-b" @click="downShip()" :disabled='submitStatus'
				:loading='submitStatus'>{{btnText}}</button>
		</view>
	</view>
</template>

<script>
	import {
		commonUse,
		goods
	} from '@/common/mixins/mixinsHelper.js';
	export default {
		mixins: [commonUse, goods],
		data() {
			return {
				isUseServer: false, // 是否勾选使用积分
				
				isUsePoint: false, // 是否勾选使用积分
				userPointNums: 0, // 用户的总积分
				canUsePoint: 0, // 可以使用的积分
				pointMoney: 0, // 积分抵扣的金额
				userCoupons: [], // 用户的可用优惠券列表
				usedCoupons: {}, // 已经选择使用的优惠券
				inputCouponCode: '', // 输入的优惠券码
				optCoupon: '', // 当前选择使用的优惠券(暂存使用 如果接口返回不可用则剔除优惠券状态)
				cartData: {}, // 购物车商品详情
				memo: '', // 买家留言
				params: {
					ids: 0, // 传递过来的购物车id
					areaId: 0, // 收货地址id
					couponCode: '', // 优惠券码列表(string)多张逗号分隔
					point: 0, // 抵扣积分额
					type: 1, //购物车类型
					objectId: 0, //关联对象类型
				}, // 监听params参数信息 以重新请求接口

				server:null,
				errorType: ['message'],
				id: 0,
				form: {
					name: '',
					mobile: '',
					address: '',
					isDefault: 2,
				},
				region: ['福建省', '漳州市', '芗城区'],
				areaId: 0,
				checked: false,
				pickerValue: '',
				submitStatus: false,
				show: false,
				pickerList: this.$db.get("areaList"),
				province: this.$db.get("areaList"),
				pickerIndex: [0, 0, 0], // picker索引值
				provinceKey: -1, //省份id
				cityKey: -1, //市id
				areaKey: -1, //区域id
				rules: {
					name: [{
							required: true,
							message: '请输入姓名',
							trigger: 'blur',
						},
						{
							min: 2,
							max: 4,
							message: '姓名长度在2到4个字符',
							trigger: ['change', 'blur'],
						},
						{
							validator: (rule, value, callback) => {
								return this.$u.test.chinese(value);
							},
							message: '必须为中文',
							trigger: ['change', 'blur'],
						}
					],
					address: [{
							required: true,
							message: '请输入地址',
							trigger: 'blur',
						},
						{
							min: 5,
							max: 30,
							message: '地址长度在5到30个字符',
							trigger: ['change', 'blur'],
						}
					],
					mobile: [{
							required: true,
							message: '请输入手机号',
							trigger: ['change', 'blur'],
						},
						{
							validator: (rule, value, callback) => {
								return this.$u.test.mobile(value);
							},
							message: '手机号码不正确',
							trigger: ['change', 'blur'],
						}
					]
				},
				actionSheetList: [{
						text: '9:00-10:00'
					},
					{
						text: '14:00-15:00'
					},
					{
						text: '18:00-19:00'
					}
				],

				model: {
					paijianDay: '',
					paijianDayIndex: 0
				},
				showCalendar: false,
				actionSheetShow: false,
				minDate: '2025-03-02',
				storeSwitch: 1,
				// 新增 maxDate 变量用于存储最大日期
				maxDate: '2025-04-02',
				page: 1,
				limit: 10,
				kuaidiList: [], // 积分记录
				status: 'loadmore',
				iconType: 'flower',
				loadText: {
					loadmore: '轻轻上拉',
					loading: '努力加载中',
					nomore: '实在没有了'
				},

				store: {
					id: 0,
					name: '',
					mobile: '',
					address: ''
				},
				btnText: '下一步',
				tabCurrent: 1,
				loadStatus: 'loadmore',
				iconType: 'flower',
				loading: true,
				tabList: [{
						id: 1,
						title: '快递信息填写',
						status: 1

					},
					{
						id: 2,
						title: '收件信息填写',
						status: 0
					},
					{
						id: 3,
						title: '订单信息确认',
						status: 2
					},
				]
			};
		},
		onReady() {
			this.$refs.uForm.setRules(this.rules);
		},
		onLoad() {
			this.getDefaultStore();
			this.getShipInfoMoRen();
			this.userKuaidiList();
			// 获取用户的可用优惠券信息
			this.getUserCounpons();
			this.getUserServiceCard();
		},
		computed: {

		},
		mounted() {

		},
		// 页面滚动到底部触发事件
		onReachBottom() {
			let _this = this
			if (_this.status === 'loadmore') {
				_this.userKuaidiList()
			}
		},
		methods: {
			// 去支付
			toPay() {
				var submitDate = {};
				
				this.kuaidiList.forEach(item => {
					submitDate.expno = submitDate.expno + ',' + item.tracking_number
					submitDate.explist = submitDate.explist + ',' + item.id
				})
				if(submitDate.expno == ''){
					this.$u.toast('请选择需要代拿的快递');
					this.submitStatus = false;
					return false;
				}
				if(this.store==null){
					this.$u.toast('请选择自提门店');
					this.submitStatus = false;
					return false;
				}
				submitDate.submitDate = this.store.id
				submitDate.expcom = this.store.storeName
				
				if(this.form.name=''){
					this.$u.toast('请输入收件人');
					this.submitStatus = false;
					return false;
				}
				if(this.form.mobile=''){
					this.$u.toast('请输入手机号');
					this.submitStatus = false;
					return false;
				}
				if(this.pickerValue == ''){
					this.$u.toast('请选择省市区');
					this.submitStatus = false;
					return false;
				}
				if(this.address == ''){
					this.$u.toast('请选择详细地址');
					this.submitStatus = false;
					return false;
				}
				submitDate.recname = this.form.name
				submitDate.rectel = this.form.mobile
				submitDate.recaddr = this.pickerValue + this.address
				submitDate.sendtime = this.model.paijianDay
				submitDate.totalpay = this.cartData.goodsAmount
				submitDate.discount = this.cartData.goodsAmount - this.cartData.amount
				if(this.isUsePoint && this.userPointNums>0){
					submitDate.pointuse = this.userPointNums
				}
				if(this.isUseServer&&this.server.redeemCode!=''){
					submitDate.serverid = this.server.id
					submitDate.serverticker = this.server.redeemCode
				}
				submitDate.note = this.memo
			    this.submitStatus = true;
			    
			    this.$u.api.createOrder(data).then(res => {
			        this.submitStatus = false;
			        if (res.status) {
			            // 创建订单成功 去支付
			            // 判断是否为0元订单,如果是0元订单直接支付成功
			            if (res.data.payStatus == '2') {
			                this.$u.route({ type: 'redirectTo', url: '/pages/payment/result/result?orderId=' + res.data.orderId });
			            } else {
			                this.$u.route({ type: 'redirectTo', url: '/pages/payment/pay/pay?orderId=' + res.data.orderId + '&type=' + this.orderType });
			            }
			            this.subscription();
			        } else {
			            this.$u.toast(res.msg);
			        }
			    });
			},
			
			
			// 获取用户可用的服务卡信息
			getUserServiceCard() {
				let data = {
					id: 1005
				}
				this.$u.api.getDefaultServiceTicket(data).then(res => {
					console.log('hjx8')
					console.log(res)
					if (res.status) {
						this.server = res.data
					}else{
						this.server = null
					}
				})
			},
			// 获取用户可用的优惠券信息
			getUserCounpons() {
				let data = {
					display: 'no_used',
					ids: this.params.ids
				}
				this.$u.api.getCartCoupon(data).then(res => {
					if (res.status) {
						let _list = res.data.list
						let nowTime = Math.round(new Date().getTime() / 1000).toString()
						_list.forEach(item => {
							this.$set(item, 'checked', false)
							// 判断优惠券是否有效(开始时间)
							this.$set(item, 'disabled', item.startTime > nowTime ? true : false)
							this.$set(item, 'cla', item.disabled ? 'cci-l bg-c' : 'cci-l') // 绑定相应的class样式
						})
						this.userCoupons = _list
					}
				})
			},
			// 判断商户是否开启积分抵扣 1开启 2未开启
			isOpenPoint() {
				console.log('hjx5')
				return this.$store.state.config.pointSwitch
			},
			// 计算购物车商品数量
			productNums() {
				return this.kuaidiList.length;
			},
			memoChange(e) {
				//console.log(e);
				this.memo = e.detail.value
			},
			// 是否使用积分
			changePointHandle(e) {
				//this.switchA = e.detail.value
				if (this.userPointNums > 0) {
					this.isUsePoint = !this.isUsePoint;
					this.params.point = this.isUsePoint ? this.canUsePoint : 0;
					
					if (this.isUsePoint) {
						this.cartData.pointExchangeMoney = this.pointMoney;
						this.cartData.amount = (this.cartData.amount - this.pointMoney);
					} else {
						this.cartData.pointExchangeMoney = 0;
						this.cartData.amount = this.cartData.goodsAmount;
					}
				}
			},
			// 是否使用服务卡
			changeServerHandle(e) {
				if (this.server!=null && this.server.redeemCode != '') {
					this.isUseServer = !this.isUseServer;
					this.isUsePoint = false;
					this.params.point = 0;
					
					var cartData = this.cartData
					if (this.isUseServer) {
						cartData.pointExchangeMoney = 0;
						cartData.couponPromotionMoney = cartData.goodsAmount;
						cartData.amount = 0;
					} else {
						cartData.pointExchangeMoney = 0;
						cartData.couponPromotionMoney = 0;
						cartData.amount = cartData.goodsAmount;
					}
					this.cartData = cartData
					console.log('hjx9')
					console.log(this.isUseServer)
					console.log(this.cartData)
				}
			},


			// 获取默认店铺
			getDefaultStore() {
				if (this.storeSwitch == 1) {
					console.log("获取默认店铺");
					this.$u.api.defaultStore().then(res => {
						if (res.status) {
							if (res.data && res.data.id) {
								let store = {
									id: res.data.id || 0,
									name: res.data.storeName || '',
									mobile: res.data.mobile || '',
									address: res.data.address || '',
									logoImage: res.data.logoImage || ''
								}
								this.store = store;
							} else {
								this.$u.toast('商家未配置默认自提店铺！');
							}
						}
					});
				}
			},
			// 跳转到门店列表
			goStorelist() {
				console.log("跳转到门店列表");
				this.$u.route('/pages/placeOrder/storeList/storeList')
			},
			//获取默认门店
			// getDefaultStore() {
			//     let _this = this;
			//     let data = {
			// 		'key': '湖南',
			// 		'longitude': 0,
			// 		'latitude': 0,
			// 		'page': 1,
			// 		'limit': 10,
			// 	}
			// 	_this.$u.api.storeList(data).then(e => {
			// 		if (e.status) {
			// 			console.log(e);
			// 			e.data.fi
			// 			_this.storeList = [..._this.storeList, ...e.data]
			// 		} else {
			// 			_this.$u.toast("门店数据获取失败。");
			// 		}
			// 	});
			// },
			formatDate(date) {
				const year = date.getFullYear();
				// 月份从 0 开始，所以要加 1，并且保证是两位数
				const month = String(date.getMonth() + 1).padStart(2, '0');
				// 日期保证是两位数
				const day = String(date.getDate()).padStart(2, '0');
				return `${year}-${month}-${day}`;
			},
			upShip() {
				this.tabCurrent--;
				if (this.tabCurrent == 1) {
					this.btnText = '下一步'
				}
				if (this.tabCurrent == 2) {
					this.btnText = '下一步'
				}
				if (this.tabCurrent == 3) {
					this.btnText = '提交'
				}
				console.log(this.tabCurrent)
			},
			downShip() {
				this.tabCurrent++;
				if (this.tabCurrent == 1) {
					this.btnText = '下一步'
				}
				if (this.tabCurrent == 2) {
					this.btnText = '下一步'
				}
				if (this.tabCurrent == 3) {
					this.btnText = '提交'
				}
				console.log(this.tabCurrent)
			},

			getCalendar: function() {
				this.showCalendar = true;
			},
			calendarChange(e) {
				console.log(e);
				this.model.paijianDay = e.result;
			},
			//生日
			bindDateChange: function(e) {
				this.model.paijianDay = e.target.value;
			},
			initDateTime: function() {
				// 在组件挂载后获取当前日期并赋值给minDate
				const today = new Date();
				this.minDate = this.formatDate(today);

				// 计算当前日期加一个月的日期
				const nextMonth = new Date(today);
				// 设置月份为当前月份加 1
				nextMonth.setMonth(nextMonth.getMonth() + 1);
				this.maxDate = this.formatDate(nextMonth);
				console.log(this.minDate)
				console.log(this.maxDate)
			},
			// 点击actionSheet回调
			actionSheetCallback(index) {
				uni.hideKeyboard();
				this.model.paijianDay = this.actionSheetList[index].text;
				this.model.paijianDayIndex = index;
			},
			userKuaidiList() {
				let _this = this
				let data = {
					page: _this.page,
					limit: _this.limit,
					phone_number:'13850505037',
					store_id:'1'
				}
				_this.status = 'loading'
				_this.$u.api.kuaidiList(data).then(res => {
					console.log(res)
					if (res.status) {
						_this.kuaidiList = [..._this.kuaidiList, ...res.data]
						// 判断数据是否加载完毕
						if (_this.page < res.otherData.totalPages) {
							_this.page++
							_this.status = 'loadmore'
						} else {
							_this.status = 'nomore'
						}

						if (_this.isOpenPoint() == 1 && !_this.isUsePoint) {
							console.log('hjx7')
							console.log(_this.kuaidiList)
							var moneymini = _this.kuaidiList.length
							console.log(moneymini)
							let money = {
								orderMoney: moneymini
							}
							console.log(money)
							this.$u.api.usablePoint(money).then(res => {
								console.log(res)
								if (res.status) {
									_this.userPointNums = res.data.point // 用户总积分
									_this.canUsePoint = res.data.availablePoint // 可以使用的积分
									_this.pointMoney = res.data.pointExchangeMoney // 积分抵扣的总金额

									console.log(_this.userPointNums)
									console.log(_this.canUsePoint)
									console.log(_this.pointMoney)

								}
							})
							console.log('hjx8')
							// 所有价格转换
							_this.cartData.orderPromotionMoneyOld = 0;
							// 购物车详情
							_this.cartData.goodsAmount = _this.kuaidiList.length;
							// 商品详情
							_this.cartData.products = _this.kuaidiList
							// 优惠信息
							_this.cartData.promotions = ''
							// 使用的优惠券信息
							_this.cartData.usedCoupons = ''
							_this.cartData.inputCouponCode = ''
							_this.cartData.optCoupon = ''
							console.log(_this.cartData)
						}
					} else {
						// 接口請求出錯
						_this.$u.toast(res.msg)
						_this.status = 'loadmore'
					}
				})
			},

			// 省市区联动初始化
			showThreePicker() {
				this.show = true;
			},
			onConfirm(e) {
				console.log(e);
				let provinceName = e[0].label;
				let cityName = e[1].label;
				let countyName = e[2].label;
				this.pickerValue = e[0].label + " " + e[1].label + " " + e[2].label
				let data = {
					provinceName: provinceName,
					cityName: cityName,
					countyName: countyName
				}
				//let regionName = [provinceName, cityName, countyName];
				this.$u.api.getAreaId(data).then(res => {
					if (res.status) {
						this.areaId = res.data;
						this.init();
					} else {
						uni.showModal({
							title: '提示',
							content: '地区选择出现问题，请重新选择地区',
							showCancel: false
						});
					}
				});
			},
			//编辑获取收货地址信息
			getShipInfoMoRen() {
				this.$u.api.userDefaultShip().then(res => {
					console.log('hjx4')
					console.log(res)
					if (res.status) {
						let region = res.data.areaName.split(" ");
						this.form.name = res.data.name;
						this.form.mobile = res.data.mobile;
						this.region = region;
						this.areaId = res.data.areaId;
						this.init();
						this.pickerValue = this.region[0] + " " + this.region[1] + " " + this.region[2]
						this.form.address = res.data.address;
						this.form.isDefault = res.data.isDefault;
						if (res.data.isDefault) {
							this.checked = true;
							this.isDefault = 1;
						} else {
							this.checked = false;
							this.isDefault = 2;
						}
					} else {
						this.$u.toast('获取收货地址信息出现问题');
						// this.submitStatus = false;
					}
					this.submitStatus = false;
				});
			},
			init() {
				this.getFullPath(this.areaId, this.province);
				this.pickerIndex = [this.provinceKey, this.cityKey, this.areaKey];
				console.log(this.pickerIndex);
			},
			//倒查城市信息
			getFullPath(id, data) {
				for (var i = 0; i < data.length; i++) {
					if (id == data[i].value) {
						if (!data[i].children) {
							this.areaKey = i;
							return true;
						} else if (data[i].hasOwnProperty("children")) {
							if (data[i].children[0] && !data[i].children[0].children) {
								this.cityKey = i;
								return true;
							} else {
								this.provinceKey = i;
								return true;
							}
						}
					} else {
						if (data[i].hasOwnProperty("children")) {
							if (data[i].children[0] !== undefined) {
								if (data[i].children[0].hasOwnProperty("children")) {
									this.provinceKey = i;
								} else {
									this.cityKey = i;
								}
							}
							if (typeof data[i].children != 'undefined') {
								var res = this.getFullPath(id, data[i].children);
								if (res) {
									return true;
								}
							}
						}
					}
				}
			},
			//存储收货地址
			saveShip() {
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过');

						if (this.checked) {
							this.form.isDefault = 1;
						} else {
							this.form.isDefault = 2;
						}

						this.submitStatus = false;
						if (!this.form.name) {
							this.$u.toast('请输入收货人姓名')
							return false
						} else if (!this.form.mobile) {
							this.$u.toast('请输入收货人手机号')
							return false
						} else if (this.form.mobile.length !== 11) {
							this.$u.toast('收货人手机号格式不正确')
							return false
						} else if (this.areaId <= 0) {
							this.$u.toast('请选择地区信息')
							return false
						} else if (!this.form.address) {
							this.$u.toast('请输入收货地址详细信息')
							return false
						}

						let data = {
							name: this.form.name,
							address: this.form.address,
							mobile: this.form.mobile,
							isDefault: this.form.isDefault,
							areaId: this.areaId
						}
						if (this.id && this.id != 0) {
							//编辑存储
							data.id = this.id
							this.$u.api.editShip(data).then(res => {
								this.submitStatus = false;
								if (res.status) {
									this.$refs.uToast.show({
										title: res.msg,
										type: 'success',
										back: true
									})
								} else {
									this.$u.toast(res.msg);
									// this.submitStatus = false;
								}
							});
						} else {
							//添加
							this.$u.api.saveUserShip(data).then(res => {
								this.submitStatus = false;
								if (res.status) {
									this.$refs.uToast.show({
										title: res.msg,
										type: 'success',
										back: true
									})
								} else {
									this.$u.toast(res.msg);
									// this.submitStatus = false;
								}
							});
						}

					} else {
						console.log('验证失败');
					}
				});


			},


			onTab(id, status) {
				this.tabCurrent = id;
				this.status = status;
				this.page = 1;
				this.loadStatus = 'loading';

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