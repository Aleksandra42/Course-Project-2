   M S B A M L   `   `   `  ����B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp  5   	       RiskApp.AdminTree   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:RiskApp     5   	   -   5          � Title$  Administrator's Risk Tree��5   	    � 
ResizeMode$ NoResize=�6H    � WindowState$ Normal=�6^   ���	#FFF4E4C0.��5	      � -   5         �Name$
 grid��5	      $��0,-8,-0.4,0.6q�6   .+�5
   
   �� -   5         ��Name$ 
BackButton��5
      $��40��6%   $��40��61   $	��Top=�6V   $
��Left=�6n   $��	23,26,0,0q�6�    J� RenderTransformOrigin$ 	0.25,0.25&�6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      5      � 5   
   -   5        $ canvas��5      $��	0,0,0,200q�6   �� 5   
   -   5         ��Name$ RiskName��5      $��30,180,800,200q�6&   $
��Wrap=�6>   �� 5   
   $��	25,0,0,25q�6   $	��150��6'   $��Bottom=�64   $
��Left=�6O   $	��290��6j   .��5      � $	��145��6   $��0,0,0,0q�6    .+�5      �� $��Description:��6   $��10,70,179.6,45q�6/   �� 5      -   5         ��Name$ TextboxProbability��5      $
��Left=�66   $��
86,0,0,115q�6Q   $	��150��6e   �� 5      -   5        $ TextboxCost��5      $
��Left=�6/   $��
86,35,0,80q�6J   $	��150��6^   �� 5      -   5        $ TextboxDescription��5      $
��Left=�66   $��
86,70,0,45q�6Q   $	��150��6e   $ 1.672,-3.406&�6q   �� 5      $��Cost:��6   $��10,35,197.6,80q�6(   �� 5      $��Probability:��6   $��10,0,184.6,115q�6/   �� 5      -	   5        $ 	AddButton��5      $��Add new vertex��6,   $��10,108,53.6,7q�6E   ���	#FFE1F37C 5        5      5      5      5       �� 5!   
   $	��150��6   $��Bottom=�6!   $��Right=�6<   $	��290��6X   $��25,25q�6d   .��5"      � $	��145��6   $��-151,0,151.6,0q�6    .+�5#      �� -
   5        $ DeleteButton��5#      $��Delete selected vertex��6/   $��224,105,-160,10q�6P   ���	#FFE1F37C 5$        5%      5'      5(      5)      �� 5*   
   -   5        $ CreateReport��5*      $��Create Report��6'   $	��Top=�6?   $��Right=�6W   $��	0,25,25,0q�6s   $	��100��6�   $��30��6�    5+        5,      5.      5/      50      