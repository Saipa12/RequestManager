using RequestManager.Database.Models.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace RequestManager.API.Dto;

public class UserDto : IdentityUser
{
}