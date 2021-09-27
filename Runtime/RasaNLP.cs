/*
This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software Foundation,
Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301, USA.

The Original Code is Copyright (C) 2020 Voxell Technologies.
All rights reserved.
*/

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
        FileUtil.GetStreamingAssetFilePath(maximumEntropyTokenizerModel)
      );

      maximumEntropySentenceDetector = new EnglishMaximumEntropySentenceDetector(
        FileUtil.GetStreamingAssetFilePath(maximumEntropySentenceDetectorModel)
      );
    }
  }
}