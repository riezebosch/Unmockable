[![build status](https://ci.appveyor.com/api/projects/status/layshtg2fh5fu5fu/branch/master?svg=true)](https://ci.appveyor.com/project/riezebosch/unmockable/branch/master)
[![nuget](https://img.shields.io/nuget/v/Unmockable.svg)](https://www.nuget.org/packages/Unmockable/)

# 📢 Shout-out

A big shoutout to Microsoft and other vendors to start **unit testing your SDKs** so you'll share our pain and give us some freaking extension points.

> [_Dependency Inversion Principle_](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod)
> One should "depend upon abstractions, not on concretions."

Please, don't give us the `Unmockable<🖕>`.

## Support

Please, [retweet](https://twitter.com/MRiezebosch/status/1103973591782166528) to support this petition and `@mention` your vendor.

# Unmockable

Imagine you need a dependency on a 3rd party SDK where all types carry no interfaces, and all methods are not virtual.
Your only option is writing a wrapper that either implements an interface or has its methods marked virtual and does nothing
more than passing through calls to the underlying object.

That's where this tiny library comes in. It acts as that handwritten wrapper for you.

## Not a replacement

For dependencies under control, introduce interfaces, and use a regular mocking framework like [NSubstitute](https://nsubstitute.github.io/) or [Moq](https://github.com/moq/moq).
Kindly send an email to the vendor of the SDK you're using if they could pretty please introduce some interfaces. It is a no-brainer
to extract an interface, and it helps us to be [SOLID](https://en.wikipedia.org/wiki/SOLID).

## Feature slim

This library has a particular purpose and is deliberately not a full-fledged mocking framework. Therefore
I try to keep its features as slim as possible, meaning:

-   All mocks are strict; each invocation requires explicit setup.
-   <s>There are are no</s> wild card [argument matchers](#Matchers).
-   The API is straightforward and sparse.
-   Wrapping `static` classes is [not supported](#Statics).

That being said, I genuinely believe in _pure TDD_, so everything is written from a
red-green-refactor cycle, and refactoring is done with [SOLID principles](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod)
and [Design Patterns](https://dofactory.com/net/design-patterns) in hand.
If you spot a place where another pattern could be applied, don't hesitate to let me know.

## Different

What makes it different from [Microsoft Fakes](https://docs.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes), [Smocks](https://www.nuget.org/packages/Smocks/), or
 [Pose](https://github.com/tonerdo/pose) is that it only uses C# language constructs. There is no runtime rewriting or reflection/emit under the hood. Of course, this impacts the way you wrap and use
your dependency, but please, don't let us clean up someone else's 💩.

## Usage

I prefer `NSubstitute` over `Moq` for its crisp API. However, since we are (already) dealing
with `Expressions,` I felt it was more convenient (and easier for me to implement) to resemble the `Moq` API.

### 💉 Inject

Inject an unmockable\* object:

```c#
public class SomeLogic
{
    public SomeLogic(IUnmockable<HttpClient> client)
    {
        _client = client;
    }
}
```

Execute using an expression:

```c#
public async Task DoSomething(int input)
{
    await _client.Execute(x => x.DownloadAsync(...));
}
```

\* The `HttpClient` is just a hand-picked example and not necessarily unmockable. There have been [some debate](https://github.com/aspnet/HttpClientFactory/issues/67)
around this type. Technically it is mockable, as long as you are not afraid of message handlers.

Concrete unmockable types (pun intented) I had to deal with recently are the [`ExtensionManagementHttpClient`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.services.extensionmanagement.webapi.extensionmanagementhttpclient)
and the [`AzureServiceTokenProvider`](https://github.com/Azure/azure-sdk-for-net/blob/master/src/SdkCommon/AppAuthentication/Azure.Services.AppAuthentication/AzureServiceTokenProvider.cs).

### ↪️ Intercept

Inject an interceptor from a test using [Unmockable.Intercept](https://www.nuget.org/packages/Unmockable.Intercept/):

```c#
var client = new Intercept<HttpClient>();
client
    .Setup(x => x.DownloadAsync(...))
    .Returns(...);

var target = new SomeLogic(client);
await target.DoSomething(3);

client.Verify();
```

Only strict 'mocks' are supported, meaning all invocations require setup, and all setups demand invocation.
Using strict mocks saves you from `NullReferenceExceptions` and makes verification easy.

If you really want a [stub instead of a mock](https://martinfowler.com/articles/mocksArentStubs.html),
I'd recommend [auto-mocking](https://github.com/AutoFixture/AutoFixture/wiki/Cheat-Sheet#auto-mocking-with-moq) with [AutoFixture](https://github.com/AutoFixture/AutoFixture):

```c#
var fixture = new AutoFixture();
fixture
    .Customize(new AutoConfiguredNSubstituteCustomization());

var client = fixture
    .Create<IUnmockable<HttpClient>>();

var target = new SomeLogic(client);
await target.DoSomething(3);
```

### 🎁 Wrap

Inject the wrapper object using [Unmockable.Wrap](https://www.nuget.org/packages/Unmockable.Wrap/):

```c#
services
    .AddTransient<IUnmockable<HttpClient>, Wrap<HttpClient>>();
services
    .AddScoped<HttpClient>();
```

Or wrap an existing object:

```c#
var client = new HttpClient().Wrap();

```

Or add wrappers for all services with [Unmockable.DependencyInjection](https://www.nuget.org/packages/Unmockable.DependencyInjection/):

```c#
services
    .AddScoped<HttpClient>();
services
    .AddUnmockables();
```

**Remark:** The expressions are compiled at runtime _on every invocation_, so there is a performance penalty.
I tried to add caching here, but that turns out not to be a sinecure.

## Matchers

Collection arguments get unwrapped when matching the actual call with provided setups! Value types, anonymous types _and_
classes with a custom `GetHashCode()` & `Equals()` should be safe. You can do custom matching with `Arg.Ignore<T>()` and `Arg.Where<T>(x => ...)`, though the recommendation
 is to be explicit.

## Optional arguments not allowed in expressions

> An expression tree cannot contain a call or invocation that uses optional arguments

You can use the `default` literal for all arguments (in C# 7.1.), but be aware that this is the default
value of the _type_, which is not necessarily the same as the default value specified for the _argument_!

```c#
client.Execute(x => x.InstallExtensionByNameAsync("some-value", "some-value", default, default, default));
```

On the plus side, you now have to make it explicit both on the `Execute` and the `Setup` end making it less error-prone.

## Unmockable unmockables

What if your mocked [unmockable object returns an unmockable object](https://docs.microsoft.com/en-us/azure/cosmos-db/tutorial-develop-table-dotnet#create-a-table)?!
Just wrap the (in this case) data fetching functionality in a separate class, test it heavily using integration tests, and inject
that dependency into your current system under test.

## Static classes

At first, I added, but then I removed support for 'wrapping' static classes and invoking static methods.
In the end, it is not an unmockable _object_! If you're dependent, let's say, on `DateTime.Now` you can add a method overload
that accepts a specific DateTime. You don't need this framework for that.

```c#
public void DoSomething(DateTime now)
{
    if (now ...) {}
}

public void DoSomething() => DoSomething(DateTime.Now)
```

Or with a factory method if it has to be more dynamic.

```c#
public void DoSomething(Func<DateTime> now)
{
    while (now() <= ...) {}
}

public void DoSomething() => DoSomething(() => DateTime.Now)
```

If you don't like this change in your public API, you can extract an interface and only
include the second method (which is a good idea anyway), or you mark the overloaded method internal and
expose it to your test project with the `[InternalsVisibleTo]` attribute.


Happy coding!
