# CQRS Framework

Advantages of CQRS:
- one endpoint (/CqrsBus) with one method (POST)
- unified way of transfering data (serialized classes called Messages)
- Messages are splited into Commands (changing system state) and Queries (only for quering state)
- every Message has always one Handler
- no MVC controllers needed

## Example of usage

Anywhere in the code define:
- Message:
```
	public class TestQuery : IQuery<int>
	{
		public int Value { get; set; }
	}
```
- and Handler for Message:
```
	public class TestQueryHandler : IQueryHandler<TestQuery, Task<int>>
	{
		public Task<int> Handle(TestQuery query)
		{
			return Task.FromResult(query.Value * 2);
		}
	}
```
- to call Handler with Message use:
```
var result = await _messageBus.ExecuteFromJson("{ 'TestQuery': { "Value": 2 } }"); // result will be 4
```

## Usage in WebAPI

To use framework with .NET Core WebApi you may need one more usefull package: **tBlabs.Cqrs.Middleware** for handling requests.
In pipe line configuration just use that code:
```
services.UseCqrsBus();
```
And that's all. CQRS Framework may work with standard Mvc but what's the point? :)

## Javascript client example
```
try {
	const message = new TestQuery();
	message.Value = 2;
	const requestBody = JSON.stringify({ [message.Name]: message });
	const response = await fetch('http://app.domain/CqrsBus' { method: 'POST', headers: ..... });
	await data = await response.json();
	console.log('Response data: ', data);
}
catch (error) {
	console.log(error);
}

```
## CQRS vs MVC

You can treat CQRS as simplified MVC without Controllers and all those REST overhead.
CQRS has only one endpoint and only one method and only way of transfer data: by serialized classes.
Data can be returned in any form (as primitive or object).
Exception are handled in unified way. You just need to throw an exception of any kind and don't need to care about HTTP codes and stuff.

