   M S B A M L   `   `   `  ���� H  DmoVR_Launcher, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:moVR_LaunchermoVR_Launcher      moVR_Launcher.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     Y UFontAwesome5.Net40, Version=2.0.0.0, Culture=neutral, PublicKeyToken=9cfaf01297a008f8/fa5%http://schemas.fontawesome.com/icons/  'localclr-namespace:moVR_Launcher  -     �Name$  LauncherWindow�� � 
ResizeMode$ NoResize=� � Title$ moVR Launcher��$	��496��$	��837��.��� -    �Name$ LauncherScreen�����	#FF343A40 � RowDefinitions �� $
��131* ��� $
��336* �.+��� -    ��Name$ GameDirectory��$
��Left=�$��5,10,0,0q�$	��111��$y�Visible=�$	��456��$	��Top=�$��2��.+��� -    ��Name$ moVR_Game_Button��$	��111��$	��100�����	#FF343A40.����  �� Orientation$ Vertical=�$	��111��$	��100��.+�  FontAwesome5.SvgAwesome     Icon$ Solid_VolleyballBall=�$��60��	   
Foreground	 �	#FFFFFFFF$��0,2,0,0q�$	��Top=��� $
��moVR��$��Center=�$��91��$��44��$��26(�   
CandyBeans#��[  ���	#FFFFFFFF$��10,-2q��� -   $ BaB_Game_Button��$	��111��$	��100�����	#FF343A40

 �� Top$	
 105��.���� $ Vertical=�$	��111��$	��100��.+�  $ Solid_Robot=�$��60��	 �	#FFFFFFFF$��0,2,0,0q�$	��Top=��� $��B.A.B��$��Center=�$��91��$��44��$��26(�#��[  ���	#FFFFFFFF$��10,-2q��� -   $ Concentration_Game_Button��$	��111��$	��100�����	#FF343A40$	
 210��.���� $ Vertical=�$	��111��$	��100��.+�  $ Regular_Heart=�$��60��	 �	#FFFFFFFF$��0,2,0,0q�$	��Top=��� $��Conc.��$��Center=�$��91��$��44��$��26(�#��[  ���	#FFFFFFFF$��10,-2q��� -   $ UF_Game_Button��$	��111��$	��100�����	#FF343A40$	
 315��.���� $ Vertical=�$	��111��$	��100��.+�  $ Solid_Brain=�$��60��	 �	#FFFFFFFF$��0,2,0,0q�$	��Top=��� $��UF��$��Center=�$��91��$��44��$��26(�#��[  ���	#FFFFFFFF$��10,-2q��� -   $ 	GamePanel�����	#FF616467$��2��$	��467��$��Bottom=�$��	121,0,0,0q��� -	    ��Name$ HomeScreenText��$y�Visible=�$7��1← Please select a game 
 ← from the game bar!��$��Center=�$��Center=�$	��437��$	��175��$��30(�#��[  ���	#FFF1F1F1$��0,0,165,161q�$��Bottom=�$��Right=�$��1���� -
    ��Name$ 	GameImage��$"��/images/dodgeball_splash.png��$y�Hidden=�$��Stretch=�$��Stretch=�$��2��$��121,10,0,82q��� -   $ GameVersionNumber��$��Version 0.0.0��$y�Hidden=�$��14(����	#FFC7C7C7$��	0,0,27,61q�$��28��$��Bottom=�$��Right=�$��88��$��1���� -   $ StartButton��$��START��$y�Hidden=����	#FF7ba6d1���	#FF233821
�������$��28(�#��[  $��51��$��Center=�$��Center=�$��Bottom=�$��Right=�$	��121��$��	0,0,10,10q�$��1���� -   $ RepairButton��$��REPAIR��$y�Hidden=�$��16(�#��[  $��23��$��Bottom=����	#FF7ba6d1���	#FF233821
�������$
��Left=�$��67��$��
142,0,0,40q�$��1���� -   $ DownloadStatus��$y�Hidden=�$��Downloading...��$��Center=�$	��236��$��37��$��20(�#��[  ���	#FFF1F1F1$��
0,0,265,33q�$��1��$��Bottom=�$��Right=��� -   $ 
ReadyPanel��$y�Hidden=�$��Ready To Play��$��Center=�$��34��$��20(�#��[  ���	#FFF1F1F1$��
0,0,265,36q�$��1��$��Bottom=�$��Right=�$	��236���� -   $ NoFile��$��No Game File Available��$��20(�$��Center=�#��[  ���	#FFF58282$��
0,0,252,36q�$y�Hidden=�$��1��$��34��$��Bottom=�$��Right=�$	��276����Z� � -    �Name$ GameProgressBar��$y�Hidden=����	#FF7ba6d1$��214,0,136,10q�$��1��$��23��$��Bottom=��� -   $ DownloadPlayButton��$y�Hidden=����	#FF7ba6d1$��Center=�$��
158,0,0,10q�$��1��$
��Left=�$��23��$��23��$��Bottom=�.���� $ 
Horizontal=�$��23��.+�  $ 
Solid_Play=�
	 �����$��20��$��12���� -   $ DownloadStopButton��$y�Hidden=����	#FFba2b23$��Center=� �� Left$ 10��$	
 437��$��
186,0,0,10q�$��1��$
��Left=�$��23��$��23��$��Bottom=�.���� $ 
Horizontal=�$��23��.+�  $ 
Solid_Stop=�
	 �����$��19��$��12��