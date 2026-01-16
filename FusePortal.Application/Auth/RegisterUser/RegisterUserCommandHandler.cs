using FusePortal.Application.Interfaces;
using MediatR;

namespace FusePortal.Application.Auth.RegisterUser
{
    public class RegisterUserCommandHandler(IAuthService auth) : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IAuthService _auth = auth;

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
            => await _auth.RegisterAsync(request.Register);
    }
}
