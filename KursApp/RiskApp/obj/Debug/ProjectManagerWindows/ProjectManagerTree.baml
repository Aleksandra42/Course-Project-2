   M S B A M L   `   `   `  ����B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp  5   	        RiskApp.ProjectManagerTree   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:RiskApp     5   	   -   5          � Title$   ProjectManager's Risk Tree��5   	    � 
ResizeMode$ NoResize=�6I    � WindowState$ 	Maximized=�6_   ���	#FFF4E4C0.��5	      � -   5         �Name$
 grid��5	      $��0,-8,-0.4,0.6q�6   .+�5
   
   �� -   5         ��Name$
 Back��5
      $��40��6   $��40��6+   $	��Top=�6P   $
��Left=�6h   $��	23,26,0,0q�6�   6�   � 5   
   -   5        $ canvas��5      $��	0,0,0,200q�6   �� 5   
   -   5         ��Name$ RiskName��5      $��30,180,800,200q�6&   $
��Wrap=�6>   �� 5   
   $��	25,0,0,25q�6   $	��150��6'   $��Bottom=�64   $
��Left=�6O   $	��290��6j   .��5      � $	��145��6   $��0,0,0,0q�6    .+�5      �� $��Probability:��6   $��5,75,185,40q�6/   �� 5      -   5         ��Name$ ProbabilityTextbox��5      $
��Left=�66   $��80,75,25,40q�6Q   $	��150��6f   �� 5      -   5        $ CostTextbox��5      $
��Left=�6/   $��80,40,25,75q�6J   $	��150��6_   �� 5      -   5        $ DescriptionTextbox��5      $
��Left=�66   $��80,5,25,110q�6Q   $	��150��6f   �� 5      $��Cost:��6   $��7,40,200.8,75q�6(   �� 5      $��Description:��6   $��5,5,190,110q�6/   �� 5      -	   5        $ 	AddButton��5      $��Add New Vertex��6,   $��47,108,53.2,7q�6E   ���	#FFE1F37C J� Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      5      5      5      �� 5   
   $	��150��6   $��Bottom=�6!   $��Right=�6<   $	��290��6X   $��25,25q�6d   .��5      � $	��145��6   $��0,0,0,0q�6    .+�5       �� -
   5        $ DeleteButton��5       $��Delete Vertex��6/   $��91,105,9.6,10q�6G   ���	#FFE1F37C 5!        5"      5$      5%      5&      �� 5'   
   -   5        $ ToReportButton��5'      $��Create Report��6)   $	��Top=�6A   $��Right=�6Y   $��	0,25,25,0q�6u   $	��100��6�   $��30��6�    5(        5)      5+      5,      5-      