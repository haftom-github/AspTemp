using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Cqrs;
using AspTemp.Shared.ResultContracts;

namespace AspTemp.Features.Auth.Commands;

public record Me : IRRequest<UserProfileDto>;

public record UserProfileDto(
    Guid Id,
    string Username
);

public class MeHandler(
    ICurrentUserService currentUserService,
    IUserRepo userRepo
) : IRRequestHandler<Me, UserProfileDto>
{
    public async Task<Result<UserProfileDto>> Handle(Me request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.GetUserId();
        var user = userId.HasValue 
            ? await userRepo.GetByIdAsync(userId.Value, cancellationToken) 
            : null;
        
        if (user == null)
            return Failure.Unauthorized();

        return new UserProfileDto(
            user.Id,
            user.Email!
        );
    }
}