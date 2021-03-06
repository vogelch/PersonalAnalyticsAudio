   M S B A M L   `   `   `  ���� L  HPersonalAnalytics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:PersonalAnalytics.UploadPersonalAnalytics.Upload  H DShared, Version=2.0.6926.34311, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared +    %PersonalAnalytics.Upload.UploadWizard   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:PersonalAnalytics.Upload     9paColors$clr-namespace:Shared;assembly=Shared       � Title$&   PersonalAnalytics: Upload Wizard��$	��660��$	��950�� � 
ResizeMode$ NoResize=�.���  � 	Resources  �� 
TargetType �� BasedOn �� Property �� Value  Shared.Settings   RetrospectionColorBrush��%�   '��       7��'�� m     7���� " ��8   .���  � ��
 �������  � ��# Z ��  � ��# Z ��  � ��$ 5 0 5 0q��� " ��8   .���  � ��$ 10q� � RowDefinitions �� $��* ��� $��20 �.+�� -   	 �Name$	 Step1��$��0q�$y�Visible=�$��0��.+�� $	��Top=�$��40��#��Z .+��� $C��=Please consider sharing the collected data with us (step 1/5)��$��16(�$��Center=�$��10 0 0 0q�
��������� $��	0 50 0 50q�.+��� $
��Wrap=���We want to thank you again for participating in our study! Your feedback about PersonalAnalytics and your findings help us to improve it in the future. As a next step, we would like to ask you to share the collected data to help us perform additional research. By analyzing our study participants' data, we pursue the following goals:�� �� 
1. From a �� $
��Bold"�.�research perspective��, we could get access to a big data set and run various kinds of analysis with the data. For example, we could learn more about how developers spend their work days, what makes them feel particularly productive or unproductive, what activities they perform during their work, and we could build personalized productivity models. We hope to find some groups (i.e. clusters) of developers who perceive productivity similarly, to better optimize the retrospection for each of these target groups.�� �� FD2. A few days after sharing the data with us, we will send you some �� $
��Bold"�.�53additional insights into your work and productivity��, findings not (yet) available in the retrospection. This will be a next step towards more personalized insights, which the tool could potentially show as small toast notifications (pop-ups) during your work.�� �� ��If you agree to share the data with us, this upload wizard would let you obfuscate the data, check the data again before it is uploaded and then upload it to our secure data share. The total process should take �� $
��Bold"�.�no longer than 5 minutes.�� �� ;9As a thank you for your efforts, you will also receive a �� $
��Bold"�.�&$$10 MS Dining single-use coupon card.�� �� �� DBThe obfuscated data will be uploaded to a shared drive, which can �� $
��Bold"�.� only be accessed by you and us�� (the researchers in this study). As soon as the upload is completed, the researchers will download and delete the data from the shared folder, and store it on a secured server. As described in the consent form, the data will not be shared outside of Microsoft or with any managers or teammates of yours. �� -   $-��'\\eseserver\Andre Meyer\ConsentForm.pdf?�.�LJClick here, if you want to see the privacy statement (consent form) again.�� �� �� 42Please contact us in case you have any questions (�� -   $<��6mailto:t-anmeye@microsoft.com?CC=tzimmer@microsoft.com?�.�Andre Meyer and �� -   $<��6mailto:t-anmeye@microsoft.com?CC=tzimmer@microsoft.com?�.�Tom Zimmermann).� $��20q�$��Right=�$��Bottom=� �� $
��Auto ��� $
��Auto �
 � ColumnDefinitions
 �� $
��Auto ��� $
��Auto �.+��� -    ��Name$ QuickUploadEnabled��$��0��$��0��{�.  $)��#Quick Upload with previous settings��$	��250��
�������$��30���� -   $��0��$��1��$
��Next��$��20 0 0 0q�$	��100��$��30���� -    ��Name$! tbOneClickUploadSettingsTxt��$��1��$��2��$��0q�
�������� -   $	 Step2��$��0q�$y�	Collapsed=�$��0��.+�� $	��Top=�$��40��#��Z .+��� $6��0Additional information for the upload (step 2/5)��$��16(�$��Center=�$��10 0 0 0q�
��������� $��0 50 0 0q�.+��� $`��ZTo perform the upload and for the analysis of the data, we need a little more information:���� $��	10 0 10 0q�$
��Bold"�#��Z $D��>Please insert the subject ID we sent you via email (e.g. T63):����  �� Orientation$ 
Horizontal=�.+��� $��Your subject ID:���� -	    ��Name$ TbParticipantId��$
��Left=�$��30��$��22�� �� 	MaxLength$ 3���� $��
10 20 10 0q�$
��Bold"�$&�� What product are you working on?��#��Z �� $��
10 0 10 10q�$
��Left=�$����We would like to correlate your perceived productivity with your check-ins, the work items/bugs you worked on during the study, and the builds you made, using the CodeMine database. Please select all products you worked on during the study if you want to allow us to use these additional data sets:��$
��Wrap=��� $��	10 0 10 0q�.+��� -
    ��Name$ Azure��$��Azure���� -   $ Dynamics��$��Dynamics���� -   $ EE��$��EE���� -   $ Exchange��$��Exchange���� -   $ Office��$��Office���� -   $ 	OfficeMac��$��
Office Mac���� -   $	 OSD��$	��OSD���� -   $ 	SQLServer��$��
SQL Server���� -   $	 VSO��$	��VSO���� -   $ Windows��$��Windows���� -   $ WindowsPhone��$��Windows Phone���� -   $ WindowsServices��$��Windows Services���� -   $
 Xbox��$
��Xbox���� .���� $ 
Horizontal=�$��0q�.+��� $��Other, please specify: ��$��0q��� -   $ Other��$�� ��$��22��$��0q�$	��200���� $��
10 20 10 0q�$
��Bold"�#��Z $j��dIn case you installed PersonalAnalytics on multiple machines, please answer the following questions:���� $��	10 0 10 0q�$t��nPlease also perform the upload from the other machines you installed the tool (virtual and physical machines).��
��������� $ 
Horizontal=�.+��� $@��:On how many machines did you install the tracker in total?���� -   $ TbNumberOfMachines��$	��100��$��22���� -   $ CbIsMainMachine��$��	10 0 10 0q�$a��[Select this CheckBox if this is the main/host machine you were working on during the study.���� $��Bottom=�$��Right=�$��20q�.+��� -   $
��Next��$��Right=�$	��100��$��30���� 
�������$��0q�$a��[Once you hit 'Next', it might take a moment to check if the inserted subject ID is correct.��$
��Wrap=�� -   $	 Step3��$��0q�$y�	Collapsed=�$��0��.+�� $	��Top=�$��40��#��Z .+��� $D��>Select how you want to obfuscate the collected data (step 3/5)��$��16(�$��Center=�$��10 0 0 0q�
��������� $��	0 50 0 50q�.+��� $����This is a list of the collected data the tool will share with us. For some data sets, you can now specify if and how you want to obfuscate them. Please keep in mind that obfuscating data will also reduce what we can analyze and make your personalized productivity insights less detailed. The tool will create a copy of your original database file and obfuscate it according to your selection:��$
��Wrap=��� $
��Bold"�$"��Database Logs and Error Logs��#��Z �� $z��tThis data contains only information about the handling of the tool and error logs, and helps us to improve the tool.��$
��Wrap=�$��10 -10 10 0q��� $
��Bold"�$��Emails��#��Z �� $T��NThe tool only collected the number of emails sent/received and the inbox size.��$��10 -10 10 0q��� $
��Bold"�$'��!Mouse and Keyboard Input Activity��#��Z �� $x��rThe tool only collected the number of mouse clicks, scrolls, movements and when a key was pressed (not which one).��$
��Wrap=�$��10 -10 10 0q��� $
��Bold"�$/��)Perceived Productivity (Pop-Up Responses)��#��Z �� $����The tool collected your responses to the pop-ups about your perceived productivity. This information helps us to correlate the collected data with your perceived productivity.��$
��Wrap=�$��10 -10 10 0q��� $
��Bold"�$��Meetings��#��Z �� $x��rThe tool collected the start time, duration and meeting subject. Please select how you want to share the meetings:��$
��Wrap=�$��10 -10 10 0q��� $��	10 10 0 0q�$ 
Horizontal=�.+�	�  d� 	IsChecked$
 TrueY�$'��!Keep meeting subjects as they are�� 	� 	GroupName$ MeetingsGroup���� $��20 0 0 0q�$	��700��$z��tIf you chose this option, meeting subjects would be shared like this 'Team Scrum Meeting', 'Signing Microsoft Code'.��$
��Wrap=�
��������� $��	10 10 0 0q�$ 
Horizontal=�.+�	� -    	�Name$ RBObfuscateMeetingTitles��$ ��Obfuscate meeting subjects��$ MeetingsGroup���� $��56 0 0 0q�$	��700��$u��oAn obfuscated meeting subject which will be shared with us looks like this: 'E6D52761A44A3DCA6D432903913DF914'.��$
��Wrap=�
��������� $
��Bold"�$��Programs Used��#��Z �� $����The tool collected the currently active process and window title during your work. Please select how you want to share the window titles:��$��10 -10 10 0q��� $��	10 10 0 0q�$ 
Horizontal=�.+�	� $
 TrueY�$$��Keep window titles as they are��$ WindowTitlesGroup���� $��20 0 0 0q�$	��720��$����If you chose this option, window titles would be shared like this 'Report 1.docx - Microsoft Word', 'Funny Pictures - Facebook', 'AM.PA.MonitoringTool - Visual Studio', 'Andre Meyer - Skype'��$
��Wrap=�
��������� $��	10 10 0 0q�$ 
Horizontal=�.+�	� -   $ RBObfuscateWindowTitles��$��Obfuscate window titles��$ WindowTitlesGroup���� $��56 0 0 0q�$	��720��$����An obfuscated window title which will be shared with us looks like this: '0A63FA1530451A27831D30CE6F11B6F3'. Please keep in mind that obfuscating window titles makes it more difficult to analyze the data, and results in fewer personalized insights we could return you.��$
��Wrap=�
��������� $��Bottom=�$��10 30 10 10q�$��Right=�.+��� -   $
��Next��$��Right=�$	��100��$��30���� 
�������$��0q�$j��dOnce you hit 'Next', it might take a few minutes to obfuscate the data and prepare it for uploading.��� -   $	 Step4��$��0q�$y�	Collapsed=�$��0��.+�� $	��Top=�$��40��#��Z .+��� $L��FObfuscating the collected data and preparing for sharing... (step 3/5)��$��16(�$��Center=�$��10 0 0 0q�
��������� $��Center=�$��Center=�$D��>This might take a few minutes. Please don't close this window.��� -    $	 Step5��$��0q�$y�	Collapsed=�$��0��.+�� $	��Top=�$��40��#��Z .+��� $D��>Check the data again before they are shared with us (step 4/5)��$��16(�$��Center=�$��10 0 0 0q�
��������� $��	0 50 0 50q�.+��� $y��sThe collected data was obfuscated and a zip file for the upload was created. If you want you can have a look at it:���� -!   $)��#Open the zip which will be uploaded��$	��230��
�������   GrayColorBrush#��Z #��Z �� $��0 5 0 0q�$��Center=�
�������$	��600�� �� TextAlignment$ Center=�$
��Wrap=�CAYou can use the DB Browser for SQLite to open the database file. �� -"   $]��W\\ESESERVER\Andre Meyer\SQLiteDatabaseBrowserPortable\SQLiteDatabaseBrowserPortable.exe?�.�Run DB Browser for SQLiteom and simply drag and drop the database file into the DB Browser for SQLite to check the obfuscated data sets.�� $��10 30 10 10q�$��Center=�$��Center=� �� Padding$ 10q�$	��400��#��Z $��1q�._��� $��Center=�.+��� -#   $��Upload the data now��$��30��$	��140���� 
�������$ Center=�$��0q�$
��Wrap=�$s��mOnce you start the upload, it might take a few minutes to copy the obfuscated data file to our shared folder.��� -$   $	 Step6��$��0q�$y�	Collapsed=�$��0��.+�� $	��Top=�$��40��#��Z .+��� $J��DCopying the obfuscated data files to the shared folder... (step 4/5)��$��16(�$��Center=�$��10 0 0 0q�
��������� $��Center=�$��Center=�$D��>This might take a few minutes. Please don't close this window.��� -%   $	 Step7��$��0q�$y�	Collapsed=�$��0��.+�� $	��Top=�$��40��#��Z .+��� $1��+Thank you for sharing your data! (step 5/5)��$��16(�$��Center=�$��10 0 0 0q�
��������� $��Center=�$��Center=�$��	0 50 0 50q�.+��� $��Center=�$M��GThe obfuscated data was successfully uploaded to our server. Thank you!���� $��	10 0 10 0q�$t��nPlease also perform the upload from the other machines you installed the tool (virtual and physical machines).��
��������� -&   $��Bottom=�$��10q�$��Close the upload wizard��$	��200��$��30��� #��Z $��1��
 �� $��* ��� $
��Auto �.+��� -'   $ Status��$��0��$��10 0 0 0q�
��������� -(   $��1��$��Questions or Feedback?��$��Center=�$��0 0 5 0q�
�������#��Z #��Z 