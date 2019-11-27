// 创建Get请求对象
var CreateGetObject = function CreateGetObject(
  IsPaging,
  PageSize,
  PageIndex,
  QueryConditions,
  OrderByConditions
) {
  var request = {
    isPaging: false,
    pageSize: 25,
    pageIndex: 1,
    postData: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (IsPaging !== undefined) {
    request.isPaging = IsPaging;
  }
  if (PageSize !== undefined) {
    request.pageSize = PageSize;
  }
  if (PageIndex !== undefined) {
    request.pageIndex = PageIndex;
  }
  if (QueryConditions !== undefined) {
    request.queryConditions = QueryConditions;
  }
  if (OrderByConditions !== undefined) {
    request.orderByConditions = OrderByConditions;
  }
  return request;
};
// 创建Post、Put、Delete请求对象
var CreatePostObject = function CreatePostObject(
  PostData,
  PostDataList,
  PostDataEdit,
  postDataEditList
) {
  var request = {
    isPaging: false,
    pageSize: 25,
    pageIndex: 1,
    postData: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (PostData !== undefined) {
    request.postData = PostData;
  }
  if (PostDataList !== undefined) {
    request.postDataList = PostDataList;
  }
  if (PostDataEdit !== undefined) {
    request.PostDataEdit = PostDataEdit;
  }
  if (postDataEditList !== undefined) {
    request.postDataEditList = postDataEditList;
  }
  return request;
};
var CreateNewPostObject = function CreateNewPostObject(
  PostData,
  PostDataList,
  PostDataEdit,
  postDataEditList
) {
  var request = {
    isPaging: false,
    pageSize: 25,
    pageIndex: 1,
    postData: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (PostData !== undefined) {
    request.postData = PostData;
  }
  if (PostDataList !== undefined) {
    request.postDataList = PostDataList;
  }
  if (PostDataEdit !== undefined) {
    request.PostDataEdit = PostDataEdit;
  }
  if (postDataEditList !== undefined) {
    request.postDataEditList = postDataEditList;
  }
  return request;
};
var CreateQueryObject = function CreateQueryObject(PostData, QueryDataList) {
  var request = {
    isPaging: false,
    pageSize: 25,
    pageIndex: 1,
    postData: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (PostData !== undefined) {
    request.postData = PostData;
  }
  if (QueryDataList !== undefined) {
    request.queryConditions = QueryDataList;
  }
  return request;
};
/**
 * GetObject
 * 系统管理get请求
 *
 * @param {*提交对象用于单个} PostData
 * @param {*提交数组用于多个} postDataList
 * @param {*条件查询} QueryDataList
 */
var GetObject = function CreateQueryObject(
  PostData,
  postDataList,
  QueryDataList,
  postDataEdit
) {
  var request = {
    info: "",
    code: 0,
    totalNumber: 0,
    isPaging: false,
    pageSize: 0,
    pageIndex: 0,
    postData: null,
    postDataEdit: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (PostData !== undefined) {
    request.postData = PostData;
  }
  if (postDataList !== undefined) {
    request.postDataList = postDataList;
  }
  if (QueryDataList !== undefined) {
    request.queryConditions = QueryDataList;
  }
  if (postDataEdit !== undefined) {
    request.postDataEdit = postDataEdit;
  }
  return request;
};
/**
 * DeleteObject
 * 部门维护
 * @param {*提交对象用于单个} PostData
 * @param {*提交数组用于多个} postDataList
 * @param {*条件查询} QueryDataList
 */
var DeleteObject = function CreateQueryObject(
  PostData,
  postDataList,
  QueryDataList
) {
  var request = {
    info: "",
    code: 0,
    totalNumber: 0,
    isPaging: false,
    pageSize: 0,
    pageIndex: 0,
    postData: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (PostData !== undefined) {
    request.postData = PostData;
  }
  if (postDataList !== undefined) {
    request.postDataList = postDataList;
  }
  if (QueryDataList !== undefined) {
    request.queryConditions = QueryDataList;
  }
  return request;
};

var LonginBookObject = function CreateGetObject(
  IsPaging,
  PageSize,
  PageIndex,
  postData,
  QueryConditions,
  orderConditions
) {
  var request = {
    isPaging: false,
    pageSize: 25,
    pageIndex: 1,
    postData: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (IsPaging !== undefined) {
    request.isPaging = IsPaging;
  }
  if (PageSize !== undefined) {
    request.pageSize = PageSize;
  }
  if (PageIndex !== undefined) {
    request.pageIndex = PageIndex;
  }
  if (postData !== undefined) {
    request.postData = postData;
  }
  if (QueryConditions !== undefined) {
    request.queryConditions = QueryConditions;
  }
  if(orderConditions!==undefined) {
    request.orderByConditions = orderConditions
  }
  return request;
};
/**
 *
 * 分页查询
 *CreateGetObjectFenye
 *Get请求
 */
var CreateGetObjectFenye = function CreateGetObjectFenye(
  IsPaging,
  PageSize,
  PageIndex,
  postData,
  QueryConditions,
  OrderByConditions
) {
  var request = {
    isPaging: false,
    pageSize: 25,
    pageIndex: 1,
    postData: null,
    postDataList: null,
    queryConditions: null,
    orderByConditions: null
  };
  if (IsPaging !== undefined) {
    request.isPaging = IsPaging;
  }
  if (PageSize !== undefined) {
    request.pageSize = PageSize;
  }
  if (PageIndex !== undefined) {
    request.pageIndex = PageIndex;
  }
  if (postData !== undefined) {
    request.postData = postData;
  }
  if (QueryConditions !== undefined) {
    request.queryConditions = QueryConditions;
  }
  if (OrderByConditions !== undefined) {
    request.orderByConditions = OrderByConditions;
  }
  return request;
};
const RequestObject = {
  CreateGetObject: CreateGetObject,
  CreatePostObject: CreatePostObject,
  CreateQueryObject: CreateQueryObject,
  GetObject: GetObject,
  DeleteObject: DeleteObject,
  LonginBookObject: LonginBookObject,
  CreateGetObjectFenye: CreateGetObjectFenye,
  CreateNewPostObject: CreateNewPostObject,
};

export default RequestObject;
