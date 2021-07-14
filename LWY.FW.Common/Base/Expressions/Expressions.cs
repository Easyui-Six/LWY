using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWY.FW.Common
{
    /// <summary>
    /// linq 重写方法
    /// </summary>
    public static class ExpressionExtends
    {

        public static IOrderedEnumerable<TElement> SortBy<TElement, TValue>(this IQueryable<TElement> query, Dictionary<Func<TElement, TValue>, SortType> selector)
        {
            IOrderedEnumerable<TElement> result = null;
            if (selector == null || selector.Count <= 0)
                throw new ArgumentNullException("排序字段不能为空");
            int i = 0;
            foreach (var item in selector)
            {
                if (i > 0)
                {
                    if (item.Value == SortType.Asc)
                        result.ThenBy(item.Key);
                    else
                        result.ThenByDescending(item.Key);
                }
                else
                {
                    if (item.Value == SortType.Asc)
                        result = query.OrderBy(item.Key);
                    else
                        result = query.OrderByDescending(item.Key);
                }
                i++;
            }
            return result;
        }



    }

    //public class ReplaceExpressionVisitor : ExpressionVisitor
    //{
    //    private readonly Expression _oldValue;
    //    private readonly Expression _newValue;

    //    public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
    //    {
    //        _oldValue = oldValue;
    //        _newValue = newValue;
    //    }

    //    public override Expression Visit(Expression node)
    //    {
    //        if (node == _oldValue)
    //            return _newValue;
    //        return base.Visit(node);
    //    }
    //}

}
