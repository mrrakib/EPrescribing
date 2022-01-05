GO
SET IDENTITY_INSERT [dbo].[Brands] ON 

INSERT [dbo].[Brands] ([Id], [BrandName], [Location], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Renata Limited', N'Dhaka', CAST(N'2021-09-25T18:48:00.000' AS DateTime), CAST(N'2021-09-25T18:49:40.250' AS DateTime), 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Location], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'Square Pharmaceuticals Ltd', N'Dhaka', CAST(N'2021-09-25T18:48:00.000' AS DateTime), CAST(N'2021-09-25T18:50:00.287' AS DateTime), 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Location], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (3, N'Eskayef Pharmaceuticals Ltd', N'Dhaka', CAST(N'2021-09-25T18:49:23.417' AS DateTime), NULL, 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Location], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (4, N'Beximco Pharmaceuticals Ltd', N'Dhaka', CAST(N'2021-09-25T18:53:16.760' AS DateTime), NULL, 1)
INSERT [dbo].[Brands] ([Id], [BrandName], [Location], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (5, N'ACI Limited', N'Dhaka', CAST(N'2021-09-25T21:48:59.197' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Brands] OFF
GO
SET IDENTITY_INSERT [dbo].[Generics] ON 

INSERT [dbo].[Generics] ([Id], [GenericName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Omeprazole', CAST(N'2021-09-25T18:50:29.130' AS DateTime), NULL, 1)
INSERT [dbo].[Generics] ([Id], [GenericName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'Paracetamol', CAST(N'2021-09-25T18:50:42.590' AS DateTime), NULL, 1)
INSERT [dbo].[Generics] ([Id], [GenericName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (3, N'Etoricoxib', CAST(N'2021-09-25T18:51:38.857' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Generics] OFF
GO
SET IDENTITY_INSERT [dbo].[Medicines] ON 

INSERT [dbo].[Medicines] ([Id], [Name], [GenericId], [BrandId], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Tory-90mg', 3, 2, CAST(N'2021-09-25T18:52:40.077' AS DateTime), CAST(N'2021-09-25T21:53:03.587' AS DateTime), 0)
INSERT [dbo].[Medicines] ([Id], [Name], [GenericId], [BrandId], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'Napa-500mg', 2, 4, CAST(N'2021-09-25T18:53:29.723' AS DateTime), NULL, 1)
INSERT [dbo].[Medicines] ([Id], [Name], [GenericId], [BrandId], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (3, N'Ace Plus -500mg', 2, 2, CAST(N'2021-09-25T18:53:54.140' AS DateTime), NULL, 1)
INSERT [dbo].[Medicines] ([Id], [Name], [GenericId], [BrandId], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (4, N'Losectil-20mg', 1, 3, CAST(N'2021-09-25T18:54:28.153' AS DateTime), NULL, 1)
INSERT [dbo].[Medicines] ([Id], [Name], [GenericId], [BrandId], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (5, N'MAX Pro-20mg', 1, 5, CAST(N'2021-09-25T21:49:00.000' AS DateTime), CAST(N'2021-09-25T21:51:13.263' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Medicines] OFF
GO
SET IDENTITY_INSERT [dbo].[Advices] ON 

INSERT [dbo].[Advices] ([Id], [AdviceName], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Regular Morning Exercise', CAST(N'2021-09-25T18:48:28.247' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Advices] OFF
GO
SET IDENTITY_INSERT [dbo].[DentalSchools] ON 

INSERT [dbo].[DentalSchools] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Bangladesh Dental College', N'BDS101', CAST(N'2021-09-25T18:45:00.000' AS DateTime), CAST(N'2021-09-25T18:45:49.297' AS DateTime), 1)
INSERT [dbo].[DentalSchools] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'Dhaka Dental College', N'D102', CAST(N'2021-09-25T18:45:27.383' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[DentalSchools] OFF
GO
SET IDENTITY_INSERT [dbo].[Designations] ON 

INSERT [dbo].[Designations] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive], [IsSubscriptionFree]) VALUES (1, N'BDS STUDENT', N'D101', CAST(N'2021-09-25T18:37:00.000' AS DateTime), CAST(N'2021-09-25T18:37:41.430' AS DateTime), 1, 1)
INSERT [dbo].[Designations] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive], [IsSubscriptionFree]) VALUES (2, N'INTERN DOCTOR', N'D102', CAST(N'2021-09-25T18:37:31.967' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Designations] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive], [IsSubscriptionFree]) VALUES (3, N'DENTALL SPECIALIST', N'D1011', CAST(N'2021-09-25T18:37:00.000' AS DateTime), CAST(N'2021-09-25T18:43:39.403' AS DateTime), 1, 0)
INSERT [dbo].[Designations] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive], [IsSubscriptionFree]) VALUES (5, N'BDS DOCTOR', N'S101', CAST(N'2021-09-25T18:39:00.000' AS DateTime), CAST(N'2021-09-25T18:40:06.853' AS DateTime), 1, 0)
INSERT [dbo].[Designations] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive], [IsSubscriptionFree]) VALUES (6, N'BDS DENTIST', N'D1023', CAST(N'2021-09-25T18:42:00.000' AS DateTime), CAST(N'2021-09-25T20:24:02.953' AS DateTime), 1, 1)
INSERT [dbo].[Designations] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive], [IsSubscriptionFree]) VALUES (7, N'POST-GRADUATE TRAINEE', N'D101', CAST(N'2021-09-25T18:43:19.363' AS DateTime), NULL, 1, 0)
INSERT [dbo].[Designations] ([Id], [Name], [Code], [CreatedDate], [UpdatedDate], [IsActive], [IsSubscriptionFree]) VALUES (8, N'Test Designation', N'D109', CAST(N'2021-09-25T21:55:26.680' AS DateTime), NULL, 1, 0)
SET IDENTITY_INSERT [dbo].[Designations] OFF
GO
SET IDENTITY_INSERT [dbo].[Diseases] ON 

INSERT [dbo].[Diseases] ([Id], [Name], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Toothache', CAST(N'2021-09-25T18:55:11.043' AS DateTime), NULL, 1)
INSERT [dbo].[Diseases] ([Id], [Name], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'Stained Teeth', CAST(N'2021-09-25T18:55:22.383' AS DateTime), NULL, 1)
INSERT [dbo].[Diseases] ([Id], [Name], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (3, N'Cavities', CAST(N'2021-09-25T18:55:30.577' AS DateTime), NULL, 1)
INSERT [dbo].[Diseases] ([Id], [Name], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (4, N'Sensitive Tooth', CAST(N'2021-09-25T18:56:18.760' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Diseases] OFF
GO
SET IDENTITY_INSERT [dbo].[Pathologies] ON 

INSERT [dbo].[Pathologies] ([Id], [Name], [Description], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'CT scan', N'A computerized tomography (CT) scan', CAST(N'2021-09-25T18:58:00.000' AS DateTime), CAST(N'2021-09-25T20:35:13.003' AS DateTime), 1)
INSERT [dbo].[Pathologies] ([Id], [Name], [Description], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'X-ray', N'X-rays are a type of radiation called electromagnetic waves', CAST(N'2021-09-25T19:00:00.000' AS DateTime), CAST(N'2021-09-25T19:01:32.543' AS DateTime), 1)
INSERT [dbo].[Pathologies] ([Id], [Name], [Description], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (3, N'MRI scan', N'Magnetic resonance imaging (MRI)', CAST(N'2021-09-25T19:00:00.000' AS DateTime), CAST(N'2021-09-25T19:00:52.877' AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Pathologies] OFF
GO
SET IDENTITY_INSERT [dbo].[Subscriptions] ON 

INSERT [dbo].[Subscriptions] ([Id], [Name], [Description], [Cost], [CreatedDate], [UpdatedDate], [IsActive], [EvaluationPeriodInDay]) VALUES (1, N'FREE', N'BDS STUDENT/INTERN DOCTOR', 0, CAST(N'2021-09-25T18:33:09.267' AS DateTime), NULL, 1, 0)
INSERT [dbo].[Subscriptions] ([Id], [Name], [Description], [Cost], [CreatedDate], [UpdatedDate], [IsActive], [EvaluationPeriodInDay]) VALUES (3, N'Monthly', N'Monthly Subscription @ Tk 199', 199, CAST(N'2021-09-25T18:34:12.110' AS DateTime), NULL, 1, 30)
INSERT [dbo].[Subscriptions] ([Id], [Name], [Description], [Cost], [CreatedDate], [UpdatedDate], [IsActive], [EvaluationPeriodInDay]) VALUES (4, N'6-Month', N'6-month Subscription @ Tk 180/month', 1080, CAST(N'2021-09-25T18:35:00.000' AS DateTime), NULL, 1, 180)
INSERT [dbo].[Subscriptions] ([Id], [Name], [Description], [Cost], [CreatedDate], [UpdatedDate], [IsActive], [EvaluationPeriodInDay]) VALUES (5, N'1-Year', N'1 year Subscription @ Tk 165/month', 1980, CAST(N'2021-09-25T18:35:00.000' AS DateTime), NULL, 1, 365)
SET IDENTITY_INSERT [dbo].[Subscriptions] OFF
GO
SET IDENTITY_INSERT [dbo].[Treatments] ON 

INSERT [dbo].[Treatments] ([Id], [Name], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (1, N'Fillings', CAST(N'2021-09-25T19:02:18.307' AS DateTime), NULL, 1)
INSERT [dbo].[Treatments] ([Id], [Name], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (2, N'Tooth extractions', CAST(N'2021-09-25T19:02:40.570' AS DateTime), NULL, 1)
INSERT [dbo].[Treatments] ([Id], [Name], [CreatedDate], [UpdatedDate], [IsActive]) VALUES (3, N'Dental implants', CAST(N'2021-09-25T19:02:51.417' AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[Treatments] OFF
GO