   M S B A M L   `   `   `  ����F  BGallery_WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Gallery_WPFGallery_WPF  5   	       Gallery_WPF.WaterMarkText   5   	   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Gallery_WPF     5   	     � Title$  PictureInfo��5      $	��200��6   $	��450��6)    � AllowsTransparency$
 True��66    � WindowStyle$
 None=�5	   	    � 
ResizeMode$ CanMinimize=�6    � WindowStartupLocation$ CenterScreen=�66   
������� J� Clip 5      �� 5   
    �� Rect$ 0 0 450 200�6    �� RadiusX$ 25Q�6/    �� RadiusY$ 25Q�6<   	 � 	Resources	 5         FocusVisual
 �� Property �� Value �� StrokeDashArray J� SnapsToDevicePixels  Button.Static.Background �� Color  Button.Static.Border  Button.MouseOver.Background  Button.MouseOver.Border  Button.Pressed.Background  Button.Pressed.Border  Button.Disabled.Background  Button.Disabled.Border 	 Button.Disabled.Foreground 
 RoundedButtonStyle �� 
TargetType �� 
TargetType ��Name �� CornerRadius ��Name �� Triggers P� Property P� Value �� 
TargetName��%�  &	        &	 �     &	 �     &	 �     &	     &	 ?    &	 b    &	 �    &	 �    &		 �    &	
 �    7   7  7  �� 5   
   .�5      �� 
 � �� 5      �� 5      .R�5      �� $��2q�6$   $	 1 2R�6/   #��� ��6E   $
 true��6�   $��1��6�   5      5      5      �� 5   
   $ 	#FFDDDDDD��6;   �� 5   
   $ 	#FFDDDDDD��67   �� 5   
   $ 	#FFBEE6FD��6>   �� 5   
   $ 	#FF3C7FB1��6:   �� 5   
   $ 	#FFDDDDDD��6<   �� 5   
   $ 	#FF2C628B��68   �� 5   
   $ 	#FFDDDDDD��6=   �� 5    
   $ 	#FFDDDDDD��69   �� 5!   
   $ 	#FFDDDDDD��6=   �� 5"   
   " ��6+   .�5#      �� 
 �@�FocusVisualStyle8   61   �� 5$      
 � ��8  6+   �� 5%      
 � ��8  6,   �� 5&      
 � ��# � ��6+   �� 5'      
 � ��$ 0q�60   �� 5(      
 � ��$ Center=�6;   �� 5)      
 � ��$ Center=�69   �� 5*      
 � ��$ 1q�6(   �� 5+      
 � �� 5,      �� 5-      " ��6&   .R�5.      �� $ border��6!   $ 20��61   #��z��6C   #��z��6m   #��z��6�   $
 true��6�   ._�5/      �� $ contentPresenter��6/   $|�False��6I   #��z��6[   #��z��6�   $
��True��6�   # z 6�   #��z��6  50       51      P� 52       �@��IsDefaulted$
 true��6=   . �53   "   �� $ border��6@   # � ��6T   
 � ��54      P� 55       �@J�IsMouseOver$
 true��6=   . �56   "   �� $ border��6?   
 � �� �#2807e66S   �� 57   "   $ border��6@   
 � �� �#2807e66T   58      P� 59       � ��$
 true��6;   . �5:   "   �� $ border��6?   
 � �� �#05FAD66S   �� 5;   "   $ border��6@   
 � �� �#05FAD66T   5<      P� 5=       � {�$ false��6;   . �5>   "   �� $ border��6?   
 � �� �#05FAD66S   �� 5?   "   $ border��6@   
 � �� �#05FAD66T   �� 5@   "   $ contentPresenter��6K   
 � �� �	#FFDDDDDD6i   5A      5C      5E      5F      5G      .��5H      � .+�5I   
   �� $��i��6   $��23(�6   $��Yu Gothic Medium)�6*   $��423,-7,10,177q�6H    � ToolTip 5J      _� 5K      $[��UThe image will be updated in the original folder as soon as you click the Save button��6   $��13(�6z   $��Yu Gothic Medium)�6�   5M      �� 5O   
   $��Enter text for watermark:��6   $��88,19,62,140q�65   $��23(�6K   $��Yu Gothic Medium)�6Y   �� 5Q   
   -   5         ��Name$ tbWaterMark��5Q      $��88,80,88,95q�6'   $��15(�6<   $��Yu Gothic Medium)�6J   �� 5R   
   -   5         ��Name$ btnSave��5R      $
��Save��6"   $��88,124,88,42q�61   #��� 
 6]   $
��Bold"�6�   $��20(�6�   ���	#FF1B9811���	#FFB5B4B46�   5S      5T      