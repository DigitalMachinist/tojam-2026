using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewPlayModeTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void NewPlayModeTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
        Assert.Pass();
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator NewPlayModeTestScriptWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
        Assert.Pass();
    }
}
