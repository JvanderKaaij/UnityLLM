using System.Collections;
using System.Threading.Tasks;

public static class EditorCoroutine
{
    public static async void Run(IEnumerator coroutine)
    {
        while (coroutine.MoveNext())
        {
            await Task.Delay(10); // wait for 10 ms or adjust as necessary
        }
    }
}