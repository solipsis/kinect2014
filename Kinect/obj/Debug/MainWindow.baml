   M S B A M L   `   `   `  ����
  Kinect    Kinect.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   d `Microsoft.Kinect.Wpf.Controls, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e350k(http://schemas.microsoft.com/kinect/2014  5   	   localGameInfo.cs  5   	     � Title$  Kinect��5   	    � WindowStyle$
 None=�6    � WindowState$ 	Maximized=�6,   $��1196.358��6D   $��795.821��6U   ��5	      �� 5   
    �� Color$
 Blue��6   .��5      0  *Microsoft.Kinect.Wpf.Controls.KinectRegion  -   5        $��KinectRegion��5      $��0,0,-1,0q�6)   .��5   
   � -   5         �Name$ FullGrid��5      $��	0,0,788,0q�6!   
�������.+�5      �� .��5      � -   5        $ MainGrid��5       � ColumnDefinitions 5      �� 5      $��1* �6+   �� 5      $��1* �6+    � RowDefinitions 5      �� 5      $��1* �6(   �� 5      $��1* �6(   �� 5      $��1* �6(   �� 5      $��1* �6(   5      5      �� 5      -   5        $��SelectedTitle��5       �� TextAlignment$ Center=�6.   $��Right=�6E   $	��205��6a   $��
0,0,-785,0q�6n   $
��Wrap=�6�   $��TextBox��6�   $	��Top=�6�   $	��780��6�   
�������$��Courier New)�6�   $
��Bold"�6�   �� 5      -   5        $��SelectedDescription��5      $
 Left=�63   $��Right=�6H   $��0,306,-785,0q�6d   $
��Wrap=�6z   $��	TextBlock��6�   $	��Top=�6�   $	��780��6�   $	��212��6�   
�������$��Courier New)�6�   $
��Bold"�6�   �� 5      -   5        $��
HighScores��5      $ Center=�6*   $��Right=�6A   $��0,522,-785,23q�6]   $
��Wrap=�6t   $��	TextBlock��6�   $��Center=�6�   $	��780��6�   $	��220��6�   
�������$��Courier New)�6�   � 5       -   5        $ PlayGrid��5       $��0,105,-785,560q�6%   $��Right=�6=   $��Center=�6Y    J� RenderTransformOrigin$ 0.5,0.5&�6t   $	��780��6�   $	��100��6�   5"      5%      5&      