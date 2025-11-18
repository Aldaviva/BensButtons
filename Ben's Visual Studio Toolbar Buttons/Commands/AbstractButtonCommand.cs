#nullable enable

using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aldaviva.VisualStudioToolbarButtons.Commands;

internal abstract class AbstractButtonCommand {

    public static readonly Guid COMMAND_SET = new("61da83b2-5868-4375-9649-92d5e4fabdcc");

    public required AsyncPackage extensionPackage { get; init; }
    public required DTE2 dte { get; init; }

    protected CancellationToken disposed => extensionPackage.DisposalToken;

    public abstract int commandId { get; }

    public void register(OleMenuCommandService menuCommandService) {
        CommandID   menuCommandId = new(COMMAND_SET, commandId);
        MenuCommand menuItem      = new(execute, menuCommandId);
        menuCommandService.AddCommand(menuItem);
    }

    // ReSharper disable once AsyncVoidEventHandlerMethod - all exceptions are already caught in the method body
    private async void execute(object sender, EventArgs eventArgs) {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(disposed);

        try {
            await onClick();
        } catch (Exception e) when (e is not OutOfMemoryException) {
            MessageBox.Show(e.ToString(), $"Uncaught {e.GetType().Name} in {nameof(onClick)}", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    protected string? getSolutionDir() => dte.Solution?.FullName is { Length: not 0 } solutionFilename ? Path.GetDirectoryName(solutionFilename) : null;

    protected string? getProjectDir() => dte.ActiveDocument?.ProjectItem?.ContainingProject?.FullName is {} projectFilename ? Path.GetDirectoryName(projectFilename) : null;

    protected abstract Task onClick();

}