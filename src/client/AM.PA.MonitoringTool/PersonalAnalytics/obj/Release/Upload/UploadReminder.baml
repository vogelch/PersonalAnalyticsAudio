   M S B A M L   `   `   `  ���� L  HPersonalAnalytics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:PersonalAnalytics.UploadPersonalAnalytics.Upload  H DShared, Version=2.0.6926.34311, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared -    'PersonalAnalytics.Upload.UploadReminder   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     8local&clr-namespace:PersonalAnalytics.Upload     9paColors$clr-namespace:Shared;assembly=Shared       � SizeToContent$  Height=�$	��215��$	��550�� � WindowStyle$
 None=� � AllowsTransparency . 
������ .���  J� RenderTransformOrigin$	 0,1&�.+��� $��1q�  Shared.Settings   RetrospectionColorBrush#��Z 
���   �._��� $��10q�.+��� $M��GWe would like to ask you to share the collected data to help us perform��
�������$
��Bold"�$��15(��� $;��5additional research and to improve PersonalAnalytics.��
�������$
��Bold"�$��15(�� $��	0 50 0 50q� � ColumnDefinitions �� $��* ��� $��* �.+��� -   $��0��$	��200��$��Share the data��   GrayColorBrush#��Z $��20��$��0 0 5 0q��� -   $��1��$	��200��$��Don't share the data��#��Z $��20��$��0 0 5 0q��� $��	30 0 30 0q�$
��Wrap=�
�������$����Hint: If you hit 'Share the data', you will be able to obfuscate and check the data before it's uploaded to a secured, internal server. This pop-up will be shown again in a week.�� � Triggers :�  :� RoutedEvent$ FrameworkElement.Loaded��.[��� .���� .��U� 	 �� TargetProperty$9	 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�.\��� 
 O� KeyTime$
 0:0:0�� O� Value$ 0Q��� $
 0:0:0.5��$ 1Q�z���  �� ScaleY$ 1Q�