   M S B A M L   `   `   `  ����O  KRetrospection, Version=1.0.6927.23898, Culture=neutral, PublicKeyToken=null-clr-namespace:RetrospectionRetrospection  5   	   [ WSystem.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089Y@clr-namespace:System.Windows.Forms;assembly=System.Windows.FormsSystem.Windows.Forms 5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   H DShared, Version=2.0.6924.32185, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared 5	   	   '    !Retrospection.RetrospectionWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Retrospection     5   	   Owf@clr-namespace:System.Windows.Forms;assembly=System.Windows.Forms     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   9paColors$clr-namespace:Shared;assembly=Shared     5	   	   -   5          � Title$1  +PersonalAnalytics: Retrospection & Insights��5   	   $
��1200��5   	   $
��1310��6   $	��500��6$   $	��890��64   .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   � $��0��6     Shared.Settings   RetrospectionColorBrush#��Z 6    � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5      �� -   5         ��Name$ RetrospectionTitle��5   (   $��0��6   $ ��Retrospection and Insights��6D   $��Center=�6f   
�������$��16(�6�   $��10 0 0 0q�6�   �� 5!      -   5         ��Name$ 
DayControl��5!      $��1��6-    �� Orientation$ 
Horizontal=�6=   $��Right=�6V    � 	Resources 5"       �� 
TargetType	 �� BasedOn
 �� Property �� Value��%�   '��       7���� 5#      " ��6   8	   .�5$      �� 
 � ��
 �����67   �� 5%      
 � ��# Z 67   �� 5&      
 � ��
 �����68   �� 5'      
 � ��$ 5 2 5 2q�64   5(      5)      .+�5+      �� -   5         ��Name$ SwitchToWeekButton��5+      $$��Switch to Weekly Retrospection��65   $��Center=�6{   �� 5,      -   5        $ SwitchToDayButton��5,      $#��Switch to Daily Retrospection��64   $��Center=�6x   �� 5.      
�������	 �� X1$ 0��6&   	 �� Y1$ 31��6-   	 �� X2$ 0��65   	 �� Y2$ 9��6<   $��	10 0 10 0q�6C   �� 50      -   5        $ TodayButton��50      $��Today��6.   $��Center=�6T   $��0 0 10 0q�6o   �� 51      -   5        $ MinusOneButton��51      $��<��61   $��Center=�6Y   $��0 0 4 0q�6t   (  "System.Windows.Controls.DatePicker  52      -   5        $��
DatePicker��52      #��Z 6/   #��Z 6q   $��Right=�6�   $��Center=�6�   
 �� Now   SelectedDate# Z 6�   6  �� 53      -	   5        $ PlusOneButton��53      $��>��60   $��Center=�6W   $��	-1 0 10 0q�6r   54      55      7  1System.Windows.Forms.Integration.WindowsFormsHost  57   
   -
   5        $��
wbWinForms��57      $��1��6-   $��Stretch=�6:   $��Stretch=�6X      Child 58      %  System.Windows.Forms.WebBrowser  59      -   5        59       5;      �� 5=   
   -   5         ��Name$ LoadingSign��5=         GrayColorBrush#��Z 6'   $��1��6`   ._�5>      �� $��Center=�6   $��Center=�64   .+�5?      �� $��Center=�6   $&�� Preparing your Retrospection ...��69   #��Z 6a   $��30(�6�   �� 5@      $��Center=�6   $A��;This might take a few moments. It will be worth the wait :)��69   $��0 5 0 0q�6|   5A      5B      � 5D   
   #��Z 6   $��3��6H    5E         DarkGrayColorBrush��%�   '��       7���� 5F      " ��6   8	   .�5G      �� 
 � ��# Z 63   �� 5H      
 � ��# Z 63   �� 5I      
 � ��# Z 64   �� 5J      
 � ��$ 5 0 5 0q�60   5K      5L       5N      �� 5O      $��* �6#   �� 5P      $
��Auto �6#   .+�5S      �� $��0��6   $�� ��6(   �� 5U      $��1��6   $ 
Horizontal=�6)   .+�5V      �� -   5        $��Refresh��5V      $��Center=�6C   $��0 0 10 0q�6^   �� 5W      -   5        $ FeedbackInfo��5W      $��Feedback��6/   $��Center=�6[   $��0 0 10 0q�6v   �� 5X      -   5        $ PrivacyStatement��5X      $��Privacy Statement��63   $��Center=�6p   $��0 0 10 0q�6�   �� 5Z      -   5        $ 	AboutInfo��5Z      $��About��6,   $��Center=�6R   $��0 0 10 0q�6m   5[      5\      5^      5_      