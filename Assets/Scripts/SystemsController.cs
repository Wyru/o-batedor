
public static class SystemsController
{
    public static void RunningDialogue(bool b){
        InteractSystem.running = !b;
        PlayerMovement.running = !b;
    }
}
