[![build status](https://ci.appveyor.com/api/projects/status/layshtg2fh5fu5fu/branch/master?svg=true)](https://ci.appveyor.com/project/riezebosch/unmockable/branch/master)
[![nuget](https://img.shields.io/nuget/v/Unmockable.svg)](https://www.nuget.org/packages/Unmockable/)

# Unmockable

Imagine you need a dependency on a 3rd party SDK where all types carry no interfaces, and all methods are not virtual.
Your only option is writing a wrapper that either implements an interface or has it methods marked virtual and does nothing 
more than passing through calls to the underlying object.

That's where this tiny library comes in. It acts as that handwritten wrapper for you.

## Not a replacement 

For dependencies you have under control, introduce interfaces and regular mocking frameworks like [NSubstitute](https://nsubstitute.github.io/) and [Moq](https://github.com/moq/moq). 
Kindly send an email to the vendor of the SDK you're using if they could pretty please introduce some interfaces. It is a no brainer
to extract an interface and it helps you to be [SOLID](https://en.wikipedia.org/wiki/SOLID).

## Feature slim

All mocks are strict, each invocation requires explicit setup, <s>and there are no wild card argument matchers</s>.

## Different

What makes it different from [Microsoft Fakes](https://docs.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes) or [Smocks](https://www.nuget.org/packages/Smocks/) is
that it only uses C# language constructs. There is no runtime rewriting or reflection/emit under the hood. Of course, this impacts the way you wrap and use
your dependency, but please, don't let us clean up someone else's dirt.

To be honest, [Pose](https://github.com/tonerdo/pose) looks also promising!

## Usage

I prefer `NSubstitute` over `Moq` for its clean API. However, since we are (already) dealing
with `Expressions,` I felt it was more convenient (and easier for me to implement) to resemble the `Moq` API.  

### Inject

Inject an unmockable* object:

```cs
public class SomeLogic
{
    public SomeLogic(IUnmockable<HttpClient> client)
    {
        _client = client;
    }
}
```

Execute using an expression:

```cs
public async Task DoSomething(int input)
{
    await _client.Execute(x => x.DownloadAsync(...));
}
```

### Wrap

Inject the wrapper object using [Unmockable.Wrap](https://www.nuget.org/packages/Unmockable.Wrap/):

```cs
services
    .AddTransient<IUnmockable<HttpClient>, Wrap<HttpClient>>();
services
    .AddScoped<HttpClient>();
```

Or wrap an existing object:

```cs
var client = new HttpClient().Wrap();
    
```

Or add wrappers for all services with [Unmockable.DependencyInjection](https://www.nuget.org/packages/Unmockable.DependencyInjection/):

```cs
services
    .AddScoped<HttpClient>();
services
    .AddUnmockables();
```

**Remark:** The expressions are compiled on every invocation so it'll affect performance. 
I tried to add caching here but that turns out not te be a sinecure.

### Intercept

Inject an interceptor from a test using [Unmockable.Intercept](https://www.nuget.org/packages/Unmockable.Intercept/):

```cs
var client = new Intercept<HttpClient>();
client
    .Setup(x => x.DownloadAsync(...))
    .Returns(...);

var target = new SomeLogic(client);
await target.DoSomething(3);

client.Verify();
```

\* The `HttpClient` is just a hand-picked example and not necessarily unmockable. There have been [some debate](https://github.com/aspnet/HttpClientFactory/issues/67)
around this type and it turns out to be mockable. As long as you are not afraid of message handlers. 

Concrete unmockable types (pun intented) I had to deal with recently are the [`ExtensionManagementHttpClient`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.services.extensionmanagement.webapi.extensionmanagementhttpclient) 
and the [`AzureServiceTokenProvider`](https://github.com/Azure/azure-sdk-for-net/blob/master/src/SdkCommon/AppAuthentication/Azure.Services.AppAuthentication/AzureServiceTokenProvider.cs).

## Optional arguments

Because of the `Expressions`, it is not possible to use default values from optional arguments.
> An expression tree cannot contain a call or invocation that uses optional arguments
 
Luckily this is easily solved by passing in `default` for all arguments:

```cs
client
    .Setup(x => x.InstallExtensionByNameAsync("asdf", "setvar", default, default, default))
    .Returns(new InstalledExtension());
``` 

**Remark**: This is the default value of the *type*, not necessarily the same as the default value of the *optional argument*!

## Matchers

<s>Unfortunately dealing with inline constructed reference types is (currently) not supported. I deliberately
try not to create a `YAMF`. Strife for injecting these values from the test, so the instances are the same 
in `Setup` and `Execute`.</s> 

Collection arguments are unwrapped when matching the actual call with provided setups! Value types, anonymous types *and* reference types with a custom `GetHashCode()` should be safe.

Custom matching is done with `Arg.Ignore<T>()` and `Arg.Equals<T>(x => true/false)`, though the recommendation
still is to be explicit. 

## Statics

I first added and then removed support for 'wrapping' static classes and invoking static methods.
Because it is not an unmockable *object*! If you're dependent, let's say, on `DateTime.Now` you can already create an overloaded method that accepts the DateTime. You don't need a framework for that.

```cs
public void DoSomething(DateTime now)
{
    if (now ...) {}
}

public void DoSomething() => DoSomething(DateTime.Now)
```

Or with a factory method.
```cs
public void DoSomething(Func<DateTime> now)
{
    if (now() ...) {}
}

public void DoSomething() => DoSomething(() => DateTime.Now)
```

If you don't like this as a public API, you can extract an interface and only
include the second method or you mark the top method internal and
make it visible to your test project using `[InternalsVisibleTo]`.  

## &#128226; Shout-out

A big shoutout to Microsoft and other vendors to start **unit testing your SDKs** so you'll share our pain and give us some freaking extension points.

> [*Dependency Inversion Principle*](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod)  
> One should "depend upon abstractions, not on concretions."

Please, don't give us the `Unmockable<🖕>`.

## Support 

Please, retweet:

[![tweet](tweet.png)](https://twitter.com/MRiezebosch/status/1103973591782166528)

