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

using System.Threading.Tasks;
using Alteridem.GitHub.Interfaces;

namespace Alteridem.GitHub.Extension.Test.Mocks
{
    public class GravatarCacheMock : IGravatarCache
    {
#pragma warning disable 1998    // I know I am not awaiting anything, this is a test
        /// <summary>
        /// Fetches a gravatar and stores it on disk. Once it is stored on
        /// disk, it will get the version from the cache
        /// </summary>
        /// <param name="gravatarId"></param>
        /// <param name="size"></param>
        /// <returns>The location of the gravatar</returns>
        public async Task<string> GetGravatar(string gravatarId, int size)
        {
            return string.Format("{0}_{1}.png", gravatarId, size);
        }
#pragma warning restore 1998
    }
}