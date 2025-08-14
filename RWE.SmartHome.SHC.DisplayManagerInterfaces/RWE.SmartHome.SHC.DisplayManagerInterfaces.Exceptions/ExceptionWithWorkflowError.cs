using System;

namespace RWE.SmartHome.SHC.DisplayManagerInterfaces.Exceptions;

public class ExceptionWithWorkflowError : Exception
{
	public WorkflowError WorkflowError { get; set; }

	public ExceptionWithWorkflowError(Exception innerException, WorkflowError workflowError)
		: base(workflowError.ToString(), innerException)
	{
		WorkflowError = workflowError;
	}

	public static T WrapException<T>(Func<T> function, WorkflowError workflowError)
	{
		try
		{
			return function();
		}
		catch (Exception innerException)
		{
			throw new ExceptionWithWorkflowError(innerException, workflowError);
		}
	}

	public static void WrapException(Action action, WorkflowError workflowError)
	{
		try
		{
			action();
		}
		catch (Exception innerException)
		{
			throw new ExceptionWithWorkflowError(innerException, workflowError);
		}
	}
}
