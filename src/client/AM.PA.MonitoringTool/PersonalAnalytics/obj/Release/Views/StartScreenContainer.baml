   M S B A M L   `   `   `  ���� L  HPersonalAnalytics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullA%clr-namespace:PersonalAnalytics.ViewsPersonalAnalytics.Views  H DShared, Version=2.0.6926.34311, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared 2    ,PersonalAnalytics.Views.StartScreenContainer   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     7local%clr-namespace:PersonalAnalytics.Views     9paColors$clr-namespace:Shared;assembly=Shared       � Title$   PersonalAnalytics: Welcome��$	��540��$	��700��.���  � RowDefinitions �� $��40 ��� $��* ��� $
��auto ��� $
��auto �.+�� $��0��  Shared.Settings   RetrospectionColorBrush#��Z .+��� -   $	��Top��$��10 0 0 0q�$��16(�
�������$��Center=��� -   $��Content��$��1��$��10q��� -   $��Buttons��$��2�� �� Orientation$ 
Horizontal=�$��Right=�$��30��$��10q�.+��� -   $
��Back��$
��Back��$��50��$��10 0 0 0q�
�������   DarkGrayColorBrush#��Z #��Z {�.  �� -   $
��Next��$
��Next��$��50��$��10 0 0 0q�
�������#��Z #��Z �    GrayColorBrush#��Z $��3�� � 	Resources  �� 
TargetType �� BasedOn	 �� Property
 �� Value��%{   '��       7���� " ��8   .��� 	 � ��

 ������� 	 � ��#
 Z �� 	 � ��#
 Z �� 	 � ��$
 5 0 5 0q� � ColumnDefinitions �� $��* ��� $
��Auto �.+��� -    ��Name$ 	TbVersion��$��0��$��10 0 0 0q�#��Z �� -   $��1��$��Feedback or Questions��$��Center=�$��0 0 5 0q�