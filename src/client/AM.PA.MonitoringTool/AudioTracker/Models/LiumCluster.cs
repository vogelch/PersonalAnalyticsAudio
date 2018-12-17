// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System.Collections.Generic;
using Shared;

namespace AudioTracker.Models
{
    // analogous to http://www-lium.univ-lemans.fr/diarization/javadoc/fr/lium/spkDiarization/libClusteringData/Cluster.html
    class LiumCluster
    {
        public string ClusterLabel;
        public double ScoreFS;
        public double ScoreFT;
        public double ScoreMS;
        public double ScoreMT;
        public string MergeText;
        public double? MergeWith;
        public List<LiumSegment> SegmentSet;

        internal LiumCluster(string lineFromFile)
        {
            int LengthOfScoreHeader = 12;
            string[] ClusterProperties = lineFromFile.Split('[');
            string ClusterLabelWithoutHeader = ClusterProperties[0].Substring(11);
            ClusterLabel = ClusterLabelWithoutHeader.Remove(ClusterLabelWithoutHeader.Length - 1);
            foreach (string CurrentProperty in ClusterProperties)
            {
                string PropertyHeader = CurrentProperty.Substring(1, 8);
                string PropertyWithoutHeader = CurrentProperty.Substring(LengthOfScoreHeader);
                string PropertyClean = PropertyWithoutHeader.Remove(PropertyWithoutHeader.Length - 2);
                MergeText = null;
                MergeWith = null;
                if (PropertyHeader == "score:FS")
                {
                    ScoreFS = double.Parse(PropertyClean);
                }
                else if (PropertyHeader == "score:FT")
                {
                    ScoreFT = double.Parse(PropertyClean);
                }
                else if (PropertyHeader == "score:MS")
                {
                    ScoreMS = double.Parse(PropertyClean);
                }
                else if (PropertyHeader == "score:MT")
                {
                    ScoreMT = double.Parse(PropertyClean);
                }
                else if (PropertyHeader == "merge HC")
                {
                    MergeText = CurrentProperty.Substring(1, CurrentProperty.Length - CurrentProperty.IndexOf(" with") - 3);
                    string MergeWithWithoutHeader = CurrentProperty.Substring(MergeText.Length + 6);
                    MergeWith = double.Parse(MergeWithWithoutHeader.Remove(MergeWithWithoutHeader.Length - 2));
                }
            }
        }

    }

    //string bandwidth
    //string channel

    //getNumberOfGenders
    //getNumberOfSegments

}
