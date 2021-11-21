using UnityEngine.UIElements;

namespace Voxell.UI
{
  public class SplitView : TwoPaneSplitView
  {
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> {}
  }
}
