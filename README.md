#Traits

Have you ever wished you had Rust like traits in C#? `Traits` is a .NET implementation of traits like those in **Rust**.
If you prefer or need **composition over inheritance**, this is a good alternative.

## What are traits?

A trait is a composition of behaviors represented by methods that can be shared. It enables you to extend types even if they are `sealed`. You can also extend C# types with multiple traits. This is called composition. You can compose new types by aggregating existing components.

## Rust example

THis is an excerpt of an example of the official Rust documentation.

```rust
trait Animal {
    // &self is a reference to the implementor of the trait.
    fn name(&self) -> &'static str;
    fn noise(&self) -> &'static str;
}

struct Sheep { naked: bool, name: &'static str }

impl Sheep {
    fn is_naked(&self) -> bool {
        self.naked
    }

    fn shear(&mut self) {
        if self.is_naked() {
            println!("{} is already naked...", self.name());
        } else {
            println!("{} gets a haircut!", self.name);

            self.naked = true;
        }
    }
}

// Implement the `Animal` trait for `Sheep`.
impl Animal for Sheep {
    fn name(&self) -> &'static str {
        self.name
    }

    fn noise(&self) -> &'static str {
        if self.is_naked() {
            "baaaaah?"
        } else {
            "baaaaah!"
        }
    }
}

fn main() {
    let mut dolly: Sheep = Animal::new("Dolly");

    dolly.noise();
    dolly.shear();
    dolly.noise();
}
```

## C# Examples

### Composition Instead Of Inheritance

You all know the problem of the **decorator pattern**. We always have to do the annoying mapping of the aggregated components which interfaces we inherit.

#### With standard Object Orientied Design (OOD)

```csharp
public interface IPlannable
{
    DateTime? EstimatedBegin { get; }
    TimeSpan? EstimatedDuration { get; }
    DateTime? EstimatedEnd { get; }
}

public interface IPrioritizable
{
    int Priority { get; }
}

public class Issue
    : IPlannable
    , IPrioritizable
{
    private readonly IPlannable _plannable;
    private Prioritizable _prioritizable;

    public Issue(string id, string? description)
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
```

As you can see, the annoying part is that mapping of the aggregated components.
How would it look like with a trait approach?

#### With Traits

```csharp
public record Issue(string Id, string? Description)
    : ITrait<Plannable>
    , ITrait<Prioritizable>
{
    Plannable ITrait<Plannable>.Trait { get; } = new Plannable();
    Prioritizable ITrait<Prioritizable>.Trait { get; } = new Prioritizable(0);
}
```

This is how we can access the aggregated components:

```csharp
var issue = new Issue("T0001", "my task");

// get component value with As<T>
var priorizable = issue.As<Prioritizable>();
Console.WriteLine($"Priority: {priorizable.Priority}");

var plannable = issue.As<Plannable>()!;

plannable.EstimatedBegin = DateTime.Now;
plannable.EstimatedEnd = plannable.EstimatedBegin + TimeSpan.FromHours(8);
```

##### Mutable Traits

Sometimes you want to change the value of an aggregated **immutable component** (value object). How can you do this?
For this you have to use `IMutableTrait<T>`.

Here is an example:

```csharp
public record Issue(string Id, string? Description)
    : ITrait<Plannable>
    , IMutableTrait<Prioritizable> //----> now the component can be replaced.
{
    Plannable ITrait<Plannable>.Trait { get; } = new Plannable();

    // this now has also a setter.
    Prioritizable IMutableTrait<Prioritizable>.Trait { get; set; } = new Prioritizable(0);
}

var issue = new Issue("T0001", "my task");

var priorizable = issue.As<Prioritizable>();
Console.WriteLine($"Priority: {priorizable.Priority}");

issue.SetTraitValue(new Prioritizable(2)); // repace component.

priorizable = issue.As<Prioritizable>();

Console.WriteLine($"Priority: {priorizable.Priority}");

// output:
// 
// Priority: 0
// Priority: 2
```

##### Create Trait From Sealed Type

```csharp
// example

public sealed class Bird
{
    public bool CanFly => 0 < NumberOfWings;
    public int NumberOfWings { get; set; } = 2;
}

public static class BirdImpl
{
    public static void Chirp(this ITrait<Bird> bird)
    {
        var can = bird.Trait.CanFly ? "can" : "can not";

        Console.WriteLine($"With {bird.Trait.NumberOfWings} wings I {can} fly");
    }
}

var bird = new Bird { NumberOfWings = 2 };

var birdTrait = bird.ToTrait();
birdTrait.Chirp();
```

##### Inherit Multiple Traits

```csharp
public class Dog : ITrait<Dog>
{
    public bool CanRun => 0 < NumberOfLegs;
    public int NumberOfLegs { get; set; } = 4;
    public Dog Trait => this;
}

public static class DogImpl
{
    public static void Bark(this ITrait<Dog> dog)
    {
        var run = 4 == dog.Trait.NumberOfLegs ? "run fast" : "walk";

        Console.WriteLine($"With {dog.Trait.NumberOfLegs} legs I can {run}");
    }
}

public class Fish : ITrait<Fish>
{
    public bool CanSwim => true;
    public Fish Trait => this;
}

public static class FishImpl
{
    public static void Bubble(this ITrait<Fish> fish)
    {
        var can = fish.Trait.CanSwim ? "can" : "can not";
        Console.WriteLine($"I {can} swim");
    }
}

public class Wolpertinger
    : ITrait<Wolpertinger>
    , ITrait<Bird>
    , ITrait<Dog>
    , ITrait<Fish>
{
    Bird ITrait<Bird>.Trait { get; } = new Bird();
    Dog ITrait<Dog>.Trait { get; } = new Dog();
    Fish ITrait<Fish>.Trait { get; } = new Fish();
    public Wolpertinger Trait => this;
}

public static class WolpertingerImpl
{
    public static void Giggle(this ITrait<Wolpertinger> wolpertinger)
    {
        Console.WriteLine("Giggle");
        Console.WriteLine();
        
        wolpertinger.Trait.Bark();
        wolpertinger.Trait.Bubble();
        wolpertinger.Trait.Chirp();
    }
}

var wolpertinger = new Wolpertinger();

wolpertinger.Giggle();

wolpertinger.Bubble();
wolpertinger.Chirp();
wolpertinger.Bark();
```

##### Create Trait Dynamically

```csharp
public record class DebugMode(bool Debug) : ITrait<DebugMode>
{
    public DebugMode Trait => this;
    public override string ToString() => $"Debug: {Debug}";
}

public record class HasName(string Name) : ITrait<HasName>
{
    public HasName Trait => this;
    public override string ToString() => $"Name: {Name}";
}

var x = 5.ToTrait().ComposeMutable(new DebugMode(true), new HasName("Stars"));

var number = x.AsValue<int>();

var debugMode = x.As<DebugMode>()!;
x.SetTrait(new DebugMode(false));

 debugMode = x.As<DebugMode>();
 
 var hasName = x.As<HasName>();
 var mut = hasName.ToMutableTrait();
 mut.SetTraitValue(new HasName("Rating"));
```
