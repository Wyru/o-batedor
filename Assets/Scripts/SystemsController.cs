
public static class SystemsController
{
    public static void RunningDialogue(bool b){
        InteractSystem.running = !b;
        PlayerMovement.running = !b;
    }

    public static void RunningAlbum(bool b){
        InteractSystem.running = !b;
        PlayerMovement.running = !b;
    }
}
