# CQRS Framework

Advantages of CQRS:
- drasticly simplifies client-server comunication
- only one endpoint (ex. /CqrsBus) with one (or many) method (ex POST and/or GET)
- unified way of transfering data (serialized classes called `Messages`)
- `Messages` are splited into `Commands` (changing system state) and `Queries` (only for quering state)
- every `Message` has always one `MessageHandler`
- every `Handler` has access to IoC
- `Handlers` are easy to test
- easy error handling
- no MVC controllers needed

## Nuget's

- tBlabs.Cqrs
- tBlabs.Cqrs.Middleware

## Example of usage

Anywhere in the code define a `Message` (`Command` or `Query`):
```
	public class SampleCommand : ICommand
	{
		public string Foo { get; set; }
	}

	public class SampleQuery : IQuery<int>
	{
		public int Foo { get; set; }
	}

```
You also need to define a `Handler` for your `Message`, like so:
```
	public class SampleCommandHandler : ICommandHandler<SampleCommand>
	{
		public Task Handle(SampleCommand command)
		{
			return Task.CompletedTask;
		}
	}

	public class SampleQueryHandler : IQueryHandler<SampleQuery, Task<int>>
	{
		public Task<int> Handle(SampleQuery query)
		{
			return Task.FromResult(query.Foo * 2);
		}
	}
```
To call `Handler` with `Message` use `MessageBus.Execute()`, like here:
```
var message = "{ 'TestQuery': { 'Foo': 2 } }";
var result = await _messageBus.Execute(message); 
result.ShouldBe(4);
```
But - in case of Web API - it's better to use a dedicated middleware from `tbLabs.Cqrs.Middleware` Nuget (more in next section).

## Files upload

There is also one more type of `Message`:
- `ICommandWithStream` or
- `IQueryWithStream`

We send it like regular `Command` or `Query`. The only difference is on the `Handler` side: every message will get an extra property - `Stream` of Stream type which is a file itself.

## Usage in WebAPI

To use this framework with `.NET Core Web API` you may need one more usefull package: `tBlabs.Cqrs.Middleware` for handling requests.  
In pipeline configuration just use that code:
```
services.UseCqrsBus();
```
And that's all. CQRS Framework may work with standard MVC (but what's the point? :)

### Middleware options

Middleware may be configured with `CqrsBusMiddlewareOptions` object, like so:
```
app.UseCqrsBus(new CqrsBusMiddlewareOptions() { EndpointUrl = "/SomeEndpoint" });
```
Default value for `EndpointUrl` is `/CqrsBus` so there is no need to define one. Every HTTP method is accepted.

## Javascript client

A lot of examples for Javascript Web Browser client is in `WebApiHost` project in `StaticFiles` folder.
To run these examples run `WebApiHost` and go to `http://localhost:{port}/files/test.html` page or copy them from `/StaticFiles/test.html`.

## CQRS vs MVC

You can treat CQRS as simplified MVC without Controllers and all those REST overhead.
CQRS has only one endpoint, one http method and only way to transfer data: by serialized classes.
Data can be returned in any form (as primitive or object).
Exception are handled in unified way. You just need to throw an exception of any kind and don't need to care about HTTP codes and stuff.
This really simplifies life. Belive me ;)
