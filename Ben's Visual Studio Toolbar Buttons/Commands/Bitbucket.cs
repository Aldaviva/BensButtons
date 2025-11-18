#nullable enable

namespace Aldaviva.VisualStudioToolbarButtons.Commands;

internal class Bitbucket: ScmRepoHostingCommand {

    private const string GIT_EXECUTABLE = "git";

    public override int commandId => 0x104;

    protected override string hostname => "bitbucket.org";

    protected override string fallbackUrl => "https://bitbucket.org/aldaviva/workspace/repositories/";

    protected override string repoWebUrl(string username, string repoName) => $"https://bitbucket.org/{username}/{repoName}";

    /*protected override async Task onClick() {
        string? githubUrl = null;
        if (getSolutionDir() is {} solutionDir) {
            (int exitCode, string stdout, string stderr) upstream = await Processes.ExecFile(GIT_EXECUTABLE,
                ["rev-parse", "--abbrev-ref", "--symbolic-full-name", "@{u}"],
                workingDirectory: solutionDir, hideWindow: true, cancellationToken: disposed);

            string upstreamName = upstream.stdout.Substring(0, upstream.stdout.IndexOf('/'));

            (int exitCode, string stdout, string stderr) upstreamUrl = await Processes.ExecFile(GIT_EXECUTABLE,
                ["remote", "get-url", upstreamName],
                workingDirectory: solutionDir, hideWindow: true, cancellationToken: disposed);

            string? githubPath = null;
            try {
                Uri url = new(upstreamUrl.stdout);
                if (url.Host == "bitbucket.org") {
                    githubPath = url.LocalPath.TrimEnd(".git");
                }
            } catch (UriFormatException) {
                githubPath = upstreamUrl.stdout.Substring(upstreamUrl.stdout.IndexOf(':') + 1).TrimEnd(".git");
            }

            if (githubPath is not null) {
                string[] githubPaths = githubPath.Split(['/'], 2, StringSplitOptions.RemoveEmptyEntries);

                githubUrl = new UrlBuilder("https", "bitbucket.org")
                    .Path(githubPaths[0], false)
                    .Path(githubPaths[1], false).ToString();

            }
        } else {
            githubUrl = "https://bitbucket.org/aldaviva/workspace/repositories/";
        }

        if (githubUrl is not null) {
            using Process? browserLaunch = Process.Start(githubUrl);
        }
    }*/

}