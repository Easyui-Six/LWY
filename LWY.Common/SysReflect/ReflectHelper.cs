using LWY.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LWY.Common
{
    /// <summary>
    /// 反射操作类
    /// </summary>    
    public class ReflectHelper
    {

        private static ReflectHelper _reflectHelper = new ReflectHelper();
        private ReflectHelper() { }
        public static ReflectHelper GetReflextHelper()
        {
            return _reflectHelper;
        }

        #region 加载程序集
        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称,不要加上程序集的后缀，如.dll</param>        
        public static Assembly LoadAssembly(string assemblyName)
        {
            try
            {
                return Assembly.Load(assemblyName);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                return null;
            }
        }
        #endregion

        #region 获取程序集中的类型
        /// <summary>
        /// 获取本地程序集中的类型
        /// </summary>
        /// <param name="typeName">类型名称，范例格式："命名空间.类名",类型名称必须在本地程序集中</param>        
        public static Type GetType(string typeName)
        {
            try
            {
                return Type.GetType(typeName);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取指定程序集中的类型
        /// </summary>
        /// <param name="assembly">指定的程序集</param>
        /// <param name="typeName">类型名称，范例格式："命名空间.类名",类型名称必须在assembly程序集中</param>
        /// <returns></returns>
        public static Type GetType(Assembly assembly, string typeName)
        {
            try
            {
                return assembly.GetType(typeName);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                return null;
            }
        }
        #endregion

        #region 动态创建对象实例
        /// <summary>
        /// 创建类型的实例
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static object CreateInstance(Type type, params object[] parameters)
        {
            return Activator.CreateInstance(type, parameters);
        }

        /// <summary>
        /// 创建类的实例
        /// </summary>
        /// <param name="className">类名，格式:"命名空间.类名"</param>
        /// <param name="parameters">传递给构造函数的参数</param>        
        public static object CreateInstance(string className, params object[] parameters)
        {
            try
            {
                //获取类型
                Type type = GetType(className);

                //类型为空则返回
                if (type == null)
                {
                    return null;
                }

                return CreateInstance(type, parameters);
            }
            catch (Exception ex)
            {
                string errMsg = ex.Message;
                return null;
            }
        }

        #endregion

        #region 获取类的命名空间
        /// <summary>
        /// 获取类的命名空间
        /// </summary>
        /// <typeparam name="T">类名或接口名</typeparam>        
        public static string GetNamespace<T>()
        {
            return typeof(T).Namespace;
        }
        #endregion

        #region 设置成员的值

        #region 设置属性值
        /// <summary>
        /// 将值装载到目标对象的指定属性中
        /// </summary>
        /// <param name="target">要装载数据的目标对象</param>
        /// <param name="propertyName">目标对象的属性名</param>
        /// <param name="value">要装载的值</param>
        public static void SetPropertyValue(object target, string propertyName, object value)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            SetValue(target, propertyInfo, value);
        }
        #endregion

        #region 设置成员的值
        /// <summary>
        /// 设置成员的值
        /// </summary>
        /// <param name="target">要装载数据的目标对象</param>
        /// <param name="memberInfo">目标对象的成员</param>
        /// <param name="value">要装载的值</param>
        private static void SetValue(object target, MemberInfo memberInfo, object value)
        {
            if (value != null)
            {
                //获取成员类型
                Type pType;
                if (memberInfo.MemberType == MemberTypes.Property)
                    pType = ((PropertyInfo)memberInfo).PropertyType;
                else
                    pType = ((FieldInfo)memberInfo).FieldType;

                //获取值的类型
                Type vType = GetPropertyType(value.GetType());

                //强制将值转换为属性类型
                value = CoerceValue(pType, vType, value);
            }
            if (memberInfo.MemberType == MemberTypes.Property)
                ((PropertyInfo)memberInfo).SetValue(target, value, null);
            else
                ((FieldInfo)memberInfo).SetValue(target, value);
        }
        #endregion

        #region 强制将值转换为指定类型
        /// <summary>
        /// 强制将值转换为指定类型
        /// </summary>
        /// <param name="propertyType">目标类型</param>
        /// <param name="valueType">值的类型</param>
        /// <param name="value">值</param>
        private static object CoerceValue(Type propertyType, Type valueType, object value)
        {
            //如果值的类型与目标类型相同则直接返回,否则进行转换
            if (propertyType.Equals(valueType))
            {
                return value;
            }
            else
            {
                if (propertyType.IsGenericType)
                {
                    if (propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        if (value == null)
                            return null;
                        else if (valueType.Equals(typeof(string)) && (string)value == string.Empty)
                            return null;
                    }
                    propertyType = GetPropertyType(propertyType);
                }

                if (propertyType.IsEnum && valueType.Equals(typeof(string)))
                    return Enum.Parse(propertyType, value.ToString());

                if (propertyType.IsPrimitive && valueType.Equals(typeof(string)) && string.IsNullOrEmpty((string)value))
                    value = 0;

                try
                {
                    return Convert.ChangeType(value, GetPropertyType(propertyType));
                }
                catch (Exception ex)
                {
                    TypeConverter cnv = TypeDescriptor.GetConverter(GetPropertyType(propertyType));
                    if (cnv != null && cnv.CanConvertFrom(value.GetType()))
                        return cnv.ConvertFrom(value);
                    else
                        throw ex;
                }
            }
        }
        #endregion

        #region 获取类型,如果类型为Nullable<>，则返回Nullable<>的基础类型
        /// <summary>
        /// 获取类型,如果类型为Nullable(of T)，则返回Nullable(of T)的基础类型
        /// </summary>
        /// <param name="propertyType">需要转换的类型</param>
        private static Type GetPropertyType(Type propertyType)
        {
            Type type = propertyType;
            if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>)))
                return Nullable.GetUnderlyingType(type);
            return type;
        }
        #endregion

        #region 设置标题的 通用方法

        //public static List<Columns> GetField_AttributeStr<T>() where T : class
        //{
        //    List<Columns> columns = new List<Columns>();
        //    Type t = GetPropertyType(typeof(T));
        //    PropertyInfo[] Prs = t.GetProperties();
        //    foreach (dynamic p in Prs)
        //    {
        //        object[] Fields = p.GetCustomAttributes(false);
        //        foreach (dynamic attrs in Fields)
        //        {
        //            if (attrs is FieldAttribute)
        //            {
        //                FieldAttribute column = attrs as FieldAttribute;
        //                var model = new Columns();
        //                model.field = column.Fields;
        //                model.title = column.Description;
        //                model.sortable = false;
        //                model.Sort = column.Sort;

        //                if (model.title == "排序" || model.title == "创建时间" || model.title == "修改时间")
        //                {
        //                    model.sortable = true;
        //                }//开放字段排序功能
        //                model.formatter = "function (value,row) { if(value.length>13) return '' + value.substring(0,13) + '' + '……';  else return ''+value+'';}";

        //                switch (column.IsEnable)
        //                {
        //                    case true:
        //                        columns.Add(model);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    return columns.OrderBy(x => x.Sort).ToList();
        //}

        //public static List<Columns> GetField_AttributeStrTree<T>() where T : class
        //{
        //    List<Columns> columns = new List<Columns>();
        //    Type t = GetPropertyType(typeof(T));
        //    PropertyInfo[] Prs = t.GetProperties();
        //    foreach (dynamic p in Prs)
        //    {
        //        object[] Fields = p.GetCustomAttributes(false);
        //        foreach (dynamic attrs in Fields)
        //        {
        //            if (attrs is FieldAttribute)
        //            {
        //                FieldAttribute column = attrs as FieldAttribute;
        //                var model = new Columns();
        //                model.field = column.Fields;
        //                model.title = column.Description;
        //                switch (column.IsEnable)
        //                {
        //                    case true:
        //                        columns.Add(model);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }
        //    }
        //    return columns;
        //}

        //public static List<T> PostList<T>(string url)
        //{
        //    var str_Object = new JavaScriptSerializer().Deserialize<List<T>>(Encoding.UTF8.GetString(new WebClient().UploadValues(url, "POST", new NameValueCollection { })));
        //    return str_Object;
        //}
        //public static string Str<T>() where T : class
        //{
        //    string Str = "";
        //    Type t = typeof(T);
        //    PropertyInfo[] Proper = t.GetProperties();
        //    foreach (PropertyInfo p in Proper)
        //    {
        //        Str += p.Name + ",";
        //    }
        //    return Str.TrimEnd(',');
        //}



        #endregion

        #endregion

        ///////////////////////////////////////////

        #region 获取成员的值

        public object GetPropertyValue(object target, string propertyName)
        {
            PropertyInfo propertyInfo = target.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

            return propertyInfo.GetValue(target);
        }

        #endregion

        #region   获取或设置 类的成员的特性


        /// <summary>
        /// 功能：返回对象类 中的成员的File属性集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<FieldViewModel> GetField_AttributeStrB<T>() where T : class
        {
            List<FieldViewModel> columns = new List<FieldViewModel>();

            Type t = GetPropertyType(typeof(T));
            PropertyInfo[] Prs = t.GetProperties();
            foreach (dynamic p in Prs)
            {
                if (p == null)
                {
                    continue;
                }
                object[] Fields = p.GetCustomAttributes(false);

                foreach (dynamic attrs in Fields)
                {
                    if (attrs is FieldAttribute)
                    {
                        FieldAttribute column = attrs as FieldAttribute;
                        var model = new FieldViewModel();
                        model.field = column.Fields;
                        model.Description = column.Description;
                        model.IsExport = column.IsExport;
                        model.IsImport = column.IsImport;
                        model.ExportWidth = column.ExportWidth;
                        model.Sort = column.Sort;
                        columns.Add(model);
                    }
                }
            }
            return columns.OrderBy(x => x.Sort).ToList();
        }

        #endregion

        #region  动态创建类

        /// <summary>
        /// 创建类型的实例
        /// </summary>
        /// <param name="type">类型</param>      
        public object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }



        #endregion

        #region  获取类的 描述


        /// <summary>
        /// 获取类里的描述字段
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<Member> GetMembers<T>() where T : class
        {
            List<Member> members = new List<Member>();

            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo property in properties)
            {
                object[] proDescrition = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (proDescrition.Length > 0)
                {
                    members.Add(new Member()
                    {
                        Name = property.Name,
                        Description = ((DescriptionAttribute)proDescrition[0]).Description
                    });
                }
            }
            return members;
        }

        #endregion  


        #region 用dll文件的名字  和要执行的方法做入参 取值   相当于new一个对象

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">出参</typeparam>
        /// <param name="dllName">dll的全称</param>
        /// <param name="className">类名</param>
        /// <param name="actionName">方法名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public T GetResultByDllName<T>(string dllName, string className, string actionName, object[] param)
        {
            //加载程序集(dll文件地址)，使用Assembly类   从bin文件下加载 
            Assembly assembly = Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + $"{dllName}.dll");
            var dlltype = assembly.GetTypes().Where(a => a.Name == className).FirstOrDefault();
            //获取类型，参数（名称空间+类）   
            Type type = assembly.GetType($"{dllName}.{className}");
            //创建该对象的实例，object类型，参数（名称空间+类）   
            object instance = assembly.CreateInstance($"{dllName}.{className}");

            var method = instance.GetType().GetMethod(actionName);//取到该dll的方法
            var result = (T)method.Invoke(instance, param);

            return result;

        }

        #endregion

        //反射调用本类中的公用方法
        //System.Reflection.MethodInfo methodInfo = GetType().GetMethod(actionName);
        //T doc =
        //    (T)methodInfo.Invoke(this,
        //        new object[] { param1, param2, param3, param4 }); //返回出参
    }

}
