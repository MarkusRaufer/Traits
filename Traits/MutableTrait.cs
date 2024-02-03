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
﻿namespace Traits
{
    public class MutableTrait<T> 
        : IMutableTrait<T>
        , IEquatable<MutableTrait<T>>
    {
        private T _trait;

        public MutableTrait(T trait)
        {
            _trait = trait ?? throw new ArgumentNullException(nameof(trait));
        }

        public T Trait
        {
            get => _trait;
            set =>_trait = value ?? throw new ArgumentNullException(nameof(value));
        }

        public override bool Equals(object? obj) => Equals(obj as MutableTrait<T>);

        public bool Equals(MutableTrait<T>? other)
        {
            return null != other && Trait!.Equals(other.Trait);
        }

        public override int GetHashCode() => Trait!.GetHashCode();

        public override string ToString() => $"{Trait}";
    }
}
