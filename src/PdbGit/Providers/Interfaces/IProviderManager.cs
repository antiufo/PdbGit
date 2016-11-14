// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProviderManager.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2016 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PdbGit.Providers
{
    public interface IProviderManager
    {
        ProviderBase GetProvider(string url);
    }
}