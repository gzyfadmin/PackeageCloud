
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using YfCloud.Models;

namespace YfCloud.Utilities.SqlSugar
{
    /// <summary>
    /// SqlSugar Util
    /// </summary>
    public static class SqlSugarUtil
    {
        /// <summary>
        /// 获取查询条件表达式集合
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        public static List<string> GetQueryExpressions(List<QueryCondition> queryConditions)
        {
            var result = new List<string>();
            queryConditions.ForEach(p => result.Add(GetQueryExpression(p)));
            return result;
        }

        /// <summary>
        /// 获取查询模型集合
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        public static List<IConditionalModel> GetConditionalModels(List<QueryCondition> queryConditions)
        {
            var result = new List<IConditionalModel>();
            queryConditions.ForEach(p => GetConditionalModel(p).ForEach(p2 => result.Add(p2)));
            return result;
        }

        public static List<ConditionalModel> GetConditionalFull(List<QueryCondition> queryConditions)
        {
            var result = new List<ConditionalModel>();
            queryConditions.ForEach(p => GetConditionalModel(p).ForEach(p2 => result.Add(p2)));
            return result;
        }

        private static List<ConditionalModel> GetConditionalModel(QueryCondition queryCondition)
        {
            switch (queryCondition.Condition)
            {

                case ConditionEnum.Equal:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.Equal) };
                case ConditionEnum.LessThan:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.LessThan) };
                case ConditionEnum.LessThanOrEqual:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.LessThanOrEqual) };
                case ConditionEnum.GreaterThan:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.GreaterThan) };
                case ConditionEnum.GreaterThanOrEqual:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.GreaterThanOrEqual) };
                case ConditionEnum.NotEqual:
                case ConditionEnum.NoEqual:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.NoEqual) };
                case ConditionEnum.Like:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.Like) };
                case ConditionEnum.LikeLeft:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.LikeLeft) };
                case ConditionEnum.LikeRight:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.LikeRight) };
                case ConditionEnum.In:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.In) };
                case ConditionEnum.Between:
                    return new List<ConditionalModel> {
                        GetModel(queryCondition.Column, queryCondition.Content.Split(',')[0], ConditionalType.GreaterThanOrEqual),
                        GetModel(queryCondition.Column, queryCondition.Content.Split(',')[1], ConditionalType.LessThanOrEqual)
                    };
                case ConditionEnum.IsNullOrEmpty:
                case ConditionEnum.IsNull:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.IsNullOrEmpty) };
                case ConditionEnum.IsNot:
                case ConditionEnum.IsNotNull:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.IsNot) };
                case ConditionEnum.NotIn:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.NotIn) };                
                case ConditionEnum.NoLike:
                    return new List<ConditionalModel> { GetModel(queryCondition.Column, queryCondition.Content, ConditionalType.NoLike) };
                default:
                    return null;
            }
        }

        private static ConditionalModel GetModel(string fieldName, string fieldValue, ConditionalType ct)
        {
            return new ConditionalModel
            {
                ConditionalType = ct,
                FieldName = fieldName,
                FieldValue = fieldValue
            };
        }

        /// <summary>
        /// 获取查询表达式
        /// </summary>
        /// <param name="queryCondition"></param>
        /// <returns></returns>
        private static string GetQueryExpression(QueryCondition queryCondition)
        {
            switch (queryCondition.Condition)
            {
                case ConditionEnum.Between:
                    return GetBetweenExpression(queryCondition.Column, queryCondition.Content);
                case ConditionEnum.Equal:
                    return $"{queryCondition.Column} = '{queryCondition.Content}'";
                case ConditionEnum.GreaterThan:
                    return $"{queryCondition.Column} > '{queryCondition.Content}'";
                case ConditionEnum.GreaterThanOrEqual:
                    return $"{queryCondition.Column} >= '{queryCondition.Content}'";
                case ConditionEnum.In:
                    return GetInExpression(queryCondition.Column, queryCondition.Content);
                case ConditionEnum.IsNotNull:
                    return $"{queryCondition.Column} is not null";
                case ConditionEnum.IsNull:
                    return $"{queryCondition.Column} is null";
                case ConditionEnum.LessThan:
                    return $"{queryCondition.Column} < '{queryCondition.Content}'";
                case ConditionEnum.LessThanOrEqual:
                    return $"{queryCondition.Column} <= '{queryCondition.Content}'";
                case ConditionEnum.Like:
                    return $"{queryCondition.Column} like '%{queryCondition.Content}%'";
                case ConditionEnum.LikeLeft:
                    return $"{queryCondition.Column} like '{queryCondition.Content}%'";
                case ConditionEnum.LikeRight:
                    return $"{queryCondition.Column} like '%{queryCondition.Content}'";
                case ConditionEnum.NotEqual:
                    return $"{queryCondition.Column} <> '{queryCondition.Content}'";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 获取Between表达式
        /// </summary>
        /// <param name="strColumn"></param>
        /// <param name="strContent"></param>
        /// <returns></returns>
        private static string GetBetweenExpression(string strColumn, string strContent)
        {
            try
            {
                var start = strContent.Split(',')[0];
                var end = strContent.Split(',')[1];
                return $"{strColumn} between '{start}' and '{end}'";
            }
            catch 
            {
                throw new Exception("Between查询条件，数据格式错误");
            }
        }

        /// <summary>
        /// 获取In表达式
        /// </summary>
        /// <param name="strColumn"></param>
        /// <param name="strContent"></param>
        /// <returns></returns>
        private static string GetInExpression(string strColumn, string strContent)
        {
            try
            {
                var inString = strContent.Replace(",", "','");
                return $"{strColumn} in ('{inString}')";
            }
            catch 
            {
                throw new Exception("In查询条件，数据格式错误");
            }
        }

        /// <summary>
        /// 获取Order By委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strColumn"></param>
        /// <returns></returns>
        public static LambdaExpression GetOrderByLambda<T>(string strColumn)
        {
           return GetLambda<T>(strColumn);
        }

        private static LambdaExpression GetLambda<T>(string strColumn)
        {
            try
            {
                var param = Expression.Parameter(typeof(T), "p");
                var member = typeof(T).GetMembers().Where(p => p.Name.ToLower() == strColumn.ToLower()).First();
                var fieldExp = Expression.MakeMemberAccess(param, member);
                var lambda = Expression.Lambda(fieldExp, param);
                return lambda;
            }
            catch
            {
                return null;
            }
        }

        public static Expression<Func<T, object>> GetExpression<T>(string property)
        {
            try
            {
                var param = Expression.Parameter(typeof(T), "p");
                Expression body = param;

                body = Expression.PropertyOrField(body, property);

                var convert = Expression.Convert(body, typeof(object));
                return (Expression<Func<T, object>>)Expression.Lambda(convert, param);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
