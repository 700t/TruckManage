using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace MS.Common.Extensions
{
    public static class SortExtension
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source, bool isDescending, string property)
        {
            if (isDescending)
            {
                return source.OrderByDescending(x => x.GetReflectedPropertyValue(property));
            }
            else
            {
                return source.OrderBy(x => x.GetReflectedPropertyValue(property));
            }
        }

        private static object GetReflectedPropertyValue(this object subject, string property)
        {
            return subject.GetType().GetProperty(property).GetValue(subject, null);
        }

        /// <summary>
        /// 根据指定属性名称对序列进行排序
        /// </summary>
        /// <typeparam name="TSource">source中的元素的类型</typeparam>
        /// <param name="source">一个要排序的值序列</param>
        /// <param name="property">属性名称</param>
        /// <param name="descending">是否降序</param>
        /// <returns></returns>
        public static IQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string property, bool descending) where TSource : class
        {
            ParameterExpression param = Expression.Parameter(typeof(TSource), "c");
            PropertyInfo pi = typeof(TSource).GetProperty(property);
            MemberExpression selector = Expression.MakeMemberAccess(param, pi);
            LambdaExpression le = Expression.Lambda(selector, param);
            string methodName = (descending) ? "OrderByDescending" : "OrderBy";
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TSource), pi.PropertyType }, source.Expression, le);
            return source.Provider.CreateQuery<TSource>(resultExp);

        }
    }
}
