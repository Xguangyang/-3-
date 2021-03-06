using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Model.Entity.Contract
{
    /// <summary>
    /// 货主合同管理表
    /// </summary>
    public class OwnerOfCargoContract
    {
        /// <summary>
        /// 合同ID
        /// </summary>
        public int OwnerOfCargoContractID { get; set; }
        /// <summary>
        /// 合同编号
        /// </summary>
        public string OwnerOfCargoContractNo { get; set; }
        /// <summary>
        /// 合同标题
        /// </summary>
        public string OwnerOfCargoContractTitle { get; set; }
        /// <summary>
        /// 承运单位
        /// </summary>
        public string OwnerOfCargoContractUnit { get; set; }
        /// <summary>
        /// 货主负责人
        /// </summary>
        public string OwnerOfCargoContractName { get; set; }
        /// <summary>
        /// 路线
        /// </summary>
        public string CommonContractCircuit { get; set; }
        /// <summary>
        /// 吨运价
        /// </summary>
        public decimal TonRunPrice { get; set; }
        /// <summary>
        /// 包车条件吨位
        /// </summary>
        public int CharteredBusConditionTonNum { get; set; }
        /// <summary>
        /// 包车金额
        /// </summary>
        public int CharteredBusPrice { get; set; }
        /// <summary>
        /// 签订日期
        /// </summary>
        public DateTime DateOfSigningTime { get; set; }
        /// <summary>
        /// 合同标的或项目说明
        /// </summary>
        public string OwnerOfCargoContractRemark { get; set; }
        /// <summary>
        /// 合同主要条款/变更条款
        /// </summary>
        public string OwnerOfCargoContractPrice { get; set; }
        /// <summary>
        /// 合同文本（附件）
        /// </summary>
        public string OwnerOfCargoContractFile { get; set; }
        /// <summary>
        /// 经办人
        /// </summary>
        public string CircuitResponsibleName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public int CommonContractStatus { get; set; }
        /// <summary>
        /// 审核人
        /// </summary>
        public string CommonContractName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int OwnerOfCargoContractStatus { get; set; }
    }
}
