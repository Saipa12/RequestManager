//using RequestManager.API.Dto;
//using RequestManager.API.Repositories;
//using RequestManager.Core.Handlers;

//namespace RequestManager.API.Handlers.GetUsers;

//public record GetUsersRequest;
//public record GetUsersResponse(IEnumerable<UserDto> Users);

//public class GetUsersHandler : IAsyncHandler<GetUsersRequest, GetUsersResponse>
//{
//    private readonly UserRepository _userRepository;

//    public GetUsersHandler(UserRepository userRepository)
//    {
//        _userRepository = userRepository;
//    }

//    public async Task<GetUsersResponse> Handle(GetUsersRequest request = default)
//    {
//        return new GetUsersResponse(await _userRepository.GetAsync());
//    }
//}