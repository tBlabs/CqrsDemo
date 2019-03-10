using System;

namespace Core.Cqrs
{
	public class AppResponse 
	{
		public bool IsException { get; set; }

	}
	public class AppResponse<T>  : AppResponse
	{
		public T Response { get; set; }
	}

	public class OkResponse : AppResponse<object>
	{
		public OkResponse(object res)
		{
			IsException = false;
			Response = res;
		}
	}

	public class ErrResponse : AppResponse<string>
	{
		public ErrResponse(Exception e)
		{
			IsException = true;
			Response = e.Message;
		}
	}

	public interface IAppResponse
	{

	}

	public interface IAppResponse<T> : IAppResponse
	{
		bool IsException { get; set; }
		T Response { get; set; }
	}

	public class SuccessResponse<T> : IAppResponse<T>
	{
		public SuccessResponse(T response)
		{
			IsException = false;
			Response = response;
		}

		public bool IsException { get; set; }
		public T Response { get; set; }
	}

	public class ErrorResponse : IAppResponse<string>
	{
		public ErrorResponse(Exception exception)
		{
			IsException = true;
			Response = exception.Message;
		}

		public bool IsException { get; set; }
		public string Response { get; set; }
	}
}
