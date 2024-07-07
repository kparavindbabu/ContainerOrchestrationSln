namespace ContainerOrchestration.Api;

public class InstanceService
{
    public InstanceService()
    {
        InstanceId = Guid.NewGuid();
    }
    
    public Guid InstanceId { get; private set; }
}