   M S B A M L   `   `   `  ����B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp  5   	        RiskApp.RiskSettingsWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:RiskApp     5   	   -   5          � Title$  Risk Settings Window��5   	   $��196.733��6&   $��440.458��67    � 
ResizeMode$ NoResize=�6d   .��5	      � $��-5,0,3,-0.6q�6    � RowDefinitions 5
   
   �� 5      $	��58* �6   �� 5      $
��107* �6   .+�5   
   �� -   5         ��Name$ ProbabilityTextbox��5      $��0,17,193.4,0q�6.   $��30��6D   $	��Top=�6P   $��Right=�6h   $	��123��6�   �� 5   
   -   5         ��Name$ LabelInfluence��5      $��	Influence��6(   $��15,1.688,0,0q�6<   $��30��6R   $	��Top=�6^   $
��Left=�6v   $	��114��6�   $��1��6�   �� 5   
   -   5        $ LabelProbability��5      $��Probability��6*   $��	15,15,0,0q�6@   $��30��6S   $	��Top=�6_   $
��Left=�6w   $	��114��6�   �� 5   
   -   5         ��Name$ UsersCombobox��5       �� Text$ Choose Owner��6*   $��0,50.4,10,11.2q�6>    �� 
IsEditable .  �� 
IsReadOnly . $��1��6z   $��Right=�6�   $	��268��6�   �� 5   
   -   5        	 ��Name$	 SetRiskButton��5      $��Create Estimated Risk��6(   $��0,0,10,12.6q�6g   $��30��6|   $��Bottom=�6�   $��Right=�6�   $	��167��6�   �� 5   
   -   5        $	 CleanRiskButton��5      $��Create Clean Risk��6+   $��0,0,10,74.2q�6_   $��30��6t   $��Bottom=�6�   $��Right=�6�   $	��167��6�   $��1��6�   �� 5   
   -   5        $ InfluenceTextbox��5      $��0,4.8,193.4,0q�6,   $��30��6C   $	��Top=�6O   $��Right=�6g   $	��122��6�   $��1��6�   �� 5   
   $��Choose Risk's Owner��6   $
��Left=�6.   $��15,50.688,0,0q�6I   $	��Top=�6`   $��1��6x   5      5      