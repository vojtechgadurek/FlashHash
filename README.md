# Flash Hash

## About
FlashHash is a fast implementation of many (now two) standard hashing function families (Linear Congruence and Multiply Shift). It is based on expression trees. 
It provides unlimited hashing functions from these families with the same performance as hashing functions written during compile time.

## Performance 
This library provides Circe four times faster hashing functions than functions gained by this approach:

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
But the creation process is much more expensive, sitting at 0.5 ms (The code is compiled during runtime). 

## Usage
There are there main objects: Schemes, Families, Provider

Families are abstractions for hashing function families and schemes are descriptions of individual hashing functions.

### Schemes
``` cs
var scheme = new LinearCongruenceScheme(10, 5, 7)
//from this scheme, we are going to build this hashingfunction:
//  (10*x + 5) % Prime % 7
// Prime is closet mersene prime greater than size (7)

Expression<Func<ulong, ulong>> expression = scheme.Create()
//Building expression tree is not expensive

Func<ulong, ulong> hashingFunction = expression.Compile();
//Compiling actually is 
```

### Families
``` cs
var family = new LinearCongruenceFamily();
family.SetRandomness(new Random(42));
//You can set your own randomness
ulong size = 10;
Func<ulong, ulong> hashingFunction = family.GetScheme(size).Create().Compile()
//Returns random hashing function from linear congruence family
```

### Provider
They are useful when you need to get Hashing Functions by their type.

``` cs

//You can set your own randomness
ulong size = 10;
Func<ulong, ulong> hashingFunction = HashingFunctionProvider.Get(typeof(LinearCongruenceFamily), size).Create().Compile();
//Returns random hashing function from linear congruence family
//HashingFunctionProvider via reflection finds every class implementing IHashingFunctionFamily interface
```

### Tips
As the newly created function is a delegate for better performance you can buffer it:

``` cs
using LittleSharp.Utils
//You can set your own randomness
Expression<Func<ulong, ulong>> hashingFunction = HashingFunctionProvider.Get(typeof(LinearCongruenceFamily), size).Create();
Action<ulong[], ulong[], int, int> bufferedHashingFunction = Buffers.BufferFunction(hashingFunction);
//Returns random hashing function from linear congruence family
//HashingFunctionProvider via reflection finds every class implementing IHashingFunctionFamily interface
```

Also for better expression trees, I advise to check LittleSharp.





