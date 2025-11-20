#nullable enable

namespace BensButtons.Commands;

internal class Bitbucket: ScmRepoHostingCommand {

    private const string GIT_EXECUTABLE = "git";

    public override int commandId => 101;

    protected override string hostname => "bitbucket.org";

    protected override string fallbackUrl => "https://bitbucket.org/aldaviva/workspace/repositories/";

    protected override string repoWebUrl(string username, string repoName) => $"https://bitbucket.org/{username}/{repoName}";

}