using RWE.SmartHome.SHC.Core;

namespace RWE.SmartHome.SHC.DisplayManagerInterfaces;

public interface IDisplayManager : IService
{
	void WorkflowProceeded(Workflow workflow, WorkflowMessage message, bool forceDisplay);

	void WorkflowDynamicMessage(Workflow workflow, string message);

	void WorkflowFinished(Workflow workflow);

	void WorkflowFailed(Workflow workflow, WorkflowError workflowError);

	bool UserHasRequestedFactoryReset();
}
