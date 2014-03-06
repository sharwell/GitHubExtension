using Alteridem.GitHub.Model;
using Ninject.Modules;
using Octokit;
using ProductHeaderValue = Octokit.ProductHeaderValue;

namespace Alteridem.GitHub.Modules
{
    public class GitHubModule : NinjectModule
    {
        public override void Load()
        {
            Bind<GitHubClient>()
                .To<GitHubClient>()
                .WithConstructorArgument("productInformation", c => new ProductHeaderValue("GitHubExtension"));

            Bind<GitHubApi>()
                .To<GitHubApi>()
                .InSingletonScope();
        }
    }
}