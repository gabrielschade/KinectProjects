   M S B A M L   `   `   `  ����G  CMyComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS9clr-namespace:MyComponents.Controls;assembly=MyComponentsMyComponents.Controls  5      M IMyKinectComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulleEclr-namespace:MyKinectComponents.Controls;assembly=MyKinectComponentsMyKinectComponents.Controls 5      iDclr-namespace:MyComponents.VisibilityAnimation;assembly=MyComponents MyComponents.VisibilityAnimation  5	       Kinect Center!   Kinect_Center.View.Arkanoid   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      \ XMicrosoft.Kinect.Toolkit.Controls, Version=0.0.0.0, Culture=neutral, PublicKeyToken=nullS OMicrosoft.Kinect.Toolkit, Version=1.8.0.0, Culture=neutral, PublicKeyToken=null2k(http://schemas.microsoft.com/kinect/2013   5      Fmc;http://schemas.openxmlformats.org/markup-compatibility/2006   5      Jcontrols9clr-namespace:MyComponents.Controls;assembly=MyComponents   5      \kinectControlsEclr-namespace:MyKinectComponents.Controls;assembly=MyKinectComponents   5      <d2http://schemas.microsoft.com/expression/blend/2008   5      V	animationDclr-namespace:MyComponents.VisibilityAnimation;assembly=MyComponents   5	      -   5        .��5      � -   5          �Name$
  Root��5      
�������.+�5   
   � -   5        $  gamePlay��5       � RowDefinitions 5      �� 5      $��* �6    �� 5      $��* �6     � ColumnDefinitions 5      �� 5      $��* �6#   �� 5      $	��150 �6#   .+�5      (� $��0��6    (� Stretch$ Uniform=�6#   $��1��65   .��5      �� $��1��6   $
��0.52��6#   .+�5      �� -   5         ��Name$ waveDisplay��5   8   $
��0.74��6   $��0.185��6)   $	��Top=�6M   $
��Left=�6e   z�5      T� 5       T� X$
 0.13Q�61    T� Y$ 0.0353Q�6:   5      F� 5      T����K @R Ѡz �\ נ� �\  0W 0W �e�$� @R �K @R ��� �� �e	��5      
� 5        
� Center$ 	0.5 -0.32&�62    
� RadiusX$
 0.65Q�6E   	 
� RadiusY$		 1.1Q�6T   ��5"   &   � 
 � Color$
 	LightGray��63    � Offset$ 0Q�6E   � 5#   &   $
 	LightGray��63   $	 0.4Q�6E   � 5$   &   $
 
WhiteSmoke��63   $	 0.6Q�6F   � 5%   &   $
 
WhiteSmoke��63   $ 1Q�6F   5'      5)      F� 5*      T����� � �K @R �$� @R  �� �� �e�(R � �� � � S  S �e	��5+      �� 5,       �� 
StartPoint$ 0,0.5&�62    �� EndPoint$ 1,0.5&�6E   ��5.   &   � -   5        sourceGsStart��$
 White��5.   K   $ 0Q�6[   � 5/   &   -   5        sourceGsPre��$
 White��5/   K   $
 0.45Q�6[   � 50   &   -   5        sourceGsMain��$

 Blue��50   K   $	 0.5Q�6Y   � 51   &   -   5        sourceGsPost��$
 White��51   K   $
 0.55Q�6[   � 52   &   -	   5        sourceGsEnd��$
 White��52   K   $ 1Q�6[    �� 	Transform 54   "   �� 55   &   -
   5        sourceRotation�� �� CenterX$	 0.5Q�55   N    �� CenterY$	 0.0Q�6\    �� Angle$ 0Q�6j   6v   57      59      F� 5:      "���� 0W Ѩ� 0W � � �� 	z�5;      �� 5<      -   5        beamRotation��$	 0.5Q�5<   E   $	 0.0Q�6S   $ 0Q�6a   ��5>      �� 5?      .��5@   "   � $
 	LightGray��6/   $ 0Q�6A   � 5A   "   $

 Blue��6/   $	 0.5Q�6<   � 5B   "   $

 Blue��6/   $ 1Q�6<   5C      5E      F� 5F      T����� � Ѡz �\ נ� �\  0W 0W �e�(R � �� � � S  S �e	$��0.001��6�   
�������6�   F� 5G      +���Рz �\ ��� �\ ��� �  ��I �  	$��0.001��6\   
�������6t   F� 5H      +���Р� �\ �pF �\ �pF �  �P� �  	$��0.001��6\   
�������6t   5I      5J      �� 5N      -   5         ��Name$ 
playground��5N      $��2��6)   $��2��6=   .+�5O      �� -   5         ��Name$ paddle��5O      $	��200��6,   $��20��68   ���	#FF4E86FF6D   A� 5P      -   5         A�Name$
 ball��5P      $��20��6(   $��20��63   
���   �6?   5Q      �� 5S      -   5         ��Name$ 	scoreText��5S      $��52(�6+   $	��Top=�69   $��Right=�6Q   $��0,0,5,0q�6m   
���   �$��Segoe UI Light)�6�   4  .Microsoft.Kinect.Toolkit.Controls.KinectRegion  5T      -   5          Name$ kinectRegion��5T   U   $��1��6   $��2��6-   $y�	Collapsed=�6>   .��5U      3  -MyKinectComponents.Controls.KinectCommandsBar  -   5          Name$ commandsBar��5U   4   5V      5W      5X      5Y      