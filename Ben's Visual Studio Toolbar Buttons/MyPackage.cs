#nullable enable

using Aldaviva.VisualStudioToolbarButtons.Commands;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

[assembly: ProvideCodeBase(AssemblyName = "Aldaviva.VisualStudioToolbarButtons", Version = "1.0.0.0", CodeBase = "$PackageFolder$\\Aldaviva.VisualStudioToolbarButtons.dll")]

namespace Aldaviva.VisualStudioToolbarButtons;

[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
[Guid(PACKAGE_GUID_STRING)]
[ProvideMenuResource("Menus.ctmenu", 1)]
public sealed class MyPackage: AsyncPackage {

    public const string PACKAGE_GUID_STRING = "5ab74aa4-0959-41d9-a8a4-ea5dfc612111";

    protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress) {
        await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

        OleMenuCommandService commandService = (OleMenuCommandService) (await GetServiceAsync(typeof(IMenuCommandService)))!;
        DTE2                  dte            = (DTE2) (await GetServiceAsync(typeof(DTE)))!;

        new GitExtensions { extensionPackage  = this, visualStudio = dte }.register(commandService);
        new TotalCommander { extensionPackage = this, visualStudio = dte }.register(commandService);
        new GitHub { extensionPackage         = this, visualStudio = dte }.register(commandService);
        new Bitbucket { extensionPackage      = this, visualStudio = dte }.register(commandService);
        new SublimeText { extensionPackage    = this, visualStudio = dte }.register(commandService);

        // Add new buttons here
    }

}