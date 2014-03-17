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

using System;
using Alteridem.GitHub.Extension.Interfaces;
using Alteridem.GitHub.Interfaces;

namespace Alteridem.GitHub.Extension.ViewModel
{
    public class GravatarViewModel : BaseViewModel, IGravatar
    {
        private string _gravatarId;
        private double _size;
        private string _gravatarUrl = string.Empty;

        public double Size
        {
            get { return _size; }
            set
            {
                _size = value;
                OnPropertyChanged();
                if ( _size > 0 && !Double.IsNaN(_size) && !string.IsNullOrWhiteSpace(_gravatarId) )
                    GetGravatarUrl();
            }
        }

        public string GravatarId
        {
            get { return _gravatarId; }
            set
            {
                if (value == _gravatarId) return;
                _gravatarId = value;
                OnPropertyChanged();
                if (_size > 0 && !Double.IsNaN(_size) && !string.IsNullOrWhiteSpace(_gravatarId))
                    GetGravatarUrl();
            }
        }

        public string GravatarUrl
        {
            get { return _gravatarUrl; }
        }

        private async void GetGravatarUrl()
        {
            var provider = Factory.Get<IGravatarProvider>();
            _gravatarUrl = await provider.GravatarUrl(GravatarId, Size);
            OnPropertyChanged("GravatarUrl");
        }
    }
}