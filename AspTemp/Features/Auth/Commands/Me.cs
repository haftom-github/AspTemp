using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Application.Contracts.Cqrs;
using AspTemp.Shared.Application.Contracts.ResultContracts;

namespace AspTemp.Features.Auth.Commands;

public record Me : IRRequest<UserProfileDto>;

public record UserProfileDto(
    Guid Id,
    string Username
);

public class MeHandler(
    ICurrentUserService currentUserService
) : IRRequestHandler<Me, UserProfileDto>
{
    public async Task<Result<UserProfileDto>> Handle(Me request, CancellationToken cancellationToken)
    {
        var user = await currentUserService.GetAsync(cancellationToken);
        if (user == null)
            return Failure.Unauthorized();

        return new UserProfileDto(
            user.Id,
            user.Email!
        );
    }
}