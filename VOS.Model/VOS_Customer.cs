using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class VOS_Customer : PersistPoco
    {
        public enum level
        {
            普通客户,
            VIP客户,
            VVIP客户
        }
        public enum flag
        {
            正常,
            流失,
            已删除
        }

        public enum linksex { 男,女}

        [Display(Name = "客户编号")]
        [Required(ErrorMessage = "客户编号不能为空")]
        [StringLength(20, ErrorMessage = "客户编号长度超过限制")]
        public string cust_no { get; set; }
        [Display(Name = "客户名称")]
        [Required(ErrorMessage = "客户名称不能为空")]
        [StringLength(50, ErrorMessage = "客户名称长度超过限制")]
        public string cust_name { get; set; }
        [Display(Name = "客户地区")]
        public City cust_region { get; set; }
        [Display(Name = "客户地区")]
        public Guid? cust_regionId { get; set; }
        [Display(Name = "客户地址")]
        [StringLength(100, ErrorMessage = "客户地址长度超过限制")]
        public string cust_address { get; set; }
        [Display(Name = "客户等级")]
        public level cust_level { get; set; }
        [Display(Name = "客户电话")]
        [RegularExpression(@"^((\d{11})|(\d{7,8})|(\d{4}|\d{3})-(\d{7,8}))$", ErrorMessage = "请输入正确的联系方式")]
        public string cust_telephone { get; set; }
        [Display(Name = "客户标识")]
        public flag cust_flag { get; set; }
        [Display(Name = "联系人")]
        [StringLength(20, ErrorMessage = "联系人长度超过限制")]
        public string link_name { get; set; }
        [Display(Name = "性别")]
        public linksex link_sex { get; set; }
        [Display(Name = "职位")]
        [StringLength(20, ErrorMessage = "职位长度超过限制")]
        public string link_position { get; set; }
        [Display(Name = "手机")]
        [RegularExpression(@"^1[3|4|5|7|8][0-9]{9}$", ErrorMessage = "请输入正确的手机号")]
        public string link_mobile { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name ="组织机构")]
        public Guid? DistributionID { get; set; }
        [Display(Name = "组织机构")]
        public VOS_Distribution Distribution { get; set; }
    }
}
