using FusePortal.Application.Interfaces;
using MediatR;

namespace FusePortal.Application.Auth.LoginUser
{
    public class LoginUserCommandHandler(IAuthService auth) : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly IAuthService _auth = auth;

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
            => await _auth.LoginAsync(request.Login);
    }
}
