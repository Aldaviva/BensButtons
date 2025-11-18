#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Unfucked;

namespace Aldaviva.VisualStudioToolbarButtons.Commands;

internal abstract class ScmRepoHostingCommand: AbstractButtonCommand {

    private const string GIT_EXECUTABLE = "git";

    protected abstract string hostname { get; }

    protected abstract string fallbackUrl { get; }

    protected abstract string repoWebUrl(string username, string repoName);

    protected override async Task onClick() {
        string? hostingUrl = null;
        if (getSolutionDir() is {} solutionDir) {
            string upstreamName = await execGit(["rev-parse", "--abbrev-ref", "--symbolic-full-name", "@{u}"],
                workingDirectory: solutionDir, cancellationToken: disposed);

            upstreamName = upstreamName.Substring(0, upstreamName.IndexOf('/'));

            string upstreamUrl = await execGit(["remote", "get-url", upstreamName],
                workingDirectory: solutionDir, cancellationToken: disposed);

            string? hostingPath = null;
            try {
                Uri url = new(upstreamUrl);
                if (url.Host == hostname) {
                    hostingPath = url.LocalPath.TrimEnd(".git");
                }
            } catch (UriFormatException) {
                hostingPath = upstreamUrl.Substring(upstreamUrl.IndexOf(':') + 1).TrimEnd(".git");
            }

            if (hostingPath is not null) {
                string[] hostingPaths = hostingPath.Split(['/'], 3, StringSplitOptions.RemoveEmptyEntries);

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