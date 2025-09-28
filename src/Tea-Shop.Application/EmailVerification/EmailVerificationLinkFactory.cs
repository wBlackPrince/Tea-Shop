using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Tea_Shop.Domain;
using Tea_Shop.Shared;

namespace Tea_Shop.Application.EmailVerification;

public class EmailVerificationLinkFactory(
    IHttpContextAccessor httpContextAccessor,
    LinkGenerator linkGenerator)
{
    public string Create(EmailVerificationToken token)
    {
        string? verificationLink = linkGenerator.GetUriByName(
            httpContextAccessor.HttpContext!,
            Constants.VerifyEmail,
            new { token = token.Id });

        return verificationLink ?? throw new Exception("Could not create verification link");
    }
}