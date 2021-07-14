using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static LWY.Common.LambdaExpressionBuilder;

namespace LWY.Common
{
    public class Demo
    {
        #region  初始的 动态生成表达树的方法，只有等于 的类别
        public void demo1()
        {
            List<CollectMan> collectMans = new List<CollectMan>()
            { new CollectMan(){ OperCode = "demo1", CollectName = "haha" },
              new CollectMan(){ OperCode = "demo2", CollectName = "xixi" },
              new CollectMan(){ OperCode = "demo3", CollectName = "hengheng" },
             new CollectMan(){ OperCode = "demo4", CollectName = "jiji" }};
            Persion persion = new Persion()
            {
                OperCode = "dingding",
                CollectName = "jiji",
                OrgID = 4,
                dt = DateTime.Now.AddDays(3)
            };

            var compareExp = simpleCompare<CollectMan>("OperCode", "demo3");
            var daisys = collectMans.Where(compareExp).ToList();
        }

        public static Func<TSource, bool> simpleCompare<TSource>(string property, object value)
        {
            var type = typeof(TSource);
            var pe = Expression.Parameter(type, "p");
            var propertyReference = Expression.Property(pe, property);
            var constantReference = Expression.Constant(value);
            //compile 是表达式的一个接口，生成该lambda表达式树对的委托
            return Expression.Lambda<Func<TSource, bool>>(Expression.Equal(propertyReference, constantReference), pe).Compile();
        }
        #endregion

        #region  包含所有 操作符的 动态生成表达树

        public void demo2()
        {
            List<Persion> persions = new List<Persion>()
            { new Persion(){ OperCode = "demo1", CollectName = "haha",OrgID=1,dt=DateTime.Now },
              new Persion(){ OperCode = "demo3", CollectName = "xixi",OrgID=2,dt=DateTime.Now.AddDays(1) },
              new Persion(){ OperCode = "demo3", CollectName = "hengheng",OrgID=3,dt=DateTime.Now.AddDays(2) },
              new Persion(){ OperCode = "demo3", CollectName = "hengheng",OrgID=3,dt=DateTime.Now.AddDays(2) },
                new Persion(){ OperCode = "dingding", CollectName = "jiji",OrgID=4,dt=DateTime.Now.AddDays(3) }};

            FilterCollection filters = new FilterCollection();
            IList<Filter> filters1 = new List<Filter>();
            IList<Filter> filters2 = new List<Filter>();
            filters1.Add(new Filter()
            {
                PropertyName = "OperCode",
                Operation = Op.Contains,
                Value = "1"
            });
            filters2.Add(new Filter()
            {
                PropertyName = "CollectName",
                Operation = Op.Contains,
                Value = "h"
            });
            //filters1.Add(new Filter()
            //{
            //    PropertyName = "OrgID",
            //    Operation = Op.GreaterThanOrEqual,
            //    Value = 1
            //});
            //filters2.Add(new Filter()
            //{
            //    PropertyName = "OrgID",
            //    Operation = Op.LessThanOrEqual,
            //    Value = 5
            //});
            //filters1.Add(new Filter()
            //{
            //    PropertyName = "dt",
            //    Operation = Op.GreaterThan,
            //    Value = DateTime.Now
            //});
            //filters1.Add(new Filter()
            //{
            //    PropertyName = "dt",
            //    Operation = Op.LessThanOrEqual,
            //    Value = DateTime.Now.AddDays(8)
            //});
            filters.Add(filters1); filters.Add(filters2);

            var compareExp = LambdaExpressionBuilder.GetExpression<Persion>(filters).Compile();
            var tmpresultA = persions.Where(compareExp).ToList();




        }

        #endregion


        #region  Demo  所需的辅助对象
        public partial class CollectMan
        {
            /// <summary>
            /// 声明主键
            /// </summary>
            ///[ForeignKey(nameof(CollectID))]


            [Key]
            public int CollectID { get; set; }
            public int OrgID { get; set; }
            public string OperCode { get; set; }
            public string CollectName { get; set; }
            public string CollectSign { get; set; }
        }


        public partial class Persion
        {
            public int OrgID { get; set; }
            public string OperCode { get; set; }
            public string CollectName { get; set; }
            public System.DateTime dt { get; set; }
        }

        #endregion 
    }
}
