#nullable enable

using BensButtons.Commands;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Task = System.Threading.Tasks.Task;

[assembly: ProvideCodeBase(AssemblyName = "BensButtons", Version = "1.0.0.0", CodeBase = "$PackageFolder$\\BensButtons.dll")]

namespace BensButtons;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[Guid(PACKAGE_GUID_STRING)]
[ProvideMenuResource("Menus.ctmenu", 1)]
public sealed class BensButtons: AsyncPackage {

    public const string PACKAGE_GUID_STRING = "5ab74aa4-0959-41d9-a8a4-ea5dfc612111";

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress) {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        OleMenuCommandService commandService = (OleMenuCommandService) (await GetServiceAsync(typeof(IMenuCommandService)))!;
        DTE2                  dte            = (DTE2) (await GetServiceAsync(typeof(DTE)))!;

        registerButtons(dte, commandService);
    }

    private void registerButtons(DTE2 visualStudio, OleMenuCommandService commandService) {
        new GitExtensions { extensionPackage  = this, visualStudio = visualStudio }.register(commandService);
        new TotalCommander { extensionPackage = this, visualStudio = visualStudio }.register(commandService);
        new GitHub { extensionPackage         = this, visualStudio = visualStudio }.register(commandService);
        new Bitbucket { extensionPackage      = this, visualStudio = visualStudio }.register(commandService);
        new SublimeText { extensionPackage    = this, visualStudio = visualStudio }.register(commandService);

        // Add new buttons here
    }

}