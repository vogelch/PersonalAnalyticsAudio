   M S B A M L   `   `   `  ���� O  KRetrospection, Version=1.0.6927.23886, Culture=neutral, PublicKeyToken=null-clr-namespace:RetrospectionRetrospection  H DShared, Version=2.0.6926.34311, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared "    Retrospection.SettingsWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:Retrospection     9paColors$clr-namespace:Shared;assembly=Shared       � Title$!  PersonalAnalytics: Settings��$	��750��$	��770�� � 
ResizeMode$ NoResize=�.���  � RowDefinitions �� $��40 ��� $��* ��� $
��Auto ��� $��20 �.+�� $��0��  Shared.Settings   RetrospectionColorBrush#��Z .+��� $��0��$��16(�$��Center=�$��10 0 0 0q�$��Settings��
��������� $��1��$��10q�.@��� $��22��$	��Top=�$��Data Tracker Settings��.���� $��10q�.+��� $.��(Selected microphone for audio recording:���� -    ��Name$ CbMicrophoneSelection��$	��220��$��0,5,0,0q�$
��Left=� �� SelectedValuePath$ Content���� $��20 0 0 0q�
�������$
��Wrap=��� $�Hint: T���� $���o mute the microphone, press the blue button on the device. To stop audio data collection, you can also pause PersonalAnalytics ���� $0�*(right click on the icon in the task bar).���� �� �� -    ��Name$ CbOfficeApiEnabled��$��0 20 0 0q�$ ��Office 365 Tracker enabled���� -    ��Name$ TbOffice365Hint��$��20 0 0 0q�
�������$����Hint: If you enable this tracker, you need to log-in with your Office 365 work account. This tracker will then regularly collect the meetings in your calendar, and the number of emails sent, received an your inbox size. The contents of the emails and meetings are NOT accessed.��$
��Wrap=��� -   $% CbOpenRetrospectionInFullScreen��$��0 20 0 0q�$2��,Always open the retrospection in full screen���� -   $$ CbTimeSpentShowProgramsEnabled��$��0 20 0 0q�$0��*Show programs used in the Time Spent-table���� -   $4 .CbTimeSpentHideMeetingsWithoutAttendeesEnabled��$��0 5 0 0q�$=��7Hide meetings with no attendees in the Time Spent-table���� $��	20 20 0 0q��� $��2�� �� Orientation$ 
Horizontal=�$��10q�.+��� -   	 ��Name$	 BtnSave��$
��Save��
�������#��Z #��Z $��5 0 5 0q�$��20 0 0 0q��� -   $	 	BtnCancel��$��Cancel��
�������
   GrayColorBrush#��Z
 #��Z
 $��5 0 5 0q�$��10 0 0 0q�� #��Z
 $��3�� � 	Resources  �� 
TargetType �� BasedOn �� Property   DarkGrayColorBrush �� Value��%w   '��       7���� " ��8   .���  � ��# Z ��  � ��# Z
 ��  � ��# Z
 ��  � ��$ 5 0 5 0q� � ColumnDefinitions �� $��* ��� $
��Auto ��� $
��Auto �.+��� -	   $ 	TbVersion��$��0��$��10 0 0 0q�#��Z �� -
   $��1��$��Feedback��$��Center=�$��0 0 5 0q��� -   $��2��$��Privacy Statement��$��Center=�$��0 0 10 0q�