🛠 Ben's Buttons
===

[![Visual Studio Marketplace](https://img.shields.io/visual-studio-marketplace/v/aldaviva.BensButtons?label=vs%20marketplace&include_prereleases&logo=data:image/svg%2bxml;base64,PHN2ZyB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC9zdmciIGZpbGw9IiNmZmYiIHZpZXdCb3g9IjAgMCAxNiAxNiI+PHBhdGggZD0iTTQuNTYgOC45MiA0IDkuODZsLS40Ny43OGMtLjMuNTEtLjc5LjgxLTEuMzQuODlsMi41NCAxLjIzLjAxLjAxcS4yNi4xMy41Ny4xM2MuNDQgMCAuODMtLjIyIDEuMDctLjU1di0uMDFsLjExLS4xNXpNMi42NyA1Ljc1YS45LjkgMCAwIDAtMS4wMS0uNDEuOS45IDAgMCAwLS42Ni44N3YzLjQ2YzAgLjUxLjM0Ljc4LjY2Ljg3cS4xMi4wNC4yNi4wNGMuMjcgMCAuNTYtLjEyLjc1LS40NWwxLjMxLTIuMTl6TTE1IDQuMjl2Ny4zYzAgLjU0LS4zMyAxLjAxLS44MSAxLjIxbC0uMDguMDMtMy45NSAxLjQxcS0uMjMuMDktLjQ3LjA5YTEuMyAxLjMgMCAwIDEtMS4xLS42bC0uMjgtLjQ0LS4wOC0uMTItNC43LTcuOTNjLS4yOS0uNS0uNzgtLjgxLTEuMzItLjg5bDIuNTItMS4yMy4wMS0uMDFxLjI2LS4xMi41Ny0uMTNjLjQ0IDAgLjgzLjIyIDEuMDcuNTV2LjAxTDExIDEwLjA1VjUuNTFsLTEgMS40MS0yLjMyLTMuMjcuODctMS41LjA0LS4wNWMuMjMtLjM2LjY0LS42IDEuMS0uNi4zNiAwLS4wNC0uMDYgNC40NSAxLjU2bC4wNS4wMmMuNDguMi44MS42Ny44MSAxLjIxIi8+PC9zdmc+)](https://marketplace.visualstudio.com/items?itemName=aldaviva.BensButtons)

*Add custom buttons to the Visual Studio toolbar.*

External Tools are easy to create, but they don't let you specify custom icons for some reason.

<!-- MarkdownTOC autolink="true" bracket="round" autoanchor="false" levels="1,2,3" bullets="-" -->

- [Requirements](#requirements)
- [Buttons](#buttons)
- [Installation](#installation)
- [Usage](#usage)
- [Adding new buttons](#adding-new-buttons)

<!-- /MarkdownTOC -->

## Requirements
- [Visual Studio](https://visualstudio.microsoft.com/vs/) 2022, 2026, or later for Windows

## Buttons

<p align="center"><img src="./Ben's%20Buttons/screenshot1.png" alt="Custom buttons in VS toolbar" /></p>

- Bitbucket
    - Browse current repository's [Bitbucket](https://bitbucket.org/) web page
- Git Extensions
    - Opens current repository in [Git Extensions](https://gitextensions.github.io)
- GitHub
    - Browse current repository's [GitHub](https://github.com/) web page
- Sublime Text
    - Edit current file in [Sublime Text](https://www.sublimetext.com)
- Total Commander
    - Opens current project directory in a new [Total Commander](https://www.ghisler.com) tab

## Installation
- From [Visual Studio Marketplace](https://marketplace.visualstudio.com/vs)
    1. TBD
- From VSIX file
    1. Download and run the [latest release](https://github.com/Aldaviva/BensButtons/releases/latest)'s VSIX file (TBD)

## Usage
- Buttons are available in the Tools › **Ben's Buttons** submenu
- Add a button to a toolbar
    1. On the toolbar, click <kbd>…</kbd> › Add or Remove Buttons › Customize…
    1. Click Add Command…
    1. Select the Tools category
    1. Select one of the [buttons](#buttons) provided by this extension, then click OK
    1. Arrange the new button using Move Up, Move Down, and Modify Selection to make it appear as desired
- Add a keyboard shortcut to a tool
    1. Go to Tools › Options › More Settings (VS ≥ 2026) › Keyboard
    1. Search for the command with the `Tools.BensButtons.` prefix, such as `Tools.BensButtons.GitExtensions`
    1. Type the desired key combination in the Press shortcut keys text box
    1. Click Assign

## Adding new buttons
To add or edit buttons, you can fork this repository and edit its source.

In general, just look for all 5 instances of `Add new buttons here` and make a new one that looks like the existing buttons. It's easier than the piles of XML make it look.

1. Add new button to `menus.vsct`.
    - New buttons go in `/CommandTable/Commands/Buttons`
    - Add a new button definition that looks like
        ```xml
        <Button guid="commandSet" id="myProgram" priority="106" type="Button">
            <Parent guid="commandSet" id="submenuGroup" />
            <Icon guid="imageManifest" id="myProgram" />
            <CommandFlag>IconIsMoniker</CommandFlag>
            <Strings>
                <ButtonText>My Program</ButtonText>
                <ToolTipText>Open file in My Program</ToolTipText>
                <CanonicalName>BensButtons.MyProgram</CanonicalName>
                <LocCanonicalName>BensButtons.MyProgram</LocCanonicalName>
            </Strings>
        </Button>
        ```
    - Replace the `myProgram` command ID symbol with your own arbitrary command ID symbol
    - Replace `ButtonText`, `ToolTipText`, `CanonicalName`, and `LocCanonicalName` with the friendly name of the program, a description of the button action, and two versions without spaces, respectively
    - Replace `priority` with the sort index of this button, which should be unique among buttons in the `submenuGroup` command group
    - Add two new command IDs to the `/CommandTable/Symbols/GuidSymbol` elements
        - The `commandSet` should contain a command ID like
            ```xml
            <IDSymbol name="myProgram" value="106" />
            ```
            where the `name` was used above in the `Button/@id`, and the `value` is not used by any other commands and is therefore unique in the command set
        - The `imageManifest` should contain the icon ID like
            ```xml
            <IDSymbol name="myProgram" value="106" />
            ```
            where the `name` was used above in the `Button/Icon/@id`, and the `value` is unique in the image manifest; it doesn't have to be the same as the `value` from the `commandSet`
1. Import images into the project.
    - Image files go in the `Resources` directory
    - Native raster dimensions are 16×16px for 100% scaling, and vector images are scaled correctly regardless of the viewbox size
    - All image files must have their Build Action changed from Content to **Resource**
    - Accepted formats are PNG (raster, 32bpp including transparency) and XAML (vector)
    - Different formats can be mixed in the same image, for example a 16px raster image for 100% scaling and a vector fallback for all other scaled sizes like 32px (200%)
    - Images can have variations for light and dark mode
1. Add new [images](https://learn.microsoft.com/en-us/visualstudio/extensibility/image-service-and-catalog) to `images.imagemanifest`
    - New `Image` elements go in `/ImageManifest/Images`
    - Add a new image definition that looks like
        ```xml
        <Image Guid="$(imageManifest)" ID="106" AllowColorInversion="false">
            <Source Uri="$(Resources)/myProgram-16.png">
                <Size Value="16" />
            </Source>
            <Source Uri="$(Resources)/myProgram-24.png">
                <Size Value="24" />
            </Source>
            <Source Uri="$(Resources)/myProgram-32.png">
                <Size Value="32" />
            </Source>
            <Source Uri="$(Resources)/myProgram-48.png">
                <Size Value="48" />
            </Source>
        </Image>
        ```
    - Replace the ID with the number used in `menus.vsct` in the `value` for the `IDSymbol` for `imageManifest`
    - Replace the filename with the PNG or XAML filename, prefixed with `$(Resources)/`
    - The size is the edge length of the image in physical pixels when this source is allowed to be used (16px at 100% scaling, 24px at 150%, etc)
    - To specify different images for light and dark mode, set the `Background` attribute on the `Source` to `Light` or `Dark`, respectively; otherwise you can set `Image/@AllowColorInversion` to `false` to prevent Visual Studio from inverting the black and white pixels of the image in dark mode
        ```xml
        <Image Guid="$(imageManifest)" ID="106">
            <Source Uri="$(Resources)/myprogram-light.xaml" Background="Light" />
            <Source Uri="$(Resources)/myprogram-dark.xaml" Background="Dark" />
        </Image>
        ```
    - Vector images can be converted using [SvgToXaml](https://github.com/BerndK/SvgToXaml), although the XAML it generates needs to be massaged to work in a standalone XAML file: the `DrawingGroup` must be removed from the `DrawingImage` and put in a `DrawingBrush`'s `Drawing` property, which goes inside a `Rectangle` inside a `Viewbox`; see [`github-dark.xaml`](https://github.com/Aldaviva/BensButtons/blob/master/Ben's%20Buttons/Resources/github-dark.xaml) and [`bitbucket.xaml`](https://github.com/Aldaviva/BensButtons/blob/master/Ben's%20Buttons/Resources/bitbucket.xaml) for examples
        ```xml
        <Viewbox xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">
            <Rectangle Width="16" Height="16">
                <Rectangle.Fill>
                    <DrawingBrush>
                        <DrawingBrush.Drawing>
                            <!-- DrawingGroup element and descendants from SvgToXaml -->
                        </DrawingBrush.Drawing>
                    </DrawingBrush>
                </Rectangle.Fill>
            </Rectangle>
        </Viewbox>
        ```
1. Add a new button command class.
    - Subclass `AbstractButtonCommand` and put it in the `Commands` namespace
        ```cs
        namespace BensButtons.Commands;

        internal class MyProgram: AbstractButtonCommand {

            public override int commandId => 106;

            protected override async Task onClick() {
                // TODO: button was clicked
            }
        }
        ```
    - Replace the `commandId` property value with the `value` of your new `IDSymbol` for `commandSet` in `menus.vsct`
    - Handle the button click event in the `onClick` method
1. Register the new button class in the `MyPackage` class
    - Add a new constructor call to your button class in the `registerButtons` method, and call `register` on the new instance
        ```cs
        new MyProgram { extensionPackage = this, visualStudio = visualStudio }.register(commandService);
        ```
1. Test your changes in another instance of Visual Studio
    1. Delete `%LOCALAPPDATA%\Microsoft\VisualStudio\*Exp\ImageLibrary\ImageLibrary.cache` if it already exists, to force Visual Studio to re-read new and updated images from your extension
    1. Clean and Rebuild your extension (not just Build) to make sure new image files aren't ignored
    1. Click Start without Debugging to install the extension into another Visual Studio instance and launch it
    1. Look in the Tools › Ben's Buttons menu