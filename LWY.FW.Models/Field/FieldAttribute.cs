using System;


namespace LWY.FW.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class FieldAttribute : Attribute
    {
        private string _Fields; //字段
        private string _Description;// 字段描述(导入 的列名或者前端的heard都可以使用该字段)
        private bool _IsImport;// 是否可以导入
        private bool _IsExport;//是否可以导出
        private double _ExportWidth;//导出列对应的宽度
        private int _Sort; // 导入或导出的顺序


        public string Fields
        {
            get { return _Fields; }

        }
        public bool IsImport
        {
            get { return _IsImport; }

        }
        public bool IsExport
        {
            get { return _IsExport; }

        }

        public double ExportWidth
        {
            get { return _ExportWidth; }
        }
        public string Description
        {
            get { return _Description; }

        }

        public int Sort
        {
            get { return _Sort; }
            set { _Sort = value; }
        }


        /// <summary>
        /// 这里为了 约束字段，统一为有值的数据类型
        /// </summary>
        /// <param name="fields">字段</param>
        /// <param name="description">字段描述</param>
        /// <param name="isImport">是否可以导入</param>
        /// <param name="isExport">是否可以导出</param>
        /// <param name="exportWidth">导出列对应的宽度</param>
        /// <param name="sort">导入或导出的顺序 如果导入和导出的顺序不一致，则可以再添加一个 导出排序属性</param>
        public FieldAttribute(string fields, string description, bool isImport, bool isExport, double exportWidth, int sort)
        {
            _Fields = fields;
            _Description = description;
            _IsImport = isImport;
            _IsExport = isExport;
            _ExportWidth = exportWidth;
            this.Sort = sort;
        }
    }

    public class FieldViewModel
    {
        /// <summary>
        /// 字段
        /// </summary>
        public string field;
        /// <summary>
        /// 字段描述  可做导出列明 前端界面title 使用
        /// </summary>
        public string Description;
        /// <summary>
        /// 是否导入
        /// </summary>
        public bool IsImport;
        /// <summary>
        /// 是否导出
        /// </summary>
        public bool IsExport;

        /// <summary>
        /// 导出字段对应的列宽
        /// </summary>
        public double ExportWidth;
        /// <summary>
        /// 导入导出 的顺序
        /// </summary>
        public int Sort;

    }

    public class Member
    {
        /// <summary>
        /// 类字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类字段描述
        /// </summary>
        public string Description { get; set; }
    }
}
