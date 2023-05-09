using UnityEngine;

public class PropertyInjector
{
    private LevelHandler levelHandler;
    private IInputHandler inputHandler;
    private IEventHandler eventHandler;

    public GameObject Level
    {
        get { return inputHandler.level; }
        set { inputHandler.level = value; }
    }

    public IInputHandler InputHandler
    {
        get { return inputHandler; }
        set { inputHandler = value; }
    }

    public IEventHandler EventHandler
    {
        get { return eventHandler; }
        set { eventHandler = value; }
    }

    public void InitializeHandlers()
    {
        if (eventHandler == null)
        {
            eventHandler = UnityEngine.Object.FindObjectOfType<EventSystemHandler>();
        }

        levelHandler = UnityEngine.Object.FindObjectOfType<LevelHandler>();
        inputHandler = UnityEngine.Object.FindObjectOfType<InputHandler>();
        inputHandler.level = levelHandler.level;
    }
}
