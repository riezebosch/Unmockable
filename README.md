# Unmockable

Imagine you need a dependency on a 3rd party SDK where all types carry no interfaces, and all methods are not virtual.
Your only option is writing a wrapper that either implements an interface or has it methods marked virtual and does nothing 
more than passing through calls to the underlying object.

That's where this tiny library comes in. It acts as that handwritten wrapper for you.  

## Not a replacement 

For dependencies you have under control, introduce interfaces and regular mocking frameworks like [NSubstitute](https://nsubstitute.github.io/) and [Moq](https://github.com/moq/moq). 
Kindly send an email to the vendor of the SDK you're using if they could pretty please introduce some interfaces. It is a no brainer
to extract an interface and it helps you to do [SOLID](https://en.wikipedia.org/wiki/SOLID).


## Feature slim

All mocks are strict, each invocation requires explicit setup, and there are no wild card argument matchers.
Probably we need added some argument matcher for reference types in the feature because matching currently relies on
the hash code of the value of the arguments.

## Usage

I prefer `NSubstitute` over `Moq` for its clean API. However, since we are (already) dealing
with `Expressions,` I felt it was more convenient (and easier for me to implement) to resemble the `Moq` API.  

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

Inject the wrapper object:

```cs
services
    .AddScoped<IUnmockable<HttpClient>, Wrap<HttpClient>>();
services
    .AddScoped<HttpClient>();
```

Inject an interceptor from a test:

```cs
var client = new Intercept<HttpClient>();
client
    .Setup(x => x.DownloadAsync(...))
    .Returns(Task.Completed);

var target = new SomeLogic(client);
await target.DoSomething(3);
```

\* The `HttpClient` is just a hand-picked example and not necessarily unmockable. 
Unmockable types I had to deal with recently are the [`ExtensionManagementHttpClient`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.services.extensionmanagement.webapi.extensionmanagementhttpclient) 
and the [`AzureServiceTokenProvider`](https://github.com/Azure/azure-sdk-for-net/blob/master/src/SdkCommon/AppAuthentication/Azure.Services.AppAuthentication/AzureServiceTokenProvider.cs).

## Shout-out

A big shoutout to Microsoft and other vendors to start **unit testing your SDKs** so you'll share our pain and give us some freaking extension points.

> [*Dependency Inversion Principle*](http://butunclebob.com/ArticleS.UncleBob.PrinciplesOfOod)  
  One should "depend upon abstractions, not on concretions."

Please, don't give us the `Unmockable<🖕>`.