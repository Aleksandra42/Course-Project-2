   M S B A M L   `   `   `  ���� B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp  #    RiskApp.ProjectManagerGraphic   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:RiskApp     -     � Title$  Graphic Window��$	��700��$
��1200�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=����	#FFD7F7B2.��� -    �Name$
 grid��.+��� -    ��Name$ 
BackButton��$��40��$��40��$	��Top=�$
��Left=�$��	10,10,0,0q� J� RenderTransformOrigin$ 
0.54,1.047&� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  �� 	 �� X1$ 75��	 �� X2$ 75��		 �� Y1$	 50��	
 �� Y2$	
 400��
���   ��� $ 75��$	 500��$		 400��$	
 400��
���   ��� -    ��Name$ InfluenceLabel��$��	Influence��$��10,55,1109.6,575.4q��� -   $ ProbabilityLabel��$��Probability��$��430,395,685,230q�� 
���   � � Points' �   �, ���hx��� �� ��� 
���   �' �   ��4�8� e� ����4@�#�� -    ��Name$ canvas��$��0,0,640,250q��� -    ��Name$ 	RiskTable��$��600,50,25,90q�.@��� -    ��Name$ RiskData�����	#FF044803����  �� EndPoint$	 0,1&� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFF0F0F0�� � Offset$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�	Risk Data���� .+��� -	    ��Name$ ComboBoxTypes�� �� Text$ Risk Classificated��$	��232��$��30��$
��Left=�$	��Top=��� -
    ��Name$ 	listRisks��$	��0,0q���.  �� View �� .P���  �� Width$	 200��$
��Name�� �� CellTemplate �� .R��� $
��Wrap=����� *
RiskName+�� $	 100��$
��Type�� �� DisplayMemberBinding �� *Type+�� $	 100��$��Source�� �� .R��� $
��Wrap=����� *Source+�� $	 100��$��Add To Project�� �� .R��� $ 	AddButton��-   $��Add To Project���� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�Active Risks���� $��0,0,0,0q�.+��� -   $ listSelected��$	��370�� �� .P��� $	 156��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 100��.��Owner �� .R��� $
��Wrap=����� *
OwnerLogin+�� $ 75��.��	Influence �� .R��� $
��Wrap=����� *	Influence+�� $ 75��.��Probability �� .R��� $
��Wrap=����� *Probability+�� $	 150��.��Send To InActive �� .R��� $ DeleteButton��-   $��Delete��� $	��100��.+��� -    ��Name$ ProbabilityTextbox��$��150,5,300,70q��� -   $
 LInf��$��	Influence��$��0,35,400,35q��� -   $ LProb��$��Probability��$��
0,5,400,70q��� -   $ OwnerCombobox��$ Owner��$��300,15,50.2,38q� �� 
IsEditable .  �� 
IsReadOnly . �� -   $ SetUp��$��Set Up��$��5,70,450,10q�   �� -   $ InfluenceTextbox��$��150,35,300,40q��� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�Dangerous Risks���� .+��� -   $ listDangerous��$	��400�� �� .P��� $	 175��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 175��.��Source Of Risk �� .R��� $
��Wrap=����� *Source+�� $	 175��.��Rank �� .R��� $
��Wrap=����� *Rank+�� $	 175��.��Possible Solution �� .R��� $
��Wrap=����� *
Solution+�� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�	New Risks���� $��0,0,0,0q�.+��� -   $ listNewRisks��$	��370�� �� .P��� $	 200��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 150��.��	Influence �� .R��� $
��Wrap=� �� TextAlignment$ Center=����� *	Influence+�� $	 150��.��Probability �� .R��� $
��Wrap=�$ Center=����� *Probability+� $	��100��.+��� -   $ NewProbabilityTextbox��$��150,5,300,70q��� -   $ LInfNew��$��	Influence��$��0,35,400,35q��� -   $ LProbNew��$��Probability��$��
0,5,400,70q��� -   $ OwnerNewCombobox��$ Owner��$��300,15,50.2,38q� .  . �� -   $ SetUpNewButton��$��Set Up��$��5,70,450,10q�   �� -   $ NewInfluenceTextbox��$��150,35,300,40q��� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�Inactive Risks���� $��0,0,0,0q�.+��� -   $ listUnSelected��$	��370�� �� .P��� $	 150��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 125��.��	Influence �� .R��� $
��Wrap=����� *	Influence+�� $	 125��.��Probability �� .R��� $
��Wrap=����� *Probability+�� $	 175��.��Add to Active �� .R��� $ AddToActive��-   $��Add To Active��