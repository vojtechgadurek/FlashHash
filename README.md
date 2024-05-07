# Flash Hash

## Overview
FlashHash offers a fast implementation of two standard hashing function families: Linear Congruence and Multiply Shift. It leverages expression trees to deliver hashing functions (ulong to ulong) from these families, with performance akin to compile-time implementations.

## Performance 
This library offer hashing functions four times faster than those generated through conventional means:

``` cs
var CreateLinearCongruenceHashingFunction = 
    (long size) =>
    {
        ulong a = Random.GetRandomUInt64();
        ulong b = Random.GetRandomUInt64();
        ulong p = Random.GetGoodPrime(size);
        return (ulong x) => (a * x + b) % size % prime;
    }
```
However, the creation process is relatively more resource-intensive, consuming approximately 0.5 ms (as code compilation occurs at runtime).

## Usage
FlashHash revolves around three primary components: Schemes, Families, and Provider.

Families serve as abstract representations for hashing function families, while schemes describe individual hashing functions.

### Schemes
``` cs
var scheme = new LinearCongruenceScheme(10, 5, 7)
// This scheme yields the following hashing function:
//  (10*x + 5) % Prime % 7
// Prime represents the closest Mersene prime greater than the size (7)

Expression<Func<ulong, ulong>> expression = scheme.Create()
// Building an expression tree is not computationally expensive

Func<ulong, ulong> hashingFunction = expression.Compile();
// However, compilation is resource-intensive
```

### Families
``` cs
var family = new LinearCongruenceFamily();
family.SetRandomness(new Random(42));
// Custom randomness can be set
ulong size = 10;
Func<ulong, ulong> hashingFunction = family.GetScheme(size).Create().Compile()
// Retrieves a random hashing function from the linear congruence family
```

### Provider
Providers are valuable when you need to retrieve hashing functions by their type.

``` cs

// Custom randomness can be set
ulong size = 10;
Func<ulong, ulong> hashingFunction = HashingFunctionProvider.Get(typeof(LinearCongruenceFamily), size).Create().Compile();
// Retrieves a random hashing function from the linear congruence family
// HashingFunctionProvider uses reflection to identify every class implementing the IHashingFunctionFamily interface
```

### Tips
As the newly created function is a delegate for better performance, it can be buffered:

``` cs
using LittleSharp.Utils
// Custom randomness can be set
Expression<Func<ulong, ulong>> hashingFunction = HashingFunctionProvider.Get(typeof(LinearCongruenceFamily), size).Create();
Action<ulong[], ulong[], int, int> bufferedHashingFunction = Buffers.BufferFunction(hashingFunction);
// Retrieves a random hashing function from the linear congruence family
// HashingFunctionProvider uses reflection to identify every class implementing the IHashingFunctionFamily interface
```

For optimal expression trees, LittleSharp is recommended.





