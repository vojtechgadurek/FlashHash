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
There are there main objects: Schemes, Families, Providers

Families are abstractions for hashing function families and schemes are descriptions of individual hashing functions.

``` cs
var scheme = new LinearCongruenceScheme(10, 5, 7)
//from this scheme, we are going to build this hashingfunction:
//  (10*x + 5) % Prime % 7
// Prime is closet mersene prime greater than size (7)

Expression<Func<ulong, ulong>> expression = scheme.Create()
//Building expression tree is not expensive

Func<ulong, ulong> hashingFunction = expression.Compile()
//Compiling actually is 
```

