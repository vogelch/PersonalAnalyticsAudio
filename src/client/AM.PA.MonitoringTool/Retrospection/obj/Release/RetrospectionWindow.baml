   M S B A M L   `   `   `  ���� O  KRetrospection, Version=1.0.6927.23886, Culture=neutral, PublicKeyToken=null-clr-namespace:RetrospectionRetrospection  [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem H DShared, Version=2.0.6926.34311, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared '    !Retrospection.RetrospectionWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:Retrospection     Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     6sys&clr-namespace:System;assembly=mscorlib     9paColors$clr-namespace:Shared;assembly=Shared     -     � Title$1  +PersonalAnalytics: Retrospection & Insights��$
��1200��$
��1310��$	��500��$	��890��.���  � RowDefinitions �� $��40 ��� $��* ��� $��20 �.+�� $��0��  Shared.Settings   RetrospectionColorBrush#��Z  � ColumnDefinitions �� $��* ��� $
��Auto ��� $
��Auto �.+��� -    ��Name$ RetrospectionTitle��$��0��$ ��Retrospection and Insights��$��Center=�
�������$��16(�$��10 0 0 0q��� -    ��Name$ 
DayControl��$��1�� �� Orientation$ 
Horizontal=�$��Right=� � 	Resources  �� 
TargetType	 �� BasedOn
 �� Property �� Value��%   '��       7���� " ��8	   .��� 
 � ��
 ������� 
 � ��# Z �� 
 � ��
 ������� 
 � ��$ 5 2 5 2q�.+��� -    ��Name$ SwitchToWeekButton��$$��Switch to Weekly Retrospection��$��Center=��� -   $ SwitchToDayButton��$#��Switch to Daily Retrospection��$��Center=��� 
�������	 �� X1$ 0��	 �� Y1$ 31��	 �� X2$ 0��	 �� Y2$ 9��$��	10 0 10 0q��� -   $ TodayButton��$��Today��$��Center=�$��0 0 10 0q��� -   $ MinusOneButton��$��<��$��Center=�$��0 0 4 0q�(  "System.Windows.Controls.DatePicker  -   $��
DatePicker��#��Z #��Z $��Right=�$��Center=�
 �� Now   SelectedDate# Z �� -	   $ PlusOneButton��$��>��$��Center=�$��	-1 0 10 0q�7  1System.Windows.Forms.Integration.WindowsFormsHost  -
   $��
wbWinForms��$��1��$��Stretch=�$��Stretch=�   Child %  System.Windows.Forms.WebBrowser  -   �� -    ��Name$ LoadingSign��   GrayColorBrush#��Z $��1��._��� $��Center=�$��Center=�.+��� $��Center=�$&�� Preparing your Retrospection ...��#��Z $��30(��� $��Center=�$A��;This might take a few moments. It will be worth the wait :)��$��0 5 0 0q�� #��Z $��3��    DarkGrayColorBrush��%w   '��       7���� " ��8	   .��� 
 � ��# Z �� 
 � ��# Z �� 
 � ��# Z �� 
 � ��$ 5 0 5 0q� �� $��* ��� $
��Auto �.+��� $��0��$�� ���� $��1��$ 
Horizontal=�.+��� -   $��Refresh��$��Center=�$��0 0 10 0q��� -   $ FeedbackInfo��$��Feedback��$��Center=�$��0 0 10 0q��� -   $ PrivacyStatement��$��Privacy Statement��$��Center=�$��0 0 10 0q��� -   $ 	AboutInfo��$��About��$��Center=�$��0 0 10 0q�