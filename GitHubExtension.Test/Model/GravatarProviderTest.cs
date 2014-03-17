// **********************************************************************************
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
using System.Threading.Tasks;
using Alteridem.GitHub.Extension.Test.Mocks;
using Alteridem.GitHub.Interfaces;
using Alteridem.GitHub.Model;
using Microsoft.VisualStudio.Shell;
using NUnit.Framework;

#endregion

namespace Alteridem.GitHub.Extension.Test.Model
{
    [TestFixture]
    public class GravatarProviderTest
    {
        private IGravatarProvider _provider;

        [SetUp]
        public void SetUp()
        {
            Factory.Rebind<IGravatarProvider>().To<GravatarProvider>();
            Factory.Rebind<IGravatarCache>().To<GravatarCacheMock>();
            _provider = Factory.Get<IGravatarProvider>();
        }

        [Test]
        public void TestGravatarUrlIsEmptyIfSizeIsLessThanOrEqualToZero()
        {
            Assert.That(_provider.GravatarUrl("gravatar_id", 0).Result, Is.Empty);
        }

        [Test]
        public void TestGravatarUrlIsEmptyIfSizeIsNaN()
        {
            Assert.That(_provider.GravatarUrl("gravatar_id", double.NaN).Result, Is.Empty);
        }

        [Test]
        public void TestGravatarUrlIsEmptyIfGravatarIdIsEmpty()
        {
            Assert.That(_provider.GravatarUrl("", 20).Result, Is.Empty);
        }

        [Test]
        public async void TestGravatarUrlContainsSizeAndGravatarId()
        {
            string url = await _provider.GravatarUrl("gravatar_id", 20);

            Assert.That(url, Contains.Substring("gravatar_id"));
            Assert.That(url, Contains.Substring("20"));
        }
    }
}