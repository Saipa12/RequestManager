using Microsoft.AspNetCore.Identity;
using RequestManager.API.Common;
using RequestManager.Database.Models;

namespace RequestManager.API.Dto;

public class UserDto : IdentityUser, IMapFrom<User>
{
}