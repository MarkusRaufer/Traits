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

public static class TraitExtensions
{
    /// <summary>
    /// returns an object that implements a specific mutable trait.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trait"></param>
    /// <returns></returns>
    public static T? As<T>(this IMutableTrait<T> trait)
    {
        ArgumentNullException.ThrowIfNull(trait, nameof(trait));

        if (trait.Trait is T value) return value;

        return default;
    }

    /// <summary>
    /// Returns an object that implements a specific trait.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trait"></param>
    /// <returns></returns>
    public static T? As<T>(this ITrait trait)
    {
        ArgumentNullException.ThrowIfNull(trait, nameof(trait));

        if (trait is ITrait<T> immutable) return immutable.Trait;

        if (trait is IMutableTrait<T> mutable) return mutable.Trait;

        if (trait is ICompositeTrait composite) return composite.AsValue<T>();

        return default;
    }

    /// <summary>
    /// Returns a an object that implements a specific trait.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trait"></param>
    /// <returns></returns>
    public static T? As<T>(this ITrait<T> trait)
    {
        ArgumentNullException.ThrowIfNull(trait, nameof(trait));

        if (trait.Trait is T value) return value;

        return default;
    }

    /// <summary>
    /// Creates a composite trait from single traits.
    /// </summary>
    /// <param name="lhs">single trait.</param>
    /// <param name="rhs">list of traits.</param>
    /// <returns></returns>
    public static ICompositeTrait Compose(this ITrait lhs, params ITrait[] rhs)
    {
        return new CompositeTrait(rhs.Append(lhs));
    }

    public static IMutableCompositeTrait ComposeMutable(this ITrait lhs, params ITrait[] rhs)
    {
        return new MutableCompositeTrait(rhs.Append(lhs));
    }

    /// <summary>
    /// Sets a trait as value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="trait"></param>
    /// <param name="value"></param>
    public static void SetTraitValue<T>(this IMutableTrait<T> trait, T value)
    {
        trait.Trait = value;
    }

    public static ICompositeTrait ToCompositeTrait(this ITrait trait)
    {
        var composite = new CompositeTrait(Array.Empty<ITrait>().Append(trait));

        return composite;
    }

    public static IMutableTrait<T>? ToMutableTrait<T>(this ITrait trait)
    {
        ArgumentNullException.ThrowIfNull(trait, nameof(trait));

        if (trait is IMutableTrait<T> mutable) return mutable;

        if (trait is ITrait<T> immutable) return new MutableTrait<T>(immutable.Trait);

        if (trait is ICompositeTrait composite)
        {
            var found = composite.FromValue<T>();
            if (found is IMutableTrait<T> mt) return mt;

            if (found is ITrait<T> t) return new MutableTrait<T>(t.Trait);
        }

        return default;
    }

    public static ITrait<T>? ToTrait<T>(this ITrait trait)
    {
        ArgumentNullException.ThrowIfNull(trait, nameof(trait));

        if (trait is ITrait<T> immutable) return immutable;

        if (trait is IMutableTrait<T> mutable) return new TraitImpl<T>(mutable.Trait);

        if (trait is ICompositeTrait composite)
        {
            var found = composite.FromValue<T>();
            if (found is ITrait<T> t) return t;

            if (found is IMutableTrait<T> mt) return new TraitImpl<T>(mt.Trait);
        }

        return default;
    }
}
