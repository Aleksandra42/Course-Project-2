   M S B A M L   `   `   `  ���� B  >KursApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:KursAppKursApp      KursApp.Graphic   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:KursApp     -     � Title$  Graphic��$	��700��$
��1200�� � 
ResizeMode$ NoResize=� � WindowStartupLocation$ CenterScreen=�.��� -    �Name$
 grid��.+��� -    ��Name$
 Back��$��40��$��40��$	��Top=�$
��Left=�$��1119,17,0,0q� J� RenderTransformOrigin$ 
0.54,1.047&��� 	 �� X1$ 75��	 �� X2$ 75��	 �� Y1$ 50��		 �� Y2$		 400��
���   ��� $ 75��$	 500��$	 400��$		 400��
���   ��� -   
 ��Name$
 Im��$��	Influence��$��12,38,1107.6,591.4q��� -   $
 Probability��$��Probability��$��430,395,685,230q�� 
���   � � Points' �   �, ���hx��� �� ��� 
���   �' �   ��4�8� e� ����4@�#�� -    ��Name$	 cnv��$��0,0,640,250q��� -    ��Name$ TabRisks��$��600,50,25,90q�.@��� -    ��Name$ RiskData������ $��15(�	Risk Data���� .+��� -	    ��Name$ ComboBox�� �� Text$ Risk Classificated��$	��250��$��30��$
��Left=�$	��Top=��� -
    ��Name$ lvwrisk��$	��0,0q���.  �� View �� .P���  �� Width$	 200��.��Name �� CellTemplate �� .R��� $
��Wrap=����� *
RiskName+�� $	 100�� �� DisplayMemberBinding �� *TypeOfProject+.��Type�� $	 100��.��Source �� .R��� $
��Wrap=����� *SoursOfRisk+�� $	 100��.��Add To Project �� .R��� $	 Add��-   $��Add To Project���� ���� $��15(�Active Risks���� $��0,0,0,0q�.+��� -   $ SelRisks��$	��370�� �� .P��� $	 156��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 100��.��Owner �� .R��� $
��Wrap=����� *
OwnerLogin+�� $ 75��.��	Influence �� .R��� $
��Wrap=����� *	Influence+�� $ 75��.��Probability �� .R��� $
��Wrap=����� *Probability+�� $	 150��.��Send To InActive �� .R��� $ Delete��-   $��Delete��� $	��100��.+��� -    ��Name$ TBProb��$��150,5,300,70q��� -   $

 LInf��$��	Influence��$��0,35,400,35q��� -   $
 LProb��$��Probability��$��
0,5,400,70q��� -   $ Owner��$ Owner��$��300,15,50.2,38q� �� 
IsEditable .  �� 
IsReadOnly . �� -   $ SetUp��$��Set Up��$��5,70,450,10q��� -   $ TBINf��$��150,35,300,40q��� ���� $��15(�Dangerous Risks���� .+��� -   $ DanRisks��$	��400�� �� .P��� $	 200��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 175��.��Source Of Risk �� .R��� $
��Wrap=����� *SoursOfRisk+�� $	 175��.��Rank �� .R��� $
��Wrap=����� *Rank+�� ���� $��15(�	New Risks���� $��0,0,0,0q�.+��� -   $ NewRisks��$	��370�� �� .P��� $	 200��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 150��.��	Influence �� .R��� $
��Wrap=� �� TextAlignment$ Center=����� *	Influence+�� $	 150��.��Probability �� .R��� $
��Wrap=�$ Center=����� *Probability+� $	��100��.+��� -   $ 	TBProbNew��$��150,5,300,70q��� -   $
 LInfNew��$��	Influence��$��0,35,400,35q��� -   $
 LProbNew��$��Probability��$��
0,5,400,70q��� -   $ OwnerNew��$ Owner��$��300,15,50.2,38q� .  . �� -   $ SetUpNew��$��Set Up��$��5,70,450,10q��� -   $ TBINfNew��$��150,35,300,40q��� ���� $��15(�Inactive Risks���� $��0,0,0,0q�.+��� -   $ 
UnSelRisks��$	��370�� �� .P��� $	 150��.��Name �� .R��� $
��Wrap=����� *
RiskName+�� $	 125��.��	Influence �� .R��� $
��Wrap=����� *	Influence+�� $	 125��.��Probability �� .R��� $
��Wrap=����� *Probability+�� $	 175��.��Add to Active �� .R��� $ AddToActive��-   $��Add To Active��