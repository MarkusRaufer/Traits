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

public class CompositeTrait : ICompositeTrait
{
    public CompositeTrait(ITrait trait)
        : this(new KeyValuePair<Type, ITrait>(trait.GetType(), trait))
    {
    }

    public CompositeTrait(KeyValuePair<Type, ITrait> keyValue)
        : this(new[] { keyValue })
    {
    }

    public CompositeTrait(IEnumerable<ITrait> traits) : this(traits.ToDictionary(x => x.GetType(), x => x))
    {
    }

    public CompositeTrait(IEnumerable<KeyValuePair<Type, ITrait>> traits)
    {
        ArgumentNullException.ThrowIfNull(traits);

        Traits = traits.ToDictionary(x => x.Key, x => x.Value);
    }

    public ITrait? As(Type type) => Traits[type];

    public T? As<T>()
        where T : ITrait
    {
        if (Traits.TryGetValue(typeof(T), out var trait) && trait is T t) return t;

        return Traits.Values.OfType<T>().FirstOrDefault();
    }

    public IMutableTrait<T>? AsMutableTrait<T>()
    {
        foreach (var trait in Traits.Values)
        {
            if (trait is IMutableTrait<T> t) return t;
        }

        return default;
    }

    public ITrait<T>? AsTrait<T>()
    {
        foreach (var trait in Traits.Values)
        {
            if (trait is ITrait<T> t) return t;
        }

        return default;
    }

    public T? AsValue<T>()
    {
        foreach (var trait in Traits.Values)
        {
            if (trait is ITrait<T> t) return t.Trait;
            if (trait is IMutableTrait<T> mt) return mt.Trait;
        }

        return default;
    }

    public ITrait? FromValue<T>()
    {
        foreach (var trait in Traits.Values)
        {
            if (trait is ITrait<T>) return trait;
            if (trait is IMutableTrait<T>) return trait;
        }

        return default;
    }

    public override string ToString()
    {
        return string.Join(", ", Traits.Values.Select(x => $"({x})"));
    }

    protected IDictionary<Type, ITrait> Traits { get; }
}
