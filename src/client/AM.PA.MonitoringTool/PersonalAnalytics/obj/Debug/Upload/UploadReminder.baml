   M S B A M L   `   `   `  ����L  HPersonalAnalytics, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:PersonalAnalytics.UploadPersonalAnalytics.Upload  5   	   H DShared, Version=2.0.6924.32185, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared 5   	   -    'PersonalAnalytics.Upload.UploadReminder   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5	   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5
   	   8local&clr-namespace:PersonalAnalytics.Upload     5   	   9paColors$clr-namespace:Shared;assembly=Shared     5   	     � SizeToContent$  Height=�5   	   $	��215��6    $	��550��6-    � WindowStyle$
 None=�5   	    � AllowsTransparency . 
������ .��5      �  J� RenderTransformOrigin$	 0,1&�6   .+�5   
   �� $��1q�6     Shared.Settings   RetrospectionColorBrush#��Z 6%   
���   �._�5      �� $��10q�6   .+�5      �� $M��GWe would like to ask you to share the collected data to help us perform��6   
�������$
��Bold"�6~   $��15(�6�   �� 5      $;��5additional research and to improve PersonalAnalytics.��6   
�������$
��Bold"�6l   $��15(�6~   � 5      $��	0 50 0 50q�6    � ColumnDefinitions 5      �� 5      $��* �6+   �� 5      $��* �6+   .+�5      �� -   5        $��0��5      $	��200��6-   $��Share the data��69      GrayColorBrush#��Z 6R   $��20��6�   $��0 0 5 0q�6�   6�   �� 5      -   5        $��1��5      $	��200��6-   $��Don't share the data��69   #��Z 6X   $��20��6�   $��0 0 5 0q�6�   6�   5       �� 5!      $��	30 0 30 0q�6   $
��Wrap=�6/   
�������$����Hint: If you hit 'Share the data', you will be able to obfuscate and check the data before it's uploaded to a secured, internal server. This pop-up will be shown again in a week.��6V   5"      5#       � Triggers 5&   
   :� 5'       :� RoutedEvent$ FrameworkElement.Loaded��6   .[�5(      �� .��5)      �� .��5*      U� 	 �� TargetProperty$9	 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�68   .\�5+      �� 
 O� KeyTime$
 0:0:0��63    O� Value$ 0Q�6C   �� 5,      $
 0:0:0.5��63   $ 1Q�6E   5-      52      53      54      z�57   
   �� 58       �� ScaleY$ 1Q�6   5;      5=      