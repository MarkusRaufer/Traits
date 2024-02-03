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
﻿using ConsoleApp1;
using ConsoleApp1.Animals;
using ConsoleApp1.IssueDomain;
using ConsoleApp1.Planning;
using Traits;

//IssueTest();

AnimalTest();

//ExtendType();

void IssueTest()
{
    var issue = new Issue("T0001", "my task");

    var priorizable = issue.As<Prioritizable>();
    Console.WriteLine($"Priority: {priorizable.Priority}");

    issue.SetTraitValue(new Prioritizable(2));

    priorizable = issue.As<Prioritizable>();

    Console.WriteLine($"Priority: {priorizable.Priority}");

    var plannable = issue.As<Plannable>()!;

    plannable.EstimatedBegin = DateTime.Now;
    plannable.EstimatedEnd = plannable.EstimatedBegin + TimeSpan.FromHours(8);

    Console.WriteLine($"begin: {plannable.EstimatedBegin}, end: {plannable.EstimatedEnd}, duration: {plannable.EstimatedDuration}");
}

void AnimalTest()
{
    var duck = new Duck();
    duck.Chirp();
    duck.Quack();
    var bird = duck.As<Bird>();
    
    Console.WriteLine("---------------------------");

    var wolpertinger = new Wolpertinger();

    wolpertinger.Giggle();

    Console.WriteLine("---------------------------");

    wolpertinger.Bubble();

    wolpertinger.Chirp();

    var dog = wolpertinger.As<Dog>()!;

    var numberOfLegs = dog.NumberOfLegs;
    dog.NumberOfLegs = 3;
    dog.Bark();

    bird = wolpertinger.As<Bird>()!;
    bird.NumberOfWings = 0;

    bird.ToTrait().Chirp();

    var fish = wolpertinger.As<Fish>()!;
}

void ExtendType()
{

    var x = 5.ToTrait().ComposeMutable(new DebugMode(true), new HasName("Stars"));

    var number = x.AsValue<int>();
    
    Console.WriteLine(number);

    var debugMode = x.As<DebugMode>()!;
    Console.WriteLine(debugMode);

    x.SetTrait(new DebugMode(false));

    debugMode = x.As<DebugMode>();
    Console.WriteLine(debugMode);

    var hasName = x.As<HasName>();
    var mut = hasName.ToMutableTrait();
    mut.SetTraitValue(new HasName("Rating"));

    Console.WriteLine(hasName);
}
