   M S B A M L   `   `   `  ���� O  KRetrospection, Version=1.0.6927.23886, Culture=neutral, PublicKeyToken=null-clr-namespace:RetrospectionRetrospection  H DShared, Version=2.0.6926.34311, Culture=neutral, PublicKeyToken=null/$clr-namespace:Shared;assembly=SharedShared     Retrospection.AboutWindow   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -localclr-namespace:Retrospection     9paColors$clr-namespace:Shared;assembly=Shared       � Title$  PersonalAnalytics: About��$	��420��$	��800�� � 
ResizeMode$ NoResize=�.���  � RowDefinitions �� $��40 ��� $��* ��� $��20 �.+�� $��0��  Shared.Settings   RetrospectionColorBrush#��Z .+��� $��0��$��16(�$��Center=�$��10 0 0 0q�$��About the Application��
��������� $��10q�$��1��.+��� $��0 0 0 5q�$��Authors:��$
��Wrap=�$
��Bold"��� -   $��0 0 0 5q� �� Orientation$ 
Horizontal=�.+��� $7��1Andre Meyer (PhD Student, University of Zurich): ���� $��ameyer@ifi.uzh.ch��$��	Underline{��� $"�� (Developer of the software)���� -   $��0 0 0 5q�$ 
Horizontal=�.+��� $>��8Tom Zimmermann (Senior Researcher, Microsoft Research): ���� $��tzimmer@microsoft.com��$��	Underline{��� -   $��0 0 0 30q�$ 
Horizontal=�.+��� $6��0Thomas Fritz (Professor, University of Zurich): ���� $��fritz@ifi.uzh.ch��$��	Underline{��� $��0 0 0 5q�$��	Releases:��$
��Wrap=�$
��Bold"��� $��0 0 0 30q�$
��Wrap=�xvAn overview about all releases of PersonalAnalytics, including detailed change logs, can be found on our Github page: �� -   $;��5https://github.com/sealuzh/PersonalAnalytics/releases?�.�75https://github.com/sealuzh/PersonalAnalytics/releases�� $��0 0 0 5q�$��Privacy Statement:��$
��Wrap=�$
��Bold"��� -   $��0 0 0 30q�$ 
Horizontal=�.+��� $��Access from this link: ��$
��Wrap=��� -    ��Name$ PrivacyStatementUriText��$	��N/A��$��	Underline{��� $��0 0 0 5q�$��Credits:��$
��Wrap=�$
��Bold"��� $��0 0 0 30q�$a��[SQLite, JQuery, Masonry, D3.js, C3.js, HTML FilterTable, MousekeyHook, Hardcodet.NotifyIcon��$
��Wrap=��    GrayColorBrush#��Z $��2�� � 	Resources  �� 
TargetType	 �� BasedOn
 �� Property   DarkGrayColorBrush �� Value��%w   '��       7���� " ��8	   .��� 
 � ��# Z �� 
 � ��# Z �� 
 � ��# Z �� 
 � ��$ 5 0 5 0q� � ColumnDefinitions �� $��* ��� $
��Auto �.+��� -   $ 	TbVersion��$��0��$��10 0 0 0q�#��Z �� -   $��1��$��Feedback��$��Center=�$��0 0 5 0q�