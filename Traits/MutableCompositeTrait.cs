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
﻿namespace Traits;

public class MutableCompositeTrait
    : CompositeTrait
    , IMutableCompositeTrait
{
    public MutableCompositeTrait() :base(Array.Empty<ITrait>())
    {
    }

    public MutableCompositeTrait(ITrait trait) : base(trait)
    {
    }


    public MutableCompositeTrait(KeyValuePair<Type, ITrait> keyValue) : base(keyValue)
    {
    }
    
    public MutableCompositeTrait(IEnumerable<ITrait> traits) : base(traits)
    {
    }

    public MutableCompositeTrait(IEnumerable<KeyValuePair<Type, ITrait>> traits) : base(traits)
    {
    }

    public ICompositeTrait SetTrait(ITrait trait)
    {
        ArgumentNullException.ThrowIfNull(trait, nameof(trait));

        Traits[trait.GetType()] = trait;
        return this;
    }

    public ICompositeTrait SetTrait<T>(T trait)
        where T : ITrait
    {
        ArgumentNullException.ThrowIfNull(trait, nameof(trait));

        Traits[typeof(T)] = trait;
        return this;
    }
}
