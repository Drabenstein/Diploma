﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Dtos;
public class SubmitReviewDto
{
    public int ReviewId { get; set; }
    public FilledReviewModuleDto[] ReviewModules { get; set; }
    public string Grade { get; set; }
}

