#nullable enable

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Process = System.Diagnostics.Process;

namespace Aldaviva.VisualStudioToolbarButtons.Commands;

internal sealed class TotalCommander: AbstractButtonCommand {

    // 32-bit installations may actually appear in WOW6432Node
    private static readonly string[] UNINSTALL_KEYS = [
        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Totalcmd64",
        @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Totalcmd",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Totalcmd64",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Totalcmd"
    ];

    private static readonly string[] EXECUTABLE_FILENAMES = [
        "totalcmd64.exe",
        "totalcmd.exe"
    ];

    public override int commandId { get; } = 0x0102;

    protected override async Task onClick() {
        if (UNINSTALL_KEYS.Select(key => Registry.GetValue(key, "InstallLocation", null) as string).FirstOrDefault(s => s is not null) is not {} installationDirectory) {
            return;
        }

        if (EXECUTABLE_FILENAMES.Select(file => Path.Combine(installationDirectory, file)).FirstOrDefault(File.Exists) is not {} absoluteFilename) {
            return;
        }

        ProcessStartInfo invocation = new(absoluteFilename) {
            Arguments = fetchProjectDir() is {} projectDir ? $@"/o /t ""{projectDir}""" : "/o"
        };

        using Process? proc = Process.Start(invocation);
    }

}