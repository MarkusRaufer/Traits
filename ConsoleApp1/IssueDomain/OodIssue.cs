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
﻿using ConsoleApp1.Planning;

namespace ConsoleApp1.IssueDomain;

public class OodIssue
    : IPlannable
    , IPrioritizable
{
    private readonly IPlannable _plannable;
    private Prioritizable _prioritizable;

    public OodIssue(string id, string? description)
    {
        Id = id;
        Description = description;

        _plannable = new Plannable();
        _prioritizable = new Prioritizable(0);
     }

    public string? Description { get; }

    public string Id { get; }

    public DateTime? EstimatedBegin => _plannable.EstimatedBegin;

    public TimeSpan? EstimatedDuration => _plannable.EstimatedDuration;

    public DateTime? EstimatedEnd => _plannable.EstimatedEnd;

    public int Priority => _prioritizable.Priority;

    public void SetPriority(int priority) => _prioritizable = new(priority);
}
