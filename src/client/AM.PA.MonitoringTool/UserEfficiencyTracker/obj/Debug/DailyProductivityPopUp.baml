   M S B A M L   `   `   `  ����H  DShared, Version=2.0.6924.32185, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared  5	   	    UserEfficiencyTracker2   ,UserEfficiencyTracker.DailyProductivityPopUp   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   8paColor$clr-namespace:Shared;assembly=Shared     5	   	   -   5          � SizeToContent$  Height=�5
   	   $	��250��6    $	��500��6-    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������ .��5      �  J� RenderTransformOrigin$	 0,1&�6   .+�5   
   �� $��1q�6   
�������   Shared.Settings   RetrospectionColorBrush#��Z 68   ._�5      �� $��10q�6   .+�5      �� #��Z 6   $
��Bold"�6]   $��16(�6o   ?=Compared to your normal level of productivity, how productive�� 5   S   do you consider your �� 5   +    �� TextDecorations$ 	Underline{�60   .�6L   previous workday6^   �� 6d   -   5         ��Name$ LastTimeWorked��5   h   6�   ?5      �� 5      $J��DHint: you can also just type the key 1-7 if this pop-up is in focus.��6   
�������6h   �� 5      $��
-2 30 -2 5q�6   $
��Left=�61    �� Orientation$ 
Horizontal=�6L    � 	Resources 5      	 �� 
TargetType
 �� BasedOn �� Property �� Value��%�   '��       7���� 5      "	 ��6    8
   .�5      ��  � ��
 �����6;   �� 5        � ��# Z 6;   �� 5!       � ��# Z 6<   5"      5#      .+�5%      �� -   5        $��1��5%      $��30��6)   $��30��64   $��	15 0 40 0q�6@   6S   �� 5&      -   5        $��2��5&      $��30��6)   $��30��64   $��0 0 40 0q�6@   6R   �� 5'      -   5        $��3��5'      $��30��6)   $��30��64   $��0 0 40 0q�6@   6R   �� 5(      -   5        $��4��5(      $��30��6)   $��30��64   $��0 0 40 0q�6@   6R   �� 5)      -   5        $��5��5)      $��30��6)   $��30��64   $��0 0 40 0q�6@   6R   �� 5*      -   5        $��6��5*      $��30��6)   $��30��64   $��0 0 40 0q�6@   6R   �� 5+      -	   5        $��7��5+      $��30��6)   $��30��64   6@   5,      �� 5-      $ 
Horizontal=�6   .+�5.      �� $
��Left=�6    $��0 0 0 0q�6;   $��not at all productive��6L   �� 5/      $
��Left=�6    $��75 0 0 0q�6;   $��moderately productive��6M   �� 50      $
��Left=�6    $��95 0 0 0q�6;   $��very productive��6M   51      �� 53      $
��Left=�6   $��0 45 0 5q�67   
�������$&�� Or, don't respond to the pop-up:��6[   $
��Bold"�6�   � 54       � ColumnDefinitions 55      �� 56      $��* �6+   �� 57      $��* �6+   �� 58      $��* �6+   �� 59      $��* �6+   .+�5;      �� -
   5        $��0��5;      $��Skip response��6-   
�������
�������
�������$��20��6}   $��0 0 5 0q�6�   6�   5?      5@      5A       � Triggers 5D   
   :� 5E       :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5F      �� .��5G      �� .��5H      U�  �� TargetProperty$9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�68   .\�5I      ��  O� KeyTime$ 0:0:0��63    O� Value$ 0Q�6C   �� 5J      $ 0:0:0.5��63   $ 1Q�6E   5K      5P      5Q      5R      z�5U   
   �� 5V       �� ScaleY$ 1Q�6   5Y      5[      