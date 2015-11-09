using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dto = StepMap.ServiceContracts.DTO;
using dal = StepMap.DAL;

namespace StepMap.ServiceImpl.Converters
{
    public static class ProjectConverter
    {
        public static dto.Project ConvertProject(dal.Project project)
        {
            dto.Project ret = new dto.Project();
            ret.BadPoint = project.BadPoint;
            ret.GoodPoint = project.GoodPoint;
            ret.Name = project.Name;
            ret.StartDate = project.StartDate;

            ret.NextStep = project.NextStep == null ? null : StepConverter.ConvertStep(project.NextStep);
            ret.FinishedSteps = project.FinishedSteps.Select(s => StepConverter.ConvertStep(s)).ToList();
            return ret;
        }
    }
}
