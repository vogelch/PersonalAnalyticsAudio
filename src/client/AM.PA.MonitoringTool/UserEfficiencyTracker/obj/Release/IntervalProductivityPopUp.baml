   M S B A M L   `   `   `  ���� H  DShared, Version=2.0.6926.34311, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared   UserEfficiencyTracker5   /UserEfficiencyTracker.IntervalProductivityPopUp   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9paColors$clr-namespace:Shared;assembly=Shared     -     � SizeToContent$  Height=�$	��250��$	��500�� � WindowStyle$
 None=� � AllowsTransparency . 
������ .���  J� RenderTransformOrigin$	 0,1&�.+��� $��1q�
�������   Shared.Settings   RetrospectionColorBrush#��Z ._��� $��10q�.+��� #��Z $
��Bold"�$��17(�0.Compared to your normal level of productivity,�� %#how productive do you consider the ��  �� TextDecorations$ 	Underline{�.�previous session?�� $J��DHint: you can also just type the key 1-7 if this pop-up is in focus.��
��������� -    ��Name$ LastTimeFilledOut��$�� ��
��������� $��
-2 30 -2 5q�$
��Left=� �� Orientation$ 
Horizontal=� � 	Resources 	 �� 
TargetType
 �� BasedOn �� Property �� Value��%`   '��       7���� "	 ��8
   .���  � ��
 �������  � ��# Z ��  � ��# Z .+��� -   $��1��$��30��$��30��$��	15 0 40 0q��� -   $��2��$��30��$��30��$��0 0 40 0q��� -   $��3��$��30��$��30��$��0 0 40 0q��� -   $��4��$��30��$��30��$��0 0 40 0q��� -   $��5��$��30��$��30��$��0 0 40 0q��� -   $��6��$��30��$��30��$��0 0 40 0q��� -	   $��7��$��30��$��30���� $ 
Horizontal=�.+��� $
��Left=�$��0 0 0 0q�$��not at all productive���� $
��Left=�$��75 0 0 0q�$��moderately productive���� $
��Left=�$��95 0 0 0q�$��very productive���� $
��Left=�$��0 29 0 5q�
�������$��Or, postpone the pop-up:��$
��Bold"��  � ColumnDefinitions �� $��* ��� $��* ��� $��* ��� $��* � ��%h   '��       7���� "	 ��8
   .���  � ��
 �������  � ��
 �������  � ��
 �����.+��� -
   $��0��$��I didn't work��$��20��$��0 0 5 0q��� -   $��1��$��Postpone for 5mins��$��20��$��0 0 5 0q��� -   $��2��$��Postpone for 1hr��$��20��$��0 0 5 0q��� -   $��3��$��Postpone for 6hrs��$��20�� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��.[��� .���� .��U�  �� TargetProperty$9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�.\���  O� KeyTime$ 0:0:0�� O� Value$ 0Q��� $ 0:0:0.5��$ 1Q�z���  �� ScaleY$ 1Q�