using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineCollection : CustomYieldInstruction
{
    int numCoroutinesRunning;

    public IEnumerator CollectCoroutine(IEnumerator coroutine)
    {
        //Increase the number of coroutines running
        numCoroutinesRunning++;

        yield return coroutine;

        //Decrease the number of coroutines running once the coroutine has finished
        numCoroutinesRunning--;
    }

    //Wait until the number of coroutines running is 0
    public override bool keepWaiting => numCoroutinesRunning > 0;
}
