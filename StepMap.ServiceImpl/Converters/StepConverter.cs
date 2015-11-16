using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = StepMap.ServiceContracts.DTO;
using dal = StepMap.DAL;

namespace StepMap.ServiceImpl.Converters
{
    public static class StepConverter
    {
        public static dto.Step ConvertStep(dal.Step step)
        {
            dto.Step ret = new dto.Step();
            ret.Id = step.Id;
            ret.ProjectId = step.ProjectId;
            ret.Deadline = step.Deadline;
            ret.FinishDate = step.FinishDate;
            ret.Name = step.Name;
            ret.SentReminder = step.SentReminder;
            return ret;
        }

        public static dal.Step ConvertStep(dto.Step step)
        {
            dal.Step ret = new dal.Step();
            ret.Id = step.Id;
            ret.ProjectId = step.ProjectId;
            ret.Deadline = step.Deadline;
            ret.FinishDate = step.FinishDate;
            ret.Name = step.Name;
            ret.SentReminder = step.SentReminder;
            return ret;
        }

    }
}
