/****** SSMS 中 SelectTopNRows 命令的指令碼  ******/
SELECT TOP (1000) [Id]
      ,[StoreNo]
      ,[EcCode1]
      ,[EcCode2]
      ,[EcLayer]
      ,[Name]
      ,[CompanyName]
      ,[InDate]
      ,[Price]
      ,[State]
      ,[DeliType]
      ,[NewNumber]
      ,[EndThreeYard]
	  ,[ECCabinetBarcode]
      ,[ECCabinetName]
  FROM [ECChk].[dbo].[IFECCUTFs]
  where StoreNo='197308'
And EndThreeYard='858'
--ORDER BY EndThreeYard
  --ORDER BY NewNumber
   --ORDER BY State
  --ORDER BY Name
   --ORDER BY CompanyName
   --ORDER BY  InDate desc,Name


 --ORDER BY State





