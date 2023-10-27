﻿using RequestManager.API.Common;
using System.ComponentModel.DataAnnotations;

namespace RequestManager.API.Dto;

public abstract class DatabaseEntity : IMapFrom<Database.Models.Common.DatabaseEntity>
{
    public long Id { get; set; }
}