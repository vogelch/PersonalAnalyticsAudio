   M S B A M L   `   `   `  ����O  KRetrospection, Version=1.0.6927.23898, Culture=neutral, PublicKeyToken=null-clr-namespace:RetrospectionRetrospection  5   	   H DShared, Version=2.0.6924.32185, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared 5   	       Retrospection.AboutWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Retrospection     5   	   9paColors$clr-namespace:Shared;assembly=Shared     5   	     � Title$  PersonalAnalytics: About��5	   	   $	��420��6*   $	��800��67    � 
ResizeMode$ NoResize=�6C   .��5      �  � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      $��* �6   �� 5      $��20 �6   .+�5   
   � $��0��6     Shared.Settings   RetrospectionColorBrush#��Z 6   .+�5      �� $��0��6   $��16(�6%   $��Center=�63   $��10 0 0 0q�6N   $��About the Application��6`   
�������6}   5      �� 5   
   $��10q�6   $��1��6!   .+�5      �� $��0 0 0 5q�6   $��Authors:��6)   $
��Wrap=�69   $
��Bold"�6M   �� 5      -   5        $��0 0 0 5q�5       �� Orientation$ 
Horizontal=�6K   .+�5      �� $7��1Andre Meyer (PhD Student, University of Zurich): ��6   �� 5      $��ameyer@ifi.uzh.ch��6   $��	Underline{�65   �� 5      $"�� (Developer of the software)��6   5      �� 5      -   5        $��0 0 0 5q�5      $ 
Horizontal=�6I   .+�5      �� $>��8Tom Zimmermann (Senior Researcher, Microsoft Research): ��6   �� 5      $��tzimmer@microsoft.com��6   $��	Underline{�69   5       �� 5!      -   5        $��0 0 0 30q�5!      $ 
Horizontal=�6M   .+�5"      �� $6��0Thomas Fritz (Professor, University of Zurich): ��6   �� 5#      $��fritz@ifi.uzh.ch��6   $��	Underline{�64   5$      �� 5&      $��0 0 0 5q�6   $��	Releases:��6)   $
��Wrap=�6:   $
��Bold"�6N   �� 5'      $��0 0 0 30q�6   $
��Wrap=�6*   xvAn overview about all releases of PersonalAnalytics, including detailed change logs, can be found on our Github page: �� 5)      -   5        $;��5https://github.com/sealuzh/PersonalAnalytics/releases?�5*      .�5+   =   75https://github.com/sealuzh/PersonalAnalytics/releases5-      5.      �� 50      $��0 0 0 5q�6   $��Privacy Statement:��6)   $
��Wrap=�6C   $
��Bold"�6W   �� 52      -   5        $��0 0 0 30q�52      $ 
Horizontal=�6W   .+�53      �� $��Access from this link: ��6   $
��Wrap=�6;   �� 54      -   5         ��Name$ PrivacyStatementUriText��54      $	��N/A��6=   $��	Underline{�6H   55      �� 57      $��0 0 0 5q�6   $��Credits:��6)   $
��Wrap=�69   $
��Bold"�6M   �� 58      $��0 0 0 30q�6   $a��[SQLite, JQuery, Masonry, D3.js, C3.js, HTML FilterTable, MousekeyHook, Hardcodet.NotifyIcon��6*   $
��Wrap=�6�   5<      � 5>   
      GrayColorBrush#��Z 6   $��2��6H    � 	Resources 5?       �� 
TargetType	 �� BasedOn
 �� Property   DarkGrayColorBrush �� Value��%�   '��       7���� 5@      " ��6   8	   .�5A      �� 
 � ��# Z 63   �� 5B      
 � ��# Z 63   �� 5C      
 � ��# Z 64   �� 5D      
 � ��$ 5 0 5 0q�60   5E      5F       � ColumnDefinitions 5G      �� 5H      $��* �6#   �� 5I      $
��Auto �6#   .+�5L      �� -   5        $ 	TbVersion��5L   (   $��0��6   $��10 0 0 0q�6;   #��Z 6M   �� 5M      -   5        $��1��5M      $��Feedback��6%   $��Center=�6Q   $��0 0 5 0q�6l   5N      5O      5P      