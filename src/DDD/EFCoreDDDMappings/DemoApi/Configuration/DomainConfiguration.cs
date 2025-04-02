using System.Security.Claims;
using DemoApi.Rules;
using DemoApi.Rules.Implementation;
using DemoApi.Endpoints.Rules;
using System.Globalization;
using DemoApi.Common;
using DemoApi.Models;

namespace DemoApi.Configuration;

static class DomainConfiguration
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddScoped<Func<ClaimsPrincipal>>(sp =>
        {
            var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
            return () => httpContextAccessor.HttpContext?.User ?? new ClaimsPrincipal();
        });

        services.AddSingleton<ITitleValidity>(svc =>
            new AllTitleValidity(
                // new UserRoleTitleRule(svc.GetRequiredService<Func<ClaimsPrincipal>>(), "Editor"),
                new NonEmptyTitleRule(),
                new TitleWhitespaceRule(),
                new TitleCaseRule()));

        services.AddSingleton<PersonalNameToSlug>(_ => (culture, name) =>
            new Handle(name.First, name.Last)
                .Transform(
                    HandleTransforms.ToLowercase(culture),
                    HandleTransforms.IntoLetterAndDigitRuns)
                .ToSlug(HandleToSlugConversions.Hyphenate));

        services.AddSingleton<BookTitleToSlug>(_ => (culture, title) =>
            new Handle(title)
                .Transform(
                    HandleTransforms.ToLowercase(culture),
                    HandleTransforms.StopAtColon,
                    HandleTransforms.IntoLetterAndDigitRuns)
                .ToSlug(HandleToSlugConversions.Hyphenate));

        services.AddSingleton<PublisherNameToSlug>(_ => (name) =>
            new Handle(name)
                .Transform(
                    HandleTransforms.ToLowercase(CultureInfo.InvariantCulture),
                    HandleTransforms.IntoLetterAndDigitRuns)
                .ToSlug(HandleToSlugConversions.Hyphenate));
    }

}