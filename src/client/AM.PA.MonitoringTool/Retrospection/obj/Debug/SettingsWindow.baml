   M S B A M L   `   `   `  ����O  KRetrospection, Version=1.0.6927.23898, Culture=neutral, PublicKeyToken=null-clr-namespace:RetrospectionRetrospection  5   	   H DShared, Version=2.0.6924.32185, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared 5   	   "    Retrospection.SettingsWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Retrospection     5   	   9paColors$clr-namespace:Shared;assembly=Shared     5   	     � Title$!  PersonalAnalytics: Settings��5	   	   $	��750��6-   $	��770��6:    � 
ResizeMode$ NoResize=�6F   .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��20 �6   .+�5   
   � $��0��6     Shared.Settings   RetrospectionColorBrush#��Z 6   .+�5      �� $��0��6   $��16(�6%   $��Center=�63   $��10 0 0 0q�6N   $��Settings��6`   
�������6p   5      �� 5   
   $��1��6   $��10q�6"   .@�5      �� $��22��6   $	��Top=�5      $��Data Tracker Settings��5      .��5      �� $��10q�6   .+�5      �� $.��(Selected microphone for audio recording:��6    �� 5      -   5         ��Name$ CbMicrophoneSelection��5      $	��220��5   #   $��0,5,0,0q�5    #   $
��Left=�5!   #    �� SelectedValuePath$ Content��5"   #   5#      �� 5$      $��20 0 0 0q�6    
�������$
��Wrap=�5&      �� 6/   $�Hint: T��63   �� 6D   $���o mute the microphone, press the blue button on the device. To stop audio data collection, you can also pause PersonalAnalytics ��6H   �� 6�   $0�*(right click on the icon in the task bar).��6�   �� 6
  �� 6  6  �� 5'      -   5         ��Name$ CbOfficeApiEnabled��5'      $��0 20 0 0q�5(      $ ��Office 365 Tracker enabled��5)      �� 5*      -   5         ��Name$ TbOffice365Hint��5*       $��20 0 0 0q�5+       
�������$����Hint: If you enable this tracker, you need to log-in with your Office 365 work account. This tracker will then regularly collect the meetings in your calendar, and the number of emails sent, received an your inbox size. The contents of the emails and meetings are NOT accessed.��5-       $
��Wrap=�5.       �� 5/      -   5        $% CbOpenRetrospectionInFullScreen��5/      $��0 20 0 0q�50      $2��,Always open the retrospection in full screen��51      �� 52      -   5        $$ CbTimeSpentShowProgramsEnabled��52      $��0 20 0 0q�53      $0��*Show programs used in the Time Spent-table��54      �� 55      -   5        $4 .CbTimeSpentHideMeetingsWithoutAttendeesEnabled��55      $��0 5 0 0q�56      $=��7Hide meetings with no attendees in the Time Spent-table��57      �� 59      $��	20 20 0 0q�6!   5:      5;      5<      �� 5?   
   $��2��6    �� Orientation$ 
Horizontal=�6"   $��10q�6;   .+�5@      �� -   5        	 ��Name$	 BtnSave��5@   $   $
��Save��6   
�������#��Z 6\   #��Z 6�   $��5 0 5 0q�6�   $��20 0 0 0q�6�   �� 5A      -   5        $	 	BtnCancel��5A   &   $��Cancel��6   
�������
   GrayColorBrush#��Z
 6a   #��Z
 6�   $��5 0 5 0q�6�   $��10 0 0 0q�6�   5B      � 5D   
   #��Z
 6   $��3��6H    � 	Resources 5E       �� 
TargetType �� BasedOn �� Property   DarkGrayColorBrush �� Value��%�   '��       7���� 5F      " ��6   8   .�5G      ��  � ��# Z 63   �� 5H       � ��# Z
 63   �� 5I       � ��# Z
 64   �� 5J       � ��$ 5 0 5 0q�60   5K      5L       � ColumnDefinitions 5M      �� 5N      $��* �6#   �� 5O      $
��Auto �6#   �� 5P      $
��Auto �6#   .+�5S      �� -	   5        $ 	TbVersion��5S   (   $��0��6   $��10 0 0 0q�6;   #��Z 6M   �� 5T      -
   5        $��1��5T      $��Feedback��6%   $��Center=�6Q   $��0 0 5 0q�6l   �� 5U      -   5        $��2��5U      $��Privacy Statement��6%   $��Center=�6b   $��0 0 10 0q�6}   5V      5W      5X      