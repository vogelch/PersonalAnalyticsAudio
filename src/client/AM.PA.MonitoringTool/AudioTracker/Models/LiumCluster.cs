// Created by Christoph Vogel (christoph.vogel@uzh.ch) from the University of Zurich
// Created: 2018-10-01
// 
// Licensed under the MIT License.

using System.Collections.Generic;

namespace AudioTracker.Models
{
    // analog http://www-lium.univ-lemans.fr/diarization/javadoc/fr/lium/spkDiarization/libClusteringData/Cluster.html
    class LiumCluster
    {
        public string ClusterLabel;
        public double ScoreFS = -33.51189410445919;
        public double scoreFT = -33.76490556665477;
        public double scoreMS = -32.95000893937814;
        public double scoreMT = -33.73890703968869;
        public List<LiumSegment> SegmentSet;
    }

    //string bandwidth
    //string channel

    //getNumberOfGenders
    //getNumberOfSegments

}
