PdbGit
==========

[![Build status](https://ci.appveyor.com/api/projects/status/x2kj88duew7fmr9f/branch/master?svg=true)](https://ci.appveyor.com/project/AArnott/pdbgit/branch/master)

[![Join the chat at https://gitter.im/PdbGit/Lobby](https://badges.gitter.im/PdbGit/Lobby.svg)](https://gitter.im/PdbGit/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

![License](https://img.shields.io/github/license/aarnott/PdbGit.svg)
[![Version](https://img.shields.io/nuget/v/PdbGit.svg)][NuGetDownload]
[![Pre-release version](https://img.shields.io/nuget/vpre/PdbGit.svg)][NuGetDownload]

PdbGit lets users step through your code hosted on GitHub!
Just install the [PdbGit][NuGetDownload] NuGet package to your project.

PdbGit makes dedicated source servers obsolete by reusing your git server as a source server.

![Stepping through external source code](doc/images/GitLink_example.gif)  

The only requirement is to ensure the check the `Enable source server support` option in Visual Studio as shown below:

![Enabling source server support](doc/images/visualstudio_enablesourceserversupport.png)  

# How to use

The simplest way to use PdbGit is to [install its NuGet package][NuGetDownload] into your project.

    Install-Package PdbGit

Once installed, it automatically integrates with MSBuild to add source download instructions to your PDB.
Indexing source URLs into your PDB files is very fast. A solution with over 85 projects will be handled in less than 30 seconds.

## Command line tool

If you want to install the tool on your (build) computer, the package is available via <a href="https://chocolatey.org/" target="_blank">Chocolatey</a>. To install, use the following command:

    choco install PdbGit

Using PdbGit via the command line is very simple:

    PdbGit.exe <pdbfile>

The PDB file already has paths to your local files in it. The command line tool simply discovers the git repo
behind those files, the remote git server, and creates the URLs for them and adds them to the PDB file.
The tool emits warnings if the source files on disk do not match the versions used to compile the PDB.

### When your project has not .git folder

If running in an environment where there is no .git folder (e.g. some CI server that only downloads the latest source
as a .tgz), you can still use PdbGit by specifying extra parameters:

    PdbGit.exe path-to-your.pdb -u https://github.com/username/project --baseDir C:\root-of-your-project\ --commit your-commit-ID

### Running for a custom raw content URL

When working with a content proxy or an alternative git VCS system that supports direct HTTP access to specific file revisions use the `-u` parameter with the custom raw content root URL

    PdbGit.exe path-to-your.pdb -u https://raw.githubusercontent.com/catel/catel
    
The custom url will be used to fill in the following pattern `{customUrl}/{revision}/{relativeFilePath}` when generating the source mapping.

When working with a repository using uncommon URL you can use placeholders to specify where the filename and revision hash should be, use `-u` parameter with the custom URL

    PdbGit.exe path-to-your.pdb -u "https://host/projects/catel/repos/catel/browse/{filename}?at={revision}&raw"

The custom url will be used to fill the placeholders with the relative file path and the revision hash.

### More options

There are many more parameters you can use. Display the usage doc with the following command line:

    PdbGit.exe -h

# How does it work

The SrcSrv tool (Srcsrv.dll) enables a client to retrieve the exact version of the source files that were used to build an application. Because the source code for a module can change between versions and over the course of years, it is important to look at the source code as it existed when the version of the module in question was built.

For more information, see the <a href="http://msdn.microsoft.com/en-us/library/windows/hardware/ff558791(v=vs.85).aspx" target="_blank">official documentation of SrcSrv</a>.

PdbGit creates a source index file and updates the PDB file so it will retrieve the files from the Git host file handler.
To do this, PdbGit must be aware of the public URL from which the source files you compiled with can be retrieved.
PdbGit.exe reads your compiler-generated PDB, which already contains full paths to your local source files.
It then searches for a git repo that contains those source files and looks up the commit that HEAD points to.
It also searches your remotes for a URL pattern that it recognizes (e.g. https://github.com/name/repo).
It combines the URL and the commit ID to create a unique URL for each source file of this exact version, and adds this information to your PDB.

When you share your PDB alongside your DLL, your users who debug with Source Server support enabled will automatically be able to step into your source code. 

# Supported git providers

PdbGit supports the following providers out of the box (will auto-detect based on the url):

* <a href="https://bitbucket.org/" target="_blank">BitBucket</a>
* <a href="https://github.com/" target="_blank">GitHub</a>

If your git host isn't listed here, PdbGit can still work for you given these requirements on your git host:

1. URLs to the raw content of each file for each commit exist.
2. The content is available for anonymous access to the public.

# Troubleshooting

## Source Stepping isn't working

* Visual Studio 2012 needs to run elevated in order to download the source server files

* Specify a value for Visual Studio -> Options -> Debugging -> Symbols -> `Cache Symbols in this directory`

![Enabling source server support](doc/images/visualstudio_symbolslocation.png)

## Source Stepping returns HTML
If your repository is private, you are likely seeing the logon HTML from your git host.

* Log onto your git host in Internet Explorer
* Purge your local symbol cache

Note that this approach is not guaranteed to work.  Visual Studio needs to authenticate to retrieve the source files
but does not ask the user for credentials to do so.  There are ways to work around this, but no mechanism is currently
provided out-of-the-box in *PdbGit*.

Possible workarounds
* Include a mechanism in the pdb to retrieve credentials (using PowerShell and Windows credentials store) (see [#37](https://github.com/GitTools/GitLink/issues/37))
* Use a proxy service that does not require authentication (see [#66](https://github.com/GitTools/GitLink/issues/66) and [Source server with Git repository](https://shonnlyga.wordpress.com/2016/05/28/source-server-with-git-repository))

# History

PdbGit is a fork of [GitLink][Upstream], which was a project where great contributors did the heavy lifting of reverse-engineering Microsoft's PDB file format in order to add source server support for Git. This fork became permanent and was renamed to PdbGit when GitLink committers [resisted](https://github.com/GitTools/GitLink/pull/110) accepting pull requests that added significant value to their project and simplified usage, including:

1. Installing the GitLink nuget package into a project automatically activates GitLink in that project's build. The README instructions are updated to reflect just how easy it is now compared to before.
2. gitlink.exe is now focused on the PDB instead of Visual Studio solutions. The only argument required is the PDB file. Everything else is automatic.
3. pdb file information always has the canonical capitalization expected by the web server.
4. added support for PDBs that carry SHA1 hashes instead of MD5. I don't know who still uses MD5, but csc.exe seems to always produce SHA1 hashes.
5. packages.config have been migrated to project.json, resulting in cleaner, simpler build authoring that's easier to manage.
6. packages and assemblies versioned with git information.
7. NuGet package builds with the same MSBuild invocation that builds the solution. And if the NuProj extension for VS is installed, it builds within VS as well.
8. Builds on appveyor. Once this PR is accepted, appveyor should be added to this repo and the README badge can be updated to point to your appveyor build instead of mine.
9. Print out how many files ended up getting indexed into the PDB, to make noticing issues easier.
10. Tests are upgraded to NUnit 3, and the NUnit test adapter for VS is added so that tests automatically run in VS even for folks who haven't installed the NUnit extension.
11. Recognize and report failure even when pdbstr.exe fails with a 0 exit code.
12. README file is reordered to emphasize benefits and usage instructions over troubleshooting and how it works internally.
13. GitLink NuPkg is ready for chocolatey. That is, the same package for nuget.org also works on chocolatey directly. 
14. nupkg license points at the exact license that produced the package rather than the tip of 'develop', which may have diverged from what the package was built with. (mostly theoretical, but sound).

[NuGetDownload]: https://www.nuget.org/packages/PdbGit
[Upstream]: https://github.com/gittools/gitlink
