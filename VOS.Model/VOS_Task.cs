using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public enum TaskType
    {
        搜索单,
        隔天单,
        非搜单,
        动销单,
        其他
    }
    public enum Eweight
    {
        低,
        中,
        高
    }

    public enum OrderState
    {
        未分配,
        已分配,
        进行中,
        已完成,
        已返款
    }

    public class VOS_Task : PersistPoco
    {
        [Display(Name = "任务编号")]
        [Required(ErrorMessage = "任务编号不能为空")]
        [StringLength(20, ErrorMessage = "任务编号长度超过限制")]
        public string Task_no { get; set; }

        [Display(Name = "做单方法")]
        public TaskType TaskType { get; set; }

        [Display(Name = "计划编号")]
        public VOS_Plan Plan { get; set; }

        [Display(Name = "计划编号")]
        public Guid PlanId { get; set; }

        [Display(Name = "佣金折扣")]
        [Required(ErrorMessage = "佣金折扣不能为空")]
        public string ComDis { get; set; }

        [Display(Name = "客服")]
        public string CustomerService { get; set; }

        [Display(Name = "联系方式")]
        public string Contact { get; set; }

        [Display(Name = "运营负责人")]
        public string ShopCharge { get; set; }

        [Display(Name = "运营电话")]
        public string ShopChargeContact { get; set; }

        [Display(Name = "执行开始")]
        public DateTime ImplementStartTime { get; set; }

        [Display(Name = "执行结束")]
        public DateTime ImplementEndTime { get; set; }

        [Display(Name = "商品类目")]
        public Category TaskCate { get; set; }
        [Display(Name = "商品类目")]
        public Guid? TaskCateId { get; set; }

        [Display(Name = "商品名称")]
        [Required(ErrorMessage = "商品名称不能为空")]
        public string CommodityName { get; set; }

        [Display(Name = "商品链接")]
        [Required(ErrorMessage = "商品链接不能为空")]
        public string CommodityLink { get; set; }


        [Display(Name = "商品图片")]
        public FileAttachment CommodityPic { get; set; }
        [Display(Name = "商品图片")]
        public Guid? CommodityPicId { get; set; }


        [Display(Name = "商品权重")]
        public Eweight Eweight { get; set; }

        [Display(Name = "任务分")]
        public string TaskFen { get; set; }

        [Display(Name = "商品价格")]
        [Required(ErrorMessage = "商品价格不能为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的商品价格")]
        public string CommodityPrice { get; set; }

        [Display(Name = "公司佣金")]
        [Required(ErrorMessage = "公司佣金不能为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的公司佣金")]
        public string Commission { get; set; }

        [Display(Name = "其他费用")]
        [Required(ErrorMessage = "其他费用不能为空")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的其他费用")]
        public string OtherExpenses { get; set; }

        [Display(Name = "其他要求")]
        public string ORequirement { get; set; }

        [Display(Name = "做单要求")]
        public string TRequirement { get; set; }

        [Display(Name = "客服备注")]
        public string CRemarks { get; set; }

        [Display(Name = "区域要求")]
        public string AreaRequirement { get; set; }

        [Display(Name = "是否TP")]
        public bool IsTP { get; set; }

        [Display(Name = "搜索关键字")]
        public string SearchKeyword { get; set; }

        [Display(Name = "成交关键字")]
        public string DealKeyword { get; set; }

        [Display(Name = "SKU")]
        public string SKU { get; set; }

        [Display(Name = "是否解锁")]
        public bool IsLock { get; set; }

        [Display(Name = "解锁人")]
        public Guid? UnlockerId { get; set; }

        [Display(Name = "解锁人")]
        public VOS_User Unlocker { get; set; }

        [Display(Name = "解锁时间")]
        public DateTime? UnlockTime { get; set; }

        [Display(Name = "执行人")]
        public Guid? ExecutorId { get; set; }

        [Display(Name = "执行人")]
        public VOS_User Executor { get; set; }

        [Display(Name = "分配人")]
        public Guid? DistributorId { get; set; }

        [Display(Name = "分配人")]
        public VOS_User Distributor { get; set; }
        [Display(Name = "分配时间")]
        public DateTime? DistributionTime { get; set; }
        [Display(Name = "刷手")]
        public Guid? EmployeeId { get; set; }
        [Display(Name = "刷手")]
        public VOS_PEmployee Employee { get; set; }

        [Display(Name = "刷手佣金")]
        [RegularExpression(@"(?!^0*(\.0{1,2})?$)^\d{1,13}(\.\d{1,2})?$", ErrorMessage = "请输入正确的刷手佣金")]
        public string EmployeeCommission { get; set; }
        [Display(Name = "刷单单号")]
        public string VOrderCode { get; set; }
        [Display(Name = "手机")]
        public string Phone { get; set; }
        [Display(Name = "淘宝账号")]
        public string TBAccount { get; set; }
        [Display(Name = "微信账号")]
        public string WXAccount { get; set; }
        [Display(Name = "完成人")]
        public Guid? CompleterId { get; set; }

        [Display(Name = "完成人")]
        public VOS_User Completer { get; set; }
        [Display(Name = "完成时间")]
        public DateTime? CompleteTime { get; set; }
        [Display(Name = "返款人")]
        public Guid? RefunderId { get; set; }

        [Display(Name = "返款人")]
        public VOS_User Refunder { get; set; }
        [Display(Name = "返款时间")]
        public DateTime? RefundTime { get; set; }
        [Display(Name = "任务状态")]
        public OrderState OrderState { get; set; }

        [Display(Name = "单量")]
        [NotMapped]
        [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "请输入正确单量")]
        public int? VOS_Number { get; set; }
        [Display(Name = "Base64")]
        [NotMapped]
        public string base64 { get; set; }
    }
}
