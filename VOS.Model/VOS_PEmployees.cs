using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WalkingTec.Mvvm.Core;

namespace VOS.Model
{
    public class VOS_PEmployee : PersistPoco
    {
        public enum state 
        {
            正常=0,
            休息=1,
            黑名单=2
        }
        public enum EnumSex
        {
            男,女
        }
        [Display(Name = "推荐人")]
        public VOS_PEmployee Recom { get; set; }
        [Display(Name = "推荐人")]
        public Guid? RecomId { get; set; }

        [Display(Name = "区域")]
        public City Area { get; set; }
        [Display(Name = "区域")]
        public Guid? AreaId { get; set; }

        [Display(Name = "地址")]
        [Required(ErrorMessage = "地址不能为空")]
        [StringLength(100,ErrorMessage ="地址长度超过限制")]
        public string Address { get; set; }
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "姓名不能为空")]
        [StringLength(20, ErrorMessage = "姓名长度超过限制")]
        public string FullName { get; set; }
        [Display(Name = "性别")]
        public EnumSex? Sex { get; set; }
        [Display(Name = "联系电话")]
        [Required(ErrorMessage = "联系电话不能为空")]
        //[RegularExpression(@"^1[3|4|5|7|8][0-9]{9}$",ErrorMessage ="请输入正确的手机号")]
        [RegularExpression(@"^1[0-9]{10}$",ErrorMessage ="请输入正确的手机号")]
        public string Mobile { get; set; }
        [Display(Name = "微信账号")]
        [Required(ErrorMessage = "微信账号不能为空")]
        [StringLength(50, ErrorMessage = "微信账号长度超过限制")]
        public string WeChat { get; set; }
        [Display(Name = "淘宝账号")]
        [Required(ErrorMessage = "淘宝账号必填项")]
        [StringLength(50, ErrorMessage = "淘宝账号长度超过限制")]
        public string TaobaAccount { get; set; }
        [Display(Name = "京东账号")]
        [StringLength(50, ErrorMessage = "京东账号长度超过限制")]
        public string JDAccount { get; set; }
        [Display(Name = "支付宝")]
        public Guid? AlipayPicId { get; set; }
        [Display(Name = "支付宝")]
        public FileAttachment AlipayPic { get; set; }
        [Display(Name = "微信")]
        public Guid? WeChatPicId { get; set; }
        [Display(Name = "微信")]
        public FileAttachment WeChatPic { get; set; }
        [Display(Name = "微信实名")]
        public Guid? WeChatRealNamePicId { get; set; }
        [Display(Name = "微信实名")]
        public FileAttachment WeChatRealNamePic { get; set; }
        [Display(Name ="QQ")]
        [StringLength(20, ErrorMessage = "QQ账号长度超过限制")]
        public string QQAccount { get; set; }
        [Display(Name = "备注")]
        public string Remark { get; set; }
        [Display(Name ="刷手状态")]
        public state PEstate { get; set; }
        
        [NotMapped]
        public bool button_show { get; set; }

        [Display(Name = "组织机构")]
        public Guid OrganizationID { get; set; }

        [Display(Name = "组织机构")]
        public VOS_Organization Organization { get; set; }
    }
}
