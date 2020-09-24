SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([Id], [Name], [Login], [Password], [Position]) VALUES (1, N'Mr Tester', N'Test', N'qwerty', N'RiskManager')
INSERT INTO [dbo].[Users] ([Id], [Name], [Login], [Password], [Position]) VALUES (2, N'Aleksandra Karpova', N'Alex', N'1701', N'MainManager')
INSERT INTO [dbo].[Users] ([Id], [Name], [Login], [Password], [Position]) VALUES (3, N'Maria ', N'Mary', N'1234', N'ProjectManager')
SET IDENTITY_INSERT [dbo].[Users] OFF
