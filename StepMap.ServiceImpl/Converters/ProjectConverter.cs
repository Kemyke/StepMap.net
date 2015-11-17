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
            ret.Id = project.Id;
            ret.UserId = project.UserId;
            ret.BadPoint = project.BadPoint;
            ret.GoodPoint = project.GoodPoint;
            ret.Name = project.Name;
            ret.StartDate = project.StartDate;
            ret.Position = project.Position;

            ret.FinishedSteps = project.FinishedSteps.Select(s => StepConverter.ConvertStep(s)).ToList();
            return ret;
        }

        public static dal.Project ConvertProject(dto.Project project)
        {
            dal.Project ret = new dal.Project();
            ret.Id = project.Id;
            ret.UserId = project.UserId;
            ret.BadPoint = project.BadPoint;
            ret.GoodPoint = project.GoodPoint;
            ret.Name = project.Name;
            ret.StartDate = project.StartDate;
            ret.Position = project.Position;

            ret.FinishedSteps = project.FinishedSteps.Select(s => StepConverter.ConvertStep(s)).ToList();
            return ret;
        }
    }
}
