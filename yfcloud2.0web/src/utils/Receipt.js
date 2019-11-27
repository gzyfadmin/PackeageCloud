import { newGuid } from "@/utils/guid";
import RequestObject from "@/utils/requestObject";
import request from "@/utils/request";
import { trim } from "@/utils/common.js";
// import MaterialSelect from "@/components/MaterialSelect";
export const Receipt = {
	data() {
		return {
			tableData2: [],
			materielData: [], //物料信息=
			materielDataHis: [],//缓存物料信息
			isShowFlag: true,
			totalCountHis: 0,
		};
	},
	activated() {

	},
	destroyed() {
		window.removeEventListener("keydown", this.onKeydown);
	},
	created() {

	},
	mounted() {

	},
	components: {
		// MaterialSelect,
	},
	methods: {
		setMaterielData() {
			this.pageIndex = 1;
			this.pageSize = 25;
			this.materielData = this.materielData2;
		},
		onKeydown(event) {
			if (event.keyCode !== 9) return;
			var data = this.findCheck(event);
			this.setCheck(data);
		},
		findCheck(event) {
			var data = {};
			for (var h = 0; h < this.tableData.length; h++) {
				for (var i in this.tableData[h]) {
					if (typeof this.tableData[h][i] == "object") {
						if (this.tableData[h][i]["isShow"] === true) {
							for (var k = 0; k < this.TabArr.length; k++) {
								if (this.TabArr[k] === i) {
									event.preventDefault();
									var l = h;
									if (k + 1 > this.TabArr.length - 1) {
										var go = 0;
										// console.log("-");
										if (l >= this.tableData.length) {
											l = this.tableData.length;
										} else {
											l = l + 1;
										}
									} else {
										// console.log("+");
										var go = k + 1;
									}
									// console.log(go);
									var set = this.TabArr[go];
									data.index = l;
									data.name = set;
									data.item = this.tableData[l];
									// console.log(data);
									return data;
								}
							}
						}
					}
				}
			}
		},
		setCheck(data) {
			if (data == undefined) return;
			var { index, name, item } = data;
			this.defaultAll();
			for (var k in this.tableData[index]) {
				if (k == name) {
					this.$set(this.tableData[index][name], `isShow`, true);
					this.$nextTick(() => {
						var id = this.tableData[index][name]["id"];
						document.getElementById(id).focus();
						document.getElementById(id).select();
						if (
							document.getElementById(id).getAttribute("readonly") == "readonly"
						) {
							document.getElementById(id).click();
						}
					});
				}
			}
		},
		setTable(num) {
			// 初始化数据
			var listArr = [];
			for (var i = 0; i < num; i++) {
				var list = {};
				for (var j in this.tableData2[0]) {
					if (j == "id") {
						list["id"] = newGuid();
					} else {
						if (typeof this.tableData2[0][j] === "object") {
							list[j] = {};
							for (var k in this.tableData2[0][j]) {
								if (k == "id") {
									list[j][k] = newGuid();
									continue;
								}
								if (typeof this.tableData2[0][j][k] === "boolean") {
									list[j][k] = false;
								} else {
									list[j][k] = "";
								}
							}
						} else {
							list[j] = "";
						}
					}
				}
				listArr.push(list);
			}
			return listArr;
		},
		setStyle() {
			// 设置页面样式
			this.$nextTick(() => {
				var btn = 30; //按钮高度
				var navbar = document.getElementById("navbar_yfkj");
				var nv = navbar.clientHeight || navbar.offsetHeight;
				var b = document.documentElement.clientHeight - nv;
				var elheader = document.getElementById("elheader");
				var elfooter = document.getElementById("elfooter");
				var h = elheader.clientHeight || elheader.offsetHeight;
				var f = elfooter.clientHeight || elfooter.offsetHeight;
				this.tableHeight = b - h - f - 20 - btn;
			});
		},
		findBox(item, name) {
			var IH = document.getElementById(item.id).offsetHeight + 8;
			var IW = document.getElementById(item.id).offsetWidth + 24;
			if (this.$store.getters.sidebar.opened) {
				var ol = 210;
			} else {
				var ol = 54;
			}
			var wl = document.documentElement.clientWidth; // 屏幕宽度
			var wh = document.documentElement.clientHeight; // 屏幕宽度
			var PoL = document.getElementById(item.id).getBoundingClientRect().left; // 弹框left值
			var PoT = document.getElementById(item.id).getBoundingClientRect().top; // 弹框top值
			var PoW = parseInt(this.popoverStyle.width);
			var PoH = parseInt(this.popoverStyle.height);
			if (PoW + PoL > wl) {
				this.popoverStyle.left = PoL - ol - PoW + IW + "px";
			} else {
				this.popoverStyle.left = PoL - ol + "px";
			}
			if (PoT + PoH > wh) {
				this.popoverStyle.top = PoT - PoH - 84 - 9 + "px";
			} else {
				this.popoverStyle.top = PoT - 84 + IH + "px";
			}
			this.popoverState = true;
		},
		listenClick() {
			this.popoverState = false;
			this.defaultAll();
			// this.doDefault(this.doItem);
		},
		selectInput(doItem, name) {
			this.listenClick();
			this.$set(doItem[name], `isShow`, true);
			this.$nextTick(() => {
				var id_ = doItem[name].id;
				document.getElementById(id_).focus();
				document.getElementById(id_).select();
				if (
					document.getElementById(id_).getAttribute("readonly") == "readonly"
				) {
					document.getElementById(id_).click();
				}
			});
		},
		resMaterielData() {
			//重置物料
			this.pageSize = 25,
				this.pageIndex = 1,
				this.materielData = this.materielDataHis;
			this.totalCount = this.totalCountHis;

		},
		getMaterielData(item, type) {
			//获取物料档案信息
			this.materielData = [];
			var queryData = [];
			if (item) {
				//物料代码查询
				if (type == "materialCode") {
					if (trim(item.materialCode.value) == "") {
						this.resMaterielData()
						return
					}
					queryData.push({
						column: "materialCode",
						content: trim(item.materialCode.value),
						condition: 6
					});
				}
				if (type == "materialName") {
					//物料名称查询
					if (trim(item.materialName.value) == "") {
						this.resMaterielData()
						return
					}
					queryData.push({
						column: "materialName",
						content: trim(item.materialName.value),
						condition: 6
					});
				}
			}

			var reqObject;
			reqObject = RequestObject.LonginBookObject(
				true,
				this.pageSize,
				this.pageIndex,
				null,
				queryData
			);
			request({
				url: "/basicset/api/TBMMaterialFile",
				method: "get",
				params: {
					requestObject: JSON.stringify(reqObject)
				}
			}).then(res => {
				this.loading = false;
				if (res.code == -1) {
					this.$confirm(res.info, "错误信息", {
						confirmButtonText: "确定",
						type: "error",
						showCancelButton: false
					});
				} else {
					this.materielData = res.data;
					if (this.materielDataHis.length == 0) {
						this.materielDataHis = res.data;
						this.totalCountHis = res.totalNumber;
					}

					this.totalCount = res.totalNumber;
				}
			});
		},
		getPackageData(item, type) {
			//获取包型信息
			this.materielData = [];
			var queryData = [];
			if (item) {
				//物料代码查询
				if (type == "dicCode") {
					if (trim(item.materialCode.value) == "") {
						this.resMaterielData()
						return
					}
					queryData.push({
						column: "dicCode",
						content: trim(item.materialCode.value),
						condition: 6
					});
				}
				if (type == "dicValue") {
					//物料名称查询
					if (trim(item.materialName.value) == "") {
						this.resMaterielData()
						return
					}
					queryData.push({
						column: "dicValue",
						content: trim(item.materialName.value),
						condition: 6
					});
				}
			}

			var reqObject;
			reqObject = RequestObject.LonginBookObject(
				true,
				this.pageSize,
				this.pageIndex,
				null,
				queryData
			);
			request({
				url: "/basicset/api/TBMPackage",
				method: "get",
				params: {
					requestObject: JSON.stringify(reqObject)
				}
			}).then(res => {
				this.loading = false;
				if (res.code == -1) {
					this.$confirm(res.info, "错误信息", {
						confirmButtonText: "确定",
						type: "error",
						showCancelButton: false
					});
				} else {
					this.materielData = res.data;
					if (this.materielDataHis.length == 0) {
						this.materielDataHis = res.data;
						this.totalCountHis = res.totalNumber;
					}

					this.totalCount = res.totalNumber;
				}
			});
		},
	}
};