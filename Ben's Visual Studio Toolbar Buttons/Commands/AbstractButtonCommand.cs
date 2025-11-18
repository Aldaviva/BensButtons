#nullable enable

using Aldaviva.VisualStudioToolbarButtons.Dependencies.Foregrounder;
using Aldaviva.VisualStudioToolbarButtons.Dependencies.Unfucked;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aldaviva.VisualStudioToolbarButtons.Commands;

internal abstract class AbstractButtonCommand {

    public static readonly Guid COMMAND_SET = new("61da83b2-5868-4375-9649-92d5e4fabdcc");

    public required AsyncPackage extensionPackage { get; init; }
    public required DTE2 visualStudio { get; init; }

    protected CancellationToken disposed => extensionPackage.DisposalToken;

    public abstract int commandId { get; }

    public void register(OleMenuCommandService menuCommandService) =>
        menuCommandService.AddCommand(new MenuCommand(execute, new CommandID(COMMAND_SET, commandId)));

    // ReSharper disable once AsyncVoidEventHandlerMethod - all exceptions are already caught in the method body
    private async void execute(object sender, EventArgs eventArgs) {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(disposed);

        try {
            await onClick();
        } catch (Exception e) when (e is not OutOfMemoryException) {
            MessageBox.Show(e.ToString(), $"Uncaught {e.GetType().Name} in {GetType().Name}.{nameof(onClick)}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected string? fetchDocument() => visualStudio.ActiveDocument?.FullName;

    protected string? fetchSolutionDir() => visualStudio.Solution?.FullName is { Length: not 0 } solutionFilename ? Path.GetDirectoryName(solutionFilename) : null;

    protected string? fetchProjectDir() => visualStudio.ActiveDocument?.ProjectItem?.ContainingProject?.FullName is {} projectFilename ? Path.GetDirectoryName(projectFilename) : null;

    protected static bool focusExistingWindow(string processBaseName) {
        Process[] existingProcesses = Process.GetProcessesByName(processBaseName.TrimEnd(1, ".exe"));

        if (existingProcesses.FirstOrDefault() is {} existingProcess) {
            Foregrounder.BringToForeground(existingProcess.MainWindowHandle);

            foreach (Process process in existingProcesses) {
                process.Dispose();
            }
            return true;
        } else {
            return false;
        }
    }

    protected abstract Task onClick();

}