   M S B A M L   `   `   `  ���� B  >RiskApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:RiskAppRiskApp  "    RiskApp.AdministratorGraphic   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:RiskApp     -     � Title$  Graphic Window��$	��700��$
��1200�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=����	#FFD7F7B2.��� -    �Name$
 grid��.+��� -    ��Name$ 
BackButton��$��40��$��40��$	��Top=�$
��Left=�$��	12,10,0,0q� J� RenderTransformOrigin$ 
0.54,1.047&� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect  �� 	 �� X1$ 75��	 �� X2$ 75��		 �� Y1$	 50��	
 �� Y2$	
 400��
���   ��� $ 75��$	 500��$		 400��$	
 400��
���   ��� -    ��Name$ InfluenceLabel��$��	Influence��$��12,68,1107.6,561.4q��� -   $ ProbabilityLabel��$��Probability��$��430,395,685,230q�� 
���   � � Points' �   �, ���hx��� �� ��� 
���   �' �   ��4�8� e� ����4@�#�� -    ��Name$ canvas��$��0,0,640,250q��� -    ��Name$ 	RiskTable��$��615,78,9.6,10.4q����	#FF092E06.@��� -    ��Name$
 Data�����	#FF044803����  �� EndPoint$	 0,1&� �� 
StartPoint$	 0,0&�.���  � Color$ 	#FFF0F0F0�� � Offset$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�	Risk Data���� .+��� -	    ��Name$ SeletionCombobox�� �� Text$ Risk Classificated��$	��278��$��30��$��Right=�$	��Top=�$��	0,0,0.2,0q��� -
    ��Name$ listOfRisks��$	��0,0q���.  �� View �� .P���  �� Width$	 200��$
��Name�� �� CellTemplate �� .R��� $
��Wrap=����� *
RiskName+�� $	 100��$
��Type�� �� DisplayMemberBinding �� *Type+�� $	 100��$��Source�� �� .R��� $
��Wrap=����� *Source+�� $	 100��$��Add to Project�� �� .R��� $ 	AddButton��-   $��Add to Project���� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�Active Risks���� $��0,0,0,0q�.+��� -   $ listRisksSelected��$	��370�� �� .P��� $	 156��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 100��.��Owner �� .R��� $
��Wrap=����� *
OwnerLogin+�� $ 75��.��	Influence �� .R��� $
��Wrap=����� *	Influence+�� $ 75��.��Probability �� .R��� $
��Wrap=����� *Probability+�� $	 150��.��Send To Non-Active �� .R��� $ DeleteButton��-   $��Delete��� $	��100��$��	0,0,0.2,0q� � ColumnDefinitions �� $
��296* ��� $
��267* �.+��� -    ��Name$ ProbabilityTextbox��$��150,5,31.8,70q��� -   $ InfluenceLab��$��	Influence��$��0,35,132.8,35q��� -   $ ProbabilityLab��$��Probability��$��0,5,132.8,70q��� -   $ Owner��$ Owner��$��150,65,200,-12q� �� 
IsEditable .  �� 
IsReadOnly . $��2���� -   $ SetUpRiskButton��$��Set Up Risk��$��98.2,92,10,-12q�$��1��   �� -   $ InfluenceTextbox��$��150,35,31.8,40q��� $��Choose the Owner:��$
��Left=�$��0,76,0,0q�$	��Top=�$	��132���� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�Dangerous Risks���� .+��� -   $ listDangerous��$	��400��$��	0,0,0.2,0q� �� .P��� $	 150��$��	Risk Name�� �� .R��� $
��Wrap=����� *
RiskName+�� $	 150��$��Risk Source�� �� .R��� $
��Wrap=����� *Source+�� $	 150��$��Rank of the Risk�� �� .R��� $
��Wrap=����� *Rank+�� $	 175��$��Possible Solution�� �� .R��� $
��Wrap=����� *
Solution+�� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�	New Risks���� $��0,0,0,0q�.+��� -   $ listNewRisks��$	��370�� �� .P��� $	 200��.��	Risk Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 150��.��	Influence �� .R��� $
��Wrap=� �� TextAlignment$ Center=����� *	Influence+�� $	 150��.��Probability �� .R��� $
��Wrap=�$ Center=����� *Probability+� $	��100��.+��� -   $ NewProbabilityTextbox��$��150,5,300,70q��� -   $ LabelInfluenceNew��$��	Influence��$��0,35,400,35q��� -   $ LabelProbabilityNew��$��Probability��$��
0,5,400,70q��� -   $ NewOwnerCombobox��$ Owner��$��300,15,50.2,38q� .  . ���	#FF080505�� -   $ SetUpNewRiskButton��$��Set Up��$��5,70,450,10q�   �� -   $ NewInfluenceTextbox��$��150,35,300,40q��� ���	#FF044803���� $	 0,1&�$	 0,0&�.��� $ 	#FFF0F0F0��$ 0Q�� $ 	#FF50E50C��$ 1Q����� $��15(�Inactive Risks���� $��0,0,0,0q�.+��� -   $ listRisksNonselected��$	��370�� �� .P��� $	 150��.��Name of Risk �� .R��� $
��Wrap=����� *
RiskName+�� $	 125��.��	Influence �� .R��� $
��Wrap=����� *	Influence+�� $	 125��.��Probability �� .R��� $
��Wrap=����� *Probability+�� $	 175��.��Send to Active �� .R��� $ AddToActiveRiskButton��-   $��Add To Active���� -   $ ZoomOUTButton��$��Zoom out��$��
10,198,0,0q�$ -7.884,0.204&�$
��Left=�$��61��$��26��$	��Top=��� -   $ ZoomINButton��$��Zoom in��$��
10,170,0,0q�$ 0.5,0.5&�$
��Left=�$��61��$��23��$	��Top=�