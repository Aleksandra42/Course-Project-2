   M S B A M L   `   `   `  ����B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp  5   	       RiskApp.ReportWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:RiskApp     5   	   -   5          � Title$  Report Window��5   	    � 
ResizeMode$ NoResize=�6<    � WindowState$ 	Maximized=�6R   ���	#FFF4D3D3.��5	      � -   5         �Name$
 grid��5	      .+�5
   
   �� -   5         ��Name$ CloseButton��5
      $��Close Report��6&   $��20��6Q   $��92��6]   $	��Top=�6h   $��Right=�6�   $��
0,10,9.6,0q�6�    J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      5      �� 5   
   -   5        $��Open Document��5      $��20��6)   $	��Top=�65   $
��Left=�6M   $��	10,10,0,0q�6�    5        5      5      �� 5   
   	 �� X1$ 75��6   	 �� X2$ 75��6   	 �� Y1$ 50��6   		 �� Y2$		 400��6'   
���   �60   �� 5   
   $ 75��6   $	 500��6   $	 400��6    $		 400��6)   
���   �62   �� 5   
   -   5        
 ��Name$
 InfluenceLabel��5      $��Impact��6(   $��20,40,1100,590q�69   �� 5   
   -   5        $
 ProbabilityLabel��5      $��Probability��6*   $��430,395,685,230q�6@   � 5   
   
���   � � Points' �   �, ���hx��� �� ��6"   � 5   
   
���   �' �   ��4�8� e� ����4@�#6"   �� 5   
   -   5         ��Name$ canvas��5      5      �� 5   
   $��The most dangerous risks��6   $��Bottom=�63   $��Center=�6N   $��22(�6k   $��	0,0,0,300q�6z   �� 5   
   -   5         ��Name$ listDangerous��5      $��Center=�6*   $��Bottom=�6G   $	��250��6b   $��0,0,0,50q�6o    �� View 5      �� 5       .P�5!      ��  �� Width$	 150��6%   .��62   Name of Risk �� CellTemplate 5#      �� 5$      .R�5%   "   �� $
��Wrap=�6F   ��6,   �� *
RiskName+6F   5&      5(      �� 5)      $	 150��6%   .��62   Source 5+      �� 5,      .R�5-   "   �� $
��Wrap=�6D   ��6,   �� *Source+6D   5.      50      �� 51      $	 150��6%   .��62   Possible Solution 53      �� 54      .R�55   "   �� $
��Wrap=�6F   ��6,   �� *
Solution+6F   56      58      �� 59      $	 150��6%   .��62   	Effects 5;      �� 5<      .R�5=   "   �� $
��Wrap=�6E   ��6,   �� *	Effects+6E   5>      5@      �� 5A      $	 150��6%   .��61   Rank 5C      �� 5D      .R�5E   "   �� $
��Wrap=�6B   ��6,   �� *Rank+6B   5F      5H      5I      5K      5L      5M      