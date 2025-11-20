#nullable enable

namespace BensButtons.Commands;

internal abstract class ScmRepoHostingCommand: AbstractButtonCommand {

    private const string GIT_EXECUTABLE = "git";

    protected abstract string hostname { get; }

    protected abstract string fallbackUrl { get; }

    protected abstract string repoWebUrl(string username, string repoName);

    protected override async Task onClick() {
        string? hostingUrl = null;
        if (fetchSolutionDir() is {} solutionDir) {
            string upstreamName = await execGit(["rev-parse", "--abbrev-ref", "--symbolic-full-name", "@{u}"],
                workingDirectory: solutionDir, cancellationToken: disposed);

            upstreamName = upstreamName.Substring(0, upstreamName.IndexOf('/'));

            string upstreamUrl = await execGit(["remote", "get-url", upstreamName],
                workingDirectory: solutionDir, cancellationToken: disposed);

            string? hostingPath = null;
            try {
                Uri url = new(upstreamUrl);
                if (hostname.Equals(url.Host, StringComparison.OrdinalIgnoreCase)) {
                    hostingPath = url.LocalPath;
                }
            } catch (UriFormatException) {
                int colonIndex = upstreamUrl.LastIndexOf(':');
                if (colonIndex != -1) {
                    int    hostStart = upstreamUrl.IndexOf('@', 0, colonIndex) + 1;
                    string host      = upstreamUrl.Substring(hostStart, colonIndex - hostStart);
                    if (hostname.Equals(host, StringComparison.OrdinalIgnoreCase)) {
                        hostingPath = upstreamUrl.Substring(colonIndex + 1);
                    }
                }
            }

            if (hostingPath is not null) {
                string[] hostingPaths = hostingPath.TrimEnd(1, ".git").Split(['/'], 3, StringSplitOptions.RemoveEmptyEntries);
                hostingUrl = repoWebUrl(hostingPaths[0], hostingPaths[1]);
            }
        } else {
            hostingUrl = fallbackUrl;
        }

        if (hostingUrl is not null) {
            using Process? browserLaunch = Process.Start(hostingUrl);
        }
    }

    private static async Task<string> execGit(IEnumerable<string> args, string workingDirectory, CancellationToken cancellationToken) {
        (int exitCode, string stdout, string stderr) = await Processes.ExecFile(GIT_EXECUTABLE, args, workingDirectory: workingDirectory, hideWindow: true, cancellationToken: cancellationToken);
        return stdout;
    }

}