   M S B A M L   `   `   `  ����L  HPersonalAnalytics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:PersonalAnalytics.ViewsPersonalAnalytics.Views  5   	   H DShared, Version=2.0.6924.32185, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared 5   	   2    ,PersonalAnalytics.Views.StartScreenContainer   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   7local%clr-namespace:PersonalAnalytics.Views     5   	   9paColors$clr-namespace:Shared;assembly=Shared     5   	     � Title$   PersonalAnalytics: Welcome��5	   	   $	��540��6,   $	��700��69   .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   �� 5      $
��auto �6   �� 5      $
��auto �6   .+�5   
   � $��0��6     Shared.Settings   RetrospectionColorBrush#��Z 6   .+�5      �� -   5        $	��Top��5   *   $��10 0 0 0q�6   $��16(�65   
�������$��Center=�6V   6s   5      �� 5   
   -   5        $��Content��5      $��1��6$   $��10q�61   6?   �� 5   
   -   5        $��Buttons��5   o   $��2��6    �� Orientation$ 
Horizontal=�6"   $��Right=�6;   $��30��6W   $��10q�6c   .+�5      �� -   5        $
��Back��5      $
��Back��6!   $��50��60   $��10 0 0 0q�6;   
�������   DarkGrayColorBrush#��Z 6t   #��Z 6�   {�.  6�   �� 5      -   5        $
��Next��5      $
��Next��6!   $��50��60   $��10 0 0 0q�6;   
�������#��Z 6t   #��Z 6�   5      � 5   
      GrayColorBrush#��Z 6   $��3��6H    � 	Resources 5       �� 
TargetType �� BasedOn	 �� Property
 �� Value��%�   '��       7���� 5       " ��6   8   .�5!      �� 	 � ��

 �����63   �� 5"      	 � ��#
 Z 63   �� 5#      	 � ��#
 Z 64   �� 5$      	 � ��$
 5 0 5 0q�60   5%      5&       � ColumnDefinitions 5'      �� 5(      $��* �6#   �� 5)      $
��Auto �6#   .+�5,      �� -   5         ��Name$ 	TbVersion��5,   (   $��0��6   $��10 0 0 0q�6;   #��Z 6M   �� 5-      -   5        $��1��5-      $��Feedback or Questions��6%   $��Center=�6^   $��0 0 5 0q�6y   5.      5/      50      