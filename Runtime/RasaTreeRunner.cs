using UnityEngine;
using Voxell.Inspector;

namespace Voxell.Rasa
{
  public class RasaTreeRunner : MonoBehaviour
  {
    public RasaTree rasaTree;
    public RasaNLP rasaNLP;

    [Button]
    void Start()
    {
      rasaTree.ResetTree();
      rasaNLP.Init();
    }

    [Button]
    void Update()
    {
      RasaState rasaState = rasaTree.UpdateTree(ref rasaNLP);
      if (rasaState == RasaState.Success) rasaTree.ResetTree();
    }
  }
}