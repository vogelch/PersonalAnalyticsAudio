   M S B A M L   `   `   `  ����L  HPersonalAnalytics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:PersonalAnalytics.UploadPersonalAnalytics.Upload  5   	   H DShared, Version=2.0.6924.32185, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared 5   	   +    %PersonalAnalytics.Upload.UploadWizard   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   8local&clr-namespace:PersonalAnalytics.Upload     5   	   9paColors$clr-namespace:Shared;assembly=Shared     5   	     � Title$&   PersonalAnalytics: Upload Wizard��5	   	   $	��660��5
   	   $	��950��6    � 
ResizeMode$ NoResize=�6"   .��5      �  � 	Resources 5   
    �� 
TargetType �� BasedOn �� Property �� Value  Shared.Settings   RetrospectionColorBrush��%%  '��       7��'�� �     7���� 5      " ��6   8   .�5      ��  � ��
 �����6/   �� 5       � ��# Z 6/   �� 5       � ��# Z 60   �� 5       � ��$ 5 0 5 0q�6,   5      �� 5      " ��6   8   .�5      ��  � ��$ 10q�6+   5      5       � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   � -   5        	 �Name$	 Step1��5      $��0q�6   $y�Visible=�6)   $��0��6>   .+�5      � $	��Top=�6   $��40��6+   #��Z 67   .+�5       �� $C��=Please consider sharing the collected data with us (step 1/5)��6   $��16(�6a   $��Center=�6o   $��10 0 0 0q�6�   
�������6�   5!      �� 5#      $��	0 50 0 50q�6   .+�5$      �� $
��Wrap=�6   ��We want to thank you again for participating in our study! Your feedback about PersonalAnalytics and your findings help us to improve it in the future. As a next step, we would like to ask you to share the collected data to help us perform additional research. By analyzing our study participants' data, we pursue the following goals:�� 5&      �� 6$   
1. From a �� 5'       $
��Bold"�6%   .�67   research perspective6M   ��, we could get access to a big data set and run various kinds of analysis with the data. For example, we could learn more about how developers spend their work days, what makes them feel particularly productive or unproductive, what activities they perform during their work, and we could build personalized productivity models. We hope to find some groups (i.e. clusters) of developers who perceive productivity similarly, to better optimize the retrospection for each of these target groups.�� 5(      �� 6#   FD2. A few days after sharing the data with us, we will send you some �� 5)   Z   $
��Bold"�6_   .�6q   53additional insights into your work and productivity6�   ��, findings not (yet) available in the retrospection. This will be a next step towards more personalized insights, which the tool could potentially show as small toast notifications (pop-ups) during your work.�� 5*      �� 6#   ��If you agree to share the data with us, this upload wizard would let you obfuscate the data, check the data again before it is uploaded and then upload it to our secure data share. The total process should take �� 5+   �   $
��Bold"�6�   .�6   no longer than 5 minutes6  .�� 5,      �� 6#   ;9As a thank you for your efforts, you will also receive a �� 5-   O   $
��Bold"�6T   .�6f   &$$10 MS Dining single-use coupon card6�   .�� 5.      �� 6#   �� 60   DBThe obfuscated data will be uploaded to a shared drive, which can �� 5/   X   $
��Bold"�6]   .�6o    only be accessed by you and us6�   �� (the researchers in this study). As soon as the upload is completed, the researchers will download and delete the data from the shared folder, and store it on a secured server. As described in the consent form, the data will not be shared outside of Microsoft or with any managers or teammates of yours. �� 6�  -   5        $-��'\\eseserver\Andre Meyer\ConsentForm.pdf?�5/   �  .�62  LJClick here, if you want to see the privacy statement (consent form) again.6~  �� 50      �� 6#   �� 60   42Please contact us in case you have any questions (�� 52      -   5        $<��6mailto:t-anmeye@microsoft.com?CC=tzimmer@microsoft.com?�52   M   .�6�   Andre Meyer6�    and �� 53      -   5        $<��6mailto:t-anmeye@microsoft.com?CC=tzimmer@microsoft.com?�53   L   .�6�   Tom Zimmermann6�   ).54      55      � 58      $��20q�6   $��Right=�6   $��Bottom=�6;    59      �� 5:      $
��Auto �6$   �� 5;      $
��Auto �6$   
 � ColumnDefinitions
 5=      �� 5>      $
��Auto �6'   �� 5?      $
��Auto �6'   .+�5B      �� -   5         ��Name$ QuickUploadEnabled��5B   6   $��0��6   $��0��6&   {�.  $)��#Quick Upload with previous settings��6d   $	��250��6�   
�������$��30��6�   �� 5C      -   5        $��0��5C      $��1��6&   $
��Next��66   $��20 0 0 0q�6E   $	��100��6w   $��30��6�   �� 5D      -   5         ��Name$! tbOneClickUploadSettingsTxt��5D   =   $��1��6   $��2��6)   $��0q�6b   
�������6m   5E      5F      � 5I   
   -   5        $	 Step2��5I      $��0q�6   $y�	Collapsed=�6)   $��0��6@   .+�5J      � $	��Top=�6   $��40��6+   #��Z 67   .+�5K      �� $6��0Additional information for the upload (step 2/5)��6   $��16(�6T   $��Center=�6b   $��10 0 0 0q�6}   
�������6�   5L      �� 5N      $��0 50 0 0q�6   .+�5O      �� $`��ZTo perform the upload and for the analysis of the data, we need a little more information:��6   �� 5Q      $��	10 0 10 0q�6   $
��Bold"�6/   #��Z 6A   $D��>Please insert the subject ID we sent you via email (e.g. T63):��6�   �� 5R       �� Orientation$ 
Horizontal=�6   .+�5S      �� $��Your subject ID:��6    �� 5T      -	   5         ��Name$ TbParticipantId��5T      $
��Left=�67   $��30��6R   $��22��6]    �� 	MaxLength$ 3��6i   5U      �� 5X      $��
10 20 10 0q�6   $
��Bold"�61   $&�� What product are you working on?��6C   #��Z 6k   �� 5Y      $��
10 0 10 10q�6   $
��Left=�60   $����We would like to correlate your perceived productivity with your check-ins, the work items/bugs you worked on during the study, and the builds you made, using the CodeMine database. Please select all products you worked on during the study if you want to allow us to use these additional data sets:��6K   $
��Wrap=�6}  �� 5Z      $��	10 0 10 0q�6   .+�5[      �� -
   5         ��Name$ Azure��5[      $��Azure��6.   �� 5\      -   5        $ Dynamics��5\      $��Dynamics��61   �� 5]      -   5        $ EE��5]      $��EE��6+   �� 5^      -   5        $ Exchange��5^      $��Exchange��61   �� 5_      -   5        $ Office��5_      $��Office��6/   �� 5`      -   5        $ 	OfficeMac��5`      $��
Office Mac��62   �� 5a      -   5        $	 OSD��5a      $	��OSD��6,   �� 5b      -   5        $ 	SQLServer��5b      $��
SQL Server��62   �� 5c      -   5        $	 VSO��5c      $	��VSO��6,   �� 5d      -   5        $ Windows��5d      $��Windows��60   �� 5e      -   5        $ WindowsPhone��5e      $��Windows Phone��65   �� 5f      -   5        $ WindowsServices��5f      $��Windows Services��68   �� 5g      -   5        $
 Xbox��5g      $
��Xbox��6-   �� 5h      .��5i      �� $ 
Horizontal=�6%   $��0q�6>   .+�5j      �� $��Other, please specify: ��6(   $��0q�6G   �� 5k      -   5        $ Other��5k   &   $�� ��65   $��22��6=   $��0q�6I   $	��200��6T   5l      5m      5n      �� 5q      $��
10 20 10 0q�6   $
��Bold"�60   #��Z 6B   $j��dIn case you installed PersonalAnalytics on multiple machines, please answer the following questions:��6�   �� 5r      $��	10 0 10 0q�6   $t��nPlease also perform the upload from the other machines you installed the tool (virtual and physical machines).��6/   
�������6�   �� 5s      $ 
Horizontal=�6   .+�5t      �� $@��:On how many machines did you install the tracker in total?��6    �� 5u      -   5        $ TbNumberOfMachines��5u      $	��100��6:   $��22��6F   5v      �� 5w      -   5        $ CbIsMainMachine��5w   .   $��	10 0 10 0q�6   $a��[Select this CheckBox if this is the main/host machine you were working on during the study.��6G   5x      �� 5z      $��Bottom=�6   $��Right=�64   $��20q�6P   .+�5{      �� -   5        $
��Next��5{      $��Right=�6H   $	��100��6d   $��30��6p   �� 5|      
�������$��0q�6.   $a��[Once you hit 'Next', it might take a moment to check if the inserted subject ID is correct.��69   $
��Wrap=�6�   5}      5~      � 5�   
   -   5        $	 Step3��5�      $��0q�6   $y�	Collapsed=�6)   $��0��6@   .+�5�      � $	��Top=�6   $��40��6+   #��Z 67   .+�5�      �� $D��>Select how you want to obfuscate the collected data (step 3/5)��6   $��16(�6b   $��Center=�6p   $��10 0 0 0q�6�   
�������6�   5�      �� 5�      $��	0 50 0 50q�6   .+�5�      �� $����This is a list of the collected data the tool will share with us. For some data sets, you can now specify if and how you want to obfuscate them. Please keep in mind that obfuscating data will also reduce what we can analyze and make your personalized productivity insights less detailed. The tool will create a copy of your original database file and obfuscate it according to your selection:��6   $
��Wrap=�6�  �� 5�      $
��Bold"�6   $"��Database Logs and Error Logs��6.   #��Z 6R   �� 5�      $z��tThis data contains only information about the handling of the tool and error logs, and helps us to improve the tool.��6   $
��Wrap=�6�   $��10 -10 10 0q�6�   �� 5�      $
��Bold"�6   $��Emails��6.   #��Z 6<   �� 5�      $T��NThe tool only collected the number of emails sent/received and the inbox size.��6   $��10 -10 10 0q�6r   �� 5�      $
��Bold"�6   $'��!Mouse and Keyboard Input Activity��6.   #��Z 6W   �� 5�      $x��rThe tool only collected the number of mouse clicks, scrolls, movements and when a key was pressed (not which one).��6   $
��Wrap=�6�   $��10 -10 10 0q�6�   �� 5�      $
��Bold"�6   $/��)Perceived Productivity (Pop-Up Responses)��6.   #��Z 6_   �� 5�      $����The tool collected your responses to the pop-ups about your perceived productivity. This information helps us to correlate the collected data with your perceived productivity.��6   $
��Wrap=�6�   $��10 -10 10 0q�6�   �� 5�      $
��Bold"�6   $��Meetings��6.   #��Z 6>   �� 5�      $x��rThe tool collected the start time, duration and meeting subject. Please select how you want to share the meetings:��6   $
��Wrap=�6�   $��10 -10 10 0q�6�   �� 5�      $��	10 10 0 0q�6   $ 
Horizontal=�60   .+�5�      	�  d� 	IsChecked$
 TrueY�6"   $'��!Keep meeting subjects as they are��63    	� 	GroupName$ MeetingsGroup��6_   �� 5�      $��20 0 0 0q�6    $	��700��62   $z��tIf you chose this option, meeting subjects would be shared like this 'Team Scrum Meeting', 'Signing Microsoft Code'.��6>   $
��Wrap=�6�   
�������6�   5�      �� 5�      $��	10 10 0 0q�6   $ 
Horizontal=�60   .+�5�      	� -   5         	�Name$ RBObfuscateMeetingTitles��5�   "   $ ��Obfuscate meeting subjects��6D   $ MeetingsGroup��6i   �� 5�      $��56 0 0 0q�6    $	��700��62   $u��oAn obfuscated meeting subject which will be shared with us looks like this: 'E6D52761A44A3DCA6D432903913DF914'.��6>   $
��Wrap=�6�   
�������6�   5�      �� 5�      $
��Bold"�6   $��Programs Used��6.   #��Z 6C   �� 5�      $����The tool collected the currently active process and window title during your work. Please select how you want to share the window titles:��6   $��10 -10 10 0q�6�   �� 5�      $��	10 10 0 0q�6   $ 
Horizontal=�60   .+�5�      	� $
 TrueY�6"   $$��Keep window titles as they are��63   $ WindowTitlesGroup��6\   �� 5�      $��20 0 0 0q�6    $	��720��62   $����If you chose this option, window titles would be shared like this 'Report 1.docx - Microsoft Word', 'Funny Pictures - Facebook', 'AM.PA.MonitoringTool - Visual Studio', 'Andre Meyer - Skype'��6>   $
��Wrap=�6  
�������6  5�      �� 5�      $��	10 10 0 0q�6   $ 
Horizontal=�60   .+�5�      	� -   5        $ RBObfuscateWindowTitles��5�   D   $��Obfuscate window titles��6"   $ WindowTitlesGroup��6e   �� 5�      $��56 0 0 0q�6    $	��720��62   $����An obfuscated window title which will be shared with us looks like this: '0A63FA1530451A27831D30CE6F11B6F3'. Please keep in mind that obfuscating window titles makes it more difficult to analyze the data, and results in fewer personalized insights we could return you.��6>   $
��Wrap=�6R  
�������6f  5�      5�      �� 5�      $��Bottom=�6   $��10 30 10 10q�64   $��Right=�6I   .+�5�      �� -   5        $
��Next��5�      $��Right=�6(   $	��100��6c   $��30��6o   �� 5�      
�������$��0q�6.   $j��dOnce you hit 'Next', it might take a few minutes to obfuscate the data and prepare it for uploading.��69   5�      5�      � 5�   
   -   5        $	 Step4��5�      $��0q�6   $y�	Collapsed=�6)   $��0��6@   .+�5�      � $	��Top=�6   $��40��6+   #��Z 67   .+�5�      �� $L��FObfuscating the collected data and preparing for sharing... (step 3/5)��6   $��16(�6j   $��Center=�6x   $��10 0 0 0q�6�   
�������6�   5�      �� 5�      $��Center=�6   $��Center=�63   $D��>This might take a few minutes. Please don't close this window.��6P   5�      � 5�   
   -    5        $	 Step5��5�      $��0q�6   $y�	Collapsed=�6)   $��0��6@   .+�5�      � $	��Top=�6   $��40��6+   #��Z 67   .+�5�      �� $D��>Check the data again before they are shared with us (step 4/5)��6   $��16(�6b   $��Center=�6p   $��10 0 0 0q�6�   
�������6�   5�      �� 5�      $��	0 50 0 50q�6   .+�5�      �� $y��sThe collected data was obfuscated and a zip file for the upload was created. If you want you can have a look at it:��6   �� 5�      -!   5        $)��#Open the zip which will be uploaded��5�      $	��230��6^   
�������   GrayColorBrush#��Z 6|   #��Z 6�   �� 5�      $��0 5 0 0q�6   $��Center=�6-   
�������$	��600��6\    �� TextAlignment$ Center=�6h   $
��Wrap=�6   CAYou can use the DB Browser for SQLite to open the database file. �� 5�      -"   5        $]��W\\ESESERVER\Andre Meyer\SQLiteDatabaseBrowserPortable\SQLiteDatabaseBrowserPortable.exe?�5�   L   .�6�   Run DB Browser for SQLite6�   om and simply drag and drop the database file into the DB Browser for SQLite to check the obfuscated data sets.5�      5�      �� 5�      $��10 30 10 10q�6   $��Center=�6*   $��Center=�6E    �� Padding$ 10q�6b   $	��400��6o   #��Z 6{   $��1q�6�   ._�5�      �� $��Center=�6   .+�5�      �� -#   5        $��Upload the data now��5�      $��30��6U   $	��140��6a   �� 5�      
�������$ Center=�62   $��0q�6I   $
��Wrap=�6T   $s��mOnce you start the upload, it might take a few minutes to copy the obfuscated data file to our shared folder.��6h   5�      5�      5�      � 5�   
   -$   5        $	 Step6��5�      $��0q�6   $y�	Collapsed=�6)   $��0��6@   .+�5�      � $	��Top=�6   $��40��6+   #��Z 67   .+�5�      �� $J��DCopying the obfuscated data files to the shared folder... (step 4/5)��6   $��16(�6h   $��Center=�6v   $��10 0 0 0q�6�   
�������6�   5�      �� 5�      $��Center=�6   $��Center=�63   $D��>This might take a few minutes. Please don't close this window.��6P   5�      � 5�   
   -%   5        $	 Step7��5�      $��0q�6   $y�	Collapsed=�6)   $��0��6@   .+�5�      � $	��Top=�6   $��40��6+   #��Z 67   .+�5�      �� $1��+Thank you for sharing your data! (step 5/5)��6   $��16(�6O   $��Center=�6]   $��10 0 0 0q�6x   
�������6�   5�      �� 5�      $��Center=�6   $��Center=�66   $��	0 50 0 50q�6Q   .+�5�      �� $��Center=�6   $M��GThe obfuscated data was successfully uploaded to our server. Thank you!��69   �� 5�      $��	10 0 10 0q�6   $t��nPlease also perform the upload from the other machines you installed the tool (virtual and physical machines).��6/   
�������6�   �� 5�      -&   5        $��Bottom=�5�      $��10q�64   $��Close the upload wizard��6@   $	��200��6b   $��30��6�   5�      5�      � 5�   
   #��Z 6   $��1��6H   
 5�      �� 5�      $��* �6#   �� 5�      $
��Auto �6#   .+�5�      �� -'   5        $ Status��5�   (   $��0��6   $��10 0 0 0q�68   
�������6J   �� 5�      -(   5        $��1��5�      $��Questions or Feedback?��6%   $��Center=�6_   $��0 0 5 0q�6z   
�������#��Z 6�   #��Z 6�   5�      5�      5�      