using UnityEngine;
using Voxell.NLP.Tokenize;
using Voxell.NLP.SentenceDetect;
using Voxell.Inspector;

namespace Voxell.Rasa
{
  [System.Serializable]
  public struct RasaNLP
  {
    /// <summary>Checks if the tools have been initialized.</summary>
    public bool IsInitialized => maximumEntropyTokenizer != null;

    public EnglishMaximumEntropyTokenizer maximumEntropyTokenizer;
    [SerializeField, StreamingAssetFilePath]
    private string maximumEntropyTokenizerModel;

    public EnglishMaximumEntropySentenceDetector maximumEntropySentenceDetector;
    [SerializeField, StreamingAssetFilePath]
    private string maximumEntropySentenceDetectorModel;

    public void Init()
    {
      maximumEntropyTokenizer = new EnglishMaximumEntropyTokenizer(
        FileUtilx.GetStreamingAssetFilePath(maximumEntropyTokenizerModel)
      );

      maximumEntropySentenceDetector = new EnglishMaximumEntropySentenceDetector(
        FileUtilx.GetStreamingAssetFilePath(maximumEntropySentenceDetectorModel)
      );
    }
  }
}