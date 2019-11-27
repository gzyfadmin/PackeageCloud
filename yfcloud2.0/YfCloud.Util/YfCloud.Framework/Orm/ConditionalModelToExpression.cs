using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using SqlSugar;
using YfCloud.Utilities.WebApi;

namespace YfCloud.Framework.Orm
{
    public class ConditionalModelToExpression
    {
        private static readonly MethodInfo stringContainsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

        private static readonly MethodInfo startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });

        private static readonly MethodInfo endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });

        static readonly Dictionary<ConditionalType, Func<Expression, Expression, Expression>> Expressions;

        static ConditionalModelToExpression()
        {
            Expressions = new Dictionary<ConditionalType, Func<Expression, Expression, Expression>>();
            Expressions.Add(ConditionalType.Equal,(member, constant) => Expression.Equal(member, constant));
            Expressions.Add(ConditionalType.Like, (member, constant) => Expression.Call(member, stringContainsMethod, constant));
            Expressions.Add(ConditionalType.GreaterThan,(member, constant) => Expression.GreaterThan(member, constant));
            Expressions.Add(ConditionalType.GreaterThanOrEqual,(member, constant) => Expression.GreaterThanOrEqual(member, constant));
            Expressions.Add(ConditionalType.LessThan,(member, constant) => Expression.LessThan(member, constant));
            Expressions.Add(ConditionalType.LessThanOrEqual,(member, constant) => Expression.LessThanOrEqual(member, constant));

            Expressions.Add(ConditionalType.In, (member, constant) => GetIn_Expression(member, constant));
            Expressions.Add(ConditionalType.NotIn, (member, constant) => Get_NotInExpression(member, constant));

            Expressions.Add(ConditionalType.LikeLeft, (member, constant) => Expression.Call(member, startsWithMethod, constant));

            Expressions.Add(ConditionalType.LikeRight, (member, constant) => Expression.Call(member, endsWithMethod, constant));

            Expressions.Add(ConditionalType.NoEqual, (member, constant) => Expression.NotEqual(member, constant));

            Expressions.Add(ConditionalType.IsNullOrEmpty, (member, constant) => GetIsNullOrEmpty_Expression(member,constant));
            Expressions.Add(ConditionalType.NoLike, (member, constant) => Expression.Not(Expression.Call(member, stringContainsMethod, constant)));
        }

        private static Expression GetIsNullOrEmpty_Expression(Expression member, Expression constant1)
        {
            Expression exprNull = Expression.Constant(null);
            Expression exprEmpty = Expression.Constant(string.Empty);
            return Expression.OrElse(
                Expression.Equal(member, exprNull),
                Expression.AndAlso(
                    Expression.NotEqual(member, exprNull),
                    Expression.Equal(((MemberExpression)member).TrimToLower(), exprEmpty)));
        }

        private static Expression GetIn_Expression(Expression member, Expression constant1)
        {
            ConstantExpression factconstant1 = constant1 as ConstantExpression;
            if (!(factconstant1.Value is IList) || !factconstant1.Value.GetType().IsGenericType)
            {
                throw new ArgumentException("The 'In' operation only supports lists as parameters.");
            }

            var type = factconstant1.Value.GetType();
            var inInfo = type.GetMethod("Contains", new[] { type.GetGenericArguments()[0] });

            return GetExpressionHandlingNullables((MemberExpression)member, factconstant1, type, inInfo) ?? Expression.Call(constant1, inInfo, member);
        }

        private static Expression GetExpressionHandlingNullables(MemberExpression member, ConstantExpression constant1, Type type,

            MethodInfo inInfo)
        {
            var listUnderlyingType = Nullable.GetUnderlyingType(type.GetGenericArguments()[0]);
            var memberUnderlingType = Nullable.GetUnderlyingType(member.Type);
            if (listUnderlyingType != null && memberUnderlingType == null)
            {
                return Expression.Call(constant1, inInfo, member.Expression);
            }

            return null;
        }

        private static Expression Get_NotInExpression(Expression member, Expression constant1)
        {
            ConstantExpression factconstant1 = constant1 as ConstantExpression;

            if (!(factconstant1.Value is IList) || !factconstant1.Value.GetType().IsGenericType)
            {
                throw new ArgumentException("The 'NotIn' operation only supports lists as parameters.");
            }

            var type = factconstant1.Value.GetType();
            var inInfo = type.GetMethod("Contains", new[] { type.GetGenericArguments()[0] });
            var contains = Expression.Call(constant1, inInfo, member);
            return Expression.Not(contains);
        }

        /// <summary>
        /// 转化为表达式目录树
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditionalModelList"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> BuildExpression<T>(List<ConditionalModel> conditionalModelList)
        {
            Expression finalExpression = Expression.Constant(true);
            var parameter = Expression.Parameter(typeof(T), "t");

            foreach (var statement in conditionalModelList)
            {
                var member = parameter.GetMemberExpression(statement.FieldName);
                if (member.Type == typeof(bool))
                {
                    if (statement.FieldValue == "1")
                    {
                        var constant = Expression.Constant(true);

                        var expression = Expressions[statement.ConditionalType].Invoke(member, constant);
                        finalExpression = Expression.AndAlso(finalExpression, expression);
                    }
                    else
                    {
                        var constant = Expression.Constant(false);

                        var expression = Expressions[statement.ConditionalType].Invoke(member, constant);
                        finalExpression = Expression.AndAlso(finalExpression, expression);
                    }
                }
                else
                {
                    var constant = Expression.Constant(statement.FieldValue);

                    var expression = Expressions[statement.ConditionalType].Invoke(member, constant);

                    finalExpression = Expression.AndAlso(finalExpression, expression);
                }
            }

            return Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
        }

        
    }
}
