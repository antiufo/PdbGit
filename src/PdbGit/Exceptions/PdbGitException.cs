// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PdbGitException.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2016 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PdbGit
{
    using System;

    public class PdbGitException : Exception
    {
        public PdbGitException(string message)
            : base(message)
        {
        }
    }
}