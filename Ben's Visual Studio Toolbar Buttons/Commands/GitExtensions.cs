#nullable enable

using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace Aldaviva.VisualStudioToolbarButtons.Commands;

internal sealed class GitExtensions: AbstractButtonCommand {

    public override int commandId { get; } = 0x101;

    public const int CommandId = 0x0101;

    private static readonly string[] schemeHandlerKeys = [
        @"HKEY_CURRENT_USER\SOFTWARE\Classes\GitExtensions\shell\open\command",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\GitExtensions\shell\open\command"
    ];

    protected override async Task onClick() {
        if (schemeHandlerKeys.Select(key => Registry.GetValue(key, string.Empty, null) as string).FirstOrDefault(s => s is not null) is not {} schemeHandler) {
            return;
        }

        ProcessStartInfo invocation = new(schemeHandler.Substring(0, schemeHandler.IndexOf(" openrepo", StringComparison.Ordinal)).Trim('"')) {
            Arguments = getSolutionDir() is {} solutionDir ? $@"browse ""{solutionDir}""" : string.Empty
        };

        using Process proc = Process.Start(invocation);
    }

}