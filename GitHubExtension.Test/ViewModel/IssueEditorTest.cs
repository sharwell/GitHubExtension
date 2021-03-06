﻿// **********************************************************************************
// The MIT License (MIT)
// 
// Copyright (c) 2014 Rob Prouse
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 
// **********************************************************************************

#region Using Directives

using System;
using Alteridem.GitHub.Extension.Interfaces;
using Alteridem.GitHub.Extension.Test.Mocks;
using Alteridem.GitHub.Extension.View;
using Alteridem.GitHub.Extension.ViewModel;
using Alteridem.GitHub.Model;
using NUnit.Framework;

#endregion

namespace Alteridem.GitHub.Extension.Test.ViewModel
{
    [TestFixture]
    public class IssueEditorTest
    {
        [SetUp]
        public void SetUp()
        {
            Factory.Rebind<GitHubApiBase>().To<GitHubApiMock>().InScope(o => this);
        }

        [Test]
        public void TestCanSave()
        {
            var viewModel = Factory.Get<IssueEditorViewModel>();
            viewModel.SetIssue(null);

            Assert.That(viewModel.CanSave(), Is.False);
            viewModel.Title = "title";
            Assert.That(viewModel.CanSave(), Is.True);
        }

        [Test]
        public void TestAddNewIssue()
        {
            var viewModel = Factory.Get<IssueEditorViewModel>();
            viewModel.SetIssue(null);

            viewModel.Title = "TestAddNewIssue";
            viewModel.Save(null);

            var api = Factory.Get<GitHubApiBase>();
            Assert.That(api.Issue, Is.Not.Null);
            Assert.That(api.Issue.Title, Is.EqualTo("TestAddNewIssue"));
            Assert.That(api.Issue.Number, Is.EqualTo(69));
        }

        [Test]
        public void TestUpdateIssue()
        {
            var api = Factory.Get<GitHubApiBase>();
            int number = api.Issue.Number;
            var viewModel = Factory.Get<IssueEditorViewModel>();
            viewModel.SetIssue(api.Issue);

            viewModel.Title = "TestUpdateIssue";
            viewModel.Save(null);

            Assert.That(api.Issue, Is.Not.Null);
            Assert.That(api.Issue.Title, Is.EqualTo("TestUpdateIssue"));
            Assert.That(api.Issue.Number, Is.EqualTo(number));
        }
    }
}