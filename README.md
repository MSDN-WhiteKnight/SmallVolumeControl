# SmallVolumeControl
Simple program to control certain sound device's volume with global hotkeys

License: WTFPL

Requirements: Windows Vista, .NET 4.5 (or newer)

Build command line:

> %WINDIR%\Microsoft.NET\Framework\v4.0.30319\Csc.exe /noconfig
> /nowarn:1701,1702,2008 /nostdlib+ /platform:anycpu32bitpreferred
> /errorreport:prompt /warn:4 /define:TRACE /errorendlocation
> /preferreduilang:ru-RU /highentropyva+
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.CSharp.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\mscorlib.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Core.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Data.DataSetExtensions.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Data.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Deployment.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Drawing.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Windows.Forms.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.dll"
> /reference:"%ProgramFiles(x86)%\Reference
> Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.Linq.dll"
> /debug:pdbonly /filealign:512 /optimize+ /out:SmallVolumeControl.exe
> /subsystemversion:6.00 /target:winexe /utf8output Program.cs

Configured via **Documents\volume.xml**:

**DeviceID** - String of format "{0.0.0.00000000}.{GUID}", where GUID stands for registry subkey name corresponding to controlled device under HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\MMDevices\Audio\Render

**Increment** - Volume increment on key press (0.1 = 10%)

**DownKeyCode** - Virtual key code to decrement volume

**UpKeyCode**  - Virtual key code to increment volume

**ExitKeyCode**  - Virtual key code to exit program

Based on SO answer by Vimes: https://stackoverflow.com/a/31751275/8674428


