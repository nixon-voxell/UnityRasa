using UnityEngine;
using Voxell.Inspector;

namespace Voxell.Rasa
{
  public class RasaTreeRunner : MonoBehaviour
  {
    public RasaTree rasaTree;
    public RasaNLP rasaNLP;

    [Button]
    private void Start()
    {
      rasaTree.ResetTree();
      rasaNLP.Init();
    }

    [Button]
    private void Update()
    {
      RasaState rasaState = rasaTree.UpdateTree(ref rasaNLP);
      // as an example, we just reset the tree when the entire tree has been completed
      if (rasaState == RasaState.Success) rasaTree.ResetTree();
    }
  }
}