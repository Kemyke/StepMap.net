﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StepMap.ServiceContracts.DTO;

namespace StepMap.WebClient.ViewModels
{
    public class UserStepMapViewModel
    {
        public string UserName { get; set; }
        public IList<Project> Projects { get; set; }
    }
}