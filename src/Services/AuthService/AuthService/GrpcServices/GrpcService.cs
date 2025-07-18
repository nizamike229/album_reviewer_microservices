using AuthService.Context;
using Grpc.Core;
using GrpcAuthService;
using Microsoft.EntityFrameworkCore;

namespace AuthService.GrpcServices;

public class GrpcService : GetUsernameById.GetUsernameByIdBase
{
    private readonly AuthDbContext _context;

    public GrpcService(AuthDbContext context)
    {
        _context = context;
    }

    public override async Task<DataReply> SendData(DataRequest request, ServerCallContext context)
    {
        var username = (await _context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId))!.Username;
        return new DataReply
        {
            Username = username
        };
    }
}