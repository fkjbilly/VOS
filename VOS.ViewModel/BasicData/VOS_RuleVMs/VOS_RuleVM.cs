﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WalkingTec.Mvvm.Core;
using WalkingTec.Mvvm.Core.Extensions;
using VOS.Model;


namespace VOS.ViewModel.BasicData.VOS_RuleVMs
{
    public partial class VOS_RuleVM : BaseCRUDVM<VOS_Rule>
    {

        public VOS_RuleVM()
        {
        }

        protected override void InitVM()
        {
        }

        public override void DoAdd()
        {           
            base.DoAdd();
        }

        public override void DoEdit(bool updateAllFields = false)
        {
            base.DoEdit(updateAllFields);
        }

        public override void DoDelete()
        {
            var _Rule = DC.Set<VOS_Rule>().Where(x => x.ID.Equals(Entity.ID)).SingleOrDefault();
            DC.Set<VOS_Rule>().Update(_Rule).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            DC.SaveChanges();
            //base.DoDelete();
        }

        public override DuplicatedInfo<VOS_Rule> SetDuplicatedCheck()
        {
            var rv = CreateFieldsInfo(SimpleField(a => a.RuleName));
            rv.AddGroup(SimpleField(a => a.RuleType));
            return rv;
        }
    }
}
