// Copyright (C) 2020 Markus Raufer
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿namespace ConsoleApp1.Planning
{
    public class Plannable : IPlannable
    {
        private DateTime? _estimatedBegin;
        private TimeSpan? _estimatedDuration;
        private DateTime? _estimatedEnd;

        public DateTime? EstimatedBegin
        {
            get => _estimatedBegin;
            set
            {
                _estimatedBegin = value;

                if (_estimatedEnd.HasValue)
                {
                    //is valid _estimatedEnd
                    if (_estimatedEnd >= _estimatedBegin)
                    {
                        _estimatedDuration = _estimatedEnd - _estimatedBegin;
                        return;
                    }
                }

                if (!_estimatedDuration.HasValue)
                {
                    _estimatedEnd = null;
                    return;
                }

                _estimatedEnd = _estimatedBegin + _estimatedDuration;
            }
        }

        public TimeSpan? EstimatedDuration
        {
            get => _estimatedDuration;
            set
            {
                _estimatedDuration = value;

                if (_estimatedBegin.HasValue)
                {
                    _estimatedEnd = _estimatedBegin + _estimatedDuration;
                    return;
                }

                if (_estimatedEnd.HasValue)
                    _estimatedBegin = _estimatedEnd - _estimatedDuration;
            }
        }

        public DateTime? EstimatedEnd
        {
            get => _estimatedEnd;
            set
            {
                _estimatedEnd = value;

                if (_estimatedBegin.HasValue)
                {
                    //is valid _estimatedBegin
                    if (_estimatedEnd >= _estimatedBegin)
                    {
                        _estimatedDuration = _estimatedEnd - _estimatedBegin;
                        return;
                    }
                }

                if (!_estimatedDuration.HasValue)
                {
                    _estimatedBegin = null;
                    return;
                }

                _estimatedBegin = _estimatedEnd - _estimatedDuration;
            }
        }
    }
}
