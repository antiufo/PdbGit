// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HgRepository.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2016 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PdbGit
{
    using System.Collections.Generic;
    using System.Linq;
    using PdbGit.Providers;
    using Shaman;
    using Shaman.Runtime;

    internal class HgRepository
    {
        private string path;

        public HgRepository(string path)
        {
            this.path = path;
        }

        public string GetCurrentChangeset()
        {
            return ProcessUtils.RunFrom(path, "hg", "id", "--id", "--debug").Trim().Trim('+');
        }

        public IReadOnlyList<string> GetRemotes()
        {
            return ProcessUtils.RunFrom(path, "hg", "paths").Split('\n').Select(x => x.TryCaptureAfter(" = ")?.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToList();
        }
    }
}
