#nullable enable

using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.Collections.Frozen;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Process = System.Diagnostics.Process;

namespace BensButtons.Commands;

internal abstract class AbstractButtonCommand {

    public static readonly Guid COMMAND_SET = new("61da83b2-5868-4375-9649-92d5e4fabdcc");

    private static readonly FrozenSet<string> PROJECT_FILE_EXTENSIONS = [".csproj", ".fsproj", ".vcproj"];

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

    protected string? fetchProjectDir() {
        Document? doc = visualStudio.ActiveDocument;
        if (doc?.ProjectItem?.ContainingProject?.FullName is {} projectFilename) {
            return Path.GetDirectoryName(projectFilename);
        } else if (doc?.FullName is {} filename && PROJECT_FILE_EXTENSIONS.Contains(Path.GetExtension(filename).ToLowerInvariant())) {
            return Path.GetDirectoryName(filename);
        } else {
            return null;
        }
    }

    protected static bool focusExistingWindow(string processBaseName) {
        Process[] existingProcesses = Process.GetProcessesByName(processBaseName.TrimEnd(1, ".exe"));

        if (existingProcesses.FirstOrDefault() is {} existingProcess) {
            Foregrounder.Foregrounder.BringToForeground(existingProcess.MainWindowHandle);

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