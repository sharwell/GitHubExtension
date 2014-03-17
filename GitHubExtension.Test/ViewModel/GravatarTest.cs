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
using Alteridem.GitHub.Extension.ViewModel;
using Alteridem.GitHub.Interfaces;
using Alteridem.GitHub.Model;
using NUnit.Framework;

#endregion

namespace Alteridem.GitHub.Extension.Test.ViewModel
{
    [TestFixture]
    public class GravatarTest
    {
        private IGravatar _viewModel;

        [SetUp]
        public void SetUp()
        {
            Factory.Rebind<GitHubApiBase>().To<GitHubApiMock>().InScope(o => this);
            Factory.Rebind<IGravatarProvider>().To<GravatarProvider>();
            Factory.Rebind<IGravatarCache>().To<GravatarCacheMock>();
            _viewModel = Factory.Get<IGravatar>();
        }

        [Test]
        public void TestGravatarUrlIsEmptyIfSizeIsLessThanOrEqualToZero()
        {
            _viewModel.GravatarId = "gravatar_id";
            _viewModel.Size = 0;

            Assert.That(_viewModel.GravatarUrl, Is.Empty);
        }

        [Test]
        public void TestGravatarUrlIsEmptyIfSizeIsNaN()
        {
            _viewModel.GravatarId = "gravatar_id";
            _viewModel.Size = double.NaN;

            Assert.That(_viewModel.GravatarUrl, Is.Empty);
        }

        [Test]
        public void TestGravatarUrlIsEmptyIfGravatarIdIsEmpty()
        {
            _viewModel.GravatarId = "";
            _viewModel.Size = 20;

            Assert.That(_viewModel.GravatarUrl, Is.Empty);
        }

        [Test]
        public void TestGravatarUrlContainsSizeAndGravatarId()
        {
            _viewModel.GravatarId = "gravatar_id";
            _viewModel.Size = 20;

            Assert.That(_viewModel.GravatarUrl, Contains.Substring("gravatar_id"));
            Assert.That(_viewModel.GravatarUrl, Contains.Substring("20"));
        }
    }
}