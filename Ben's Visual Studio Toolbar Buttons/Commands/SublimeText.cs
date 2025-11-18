#nullable enable

using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Aldaviva.VisualStudioToolbarButtons.Commands;

internal class SublimeText: AbstractButtonCommand {

    public override int commandId => 0x105;

    private static readonly string[] UNINSTALL_KEYS = [
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sublime Text 4_is1",
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Sublime Text 3_is1"
    ];

    protected override async Task onClick() {
        if (UNINSTALL_KEYS.Select(key => Registry.GetValue(key, "InstallLocation", null) as string).FirstOrDefault(s => s is not null) is not {} installLocation) {
            return;
        }

        string executableAbsoluteFilename = Path.Combine(installLocation, "sublime_text.exe");
        if (!File.Exists(executableAbsoluteFilename)) return;

        string? documentPath = fetchDocument();

        if (documentPath is not null) {
            using Process? proc = Process.Start(executableAbsoluteFilename, $@"""{documentPath}""");
        } else if (!focusExistingWindow("sublime_text")) {
            using Process? proc = Process.Start(executableAbsoluteFilename);
        }
    }

}