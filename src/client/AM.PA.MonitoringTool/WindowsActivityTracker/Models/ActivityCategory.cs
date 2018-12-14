// Created by André Meyer (ameyer@ifi.uzh.ch) from the University of Zurich
// Created: 2017-01-04
// 
// Licensed under the MIT License.

using System.ComponentModel;

namespace WindowsActivityTracker.Models
{
    /// <summary>
    /// Activitiy categories which are automatically mapped for the retrospection
    /// by the ProcessToActivityMapper
    /// </summary>
    public enum ActivityCategory
    {
        [DescriptionAttribute("Uncategorized")]
        Uncategorized, // default
        [DescriptionAttribute("Development")]
        DevCode,
        [DescriptionAttribute("Debugger Use")]
        DevDebug,
        [DescriptionAttribute("Code Reviewing")]
        DevReview,
        [DescriptionAttribute("Version Control")]
        DevVc,
        [DescriptionAttribute("Emails")]
        Email,
        [DescriptionAttribute("Planning")]
        Planning,
        [DescriptionAttribute("Reading/Editing Documents")]
        ReadWriteDocument,
        [DescriptionAttribute("Scheduled meetings")]
        PlannedMeeting,
        [DescriptionAttribute("Instant Messaging")]
        InformalMeeting,
        [DescriptionAttribute("Instant Messaging")]
        InstantMessaging, // subcategory of InformalMeeting
        //WebBrowsing, // uncategorized web browsing
        [DescriptionAttribute("Browsing work-related")]
        WorkRelatedBrowsing,
        [DescriptionAttribute("Browsing work-unrelated")]
        WorkUnrelatedBrowsing,
        [DescriptionAttribute("Navigation in File Explorer")]
        FileNavigationInExplorer,
        [DescriptionAttribute("Other")]
        Other,
        [DescriptionAttribute("RDP (uncategorized)")]
        OtherRdp,
        [DescriptionAttribute("Idle (e.g. break, lunch, meeting)")]
        Idle, // all IDLE events that can't be mapped elsewhere
        [DescriptionAttribute("Uncategorized")]
        Unknown

    }

}
