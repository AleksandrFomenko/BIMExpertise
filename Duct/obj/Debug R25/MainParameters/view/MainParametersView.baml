   M S B A M L   `   `   `  ����   Duct1    +Duct.MainParameters.view.MainParametersView   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     _ [MaterialDesignThemes.Wpf, Version=5.2.1.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes    � 	Resources   �� 
TargetType   MyTextBlock �� Property  	TextBrush �� Value  
MyComboBox  MaterialDesignComboBox �� BasedOn $  MaterialDesignSwitchToggleButton  MyToggleButton �� MergedDictionaries+  %MaterialDesignThemes.Wpf.BundledTheme   PrimaryColor   SecondaryColor   	BaseTheme	 �� Source��     BlueGrey 	LightBlue	 Light�� $d	 ^pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesign3.Defaults.xaml?��� $S	 Mpack://application:,,,/WpfResourcesBimExpertise;component/Themes/Generic.xaml?�%I  &	        7  &	 |     7  7  &	 �     7  �� " ��.���  � ��8   ��  � ��$ 14(���  � ��$
 Left=���  � ��$ Center=���  � ��$ 5q��� " ��8   .���  � ��8  ��  � ��$ 14(���  � ��$ 200, 0,10,0q��� " d�8   .���  � ��$ 10,0,0,0q���  � ��$
 Left=�.��� 
 � RowDefinitions
 �� $��80 ��� �� �� $��20 ��� $��50 �.+�� $��0�� � ColumnDefinitions �� �� .+��� $��0��  InputBorder#��[ $��5q�._�� 
 �� �� .+��� $��0��#��[  $#��Диапозон выбора���� $��1��$��5q�$��Center=�#��[   	LineBrush#��[  �� DisplayMemberPath$
 Name������ *
Options1+ �� SelectedItem �� *	Option1+ �� Mode$ TwoWay=��� $��1��#��[ $��5q�._�� 
 �� �� .+��� $��0��#��[  $��Способ отбора���� $��1��$��5q�$��Center=�$
��Left=�#��[ #��[ $
 Name������ *Option2.WidthComboBox+���� *
Options2+ �� *	Option2+$ TwoWay=��� $��1��#��[ $��5q�._�� 
 �� �� ��  �� �� .+��� $��0��$��0��#��[  $5��/Геометрические параметры��$��20,0,0,0q�d� $��1��$��0��#��[  d� 	IsChecked �� *
IsHeight+$ TwoWay=��� $��1��$��0��#��[  $��Высота��$��	70, 0,0,0q��� $��1��$��0��#��[ ���� *GeomParameters+ �� *HeightParameter+$ TwoWay=�d� $��2��$��0��#��[  �� *IsWidthDiameter+$ TwoWay=��� $��2��$��0��#��[  $!��Ширина/Диаметр��$��	70, 0,0,0q��� $��2��$��0��#��[ ���� *GeomParameters+ �� *WidthDiameterParameter+$ TwoWay=�d� $��1��$��1��#��[  �� *
IsLength+$ TwoWay=��� $��1��$��1��#��[  $��
Длина��$��	70, 0,0,0q��� $��1��$��1��$��
125,0,20,0q�#��[ ���� *GeomParameters+ �� *LengthParameter+$ TwoWay=�d� $��2��$��1��#��[  �� *IsMass+$ TwoWay=��� $��2��$��1��#��[  $��
Масса��$��	70, 0,0,0q��� $��2��$��1��$��
125,0,20,0q�#��[ ���� *GeomParameters+ �� *MassParameter+$ TwoWay=��� $��2��#��[ $��5q�._�� 
 �� �� �� �� � -    �Name$! DeterminateCircularProgress��$��3�� % !MaterialDesignCircularProgressBar#��[  � IsIndeterminate$ False�� � Maximum �� *ProgressMaximum+ � Value �� *ProgressValue+�� $��4�� "	 MaterialDesignRaisedDarkButton#��[	 #��[ $��Center=�$��Center=�{��� *ButtonEnable+���� *ExecuteCommand+.���� $��Execute��