#nullable enable

namespace BensButtons.Commands;

internal class GitHub: ScmRepoHostingCommand {

    public override int commandId => 103;

    protected override string hostname => "github.com";

    protected override string fallbackUrl => "https://github.com/Aldaviva?tab=repositories";

    protected override string repoWebUrl(string username, string repoName) => $"https://github.com/{username}/{repoName}";

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
                if (url.Host == "github.com") {
                    githubPath = url.LocalPath.TrimEnd(".git");
                }
            } catch (UriFormatException) {
                githubPath = upstreamUrl.stdout.Substring(upstreamUrl.stdout.IndexOf(':') + 1).TrimEnd(".git");
            }

            if (githubPath is not null) {
                string[] githubPaths = githubPath.Split(['/'], 2, StringSplitOptions.RemoveEmptyEntries);

                githubUrl = $"https://github.com/{githubPaths[0]}/{githubPaths[1]}";

            }
        } else {
            githubUrl = "https://github.com/Aldaviva?tab=repositories";
        }

        if (githubUrl is not null) {
            using Process? browserLaunch = Process.Start(githubUrl);
        }
    }*/

}