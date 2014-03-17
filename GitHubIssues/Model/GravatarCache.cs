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

using System;
using System.IO;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Alteridem.GitHub.Akavache;
using Alteridem.GitHub.Interfaces;
using NLog;

namespace Alteridem.GitHub.Model
{
    public class GravatarCache : IGravatarCache
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private string _cacheDirectory;

        public GravatarCache()
        {
            _cacheDirectory = GetCacheDirectory();
            new DirectoryInfo(_cacheDirectory).CreateRecursive();
        }

        /// <summary>
        /// Fetches a gravatar and stores it on disk. Once it is stored on
        /// disk, it will get the version from the cache
        /// </summary>
        /// <param name="gravatarId"></param>
        /// <param name="size"></param>
        /// <returns>The location of the gravatar</returns>
        public async Task<string> GetGravatar(string gravatarId, int size)
        {
            if (string.IsNullOrWhiteSpace(gravatarId) || size <= 0)
                return string.Empty;

            log.Info("Getting gravatar {0} at {1}px", gravatarId, size);
            var filename = GetFilename(gravatarId, size);

            if (!IsInCache(filename) || IsStale(filename))
            {
                filename = await GetFromWeb(gravatarId, size);
            }
            return filename;
        }

        private string GetCacheDirectory()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), BlobCache.ApplicationName, "Gravatar");
        }

        private string GetFilename(string gravatarId, int size)
        {
            var file = string.Format("{0}_{1}.png", gravatarId, size);
            return Path.Combine(_cacheDirectory, file);
        }

        private async Task<string> GetFromWeb(string gravatarId, int size)
        {
            var url = GetGravatarUrl(gravatarId, size);
            var filename = GetFilename(gravatarId, size);
            try
            {
                using (var http = new HttpClient())
                using (var stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    var response = await http.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    await response.Content.CopyToAsync(stream);
                }
            }
            catch (HttpRequestException e)
            {
                log.ErrorException("Failed to get gravatar from " + url, e);
            }
            catch (Exception e)
            {
                log.ErrorException("Failed to get gravatar", e);
            }
            return string.Empty;
        }

        private string GetGravatarUrl(string gravatarId, int size)
        {
            string gravatar =
                   string.Format(
                       "https://www.gravatar.com/avatar/{0}?s={1}&r=x",
                       gravatarId,
                       size);
            return gravatar;
        }

        private bool IsInCache(string filename)
        {
            return File.Exists(filename);
        }

        /// <summary>
        /// We will reget the gravatar once a day
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool IsStale(string filename)
        {
            try
            {
                var info = new FileInfo(filename);
                if (!info.Exists)
                    return true;

                var age = DateTime.UtcNow.Subtract(info.LastWriteTimeUtc);
                return age.TotalHours >= 24;
            }
            catch (Exception e)
            {
                log.ErrorException("Error determining if file is stale, " + filename, e );
                return true;
            }
        }
    }
}