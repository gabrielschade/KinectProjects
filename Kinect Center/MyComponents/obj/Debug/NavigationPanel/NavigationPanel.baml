   M S B A M L   `   `   `  ����G  CMyComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullK*clr-namespace:MyComponents.NavigationPanelMyComponents.NavigationPanel  5      U QMahApps.Metro, Version=0.10.0.1, Culture=neutral, PublicKeyToken=f4fb5a3c4d1e5b4fV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5      2    ,MyComponents.NavigationPanel.NavigationPanel   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      <local*clr-namespace:MyComponents.NavigationPanel     5      PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5        � 	Resources  5
       �� 
TargetType �� Property �� Value2   ,MyComponents.NavigationPanel.CenterConverter   centerConverter  SlideAndFadeIn g� Duration �� TargetProperty t� From	 t� To g� DecelerationRatio	 W� From	
 W� To  SlideAndFadeOut g� AccelerationRatio  SlideAndFadeBackIn  SlideAndFadeBackOut��%�  '��       &	  n     &	 �     &	 P    &	     &	 �    �� 5      " ��6
   .�5      ��  �@�LayoutTransform 5      �� 5      5      5        5      6   �� 5      .��5      t� $ 0:0:.25D�6   $ Margin�6+   $ 300,0,-300,0X�6N   $ 0X�6b   $	 .95Q�6i   W� 5      $ 0:0:.25D�6   $ Opacity�6(   $	 0X�6L   $
 1X�6U   5      �� 5      .��5      t� $ 0:0:.5D�6   $ Margin�6*   $ -500,0,500,0X�6M   $ .9Q�6_   W� 5      $ 0:0:.5D�6   $ Opacity�6'   $
 0X�6K   5      �� 5"   
   .��5#      t� $ 0:0:.25D�6!   $ Margin�64   $ -300,0,300,0X�6W   $ 0X�6k   $	 .95Q�6r   W� 5$      $ 0:0:.25D�6   $ Opacity�61   $	 0X�6U   $
 1X�6^   5%      �� 5'   
   .��5(      t� $ 0:0:.5D�6!   $ Margin�63   $ 500,0,-500,0X�6V   $ .9Q�6h   W� 5)      $ 0:0:.5D�6   $ Opacity�60   $
 0X�6T   5*      5,      .��5.      � -   5        $
��page��5.      .+�50      �� -   5        $��contentPresenter��50      z�51      V� 52      .~�53      ��  �� ScaleX$ 1Q�6    �� ScaleY$ 1Q�6!    �� CenterX 54   
   ��  �� RelativeSource ��  �� AncestorType$
 GridL� �� Mode$ FindAncestor=� �� Path$ ActualWidth� �� 	Converter# [   �� CenterY 55   
   ��  �� $
 GridL�$ FindAncestor=�$ ActualHeight�# [  �� 56       �� AngleX$ 0Q�6    �� AngleY$ 0Q�6     �� CenterX 57      ��  �� $
 GridL�$ FindAncestor=�$ ActualWidth�# [   �� CenterY 58      ��  �� $
 GridL�$ FindAncestor=�$ ActualHeight�# [  �� 59       �� Angle$ 0Q�6    �� CenterX 5:      ��  �� $
 GridL�$ FindAncestor=�$ ActualWidth�# [   �� CenterY 5;      ��  �� $
 GridL�$ FindAncestor=�$ ActualHeight�# [  T� 5<       T� X$ 0Q�6    T� Y$ 0Q�6    5=      5?      5A      5C      