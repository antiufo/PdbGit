﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitHubProviderFacts.cs" company="CatenaLogic">
//   Copyright (c) 2014 - 2016 CatenaLogic. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PdbGit.Tests.Providers
{
    using PdbGit.Providers;
    using NUnit.Framework;

    public class GitHubProviderFacts
    {
        [TestFixture]
        public class TheGitHubProviderInitialization
        {
            [TestCase]
            public void ReturnsValidInitialization()
            {
                var provider = new GitHubProvider();
                var valid = provider.Initialize("https://github.com/CatenaLogic/GitLink");

                Assert.IsTrue(valid);
            }

            [TestCase]
            public void ReturnsInvalidInitialization()
            {
                var provider = new GitHubProvider();
                var valid = provider.Initialize("https://bitbucket.org/CatenaLogic/GitLink");

                Assert.IsFalse(valid);
            }
        }

        [TestFixture]
        public class TheGitHubProviderProperties
        {
            [TestCase]
            public void ReturnsValidCompany()
            {
                var provider = new GitHubProvider();
                provider.Initialize("https://github.com/CatenaLogic/GitLink");

                Assert.AreEqual("CatenaLogic", provider.CompanyName);
            }

            [TestCase]
            public void ReturnsValidCompanyUrl()
            {
                var provider = new GitHubProvider();
                provider.Initialize("https://github.com/CatenaLogic/GitLink");

                Assert.AreEqual("https://github.com/CatenaLogic", provider.CompanyUrl);
            }

            [TestCase]
            public void ReturnsValidProject()
            {
                var provider = new GitHubProvider();
                provider.Initialize("https://github.com/CatenaLogic/GitLink");

                Assert.AreEqual("GitLink", provider.ProjectName);
            }

            [TestCase]
            public void ReturnsValidProjectUrl()
            {
                var provider = new GitHubProvider();
                provider.Initialize("https://github.com/CatenaLogic/GitLink");

                Assert.AreEqual("https://github.com/CatenaLogic/GitLink", provider.ProjectUrl);
            }

            [TestCase]
            public void ReturnsValidRawGitUrl()
            {
                var provider = new GitHubProvider();
                provider.Initialize("https://github.com/CatenaLogic/GitLink");

                Assert.AreEqual("https://raw.github.com/CatenaLogic/GitLink", provider.RawGitUrl);
            }

            [TestCase]
            public void ReturnsValidRawGitUrlWhenDotGitSuffixIsPresent()
            {
                var provider = new GitHubProvider();
                provider.Initialize("https://github.com/CatenaLogic/GitLink.git");

                Assert.AreEqual("https://raw.github.com/CatenaLogic/GitLink", provider.RawGitUrl);
            }
        }
    }
}