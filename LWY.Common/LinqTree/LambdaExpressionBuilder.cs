using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace LWY.Common
{

    #region  实现类
    /// <summary>
    /// 动态 lambda表达式
    /// </summary>
    public static class LambdaExpressionBuilder
    {
        private static MethodInfo containsMethod = typeof(string).GetMethod("Contains", new Type[] { typeof(string) });
        private static MethodInfo startsWithMethod =
                                typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) });
        private static MethodInfo endsWithMethod =
                                typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) });
        private static Expression GetExpression(ParameterExpression param, Filter filter)
        {
            MemberExpression member = Expression.Property(param, filter.PropertyName);
            Expression handledMember = member;
            ConstantExpression constant = Expression.Constant(filter.Value);

            if (member.Member.MemberType == MemberTypes.Property)
            {
                Type propertyType = ((PropertyInfo)member.Member).PropertyType;
                if (propertyType == typeof(string))
                {
                    handledMember = Expression.Call(member, typeof(string).GetMethod("ToLower", System.Type.EmptyTypes));
                }
                if (propertyType == typeof(DateTime?))
                {
                    handledMember = Expression.Property(member, typeof(DateTime?).GetProperty("Value"));
                }
            }

            switch (filter.Operation)
            {
                case Op.Equals:
                    return Expression.Equal(handledMember, constant);
                case Op.GreaterThan:
                    return Expression.GreaterThan(handledMember, constant);
                case Op.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(handledMember, constant);
                case Op.LessThan:
                    return Expression.LessThan(handledMember, constant);
                case Op.LessThanOrEqual:
                    return Expression.LessThanOrEqual(handledMember, constant);
                case Op.Contains:
                    return Expression.Call(handledMember, containsMethod, constant);
                case Op.StartsWith:
                    return Expression.Call(handledMember, startsWithMethod, constant);
                case Op.EndsWith:
                    return Expression.Call(handledMember, endsWithMethod, constant);
            }

            return null;
        }
        private static BinaryExpression GetORExpression(ParameterExpression param, Filter filter1, Filter filter2)
        {
            Expression bin1 = GetExpression(param, filter1);
            Expression bin2 = GetExpression(param, filter2);

            return Expression.Or(bin1, bin2);
        }

        private static Expression GetExpression(ParameterExpression param, IList<Filter> orFilters)
        {
            if (orFilters.Count == 0)
                return null;

            Expression exp = null;

            if (orFilters.Count == 1)
            {
                exp = GetExpression(param, orFilters[0]);
            }
            else if (orFilters.Count == 2)
            {
                exp = GetORExpression(param, orFilters[0], orFilters[1]);
            }
            else
            {
                while (orFilters.Count > 0)
                {
                    var f1 = orFilters[0];
                    var f2 = orFilters[1];

                    if (exp == null)
                    {
                        exp = GetORExpression(param, orFilters[0], orFilters[1]);
                    }
                    else
                    {
                        exp = Expression.Or(exp, GetORExpression(param, orFilters[0], orFilters[1]));
                    }
                    orFilters.Remove(f1);
                    orFilters.Remove(f2);

                    if (orFilters.Count == 1)
                    {
                        exp = Expression.Or(exp, GetExpression(param, orFilters[0]));
                        orFilters.RemoveAt(0);
                    }
                }
            }

            return exp;
        }

        public static Expression<Func<T, bool>> GetExpression<T>(FilterCollection filters)
        {
            if (filters == null || filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(typeof(T), "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression(param, filters[0]);
            }
            else if (filters.Count == 2)
            {
                exp = Expression.AndAlso(GetExpression(param, filters[0]), GetExpression(param, filters[1]));
            }

            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];
                    var f1Andf2 = Expression.AndAlso(GetExpression(param, filters[0]), GetExpression(param, filters[1]));
                    if (exp == null)
                    {
                        exp = f1Andf2;
                    }
                    else
                    {
                        exp = Expression.AndAlso(exp, f1Andf2);
                    }

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<T, bool>>(exp, param);
        }

        public static Expression<Func<TSource, bool>> GetExpressionVB<TSource>(FilterCollection filters)
        {
            var type = typeof(TSource);
            if (filters == null || filters.Count == 0)
                return null;

            ParameterExpression param = Expression.Parameter(type, "t");
            Expression exp = null;

            if (filters.Count == 1)
            {
                exp = GetExpression(param, filters[0]);
            }
            else if (filters.Count == 2)
            {
                exp = Expression.AndAlso(GetExpression(param, filters[0]), GetExpression(param, filters[1]));
            }

            else
            {
                while (filters.Count > 0)
                {
                    var f1 = filters[0];
                    var f2 = filters[1];
                    var f1Andf2 = Expression.AndAlso(GetExpression(param, filters[0]), GetExpression(param, filters[1]));
                    if (exp == null)
                    {
                        exp = f1Andf2;
                    }
                    else
                    {
                        exp = Expression.AndAlso(exp, f1Andf2);
                    }

                    filters.Remove(f1);
                    filters.Remove(f2);

                    if (filters.Count == 1)
                    {
                        exp = Expression.AndAlso(exp, GetExpression(param, filters[0]));
                        filters.RemoveAt(0);
                    }
                }
            }

            return Expression.Lambda<Func<TSource, bool>>(exp, param);
        }



        #endregion

        #region 辅助对象
        public class FilterCollection : Collection<IList<Filter>>
        {
            public FilterCollection()
                : base()
            { }
        }
        /// <summary>
        /// 动态构建linq表达树时  所需的过滤对象
        /// </summary>
        public class Filter
        {
            /// <summary>
            /// 字段名，操作符 左侧的字段
            /// </summary>
            public string PropertyName { get; set; }
            /// <summary>
            /// 操作符， 大于 等于 小于  包含  
            /// </summary>
            public Op Operation { get; set; }
            /// <summary>
            /// 操作符 右边的值
            /// </summary>
            public object Value { get; set; }
        }

        public enum Op
        {
            /// <summary>
            /// 等于
            /// </summary>
            Equals,
            /// <summary>
            /// 大于
            /// </summary>
            GreaterThan,
            /// <summary>
            /// 小于
            /// </summary>
            LessThan,
            /// <summary>
            /// 大于等于
            /// </summary>
            GreaterThanOrEqual,
            /// <summary>
            /// 小于等于
            /// </summary>
            LessThanOrEqual,
            /// <summary>
            /// 包含
            /// </summary>
            Contains,
            /// <summary>
            ///开始于 
            /// </summary>
            StartsWith,
            /// <summary>
            /// 结束于
            /// </summary>
            EndsWith
        }

        #endregion



    }
}
