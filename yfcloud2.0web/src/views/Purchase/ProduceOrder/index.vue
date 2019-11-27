<template>
  <el-container
    id="Purchasingorder"
    v-loading="fullscreenLoading"
    element-loading-background="rgba(150, 150, 150, 0.6)"
    element-loading-spinner="el-icon-loading"
    element-loading-text="正在加载"
    @click.native="listenClick"
  >
    <div
      id="btnHeader"
      class="btnHeader"
    >
      <div
        v-if="btnAip.add"
        class="btnHeaderBox"
      >
        <el-button
          v-if="btnAip.add.buttonCaption&&addControl"
          type="default"
          @click="addPutInStorage(1)"
        >{{ btnAip.add.buttonCaption }}</el-button>

        <el-button
          type="default"
          @click="resetData()"
        >重置</el-button>
        <!-- :disabled="addControl" -->
        <el-button
          v-if="btnAip.modify.buttonCaption"
          type="default"
          @click="addPutInStorage(2)"
        >{{ btnAip.modify.buttonCaption }}</el-button>
        <!-- :disabled="addControl" -->
        <el-button
          v-if="btnAip.delete.buttonCaption"
          type="default"
          @click="addPutInStorage(3)"
        >{{ btnAip.delete.buttonCaption }}</el-button>
        <!-- 查询 -->
        <el-button
          v-if="btnAip.query.buttonCaption"
          type="default"
          @click="handelAddClick"
        >{{ btnAip.query.buttonCaption }}</el-button>
        <!-- :disabled="addControl" -->
        <el-dropdown
          v-if="btnAip.review.buttonCaption"
          split-button
          type="default"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
        >
          {{ btnAip.review.buttonCaption }}
          <el-dropdown-menu
            slot="dropdown"
            style="border-bottom: 0px solid #ccc;"
          >
            <el-dropdown-item command="1">未通过</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </div>
    </div>
    <!-- <div style="padding:5px 0px 0px 20px;height: 37px;">
      <el-button-group v-if="btnAip.add">
        <el-button
          type="default"
          @click="addPutInStorage(1)"
          v-if="btnAip.add.buttonCaption&&addControl"
        >{{ btnAip.add.buttonCaption }}</el-button>

        <el-button type="default" @click="resetData()">重置</el-button>
        <el-button
          type="default"
          @click="addPutInStorage(2)"
          v-if="btnAip.modify.buttonCaption"
        >{{ btnAip.modify.buttonCaption }}</el-button>
        <el-button
          type="default"
          @click="addPutInStorage(3)"
          v-if="btnAip.delete.buttonCaption"
        >{{ btnAip.delete.buttonCaption }}</el-button>
        <el-button
          type="default"
          @click="handelAddClick"
          v-if="btnAip.query.buttonCaption"
        >{{ btnAip.query.buttonCaption }}</el-button>
        <el-dropdown
          split-button
          type="default"
          @click="doOtherWhAudit(2)"
          @command="doOtherWhAudit"
          v-if="btnAip.review.buttonCaption"
        >
          {{ btnAip.review.buttonCaption }}
          <el-dropdown-menu slot="dropdown" style="border-bottom: 0px solid #ccc;">
            <el-dropdown-item command="1">未通过</el-dropdown-item>
          </el-dropdown-menu>
        </el-dropdown>
      </el-button-group>
    </div>-->

    <el-header
      id="elheader"
      class="header headerBd"
      height="auto"
    >
      <!-- <div class="middleWidth"> -->
      <div>
        <el-form
          ref="dtData"
          :model="dtData"
          label-width="86px"
          class="FormInput"
          inline
          label-position="right"
          :rules="dtDataRules"
        >
          <el-form-item
            class="formItem disableType"
            label="源单号："
            prop="order"
          >
            <el-input
              v-model="dtData.order"
              readonly
              style="width:200px;"
              placeholder="源单号"
              @focus="orderVisibleBtn"
            >
              <i
                slot="suffix"
                class="el-input__icon el-icon-search"
                @click="orderVisibleBtn"
              />
            </el-input>
          </el-form-item>
          <!-- <el-form-item class="formItem disableType" label="供应商：" prop="supplierId">
            <el-select
              style="width:200px;"
              :disabled="selectRow.auditStatus === 2"
              v-model="dtData.supplierId"
              @change="connectchange"
              placeholder="请选择"
            >
              <el-option
                v-for="item in connectionData"
                :key="item.id"
                :label="item.supplierName"
                :value="item.id"
              ></el-option>
            </el-select>
          </el-form-item> -->
          <el-form-item
            class="formItem disableType"
            label="采购员："
            prop="buyerId"
          >
            <el-select
              v-model="dtData.buyerId"
              style="width:200px;"
              :disabled="selectRow.auditStatus === 2"
              placeholder="请选择"
            >
              <el-option
                v-for="item in realNameOptions"
                :key="item.id"
                :label="item.realName"
                :value="item.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item
            class="formItem disableType"
            label="采购方式："
            label-width="87px"
            prop="orderTypeId"
          >
            <el-select
              v-model="dtData.orderTypeId"
              style="width:200px;"
              :disabled="selectRow.auditStatus === 2"
              placeholder="请选择"
            >
              <el-option
                v-for="item in orderTypeoption"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item
            class="formItem"
            label="日期："
            prop="orderDate"
          >
            <el-date-picker
              v-model="dtData.orderDate"
              :readonly="selectRow.auditStatus === 2"
              type="date"
              placeholder="选择日期"
              style="width:200px"
            />
          </el-form-item>

          <el-form-item
            class="formItem disableType"
            label="结算方式："
            prop="settlementTypeId"
          >
            <el-select
              v-model="dtData.settlementTypeId"
              style="width:200px;"
              :disabled="selectRow.auditStatus === 2"
              placeholder="请选择"
            >
              <el-option
                v-for="item in settlementTypeoption"
                :key="item.id"
                :label="item.dicValue"
                :value="item.id"
              />
            </el-select>
          </el-form-item>

          <el-form-item
            class="formItem"
            label="币种："
            label-width="86px"
          >
            <div class="bbfe6">人民币</div>
          </el-form-item>

          <el-form-item
            class="formItem"
            label="编号："
            prop="orderNo"
            label-width="86px"
          >
            <div class="bbfe6">{{ dtData.orderNo }}</div>
          </el-form-item>
          <el-form-item
            class="formItem"
            label="状态："
            label-width="86px"
          >
            <div class="bbfe6">
              <span v-if="dtData.auditStatus==-1" />
              <span v-if="dtData.auditStatus==0">待审核</span>
              <span v-if="dtData.auditStatus==1">未通过</span>
              <span v-if="dtData.auditStatus==2">通过</span>
              <span v-if="dtData.auditStatus==3">撤销审核</span>
            </div>
          </el-form-item>

          <!-- </div> -->
        </el-form>
      </div>
    </el-header>
    <el-main id="elmain">
      <!-- //左边table选择框 -->

      <!-- <div class="findBox" :style="popoverStyle" v-show="popoverState" v-on:click.stop="1==1">
        <el-table :data="materielData" :height="250" @row-click="rowClick">
          <el-table-column prop="materialCode" label="物料代码" width="180" />
          <el-table-column prop="materialName" label="物料名称" />
          <el-table-column prop="spec" label="规格型号" />
          <el-table-column prop="colorName" label="颜色" />
          <el-table-column prop="baseUnitName" label="基本计量单位" />
        </el-table>
        <Pagination
          :pageIndex="pageIndex"
          :totalCount="totalCount"
          :pageSize="pageSize"
          @pagination="pagination"
        />
      </div>-->

      <div
        v-show="menuState"
        class="findBox"
        :style="menuStyle"
      >
        <ul class="menuUl">
          <!-- <li @click="doMenuAdd">增加行</li> -->
          <li @click="doMenuDel">删除行</li>
        </ul>
      </div>
      <div>
        <el-table
          v-if="showtab"
          id="tableData"
          ref="table"
          :height="tableHeight"
          :data="tableData"
          style="width: 99.9%"
          row-key="id"
          border
          :show-summary="true"
          :summary-method="getSummaries"
        >
          <el-table-column
            label="序号"
            width="70"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>序号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <div class="textAlign">
                <span @click.stop="menuBar($event,scope.$index)">
                  {{ scope.$index+1 }}
                  <i class="el-icon-setting" />
                </span>
              </div>
            </template>
          </el-table-column>

          <el-table-column
            prop="materialCode"
            label="物料代码"
          >
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料代码</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- @click="chengenum($event,scope.row,'materialCode',1)" -->
              <!-- <div :class="{validStyle:scope.row.materialCode.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.materialCode.isShow"
                >{{scope.row.materialCode.value}}</div>
                <el-input
                  v-show="scope.row.materialCode.isShow"
                  v-model="scope.row.materialCode.value"
                  @input="getMaterielDataIndex(scope.row)"
                  @focus="chengenum($event,scope.row,'materialCode',1)"
                  :id="scope.row.id+'___materialCode'"
                  size="mini"
                  @change="changeBlur(scope.row,scope.row.materialCode.value)"
                ></el-input>
              </div> -->
              <EditInput
                v-model="scope.row.materialCode"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="materialName"
            label="物料名称"
          >
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>物料名称</span>
              </span>
            </template>
            <!-- @click="chengenum($event,scope.row,'materialName',1)" -->
            <template slot-scope="scope">
              <!-- <div :class="{validStyle:scope.row.materialName.valid}">
                <div
                  class="tableTd"
                  v-show="!scope.row.materialName.isShow"
                >{{scope.row.materialName.value}}</div>
                <el-input
                  v-show="scope.row.materialName.isShow"
                  v-model="scope.row.materialName.value"
                  @input="getMaterielDataIndex(scope.row)"
                  :id="scope.row.id+'___materialName'"
                  size="mini"
                ></el-input>
              </div> -->
              <EditInput
                v-model="scope.row.materialName"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>
          <el-table-column
            prop="colorName"
            label="颜色"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>颜色</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'colorName')"
                :class="{validStyle:scope.row.colorName.valid}"
              >
                <div class="tableTd">{{scope.row.colorName.value}}</div>
              </div> -->
              <EditInput
                v-model="scope.row.colorName"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>
          <el-table-column
            prop="spec"
            width="110"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>规格型号</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{scope.row.spec.value}}</div> -->
              <EditInput
                v-model="scope.row.spec"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="baseUnitName"
            width="100"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位名称</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div class="tableTd">{{scope.row.baseUnitName.value}}</div> -->
              <EditInput
                v-model="scope.row.baseUnitName"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="actualUnitNum"
            label="基本单位数量"
            width="120"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>基本单位数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                class="tableTd"
                v-if="scope.row.actualUnitNum.value>0"
              >{{scope.row.actualUnitNum.value}}</div> -->
              <EditInput
                v-model="scope.row.actualUnitNum"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="tranPurchaseNum"
            label="可采购数量"
            width="120"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>可采购数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                class="tableTd"
              >{{scope.row.tranPurchaseNum.value}}</div> -->
              <EditInput
                v-model="scope.row.tranPurchaseNum"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="actualNum"
            label="数量"
          >
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>数量</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'actualNum')"
                :class="{validStyle:scope.row.actualNum.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.actualNum.isShow"
                >{{scope.row.actualNum.value}}</div>
                <el-input
                  v-show="scope.row.actualNum.isShow"
                  v-model="scope.row.actualNum.value"
                  :id="scope.row.id+'___actualNum'"
                  size="mini"
                ></el-input>
                </div> -->
              <!-- @input="actualNumBlur(scope.row,scope.row.actualNum.value)" -->
              <EditInput
                v-model="scope.row.actualNum"
                :class="{redCol:scope.row.actualNum.value<=0,greCol:scope.row.actualNum.value>0}"
                :type="2"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

           <!-- 【单位】该单位的取值来源是物料档案中配置的库存计量单位。 -->
          <el-table-column prop="purchaseUnitName">
            <template slot="header">
              <span class="tableHeader">
                <span>单位</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                class="tableTd"
                v-if="scope.row.purchaseUnitName.value"
              >{{scope.row.purchaseUnitName.value}}</div>
              <div
                class="tableTd"
                v-if="!scope.row.purchaseUnitName.value"
              >{{scope.row.baseUnitName.value}}</div> -->
              <EditInput
                v-model="scope.row.purchaseUnitName"
                :item-list="scope.row"
                :is-show-flag="false"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="unitPrice"
            label="单价"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>单价</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'unitPrice')"
                :class="{validStyle:scope.row.unitPrice.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.unitPrice.isShow"
                >{{scope.row.unitPrice.value}}</div>
                <el-input
                  v-show="scope.row.unitPrice.isShow"
                  v-model="scope.row.unitPrice.value"
                  :id="scope.row.id+'___unitPrice'"
                  size="mini"
                  @input="unitPriceBlur(scope.row,scope.row.unitPrice.value)"
                  @blur="unitPriceBlur2(scope.row)"
                ></el-input>
              </div> -->
              <EditInput
                v-model="scope.row.unitPrice"
                :type="3"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="Totalamount"
            label="金额"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>金额</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'Totalamount')"
                :class="{validStyle:scope.row.Totalamount.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.Totalamount.isShow"
                >{{scope.row.Totalamount.value}}</div>
                <el-input
                  v-show="scope.row.Totalamount.isShow"
                  v-model="scope.row.Totalamount.value"
                  :id="scope.row.id+'___Totalamount'"
                  size="mini"
                  @input="amountBlur(scope.row,scope.row.Totalamount.value)"
                  @blur="amountBlur2(scope.row)"
                ></el-input>
              </div> -->
              <EditInput
                v-model="scope.row.Totalamount"
                :type="4"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
                @blurCheck="blurCheck"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="validityPeriod"
            label="交货日期"
            width="143"
          >
            <template slot="header">
              <span class="tableHeader">
                <span class="start">*</span>
                <span>交货日期</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'validityPeriod')"
                :class="{validStyle:scope.row.validityPeriod.valid}"
              >
                <div
                  class="tableTd"
                  v-show="!scope.row.validityPeriod.isShow"
                >{{scope.row.validityPeriod.value | formatTDate}}</div>
                <el-date-picker
                  v-show="scope.row.validityPeriod.isShow"
                  :id="scope.row.id+'___validityPeriod'"
                  v-model="scope.row.validityPeriod.value"
                  style="width:140px;"
                  type="date"
                  placeholder="选择日期"
                  @keydown.native="onKeydown($event)"
                />
              </div> -->

              <EditInput
                v-model="scope.row.validityPeriod"
                type="date"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @onKeydown="onKeydown"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="supplierId"
            label="供应商"
            width="100"
          >
            <template slot="header">
              <span class="tableHeader">
                <span>供应商</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <el-select
              style="width:200px;"
               clearable
              v-model="scope.row.supplierId.key"
              @change="connectchange"
              placeholder="请选择"
            >
              <el-option
                v-for="item in connectionData"
                :key="item.id"
                :label="item.supplierName"
                :value="item.id"
              ></el-option>
            </el-select> -->
              <EditSelect
                v-model="scope.row.supplierId"
                :data="connectionData"
                label="supplierName"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
              />
            </template>
          </el-table-column>

          <el-table-column
            prop="remark"
            label="备注"
            width="240"
          >
            <template slot="header">
              <span class="tableHeader">
                <span class="start" />
                <span>备注</span>
              </span>
            </template>
            <template slot-scope="scope">
              <!-- <div
                @click="chengenum($event,scope.row,'remark')"
                :class="{validStyle:scope.row.remark.valid}"
              >
                <div class="tableTd" v-show="!scope.row.remark.isShow">
                  <el-popover
                    class="item"
                    trigger="hover"
                    placement="top-start"
                    :content="scope.row.remark.value"
                    v-if="scope.row.remark.value&&scope.row.remark.value.length>=15"
                  >
                    <div class="ellipsis" slot="reference">{{scope.row.remark.value}}</div>
                  </el-popover>
                  <div
                    class="ellipsis"
                    v-if="scope.row.remark.value&&scope.row.remark.value.length<15"
                  >{{scope.row.remark.value}}</div>
                </div>
                <el-input
                  v-show="scope.row.remark.isShow"
                  v-model="scope.row.remark.value"
                  :id="scope.row.id+'___remark'"
                  size="mini"
                ></el-input>
              </div> -->

              <EditInput
                v-model="scope.row.remark"
                type="remark"
                :item-list="scope.row"
                :is-show-flag="isShowFlag"
                @clickEvent="clickEvent"
                @clickEventAfter="clickEventAfter"
              />
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-main>
    <div
      id="elfooter"
      class="fromCenter"
    >
      <el-form
        ref="dtData2"
        label-width="90px"
        class="FormInput"
        :rules="dtDataRules2"
        :model="dtData"
        inline
      >
        <!-- style="width:1368px" -->
        <!-- <el-form-item label="联系人：" v-if="dtData.auditStatus!=2" prop="contactName">
          <el-input style="width:200px;" v-model="dtData.contactName"></el-input>
        </el-form-item>

        <el-form-item label="联系人：" v-if="dtData.auditStatus==2">
          <div style="width:200px;" class="falseIp">{{ dtData.contactName }}</div>
        </el-form-item>

        <el-form-item label="联系人电话：" v-if="dtData.auditStatus!=2" prop="contactNumber">
          <el-input style="width:200px;" v-model="dtData.contactNumber"></el-input>
        </el-form-item>

        <el-form-item label="联系人电话：" v-if="dtData.auditStatus==2">
          <div style="width:200px;" class="falseIp">{{ dtData.contactNumber }}</div>
        </el-form-item> -->

        <el-form-item label="审核人：">
          <div
            style="width:200px;"
            class="falseIp"
          >{{ dtData.auditName }}</div>
        </el-form-item>

        <el-form-item
          v-if="addControl"
          label="制单人："
          prop="operatorId"
        >
          <div
            v-if="addControl"
            class="falseIp"
            style="width:200px;"
          >{{ dtData.operatorName }}</div>
        </el-form-item>
        <el-form-item
          v-if="!addControl"
          label="制单人："
        >
          <div
            v-if="!addControl"
            class="falseIp"
            style="width:200px;"
          >{{ dtData.operatorName }}</div>
        </el-form-item>

        <el-form-item label="审核结果：">
          <div
            style="width:200px;"
            class="falseIp"
          >
            <span v-if="dtData.auditStatus==-1" />
            <span v-if="dtData.auditStatus==0">待审核</span>
            <span v-if="dtData.auditStatus==1">未通过</span>
            <span v-if="dtData.auditStatus==2">通过</span>
            <span v-if="dtData.auditStatus==3">撤销审核</span>
          </div>
        </el-form-item>

        <el-form-item label="审核时间：">
          <div
            style="width:200px;"
            class="falseIp"
          >{{ dtData.auditTime }}</div>
        </el-form-item>
      </el-form>
    </div>
    <InboundOrder
      ref="InboundOrder"
      @OnBtnSaveSubmit="OnBtnSaveSubmit"
    />
    <!-- 选择单据号弹窗 -->
    <el-dialog
      id="orderBox"
      title="采购订单"
      :visible.sync="orderVisible"
      width="1000px"
      :close-on-click-modal="false"
      :center="true"
    >
      <div
        v-loading="fullscreenLoading"
        element-loading-spinner="el-icon-loading"
        element-loading-background="rgba(1, 1, 1, 0)"
        @click.native="listenClick"
      >
        <!-- <el-header
          id="elheader"
          class="header"
          height="auto"
        >

        </el-header>-->
        <el-header
          id="elheader"
          class="header"
          height="auto"
        >
          <el-form
            ref="searchData"
            label-width="90px"
            class="FormInput"
            inline
            label-position="left"
          >
            <el-form-item label="生产订单号：">
              <el-input
                v-model="searchData.orderNo"
                placeholder="生产订单号"
                clearable
                style="width:320px"
                @keyup.enter.native="seleBtn()"
                @clear="seleBtn()"
              >
                <i
                  slot="suffix"
                  class="el-input__icon el-icon-search"
                  @click="seleBtn()"
                />
              </el-input>
            </el-form-item>
            <el-form-item label="订单日期：">
              <el-date-picker
                v-model="searchData.orderDate"
                style="width:360px"
                type="daterange"
                align="left"
                unlink-panels
                range-separator="至"
                start-placeholder="开始日期"
                end-placeholder="结束日期"
                clearable
                @change="seleBtn()"
                @clear="seleBtn()"
              />
            </el-form-item>
            <el-form-item label="审核状态：">
              <el-radio-group
                v-model="searchData.auditStatus"
                @change="seleBtn()"
              >
                <el-radio-button label="-1">全部</el-radio-button>
                <el-radio-button label="0">待审核</el-radio-button>
                <el-radio-button label="1">未通过</el-radio-button>
                <el-radio-button label="2">通过</el-radio-button>
                <!-- <el-radio-button label="3">撤销审核</el-radio-button> -->
              </el-radio-group>
            </el-form-item>
          </el-form>
        </el-header>
        <div style="height:400px;overflow:auto;">
          <el-table
            :data="orderData"
            :height="380"
            :highlight-current-row="true"
            @row-click="rowOrderClick"
            @row-dblclick="dblOrderClick"
            @sort-change="handelSortChange"
          >
            <!-- <el-table-column type="index" label="序号" /> -->
            <el-table-column
              label="序号"
              width="50"
            >
              <template slot-scope="scope">
                <div>{{ (pageIndex-1)*pageSize+(scope.$index+1) }}</div>
              </template>
            </el-table-column>
            <!-- <el-table-column prop="warehousingType" label="来源部门ID" /> -->
            <!-- <el-table-column
              prop="sourceId"
              label="源单号"
            />-->
            <el-table-column
              prop="sourceNo"
              label="生产订单号"
              width="150"
              sortable="custom"
            />
            <el-table-column
              prop="purchaseNo"
              label="采购申请单号"
              width="150"
              sortable="custom"
            />
            <el-table-column
              prop="applyDate"
              label="订单日期"
              width="150"
              sortable="custom"
            >
              <template slot-scope="scope">{{ scope.row.applyDate|formatTDate }}</template>
            </el-table-column>
            <el-table-column
              prop="operatorName"
              label="制单人"
            />
            <el-table-column
              prop="operatorTime"
              label="制单时间"
              width="150"
            >
              <template slot-scope="scope">{{ scope.row.operatorTime|formatTDate }}</template>
            </el-table-column>

            <el-table-column
              prop="auditStatus"
              label="审核状态"
            >
              <template slot-scope="scope">
                <div v-if="scope.row.auditStatus==0">待审核</div>
                <div v-if="scope.row.auditStatus==1">未通过</div>
                <div v-if="scope.row.auditStatus==2">通过</div>
                <div v-if="scope.row.auditStatus==3">撤销审核</div>
              </template>
            </el-table-column>
            <el-table-column
              prop="auditName"
              label="审核人"
              width="150"
            />
            <el-table-column
              prop="auditTime"
              label="审核时间"
              width="150"
            >
              <template slot-scope="scope">{{ scope.row.auditTime|formatTDate }}</template>
            </el-table-column>
            <el-table-column
              v-if="(btnAip.stoppurchase&&btnAip.stoppurchase.buttonCaption)"
              label="操作"
              width="70"
              fixed="right"
            >
              <template slot-scope="scope">
                <el-tooltip
                  v-if="btnAip.stoppurchase&&btnAip.stoppurchase.buttonCaption"
                  class="item"
                  effect="light"
                  :content="btnAip.stoppurchase.buttonCaption"
                  placement="top-start"
                  :open-delay="200"
                >
                  <el-button
                    type="danger"
                    icon="el-icon-circle-close"
                    circle
                    :disabled="scope.row.auditStatus!=2"
                    @click="StopReceipt(scope.row)"
                  />
                </el-tooltip>
              </template>
            </el-table-column>
          </el-table>
        </div>
        <Pagination
          :page-index="pageOrderIndex"
          :total-count="totalOrderCount"
          :page-size="pageOrderSize"
          @pagination="paginationOrder"
        />
        <div
          slot="footer"
          class="dialog-footer"
        >
          <el-button @click="orderVisible=false">关闭</el-button>
          <el-button
            type="primary"
            @click="OnBtnOrder"
          >选择</el-button>
        </div>
      </div>
    </el-dialog>
  </el-container>
</template>
<script>
import height from '@/utils/height'
import RequestObject from '@/utils/requestObject'
import request from '@/utils/request'
// import {
//   formatDate,
//   keepTwoDecimalFull,
//   accMul,
//   trim
// } from "@/utils/common.js";
import { newGuid } from '@/utils/guid'
import InboundOrder from './components/InboundOrder'

import Pagination from '@/components/Pagination'
// import { promises } from 'dns';
import { setTimeout } from 'timers'
import { getBtnCtr } from '@/utils/BtnControl'
import { closest } from '@/utils/Dom'
import Storage from '@/utils/storage'
import { getStyle } from '@/utils/Dom.js'
import EditInput from '@/components/EditTable/EditInput'
import EditSelect from '@/components/EditTable/EditSelect'
import BigNumber from 'bignumber.js'
import {
  formatDate,
  keepTwoDecimalFull,
  getFloat,
  accMul,
  isRealNum,
  formatMoney,
  trim
} from '@/utils/common.js'

export default {
  name: 'viewsPurchaseProduceOrderindexvue',
  filters: {
    formatTDate: value => {
      if (value == '') {
        return ''
      }
      return formatDate(value)
    }
  },
  components: {
    Pagination,
    InboundOrder,
    EditInput,
    EditSelect
  },
  data() {
    return {
      isShowFlag: true,
      sortColumn: '',
      sortOrder: '',
      getId: '',
      sourceNo: '',
      orderVisible: false,
      isOrder: true,
      Loading: false,
      orderData: [],
      pageOrderSize: 25, // 分页显示记录条数
      totalOrderCount: 0, // 总记录数
      pageOrderIndex: 1, // 页码
      showtab: true,
      connectionData: [],
      orderTypeoption: [],
      // 银行转账、现金结算、微信、支付宝
      settlementTypeoption: [],
      materialItem: {},
      remarkRel: true,
      actualRel: true,
      globalid: 0,
      disappear: true,
      nowrow: [],
      roleList: [],
      warehousepeople: [],
      operatorlist: [],
      searchData: {
        orderNo: '',
        orderDate: '',
        auditStatus: -1
      },
      popoverStyle: {
        left: '500px',
        top: '400px',
        width: '800px',
        height: '300px'
      },
      menuStyle: {
        left: '500px',
        top: '400px',
        width: '80px',
        height: 'auto',
        paddiing: '4px'
      },
      menuState: false,
      delNum: -1,
      fullscreenLoading: false,
      dtData: {
        sourceId: '',
        orderNo: '',
        remark: '',
        auditId: null,
        auditName: null,
        auditStatus: 0,
        auditTime: '',
        contactName: '',
        contactNumber: '',
        currency: '',
        order: null,
        supplierName: null,
        id: null,
        operatorId: null,
        operatorName: '',
        orderDate: new Date(),
        orderTypeId: null,
        orderTypeName: null,
        buyerId: null,
        buyerName: null,
        settlementTypeName: null,
        settlementTypeId: null,
        transferStatus: false,
        supplierId: null
      },
      dtDataRules: {
        order: [
          {
            required: true,
            message: '请选择源单号',
            trigger: ['change', 'blur']
          }
        ],
        orderOrder: [{ required: true, message: '获取编号失败' }],
        orderNo:[{ required: true, message: "获取编号失败" }],
        buyerId: [
          {
            required: true,
            message: '请选择采购员',
            trigger: ['change', 'blur']
          }
        ],
        orderTypeId: [
          {
            required: true,
            message: '请选择采购方式',
            trigger: ['change', 'blur']
          }
        ],
        orderDate: [
          { required: true, message: '请选择日期', trigger: ['change', 'blur'] }
        ]
      },
      dtDataRules2: {
        contactNumber: [
          { required: false, message: '请输入合法的电话号码', trigger: 'blur' }
          // {
          //   pattern: /^(\(\d{3,4}\)|\d{3,4}-|\s)?\d{7,14}$/,
          //   message: "请输入正确的电话格式",
          //   trigger: ["blur", "change"]
          // }
        ],
        // ^1[3|4|5|6|7|8][0-9]{9}$|

        contactName: [
          {
            required: false,
            min: 0,
            max: 20,
            message: '客户名不合法',
            trigger: 'blur'
          },
          {
            pattern: /^[\u4E00-\u9FA5A-Za-z0-9_]+$/,
            message: '客户名不合法',
            trigger: ['blur', 'change']
          }
        ]
      },
      tableHeight: 500,
      PostDataEdit: {},
      cloneTable: [],
      selectRow: [],
      popoverState: false,
      inputState: false,
      doItem: {},
      doItemName: '',
      materielData: [], // 物料信息
      TabArr: [
        // 'materialCode',
        // 'materialName',
        'actualNum',
        'unitPrice',
        'Totalamount',
        'validityPeriod',
        'supplierId',
        'remark'
      ],
      tableData2: [
        {
          id: newGuid(),
          editState: false,
          rowIndex: 0,
          materialCode: {
            value: '',
            key: '',
            isShow: false,
            valid: false
          },
          idandmainid: {
            id: 0,
            mainId: 0,
            isShow: false,
            valid: false
          },
          materialName: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          colorName: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          spec: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },

          baseUnitName: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },

          accountNum: {
            id: '',
            // 数量
            value: '',
            isShow: false,
            valid: false
          },
          actualNum: {
            id: '',
            // 数量
            value: '',
            isShow: false,
            valid: false
          },

          actualUnitNum: {
            id: '',
            // 基本单位数量
            value: '',
            isShow: false,
            valid: false
          },

          // 应采购数量
          tranPurchaseNum: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },

          unitPrice: {
            // 单价
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          Totalamount: {
            // 金额
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          profitUnitNum: {
            // 基本单位单价
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          deficitUnitNum: {
            // 基本单位金额
            id: '',
            value: '',
            isShow: false,
            valid: false
          },

          warehouseUnitId: {
            // 基本单位换算率id
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          purchaseRate: {
            // 基本单位换算率
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          // 供应商
          supplierId: {
            id: '',
            value: '',
            key: '',
            isShow: false,
            valid: false
          },
          remark: {
            // 备注
            id: '',
            value: '',
            isShow: false,
            valid: false
          },

          batchNumber: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },

          purchaseUnitName: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },

          baseUnitNumber: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          unitPrice: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          amount: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          },
          validityPeriod: {
            id: '',
            value: '',
            isShow: false,
            valid: false
          }
        }
      ], // table数据模型
      tableData: [],
      dialogVisible: false, // 编辑窗口是否显示
      pageSize: 25, // 分页显示记录条数
      totalCount: 0, // 总记录数
      pageIndex: 1, // 页码
      btnAip: '', // 按钮权限
      OtherData: {},
      realNameOptions: [], // 收货员列表
      addControl: true // 是否显示新增按钮
    }
  },
  watch: {
    // selectRow(val) {
    //   if (val.id) {
    //     this.addControl = false;
    //   } else {
    //     this.addControl = true;
    //   }
    // }
  },
  deactivated() {
    this.$refs.InboundOrder.openNo()
    window.removeEventListener('keydown', this.onKeydown)
  },

  beforeDestroy() {
    window.removeEventListener('keydown', this.onKeydown)
  },

  async activated() {
    this.setStyle() // 设置样式
    window.addEventListener('keydown', this.onKeydown)
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    )

    if (this.$route.params.type == 1 || this.$route.params.type == 2) {
      this.OtherData = this.$route.params
      this.OnBtnSaveSubmit(this.OtherData.item)
    }
    if (this.$route.params.type == 3) {
      this.resetData()
    }
  },

  async activated() {
    this.setStyle() // 设置样式
    this.showtab = false
    setTimeout(() => {
      this.showtab = true
    }, 10)
    if (this.$route.params.type == 3) {
      this.resetData()
    }
  },
  mounted() {
    this.btnAip = getBtnCtr(
      this.$route.path,
      this.$store.getters.userPermission
    )
    this.fullscreenLoading = true
    window.addEventListener('keydown', this.onKeydown)
    this.setStyle() // 设置样式
    if (this.$route.params.type == 1 || this.$route.params.type == 2) {
      this.OtherData = this.$route.params
      this.OnBtnSaveSubmit(this.OtherData.item)
    } else if (this.$route.params.type == 3) {
      var code = Storage.LStorage.get('ProduceOrder')
      if (!code) {
        this.getCode()
      } else {
        this.dtData.orderNo = code
      }
      this.dtData.operatorName = this.$store.state.user.name
      this.dtData.operatorId = 0
      this.resetData()
    } else {
      var code = Storage.LStorage.get('ProduceOrder')
      if (!code) {
        this.getCode()
      } else {
        this.dtData.orderNo = code
      }
      this.dtData.operatorName = this.$store.state.user.name
      this.dtData.operatorId = 0
    }

    this.tableData = this.setTable(this.tableLen) // 添加30行
    this.getMaterielData() // 物料数据
    this.getUserCompany() // 收货员列表

    this.getorderTypeoption() // 收货员列表

    this.getsettlementTypeoption() // 收货员列表

    this.fullscreenLoading = false
    this.getCustomer()
  },

  methods: {
    blurCheck(itemList, item, type, state) {
      // td框的类型（可用于数据校验等）
      switch (type) { // （type）对应组件type
        case 1: // 物料代码
          if (state == 2) {
            // 按下Enter请求数据
            this.getMaterielData(itemList,this.pageIndex=1)
          }
          break
        case 2: // 数量
          this.PaidInBlur(itemList) // 验证长、宽、件数、损耗
          break
        case 3: // 单价
          this.unitPriceBlur(itemList) // 封度（宽幅）
          break
        case 4: // 金额
          this.amountBlur(itemList) // 封度（宽幅）
          break
        default:
          null
      }
    },
    clickEventAfter(itemList, item, type) {
      // 显示input框之后（创建选择物料代码框）
      this.doItem = itemList
      if (type === 1) {
        this.findBox(item)
        this.getMaterielData(itemList)
      }
    },
    clickEvent() {
      this.defaultAll()
    },

    handelSortChange(currSort) {
      this.sortColumn = currSort.prop
      this.sortOrder = currSort.order == 'ascending' ? 'asc' : 'desc'
      this.orderVisibleBtn()
    },
    // 弹窗
    orderVisibleBtn() {
      if (this.selectRow.auditStatus === 2) {
        return
      }
      if (!this.addControl) {
        return
      }
      this.getId = ''
      this.searchData.orderNo = ''
      this.searchData.orderDate = ''
      this.searchData.auditStatus = -1

      this.orderVisible = true
      var list = [{ column: 'transferFlag', content: 1, condition: 0 }]
      var orderConditions = [
        {
          column: 'applyDate',
          condition: 'desc'
        }
      ]

      var rqs = RequestObject.CreateGetObject(
        true,
        this.pageOrderSize,
        this.pageOrderIndex,
        list,
        orderConditions
      )
      this.Loading = true
      this.orderData = []
      request({
        url: '/manufacturing/api/TMMPurchaseApplyMain/GetMainList',
        method: 'GET',
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.orderData = res.data
          this.totalOrderCount = res.totalNumber
          this.Loading = false
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
          this.Loading = false
        }
      })
    },
    seleBtn() {
      if (this.selectRow.auditStatus === 2) {
        return
      }
      if (!this.addControl) {
        return
      }
      this.getId = ''
      this.orderVisible = true
      var list = [{ column: 'transferFlag', content: 1, condition: 0 }]
      var orderConditions = []
      if (trim(this.searchData.orderNo) !== '') {
        list.push({
          column: 'sourceNo',
          content: trim(this.searchData.orderNo),
          condition: 6
        })
      }
      if (this.searchData.orderDate) {
        if (this.searchData.orderDate.length > 0) {
          var startDate = formatDate(this.searchData.orderDate[0])
          var endDate = formatDate(this.searchData.orderDate[1])
          list.push({
            column: 'applyDate',
            content: startDate + ',' + endDate,
            condition: 10
          })
        }
      }
      if (this.searchData.auditStatus != -1) {
        list.push({
          column: 'auditStatus',
          content: this.searchData.auditStatus,
          condition: 6
        })
      }
      if (this.sortColumn && this.sortColumn.length > 0) {
        orderConditions.push({
          column: this.sortColumn,
          condition: this.sortOrder
        })
      } else {
        orderConditions = [
          {
            column: 'applyDate',
            condition: 'desc'
          }
        ]
      }
      var rqs = RequestObject.CreateGetObject(
        true,
        this.pageOrderSize,
        this.pageOrderIndex,
        list,
        orderConditions
      )
      this.Loading = true
      this.orderData = []
      request({
        url: '/manufacturing/api/TMMPurchaseApplyMain/GetMainList',
        method: 'GET',
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.orderData = res.data
          this.totalOrderCount = res.totalNumber
          this.Loading = false
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
          this.Loading = false
        }
      })
    },
    // 获取源单号信息
    getOrder(id) {
      request({
        url: '/manufacturing/api/TMMPurchaseApplyMain/GetToPurchaseList',
        method: 'GET',
        params: { RequestObject: id }
      }).then(res => {
        if (res.code === 0) {
          // this.orderData = res.data;
          // this.totalOrderCount = res.totalNumber;
          // this.Loading = false;
          this.setTableData(res.data, true)
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
          // this.Loading = false;
        }
      })
    },
    // 单击
    rowOrderClick(row) {
      this.getId = row.id
      this.sourceNo = row.purchaseNo
    },
    // 确认单击
    OnBtnOrder() {
      if (!this.getId) {
        this.$message.error('请选择数据')
        return
      }
      this.getOrder(this.getId)
      this.dtData.order = this.sourceNo
      this.dtData.sourceId = this.getId
      this.orderVisible = false
    },
    // 双击
    dblOrderClick(row) {
      this.getOrder(row.id)
      this.dtData.order = row.purchaseNo
      this.dtData.sourceId = row.id
      this.orderVisible = false
    },
    paginationOrder(val) {
      // 改变弹窗页数
      this.pageOrderSize = val.pageSize
      this.pageOrderIndex = val.pageIndex
      // if (Object.keys(this.materialItem).length !== 0) {
      //   this.getMaterielData(this.materialItem);
      // } else {
      //   this.getMaterielData();
      // }
    },

    connectchange() {
      this.dtData.contactName = null
      this.dtData.contactNumber = null
      this.connectionData.map(item => {
        if (item.id == this.dtData.supplierId) {
          if (item.childList[0]) {
            this.dtData.contactName = item.childList[0].contactName
            this.dtData.contactNumber = item.childList[0].contactNumber
          }
        }
      })
    },
    getCustomer() {
      // 获取供应商
      var rqs = RequestObject.CreateGetObject(false, 0, 0, null)
      request({
        url: '/basicset/api/TBMSupplierFile/GetMainList',
        method: 'get',
        params: {
          requestObject: JSON.stringify(rqs)
        }
      }).then(res => {
        this.loading = false
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          // this.connectionData = res.data;
          this.connectData(res.data)
        }
      })
    },
    connectData(dt) {
      this.connectionData = []
      dt.map(item => {
        var data = {
          id: item.id,
          supplierName: item.supplierCode + ' ' + item.supplierName
        }
        this.connectionData.push(data)
      })
    },
    onKeydown(event) {
      // 按下Tab键
      if (event.keyCode !== 9) return
      var data = this.findCheck(event)
      this.setCheck(data)
    },
    findCheck(event) {
      // 寻找Tab下一个元素
      var data = {}
      for (var h = 0; h < this.tableData.length; h++) {
        for (var i in this.tableData[h]) {
          if (typeof this.tableData[h][i] === 'object') {
            if (this.tableData[h][i]['isShow'] === true) {
              for (var k = 0; k < this.TabArr.length; k++) {
                if (this.TabArr[k] === i) {
                  event.preventDefault()
                  var l = h
                  if (k + 1 > this.TabArr.length - 1) {
                    var go = 0
                    if (l >= this.tableData.length) {
                      l = this.tableData.length
                    } else {
                      l = l + 1
                    }
                  } else {
                    var go = k + 1
                  }
                  var set = this.TabArr[go]
                  data.index = l
                  data.name = set
                  data.item = this.tableData[l]
                  return data
                }
              }
            }
          }
        }
      }
    },
    setCheck(data) {
      if (data == undefined) return
      var { index, name, item } = data
      this.defaultAll()
      for (var k in this.tableData[index]) {
        if (k == name) {
          this.$set(this.tableData[index][name], `isShow`, true)
          this.$nextTick(() => {
            var id = this.tableData[index][name]['id']
            document.getElementById(id).focus()
            document.getElementById(id).select()
            if (
              document.getElementById(id).getAttribute('readonly') == 'readonly'
            ) {
              document.getElementById(id).click()
            }
          })
        }
      }
    },
    defaultAll() {
      // 重置所有切换了的Input
      this.popoverState = false
      for (var i = 0; i < this.tableData.length; i++) {
        this.doDefault(this.tableData[i])
      }
    },

    setStyle() {
      // 设置页面样式
      this.$nextTick(() => {
        var btn = 30 // 按钮高度
        var navbar = document.getElementById('navbar_yfkj')
        var nv = navbar.clientHeight || navbar.offsetHeight
        var b = document.documentElement.clientHeight - nv
        var elheader = document.getElementById('elheader')
        var elfooter = document.getElementById('elfooter')
        var h = elheader.clientHeight || elheader.offsetHeight
        var f = elfooter.clientHeight || elfooter.offsetHeight
        // this.tableHeight = b - h - f - 40 - 20 - btn;
        var pt = getStyle(document.getElementById('elmain'), 'paddingTop')
        var pb = getStyle(document.getElementById('elmain'), 'paddingBottom')
        this.tableHeight = b - h - f - parseInt(pt) - parseInt(pb) - btn
      })
    },
    resetData(state) {
      // 初始化数据
      this.isShowFlag = true
      this.$refs['dtData'].resetFields()
      this.dtData.operatorName = this.$store.state.user.name
      if (!state) {
        this.getCode()
      } else {
        var code = Storage.LStorage.get('ProduceOrder')
        this.dtData.orderNo = code
      }
      this.tableData = []
      this.tableData = this.setTable(this.tableLen)
      this.PostDataEdit = {}
      this.cloneTable = []
      this.selectRow = [];
      (this.dtData.auditId = null),
      (this.dtData.supplierId = null),
      (this.dtData.auditName = null),
      (this.dtData.auditStatus = 0),
      (this.dtData.auditTime = ''),
      (this.dtData.contactName = ''),
      (this.dtData.contactNumber = ''),
      (this.dtData.currency = ''),
      (this.dtData.order = null),
      (this.dtData.supplierName = null),
      (this.dtData.id = null),
      (this.dtData.operatorId = null),
      (this.dtData.orderDate = new Date()),
      (this.dtData.orderTypeId = null),
      (this.dtData.orderTypeName = null),
      (this.dtData.buyerId = null),
      (this.dtData.buyerName = null),
      (this.dtData.settlementTypeName = null),
      (this.dtData.settlementTypeId = null),
      (this.dtData.transferStatus = false)
      this.addControl = true
    },
    menuBar(event, num) {
      if (this.dtData.auditStatus == 2) {
        return
      }
      var el = closest(event.target, '.textAlign')
      var MT = el.getBoundingClientRect().top
      var ML = el.getBoundingClientRect().left
      var xh = 70 // 序号宽度
      if (this.$store.getters.sidebar.opened) {
        var ol = 210 - xh
      } else {
        var ol = 54 - xh
      }
      this.menuStyle.top = MT - 84 - 10 + 'px'
      this.menuStyle.left = ML - ol + 'px'
      this.listenClick()
      this.delNum = num
      this.menuState = true
    },
    doMenuAdd() {
      this.menuState = false
      // this.setTable(1);
      this.tableData = [...this.tableData, ...this.setTable(1)]
    },
    doMenuDel() {
      this.menuState = false
      if (this.delNum != -1) {
        this.tableData.splice(this.delNum, 1)
        this.delNum = -1
      }
    },
    tableRowClassName({ row, rowIndex }) {
      if (row.rowIndex === 1) {
        return 'color1'
      }
    },
    checkSelect(dt) {
      // 查找输入数据项
      return new Promise((resolve, reject) => {
        dt.map(item => {
          for (var i in item) {
            item.editState = false
            if (typeof item[i] === 'object') {
              if (item[i].value != '') {
                item.editState = true
                break
              }
            }
          }
        })
        resolve(dt)
      })
    },
    getCode() {
      request({
        url: '/purchase/api/TPSMPurchaseOrderMain/GetOrderNo',
        //
        method: 'GET'
      }).then(res => {
        if (res.code == -1) {
          this.dtData.orderNo = ''
        } else {
          Storage.LStorage.set('ProduceOrder', res)
          this.dtData.orderNo = res
        }
      })
    },

    async addPutInStorage(state) {
      // 新增
      if (state == 1 || state == 2) {
        if (this.selectRow.auditStatus === 2) {
          this.$message.error('该采购订单是通过状态，无法编辑')
          return
        }
        var flag = true
        var checkSelect = await this.checkSelect(this.tableData)
        this.$refs.dtData.validate(valid => {
          // 验证
          if (!valid) {
          } else {
            flag = true
            this.$refs.dtData2.validate(valid => {
              // 验证
              if (!valid) {
              } else {
                flag = true

                var childList = []
                childList = []
                this.remarkRel = true
                this.actualRel = true
                var actualNumRel = true
                var priceRel = true
                var res = /^\d{1,14}(\.\d{1,4})?$/
                checkSelect.map(item => {
                  // 验证数据

                  item.materialCode.valid = false
                  item.materialName.valid = false
                  item.actualNum.valid = false
                  item.validityPeriod.valid = false
                  item.remark.valid = false
                  if (state == 1) {
                    var param = {
                      id: 0,
                      mainId: 0,
                      materialId: item.materialCode.key,
                      unitPrice: item.unitPrice.value
                        ? item.unitPrice.value
                        : 0,
                      purchaseNum: item.actualNum.value
                        ? item.actualNum.value
                        : 0,
                      purchaseAmount: item.Totalamount.value
                        ? item.Totalamount.value
                        : 0,
                      deliveryPeriod: formatDate(item.validityPeriod.value),
                      tranPurchaseNum: item.tranPurchaseNum.value,
                      supplierId: item.supplierId.key ? item.supplierId.key : '',
                      remark: item.remark.value,
                      _LogName: '采购订单其中一条信息'
                    }
                  } else if (state == 2) {
                    var param = {
                      id: item.idandmainid.id ? item.idandmainid.id : 0,
                      mainId: this.globalid,
                      materialId: item.materialCode.key,
                      unitPrice: item.unitPrice.value
                        ? item.unitPrice.value
                        : 0,
                      purchaseNum: item.actualNum.value
                        ? item.actualNum.value
                        : 0,
                      purchaseAmount: item.Totalamount.value
                        ? item.Totalamount.value
                        : 0,
                      deliveryPeriod: formatDate(item.validityPeriod.value),
                      tranPurchaseNum: item.tranPurchaseNum.value,
                      supplierId: item.supplierId.key ? item.supplierId.key : '',
                      remark: item.remark.value,
                      _LogName: '采购订单其中一条信息'
                    }
                  }
                  if (param.materialId) {
                    if (!item.materialCode.key) {
                      // 验证物料代码
                      item.materialCode.valid = true
                      item.materialName.valid = true
                    }
                    // if (
                    //   !item.actualNum.value ||
                    //   item.actualNum.value > 99999999999
                    // ) {
                    //   item.actualNum.valid = true;
                    //   this.actualRel = false;
                    // }
                    if (res.test(item.actualNum.value)) {
                      item.actualNum.valid = false
                      this.actualRel = true
                    } else if (item.tranPurchaseNum.value < item.actualNum.value) {
                      item.actualNum.valid = true
                      actualNumRel = false
                    } else if (item.tranPurchaseNum.value == item.actualNum.value) {
                      item.actualNum.valid = false
                      actualNumRel = true
                    } else {
                      item.actualNum.valid = true
                      this.actualRel = false
                    }

                    if (
                      item.unitPrice.value &&
                      item.unitPrice.value > 9999999999999
                    ) {
                      item.unitPrice.valid = true
                      priceRel = false
                    }
                    if (
                      item.Totalamount.value &&
                      item.Totalamount.value > 9999999999999
                    ) {
                      item.Totalamount.valid = true
                      priceRel = false
                    }
                    if (item.remark.value && item.remark.value.length > 50) {
                      item.remark.valid = true
                    }
                    if (!item.validityPeriod.value) {
                      item.validityPeriod.valid = true
                    }

                    childList.push(param)
                    if (!param.deliveryPeriod || !param.purchaseNum) {
                      flag = false
                    }
                  }

                  if (item.remark.value && item.remark.value.length > 50) {
                    this.remarkRel = false
                    //  this.$message.error("备注数值长度超标");
                  }
                })
                if (state == 1) {
                  // 新增
                  var para = {
                    postData: {
                      id: 0,
                      supplierId: this.dtData.supplierId,
                      buyerId: this.dtData.buyerId,
                      orderNo: this.dtData.orderNo,
                      orderTypeId: this.dtData.orderTypeId,
                      settlementTypeId: this.dtData.settlementTypeId,
                      currency: this.dtData.currency,
                      orderDate: formatDate(this.dtData.orderDate),
                      operatorId: 0,
                      sourceId: this.dtData.sourceId,
                      contactName: this.dtData.contactName,
                      contactNumber: this.dtData.contactNumber,
                      childList: childList
                    }
                  }
                } else if (state == 2) {
                  // 编辑
                  var para = {
                    postData: {
                      id: this.globalid,
                      supplierId: this.dtData.supplierId,
                      buyerId: this.dtData.buyerId,
                      orderNo: this.dtData.orderNo,
                      orderTypeId: this.dtData.orderTypeId,
                      settlementTypeId: this.dtData.settlementTypeId,
                      currency: this.dtData.currency,
                      orderDate: formatDate(this.dtData.orderDate),
                      operatorId: 0,
                      sourceId: this.dtData.sourceId,
                      contactName: this.dtData.contactName,
                      contactNumber: this.dtData.contactNumber,
                      childList: childList
                    }
                  }
                }
                if (
                  !flag ||
                  !para.postData.buyerId ||
                  !para.postData.orderTypeId ||
                  para.postData.childList.length == 0
                ) {
                  this.$message.error('数据不完整')
                  return
                }
                if (!actualNumRel) {
                  this.$message.error('数量不能大于可转数量')
                  return
                }
                if (!priceRel) {
                  this.$message.error('价格不能大过99999999999999')
                  return
                }
                if (!this.actualRel) {
                  // 校验数量
                  this.$message.error('数量必须大于0小于99999999999999')
                } else if (!this.remarkRel) {
                  // 校验备注
                  this.$message.error('备注最大允许输入50个字符，请重新输入')
                } else {
                  if (state == 1) {
                    this.postData(para, state)
                  } else if (state == 2) {
                    if (this.selectRow.id == undefined) {
                      this.$message.error('请选择修改的数据')
                      return
                    }
                    // if (
                    //   !flag ||
                    //   !para.postData.buyerId ||
                    //   !para.postData.orderTypeId ||
                    //   para.postData.childList.length == 0
                    // ) {
                    //   this.$message.error("数据不完整");
                    //   return;
                    // } else {
                    this.postData(para, state)
                    // }
                  }
                }
              }
            })
          }
        })
      }

      if (state == 3) {
        // 删除
        if (this.selectRow.auditStatus === 2) {
          this.$message.error('该生产采购单是通过状态，无法删除')
          return
        }

        if (this.selectRow.id == undefined) {
          this.$message.error('请选择删除的数据')
          return
        }
        var currData = {
          id: this.selectRow.id,
          _LogName: `删除ID：${this.selectRow.id} 的采购订单`
        }

        var data = RequestObject.CreatePostObject(currData)

        this.$confirm('是否删除采购订单？', '确认信息', {
          type: 'warning'
        })
          .then(() => {
            this.postData(data, state)
          })
          .catch(() => {})
      }
    },

    postData(data, state) {
      var data = data

      var state = state
      var method = ''
      if (state == 1) {
        method = 'post'
      }
      if (state == 2) {
        method = 'put'
      }
      if (state == 3) {
        method = 'DELETE'
      }

      this.fullscreenLoading = true

      request({
        // /api/TPSMPurchaseOrderMain
        url: '/purchase/api/TPSMPurchaseOrderMain',
        method: method,
        data: data
      }).then(res => {
        if (res.code === 0) {
          this.$message({
            message: '操作成功',
            type: 'success'
          })
          if (state == 1) {
            this.setTableData(res.data.childList)
            this.globalid = res.data.id
            // this.OnBtnSaveSubmit(res.data);
            // console.log(this.dtData)
            this.selectRow = this.cloneObject(res.data)
            Storage.LStorage.remove('ProduceOrder')
            // this.resetData()
            this.addControl = false
          }

          if (state == 3) {
            this.tableData = []
            this.resetData()
          }
          setTimeout(() => {
            this.fullscreenLoading = false
          }, 500)
        } else {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
          setTimeout(() => {
            this.fullscreenLoading = false
          }, 500)
        }
      })
    },

    setTable(num) {
      // 初始化数据
      var listArr = []
      for (var i = 0; i < num; i++) {
        var list = {}
        for (var j in this.tableData2[0]) {
          if (j == 'id') {
            list['id'] = newGuid()
          } else {
            if (typeof this.tableData2[0][j] === 'object') {
              list[j] = {}
              for (var k in this.tableData2[0][j]) {
                if (k == 'id') {
                  list[j][k] = newGuid()
                  continue
                }
                if (typeof this.tableData2[0][j][k] === 'boolean') {
                  list[j][k] = false
                } else {
                  list[j][k] = ''
                }
              }
            } else {
              list[j] = ''
            }
          }
        }
        listArr.push(list)
      }
      return listArr
    },
    getMaterielData(item) {
      // 获取物料档案信息
      this.materielData = []
      var queryData = []
      if (item) {
        queryData.push({
          column: 'materialCode',
          content: trim(item.materialCode.value),
          condition: 6
        })
        queryData.push({
          column: 'materialName',
          content: trim(item.materialName.value),
          condition: 6
        })
      }
      var reqObject
      reqObject = RequestObject.LonginBookObject(
        true,
        this.pageSize,
        this.pageIndex,
        null,
        queryData
      )
      request({
        url: '/basicset/api/TBMMaterialFile',
        method: 'get',
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        this.loading = false
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.materielData = res.data
          this.totalCount = res.totalNumber
        }
      })
    },
    getUserCompany() {
      var param = []
      var robject = RequestObject.CreateGetObject(false, 0, 0, param)
      request({
        url: '/system/api/TSMUser',
        method: 'get',
        params: {
          requestObject: robject
        }
      }).then(res => {
        if (res.code === 0) {
          var operatorlist = []
          res.data.map(item => {
            var obj = new Object({
              id: item.id,
              realName: item.realName
            })
            operatorlist.push(obj)
          })
          this.operatorlist = operatorlist
        } else {
        }
      })
      // },
      var reqObject = RequestObject.LonginBookObject(false, 0, 0, null, null)
      request({
        url: '/system/api/TSMUser/GetUserInCurentCompany',
        method: 'get',
        params: {
          requestObject: JSON.stringify(reqObject)
        }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.realNameOptions = res.data
        }
      })
    },

    getorderTypeoption() {
      var rqs = RequestObject.CreateGetObject(false, 0, 0, [
        { column: 'typeId', content: 21, condition: 0 }
      ])

      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [{ column: 'TypeName', content: '采购方式', condition: 0 }],
        orderByConditions: null
      }
      request({
        url: '/basicset/api/TBMDictionary',
        method: 'get',
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.orderTypeoption = res.data
          //  dicValue
          // id
        }
      })
    },
    getsettlementTypeoption() {
      // var rqs2 = RequestObject.CreateGetObject(false, 0, 0, [
      //   { column: "typeId", content: 19, condition: 0 }
      // ]);

      var para = {
        isPaging: false,
        pageSize: 0,
        pageIndex: 0,
        queryConditions: [{ column: 'TypeName', content: '支付方式', condition: 0 }],
        orderByConditions: null
      }
      request({
        url: '/basicset/api/TBMDictionary',
        method: 'get',
        params: { RequestObject: para }
      }).then(res => {
        if (res.code == -1) {
          this.$confirm(res.info, '错误信息', {
            confirmButtonText: '确定',
            type: 'error',
            showCancelButton: false
          })
        } else {
          this.settlementTypeoption = res.data
        }
      })
    },

    // rowClick(row) {
    //   var that = this;

    //   this.doItem.materialCode.value = row.materialCode;
    //   this.doItem.materialCode.key = row.id;
    //   this.doItem.materialName.value = row.materialName;
    //   this.doItem.spec.value = row.spec;
    //   this.doItem.colorName.value = row.colorName;
    //   this.doItem.purchaseUnitName.value = row.purchaseUnitName;
    //   this.doItem.baseUnitName.value = row.baseUnitName;
    //   this.doItem.warehouseUnitId.value = row.warehouseUnitId;
    //   this.doItem.purchaseRate.value = row.purchaseRate;
    //   this.listenClick();
    //   this.doItem.actualNum.isShow = true;
    //   this.$nextTick(() => {
    //     document.getElementById(this.doItem.id + "___" + "actualNum").focus();
    //   });

    //   that.actualNumBlur(that.nowrow, that.nowrow.actualNum.value);
    // },
    getSummaries(param) {
      // table总计
      const { columns, data } = param
      const sums = []
      columns.forEach((column, index) => {
        if (index === 0) {
          sums[index] = '总计'
          return
        }
        const values = data.map(item => {
          return Number(item[column.property].value)
        })
        if (!values.every(value => isNaN(value))) {
          sums[index] = values.reduce((prev, curr) => {
            const value = Number(curr)
            if (!isNaN(value)) {
              return BigNumber(prev).plus(curr).toNumber()
            } else {
              return prev
            }
          }, 0)
          // actualNum
          if (index == 7 || index == 8 || index == 6 || index == 11) {
            sums[index] += ''
          } else {
            sums[index] = ''
          }
        } else {
          sums[index] = ''
        }
      })

      return sums
    },
    chengenum(event, item, name, state) {
      // 双击显示input,监听input数据变化
      if (this.selectRow.auditStatus === 2) {
        return
      }
      event.stopPropagation()
      this.nowrow = item
      this.defaultAll(this.doItem)
      // 这改了
      item[name].isShow = true
      this.doItem = item
      this.doItemName = name
      this.$nextTick(() => {
        if (document.getElementById(item.id + '___' + name)) {
          document.getElementById(item.id + '___' + name).focus()
        }
        if (state == 1) {
          this.findBox(item, name)
          this.getMaterielData()
        }
      })
    },
    findBox(item, name) {
      var IH = document.getElementById(item.id).offsetHeight + 8
      var IW = document.getElementById(item.id).offsetWidth + 24
      if (this.$store.getters.sidebar.opened) {
        var ol = 210
      } else {
        var ol = 54
      }
      var wl = document.documentElement.clientWidth // 屏幕宽度
      var wh = document.documentElement.clientHeight // 屏幕宽度
      var PoL = document.getElementById(item.id).getBoundingClientRect().left // 弹框left值
      var PoT = document.getElementById(item.id).getBoundingClientRect().top // 弹框top值
      var PoW = parseInt(this.popoverStyle.width)
      var PoH = parseInt(this.popoverStyle.height)
      if (PoW + PoL > wl) {
        this.popoverStyle.left = PoL - ol - PoW + IW + 'px'
      } else {
        this.popoverStyle.left = PoL - ol + 'px'
      }
      if (PoT + PoH > wh) {
        this.popoverStyle.top = PoT - PoH - 84 - 9 + 'px'
      } else {
        this.popoverStyle.top = PoT - 84 + IH + 'px'
      }
      this.popoverState = true
    },
    listenClick() {
      this.popoverState = false
      this.defaultAll()
    },
    changeBlur(item, value) {},
    getByClass(oParent, sClass) {
      var aResult = []
      var aEle = oParent.getElementsByTagName('*')

      for (var i = 0; i < aEle.length; i++) {
        if (aEle[i].className == sClass) {
          aResult.push(aEle[i])
        }
      }
      return aResult
    },

    // pagination(val) {
    //   //改变页数
    //   this.pageSize = val.pageSize;
    //   this.pageIndex = val.pageIndex;
    //   if (Object.keys(this.materialItem).length !== 0) {
    //     this.getMaterielData(this.materialItem);
    //   } else {
    //     this.getMaterielData();
    //   }
    // },
    getMaterielDataIndex(item) {
      this.pageIndex = 1
      if (item) {
        this.materialItem = item
        this.getMaterielData(item)
      } else {
        this.getMaterielData()
      }
    },
    defaultData(data) {
      data.map(item => {
        for (var i in item) {
          this.$set(item, i + '_YF_Edit', false)
        }
      })
      return data
    },
    doDefault(data) {
      this.menuState = false
      for (var i in data) {
        if (typeof data[i] === 'object') {
          if (data[i]['isShow']) {
            data[i]['isShow'] = false
          }
        }
      }
    },

    PaidInBlur(item) {
      if (isRealNum(item.actualNum.value) === false) {
        item.actualNum.value = ''
        return
      }
      if (item.actualNum.value === '' || item.actualNum.value === null) return
      if (item.actualNum.value <= 0) {
        item.actualNum.value = ''
        return
      }
      item.actualNum.value = parseFloat(item.actualNum.value)
      // item.PaidIn.value = keepTwoDecimalFull(item.PaidIn.value);
      item.actualNum.value = getFloat(item.actualNum.value, 2)
      item.actualNum.valid = false
      this.unitPriceBlur(item)
      this.amountBlur(item)
    },
    unitPriceBlur(item) {
      if (isRealNum(item.unitPrice.value) === false) {
        item.unitPrice.value = ''
        return
      }
      if (item.unitPrice.value === '' || item.unitPrice.value === null) return
      if (item.unitPrice.value <= 0) {
        item.unitPrice.value = ''
        return
      }

      if (item.actualNum.value !== '') {
        item.unitPrice.value = parseFloat(item.unitPrice.value)
        item.unitPrice.value = keepTwoDecimalFull(item.unitPrice.value)
        item.Totalamount.value = keepTwoDecimalFull(
          accMul(item.actualNum.value, item.unitPrice.value)
        )
      } else {
        item.unitPrice.value = keepTwoDecimalFull(
          parseFloat(item.unitPrice.value)
        )
      }
    },
    amountBlur(item) {
      if (isRealNum(item.Totalamount.value) === false) {
        item.Totalamount.value = ''
        return
      }
      if (item.Totalamount.value === '' || item.Totalamount.value === null) return
      if (item.Totalamount.value <= 0) {
        item.Totalamount.value = ''
        return
      }
      item.Totalamount.value = keepTwoDecimalFull(item.Totalamount.value)
      if (item.actualNum.value !== '') {
        item.Totalamount.value = parseFloat(item.Totalamount.value)
        item.unitPrice.value = keepTwoDecimalFull(
          item.Totalamount.value / item.actualNum.value
        )
      } else {
        item.Totalamount.value = keepTwoDecimalFull(parseFloat(item.Totalamount.value))
      }
    },

    handelAddClick() {
      // 查询
      // 点击添加按钮
      this.$refs.InboundOrder.open()
      this.$refs.InboundOrder.dtData.orderNo = ''
      this.$refs.InboundOrder.dtData.auditStatus = -1
      this.$refs.InboundOrder.clickRow = {}
      this.$refs.InboundOrder.getMainList()
    },
    cloneObject(origin) {
      return Object.assign({}, origin)
    },

    OnBtnSaveSubmit(row) {
      if (row.auditStatus == 2) {
        this.isShowFlag = false
      } else {
        this.isShowFlag = true
      }
      var rqs = RequestObject.CreateGetObject(false, 0, 0, [
        {
          column: 'sourceId',
          content: null,
          condition: 12
        },
        {
          column: 'id',
          content: row.id,
          condition: 0
        }
      ])
      // console.log(row)
      request({
        url: '/purchase/api/TPSMPurchaseOrderMain/GetMainList',
        method: 'GET',
        params: { RequestObject: JSON.stringify(rqs) }
      }).then(res => {
        if (res.code === 0) {
          this.selectRow = this.cloneObject(res.data[0])
          this.dtData.id = res.data[0].id
          this.dtData.sourceId = res.data[0].sourceId
          this.dtData.supplierId = res.data[0].supplierId
          this.dtData.auditName = res.data[0].auditName
          this.dtData.auditStatus = res.data[0].auditStatus
          this.dtData.auditTime =
            res.data[0].auditTime != null
              ? res.data[0].auditTime.split('T')[0]
              : ''
          this.dtData.order = res.data[0].sourceNo
          this.dtData.contactName = res.data[0].contactName
          this.dtData.contactNumber = res.data[0].contactNumber
          this.dtData.currency = res.data[0].currency
          this.dtData.supplierName = res.data[0].supplierName
          this.dtData.operatorId = res.data[0].operatorId
          this.dtData.operatorName = res.data[0].operatorName
          this.dtData.orderDate =
            res.data[0].orderDate != null
              ? res.data[0].orderDate.split('T')[0]
              : ''
          // haha
          this.dtData.orderNo = res.data[0].orderNo
          this.dtData.orderTypeId = res.data[0].orderTypeId
          this.dtData.orderTypeName = res.data[0].orderTypeName

          this.dtData.buyerId = res.data[0].buyerId
          this.dtData.buyerName = res.data[0].buyerName

          this.dtData.settlementTypeName = res.data[0].settlementTypeName
          this.dtData.settlementTypeId = res.data[0].settlementTypeId
          this.dtData.transferStatus = res.data[0].transferStatus
          request({
            url: '/purchase/api/TPSMPurchaseOrderMain/GetDetailList',
            params: { RequestObject: row.id },
            method: 'GET'
          }).then(res => {
            // this.selectRow = this.cloneObject(res.data);
            this.setTableData(res.data, false)
            this.addControl = false
          })
        }
      })
    },

    // setCurrData(data) {
    //   //设置编辑时候的日记数据
    //   var childList = [];
    //   var _LogName = "";
    //   this.cloneTable.map(item => {
    //     var param = {
    //       id: 0,
    //       mainId: 0,
    //       materialId: parseFloat(item.materialCode.key) || ""
    //       // warehouseId: parseFloat(item.defaultWarehouseName.key) || ""
    //       // actualNumber: parseFloat(item.PaidIn.value) || ""
    //     };
    //     if (item.batchNumber.value != "") {
    //       //批号
    //       param.batchNumber = item.batchNumber.value;
    //     }
    //     if (item.unitPrice.value != "" && !isNaN(item.unitPrice.value)) {
    //       //单价
    //       param.unitPrice = parseFloat(item.unitPrice.value) || 0;
    //     }
    //     if (item.amount.value != "" && !isNaN(item.amount.value)) {
    //       //金额
    //       param.amount = parseFloat(item.amount.value) || 0;
    //     }

    //     param._LogName = _LogName;
    //     childList.push(param);
    //   });
    //   var currData = {
    //     id: this.selectRow.id,
    //     orderNo: this.selectRow.orderNo,
    //     childList: childList
    //   };

    //   this.PostDataEdit = currData;
    // },

    setTableData(dt, boolean) {
      // 将数据导入table
      this.tableData = []
      if (dt[0]) {
        this.globalid = dt[0].mainId
      }

      var listArr = []
      if (boolean) {
        dt.map(item => {
          var list = {
            id: newGuid(),
            editState: false,
            rowIndex: 0,

            idandmainid: {
              id: item.id,
              mainId: item.mainId,
              isShow: false,
              valid: false
            },
            materialCode: {
              id: newGuid(),
              value: item.materialCode,
              key: item.materialId,
              isShow: false,
              valid: false
            },
            materialName: {
              id: newGuid(),
              value: item.materialName,
              isShow: false,
              valid: false
            },
            spec: {
              id: newGuid(),
              value: item.spec,
              isShow: false,
              valid: false
            },
            colorName: {
              // 颜色
              id: newGuid(),
              value: item.colorName,
              isShow: false,
              valid: false
            },

            baseUnitName: {
              // 基本单位名称
              id: newGuid(),
              value: item.baseUnitName,
              isShow: false,
              valid: false
            },
            actualNum: {
              // 数量
              id: newGuid(),
              // value: item.purchaseNum,
              value: item.tranPurchaseNum,
              isShow: false,
              valid: false
            },

            actualUnitNum: {
              // 基本单位数量
              id: newGuid(),
              value: item.baseUnitNum,
              isShow: false,
              valid: false
            },
            // 应采购数量
            tranPurchaseNum: {
              id: newGuid(),
              value: item.tranPurchaseNum,
              isShow: false,
              valid: false
            },

            unitPrice: {
              // 单价
              id: newGuid(),
              value: item.unitPrice,
              isShow: false,
              valid: false
            },
            Totalamount: {
              // 金额
              id: newGuid(),
              value: item.purchaseAmount,
              isShow: false,
              valid: false
            },

            purchaseRate: {
              // 基本单位换算率
              id: newGuid(),
              value: item.purchaseRate,
              isShow: false,
              valid: false
            },
            supplierId: {
              // 供应商
              id: newGuid(),
              value: item.supplierName,
              key: item.supplierId,
              isShow: false,
              valid: false
            },
            remark: {
              // 备注
              id: newGuid(),
              value: item.remark,
              isShow: false,
              valid: false
            },

            validityPeriod: {
              id: newGuid(),
              value: item.deliveryPeriod,
              isShow: false,
              valid: false
            },
            purchaseUnitName: {
              // 单位
              id: newGuid(),
              value: item.purchaseUnitName,
              isShow: false,
              valid: false
            },
            warehouseUnitId: {
              // 基本单位换算率id
              id: newGuid(),
              value: item.warehouseUnitId,
              isShow: false,
              valid: false
            }
          }

          listArr.push(list)
        })
      } else {
        dt.map(item => {
          var list = {
            id: newGuid(),
            editState: false,
            rowIndex: 0,

            idandmainid: {
              id: item.id,
              mainId: item.mainId,
              isShow: false,
              valid: false
            },
            materialCode: {
              id: newGuid(),
              value: item.materialCode,
              key: item.materialId,
              isShow: false,
              valid: false
            },
            materialName: {
              id: newGuid(),
              value: item.materialName,
              isShow: false,
              valid: false
            },
            spec: {
              id: newGuid(),
              value: item.spec,
              isShow: false,
              valid: false
            },
            colorName: {
              // 颜色
              id: newGuid(),
              value: item.colorName,
              isShow: false,
              valid: false
            },

            baseUnitName: {
              // 基本单位名称
              id: newGuid(),
              value: item.baseUnitName,
              isShow: false,
              valid: false
            },
            // 应采购数量
            tranPurchaseNum: {
              id: newGuid(),
              value: item.purchaseTranNnum,
              isShow: false,
              valid: false
            },
            actualNum: {
              // 数量
              id: newGuid(),
              value: item.purchaseNum,
              isShow: false,
              valid: false
            },

            actualUnitNum: {
              // 基本单位数量
              id: newGuid(),
              value: item.baseUnitNum,
              isShow: false,
              valid: false
            },
            // 应采购数量
            tranPurchaseNum: {
              id: newGuid(),
              value: item.purchaseTranNnum,
              isShow: false,
              valid: false
            },

            unitPrice: {
              // 单价
              id: newGuid(),
              value: item.unitPrice,
              isShow: false,
              valid: false
            },
            Totalamount: {
              // 金额
              id: newGuid(),
              value: item.purchaseAmount,
              isShow: false,
              valid: false
            },

            purchaseRate: {
              // 基本单位换算率
              id: newGuid(),
              value: item.purchaseRate,
              isShow: false,
              valid: false
            },
            supplierId: {
              // 供应商
              id: newGuid(),
              value: item.supplierName,
              key: item.supplierId,
              isShow: false,
              valid: false
            },
            remark: {
              // 备注
              id: newGuid(),
              value: item.remark,
              isShow: false,
              valid: false
            },

            validityPeriod: {
              id: newGuid(),
              value: item.deliveryPeriod,
              isShow: false,
              valid: false
            },
            purchaseUnitName: {
              // 单位
              id: newGuid(),
              value: item.purchaseUnitName,
              isShow: false,
              valid: false
            },
            warehouseUnitId: {
              // 基本单位换算率id
              id: newGuid(),
              value: item.warehouseUnitId,
              isShow: false,
              valid: false
            }
          }

          listArr.push(list)
        })
      }

      this.tableData = listArr
      // setTimeout(() => {
      //   this.tableData = [...listArr, ...this.setTable(30)];
      // }, 0);
    },

    doOtherWhAudit(state) {
      // 审核
      if (this.selectRow.auditStatus == undefined) {
        this.$message.error('请选择审核数据')
        return
      }
      if (this.selectRow.auditStatus == 2) {
        this.$message.error('该采购订单已经为通过状态')
        return
      }
      var data = RequestObject.CreatePostObject({
        id: this.selectRow.id,
        auditStatus: state
      })
      // var data = {
      //   postData: {
      //     id: this.selectRow.id,
      //     auditStatus: state
      //   }
      // };

      if (state == 1) {
        var tip = '确定审核未通过吗？'
      } else if (state == 2) {
        var tip = '确定审核通过吗？'
      }

      this.$confirm(tip, '确认信息', {
        type: 'warning'
      })
        .then(() => {
          request({
            url: '/purchase/api/TPSMPurchaseOrderMain/PurchaseAudit',
            method: 'put',
            data: data
          }).then(res => {
            this.fullscreenLoading = true
            if (res.code === 0) {
              this.$message({
                message: '操作成功',
                type: 'success'
              })
              this.dtData.auditStatus = state
              this.selectRow.auditStatus = state
              setTimeout(() => {
                this.fullscreenLoading = false
                this.OnBtnSaveSubmit({ id: this.globalid })
              }, 500)
            } else {
              this.$confirm(res.info, '错误信息', {
                confirmButtonText: '确定',
                type: 'error',
                showCancelButton: false
              })
              setTimeout(() => {
                this.fullscreenLoading = false
              }, 500)
            }
          })
        })
        .catch(() => {
          this.fullscreenLoading = false
        })
    },
    StopReceipt(row, state) {
      this.$confirm('是否终止？', {
        type: 'warning'
      })
        .then(() => {
          var URL = ''
          URL = '/manufacturing/api/TMMPurchaseApplyMain/StopPurchase'
          this.fullscreenLoading = true
          request({
            url: URL,
            method: 'put',
            params: { requestObject: row.id }
          }).then(res => {
            if (res.code === 0) {
              this.$message({
                message: '操作成功',
                type: 'success'
              })
              this.orderVisibleBtn()
              this.resetData(1)
              this.fullscreenLoading = false
            } else {
              this.$confirm(res.info, '错误信息', {
                confirmButtonText: '确定',
                type: 'error',
                showCancelButton: false
              })
              this.fullscreenLoading = false
            }
          })
        })
        .catch(() => {})
    }
  }
}
</script>

<style>
#Purchasingorder #elfooter .el-input__inner {
  border: 0px;
  border-radius: 0px;
  border-bottom: 1px solid #dcdfe6;
}
</style>

<style lang="scss" scoped>
#Purchasingorder /deep/ {
  .redCol {
    color: red;
  }
  .greCol {
    color: #42b983;
  }
  .el-main {
    padding-top: 0px;
  }
  #elfooter {
    padding: 0px 20px;
  }
  .findBox {
    position: absolute;
    z-index: 99;
    padding: 12px;
    box-shadow: 0 2px 12px 0 rgba(0, 0, 0, 0.1);
    border-radius: 4px;
    border: 1px solid #ebeef5;
    background: #fff;
    .menuUl {
      padding: 0px;
      margin: 0px;
      li {
        list-style: none;
        padding: 6px 0px;
        text-align: center;
        font-size: 12px;
        cursor: pointer;
      }
    }
  }
  .header {
    padding-top: 20px;
  }
  .el-popover {
    height: 500px;
    overflow: auto;
  }
  .pr {
    position: relative;
  }
  .cell {
    padding: 0px;
  }
  .tableTd {
    box-sizing: content-box;
    min-height: 23px;
    padding: 8px 10px;
  }
  .validStyle {
    background: #f3d4d4;
  }
  .el-table--small td {
    padding: 0px;
  }
  .falseIp {
    width: 300px;
    height: 32px;
    line-height: 32px;
    overflow: hidden;
    border-bottom: 1px solid #dcdfe6;
  }
  .textAlign {
    text-align: center;
  }
  .start {
    color: #ff4949;
  }
  .color1 {
    background: #ff4949 !important;
  }
  .tableHeader {
    display: inline-block;
    padding: 0px 10px;
  }
  .dropdown button {
    position: relative;
    left: -1px;
    height: 32px;
    vertical-align: middle;
    border-top-left-radius: 0px;
    border-bottom-left-radius: 0px;
  }
  .groupBtn > .el-button:last-child {
    border-top-right-radius: 0px;
    border-bottom-right-radius: 0px;
  }
  .el-dropdown {
    vertical-align: middle;
  }
  .fromCenter {
    display: flex;
    justify-content: center;
    flex-wrap: wrap;
    // justify-content: center;
  }
  .footerCenter {
    width: 1150px;
    margin: 0 auto;
  }
  .has-gutter tr td {
    text-align: center;
  }

  .el-form-item__label {
    padding-right: 4px;
  }
  .el-table {
    overflow: visible !important;
  }

  .formItem {
    // margin-left: 50px;
    width: 306px;
  }

  .middleWidth {
    width: 1598px;
  }

  .el-select .el-input.is-disabled .el-input__inner {
    cursor: default;
    color: #606266;
    background: #fff;
  }
  .el-input.is-disabled .el-input__icon {
    cursor: default;
  }
  // .disableType i {
  //   // border-top: 1px solid rgba(220, 223, 230,0);
  //   // border-bottom: 1px solid rgba(220, 223, 230,0);
  //   height: 32px;
  // }
}
#orderBox /deep/ {
  .el-dialog--center .el-dialog__body {
    // padding: 20px 20px 0px;
  }
  .el-table--small td {
    padding: 8px !important;
  }
}
.headerBd .el-form {
  border-bottom: none;
  margin-bottom: 0px !important;
}
@import "@/styles/receipts.scss";
</style>
