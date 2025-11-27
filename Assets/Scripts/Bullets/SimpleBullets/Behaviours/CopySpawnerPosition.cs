public class CopySpawnerTransform : BulletBehaviour
{
    private void Update()
    {
        transform.position = bullet.Launcher.transform.position;
    }
}
