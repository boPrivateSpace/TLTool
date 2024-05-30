using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FengXuTLTool
{
    public class HttpResult
    {

    }

    /// <summary>
    ///  执行返回结果
    /// </summary>
    public class ExecuteResult<T>
    {

        public string Status { get; set; }

        public Data<T> Data { get; set; }

    }

    public class Data<T>
    {

        /// <summary>
        /// 执行是否成功
        /// 默认为True
        /// </summary>
        public bool IsSucceed { get; set; }
        /// <summary>
        /// 执行信息（一般是错误信息）
        /// 默认置空
        /// </summary>
        public string Message { get; set; }

        public string Result { get; set; }

        public string PageResults { get; set; }

        public List<T> results { get; set; }


    }

    public class UserPermission: BaseEntity
    {
        /// <summary>
        /// 机器码
        /// </summary>
        public string MachineCode { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public string Permission { get; set; }

    }


    public abstract class BaseEntity 
    {
        public long Id { get; set; }
        public StatusCode StatusCode { get; set; }
        public long? Creator { get; set; }
        public DateTime? CreateTime { get; set; }
        public long? Modifyr { get; set; }
        public DateTime? ModifyTime { get; set; }
    }

    public enum StatusCode
    {
        Deleted = -1,//软删除，已删除的无法恢复，无法看见，暂未使用
        Enable = 0,
        Disable = 1//失效的还可以改为生效
    }
}
