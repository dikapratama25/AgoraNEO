Imports System.Configuration
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraNEO.AgoraNEO

Namespace AgoraNEO
    Public Class ContCat

        Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
        Public dtProduct As New DataTable
        'Dim objGlobal As New AppGlobals

        Public Sub Init_bindProduct()
            Dim ds As New DataSet
            dtProduct.Columns.Add("CoyId", Type.GetType("System.String"))   'PM_S_COY_ID
            dtProduct.Columns.Add("ProductCode", Type.GetType("System.String")) 'PM_PRODUCT_CODE
            dtProduct.Columns.Add("VendorItemCode", Type.GetType("System.String"))  'PM_VENDOR_ITEM_CODE
            dtProduct.Columns.Add("ItemName", Type.GetType("System.String"))    'PM_PRODUCT_DESC ' dtProduct.Columns.Add("ProductDesc", Type.GetType("System.String"))
            dtProduct.Columns.Add("ReferenceNo", Type.GetType("System.String")) 'PM_REF_NO
            dtProduct.Columns.Add("Description", Type.GetType("System.String")) 'PM_LONG_DESC
            dtProduct.Columns.Add("CommodityType", Type.GetType("System.String")) 'PM_CATEGORY_NAME 'dtProduct.Columns.Add("CategoryName", Type.GetType("System.String"))
            dtProduct.Columns.Add("AccCode", Type.GetType("System.String")) 'PM_ACCT_CODE
            dtProduct.Columns.Add("UOM", Type.GetType("System.String")) 'PM_UOM
            dtProduct.Columns.Add("CatCode", Type.GetType("System.String"))
            dtProduct.Columns.Add("MinInv", Type.GetType("System.String")) 'PM_SAFE_QTY
            dtProduct.Columns.Add("Min", Type.GetType("System.String")) 'PM_ORD_MIN_QTY
            dtProduct.Columns.Add("Max", Type.GetType("System.String")) 'PM_ORD_MAX_QTY
            dtProduct.Columns.Add("Brand", Type.GetType("System.String")) 'PM_PRODUCT_BRAND
            dtProduct.Columns.Add("Model", Type.GetType("System.String")) 'PM_PRODUCT_MODEL

            dtProduct.Columns.Add("DrawingNo", Type.GetType("System.String")) 'PM_DRAW_NO
            dtProduct.Columns.Add("VersionNo", Type.GetType("System.String")) 'PM_VERS_NO
            dtProduct.Columns.Add("GrossWeight", Type.GetType("System.String")) 'PM_GROSS_WEIGHT
            dtProduct.Columns.Add("NetWeight", Type.GetType("System.String")) 'PM_NET_WEIGHT
            dtProduct.Columns.Add("Length", Type.GetType("System.String")) 'PM_LENGHT
            dtProduct.Columns.Add("Width", Type.GetType("System.String")) 'PM_WIDTH
            dtProduct.Columns.Add("Height", Type.GetType("System.String")) 'PM_HEIGHT
            dtProduct.Columns.Add("Volume", Type.GetType("System.String")) 'PM_VOLUME
            dtProduct.Columns.Add("ColorInfo", Type.GetType("System.String")) 'PM_COLOR_INFO
            dtProduct.Columns.Add("HSCode", Type.GetType("System.String")) 'PM_HSC_CODE
            dtProduct.Columns.Add("Packing", Type.GetType("System.String")) 'PM_PACKING_REQ
            dtProduct.Columns.Add("Remark", Type.GetType("System.String")) 'PM_REMARKS
            dtProduct.Columns.Add("Deleted", Type.GetType("System.String")) 'PM_DELETED
            dtProduct.Columns.Add("Prefer", Type.GetType("System.String"))
            dtProduct.Columns.Add("1st", Type.GetType("System.String"))
            dtProduct.Columns.Add("2nd", Type.GetType("System.String"))
            dtProduct.Columns.Add("3rd", Type.GetType("System.String"))
            dtProduct.Columns.Add("PreferTax", Type.GetType("System.String"))
            dtProduct.Columns.Add("1stTax", Type.GetType("System.String"))
            dtProduct.Columns.Add("2ndTax", Type.GetType("System.String"))
            dtProduct.Columns.Add("3rdTax", Type.GetType("System.String"))

            dtProduct.Columns.Add("MaxInvQty", Type.GetType("System.String"))
            dtProduct.Columns.Add("Manu", Type.GetType("System.String"))

            dtProduct.Columns.Add("rd1", Type.GetType("System.String"))
            dtProduct.Columns.Add("rd2", Type.GetType("System.String"))

            dtProduct.Columns.Add("LeadP", Type.GetType("System.String"))
            dtProduct.Columns.Add("Lead1", Type.GetType("System.String"))
            dtProduct.Columns.Add("Lead2", Type.GetType("System.String"))
            dtProduct.Columns.Add("Lead3", Type.GetType("System.String"))

            dtProduct.Columns.Add("VenCodeP", Type.GetType("System.String"))
            dtProduct.Columns.Add("VenCode1", Type.GetType("System.String"))
            dtProduct.Columns.Add("VenCode2", Type.GetType("System.String"))
            dtProduct.Columns.Add("VenCode3", Type.GetType("System.String"))
        End Sub

        'Public Function getContractDetail(ByVal index As String) As DataSet
        '    Dim sql As String
        '    sql = "SELECT cdm_group_code,cdm_group_desc,cdm_start_date,cdm_end_date, " &
        '    "cdc_b_coy_id FROM contract_dist_mstr " &
        '    "LEFT JOIN contract_dist_coy ON cdm_group_index = cdc_group_index " &
        '    "WHERE cdm_group_index = '" & index & "'"
        '    Dim ds As New DataSet
        '    ds = objDb.FillDs(sql)
        '    Return ds
        'End Function
        'Public Function getContractItemList(ByVal index As String) As DataSet
        '    Dim sql As String
        '    sql = "SELECT CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_CURRENCY_CODE, IFNULL(CDI_GST,0) AS CDI_GST, IFNULL(CDI_GST_TAX_CODE,'N/A') AS CDI_GST_TAX_CODE, CDI_UNIT_COST, CDI_UOM, CDI_REMARK, CDM_GROUP_INDEX, CDI_PRODUCT_CODE, " &
        '    "CASE WHEN CDI_GST_RATE = 'N/A' THEN CDI_GST_RATE ELSE " &
        '    "IFNULL(IF((TAX_PERC IS NULL OR TAX_PERC = ''), CODE_DESC, CONCAT(CODE_DESC, '(', TAX_PERC,'%)')),'N/A') END AS CDI_GST_RATE " &
        '    "FROM CONTRACT_DIST_MSTR " &
        '    "RIGHT JOIN CONTRACT_DIST_ITEMS ON CDM_GROUP_INDEX = CDI_GROUP_INDEX " &
        '    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " &
        '    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CDI_GST_RATE " &
        '    "LEFT JOIN TAX ON TAX_CODE = CDI_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " &
        '    "WHERE CDM_GROUP_INDEX = '" & index & "'"
        '    Dim ds As New DataSet
        '    ds = objDb.FillDs(sql)
        '    Return ds
        'End Function
        'Public Function getContractCatalogueList(ByVal strstatus As String, ByVal strCode As String, ByVal strDesc As String, ByVal strBuyer As String, ByVal strType As String, ByVal strDateFr As String, ByVal strDateTo As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CDUM_Upload_Index, CDUM_Contract_Code, CDUM_Contract_Desc, CDUM_S_Coy_Id, CDUM_Start_Date, "
        '    strsql &= "CDUM_End_Date, CDUM_Type, CDUM_Upload_Status, CDUM_B_Reject_Cnt, CDUM_H_Reject_Cnt, "
        '    strsql &= "CM_COY_NAME , CDUD_B_Coy_Id, STATUS_DESC, CDUM_Submit_Date "
        '    strsql &= "FROM CONT_DIST_UPLOADMSTR "
        '    strsql &= "LEFT JOIN CONT_DIST_UPLOADCOY ON CDUM_Upload_Index = CDUD_Upload_Index "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON CDUD_B_Coy_Id = CM_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '    strsql &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = CDUM_Upload_Status AND STATUS_TYPE = 'CONTRACT' "
        '    strsql &= "WHERE CDUM_Type = 'C' "

        '    If strType = "H" Then
        '        strsql &= "AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyIdToken") & "' "
        '    Else
        '        strsql &= "AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    End If

        '    Select Case strType
        '        Case "D"
        '            If strstatus <> "" Then
        '                strsql &= "AND CDUM_Upload_Status IN (" & strstatus & ") "
        '            Else
        '                strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.Approved & "' "
        '            End If

        '        Case "A"
        '            strsql &= "AND CDUM_Upload_Status = '" & strstatus & "' "

        '        Case "H"
        '            If strstatus <> "" Then
        '                strsql &= "AND CDUM_Upload_Status IN (" & strstatus & ") "
        '            Else
        '                strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.Draft & "' "
        '                strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.Approved & "' "
        '                strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.BuyerRejected & "' "
        '                strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.BuyerPending & "' "
        '            End If
        '    End Select

        '    If strDateFr <> "" Then
        '        strsql &= "AND CDUM_Start_Date >= " & Common.ConvertDate(strDateFr) & " "
        '    End If

        '    If strDateTo <> "" Then
        '        strsql &= "AND CDUM_End_Date <= " & Common.ConvertDate(strDateTo & " 23:59:59.000") & " "
        '    End If

        '    If strCode <> "" Then
        '        strsql &= " AND CDUM_Contract_Code " & Common.ParseSQL(strCode) & " "
        '    End If

        '    If strDesc <> "" Then
        '        strsql &= " AND CDUM_Contract_Desc " & Common.ParseSQL(strDesc) & " "
        '    End If

        '    If strBuyer <> "" Then
        '        strsql &= " AND CM_COY_NAME " & Common.ParseSQL(strBuyer) & " "
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    getContractCatalogueList = ds
        'End Function

        'Public Function getBuyerContractCatalogueList(ByVal strstatus As String, ByVal strCode As String, ByVal strDesc As String, ByVal strVendor As String, ByVal strDateFr As String, ByVal strDateTo As String) As DataSet
        '    Dim strsql, strsql1, strsql2 As String
        '    Dim ds As New DataSet
        '    ' TODO : Approved Contract is retrieved from CONTRACT_DIST_MSTR
        '    ' ai chu modified on 10/11/2005
        '    ' Approved Contract is retrieved from CONTRACT_DIST_MSTR

        '    strsql1 = "SELECT CDUM_Upload_Index, CDUM_Contract_Code, CDUM_Contract_Desc, CDUM_S_Coy_Id, CDUM_Start_Date, "
        '    strsql1 &= "CDUM_End_Date, CDUM_Type, CDUM_Upload_Status, CDUM_B_Reject_Cnt, CDUM_H_Reject_Cnt, "
        '    strsql1 &= "CM_COY_NAME , CDUD_B_Coy_Id, STATUS_DESC, CDUM_Submit_Date "
        '    strsql1 &= "FROM CONT_DIST_UPLOADMSTR "
        '    strsql1 &= "LEFT JOIN CONT_DIST_UPLOADCOY ON CDUM_Upload_Index = CDUD_Upload_Index "
        '    strsql1 &= "LEFT JOIN COMPANY_MSTR ON CDUM_S_Coy_Id = CM_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '    strsql1 &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = CDUM_Upload_Status AND STATUS_TYPE = 'CONTRACT' "
        '    strsql1 &= "WHERE CDUM_Type = 'C' "
        '    strsql1 &= "AND CDUD_B_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "

        '    strsql2 = "SELECT CDM_GROUP_INDEX AS CDUM_Upload_Index, CDM_GROUP_CODE AS CDUM_Contract_Code, "
        '    strsql2 &= "CDM_GROUP_DESC AS CDUM_Contract_Desc, CDM_S_COY_ID AS CDUM_S_Coy_Id, "
        '    strsql2 &= "CDM_START_DATE AS CDUM_Start_Date, CDM_END_DATE AS CDUM_End_Date, CDM_TYPE AS CDUM_Type, "
        '    strsql2 &= "'6' AS CDUM_Upload_Status, 0 AS CDUM_B_Reject_Cnt, 0 AS CDUM_H_Reject_Cnt, CM_COY_NAME, "
        '    strsql2 &= "CDC_B_COY_ID, STATUS_DESC, CDM_ENT_DT AS CDUM_Submit_Date "
        '    strsql2 &= "FROM STATUS_MSTR, CONTRACT_DIST_MSTR "
        '    strsql2 &= "LEFT JOIN CONTRACT_DIST_COY ON  CDM_GROUP_INDEX = CDC_GROUP_INDEX "
        '    strsql2 &= "LEFT JOIN COMPANY_MSTR ON CDM_S_COY_ID = CM_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '    strsql2 &= "WHERE STATUS_TYPE = 'CONTRACT' AND STATUS_NO = 6 AND CDM_TYPE = 'C' "
        '    strsql2 &= "AND CDC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

        '    If strstatus = "" Then
        '        strsql = strsql1 & buildSearchCriteria(1, strstatus, strCode, strDesc, strVendor, strDateFr, strDateTo)
        '        strsql &= " UNION " & strsql2 & buildSearchCriteria(0, strstatus, strCode, strDesc, strVendor, strDateFr, strDateTo)
        '    Else
        '        If strstatus = "6" Then
        '            strsql = strsql2 & buildSearchCriteria(0, strstatus, strCode, strDesc, strVendor, strDateFr, strDateTo)
        '        ElseIf strstatus.IndexOf("6") > -1 Then
        '            strsql = strsql1 & buildSearchCriteria(1, strstatus, strCode, strDesc, strVendor, strDateFr, strDateTo)
        '            strsql &= " UNION " & strsql2 & buildSearchCriteria(0, strstatus, strCode, strDesc, strVendor, strDateFr, strDateTo)
        '        Else
        '            strsql = strsql1 & buildSearchCriteria(1, strstatus, strCode, strDesc, strVendor, strDateFr, strDateTo)
        '        End If
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    getBuyerContractCatalogueList = ds
        'End Function

        'Public Function buildSearchCriteria(ByVal intType As Integer, ByVal strstatus As String, ByVal strCode As String, ByVal strDesc As String, ByVal strVendor As String, ByVal strDateFr As String, ByVal strDateTo As String) As String
        '    Dim strsql As String = ""
        '    ' intType = 1 - retrieved from CONT_DIST_UPLOADMSTR; 
        '    ' intType = 0 - retrieved from CONTRACT_DIST_MSTR; 

        '    If intType = 1 Then
        '        If strstatus <> "" Then
        '            If strstatus.IndexOf("6") > -1 Then
        '                strstatus = strstatus.Substring(0, strstatus.IndexOf("6") - 1)
        '            End If
        '            strsql &= "AND CDUM_Upload_Status IN (" & strstatus & ") "
        '        Else
        '            strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.Draft & "' "
        '            strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.Approved & "' "
        '            ' ai chu modified on 04/10/2005
        '            ' SR raised by User1 - to allow PM to view rejected/approved contract
        '            'strsql &= "AND CDUM_Upload_Status <> '" & CatalogueStatus.Rejected & "' "
        '        End If
        '    End If

        '    If strDateFr <> "" Then
        '        strsql &= "AND " & IIf(intType = 1, "CDUM_Start_Date", "CDM_START_DATE") & " >= " & Common.ConvertDate(strDateFr) & " "
        '    End If

        '    If strDateTo <> "" Then
        '        strsql &= "AND " & IIf(intType = 1, "CDUM_End_Date", "CDM_END_DATE") & " <= " & Common.ConvertDate(strDateTo & " 23:59:59.000") & " "
        '    End If

        '    If strCode <> "" Then
        '        strsql &= " AND " & IIf(intType = 1, "CDUM_Contract_Code", "CDM_GROUP_CODE") & " " & Common.ParseSQL(strCode) & " "
        '    End If

        '    If strDesc <> "" Then
        '        strsql &= " AND " & IIf(intType = 1, "CDUM_Contract_Desc", "CDM_GROUP_DESC") & " " & Common.ParseSQL(strDesc) & " "
        '    End If

        '    If strVendor <> "" Then
        '        strsql &= " AND CM_COY_NAME " & Common.ParseSQL(strVendor) & " "
        '    End If
        '    Return strsql
        'End Function

        'Public Function getCatalogueList(ByVal strVendor As String, ByVal strType As String, ByVal strCatType As String, ByVal strCode As String, ByVal strDesc As String, ByVal strBuyer As String, ByVal strDateFr As String, ByVal strDateTo As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    Select Case strType
        '        Case "A"
        '            strsql = "SELECT CDM_GROUP_INDEX AS CDUM_Upload_Index, CDM_GROUP_CODE AS CDUM_Contract_Code, "
        '            strsql &= "CDM_GROUP_DESC AS CDUM_Contract_Desc, CDM_S_COY_ID AS CDUM_S_Coy_Id, CDM_TYPE AS CDUM_Type, "
        '            strsql &= "CDM_START_DATE AS CDUM_Start_Date, CDM_END_DATE AS CDUM_End_Date, "
        '            strsql &= "CM_COY_NAME , '' AS STATUS_DESC, '6' AS CDUM_Upload_Status, '0' AS CDUM_B_Reject_Cnt, '0' AS CDUM_H_Reject_Cnt "
        '            strsql &= "FROM CONTRACT_DIST_MSTR "
        '            strsql &= "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
        '            strsql &= "LEFT JOIN COMPANY_MSTR ON CDC_B_COY_ID = CM_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '            strsql &= "WHERE CDM_TYPE = 'C' "
        '            strsql &= "AND CDM_S_COY_ID = '" & Common.Parse(strVendor) & "' "

        '        Case "C"
        '            If strCatType = "C" Then
        '                strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, "
        '                strsql &= "CDM_START_DATE, CDM_END_DATE, CM_COY_NAME "
        '                strsql &= "FROM CONTRACT_DIST_MSTR "
        '                strsql &= "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
        '                strsql &= "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDC_B_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '            Else
        '                strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, "
        '                strsql &= "CDM_START_DATE, CDM_END_DATE, '' AS CM_COY_NAME "
        '                strsql &= "FROM CONTRACT_DIST_MSTR "
        '            End If
        '            strsql &= "WHERE CDM_TYPE = '" & strCatType & "' "
        '            strsql &= "AND CDM_S_COY_ID = '" & Common.Parse(strVendor) & "' "
        '    End Select

        '    If strCode <> "" Then
        '        strsql &= "AND CDM_GROUP_CODE " & Common.ParseSQL(strCode) & " "
        '    End If

        '    If strDesc <> "" Then
        '        strsql &= "AND CDM_GROUP_DESC " & Common.ParseSQL(strDesc) & " "
        '    End If

        '    If strBuyer <> "" Then
        '        strsql &= " AND CM_COY_NAME " & Common.ParseSQL(strBuyer) & " "
        '    End If

        '    If strDateFr <> "" Then
        '        strsql &= "AND CDM_START_DATE >= " & Common.ConvertDate(strDateFr) & " "
        '    End If

        '    If strDateTo <> "" Then
        '        strsql &= "AND CDM_END_DATE <= " & Common.ConvertDate(strDateTo & " 23:59:59.000") & " "
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    getCatalogueList = ds
        'End Function

        'Public Function getContractCatalogueList(ByVal strVendor As String, ByVal strType As String, ByVal strCatType As String, ByVal strCode As String, ByVal strDesc As String, ByVal strBuyer As String, ByVal strDateFr As String, ByVal strDateTo As String) As DataSet
        '    'Added by Joon on 6th June 2011
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    'If strCatType = "C" Then
        '    strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, CDM_START_DATE, " _
        '        & "CDM_END_DATE, CM_COY_NAME " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CDM_TYPE ='" & strCatType & "' "

        '    If strCode <> "" Then
        '        If strType = "IDX" Then
        '            strsql &= "AND CDM_GROUP_INDEX IN (" & strCode & ") "
        '        Else
        '            strsql &= "AND CDM_GROUP_CODE " & Common.ParseSQL(strCode) & " "
        '        End If

        '    End If

        '    If strDesc <> "" Then
        '        strsql &= "AND CDM_GROUP_DESC " & Common.ParseSQL(strDesc) & " "
        '    End If

        '    If strVendor <> "" Then
        '        strsql &= " AND CM_COY_NAME " & Common.ParseSQL(strVendor) & " "
        '    End If

        '    If strDateFr <> "" Then
        '        strsql &= "AND CDM_START_DATE >= " & Common.ConvertDate(strDateFr) & " "
        '    End If

        '    If strDateTo <> "" Then
        '        strsql &= "AND CDM_END_DATE <= " & Common.ConvertDate(strDateTo & " 23:59:59.000") & " "
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    getContractCatalogueList = ds
        'End Function

        'Public Function checkContractCatalogueList(ByVal strType As String, ByVal strCatType As String, ByVal strCode As String, ByVal strDateTo As String) As String
        '    Dim strsql As String
        '    Dim dtTemp As New DataTable

        '    'If strCatType = "C" Then
        '    strsql = "SELECT CAST(GROUP_CONCAT(CDM_GROUP_CODE SEPARATOR ', ') AS CHAR(1500)) AS CDM_GROUP_LIST " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CDM_TYPE ='" & strCatType & "' "

        '    If strCode <> "" Then
        '        If strType = "IDX" Then
        '            strsql &= "AND CDM_GROUP_INDEX IN (" & strCode & ") "
        '        Else
        '            strsql &= "AND CDM_GROUP_CODE " & Common.ParseSQL(strCode) & " "
        '        End If

        '    End If

        '    If strDateTo <> "" Then
        '        strsql &= "AND CDM_START_DATE >= " & Common.ConvertDate(strDateTo) & " "
        '    End If

        '    dtTemp = objDb.FillDt(strsql)
        '    If dtTemp.Rows.Count > 0 Then
        '        checkContractCatalogueList = Common.parseNull(dtTemp.Rows(0)("CDM_GROUP_LIST"))
        '    End If

        'End Function

        'Public Function saveCatalogueMasterEx(ByVal strType As String, ByVal strCatType As String, ByVal strCode As String, ByVal strDateTo As String) As Integer
        '    Dim strsql As String
        '    Dim strExist As String
        '    Dim strAryQuery(0) As String
        '    Dim dtTemp As New DataTable

        '    strsql = "UPDATE CONTRACT_DIST_MSTR " _
        '            & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '            & "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " _
        '            & "SET CONTRACT_DIST_MSTR.CDM_END_DATE = " & Common.ConvertDate(strDateTo) & " " _
        '            & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CDM_TYPE ='" & strCatType & "' AND " _
        '            & "CDM_GROUP_INDEX IN (" & strCode & ") "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "SELECT * FROM CONTRACT_DIST_MSTR " _
        '            & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '            & "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " _
        '            & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' AND CDM_TYPE ='" & strCatType & "' AND " _
        '            & "CDM_GROUP_INDEX IN (" & strCode & ") "

        '    dtTemp = objDb.FillDt(strsql)

        '    Dim i As Integer
        '    If dtTemp.Rows.Count > 0 Then
        '        For i = 0 To dtTemp.Rows.Count - 1
        '            If Convert.ToDateTime(strDateTo) <> Convert.ToDateTime(dtTemp.Rows(i)("CDM_END_DATE")) Then
        '                InsertAuditTrail(dtTemp.Rows(i)("CDM_GROUP_INDEX"), "Contract End Date", dtTemp.Rows(i)("CDM_END_DATE"), Left(Convert.ToString(Convert.ToDateTime(strDateTo)), 10), "B", "E", strAryQuery)
        '            End If
        '        Next
        '    End If

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            saveCatalogueMasterEx = WheelMsgNum.Save
        '        Else
        '            saveCatalogueMasterEx = WheelMsgNum.NotSave
        '        End If
        '    End If

        '    'If objDb.Execute(strsql) Then
        '    '    saveCatalogueMasterEx = WheelMsgNum.Save
        '    'Else
        '    '    saveCatalogueMasterEx = WheelMsgNum.NotSave
        '    'End If
        'End Function

        'Public Function getCatalogueItemList(ByVal intIndex As Integer) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    'strsql = "SELECT * FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & intIndex
        '    'strsql = "SELECT CONTRACT_DIST_ITEMS.*, " & _
        '    '        "CASE WHEN CODE_ABBR = 'EX' THEN CODE_DESC ELSE CONCAT(CODE_DESC, '(', CODE_VALUE, '%)') END AS GSTRATE " & _
        '    '        "FROM CONTRACT_DIST_ITEMS " & _
        '    '        "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'GST' AND CODE_ABBR = CDI_GST_RATE " & _
        '    '        "WHERE CDI_GROUP_INDEX = " & intIndex

        '    'Jules 2018.10.24 - Get Item Name from product_mstr
        '    'strsql = "SELECT CONTRACT_DIST_ITEMS.*, " &
        '    strsql = "SELECT CDI_GROUP_INDEX,CDI_PRODUCT_CODE,CDI_VENDOR_ITEM_CODE,PM_PRODUCT_DESC AS CDI_PRODUCT_DESC,CDI_UOM,CDI_UNIT_COST,CDI_REMARK," &
        '            "CDI_GST,CDI_GST_RATE,CDI_GST_TAX_CODE,CDI_CURRENCY_CODE, " &
        '            "CASE WHEN CDI_GST_RATE = 'N/A' THEN CDI_GST_RATE ELSE " &
        '            "IF((TAX_PERC IS NULL OR TAX_PERC = ''), CODE_DESC, CONCAT(CODE_DESC, '(', TAX_PERC,'%)')) END AS GSTRATE " &
        '            "FROM CONTRACT_DIST_ITEMS " &
        '            "INNER JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX " &
        '            "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CDI_GST_RATE " &
        '            "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " &
        '            "LEFT JOIN TAX ON TAX_CODE = CDI_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " &
        '            "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE " &
        '            "WHERE CDI_GROUP_INDEX = " & intIndex & " AND PM_DELETED <> 'Y' AND PM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' "
        '    ds = objDb.FillDs(strsql)
        '    getCatalogueItemList = ds
        'End Function

        'Public Function getBuyerCompany(ByVal strCompanyID As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CM_COY_ID, CM_COY_NAME FROM COMPANY_MSTR "
        '    strsql &= "WHERE (CM_COY_TYPE = 'BOTH' OR CM_COY_TYPE = 'BUYER') "
        '    strsql &= "AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
        '    strsql &= "AND CM_COY_ID <> '" & Common.Parse(strCompanyID) & "' "
        '    ds = objDb.FillDs(strsql)
        '    getBuyerCompany = ds
        'End Function

        'Public Function getVendorCompany() As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CM_COY_ID, CM_COY_NAME " _
        '            & "FROM company_vendor " _
        '            & "INNER JOIN COMPANY_MSTR ON CV_S_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' " _
        '            & "AND (CM_COY_TYPE = 'BOTH' OR CM_COY_TYPE = 'VENDOR') " _
        '            & "WHERE CV_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"
        '    ds = objDb.FillDs(strsql)
        '    getVendorCompany = ds
        'End Function

        'Public Function getConRefNo() As DataSet
        '    'Added by Joon on 7th June 2011
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND CDM_TYPE = 'C' " _
        '        & "ORDER BY CDM_GROUP_CODE"
        '    ds = objDb.FillDs(strsql)
        '    getConRefNo = ds
        'End Function

        'Public Function getBuyer() As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "SELECT  DISTINCT U.UM_USER_ID, U.UM_USER_NAME, " & objDb.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " _
        '        & "FROM USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM " _
        '        & "WHERE G.UU_USER_ID = U.UM_USER_ID AND U.UM_DELETED <> 'Y' AND U.UM_STATUS='A' " _
        '        & "AND G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND UM_COY_ID=UU_COY_ID " _
        '        & "AND (GM.UGM_FIXED_ROLE ='Buyer' OR GM.UGM_FIXED_ROLE= 'Purchasing Manager' OR " _
        '        & "GM.UGM_FIXED_ROLE= 'Purchasing Officer') " _
        '        & "AND UM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' ORDER BY U.UM_USER_ID "

        '    ds = objDb.FillDs(strsql)
        '    getBuyer = ds
        'End Function

        'Public Function getConRefNoSearch() As DataSet
        '    'Added by Joon on 7th June 2011
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT DISTINCT CDM_GROUP_INDEX, CDM_GROUP_CODE " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "LEFT JOIN CONTRACT_DIST_USER ON CDU_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND CDM_TYPE = 'C' AND CDM_END_DATE >= NOW() AND CDU_USER_ID ='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
        '        & "ORDER BY CDM_GROUP_CODE"
        '    ds = objDb.FillDs(strsql)
        '    getConRefNoSearch = ds
        'End Function
        'Public Function getConDesc() As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT DISTINCT CDM_GROUP_DESC " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "LEFT JOIN CONTRACT_DIST_USER ON CDU_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND CDM_TYPE = 'C' AND CDM_END_DATE >= NOW() AND CDU_USER_ID ='" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
        '        & "ORDER BY CDM_GROUP_DESC"
        '    ds = objDb.FillDs(strsql)
        '    getConDesc = ds
        'End Function

        ''Added by Joon on 5 Oct 2011 for issue 969
        'Public Function getSearchConRefNo() As DataSet
        '    'Added by Joon on 7th June 2011
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "INNER JOIN contract_dist_user ON CDU_GROUP_INDEX=CDM_GROUP_INDEX " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND CDM_TYPE = 'C' AND CDU_USER_ID='" & Common.Parse(HttpContext.Current.Session("UserId")) & "' " _
        '        & "ORDER BY CDM_GROUP_CODE"
        '    ds = objDb.FillDs(strsql)
        '    getSearchConRefNo = ds
        'End Function

        '' ai chu add on 31/10/2005
        '' retrieves all vendor company that have valid contract 
        '' strMode - A = Vendor retrieved from CONTRACT_DIST_MSTR, D = Vendor retrieved from BUYER_CATALOGUE_ITEMS
        'Public Function getVendorWithValidContract(ByVal strMode As String, Optional ByVal intBCindex As Double = -1) As DataSet
        '    Dim strsql As String
        '    Dim ds As DataSet
        '    Select Case UCase(strMode)
        '        Case "A"
        '            strsql = "SELECT DISTINCT CM_COY_ID, CM_COY_NAME FROM CONTRACT_DIST_MSTR "
        '            strsql &= "INNER JOIN COMPANY_MSTR ON CDM_S_COY_ID = CM_COY_ID "
        '            strsql &= "WHERE (GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE) AND CDM_TYPE = 'C' "
        '            strsql &= "AND CM_COY_ID IN (SELECT CV_S_COY_ID FROM COMPANY_VENDOR "
        '            strsql &= "WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
        '        Case "D"
        '            strsql = "SELECT DISTINCT CM_COY_ID, CM_COY_NAME FROM BUYER_CATALOGUE_ITEMS "
        '            strsql &= "INNER JOIN COMPANY_MSTR ON BCU_S_COY_ID = CM_COY_ID "
        '            strsql &= "WHERE BCU_SOURCE = 'CP' AND BCU_CAT_INDEX=" & intBCindex
        '            'strsql &= "AND CM_COY_ID IN (SELECT CV_S_COY_ID FROM COMPANY_VENDOR "
        '            'strsql &= "WHERE CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
        '    End Select

        '    ds = objDb.FillDs(strsql)
        '    getVendorWithValidContract = ds
        'End Function

        '' ai chu add on 31/10/2005
        '' retrieve contract code and contract description by Vendor Company Id
        '' strMode - A = Add(retrived from CONTRACT_DIST_ITEMS), D = Delete (retrieve from BUYER_CATALOGUE_ITEMS)
        'Public Function getContractGroupRef(ByVal strCoyId As String, ByVal strMode As String, Optional ByVal intBCindex As Double = -1) As DataSet
        '    Dim ds As DataSet
        '    Dim strsql As String
        '    Select Case strMode
        '        Case "A"
        '            strsql = "SELECT DISTINCT CDM_GROUP_INDEX, " & objDb.Concat("':'", "", "CDM_GROUP_CODE", "CDM_GROUP_DESC") & " AS GROUP_DESC "
        '            strsql &= "FROM CONTRACT_DIST_MSTR "
        '            strsql &= "INNER JOIN CONTRACT_DIST_ITEMS ON CDM_GROUP_INDEX = CDI_GROUP_INDEX "
        '            strsql &= "WHERE (GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE) AND CDM_S_COY_ID = '" & Common.Parse(strCoyId) & "' "
        '        Case "D"
        '            strsql = "SELECT DISTINCT CDM_GROUP_INDEX, " & objDb.Concat("':'", "", "CDM_GROUP_CODE", "CDM_GROUP_DESC") & " AS GROUP_DESC "
        '            strsql &= "FROM BUYER_CATALOGUE_ITEMS "
        '            strsql &= "INNER JOIN CONTRACT_DIST_MSTR ON BCU_CD_GROUP_INDEX = CDM_GROUP_INDEX "
        '            strsql &= "WHERE CDM_S_COY_ID = '" & Common.Parse(strCoyId) & "' "
        '            strsql &= "AND BCU_SOURCE = 'CP' AND BCU_CAT_INDEX=" & intBCindex
        '    End Select

        '    ds = objDb.FillDs(strsql)
        '    getContractGroupRef = ds
        'End Function

        'Public Function getContractCatalogueItemList(ByVal intIndex As Integer, ByVal strType As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "SELECT CDUI_Upload_Index, CDUI_Product_Code, CDUI_Vendor_Item_Code, CDUI_Product_Desc, "
        '    strsql &= "CDUI_UOM, CDUI_Unit_Cost, CDUI_GST, CDUI_Currency_Code, "

        '    Select Case strType
        '        Case "D"
        '            strsql &= "CDUI_Remark, '' as Remark2 "
        '            strsql &= "FROM CONT_DIST_UPLOADITEMS "
        '            strsql &= "WHERE CDUI_Upload_Index = " & intIndex
        '        Case "B", "R", "S"
        '            strsql &= "(SELECT TOP 1 CDUR_Remark FROM CONT_DIST_UPLOADREMARK "
        '            strsql &= "WHERE CDUR_Upload_Index = " & intIndex & " "
        '            strsql &= "AND CDUI_Product_Code = CDUR_Product_Code  "
        '            strsql &= "GROUP BY CDUI_Product_Code, CDUR_Remark, CDUR_Remark_Id "
        '            strsql &= "ORDER BY CDUR_Remark_Id DESC ) AS CDUI_Remark, CDUI_Remark AS Remark2 "
        '            strsql &= "FROM CONT_DIST_UPLOADITEMS "
        '            strsql &= "WHERE CDUI_Upload_Index = " & intIndex
        '        Case "A"
        '            strsql = "SELECT CDI_GROUP_INDEX AS CDUI_Upload_Index, CDI_PRODUCT_CODE AS CDUI_Product_Code, "
        '            strsql &= "CDI_VENDOR_ITEM_CODE AS CDUI_Vendor_Item_Code, CDI_PRODUCT_DESC AS CDUI_Product_Desc, "
        '            strsql &= "CDI_UOM AS CDUI_UOM, CDI_UNIT_COST AS CDUI_Unit_Cost, CDI_REMARK AS CDUI_Remark, '' as Remark2, "
        '            strsql &= "CDI_GST AS CDUI_GST, CDI_CURRENCY_CODE AS CDUI_Currency_Code "
        '            strsql &= "FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & intIndex
        '    End Select

        '    ds = objDb.FillDs(strsql)
        '    getContractCatalogueItemList = ds
        'End Function

        'Public Function insertContractCatalogueMaster(ByVal dtCat As DataTable) As Integer
        '    Dim strsql As String
        '    Dim strExist As String
        '    strExist = "SELECT '*' From CONT_DIST_UPLOADMSTR, CONTRACT_DIST_MSTR "
        '    strExist &= "WHERE (CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strExist &= "AND CDUM_Contract_Code = '" & Common.Parse(Common.Parse(dtCat.Rows(0)("ContractCode"))) & "') "
        '    strExist &= "OR (CDM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strExist &= "AND CDM_GROUP_CODE = '" & Common.Parse(Common.Parse(dtCat.Rows(0)("ContractCode"))) & "')"
        '    If objDb.Exist(strExist) > 0 Then
        '        insertContractCatalogueMaster = WheelMsgNum.Duplicate
        '        Exit Function
        '    End If

        '    strsql = "INSERT INTO CONT_DIST_UPLOADMSTR (CDUM_Contract_Code, "
        '    strsql &= "CDUM_Contract_Desc, CDUM_S_Coy_Id, CDUM_Start_Date, CDUM_End_Date, CDUM_Type, "
        '    strsql &= "CDUM_Upload_Status, CDUM_B_Reject_Cnt, CDUM_H_Reject_Cnt) VALUES ("
        '    strsql &= "'" & Common.Parse(dtCat.Rows(0)("ContractCode")) & "', " ' CDUM_Contract_Code
        '    strsql &= "'" & Common.Parse(dtCat.Rows(0)("ContractDesc")) & "', " 'CDUM_Contract_Desc
        '    strsql &= "'" & Common.Parse(dtCat.Rows(0)("VendorId")) & "', " 'CDUM_S_Coy_Id
        '    strsql &= Common.ConvertDate(dtCat.Rows(0)("StartDate")) & ", "  'CDUM_Start_Date
        '    strsql &= Common.ConvertDate(dtCat.Rows(0)("EndDate")) & ", "  'CDUM_End_Date
        '    strsql &= "'C', " 'CDUM_Type
        '    strsql &= "'" & CatalogueStatus.Draft & "', " 'CDUM_Upload_Status
        '    strsql &= "0, 0); "
        '    strsql &= "INSERT INTO CONT_DIST_UPLOADCOY SELECT "
        '    strsql &= "(SELECT ISNULL(MAX(CDUM_Upload_Index),0) FROM CONT_DIST_UPLOADMSTR), "
        '    strsql &= "'" & Common.Parse(dtCat.Rows(0)("BuyerID")) & "' " 'CDUD_B_Coy_Id

        '    If objDb.Execute(strsql) Then
        '        insertContractCatalogueMaster = WheelMsgNum.Save
        '    Else
        '        insertContractCatalogueMaster = WheelMsgNum.NotSave
        '    End If
        'End Function

        'Public Function updateContractCatalogueMaster(ByVal dtCat As DataTable, ByVal strOldValue As String) As Integer
        '    Dim strsql As String
        '    Dim strExist As String
        '    strExist = "SELECT '*' From CONT_DIST_UPLOADMSTR, CONTRACT_DIST_MSTR "
        '    strExist &= "WHERE (CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strExist &= "AND CDUM_Contract_Code = '" & Common.Parse(dtCat.Rows(0)("ContractCode")) & "') "
        '    strExist &= "OR (CDM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strExist &= "AND CDM_GROUP_CODE = '" & Common.Parse(dtCat.Rows(0)("ContractCode")) & "')"

        '    If strOldValue <> dtCat.Rows(0)("ContractCode") Then
        '        If objDb.Exist(strExist) > 0 Then
        '            updateContractCatalogueMaster = WheelMsgNum.Duplicate
        '            Exit Function
        '        End If
        '    End If

        '    strsql = "UPDATE CONT_DIST_UPLOADMSTR SET "
        '    strsql &= "CDUM_Contract_Code = '" & Common.Parse(dtCat.Rows(0)("ContractCode")) & "', "
        '    strsql &= "CDUM_Contract_Desc = '" & Common.Parse(dtCat.Rows(0)("ContractDesc")) & "', "
        '    strsql &= "CDUM_Start_Date = " & Common.ConvertDate(dtCat.Rows(0)("StartDate")) & ", "
        '    strsql &= "CDUM_End_Date = " & Common.ConvertDate(dtCat.Rows(0)("EndDate")) & " "
        '    strsql &= "WHERE CDUM_Upload_Index = " & dtCat.Rows(0)("Index") & " ; "

        '    strsql &= "UPDATE CONT_DIST_UPLOADCOY SET "
        '    strsql &= "CDUD_B_Coy_Id = '" & Common.Parse(dtCat.Rows(0)("BuyerID")) & "' " 'CDUD_B_Coy_Id
        '    strsql &= "WHERE CDUD_Upload_Index = " & dtCat.Rows(0)("Index") & "; "

        '    If objDb.Execute(strsql) Then
        '        updateContractCatalogueMaster = WheelMsgNum.Save
        '    Else
        '        updateContractCatalogueMaster = WheelMsgNum.NotSave
        '    End If
        'End Function

        'Public Function insertContractCatalogueItem(ByVal dtCat As DataTable)
        '    Dim strsql As String
        '    Dim strExist As String
        '    Dim strAryQuery(0) As String
        '    Dim i As Integer

        '    For i = 0 To dtCat.Rows.Count - 1
        '        strExist = "SELECT CDUI_Product_Code FROM CONT_DIST_UPLOADITEMS "
        '        strExist &= "WHERE CDUI_Upload_Index = " & dtCat.Rows(i)("Index") & " "
        '        strExist &= "AND CDUI_Product_Code = '" & Common.Parse(dtCat.Rows(i)("ProductCode")) & "' "
        '        If objDb.Exist(strExist) = 0 Then
        '            ' CONT_DIST_UPLOADITEMS
        '            strsql = "INSERT INTO CONT_DIST_UPLOADITEMS (CDUI_Upload_Index, CDUI_Product_Code, CDUI_Vendor_Item_Code, "
        '            strsql &= "CDUI_Product_Desc, CDUI_UOM, CDUI_Unit_Cost, CDUI_Remark, "
        '            strsql &= "CDUI_GST, CDUI_Currency_Code) VALUES ("
        '            strsql &= dtCat.Rows(i)("Index") & ", " ' CDUI_Upload_Index
        '            strsql &= "'" & Common.Parse(dtCat.Rows(i)("ProductCode")) & "', " ' CDUI_Product_Code
        '            strsql &= "'" & Common.Parse(dtCat.Rows(i)("VendorItemCode")) & "', " ' CDUI_Vendor_Item_Code
        '            strsql &= "'" & Common.Parse(dtCat.Rows(i)("ProductDesc")) & "', " ' CDUI_Product_Desc
        '            strsql &= "'" & Common.Parse(dtCat.Rows(i)("UOM")) & "', " ' CDUI_UOM
        '            strsql &= dtCat.Rows(i)("UnitCost") & ", " ' CDUI_Unit_Cost
        '            strsql &= "'" & Common.Parse(dtCat.Rows(i)("Remark")) & "', " ' CDUI_Remark
        '            strsql &= dtCat.Rows(i)("GST") & ", " ' CDUI_GST
        '            strsql &= "'" & Common.Parse(dtCat.Rows(i)("CurrencyCode")) & "') " ' CDUI_Currency_Code
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    Next

        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function updateContractCatalogueItem(ByVal dtCat As DataTable)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim i As Integer

        '    For i = 0 To dtCat.Rows.Count - 1
        '        ' CONT_DIST_UPLOADITEMS
        '        strsql = "UPDATE CONT_DIST_UPLOADITEMS SET "
        '        strsql &= "CDUI_Unit_Cost = " & dtCat.Rows(i)("UnitCost") & ", " ' CDUI_Unit_Cost
        '        strsql &= "CDUI_Remark = '" & Common.Parse(dtCat.Rows(i)("Remark")) & "' " ' CDUI_Remark
        '        strsql &= "WHERE CDUI_Upload_Index = " & dtCat.Rows(i)("Index") & " "
        '        strsql &= "AND CDUI_Product_Code = '" & Common.Parse(dtCat.Rows(i)("ProductCode")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next

        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function insertContractItemRemarkSQL(ByVal dr As DataRow) As String
        '    Dim strsql As String
        '    strsql = "INSERT INTO CONT_DIST_UPLOADREMARK "
        '    strsql &= "(CDUR_Upload_Index, CDUR_Product_Code, CDUR_Remark_Id, CDUR_Remark, "
        '    strsql &= "CDUR_User_Id, CDUR_User_Coy_Id, CDUR_User_Role, CDUR_Created_DateTime) SELECT "
        '    strsql &= dr("index") & ", " ' CDUI_Upload_Index
        '    strsql &= "'" & Common.Parse(dr("ProductCode")) & "', " ' CDUI_Product_Code
        '    strsql &= "(SELECT ISNULL(MAX(CDUR_Remark_Id),0) +1 FROM CONT_DIST_UPLOADREMARK " ' CDUR_Remark_Id
        '    strsql &= "WHERE CDUR_Upload_Index = " & dr("Index") & " "
        '    strsql &= "AND CDUR_Product_Code = '" & Common.Parse(dr("ProductCode")) & "'), "
        '    strsql &= "'" & Common.Parse(dr("Remark")) & "', " ' CDUR_Remark
        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', " ' CDUR_User_Id
        '    strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' CDUR_User_Coy_Id
        '    strsql &= "'" & Common.Parse(dr("UserRole")) & "', " ' CDUR_User_Role
        '    strsql &= "GETDATE() " ' CDUR_User_Role, CDUR_Created_DateTime
        '    insertContractItemRemarkSQL = strsql
        'End Function

        'Public Function getSingleContractCatalogue(ByVal strCode As String, ByVal strType As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "SELECT CDUM_Upload_Index, CDUM_Contract_Code, CDUM_Contract_Desc, CDUM_S_Coy_Id, CDUM_Start_Date, "
        '    strsql &= "CDUM_End_Date, CDUM_Type, CDUM_Upload_Status, CDUM_B_Reject_Cnt, CDUM_H_Reject_Cnt, CDUD_B_Coy_Id, STATUS_DESC, "

        '    Select Case strType
        '        Case "V"
        '            strsql &= "CM_COY_NAME "
        '            strsql &= "FROM CONT_DIST_UPLOADMSTR "
        '            strsql &= "LEFT JOIN CONT_DIST_UPLOADCOY ON CDUM_Upload_Index = CDUD_Upload_Index "
        '            strsql &= "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDUD_B_Coy_Id  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '            strsql &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = CDUM_Upload_Status AND STATUS_TYPE = 'CONTRACT' "
        '            strsql &= "WHERE CDUM_Contract_Code = '" & Common.Parse(strCode) & "' "
        '            strsql &= "AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "

        '        Case "B"
        '            strsql &= "CM_COY_NAME "
        '            strsql &= "FROM CONT_DIST_UPLOADMSTR "
        '            strsql &= "LEFT JOIN CONT_DIST_UPLOADCOY ON CDUM_Upload_Index = CDUD_Upload_Index "
        '            strsql &= "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDUM_S_Coy_Id  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '            strsql &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = CDUM_Upload_Status AND STATUS_TYPE = 'CONTRACT' "
        '            strsql &= "WHERE CDUM_Contract_Code = '" & Common.Parse(strCode) & "' "
        '            strsql &= "AND CDUD_B_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "

        '        Case "H"
        '            strsql &= "A.CM_COY_NAME AS BuyerName, B.CM_COY_NAME AS VendorName "
        '            strsql &= "FROM CONT_DIST_UPLOADMSTR "
        '            strsql &= "LEFT JOIN CONT_DIST_UPLOADCOY ON CDUM_Upload_Index = CDUD_Upload_Index "
        '            strsql &= "LEFT JOIN COMPANY_MSTR AS A ON A.CM_COY_ID = CDUD_B_Coy_Id  AND A.CM_STATUS = 'A' AND A.CM_DELETED <> 'Y' "
        '            strsql &= "LEFT JOIN COMPANY_MSTR AS B ON B.CM_COY_ID = CDUM_S_Coy_Id  AND B.CM_STATUS = 'A' AND B.CM_DELETED <> 'Y' "
        '            strsql &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = CDUM_Upload_Status AND STATUS_TYPE = 'CONTRACT' "
        '            strsql &= "WHERE CDUM_Contract_Code = '" & Common.Parse(strCode) & "' "
        '            strsql &= "AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyIdToken") & "' "

        '        Case "A"
        '            strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, "
        '            strsql &= "CDM_START_DATE, CDM_END_DATE, CM_COY_NAME "
        '            strsql &= "FROM CONTRACT_DIST_MSTR "
        '            strsql &= "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX "
        '            strsql &= "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDC_B_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '            strsql &= "WHERE CDM_TYPE = 'C' "
        '            strsql &= "AND CDM_GROUP_CODE = '" & Common.Parse(strCode) & "' "
        '            strsql &= "AND CDM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    End Select

        '    ds = objDb.FillDs(strsql)
        '    getSingleContractCatalogue = ds
        'End Function

        'Public Function getSingleCatalogue(ByVal intIndex As Integer, ByVal strCatType As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, "
        '    strsql &= "CDM_START_DATE, CDM_END_DATE "

        '    Select Case strCatType
        '        Case "C"
        '            strsql &= ", CM_COY_NAME, CDC_B_COY_ID "
        '            strsql &= "FROM CONTRACT_DIST_MSTR "
        '            strsql &= "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX "
        '            strsql &= "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDC_B_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '            strsql &= "WHERE CDM_GROUP_INDEX = " & intIndex

        '        Case "D"
        '            strsql &= "FROM CONTRACT_DIST_MSTR "
        '            strsql &= "WHERE CDM_GROUP_INDEX = " & intIndex
        '    End Select

        '    ds = objDb.FillDs(strsql)
        '    getSingleCatalogue = ds
        'End Function

        'Public Function getSingleConCatalogue(ByVal intIndex As Integer) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet

        '    strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, CDM_START_DATE, " _
        '        & "CDM_END_DATE, CM_COY_NAME, CDC_B_COY_ID " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX " _
        '        & "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' " _
        '        & "WHERE CDM_GROUP_INDEX = " & intIndex

        '    ds = objDb.FillDs(strsql)
        '    getSingleConCatalogue = ds
        'End Function

        'Public Function getSingleApproveContractCatalogue(ByVal strCode As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CDM_GROUP_INDEX AS CDUM_Upload_Index, CDM_GROUP_CODE AS CDUM_Contract_Code, "
        '    strsql &= "CDM_GROUP_DESC AS CDUM_Contract_Desc , CDM_S_COY_ID AS CDUM_S_Coy_Id, "
        '    strsql &= "CDM_START_DATE AS CDUM_Start_Date, CDM_END_DATE AS CDUM_End_Date, "
        '    strsql &= "CDM_TYPE AS CDUM_Type, CDC_B_COY_ID AS CDUD_B_Coy_Id, CM_COY_NAME "
        '    strsql &= "FROM CONTRACT_DIST_MSTR "
        '    strsql &= "LEFT JOIN CONTRACT_DIST_COY ON CDM_Upload_Index = CDC_Upload_Index "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDC_B_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '    strsql &= "WHERE CDM_GROUP_CODE = '" & Common.Parse(strCode) & "' "
        '    strsql &= "AND CDM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

        '    ds = objDb.FillDs(strsql)
        '    getSingleApproveContractCatalogue = ds
        'End Function

        'Public Function getContractPriceItem(ByVal strMode As String, ByVal strItemCodeList() As String, ByVal intIndex As Integer) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    Dim i As Integer

        '    Select Case strMode
        '        Case "add" ' from PRODUCT_MSTR
        '            strsql = "SELECT PM_PRODUCT_CODE AS PRODUCT_CODE, PM_PRODUCT_DESC AS PRODUCT_DESC, PM_VENDOR_ITEM_CODE AS VENDOR_ITEM_CODE, "
        '            strsql &= "PM_UNIT_COST AS UNIT_COST, PM_UNIT_COST AS CONTRACT_PRICE, '' AS REMARK, PM_UOM AS UOM, "
        '            strsql &= "ISNULL(CODE_VALUE,0) AS GST, PM_CURRENCY_CODE AS CURRENCYCODE, '' as Remark2 "
        '            strsql &= "FROM PRODUCT_MSTR "
        '            strsql &= "LEFT JOIN CODE_MSTR ON CODE_ABBR = PM_GST_CODE AND CODE_CATEGORY = 'SST' "
        '            strsql &= "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_CODE IN (" '& strItemCodeList & ") "
        '            For i = 0 To strItemCodeList.Length - 2
        '                strsql &= "'" & strItemCodeList(i) & "', "
        '            Next
        '            strsql &= "'" & strItemCodeList(strItemCodeList.Length - 1) & "') "

        '        Case "mod" ' from CONT_DIST_UPLOADITEMS
        '            strsql = "SELECT CDUI_Product_Code as PRODUCT_CODE, CDUI_Product_Desc as PRODUCT_DESC, CDUI_Vendor_Item_Code as VENDOR_ITEM_CODE, "
        '            strsql &= "PM_Unit_Cost AS UNIT_COST, CDUI_Unit_Cost AS CONTRACT_PRICE, CDUI_Remark AS REMARK, CDUI_UOM AS UOM, "
        '            strsql &= "CDUI_GST AS GST, CDUI_Currency_Code as CURRENCYCODE, "
        '            strsql &= "(SELECT TOP 1 CDUR_Remark FROM CONT_DIST_UPLOADREMARK "
        '            strsql &= "WHERE CDUR_Upload_Index = " & intIndex & " "
        '            strsql &= "AND CDUI_Product_Code = CDUR_Product_Code  "
        '            strsql &= "GROUP BY CDUI_Product_Code, CDUR_Remark, CDUR_Remark_Id "
        '            strsql &= "ORDER BY CDUR_Remark_Id DESC ) AS REMARK2 "
        '            strsql &= "FROM CONT_DIST_UPLOADITEMS "
        '            strsql &= "LEFT JOIN PRODUCT_MSTR ON CDUI_Product_Code = PM_Product_Code AND PM_DELETED <> 'Y' "
        '            strsql &= "WHERE CDUI_Upload_Index = " & intIndex & " "
        '            strsql &= "AND CDUI_Product_Code IN ("
        '            For i = 0 To strItemCodeList.Length - 2
        '                strsql &= "'" & strItemCodeList(i) & "', "
        '            Next
        '            strsql &= "'" & strItemCodeList(strItemCodeList.Length - 1) & "') "

        '        Case "cat"
        '            strsql = "SELECT CDI_PRODUCT_CODE AS PRODUCT_CODE, CDI_PRODUCT_DESC AS PRODUCT_DESC, CDI_VENDOR_ITEM_CODE AS VENDOR_ITEM_CODE, "
        '            strsql &= "PM_Unit_Cost AS UNIT_COST, CDI_UNIT_COST AS CONTRACT_PRICE, CDI_REMARK AS REMARK, CDI_UOM AS UOM, "
        '            strsql &= "CDI_GST AS GST, CDI_CURRENCY_CODE AS CURRENCYCODE, '' AS REMARK2 "
        '            strsql &= "FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE = PM_Product_Code AND PM_DELETED <> 'Y' "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & intIndex & " "
        '            strsql &= "AND CDI_PRODUCT_CODE IN ("
        '            For i = 0 To strItemCodeList.Length - 2
        '                strsql &= "'" & strItemCodeList(i) & "', "
        '            Next
        '            strsql &= "'" & strItemCodeList(strItemCodeList.Length - 1) & "') "
        '    End Select

        '    ds = objDb.FillDs(strsql)
        '    getContractPriceItem = ds
        'End Function

        'Public Function getContractItem(ByVal strItemCodeList() As String, ByVal intIndex As Integer) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    Dim i As Integer

        '    'Jules 2018.10.24 - Get Item Name from product_mstr
        '    'strsql = "SELECT CDI_PRODUCT_CODE, CDI_PRODUCT_DESC, CDI_VENDOR_ITEM_CODE, "
        '    strsql = "SELECT CDI_PRODUCT_CODE, PM_PRODUCT_DESC AS CDI_PRODUCT_DESC, CDI_VENDOR_ITEM_CODE, "
        '    strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_UOM, "
        '    strsql &= "CDI_GST, CDI_GST_RATE, CDI_GST_TAX_CODE, CDI_CURRENCY_CODE, CDI_REMARK "
        '    strsql &= "FROM CONTRACT_DIST_ITEMS "
        '    strsql &= "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE "
        '    strsql &= "WHERE CDI_GROUP_INDEX = " & intIndex & " "
        '    strsql &= "AND CDI_PRODUCT_CODE IN ("
        '    For i = 0 To strItemCodeList.Length - 2
        '        strsql &= "'" & strItemCodeList(i) & "', "
        '    Next
        '    strsql &= "'" & strItemCodeList(strItemCodeList.Length - 1) & "') "
        '    strsql &= "AND PM_DELETED <> 'Y' AND PM_S_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' "

        '    ds = objDb.FillDs(strsql)
        '    getContractItem = ds
        'End Function

        'Public Function getProductList(ByVal strSearchVal() As String, ByVal intIndex As Integer, ByVal strCatType As String, ByVal strType As String, ByVal strBuyer As String, ByVal strVendor As String) As DataSet
        '    'Public Function getProductList(ByVal strSearchVal() As String, ByVal intIndex As Integer, ByVal strCatType As String, ByVal strVendor As String) As DataSet
        '    ' strCatType - C (from CONT_DIST_UPLOADITEMS); D (from CONTRACT_DIST_ITEMS)
        '    ' strType - C(Contract); D(Discount)
        '    Dim strsql, strCondition As String
        '    Dim dsProduct As DataSet
        '    'Dim strCoyID As String = HttpContext.Current.Session("CompanyId")

        '    strsql = "SELECT PM_PRODUCT_CODE, PM_PRODUCT_DESC, PM_VENDOR_ITEM_CODE, PM_CURRENCY_CODE, "
        '    strsql &= "PM_UNIT_COST, PM_UOM "
        '    strsql &= "FROM PRODUCT_MSTR "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '    strsql &= "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_CODE NOT IN "

        '    'If strCatType = "C" Then ' from CONT_DIST_UPLOADITEMS
        '    '    strsql &= "(SELECT CDUI_Product_Code FROM CONT_DIST_UPLOADITEMS "
        '    '    strsql &= "WHERE CDUI_Upload_Index = " & intIndex & ") "
        '    'Else ' from CONTRACT_DIST_ITEMS
        '    '    strsql &= "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS "
        '    '    strsql &= "WHERE CDI_GROUP_INDEX = " & intIndex & ") "
        '    'End If

        '    If strType <> "DC" Then ' need to check items not exist in other valid contract catalogue (approved/not approved)
        '        strsql &= "(SELECT CDUI_Product_Code FROM CONT_DIST_UPLOADITEMS "
        '        strsql &= "WHERE CDUI_Upload_Index = " & intIndex & ") "

        '        strsql &= "AND NOT EXISTS (SELECT * FROM CONTRACT_DIST_MSTR, CONTRACT_DIST_ITEMS, CONTRACT_DIST_COY "
        '        strsql &= "WHERE CDM_GROUP_INDEX = CDI_GROUP_INDEX "
        '        strsql &= "AND CDC_GROUP_INDEX = CDM_GROUP_INDEX "
        '        strsql &= "AND CDM_S_COY_ID = '" & Common.Parse(strVendor) & "' "
        '        strsql &= "AND CDC_B_COY_ID = '" & Common.Parse(strBuyer) & "' "
        '        strsql &= "AND (getdate() BETWEEN CDM_START_DATE AND CDM_END_DATE + 1 AND CDM_TYPE = 'C' "
        '        'strsql &= "AND CDM_END_DATE < (SELECT CDM_START_DATE FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = 5)) "
        '        strsql &= "AND CDM_END_DATE >= (SELECT CDUM_Start_Date FROM CONT_DIST_UPLOADMSTR WHERE CDUM_Upload_Index = " & intIndex & ")) "
        '        strsql &= "AND PM_PRODUCT_CODE = CDI_PRODUCT_CODE) "

        '        strsql &= "AND NOT EXISTS(SELECT * FROM CONT_DIST_UPLOADMSTR, CONT_DIST_UPLOADITEMS, CONT_DIST_UPLOADCOY "
        '        strsql &= "WHERE CDUM_Upload_Index = CDUI_Upload_Index "
        '        strsql &= "AND CDUD_Upload_Index = CDUM_Upload_Index "
        '        strsql &= "AND CDUM_S_Coy_Id = '" & Common.Parse(strVendor) & "' "
        '        strsql &= "AND CDUD_B_Coy_Id = '" & Common.Parse(strBuyer) & "' "
        '        strsql &= "AND getdate() between CDUM_START_DATE AND CDUM_END_DATE + 1 "
        '        strsql &= "AND CDUM_Upload_Status IN (2,4) "
        '        strsql &= "AND PM_PRODUCT_CODE = CDUI_PRODUCT_CODE) "
        '    Else
        '        strsql &= "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS "
        '        strsql &= "WHERE CDI_GROUP_INDEX = " & intIndex & ") "
        '    End If


        '    If strSearchVal(0) <> String.Empty Then
        '        strsql &= " AND PM_PRODUCT_CODE" & Common.ParseSQL(strSearchVal(0))
        '    End If

        '    '//Product Desc
        '    If strSearchVal(1) <> String.Empty Then
        '        strsql &= " AND PM_PRODUCT_DESC" & Common.ParseSQL(strSearchVal(1))
        '    End If

        '    '//Vendor
        '    If strSearchVal(2) <> String.Empty Then
        '        strsql &= " AND PM_S_COY_ID" & Common.ParseSQL(strSearchVal(2))
        '    End If

        '    '//Vendor Item Code
        '    If strSearchVal(3) <> String.Empty Then
        '        strsql &= " AND PM_VENDOR_ITEM_CODE" & Common.ParseSQL(strSearchVal(3))
        '    End If

        '    dsProduct = objDb.FillDs(strsql)
        '    Return dsProduct
        'End Function

        'Public Function getConCatItemList(ByVal intIndex As Integer, ByVal strItemCode As String, ByVal strItemName As String, ByVal strCommType As String, ByVal strItemType As ArrayList) As DataSet
        '    'Public Function getProductList(ByVal strSearchVal() As String, ByVal intIndex As Integer, ByVal strCatType As String, ByVal strVendor As String) As DataSet
        '    ' strCatType - C (from CONT_DIST_UPLOADITEMS); D (from CONTRACT_DIST_ITEMS)
        '    ' strType - C(Contract); D(Discount)
        '    Dim strsql, strCondition As String
        '    Dim dsProduct As DataSet
        '    Dim strType As String

        '    'Dim strCoyID As String = HttpContext.Current.Session("CompanyId")

        '    'strsql = "SELECT PM_PRODUCT_CODE, PM_PRODUCT_DESC, PM_VENDOR_ITEM_CODE, PM_CURRENCY_CODE, "
        '    'strsql &= "PM_UNIT_COST, PM_UOM "
        '    'strsql &= "FROM PRODUCT_MSTR "
        '    'strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID  AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '    'strsql &= "WHERE PM_DELETED <> 'Y' AND PM_PRODUCT_CODE NOT IN "
        '    strsql = "SELECT PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_ITEM_TYPE,PM_UOM,CT_NAME,CM_CURRENCY_CODE " _
        '        & "FROM PRODUCT_MSTR " _
        '        & "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME " _
        '        & "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID " _
        '        & "AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' " _
        '        & "WHERE PM_DELETED <> 'Y' AND PM_S_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND PM_PRODUCT_CODE NOT IN (SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
        '        & "WHERE CDI_GROUP_INDEX = " & intIndex & ")"

        '    '//Vendor Item Code
        '    If strItemCode <> "" Then
        '        strsql &= " AND PM_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(strItemCode) & "%' "
        '    End If

        '    '//Product Desc
        '    If strItemName <> "" Then
        '        strsql &= " AND PM_PRODUCT_DESC LIKE '%" & Common.Parse(strItemName) & "%' "
        '    End If

        '    '//Commodity Type
        '    If strCommType <> "" Then
        '        strsql &= " AND PM_CATEGORY_NAME = '" & Common.Parse(strCommType) & "' "
        '    End If

        '    'Item Type
        '    If strItemType.Count > 0 Then
        '        For i As Integer = 0 To strItemType.Count - 1
        '            If strType = "" Then
        '                strType = "'" & strItemType(i) & "'"
        '            Else
        '                strType = strType & "," & "'" & strItemType(i) & "'"
        '            End If
        '        Next
        '        strType = "(" & strType & ")"
        '        If strType <> "" Then
        '            strsql &= " AND PM_ITEM_TYPE IN " & strType
        '        End If
        '    End If

        '    dsProduct = objDb.FillDs(strsql)
        '    Return dsProduct
        'End Function

        'Public Function getConCatSearchList(ByVal intIndex As Integer, ByVal strItemCode As String, ByVal strItemName As String, ByVal strCommType As String, ByVal strItemType As ArrayList, ByVal strVendor As String, Optional ByVal catDesc As String = "") As DataSet
        '    Dim strsql, strCondition As String
        '    Dim dsProduct As DataSet
        '    Dim strType As String

        '    strsql = "SELECT IFNULL(CDI_VENDOR_ITEM_CODE,'') AS CDI_VENDOR_ITEM_CODE, IFNULL(CDI_PRODUCT_CODE,'') AS CDI_PRODUCT_CODE," _
        '        & "IFNULL(PM_PRODUCT_DESC,'') AS CDI_PRODUCT_DESC, CDM_GROUP_CODE,CDM_GROUP_INDEX,CDI_GROUP_INDEX," _
        '        & "CM_COY_NAME,CDM_S_COY_ID,IFNULL(CDI_UOM,'') AS CDI_UOM,IFNULL(CDI_CURRENCY_CODE,'') AS CDI_CURRENCY_CODE," _
        '        & "IFNULL(CDI_UNIT_COST,0) AS CDI_UNIT_COST,IFNULL(CDI_GST,0) AS CDI_GST,IFNULL(CDI_REMARK,'') AS CDI_REMARK," _
        '        & "IFNULL(PM_CATEGORY_NAME,0) AS PM_CATEGORY_NAME,IFNULL(CT_NAME,'') AS CT_NAME,IFNULL(PM_ITEM_TYPE,'') AS PM_ITEM_TYPE, " _
        '        & "IFNULL(CDM_GROUP_DESC,'') AS CDM_GROUP_DESC, IF(CDI_GST_TAX_CODE IS NULL OR CDI_GST_TAX_CODE = '', 'N/A',CDI_GST_TAX_CODE) AS CDI_GST_TAX_CODE, " _
        '        & "CASE WHEN CDI_GST_RATE = 'N/A' THEN CDI_GST_RATE ELSE " _
        '        & "IF((TAX_PERC IS NULL OR TAX_PERC = ''), IFNULL(CODE_DESC,'N/A'), CONCAT(CODE_DESC, '(', TAX_PERC,'%)')) END AS CDI_GST_RATE " _
        '        & "FROM CONTRACT_DIST_COY " _
        '        & "INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' " _
        '        & "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX " _
        '        & "INNER JOIN contract_dist_user ON cdu_group_index = CDM_GROUP_INDEX " _
        '        & "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND PM_DELETED <> 'Y' " _
        '        & "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME " _
        '        & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CDI_GST_RATE " _
        '        & "LEFT JOIN TAX ON TAX_CODE = CDI_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND cdu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' " _
        '        & "AND CDM_TYPE = 'C' AND CDM_START_DATE<= CURRENT_DATE()  AND CDM_END_DATE>=CURRENT_DATE() " _
        '        & "AND PM_S_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"

        '    '//Vendor Item Code
        '    If strItemCode <> "" Then
        '        strsql &= " AND CDI_VENDOR_ITEM_CODE LIKE '%" & Common.Parse(strItemCode) & "%' "
        '    End If

        '    '//Product Desc
        '    If strItemName <> "" Then
        '        strsql &= " AND CDI_PRODUCT_DESC LIKE '%" & Common.Parse(strItemName) & "%' "
        '    End If

        '    'Contract catalogue description
        '    If catDesc <> "" Then
        '        strsql &= " AND CDM_GROUP_DESC = '" & Common.Parse(catDesc) & "'"
        '    End If

        '    '//Contract Ref. No.
        '    If intIndex > 0 Then
        '        strsql &= " AND CDM_GROUP_INDEX = " & intIndex
        '    End If

        '    If strVendor <> "" Then
        '        strsql &= " AND CDM_S_COY_ID = '" & strVendor & "' "
        '    End If

        '    '//Commodity Type
        '    If strCommType <> "" Then
        '        strsql &= " AND PM_CATEGORY_NAME = '" & Common.Parse(strCommType) & "' "
        '    End If

        '    'Item Type
        '    If strItemType.Count > 0 Then
        '        For i As Integer = 0 To strItemType.Count - 1
        '            If strType = "" Then
        '                strType = "'" & Common.Parse(strItemType(i)) & "'"
        '            Else
        '                strType = strType & "," & "'" & Common.Parse(strItemType(i)) & "'"
        '            End If
        '        Next
        '        strType = "(" & strType & ")"
        '        If strType <> "" Then
        '            strsql &= " AND PM_ITEM_TYPE IN " & strType
        '        End If
        '    End If

        '    dsProduct = objDb.FillDs(strsql)
        '    Return dsProduct
        'End Function

        'Public Function getContractItemRemark(ByVal intIndex As Integer, ByVal strItemCode As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CDUR_Upload_Index, CDUR_Product_Code, CDUR_Remark_Id, CDUR_Remark, "
        '    strsql &= "CDUR_User_Id, CDUR_User_Role, CDUR_Created_DateTime, UM_USER_NAME "
        '    strsql &= "FROM CONT_DIST_UPLOADREMARK "
        '    strsql &= "LEFT JOIN USER_MSTR AS A ON A.UM_USER_ID = CDUR_User_Id AND CDUR_User_Coy_Id = UM_Coy_ID "
        '    strsql &= "WHERE CDUR_Upload_Index = " & intIndex & " "
        '    strsql &= "AND CDUR_Product_Code = '" & Common.Parse(strItemCode) & "'"
        '    ds = objDb.FillDs(strsql)
        '    getContractItemRemark = ds
        'End Function

        'Public Function updateContractStatus(ByVal strStatus As String, ByVal strCode As String, ByVal intIndex As Integer)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    strsql = "UPDATE CONT_DIST_UPLOADMSTR SET "
        '    strsql &= "CDUM_Upload_Status = '" & strStatus & "', "
        '    strsql &= "CDUM_Submit_Date = getdate() "
        '    strsql &= "WHERE CDUM_Contract_Code = '" & Common.Parse(strCode) & "' "
        '    strsql &= "AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "INSERT INTO CONT_DIST_UPLOADREMARK "
        '    strsql &= "(CDUR_Upload_Index, CDUR_Product_Code, CDUR_Remark_Id, CDUR_Remark, "
        '    strsql &= "CDUR_User_Id, CDUR_User_Coy_Id, CDUR_User_Role, CDUR_Created_DateTime) "
        '    strsql &= "SELECT DISTINCT CDUM_Upload_Index, CDUI_Product_Code, "
        '    strsql &= "(SELECT ISNULL(MAX(CDUR_Remark_Id),0) + 1 FROM CONT_DIST_UPLOADREMARK "
        '    strsql &= "WHERE CDUR_Upload_Index = " & intIndex & "), " ' CDUR_Remark_Id
        '    strsql &= "CDUI_Remark, " ' CDUR_Remark
        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', " ' CDUR_User_Id
        '    strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' CDUR_User_Coy_Id
        '    strsql &= "'V', getdate() FROM CONT_DIST_UPLOADMSTR " ' CDUR_User_Role,  CDUR_Created_DateTime
        '    strsql &= "LEFT JOIN CONT_DIST_UPLOADITEMS ON CDUM_Upload_Index = CDUI_Upload_Index "
        '    strsql &= "LEFT JOIN CONT_DIST_UPLOADREMARK ON CDUR_Upload_Index = CDUI_Upload_Index AND CDUR_Product_Code = CDUI_Product_Code "
        '    strsql &= "WHERE CDUM_Contract_Code = '" & Common.Parse(strCode) & "' AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "AND CDUI_Remark <> '' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "UPDATE CONT_DIST_UPLOADITEMS SET CDUI_Remark = '' "
        '    strsql &= "WHERE CDUI_Upload_Index = " & intIndex
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If objDb.BatchExecute(strAryQuery) Then
        '        Dim objMail As New Email
        '        strsql = "SELECT CDUD_B_Coy_Id FROM CONT_DIST_UPLOADCOY LEFT JOIN CONT_DIST_UPLOADMSTR ON CDUD_Upload_Index = CDUM_Upload_Index WHERE CDUM_Contract_Code = '" & strCode & "' AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "'"
        '        Dim tDS As DataSet = objDb.FillDs(strsql)
        '        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '            objMail.sendNotification(EmailType.CatalogueApproval, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), HttpContext.Current.Session("CompanyId"), strCode, "", "B")
        '        Next
        '        objMail = Nothing
        '    End If
        'End Function

        'Public Function updateContractStatusBy(ByVal strStatus As String, ByVal strCode As String, ByVal dtRemark As DataTable, ByVal strVendor As String, Optional ByVal blnReset As Boolean = True, Optional ByVal blnDiscard As Boolean = False)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim i As Integer

        '    strsql = "UPDATE CONT_DIST_UPLOADMSTR SET "
        '    strsql &= "CDUM_Upload_Status = '" & strStatus & "' "

        '    If strStatus = CatalogueStatus.BuyerRejected Then
        '        strsql &= ", CDUM_B_Reject_Cnt = CDUM_B_Reject_Cnt + 1 "
        '    ElseIf strStatus = CatalogueStatus.Rejected Then
        '        strsql &= ", CDUM_H_Reject_Cnt = CDUM_H_Reject_Cnt + 1 "
        '        If blnReset Then
        '            strsql &= ", CDUM_B_Reject_Cnt = 0 "
        '        End If
        '    End If

        '    strsql &= "WHERE CDUM_Contract_Code = '" & strCode & "' "
        '    strsql &= "AND CDUM_S_Coy_Id = '" & Common.Parse(strVendor) & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    For i = 0 To dtRemark.Rows.Count - 1
        '        If Common.Parse(dtRemark.Rows(i)("Remark")) <> "" Then
        '            strsql = insertContractItemRemarkSQL(dtRemark.Rows(i))
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    Next

        '    If strStatus = CatalogueStatus.Approved Then
        '        ' insert into CONTRACT_DIST_MSTR from CONT_DIST_UPLOADMSTR
        '        strsql = "INSERT INTO CONTRACT_DIST_MSTR "
        '        strsql &= "(CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, "
        '        strsql &= "CDM_TYPE, CDM_START_DATE, CDM_END_DATE) "
        '        strsql &= "SELECT CDUM_Contract_Code, CDUM_Contract_Desc, CDUM_S_Coy_Id, "
        '        strsql &= "CDUM_Type, CDUM_Start_Date, CDUM_End_Date "
        '        strsql &= "FROM CONT_DIST_UPLOADMSTR "
        '        strsql &= "WHERE CDUM_Upload_Index = " & dtRemark.Rows(0)("index") & " "
        '        strsql &= "AND CDUM_S_Coy_Id = '" & Common.Parse(strVendor) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' insert into CONTRACT_DIST_ITEMS from CONT_DIST_UPLOADITEMS
        '        strsql = "INSERT INTO CONTRACT_DIST_ITEMS "
        '        strsql &= "(CDI_GROUP_INDEX, CDI_PRODUCT_CODE, CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, "
        '        strsql &= "CDI_UOM, CDI_UNIT_COST, CDI_GST, CDI_CURRENCY_CODE) "
        '        strsql &= "SELECT (SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR), "
        '        strsql &= "CDUI_Product_Code, CDUI_Vendor_Item_Code, CDUI_Product_Desc, "
        '        strsql &= "CDUI_UOM, CDUI_Unit_Cost, CDUI_GST, CDUI_Currency_Code "
        '        strsql &= "FROM CONT_DIST_UPLOADITEMS "
        '        strsql &= "WHERE CDUI_Upload_Index = " & dtRemark.Rows(0)("index") & " "
        '        'strsql &= "AND CDUI_Product_Code = '" & Common.Parse(dtRemark.Rows(0)("ProductCode")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' insert into CONTRACT_DIST_COY from CONT_DIST_UPLOADCOY
        '        strsql = "INSERT INTO CONTRACT_DIST_COY "
        '        strsql &= "(CDC_GROUP_INDEX, CDC_B_COY_ID) "
        '        strsql &= "SELECT (SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR), "
        '        strsql &= "CDUD_B_Coy_Id FROM CONT_DIST_UPLOADCOY "
        '        strsql &= "WHERE CDUD_Upload_Index = " & dtRemark.Rows(0)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If

        '    If objDb.BatchExecute(strAryQuery) Then
        '        Dim objMail As New Email
        '        strsql = "SELECT CDUD_B_Coy_Id FROM CONT_DIST_UPLOADCOY LEFT JOIN CONT_DIST_UPLOADMSTR ON CDUD_Upload_Index = CDUM_Upload_Index WHERE CDUM_Contract_Code = '" & strCode & "' AND CDUM_S_Coy_Id = '" & Common.Parse(strVendor) & "'"
        '        Dim tDS As DataSet = objDb.FillDs(strsql)
        '        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '            Select Case strStatus
        '                Case "3" ' rejected by buyer
        '                    If blnDiscard Then
        '                        objMail.sendNotification(EmailType.CatalogueDiscarded, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "B", "V")
        '                    Else
        '                        objMail.sendNotification(EmailType.CatalogueRejected, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "B", "V")
        '                    End If

        '                Case "4" ' pending approval from hub admin
        '                    objMail.sendNotification(EmailType.CatalogueApproval, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "H", "")

        '                Case "5" ' rejected by hub admin
        '                    If blnDiscard Then
        '                        objMail.sendNotification(EmailType.CatalogueDiscarded, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "H", "B")
        '                        objMail.sendNotification(EmailType.CatalogueDiscarded, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "H", "V")
        '                    Else
        '                        objMail.sendNotification(EmailType.CatalogueRejected, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "H", "B")
        '                        objMail.sendNotification(EmailType.CatalogueRejected, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "H", "V")
        '                    End If

        '                Case "6" ' approved/published
        '                    objMail.sendNotification(EmailType.CataloguePublished, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "B", "")
        '                    objMail.sendNotification(EmailType.CataloguePublished, HttpContext.Current.Session("UserId"), tDS.Tables(0).Rows(j).Item("CDUD_B_Coy_Id"), strVendor, strCode, "", "V", "")
        '            End Select
        '        Next
        '        objMail = Nothing
        '    End If
        'End Function

        'Public Function deleteContractCatalogueItem(ByVal blnDeleteMaster As Boolean, ByVal dtItem As DataTable)
        '    Dim strsql As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String

        '    For i = 0 To dtItem.Rows.Count - 1
        '        ' delete from CONT_DIST_UPLOADITEMS
        '        strsql = "DELETE FROM CONT_DIST_UPLOADITEMS "
        '        strsql &= "WHERE CDUI_Upload_Index = " & dtItem.Rows(i)("index") & " "
        '        strsql &= "AND CDUI_Product_Code = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' delete from CONT_DIST_UPLOADREMARK
        '        strsql = "DELETE FROM CONT_DIST_UPLOADREMARK "
        '        strsql &= "WHERE CDUR_Upload_Index = " & dtItem.Rows(i)("index") & " "
        '        strsql &= "AND CDUR_Product_Code = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next

        '    If blnDeleteMaster Then
        '        ' delete from CONT_DIST_UPLOADMSTR
        '        strsql = "DELETE FROM CONT_DIST_UPLOADMSTR "
        '        strsql &= "WHERE CDUM_Contract_Code = '" & Common.Parse(dtItem.Rows(0)("ContractCode")) & "' "
        '        strsql &= "AND CDUM_S_Coy_Id = '" & HttpContext.Current.Session("CompanyId") & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' delete from CONT_DIST_UPLOADCOY
        '        strsql = "DELETE FROM CONT_DIST_UPLOADCOY "
        '        strsql &= "WHERE CDUD_Upload_Index = " & dtItem.Rows(0)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If
        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function deleteDraftContract(ByVal dtCatalogue As DataTable)
        '    Dim strsql As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        strsql = "DELETE FROM CONT_DIST_UPLOADMSTR "
        '        strsql &= "WHERE CDUM_Upload_Index = " & dtCatalogue.Rows(i)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' delete from CONT_DIST_UPLOADITEMS
        '        strsql = "DELETE FROM CONT_DIST_UPLOADITEMS "
        '        strsql &= "WHERE CDUI_Upload_Index = " & dtCatalogue.Rows(i)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' delete from CONT_DIST_UPLOADCOY
        '        strsql = "DELETE FROM CONT_DIST_UPLOADCOY "
        '        strsql &= "WHERE CDUD_Upload_Index = " & dtCatalogue.Rows(i)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' delete from CONT_DIST_UPLOADREMARK
        '        strsql = "DELETE FROM CONT_DIST_UPLOADREMARK "
        '        strsql &= "WHERE CDUR_Upload_Index = " & dtCatalogue.Rows(i)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next
        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function deleteContract(ByVal dtCatalogue As DataTable, ByVal strVendor As String) As Integer
        '    Dim strsql As String
        '    Dim strExists As String
        '    Dim i As Integer
        '    Dim strDeleted As String = ""
        '    Dim strAryQuery(0) As String

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        strExists = "SELECT PRD_PRODUCT_CODE FROM PR_DETAILS "
        '        strExists &= "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(i)("CoyId")) & "' "
        '        strExists &= "WHERE PRM_PR_STATUS IN (1,2,3,4) "
        '        strExists &= "AND PRD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '        strExists &= "AND PRD_PRODUCT_CODE IN  "
        '        strExists &= "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " )"
        '        If objDb.Exist(strExists) > 0 Then
        '            deleteContract = WheelMsgNum.NotDelete
        '            Exit Function
        '        Else
        '            strDeleted &= dtCatalogue.Rows(i)("index") & ","
        '            strsql = "DELETE FROM CONTRACT_DIST_MSTR "
        '            strsql &= "WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            ' delete from CONTRACT_DIST_ITEMS
        '            strsql = "DELETE FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            Common.Insert2Ary(strAryQuery, strsql) 'CONTRACT_DIST_COY

        '            ' delete from CONTRACT_DIST_COY
        '            strsql = "DELETE FROM CONTRACT_DIST_COY "
        '            strsql &= "WHERE CDC_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    Next

        '    If objDb.BatchExecute(strAryQuery) Then
        '        'Dim objMail As New WheelMail
        '        'Dim strAryDeleted() As String
        '        'strAryDeleted = strDeleted.Split(",")
        '        'For i = 0 To strAryDeleted.Length - 2
        '        '    objMail.sendNotification(EmailType.CatalogueDeleted, HttpContext.Current.Session("UserId"), "", strVendor, strAryDeleted(i), "")
        '        'Next
        '        'objMail = Nothing
        '        deleteContract = WheelMsgNum.Delete
        '    End If
        'End Function

        'Public Function deleteConContract(ByVal dtCatalogue As DataTable) As Integer
        '    Dim strsql As String
        '    Dim strExists As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        'strExists = "SELECT PRD_PRODUCT_CODE FROM PR_DETAILS "
        '        'strExists &= "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(i)("CoyId")) & "' "
        '        'strExists &= "WHERE PRM_PR_STATUS IN (1,2,3,4) "
        '        'strExists &= "AND PRD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '        'strExists &= "AND PRD_PRODUCT_CODE IN  "
        '        'strExists &= "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " )"
        '        strExists = "SELECT PRD_PRODUCT_CODE FROM PR_DETAILS " _
        '                & "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO " _
        '                & "AND PRM_COY_ID = PRD_COY_ID AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '                & "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " " _
        '                & "AND PRD_PRODUCT_CODE IN " _
        '                & "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
        '                & "LEFT JOIN contract_dist_mstr ON CDM_GROUP_INDEX=CDI_GROUP_INDEX " _
        '                & "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & ")"

        '        If objDb.Exist(strExists) > 0 Then
        '            deleteConContract = WheelMsgNum.NotDelete
        '            Exit Function
        '        Else
        '            strExists = "SELECT POD_PRODUCT_CODE FROM po_details " _
        '                    & "LEFT JOIN po_mstr ON POM_PO_NO = POD_PO_NO " _
        '                    & "AND POM_B_COY_ID = POD_COY_ID AND POM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '                    & "WHERE POM_PO_STATUS IN (0,7,8,9) AND POD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " " _
        '                    & "AND POD_PRODUCT_CODE IN " _
        '                    & "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
        '                    & "LEFT JOIN contract_dist_mstr ON CDM_GROUP_INDEX=CDI_GROUP_INDEX " _
        '                    & "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & ")"
        '            If objDb.Exist(strExists) > 0 Then
        '                deleteConContract = WheelMsgNum.NotDelete
        '                Exit Function
        '            End If

        '            'delete from CONTRACT_DIST_MSTR
        '            strsql = "DELETE FROM CONTRACT_DIST_MSTR "
        '            strsql &= "WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            'Insert into audit trail au_contract_mstr - delete
        '            strsql1 = "SELECT * FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            dtTemp = objDb.FillDt(strsql1)
        '            If dtTemp.Rows.Count > 0 Then
        '                InsertAuditTrail(dtCatalogue.Rows(i)("index"), "Contract Ref. No.", dtTemp.Rows(0)("CDM_GROUP_CODE"), "", "H", "D", strAryQuery)
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ( "
        '                'strsql &= dtCatalogue.Rows(i)("index") & ","
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_CODE")) & "', "
        '                'strsql &= "'H', "
        '                'strsql &= "'D', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '            End If
        '            'strsql1 = "SELECT * FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            'dtTemp = objDb.FillDt(strsql1)
        '            'If dtTemp.Rows.Count > 0 Then
        '            '    strsql = "INSERT INTO au_contract_mstr "
        '            '    strsql &= "(CDM_GROUP_INDEX,CDM_GROUP_CODE,CDM_GROUP_DESC,CDM_S_COY_ID,CDM_TYPE,CDM_START_DATE,CDM_END_DATE,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '            '    strsql &= dtCatalogue.Rows(i)("index") & ","
        '            '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_CODE")) & "', "
        '            '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_DESC")) & "', "
        '            '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_S_COY_ID")) & "', "
        '            '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_TYPE")) & "', "
        '            '    strsql &= Common.ConvertDate(dtTemp.Rows(0)("CDM_START_DATE")) & ", "
        '            '    strsql &= Common.ConvertDate(dtTemp.Rows(0)("CDM_END_DATE")) & ", "
        '            '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        '            '    strsql &= Common.ConvertDate(Now()) & ",'D')"
        '            '    Common.Insert2Ary(strAryQuery, strsql)
        '            'End If

        '            ' delete from CONTRACT_DIST_ITEMS
        '            strsql = "DELETE FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            strsql1 = "SELECT * FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            dtTemp = objDb.FillDt(strsql1)
        '            For j As Integer = 0 To dtTemp.Rows.Count - 1
        '                'Insert into audit trail au_contract_items - delete
        '                InsertAuditTrail(dtCatalogue.Rows(i)("index"), "Item Code", dtTemp.Rows(j)("CDI_VENDOR_ITEM_CODE"), "", "H", "D", strAryQuery)
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                'strsql &= dtCatalogue.Rows(i)("Index") & ","
        '                'strsql &= "'Item Code: " & Common.Parse(dtTemp.Rows(j)("CDI_VENDOR_ITEM_CODE")) & "', "
        '                'strsql &= "'H', "
        '                'strsql &= "'D', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '                'strsql = "INSERT INTO au_contract_items (CDI_GROUP_INDEX,CDI_PRODUCT_CODE,CDI_VENDOR_ITEM_CODE,CDI_PRODUCT_DESC,CDI_UOM,CDI_UNIT_COST,CDI_REMARK,CDI_GST,CDI_CURRENCY_CODE,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '                'strsql &= dtCatalogue.Rows(i)("index") & ","
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(j)("CDI_PRODUCT_CODE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(j)("CDI_VENDOR_ITEM_CODE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(j)("CDI_PRODUCT_DESC")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(j)("CDI_UOM")) & "', "
        '                'strsql &= dtTemp.Rows(j)("CDI_UNIT_COST") & ", "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(j)("CDI_REMARK")) & "', "
        '                'strsql &= dtTemp.Rows(j)("CDI_GST") & ", "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(j)("CDI_CURRENCY_CODE")) & "', "
        '                'strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'D')"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '            Next

        '            ' delete from CONTRACT_DIST_COY
        '            strsql = "DELETE FROM CONTRACT_DIST_COY "
        '            strsql &= "WHERE CDC_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            ''Insert into audit trail au_contract_coy - delete
        '            'strsql1 = "SELECT * FROM CONTRACT_DIST_COY WHERE CDC_GROUP_INDEX = " & dtCatalogue.Rows(i)("index")
        '            'dtTemp = objDb.FillDt(strsql1)
        '            'If dtTemp.Rows.Count > 0 Then
        '            '    strsql = "INSERT INTO au_contract "
        '            '    strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_MODULE,AU_ACTION,AU_USER,AU_DATE) VALUES ("
        '            '    strsql &= dtCatalogue.Rows(i)("Index") & ","
        '            '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDC_B_COY_ID")) & "', "
        '            '    strsql &= "'H', "
        '            '    strsql &= "'D', "
        '            '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        '            '    strsql &= Common.ConvertDate(Now()) & ")"
        '            '    Common.Insert2Ary(strAryQuery, strsql)
        '            '    'strsql = "INSERT INTO au_contract_coy (CDC_GROUP_INDEX,CDC_B_COY_ID,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '            '    'strsql &= dtCatalogue.Rows(i)("index") & ","
        '            '    'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDC_B_COY_ID")) & "', "
        '            '    'strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'D')"
        '            '    'Common.Insert2Ary(strAryQuery, strsql)
        '            'End If
        '        End If
        '    Next

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            'Dim objMail As New WheelMail
        '            'Dim strAryDeleted() As String
        '            'strAryDeleted = strDeleted.Split(",")
        '            'For i = 0 To strAryDeleted.Length - 2
        '            '    objMail.sendNotification(EmailType.CatalogueDeleted, HttpContext.Current.Session("UserId"), "", strVendor, strAryDeleted(i), "")
        '            'Next
        '            'objMail = Nothing
        '            deleteConContract = WheelMsgNum.Delete
        '        Else
        '            deleteConContract = WheelMsgNum.NotDelete
        '        End If
        '    End If

        'End Function

        'Public Function deteleCatalogueItem(ByVal blnDeleteMaster As Boolean, ByVal dtCatalogue As DataTable, ByRef strDeleted As String, ByRef strNotDeleted As String)
        '    Dim strsql As String
        '    Dim strExist As String ' item exist in PR
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String
        '    Dim blnDelete As Boolean = False
        '    strDeleted = ""
        '    strNotDeleted = ""

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        strExist = "SELECT '*' FROM PR_DETAILS LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(i)("CoyId")) & "' "
        '        strExist &= "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' "
        '        strExist &= "AND PRD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '        If objDb.Exist(strExist) > 0 Then
        '            strNotDeleted &= dtCatalogue.Rows(i)("ProductCode") & ", "
        '        Else
        '            strDeleted &= dtCatalogue.Rows(i)("ProductCode") & ", "
        '            blnDelete = True
        '            strsql = "DELETE FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '            strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "'; "
        '            'Michelle (eBiz/134/08) - Update Shopping Cart also
        '            strsql &= "DELETE FROM SHOPPING_CART WHERE "
        '            strsql &= "SC_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '            strsql &= "AND SC_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    Next

        '    If strNotDeleted = "" And blnDeleteMaster Then
        '        ' delete from CONTRACT_DIST_MSTR
        '        strsql = "DELETE FROM CONTRACT_DIST_MSTR "
        '        strsql &= "WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(0)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ' delete from CONTRACT_DIST_COY
        '        strsql = "DELETE FROM CONTRACT_DIST_COY "
        '        strsql &= "WHERE CDC_GROUP_INDEX = " & dtCatalogue.Rows(0)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If

        '    If blnDelete Then
        '        objDb.BatchExecute(strAryQuery)
        '    End If
        'End Function

        'Public Function deteleConCatalogueItem(ByVal blnDeleteMaster As Boolean, ByVal dtCatalogue As DataTable) As Integer
        '    'Added by Joon on 8th June 2011
        '    Dim strsql As String
        '    Dim strPRExist As String ' item exist in PR
        '    Dim strPOExist As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String
        '    Dim blnDelete As Boolean = False
        '    Dim strDeleted As String
        '    Dim strNotDeleted As String
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    strDeleted = ""
        '    strNotDeleted = ""

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        'strExist = "SELECT '*' FROM PR_DETAILS LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(i)("CoyId")) & "' "
        '        'strExist &= "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' "
        '        'strExist &= "AND PRD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '        strPRExist = "SELECT '*' FROM PR_DETAILS " _
        '                & "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO " _
        '                & "AND PRM_COY_ID = PRD_COY_ID AND PRM_COY_ID = '" & Common.Parse(dtCatalogue.Rows(i)("CoyId")) & "' " _
        '                & "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' " _
        '                & "And PRD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " " _
        '                & "AND PRD_PRODUCT_CODE IN " _
        '                & "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
        '                & "LEFT JOIN contract_dist_mstr ON CDM_GROUP_INDEX=CDI_GROUP_INDEX " _
        '                & "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & ")"

        '        strPOExist = "SELECT '*' FROM po_details " _
        '                & "LEFT JOIN po_mstr ON POM_PO_NO = POD_PO_NO " _
        '                & "AND POM_B_COY_ID = POD_COY_ID AND POM_B_COY_ID = '" & Common.Parse(dtCatalogue.Rows(i)("CoyId")) & "' " _
        '                & "WHERE POM_PO_STATUS IN (0,7,8,9) AND POD_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' " _
        '                & "And POD_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " " _
        '                & "AND POD_PRODUCT_CODE IN " _
        '                & "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
        '                & "LEFT JOIN contract_dist_mstr ON CDM_GROUP_INDEX=CDI_GROUP_INDEX " _
        '                & "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & ")"

        '        If objDb.Exist(strPRExist) > 0 Or objDb.Exist(strPOExist) > 0 Then
        '            strNotDeleted &= dtCatalogue.Rows(i)("ProductCode") & ", "
        '            deteleConCatalogueItem = WheelMsgNum.NotDelete
        '            Exit Function

        '        Else
        '            strDeleted &= dtCatalogue.Rows(i)("ProductCode") & ", "
        '            blnDelete = True
        '            strsql = "DELETE FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '            strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "'"
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            'Insert into audit trail au_contract_items - delete
        '            strsql1 = "SELECT * FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " AND CDI_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "'"
        '            dtTemp = objDb.FillDt(strsql1)
        '            If dtTemp.Rows.Count > 0 Then
        '                InsertAuditTrail(dtCatalogue.Rows(i)("Index"), "Item Code", dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE"), "", "I", "D", strAryQuery)
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                'strsql &= dtCatalogue.Rows(i)("Index") & ","
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & "', "
        '                'strsql &= "'I', "
        '                'strsql &= "'D', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '                'strsql = "INSERT INTO au_contract_items (CDI_GROUP_INDEX,CDI_PRODUCT_CODE,CDI_VENDOR_ITEM_CODE,CDI_PRODUCT_DESC,CDI_UOM,CDI_UNIT_COST,CDI_REMARK,CDI_GST,CDI_CURRENCY_CODE,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '                'strsql &= dtCatalogue.Rows(i)("index") & ","
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_PRODUCT_CODE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_PRODUCT_DESC")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_UOM")) & "', "
        '                'strsql &= dtTemp.Rows(0)("CDI_UNIT_COST") & ", "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_REMARK")) & "', "
        '                'strsql &= dtTemp.Rows(0)("CDI_GST") & ", "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_CURRENCY_CODE")) & "', "
        '                'strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'D')"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '            End If

        '            ''Michelle (eBiz/134/08) - Update Shopping Cart also
        '            'strsql &= "DELETE FROM SHOPPING_CART WHERE "
        '            'strsql &= "SC_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " "
        '            'strsql &= "AND SC_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' "
        '            'Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    Next

        '    If strNotDeleted = "" And blnDeleteMaster Then
        '        ' delete from CONTRACT_DIST_MSTR
        '        strsql = "DELETE FROM CONTRACT_DIST_MSTR "
        '        strsql &= "WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(0)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        'Insert into audit trail au_contract_mstr - delete
        '        strsql1 = "SELECT * FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(0)("index")
        '        dtTemp = objDb.FillDt(strsql1)
        '        If dtTemp.Rows.Count > 0 Then
        '            InsertAuditTrail(dtCatalogue.Rows(0)("index"), "Contract Ref. No.", dtTemp.Rows(0)("CDM_GROUP_CODE"), "", "I", "D", strAryQuery)
        '            'strsql = "INSERT INTO au_contract "
        '            'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ( "
        '            'strsql &= dtCatalogue.Rows(0)("index") & ","
        '            'strsql &= "'Contract Ref. No.: " & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_CODE")) & "', "
        '            'strsql &= "'I', "
        '            'strsql &= "'D', "
        '            'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '            'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '            'strsql &= Common.ConvertDate(Now()) & ")"
        '            'Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '        'strsql1 = "SELECT * FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(0)("index")
        '        'dtTemp = objDb.FillDt(strsql1)
        '        'If dtTemp.Rows.Count > 0 Then
        '        '    strsql = "INSERT INTO au_contract_mstr "
        '        '    strsql &= "(CDM_GROUP_INDEX,CDM_GROUP_CODE,CDM_GROUP_DESC,CDM_S_COY_ID,CDM_TYPE,CDM_START_DATE,CDM_END_DATE,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '        '    strsql &= dtCatalogue.Rows(i)("index") & ","
        '        '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_CODE")) & "', "
        '        '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_DESC")) & "', "
        '        '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_S_COY_ID")) & "', "
        '        '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_TYPE")) & "', "
        '        '    strsql &= Common.ConvertDate(dtTemp.Rows(0)("CDM_START_DATE")) & ", "
        '        '    strsql &= Common.ConvertDate(dtTemp.Rows(0)("CDM_END_DATE")) & ", "
        '        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        '        '    strsql &= Common.ConvertDate(Now()) & ",'D')"
        '        '    Common.Insert2Ary(strAryQuery, strsql)
        '        'End If

        '        ' delete from CONTRACT_DIST_COY
        '        strsql = "DELETE FROM CONTRACT_DIST_COY "
        '        strsql &= "WHERE CDC_GROUP_INDEX = " & dtCatalogue.Rows(0)("index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        ''Insert into audit trail au_contract_coy - delete
        '        'strsql1 = "SELECT * FROM CONTRACT_DIST_COY WHERE CDC_GROUP_INDEX = " & dtCatalogue.Rows(0)("index")
        '        'dtTemp = objDb.FillDt(strsql1)
        '        'If dtTemp.Rows.Count > 0 Then
        '        '    strsql = "INSERT INTO au_contract_coy (CDC_GROUP_INDEX,CDC_B_COY_ID,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '        '    strsql &= dtCatalogue.Rows(i)("index") & ","
        '        '    strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDC_B_COY_ID")) & "', "
        '        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'D')"
        '        '    Common.Insert2Ary(strAryQuery, strsql)
        '        'End If
        '    End If

        '    If blnDelete Then
        '        If strAryQuery(0) <> String.Empty Then
        '            If objDb.BatchExecute(strAryQuery) Then
        '                deteleConCatalogueItem = WheelMsgNum.Delete
        '            Else
        '                deteleConCatalogueItem = WheelMsgNum.NotDelete
        '            End If
        '        End If

        '    End If
        'End Function

        'Public Function insertCatalogue(ByVal dtCatalogue As DataTable) As Integer
        '    Dim strsql As String

        '    If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_CODE = '" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "' AND CDM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "' ") > 0 Then
        '        insertCatalogue = WheelMsgNum.Duplicate
        '        Exit Function
        '    End If

        '    strsql = "INSERT INTO CONTRACT_DIST_MSTR "
        '    strsql &= "(CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, CDM_START_DATE, CDM_END_DATE, "
        '    strsql &= "CDM_ENT_BY, CDM_ENT_DT) VALUES ("
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "', "
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("CatType")) & "', "
        '    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("StartDate")) & ", "
        '    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("EndDate")) & ", "
        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', getdate()) "

        '    If Common.Parse(dtCatalogue.Rows(0)("CatType")) = "C" Then
        '        strsql &= "INSERT INTO CONTRACT_DIST_COY (CDC_GROUP_INDEX, CDC_B_COY_ID) SELECT "
        '        strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR), "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("BuyerID")) & "' "
        '    End If

        '    If objDb.Execute(strsql) Then
        '        insertCatalogue = WheelMsgNum.Save
        '    Else
        '        insertCatalogue = WheelMsgNum.NotSave
        '    End If
        'End Function

        'Public Function insertConCatalogue(ByVal dtCatalogue As DataTable) As Integer
        '    'Added by Joon on 6th June 2011
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String

        '    'If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_CODE = '" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "' AND CDM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "' ") > 0 Then
        '    If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_MSTR LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX WHERE CDM_GROUP_CODE = '" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "' AND CDM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "' AND CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
        '        insertConCatalogue = WheelMsgNum.Duplicate
        '        Exit Function
        '    End If

        '    strsql = "INSERT INTO CONTRACT_DIST_MSTR "
        '    strsql &= "(CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, CDM_START_DATE, CDM_END_DATE, "
        '    strsql &= "CDM_ENT_BY, CDM_ENT_DT) VALUES ("
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "', "
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("CatType")) & "', "
        '    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("StartDate")) & ", "
        '    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("EndDate")) & ", "
        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ")"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    'Insert into audit trail - add
        '    'strsql = "INSERT INTO au_contract "
        '    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) SELECT "
        '    'strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR),"
        '    'strsql &= "'Contract Ref. No.',"
        '    'strsql &= "NULL,"
        '    'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '    'strsql &= "'H', "
        '    'strsql &= "'A', "
        '    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '    'strsql &= Common.ConvertDate(Now())
        '    'Common.Insert2Ary(strAryQuery, strsql)
        '    '--------------------------------------
        '    'strsql = "INSERT INTO au_contract_mstr "
        '    'strsql &= "(CDM_GROUP_INDEX,CDM_GROUP_CODE,CDM_GROUP_DESC,CDM_S_COY_ID,CDM_TYPE,CDM_START_DATE,CDM_END_DATE,AU_USER,AU_DATE,AU_ACTION) SELECT "
        '    'strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR),"
        '    'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '    'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        '    'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "', "
        '    'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("CatType")) & "', "
        '    'strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("StartDate")) & ", "
        '    'strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("EndDate")) & ", "
        '    'strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        '    'strsql &= Common.ConvertDate(Now()) & ",'A'"
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "INSERT INTO CONTRACT_DIST_COY (CDC_GROUP_INDEX, CDC_B_COY_ID) SELECT "
        '    strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR), "
        '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("BuyerID")) & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    'Insert into audit trail - add
        '    'Dim intGID As Integer
        '    'intGID = getLatestIndex("DC")
        '    InsertAuditTrail(0, "Contract Ref. No.", "", dtCatalogue.Rows(0)("GroupCode"), "H", "A", strAryQuery, True)
        '    ''Insert into audit trail - add
        '    'strsql = "INSERT INTO au_contract_coy (CDC_GROUP_INDEX,CDC_B_COY_ID,AU_USER,AU_DATE,AU_ACTION) SELECT "
        '    'strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR),"
        '    'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("BuyerID")) & "', "
        '    'strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'A'"
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            insertConCatalogue = WheelMsgNum.Save
        '        Else
        '            insertConCatalogue = WheelMsgNum.NotSave
        '        End If
        '    End If

        'End Function

        ''Public Function insertConCatAuditTrail(ByVal dtCatalogue As DataTable, ByVal intGID As Integer, ByVal chrAction As Char) As Integer
        ''    'Added by Joon on 28th July 2011
        ''    Dim strsql As String
        ''    Dim strAryQuery(0) As String

        ''    strsql = "INSERT INTO au_contract_mstr "
        ''    strsql &= "(CDM_GROUP_INDEX,CDM_GROUP_CODE,CDM_GROUP_DESC,CDM_S_COY_ID,CDM_TYPE,CDM_START_DATE,CDM_END_DATE,AU_USER,AU_DATE,AU_ACTION) "
        ''    strsql &= " VALUES ("
        ''    strsql &= intGID & ","
        ''    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        ''    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        ''    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "', "
        ''    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("CatType")) & "', "
        ''    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("StartDate")) & ", "
        ''    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("EndDate")) & ", "
        ''    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        ''    strsql &= Common.ConvertDate(Now()) & ",'" & chrAction & "')"
        ''    Common.Insert2Ary(strAryQuery, strsql)

        ''    If chrAction = "A" Then 'Add Contract Catalogue
        ''        strsql = "INSERT INTO au_contract_coy (CDC_GROUP_INDEX,CDC_B_COY_ID,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        ''        strsql &= intGID & ","
        ''        strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("BuyerID")) & "', "
        ''        strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'" & chrAction & "')"
        ''        Common.Insert2Ary(strAryQuery, strsql)
        ''    End If


        ''    If objDb.BatchExecute(strAryQuery) Then
        ''        insertConCatAuditTrail = WheelMsgNum.Save
        ''    Else
        ''        insertConCatAuditTrail = WheelMsgNum.NotSave
        ''    End If
        ''End Function

        'Public Function updateConCatalogue(ByVal dtCatalogue As DataTable, ByVal strOldValue As String) As Integer
        '    'Added by Joon on 6th June 2011
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    If strOldValue <> dtCatalogue.Rows(0)("GroupCode") Then
        '        If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_MSTR LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX WHERE CDM_GROUP_CODE = '" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "' AND CDM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "' AND CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'") > 0 Then
        '            updateConCatalogue = WheelMsgNum.Duplicate
        '            Exit Function
        '        End If
        '    End If

        '    strsql = "UPDATE CONTRACT_DIST_MSTR SET "
        '    strsql &= "CDM_GROUP_CODE = '" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '    strsql &= "CDM_GROUP_DESC = '" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        '    strsql &= "CDM_START_DATE = " & Common.ConvertDate(dtCatalogue.Rows(0)("StartDate")) & ", "
        '    strsql &= "CDM_END_DATE = " & Common.ConvertDate(dtCatalogue.Rows(0)("EndDate")) & ", "
        '    strsql &= "CDM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "', "
        '    strsql &= "CDM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
        '    strsql &= "CDM_MOD_DT = " & Common.ConvertDate(Now()) & " "
        '    strsql &= "WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(0)("Index")
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    'Insert into audit trail - Edit
        '    strsql1 = "SELECT * FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(0)("Index")
        '    dtTemp = objDb.FillDt(strsql1)
        '    If dtTemp.Rows.Count > 0 Then
        '        If dtCatalogue.Rows(0)("GroupCode") <> dtTemp.Rows(0)("CDM_GROUP_CODE") Or
        '           dtCatalogue.Rows(0)("GroupDesc") <> dtTemp.Rows(0)("CDM_GROUP_DESC") Or
        '           dtCatalogue.Rows(0)("VendorID") <> dtTemp.Rows(0)("CDM_S_COY_ID") Or
        '           dtCatalogue.Rows(0)("StartDate") <> dtTemp.Rows(0)("CDM_START_DATE") Or
        '           dtCatalogue.Rows(0)("EndDate") <> dtTemp.Rows(0)("CDM_END_DATE") Then

        '            If dtCatalogue.Rows(0)("GroupCode") <> dtTemp.Rows(0)("CDM_GROUP_CODE") Then
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                'strsql &= dtCatalogue.Rows(0)("Index") & ","
        '                'strsql &= "'Contract Ref. No.',"
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_CODE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '                'strsql &= "'H', "
        '                'strsql &= "'E', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '                InsertAuditTrail(dtCatalogue.Rows(0)("Index"), "Contract Ref. No.", dtTemp.Rows(0)("CDM_GROUP_CODE"), dtCatalogue.Rows(0)("GroupCode"), "H", "E", strAryQuery)
        '            End If
        '            If dtCatalogue.Rows(0)("GroupDesc") <> dtTemp.Rows(0)("CDM_GROUP_DESC") Then
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                'strsql &= dtCatalogue.Rows(0)("Index") & ","
        '                'strsql &= "'Contract Desc.',"
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_GROUP_DESC")) & "', "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        '                'strsql &= "'H', "
        '                'strsql &= "'E', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '                InsertAuditTrail(dtCatalogue.Rows(0)("Index"), "Contract Desc.", dtTemp.Rows(0)("CDM_GROUP_DESC"), dtCatalogue.Rows(0)("GroupDesc"), "H", "E", strAryQuery)
        '            End If
        '            If dtCatalogue.Rows(0)("VendorID") <> dtTemp.Rows(0)("CDM_S_COY_ID") Then
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                'strsql &= dtCatalogue.Rows(0)("Index") & ","
        '                'strsql &= "'Vendor Comp.',"
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_S_COY_ID")) & "', "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "', "
        '                'strsql &= "'H', "
        '                'strsql &= "'E', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '                InsertAuditTrail(dtCatalogue.Rows(0)("Index"), "Vendor Comp.", dtTemp.Rows(0)("CDM_S_COY_ID"), dtCatalogue.Rows(0)("VendorID"), "H", "E", strAryQuery)
        '            End If
        '            If dtCatalogue.Rows(0)("StartDate") <> dtTemp.Rows(0)("CDM_START_DATE") Then
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                'strsql &= dtCatalogue.Rows(0)("Index") & ","
        '                'strsql &= "'Contract Start Date',"
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_START_DATE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("StartDate")) & "', "
        '                'strsql &= "'H', "
        '                'strsql &= "'E', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '                InsertAuditTrail(dtCatalogue.Rows(0)("Index"), "Contract Start Date", dtTemp.Rows(0)("CDM_START_DATE"), dtCatalogue.Rows(0)("StartDate"), "H", "E", strAryQuery)
        '            End If
        '            If dtCatalogue.Rows(0)("EndDate") <> dtTemp.Rows(0)("CDM_END_DATE") Then
        '                'strsql = "INSERT INTO au_contract "
        '                'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                'strsql &= dtCatalogue.Rows(0)("Index") & ","
        '                'strsql &= "'Contract End Date',"
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDM_END_DATE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("EndDate")) & "', "
        '                'strsql &= "'H', "
        '                'strsql &= "'E', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                'strsql &= Common.ConvertDate(Now()) & ")"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '                InsertAuditTrail(dtCatalogue.Rows(0)("Index"), "Contract End Date", dtTemp.Rows(0)("CDM_END_DATE"), dtCatalogue.Rows(0)("EndDate"), "H", "E", strAryQuery)
        '            End If
        '            'strsql = "INSERT INTO au_contract_mstr "
        '            'strsql &= "(CDM_GROUP_INDEX,CDM_GROUP_CODE,CDM_GROUP_DESC,CDM_S_COY_ID,CDM_TYPE,CDM_START_DATE,CDM_END_DATE,AU_USER,AU_DATE,AU_ACTION) SELECT "
        '            'strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR),"
        '            'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '            'If dtCatalogue.Rows(0)("GroupDesc") <> dtTemp.Rows(0)("CDM_GROUP_DESC") Then
        '            '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        '            'Else
        '            '    strsql &= "NULL, "
        '            'End If
        '            'If dtCatalogue.Rows(0)("VendorID") <> dtTemp.Rows(0)("CDM_S_COY_ID") Then
        '            '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "', "
        '            'Else
        '            '    strsql &= "NULL, "
        '            'End If
        '            'strsql &= "'" & Common.Parse(dtCatalogue.Rows(0)("CatType")) & "', "
        '            'If dtCatalogue.Rows(0)("StartDate") <> dtTemp.Rows(0)("CDM_START_DATE") Then
        '            '    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("StartDate")) & ", "
        '            'Else
        '            '    strsql &= "NULL, "
        '            'End If
        '            'If dtCatalogue.Rows(0)("EndDate") <> dtTemp.Rows(0)("CDM_END_DATE") Then
        '            '    strsql &= Common.ConvertDate(dtCatalogue.Rows(0)("EndDate")) & ", "
        '            'Else
        '            '    strsql &= "NULL, "
        '            'End If
        '            'strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        '            'strsql &= Common.ConvertDate(Now()) & ",'E'"
        '            'Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    End If

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            updateConCatalogue = WheelMsgNum.Save
        '        Else
        '            updateConCatalogue = WheelMsgNum.NotSave
        '        End If
        '    End If

        'End Function

        'Public Function getLatestIndex(ByVal strType As String) As Integer
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    If strType = "DC" Then
        '        strsql = "SELECT ISNULL(MAX(CDM_GROUP_INDEX), 1) FROM CONTRACT_DIST_MSTR "
        '    Else
        '        strsql = "SELECT ISNULL(MAX(CDUM_Upload_Index), 1) FROM CONT_DIST_UPLOADMSTR "
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        getLatestIndex = ds.Tables(0).Rows(0)(0)
        '    Else
        '        getLatestIndex = 1
        '    End If
        'End Function

        'Public Function getLatestIndex1() As Integer
        '    Dim strsql As String
        '    'Dim ds As New DataSet
        '    Dim test1 As New EAD.DBCom
        '    strsql = "SELECT IFNULL(MAX(CDM_GROUP_INDEX), 1) FROM CONTRACT_DIST_MSTR "

        '    If test1.GetVal(strsql) <> "" Then
        '        getLatestIndex1 = test1.GetVal(strsql)

        '    Else
        '        getLatestIndex1 = 1
        '    End If

        '    'ds = objDb.FillDs(strsql)
        '    'If ds.Tables(0).Rows.Count > 0 Then
        '    '    getLatestIndex = ds.Tables(0).Rows(0)(0)
        '    'Else
        '    '    getLatestIndex = 1
        '    'End If
        'End Function


        'Public Function updateCatalogue(ByVal dtCatalogue As DataTable, ByVal strOldValue As String) As Integer
        '    Dim strsql As String

        '    If strOldValue <> dtCatalogue.Rows(0)("GroupCode") Then
        '        If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_CODE = '" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "' AND CDM_S_COY_ID = '" & Common.Parse(dtCatalogue.Rows(0)("VendorID")) & "' ") > 0 Then
        '            updateCatalogue = WheelMsgNum.Duplicate
        '            Exit Function
        '        End If
        '    End If

        '    strsql = "UPDATE CONTRACT_DIST_MSTR SET "
        '    strsql &= "CDM_GROUP_CODE = '" & Common.Parse(dtCatalogue.Rows(0)("GroupCode")) & "', "
        '    strsql &= "CDM_GROUP_DESC = '" & Common.Parse(dtCatalogue.Rows(0)("GroupDesc")) & "', "
        '    strsql &= "CDM_START_DATE = " & Common.ConvertDate(dtCatalogue.Rows(0)("StartDate")) & ", "
        '    strsql &= "CDM_END_DATE = " & Common.ConvertDate(dtCatalogue.Rows(0)("EndDate")) & ", "
        '    strsql &= "CDM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
        '    strsql &= "CDM_MOD_DT = getdate() "
        '    strsql &= "WHERE CDM_GROUP_INDEX = " & dtCatalogue.Rows(0)("Index") & "; "

        '    If Common.Parse(dtCatalogue.Rows(0)("CatType")) = "C" Then
        '        strsql &= "UPDATE CONTRACT_DIST_COY SET "
        '        strsql &= "CDC_B_COY_ID = '" & Common.Parse(dtCatalogue.Rows(0)("BuyerID")) & "' "
        '        strsql &= "WHERE CDC_GROUP_INDEX = " & dtCatalogue.Rows(0)("Index")
        '    End If

        '    If objDb.Execute(strsql) Then
        '        updateCatalogue = WheelMsgNum.Save
        '    Else
        '        updateCatalogue = WheelMsgNum.NotSave
        '    End If
        'End Function

        'Public Function InsertAuditTrail(ByVal intGID As Integer, ByVal strDesc As String, ByVal strBefore As String, ByVal strAfter As String, ByVal chrModule As Char, ByVal chrAction As Char, ByRef pQuery() As String, Optional ByVal blnGetLastIndex As Boolean = False)
        '    Dim strsql As String

        '    If blnGetLastIndex = True Then
        '        strsql = "INSERT INTO au_contract "
        '        strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) SELECT "
        '        strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR),"
        '        strsql &= "'" & Common.Parse(strDesc) & "', "
        '        strsql &= "'" & Common.Parse(strBefore) & "', "
        '        strsql &= "'" & Common.Parse(strAfter) & "', "
        '        strsql &= "'" & chrModule & "', "
        '        strsql &= "'" & chrAction & "', "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '        strsql &= Common.ConvertDate(Now())
        '    Else
        '        strsql = "INSERT INTO au_contract "
        '        strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '        strsql &= intGID & ","
        '        strsql &= "'" & Common.Parse(strDesc) & "', "
        '        strsql &= "'" & Common.Parse(strBefore) & "', "
        '        strsql &= "'" & Common.Parse(strAfter) & "', "
        '        strsql &= "'" & chrModule & "', "
        '        strsql &= "'" & chrAction & "', "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '        strsql &= Common.ConvertDate(Now()) & ")"
        '    End If
        '    Common.Insert2Ary(pQuery, strsql)
        '    'objDb.Execute(strsql)
        'End Function

        'Public Function insertCatalogueItem(ByVal dtCatalogue As DataTable, Optional ByVal strSCoyId As String = "") As Integer
        '    'Modified by Joon on 9th June 2011
        '    Dim strsql As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String
        '    Dim strGroupIndex As String = ""
        '    'Dim dt1 As New DataTable
        '    'Dim dt2 As New DataTable
        '    'Dim blnDiscount As Boolean = False

        '    'dt1.Columns.Add("ItemId", Type.GetType("System.String"))
        '    'dt1.Columns.Add("ItemDesc", Type.GetType("System.String"))

        '    'dt2.Columns.Add("ItemId", Type.GetType("System.String"))
        '    'dt2.Columns.Add("ItemDesc", Type.GetType("System.String"))
        '    'dt2.Columns.Add("List", Type.GetType("System.String"))
        '    'dt2.Columns.Add("Discount", Type.GetType("System.String"))

        '    'Dim dtr As DataRow

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        strsql = "INSERT INTO CONTRACT_DIST_ITEMS "
        '        strsql &= "(CDI_GROUP_INDEX, CDI_PRODUCT_CODE, CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, "
        '        strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST, CDI_GST_RATE, CDI_GST_TAX_CODE, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) VALUES ("
        '        strsql &= dtCatalogue.Rows(i)("Index") & ", "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "', "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("VendorItemCode")) & "', "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("ProductDesc")) & "', "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("UOM")) & "', "
        '        strsql &= dtCatalogue.Rows(i)("UnitCost") & ", "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("Remark")) & "', "
        '        strsql &= Common.Parse(dtCatalogue.Rows(i)("GST")) & ", "
        '        'Jules 2014.07.17 GST Enhancement begin
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("GSTRate")) & "', "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("GSTTaxCode")) & "', "
        '        'Jules 2014.07.17 GST Enhancement end.
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("CurrencyCode")) & "', "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "', " & Common.ConvertDate(Now()) & ")"
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        'Insert into audit trail au_contract_items - add               
        '        InsertAuditTrail(dtCatalogue.Rows(i)("Index"), "Item Code", "", dtCatalogue.Rows(i)("VendorItemCode"), "I", "A", strAryQuery)
        '        'strsql = "INSERT INTO au_contract "
        '        'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '        'strsql &= dtCatalogue.Rows(i)("Index") & ", "
        '        'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("VendorItemCode")) & "', "
        '        'strsql &= "'I', "
        '        'strsql &= "'A', "
        '        'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '        'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '        'strsql &= Common.ConvertDate(Now()) & ")"
        '        'Common.Insert2Ary(strAryQuery, strsql)
        '        'strsql = "INSERT INTO au_contract_items (CDI_GROUP_INDEX,CDI_PRODUCT_CODE,CDI_VENDOR_ITEM_CODE,CDI_PRODUCT_DESC,CDI_UOM,CDI_UNIT_COST,CDI_REMARK,CDI_GST,CDI_CURRENCY_CODE,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '        'strsql &= dtCatalogue.Rows(i)("Index") & ", "
        '        'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "', "
        '        'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("VendorItemCode")) & "', "
        '        'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("ProductDesc")) & "', "
        '        'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("UOM")) & "', "
        '        'strsql &= dtCatalogue.Rows(i)("UnitCost") & ", "
        '        'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("Remark")) & "', "
        '        'strsql &= dtCatalogue.Rows(i)("GST") & ", "
        '        'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("CurrencyCode")) & "', "
        '        'strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'A')"
        '        'Common.Insert2Ary(strAryQuery, strsql)
        '        'If dtCatalogue.Rows(i)("Change") = 1 Then
        '        '    blnDiscount = True
        '        '    dtr = dt2.NewRow()
        '        '    strGroupIndex &= dtCatalogue.Rows(i)("Index") & ","
        '        '    dtr("ItemId") = Common.Parse(dtCatalogue.Rows(i)("ProductCode"))
        '        '    dtr("ItemDesc") = Common.Parse(dtCatalogue.Rows(i)("ProductDesc"))
        '        '    dtr("List") = "No"
        '        '    dtr("Discount") = "Yes"
        '        '    dt2.Rows.Add(dtr)
        '        'End If
        '    Next

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            insertCatalogueItem = WheelMsgNum.Save

        '        Else
        '            insertCatalogueItem = WheelMsgNum.NotSave
        '            'If blnDiscount Then
        '            '    Dim objMail As New Email
        '            '    If strGroupIndex <> "" Then
        '            '        strGroupIndex = strGroupIndex.Substring(0, strGroupIndex.Length - 1)
        '            '    End If
        '            '    objMail.sendItemUpdateMail(strSCoyId, strGroupIndex, dt1, dt2, False, True)
        '            '    objMail = Nothing
        '            'End If
        '        End If
        '    End If

        'End Function

        'Public Function updateCatalogueItem(ByVal dtCatalogue As DataTable, Optional ByVal strSCoyId As String = "")
        '    'Modified by Joon on 9th June 2011
        '    Dim strsql As String
        '    Dim i As Integer
        '    Dim strGroupIndex As String = ""
        '    Dim strAryQuery(0) As String
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    'Dim blnDiscount As Boolean = False
        '    'Dim dt1 As New DataTable
        '    'Dim dt2 As New DataTable

        '    'dt1.Columns.Add("ItemId", Type.GetType("System.String"))
        '    'dt1.Columns.Add("ItemDesc", Type.GetType("System.String"))

        '    'dt2.Columns.Add("ItemId", Type.GetType("System.String"))
        '    'dt2.Columns.Add("ItemDesc", Type.GetType("System.String"))
        '    'dt2.Columns.Add("List", Type.GetType("System.String"))
        '    'dt2.Columns.Add("Discount", Type.GetType("System.String"))

        '    'Dim dtr As DataRow

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        strsql = "UPDATE CONTRACT_DIST_ITEMS SET "
        '        strsql &= "CDI_CURRENCY_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("CurrencyCode")) & "', "
        '        strsql &= "CDI_UNIT_COST = " & dtCatalogue.Rows(i)("UnitCost") & ", "
        '        strsql &= "CDI_GST = " & dtCatalogue.Rows(i)("GST") & ", "
        '        'Jules 2014.07.17 GST Enhancement begin
        '        strsql &= "CDI_GST_RATE = '" & Common.Parse(dtCatalogue.Rows(i)("GSTRate")) & "', "
        '        strsql &= "CDI_GST_TAX_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("GSTTaxCode")) & "', "
        '        'Jules 2014.07.17 GST Enhancement end.
        '        strsql &= "CDI_REMARK = '" & Common.Parse(dtCatalogue.Rows(i)("Remark")) & "', "
        '        strsql &= "CDI_MOD_BY = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "', "
        '        strsql &= "CDI_MOD_DT = " & Common.ConvertDate(Now()) & " "
        '        strsql &= "WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("Index") & " "
        '        strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "'"
        '        Common.Insert2Ary(strAryQuery, strsql)
        '        ''Michelle (eBiz/134/08) - Update Shopping Cart also
        '        'strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = " & dtCatalogue.Rows(i)("UnitCost") & " "
        '        'strsql &= "WHERE SC_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' "
        '        'strsql &= " AND SC_S_COY_ID ='" & Common.Parse(strSCoyId) & "' "
        '        'strsql &= " AND SC_CD_GROUP_INDEX = " & dtCatalogue.Rows(i)("Index") & " "
        '        'Common.Insert2Ary(strAryQuery, strsql)

        '        'Insert into audit trail au_contract_items - add
        '        strsql1 = "SELECT * FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dtCatalogue.Rows(i)("index") & " AND CDI_PRODUCT_CODE = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "'"
        '        dtTemp = objDb.FillDt(strsql1)
        '        If dtTemp.Rows.Count > 0 Then
        '            'If dtCatalogue.Rows(i)("UnitCost") <> dtTemp.Rows(0)("CDI_UNIT_COST") Or _
        '            '   dtCatalogue.Rows(i)("Remark") <> dtTemp.Rows(0)("CDI_REMARK") Or _
        '            '   dtCatalogue.Rows(i)("GST") <> dtTemp.Rows(0)("CDI_GST") Or _
        '            '   dtCatalogue.Rows(i)("CurrencyCode") <> dtTemp.Rows(0)("CDI_CURRENCY_CODE") Or _                   
        '            If dtCatalogue.Rows(i)("UnitCost") <> dtTemp.Rows(0)("CDI_UNIT_COST") Or
        '            dtCatalogue.Rows(i)("Remark") <> dtTemp.Rows(0)("CDI_REMARK") Or
        '            dtCatalogue.Rows(i)("CurrencyCode") <> dtTemp.Rows(0)("CDI_CURRENCY_CODE") Or
        '            dtCatalogue.Rows(i)("GST") <> IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST").ToString), 0, dtTemp.Rows(0)("CDI_GST")) Or
        '            dtCatalogue.Rows(i)("GSTRate") <> IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST_RATE").ToString), "", dtTemp.Rows(0)("CDI_GST_RATE")) Or
        '            dtCatalogue.Rows(i)("GSTTaxCode") <> IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST_TAX_CODE").ToString), "", dtTemp.Rows(0)("CDI_GST_TAX_CODE")) Then

        '                If dtCatalogue.Rows(i)("UnitCost") <> dtTemp.Rows(0)("CDI_UNIT_COST") Then
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtCatalogue.Rows(i)("Index") & ","
        '                    'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Contract Price: " & dtCatalogue.Rows(i)("UnitCost") & "', "
        '                    'strsql &= "'I', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                    InsertAuditTrail(dtCatalogue.Rows(i)("Index"), Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Contract Price", dtTemp.Rows(0)("CDI_UNIT_COST"), dtCatalogue.Rows(i)("UnitCost"), "I", "E", strAryQuery)
        '                End If
        '                If dtCatalogue.Rows(i)("Remark") <> dtTemp.Rows(0)("CDI_REMARK") Then
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtCatalogue.Rows(i)("Index") & ","
        '                    'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Remarks: " & Common.Parse(dtCatalogue.Rows(i)("Remark")) & "', "
        '                    'strsql &= "'I', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                    InsertAuditTrail(dtCatalogue.Rows(i)("Index"), Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Remarks", dtTemp.Rows(0)("CDI_REMARK"), dtCatalogue.Rows(i)("Remark"), "I", "E", strAryQuery)
        '                End If
        '                'Jules 2014.07.17 GST Enhancement begin
        '                If dtCatalogue.Rows(i)("GST") <> IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST").ToString), 0, dtTemp.Rows(0)("CDI_GST")) Then
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtCatalogue.Rows(i)("Index") & ","
        '                    'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Tax: " & dtCatalogue.Rows(i)("GST") & "', "
        '                    'strsql &= "'I', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                    InsertAuditTrail(dtCatalogue.Rows(i)("Index"), Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Tax", IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST").ToString), System.DBNull.Value, dtTemp.Rows(0)("CDI_GST")), dtCatalogue.Rows(i)("GST"), "I", "E", strAryQuery)
        '                End If
        '                If dtCatalogue.Rows(i)("GSTRate") <> IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST_RATE").ToString), "", dtTemp.Rows(0)("CDI_GST_RATE")) Then
        '                    InsertAuditTrail(dtCatalogue.Rows(i)("Index"), Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "GST Rate", IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST_RATE").ToString), "", dtTemp.Rows(0)("CDI_GST_RATE")), dtCatalogue.Rows(i)("GSTRate"), "I", "E", strAryQuery)
        '                End If
        '                If dtCatalogue.Rows(i)("GSTTaxCode") <> IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST_TAX_CODE").ToString), "", dtTemp.Rows(0)("CDI_GST_TAX_CODE")) Then
        '                    InsertAuditTrail(dtCatalogue.Rows(i)("Index"), Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "GST Tax Code", IIf(String.IsNullOrEmpty(dtTemp.Rows(0)("CDI_GST_TAX_CODE").ToString), "", dtTemp.Rows(0)("CDI_GST_TAX_CODE")), dtCatalogue.Rows(i)("GSTTaxCode"), "I", "E", strAryQuery)
        '                End If
        '                'Jules 2014.07.17 GST Enhancement end.
        '                If dtCatalogue.Rows(i)("CurrencyCode") <> dtTemp.Rows(0)("CDI_CURRENCY_CODE") Then
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtCatalogue.Rows(i)("Index") & ","
        '                    'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Currency: " & Common.Parse(dtCatalogue.Rows(i)("CurrencyCode")) & "', "
        '                    'strsql &= "'I', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                    InsertAuditTrail(dtCatalogue.Rows(i)("Index"), Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & " - " & "Currency", dtTemp.Rows(0)("CDI_CURRENCY_CODE"), dtCatalogue.Rows(i)("CurrencyCode"), "I", "E", strAryQuery)
        '                End If

        '                'strsql = "INSERT INTO au_contract_items (CDI_GROUP_INDEX,CDI_PRODUCT_CODE,CDI_VENDOR_ITEM_CODE,CDI_PRODUCT_DESC,CDI_UOM,CDI_UNIT_COST,CDI_REMARK,CDI_GST,CDI_CURRENCY_CODE,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '                'strsql &= dtCatalogue.Rows(i)("Index") & ", "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_PRODUCT_DESC")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_UOM")) & "', "
        '                'If dtCatalogue.Rows(i)("UnitCost") <> dtTemp.Rows(0)("CDI_UNIT_COST") Then
        '                '    strsql &= dtCatalogue.Rows(i)("UnitCost") & ", "
        '                'Else
        '                '    strsql &= "NULL, "
        '                'End If
        '                'If dtCatalogue.Rows(i)("Remark") <> dtTemp.Rows(0)("CDI_REMARK") Then
        '                '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("Remark")) & "', "
        '                'Else
        '                '    strsql &= "NULL, "
        '                'End If
        '                'If dtCatalogue.Rows(i)("GST") <> dtTemp.Rows(0)("CDI_GST") Then
        '                '    strsql &= dtCatalogue.Rows(i)("GST") & ", "
        '                'Else
        '                '    strsql &= "NULL, "
        '                'End If
        '                'If dtCatalogue.Rows(i)("CurrencyCode") <> dtTemp.Rows(0)("CDI_CURRENCY_CODE") Then
        '                '    strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("CurrencyCode")) & "', "
        '                'Else
        '                '    strsql &= "NULL, "
        '                'End If
        '                'strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'E')"
        '                '---------------------
        '                'strsql = "INSERT INTO au_contract_items (CDI_GROUP_INDEX,CDI_PRODUCT_CODE,CDI_VENDOR_ITEM_CODE,CDI_PRODUCT_DESC,CDI_UOM,CDI_UNIT_COST,CDI_REMARK,CDI_GST,CDI_CURRENCY_CODE,AU_USER,AU_DATE,AU_ACTION) VALUES ("
        '                'strsql &= dtCatalogue.Rows(i)("Index") & ", "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_VENDOR_ITEM_CODE")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_PRODUCT_DESC")) & "', "
        '                'strsql &= "'" & Common.Parse(dtTemp.Rows(0)("CDI_UOM")) & "', "
        '                'strsql &= dtCatalogue.Rows(i)("UnitCost") & ", "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("Remark")) & "', "
        '                'strsql &= dtCatalogue.Rows(i)("GST") & ", "
        '                'strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("CurrencyCode")) & "', "
        '                'strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ",'E')"
        '                'Common.Insert2Ary(strAryQuery, strsql)
        '            End If
        '        End If

        '        'If dtCatalogue.Rows(i)("Change") = 1 Then
        '        '    blnDiscount = True
        '        '    strGroupIndex &= dtCatalogue.Rows(i)("Index") & ","
        '        '    dtr = dt2.NewRow()
        '        '    dtr("ItemId") = Common.Parse(dtCatalogue.Rows(i)("ProductCode"))
        '        '    dtr("ItemDesc") = Common.Parse(dtCatalogue.Rows(i)("ProductDesc"))
        '        '    dtr("List") = "No"
        '        '    dtr("Discount") = "Yes"
        '        '    dt2.Rows.Add(dtr)
        '        'End If
        '    Next

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            updateCatalogueItem = WheelMsgNum.Save

        '        Else
        '            updateCatalogueItem = WheelMsgNum.NotSave
        '            'If blnDiscount Then
        '            '    Dim objMail As New Email
        '            '    If strGroupIndex <> "" Then
        '            '        strGroupIndex = strGroupIndex.Substring(0, strGroupIndex.Length - 1)
        '            '    End If
        '            '    objMail.sendItemUpdateMail(strSCoyId, strGroupIndex, dt1, dt2, False, True)
        '            '    objMail = Nothing
        '            'End If
        '        End If
        '    End If

        'End Function

        'Public Function updateContractCatalogueItemRemark(ByVal dtCatalogue As DataTable)
        '    Dim strsql As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        strsql = "UPDATE CONT_DIST_UPLOADITEMS SET "
        '        strsql &= "CDUI_Remark = '" & Common.Parse(dtCatalogue.Rows(i)("Remark")) & "' "
        '        strsql &= "WHERE CDUI_Upload_Index = " & dtCatalogue.Rows(i)("Index") & " "
        '        strsql &= "AND CDUI_Product_Code = '" & Common.Parse(dtCatalogue.Rows(i)("ProductCode")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next
        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function insertCatalogueCompany(ByVal strIndex As String, ByVal dtCatalogue As DataTable)
        '    Dim strsql As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String

        '    strsql = "DELETE FROM CONTRACT_DIST_COY "
        '    strsql &= "WHERE CDC_GROUP_INDEX = '" & strIndex & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    For i = 0 To dtCatalogue.Rows.Count - 1
        '        strsql = "INSERT INTO CONTRACT_DIST_COY (CDC_GROUP_INDEX, CDC_B_COY_ID) VALUES ("
        '        strsql &= "'" & strIndex & "', "
        '        strsql &= "'" & Common.Parse(dtCatalogue.Rows(i)("Buyer")) & "') "
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next
        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function getCompanyAssign(ByVal intIndex As Integer, ByVal strVendor As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CM_COY_ID, CM_COY_NAME FROM COMPANY_MSTR "
        '    strsql &= "WHERE (CM_COY_TYPE = 'BOTH' OR CM_COY_TYPE = 'BUYER') "
        '    strsql &= "AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
        '    strsql &= "AND CM_COY_ID <> '" & Common.Parse(strVendor) & "' "
        '    strsql &= "AND CM_COY_ID NOT IN ("
        '    strsql &= "SELECT CDC_B_COY_ID FROM CONTRACT_DIST_COY WHERE CDC_GROUP_INDEX = " & intIndex & "); "
        '    strsql &= "SELECT CDC_B_COY_ID, CM_COY_NAME FROM CONTRACT_DIST_COY "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON CDC_B_COY_ID = CM_COY_ID "
        '    strsql &= "WHERE CDC_GROUP_INDEX = " & intIndex & "; "
        '    ds = objDb.FillDs(strsql)
        '    getCompanyAssign = ds
        'End Function

        'Public Function getCompanyCount(ByVal intIndex As Integer) As Integer
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT COUNT(CDC_B_COY_ID) FROM CONTRACT_DIST_COY WHERE CDC_GROUP_INDEX = " & intIndex
        '    ds = objDb.FillDs(strsql)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        getCompanyCount = ds.Tables(0).Rows(0)(0)
        '    Else
        '        getCompanyCount = 0
        '    End If
        'End Function

        'Public Sub GetLatestItemNo(ByRef strDocNo As String)
        Public Function GetLatestItemNo(ByRef strDocNo As String)
            Dim strSql, strPrefix, strLastUsedNo, strReplicate As String
            Dim intLeadingZero, intLen, intLoop As Integer
            Dim ds As New DataSet

            strLastUsedNo = "0"
            strPrefix = ""
            strSql = "SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_NAME = 'ItemPrefix'; "
            strSql &= "SELECT SP_PARAM_VALUE FROM SYSTEM_PARAM WHERE SP_PARAM_NAME = 'ItemLastUsedNo';"

            ds = objDb.FillDs(strSql)
            If ds.Tables(0).Rows.Count > 0 Then
                strPrefix = ds.Tables(0).Rows(0)(0)
            End If

            If ds.Tables(1).Rows.Count > 0 Then
                strLastUsedNo = ds.Tables(1).Rows(0)(0)
            End If

            If strLastUsedNo.Substring(0, 1) = "0" Then
                intLen = strLastUsedNo.Length
                strLastUsedNo = Convert.ToInt32(strLastUsedNo) + 1
                '//after perform adding, leading zero lost
                intLeadingZero = intLen - strLastUsedNo.Length
                '//add back the leading zero
                strReplicate = Strings.StrDup(intLeadingZero, "0")
            Else
                strLastUsedNo = Convert.ToInt32(strLastUsedNo) + 1
                intLeadingZero = 0
                strReplicate = Strings.StrDup(intLeadingZero, "0")
            End If

            strDocNo = strPrefix & strReplicate & strLastUsedNo
        End Function

        'Public Function getSingleModifiedProduct(ByVal strCode As String, ByVal strIndex As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT PM_PRODUCT_INDEX, PM_S_COY_ID, PM_PRODUCT_CODE, CT_NAME, PM_VENDOR_ITEM_CODE,PM_PRODUCT_DESC, "
        '    strsql &= "PM_REF_NO,PM_LONG_DESC, PM_CATEGORY_NAME,PM_GST_CODE, "
        '    strsql &= "PM_TAX_ID,"
        '    strsql &= objDb.Concat3(" ", "%", "PM_GST_CODE", "PM_TAX_PERC") & " AS TAX_CODE,"
        '    strsql &= "PM_UOM, PM_UNIT_COST, "
        '    strsql &= "PM_PRODUCT_IMAGE, PM_CURRENCY_CODE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL, "
        '    strsql &= "PM_PRODUCT_TYPE, PM_STATUS, PM_REMARK, PM_ACTION, CM_COY_NAME, "
        '    strsql &= "PM_DRAW_NO, PM_VERS_NO, PM_GROSS_WEIGHT, PM_NET_WEIGHT, "
        '    strsql &= "PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_PRODUCT_TYPE, "
        '    strsql &= "PM_STATUS, PM_REMARKS, PM_ACTION, CM_COY_NAME "
        '    strsql &= "FROM PRODUCT_MSTR_TEMP "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
        '    'strsql &= "LEFT JOIN CATEGORY_MSTR ON PM_CATEGORY_NAME = CM_CATEGORY_NAME "
        '    strsql &= "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID "
        '    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "' "
        '    strsql &= "AND PM_PRODUCT_INDEX = '" & Common.Parse(strIndex) & "'; "
        '    strsql &= "SELECT * FROM PRODUCT_MOD_TEMP WHERE PMT_PRODUCT_CODE = '" & Common.Parse(strCode) & "' "
        '    strsql &= "AND PMT_PRODUCT_INDEX = " & strIndex
        '    ds = objDb.FillDs(strsql)
        '    getSingleModifiedProduct = ds
        'End Function

        'Public Function getMapVendor(ByVal strPrdIdx As String, ByVal strCoyId As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT (SELECT CM_COY_NAME "
        '    strsql &= "FROM PRODUCT_MAP "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID "
        '    strsql &= "WHERE PM_B_PRODUCT_INDEX = '" & Common.Parse(strPrdIdx) & "' AND PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' AND PM_SEQ = '1') AS NAME1, "
        '    strsql &= "(SELECT CM_COY_NAME "
        '    strsql &= "FROM PRODUCT_MAP "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID "
        '    strsql &= "WHERE PM_B_PRODUCT_INDEX = '" & Common.Parse(strPrdIdx) & "' AND PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' AND PM_SEQ = '2') AS NAME2, "
        '    strsql &= "(SELECT CM_COY_NAME "
        '    strsql &= "FROM PRODUCT_MAP "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID "
        '    strsql &= "WHERE PM_B_PRODUCT_INDEX = '" & Common.Parse(strPrdIdx) & "' AND PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' AND PM_SEQ = '3') AS NAME3, "
        '    strsql &= "(SELECT CM_COY_NAME "
        '    strsql &= "FROM PRODUCT_MAP "
        '    strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID "
        '    strsql &= "WHERE PM_B_PRODUCT_INDEX = '" & Common.Parse(strPrdIdx) & "' AND PM_S_COY_ID = '" & Common.Parse(strCoyId) & "' AND PM_SEQ = '4') AS NAME4 "
        '    ds = objDb.FillDs(strsql)
        '    getMapVendor = ds
        'End Function

        'Public Function getSingleProduct(ByVal strCode As String, ByVal blnTemp As Boolean) As DataSet
        Public Function getSingleProduct(ByVal strCode As String, ByVal blnTemp As Boolean) As String
            Dim strsql As String
            Dim ds As New DataSet

            If blnTemp Then
                strsql = "SELECT PM_PRODUCT_INDEX, PM_S_COY_ID, PM_PRODUCT_CODE, CT_NAME, PM_VENDOR_ITEM_CODE,PM_PRODUCT_DESC, "
                strsql &= "PM_REF_NO,PM_LONG_DESC, PM_CATEGORY_NAME,PM_GST_CODE, "
                strsql &= "PM_TAX_ID,"
                strsql &= objDb.Concat3(" ", "%", "PM_GST_CODE", "PM_TAX_PERC") & " AS TAX_CODE,"
                strsql &= "PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE,"
                strsql &= "PM_PRODUCT_IMAGE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO,PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT, "
                strsql &= "PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_PRODUCT_TYPE, "
                strsql &= "PM_STATUS, PM_REMARK, PM_REMARKS, PM_ACTION, CM_COY_NAME "
                strsql &= "FROM PRODUCT_MSTR_TEMP "
                strsql &= "LEFT JOIN COMPANY_MSTR ON PM_S_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
                'strsql &= "LEFT JOIN CATEGORY_MSTR ON PM_CATEGORY_NAME = CM_CATEGORY_NAME "
                strsql &= "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID "
                strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "' "
            Else
                strsql = "SELECT PM_PRODUCT_INDEX, PM_S_COY_ID, PM_PRODUCT_CODE, CT_NAME, PM_VENDOR_ITEM_CODE,PM_PRODUCT_DESC, "
                strsql &= "PM_REF_NO,PM_LONG_DESC, PM_CATEGORY_NAME,PM_GST_CODE, "
                strsql &= "PM_TAX_ID,"
                strsql &= objDb.Concat3(" ", "%", "PM_GST_CODE", "PM_TAX_PERC") & " AS TAX_CODE,"
                strsql &= "PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, PM_PREFER_S_COY_ID_TAX_ID, PM_1ST_S_COY_ID_TAX_ID, PM_2ND_S_COY_ID_TAX_ID, PM_3RD_S_COY_ID_TAX_ID, "
                strsql &= "PM_PRODUCT_IMAGE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO,PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT, "
                strsql &= "PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_PRODUCT_TYPE, "
                'Michelle (31/10/2010) - Take the CT_NAME & the related columns for BIM
                'strsql &= " '' as PM_STATUS, '' AS PM_REMARKS, CM_COY_NAME, CM_CATEGORY_DESC "
                strsql &= " '' as PM_STATUS, PM_REMARK, PM_REMARKS, e.CM_COY_NAME, PM_ACCT_CODE, PM_SAFE_QTY, PM_ORD_MIN_QTY, PM_ORD_MAX_QTY, PM_DELETED, "
                strsql &= "PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID,  a.CM_COY_NAME AS S_COY_NAME_P,b.CM_COY_NAME AS S_COY_NAME_1,c.CM_COY_NAME AS S_COY_NAME_2,d.CM_COY_NAME AS S_COY_NAME_3, "
                strsql &= "PM_ITEM_TYPE, PM_IQC_IND, PM_MAX_INV_QTY, PM_MANUFACTURER, PM_CAT_CODE "
                strsql &= "FROM PRODUCT_MSTR "
                strsql &= "LEFT JOIN COMPANY_MSTR e ON PM_S_COY_ID = e.CM_COY_ID AND e.CM_DELETED <> 'Y' AND e.CM_STATUS = 'A' "
                strsql &= "LEFT JOIN COMPANY_MSTR a ON PM_PREFER_S_COY_ID = a.CM_COY_ID LEFT JOIN COMPANY_MSTR b ON PM_1ST_S_COY_ID = b.CM_COY_ID LEFT JOIN COMPANY_MSTR c ON PM_2ND_S_COY_ID = c.CM_COY_ID LEFT JOIN COMPANY_MSTR d ON PM_3RD_S_COY_ID = d.CM_COY_ID "
                'strsql &= "LEFT JOIN CATEGORY_MSTR ON PM_CATEGORY_NAME = CM_CATEGORY_NAME "
                strsql &= "LEFT JOIN COMMODITY_TYPE ON PM_CATEGORY_NAME = CT_ID "
                strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "' "
                'strsql &= "AND PM_DELETED <> 'Y' "
            End If
            'ds = objDb.FillDs(strsql)
            'getSingleProduct = ds
            Return strsql
        End Function

        Public Function getSingleProductVenLead(ByVal strCode As String, ByVal blnTemp As Boolean) As DataSet
            Dim strsql As String
            Dim ds As New DataSet

            Dim INDEX As String
            strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
            strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strCode) & "'"

            INDEX = objDb.GetVal(strsql)

            strsql = "SELECT PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE "
            strsql &= "FROM PIM_VENDOR "
            strsql &= "WHERE PV_PRODUCT_INDEX = '" & INDEX & "' "

            ds = objDb.FillDs(strsql)
            getSingleProductVenLead = ds
        End Function

        'Public Function getCompanyId(ByVal sCompanyName As String) As String
        '    '            getCompanyId = objDb.Get1Column("company_mstr", "CM_COY_ID", " WHERE CM_COY_NAME = '" & sCompanyName & "'")
        '    getCompanyId = objDb.Get1Column("Company_Vendor, Company_Mstr", "CV_S_COY_ID", " WHERE CM_COY_NAME = '" & sCompanyName & "' AND CV_S_COY_ID=CM_COY_ID and CV_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND CM_STATUS = 'A' ")
        'End Function

        '' inserted by Hub Admin
        'Public Function insertProductMstr(ByVal dsProduct As DataSet, ByVal strMode As String, ByVal strImageIndex As String, ByVal strOldVIC As String, ByVal blnList As Boolean, ByVal blnDiscount As Boolean) As String
        '    Dim strsql, strsql2 As String
        '    Dim strProductCode As String
        '    Dim strGroupIndex As String = ""
        '    Dim strAryQuery(0) As String
        '    Dim dt1 As New DataTable
        '    Dim dt2 As New DataTable

        '    dt1.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt1.Columns.Add("ItemDesc", Type.GetType("System.String"))

        '    dt2.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt2.Columns.Add("ItemDesc", Type.GetType("System.String"))
        '    dt2.Columns.Add("List", Type.GetType("System.String"))
        '    dt2.Columns.Add("Discount", Type.GetType("System.String"))

        '    Dim dtr As DataRow
        '    'If strOldVIC <> dsProduct.Tables(0).Rows(0)("VendorItemCode") Then
        '    '    strsql = "SELECT '*' FROM PRODUCT_MSTR WHERE "
        '    '    strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "' "
        '    '    strsql &= "AND PM_S_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '    '    If objDb.Exist(strsql) > 0 Then
        '    '        insertProductMstr = ""
        '    '        Exit Function
        '    '    End If
        '    'End If

        '    Select Case strMode
        '        Case "add"
        '            GetLatestItemNo(strProductCode)
        '            strsql = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE = SP_PARAM_VALUE + 1 WHERE SP_PARAM_NAME = 'ItemLastUsedNo' "
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            strsql = "SELECT '*' FROM PRODUCT_MSTR WHERE "
        '            strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "' "
        '            strsql &= "AND PM_S_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '            strsql &= "AND PM_PRODUCT_CODE <> '" & Common.Parse(strProductCode) & "' "
        '            strsql &= "AND PM_DELETED <> 'Y' "

        '            ' check duplication
        '            strsql2 = "SELECT '*' FROM PRODUCT_MSTR WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "

        '            If objDb.Exist(strsql) > 0 Or objDb.Exist(strsql2) > 0 Then
        '                insertProductMstr = ""
        '                Exit Function
        '            End If

        '            strsql = "INSERT INTO PRODUCT_MSTR (PM_S_COY_ID, PM_PRODUCT_CODE, PM_CATEGORY_NAME, PM_VENDOR_ITEM_CODE, "
        '            strsql &= "PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_GST_CODE, PM_CURRENCY_CODE, "
        '            strsql &= "PM_PRODUCT_BRAND, PM_PRODUCT_MODEL, PM_PRODUCT_TYPE, PM_ENT_BY, PM_ENT_DT, PM_DELETED) VALUES ("
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '            strsql &= "'" & Common.Parse(strProductCode) & "', " ' PM_PRODUCT_CODE
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CategoryName")) & "', " ' PM_CATEGORY_NAME
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '            If Common.Parse(dsProduct.Tables(0).Rows(0)("UnitCost")) = "" Then
        '                strsql &= "NULL, " ' PM_UNIT_COST
        '            Else
        '                strsql &= dsProduct.Tables(0).Rows(0)("UnitCost") & ", " ' PM_UNIT_COST
        '            End If

        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("gstcode")) & "', " ' PM_GST_CODE
        '            'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmtcode")) & "', " ' PM_MGMT_CODE
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("productbrand")) & "', " ' PM_PRODUCT_BRAND
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("productmodel")) & "', " ' PM_PRODUCT_MODEL
        '            strsql &= "NULL, " ' PM_PRODUCT_TYPE
        '            strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), " ' PM_ENT_BY, PM_ENT_DT
        '            'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmttext")) & "','N') " ' PM_MGMT_TEXT
        '            Common.Insert2Ary(strAryQuery, strsql)

        '        Case "mod"
        '            strProductCode = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))

        '            strsql = "SELECT '*' FROM PRODUCT_MSTR WHERE "
        '            strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "' "
        '            strsql &= "AND PM_S_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '            strsql &= "AND PM_PRODUCT_CODE <> '" & Common.Parse(strProductCode) & "' "
        '            strsql &= "AND PM_DELETED <> 'Y' "
        '            If objDb.Exist(strsql) > 0 Then
        '                insertProductMstr = ""
        '                Exit Function
        '            End If

        '            strsql = "UPDATE PRODUCT_MSTR SET "
        '            strsql &= "PM_CATEGORY_NAME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CategoryName")) & "', "
        '            strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', "
        '            strsql &= "PM_PRODUCT_DESC = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', "
        '            strsql &= "PM_UOM = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', "
        '            If Common.Parse(dsProduct.Tables(0).Rows(0)("UnitCost")) = "" Then
        '                strsql &= "PM_UNIT_COST = NULL, "
        '            Else
        '                strsql &= "PM_UNIT_COST =" & dsProduct.Tables(0).Rows(0)("UnitCost") & ", "
        '            End If

        '            strsql &= "PM_GST_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("gstcode")) & "', "
        '            'strsql &= "PM_MGMT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("MgmtCode")) & "', "
        '            strsql &= "PM_CURRENCY_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CurrencyCode")) & "', "
        '            strsql &= "PM_PRODUCT_BRAND = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductBrand")) & "', "
        '            strsql &= "PM_PRODUCT_MODEL = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductModel")) & "', "
        '            'strsql &= "PM_MGMT_TEXT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmttext")) & "', "
        '            strsql &= "PM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
        '            strsql &= "PM_MOD_DT = GETDATE() WHERE PM_PRODUCT_INDEX = " & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductIndex")) & ";"
        '            'Michelle (eBiz/134/08) - Update Shopping Cart also
        '            If Common.Parse(dsProduct.Tables(0).Rows(0)("UnitCost")) = "" Then
        '                strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = NULL, "
        '            Else
        '                strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = " & dsProduct.Tables(0).Rows(0)("UnitCost") & ", "
        '            End If
        '            strsql &= "SC_VENDOR_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', "
        '            strsql &= "SC_PRODUCT_DESC = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', "
        '            strsql &= "SC_CURRENCY_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CurrencyCode")) & "', "
        '            strsql &= "SC_UOM = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "' "
        '            strsql &= "WHERE SC_SOURCE = 'LP' AND SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '            strsql &= " AND SC_S_COY_ID ='" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)
        '    End Select

        '    Dim i As Integer
        '    For i = 0 To dsProduct.Tables(1).Rows.Count - 1
        '        If Not IsDBNull(dsProduct.Tables(1).Rows(i)("Change")) Then
        '            If dsProduct.Tables(1).Rows(i)("Change") = 1 Then
        '                blnDiscount = True
        '                strGroupIndex &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ","
        '            End If
        '        End If

        '        Select Case dsProduct.Tables(1).Rows(i)("chk")
        '            Case "1"
        '                If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " AND CDI_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' ") <= 0 Then
        '                    strsql = "INSERT INTO CONTRACT_DIST_ITEMS (CDI_GROUP_INDEX, CDI_PRODUCT_CODE, "
        '                    strsql &= "CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, CDI_UNIT_COST, CDI_REMARK, CDI_GST, "
        '                    strsql &= "CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) SELECT "
        '                    strsql &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ", "
        '                    strsql &= "'" & Common.Parse(strProductCode) & "', " ' PM_PRODUCT_CODE
        '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '                    strsql &= dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '                    strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "', "
        '                    strsql &= "(SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_CATEGORY = 'SST' AND CODE_ABBR = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "'), "
        '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE() "
        '                Else
        '                    ' item already exist in the catalogue
        '                    strsql = "UPDATE CONTRACT_DIST_ITEMS "
        '                    strsql &= "SET CDI_UNIT_COST = " & dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '                    strsql &= "CDI_REMARK = '" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "' "
        '                    strsql &= "WHERE CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '                    strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "'; "
        '                    'Michelle (eBiz/134/08) - Update Shopping Cart also
        '                    strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = " & dsProduct.Tables(1).Rows(i)("DiscPrice") & " "
        '                    strsql &= "WHERE SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '                    strsql &= " AND SC_S_COY_ID ='" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '                    strsql &= " AND SC_CD_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '                End If
        '                Common.Insert2Ary(strAryQuery, strsql)

        '            Case "0"
        '                strsql = "DELETE FROM CONTRACT_DIST_ITEMS WHERE "
        '                strsql &= "CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '                strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'; "
        '                strsql &= "DELETE FROM SHOPPING_CART WHERE "
        '                strsql &= "SC_CD_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '                strsql &= "AND SC_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '                Common.Insert2Ary(strAryQuery, strsql)
        '        End Select
        '    Next

        '    'If strImageIndex <> "" Then
        '    '    strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & strImageIndex & "' "
        '    '    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    '    Common.Insert2Ary(strAryQuery, strsql)
        '    'End If

        '    ' ai chu add on 19/09/2005
        '    ' copy all attachment from PRODUCT_ATTACHMENT_TEMP into PRODUCT_ATTACHMENT WHERE PA_STATUS <> 'D'
        '    ' before that, delete all attachment in PRODUCT_ATTACHMENT
        '    ' TODO check image file first then check document file
        '    ' cannot delete all image file in PRODUCT_ATTACHMENT_TEMP because user may submit item for approval for many time
        '    ' image file
        '    If strImageIndex <> "" Then
        '        strsql = "DELETE FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '        strsql &= "AND PA_TYPE = 'I'"
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
        '        strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
        '        strsql &= "WHERE PA_ATTACH_INDEX = '" & Common.Parse(strImageIndex) & "' AND PA_STATUS <> 'D' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '        'strsql &= "SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR_TEMP) "
        '        strsql &= "AND PA_TYPE = 'I' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        'strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = PA_ATTACH_INDEX "
        '        'strsql &= "FROM PRODUCT_MSTR, PRODUCT_ATTACHMENT "
        '        'strsql &= "WHERE PM_PRODUCT_CODE = PA_PRODUCT_CODE AND PA_TYPE = 'I' "
        '        'strsql &= "AND PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '        'Editted by sam 22/09/2010
        '        strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & objDb.GetVal("SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_TYPE = 'I' AND PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' ")
        '        strsql &= "' WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "

        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If

        '    ' delete 
        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    strsql &= "AND PA_TYPE = 'D'"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
        '    strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_STATUS <> 'D' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    strsql &= "AND PA_TYPE = 'D'"
        '    Common.Insert2Ary(strAryQuery, strsql)
        '    ' ai chu add end

        '    strsql = "UPDATE PRODUCT_ATTACHMENT SET "
        '    strsql &= "PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If objDb.BatchExecute(strAryQuery) Then
        '        Dim objMail As New Email
        '        If strGroupIndex <> "" Then
        '            strGroupIndex = strGroupIndex.Substring(0, strGroupIndex.Length - 1)
        '        End If

        '        If strMode = "add" Then
        '            dtr = dt1.NewRow()
        '            dtr("ItemId") = Common.Parse(strProductCode)
        '            dtr("ItemDesc") = Common.Parse(Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")))
        '            dt1.Rows.Add(dtr)

        '            'dtr = dt2.NewRow()
        '            'dtr("ItemId") = "-"
        '            'dtr("ItemDesc") = "-"
        '            'dtr("List") = "-"
        '            'dtr("Discount") = "-"
        '            'dt2.Rows.Add(dtr)
        '            objMail.sendItemUpdateMail(HttpContext.Current.Session("CompanyIdToken"), strGroupIndex, dt1, dt2, True, False)
        '        Else
        '            If blnList Or blnDiscount Then
        '                'dtr = dt1.NewRow()
        '                'dtr("ItemId") = "-"
        '                'dtr("ItemDesc") = "-"
        '                'dt1.Rows.Add(dtr)

        '                dtr = dt2.NewRow()
        '                dtr("ItemId") = Common.Parse(strProductCode)
        '                dtr("ItemDesc") = Common.Parse(Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")))
        '                dtr("List") = IIf(blnList, "Yes", "No")
        '                dtr("Discount") = IIf(blnDiscount, "Yes", "No")
        '                dt2.Rows.Add(dtr)
        '                objMail.sendItemUpdateMail(HttpContext.Current.Session("CompanyIdToken"), strGroupIndex, dt1, dt2, blnList, blnDiscount)
        '            End If
        '        End If

        '        objMail = Nothing
        '        insertProductMstr = strProductCode
        '    End If
        'End Function

        'Public Function SaveContractCatalogue(ByVal ds As DataSet, ByVal dsHeader As DataSet, ByVal SaveHeader As Integer, ByVal intID As Integer, ByVal blnGstCOD As Boolean) As Boolean
        '    Dim strsql, strsql2 As String
        '    Dim strProductCode As String
        '    Dim strAryQuery(0) As String
        '    Dim dr As DataRow
        '    Dim dtHeader As DataTable
        '    Dim dtItem As DataTable
        '    Dim strPRExist As String ' item exist in PR
        '    Dim strPOExist As String
        '    Dim i As Integer
        '    Dim intGID As Integer

        '    dtHeader = dsHeader.Tables(0)
        '    dtItem = ds.Tables(0)

        '    If SaveHeader = 0 Then
        '        'Add Contract Header
        '        strsql = "INSERT INTO CONTRACT_DIST_MSTR "
        '        strsql &= "(CDM_GROUP_CODE, CDM_GROUP_DESC, CDM_S_COY_ID, CDM_TYPE, CDM_START_DATE, CDM_END_DATE, "
        '        strsql &= "CDM_ENT_BY, CDM_ENT_DT) VALUES ("
        '        strsql &= "'" & Common.Parse(dtHeader.Rows(0)("GroupCode")) & "', "
        '        strsql &= "'" & Common.Parse(dtHeader.Rows(0)("GroupDesc")) & "', "
        '        strsql &= "'" & Common.Parse(dtHeader.Rows(0)("VendorID")) & "', "
        '        strsql &= "'" & Common.Parse(dtHeader.Rows(0)("CatType")) & "', "
        '        strsql &= Common.ConvertDate(dtHeader.Rows(0)("StartDate")) & ", "
        '        strsql &= Common.ConvertDate(dtHeader.Rows(0)("EndDate")) & ", "
        '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', " & Common.ConvertDate(Now()) & ")"
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        strsql = "INSERT INTO CONTRACT_DIST_COY (CDC_GROUP_INDEX, CDC_B_COY_ID) SELECT "
        '        strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR), "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        'Insert into audit trail - add
        '        InsertAuditTrail(intGID, "Contract Ref. No.", "", dtHeader.Rows(0)("GroupCode"), "B", "A", strAryQuery, True)
        '        'strsql = "INSERT INTO au_contract "
        '        'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) SELECT "
        '        'strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR),"
        '        'strsql &= "'Contract Ref. No.: " & Common.Parse(dtHeader.Rows(0)("GroupCode")) & "', "
        '        'strsql &= "'B', "
        '        'strsql &= "'A', "
        '        'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '        'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '        'strsql &= Common.ConvertDate(Now())
        '        'Common.Insert2Ary(strAryQuery, strsql)
        '        'Add contract items
        '        'For i = 0 To dtItem.Rows.Count - 1
        '        strsql = "INSERT INTO CONTRACT_DIST_ITEMS "
        '        strsql &= "(CDI_GROUP_INDEX, CDI_PRODUCT_CODE, CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, "
        '        If blnGstCOD = True Then
        '            strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST_RATE, CDI_GST_TAX_CODE, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) SELECT "
        '        Else
        '            strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) SELECT "
        '        End If
        '        strsql &= "(SELECT ISNULL(MAX(CDM_GROUP_INDEX),1) FROM CONTRACT_DIST_MSTR), "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("ProductCode")) & "', "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("VendorItemCode")) & "', "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("ProductDesc")) & "', "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("UOM")) & "', "
        '        strsql &= dtItem.Rows(0)("UnitCost") & ", "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("Remark")) & "', "
        '        If blnGstCOD = True Then
        '            strsql &= "'" & dtItem.Rows(0)("GSTRate") & "', "
        '            strsql &= "'" & Common.Parse(dtItem.Rows(0)("GSTTaxCode")) & "', "
        '        Else
        '            strsql &= "'" & dtItem.Rows(0)("GST") & "', "
        '        End If
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("CurrencyCode")) & "', "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "', " & Common.ConvertDate(Now()) '& ")"
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        InsertAuditTrail(intGID, "Item Code", "", dtItem.Rows(0)("VendorItemCode"), "B", "A", strAryQuery, True)

        '        If strAryQuery(0) <> String.Empty Then
        '            If objDb.BatchExecute(strAryQuery) Then
        '                SaveContractCatalogue = True

        '            Else
        '                SaveContractCatalogue = False

        '            End If
        '        End If
        '    Else
        '        intGID = intID
        '        'Add contract items
        '        'For i = 0 To dtItem.Rows.Count - 1
        '        strsql = "INSERT INTO CONTRACT_DIST_ITEMS "
        '        strsql &= "(CDI_GROUP_INDEX, CDI_PRODUCT_CODE, CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, "
        '        If blnGstCOD = True Then
        '            strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST_RATE, CDI_GST_TAX_CODE, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) VALUES ( "
        '        Else
        '            strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) VALUES ( "
        '        End If
        '        strsql &= intGID & ", "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("ProductCode")) & "', "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("VendorItemCode")) & "', "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("ProductDesc")) & "', "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("UOM")) & "', "
        '        strsql &= dtItem.Rows(0)("UnitCost") & ", "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("Remark")) & "', "
        '        If blnGstCOD = True Then
        '            strsql &= "'" & dtItem.Rows(0)("GSTRate") & "', "
        '            strsql &= "'" & Common.Parse(dtItem.Rows(0)("GSTTaxCode")) & "', "
        '        Else
        '            strsql &= "'" & dtItem.Rows(0)("GST") & "', "
        '        End If
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("CurrencyCode")) & "', "
        '        strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "', " & Common.ConvertDate(Now()) & ")"
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        InsertAuditTrail(intGID, "Item Code", "", dtItem.Rows(0)("VendorItemCode"), "B", "A", strAryQuery)

        '        If strAryQuery(0) <> String.Empty Then
        '            If objDb.BatchExecute(strAryQuery) Then
        '                SaveContractCatalogue = True

        '            Else
        '                SaveContractCatalogue = False

        '            End If
        '        End If
        '    End If
        '    'objDb = Nothing
        'End Function

        'Public Function UpdateContractCatalogue(ByVal ds As DataSet, ByVal dsHeader As DataSet, ByVal SaveHeader As Integer, ByVal blnGstCOD As Boolean) As Boolean
        '    Dim strsql, strsql2 As String
        '    Dim strProductCode As String
        '    Dim strAryQuery(0) As String
        '    Dim dr As DataRow
        '    Dim dtHeader As DataTable
        '    Dim dtItem As DataTable
        '    Dim strPRExist As String ' item exist in PR
        '    Dim strPOExist As String
        '    Dim i As Integer
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    dtHeader = dsHeader.Tables(0)
        '    dtItem = ds.Tables(0)

        '    'Amendment
        '    'Update contract header
        '    If SaveHeader = 0 Then
        '        strsql = "UPDATE CONTRACT_DIST_MSTR SET "
        '        strsql &= "CDM_GROUP_DESC = '" & Common.Parse(dtHeader.Rows(0)("GroupDesc")) & "', "
        '        strsql &= "CDM_START_DATE = " & Common.ConvertDate(dtHeader.Rows(0)("StartDate")) & ", "
        '        strsql &= "CDM_END_DATE = " & Common.ConvertDate(dtHeader.Rows(0)("EndDate")) & ", "
        '        strsql &= "CDM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
        '        strsql &= "CDM_MOD_DT = " & Common.ConvertDate(Now()) & " "
        '        strsql &= "WHERE CDM_GROUP_INDEX = " & dtHeader.Rows(0)("Index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        'Insert into audit trail - Edit
        '        strsql1 = "SELECT * FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = " & dtHeader.Rows(0)("Index")
        '        dtTemp = objDb.FillDt(strsql1)
        '        If dtTemp.Rows.Count > 0 Then
        '            If dtHeader.Rows(0)("GroupDesc") <> dtTemp.Rows(0)("CDM_GROUP_DESC") Or
        '               dtHeader.Rows(0)("StartDate") <> dtTemp.Rows(0)("CDM_START_DATE") Or
        '               dtHeader.Rows(0)("EndDate") <> dtTemp.Rows(0)("CDM_END_DATE") Then

        '                If dtHeader.Rows(0)("GroupDesc") <> dtTemp.Rows(0)("CDM_GROUP_DESC") Then
        '                    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Contract Desc.", dtTemp.Rows(0)("CDM_GROUP_DESC"), dtHeader.Rows(0)("GroupDesc"), "B", "E", strAryQuery)
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtHeader.Rows(0)("Index") & ","
        '                    'strsql &= "'Contract Desc.: " & Common.Parse(dtHeader.Rows(0)("GroupDesc")) & "', "
        '                    'strsql &= "'B', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                End If
        '                If dtHeader.Rows(0)("StartDate") <> dtTemp.Rows(0)("CDM_START_DATE") Then
        '                    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Contract Start Date", dtTemp.Rows(0)("CDM_START_DATE"), dtHeader.Rows(0)("StartDate"), "B", "E", strAryQuery)
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtHeader.Rows(0)("Index") & ","
        '                    'strsql &= "'Contract Start Date: " & Common.Parse(dtHeader.Rows(0)("StartDate")) & "', "
        '                    'strsql &= "'B', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                End If
        '                If dtHeader.Rows(0)("EndDate") <> dtTemp.Rows(0)("CDM_END_DATE") Then
        '                    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Contract End Date", dtTemp.Rows(0)("CDM_END_DATE"), dtHeader.Rows(0)("EndDate"), "B", "E", strAryQuery)
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtHeader.Rows(0)("Index") & ","
        '                    'strsql &= "'Contract End Date: " & Common.Parse(dtHeader.Rows(0)("EndDate")) & "', "
        '                    'strsql &= "'B', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                End If
        '            End If
        '        End If
        '    End If

        '    'Add contract items
        '    strsql = "INSERT INTO CONTRACT_DIST_ITEMS "
        '    strsql &= "(CDI_GROUP_INDEX, CDI_PRODUCT_CODE, CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, "
        '    If blnGstCOD = True Then
        '        strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST_RATE, CDI_GST_TAX_CODE, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) VALUES ("
        '    Else
        '        strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) VALUES ("
        '    End If
        '    strsql &= dtHeader.Rows(0)("Index") & ", "
        '    strsql &= "'" & Common.Parse(dtItem.Rows(0)("ProductCode")) & "', "
        '    strsql &= "'" & Common.Parse(dtItem.Rows(0)("VendorItemCode")) & "', "
        '    strsql &= "'" & Common.Parse(dtItem.Rows(0)("ProductDesc")) & "', "
        '    strsql &= "'" & Common.Parse(dtItem.Rows(0)("UOM")) & "', "
        '    strsql &= dtItem.Rows(0)("UnitCost") & ", "
        '    strsql &= "'" & Common.Parse(dtItem.Rows(0)("Remark")) & "', "
        '    If blnGstCOD = True Then
        '        strsql &= "'" & dtItem.Rows(0)("GSTRate") & "', "
        '        strsql &= "'" & Common.Parse(dtItem.Rows(0)("GSTTaxCode")) & "', "
        '    Else
        '        strsql &= dtItem.Rows(0)("GST") & ", "
        '    End If
        '    strsql &= "'" & Common.Parse(dtItem.Rows(0)("CurrencyCode")) & "', "
        '    strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "', " & Common.ConvertDate(Now()) & ")"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Item Code", "", dtItem.Rows(0)("VendorItemCode"), "B", "A", strAryQuery)

        '    'strsql = "INSERT INTO au_contract "
        '    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '    'strsql &= dtHeader.Rows(0)("Index") & ", "
        '    'strsql &= "'Item Code: " & Common.Parse(dtItem.Rows(0)("VendorItemCode")) & "', "
        '    'strsql &= "'B', "
        '    'strsql &= "'A', "
        '    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '    'strsql &= Common.ConvertDate(Now()) & ")"
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            UpdateContractCatalogue = True

        '        Else
        '            UpdateContractCatalogue = False

        '        End If
        '    End If
        '    'objDb = Nothing
        'End Function

        'Public Function UpdateDeletedContractCatalogue(ByVal ds As DataRow, ByVal dsHeader As DataSet, ByVal SaveHeader As Integer) As Boolean
        '    Dim strsql, strsql2 As String
        '    Dim strProductCode As String
        '    Dim strAryQuery(0) As String
        '    Dim dr As DataRow
        '    Dim dtHeader As DataTable
        '    Dim dtItem As DataTable
        '    Dim strPRExist As String ' item exist in PR
        '    Dim strPOExist As String
        '    Dim i As Integer
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    dr = ds
        '    dtHeader = dsHeader.Tables(0)

        '    'Amendment
        '    'Update contract header
        '    If SaveHeader = 0 Then
        '        strsql = "UPDATE CONTRACT_DIST_MSTR SET "
        '        strsql &= "CDM_GROUP_DESC = '" & Common.Parse(dtHeader.Rows(0)("GroupDesc")) & "', "
        '        strsql &= "CDM_START_DATE = " & Common.ConvertDate(dtHeader.Rows(0)("StartDate")) & ", "
        '        strsql &= "CDM_END_DATE = " & Common.ConvertDate(dtHeader.Rows(0)("EndDate")) & ", "
        '        strsql &= "CDM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
        '        strsql &= "CDM_MOD_DT = " & Common.ConvertDate(Now()) & " "
        '        strsql &= "WHERE CDM_GROUP_INDEX = " & dtHeader.Rows(0)("Index")
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        'Insert into audit trail - Edit
        '        strsql1 = "SELECT * FROM CONTRACT_DIST_MSTR WHERE CDM_GROUP_INDEX = " & dtHeader.Rows(0)("Index")
        '        dtTemp = objDb.FillDt(strsql1)
        '        If dtTemp.Rows.Count > 0 Then
        '            If dtHeader.Rows(0)("GroupDesc") <> dtTemp.Rows(0)("CDM_GROUP_DESC") Or
        '               dtHeader.Rows(0)("StartDate") <> dtTemp.Rows(0)("CDM_START_DATE") Or
        '               dtHeader.Rows(0)("EndDate") <> dtTemp.Rows(0)("CDM_END_DATE") Then

        '                If dtHeader.Rows(0)("GroupDesc") <> dtTemp.Rows(0)("CDM_GROUP_DESC") Then
        '                    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Contract Desc.", dtTemp.Rows(0)("CDM_GROUP_DESC"), dtHeader.Rows(0)("GroupDesc"), "B", "E", strAryQuery)
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtHeader.Rows(0)("Index") & ","
        '                    'strsql &= "'Contract Desc.: " & Common.Parse(dtHeader.Rows(0)("GroupDesc")) & "', "
        '                    'strsql &= "'B', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                End If
        '                If dtHeader.Rows(0)("StartDate") <> dtTemp.Rows(0)("CDM_START_DATE") Then
        '                    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Contract Start Date", dtTemp.Rows(0)("CDM_START_DATE"), dtHeader.Rows(0)("StartDate"), "B", "E", strAryQuery)
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtHeader.Rows(0)("Index") & ","
        '                    'strsql &= "'Contract Start Date: " & Common.Parse(dtHeader.Rows(0)("StartDate")) & "', "
        '                    'strsql &= "'B', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                End If
        '                If dtHeader.Rows(0)("EndDate") <> dtTemp.Rows(0)("CDM_END_DATE") Then
        '                    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Contract End Date", dtTemp.Rows(0)("CDM_END_DATE"), dtHeader.Rows(0)("EndDate"), "B", "E", strAryQuery)
        '                    'strsql = "INSERT INTO au_contract "
        '                    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '                    'strsql &= dtHeader.Rows(0)("Index") & ","
        '                    'strsql &= "'Contract End Date: " & Common.Parse(dtHeader.Rows(0)("EndDate")) & "', "
        '                    'strsql &= "'B', "
        '                    'strsql &= "'E', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '                    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '                    'strsql &= Common.ConvertDate(Now()) & ")"
        '                    'Common.Insert2Ary(strAryQuery, strsql)
        '                End If
        '            End If
        '        End If
        '    End If

        '    'Add contract items
        '    strsql = "INSERT INTO CONTRACT_DIST_ITEMS "
        '    strsql &= "(CDI_GROUP_INDEX, CDI_PRODUCT_CODE, CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, "
        '    'strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) VALUES ("
        '    strsql &= "CDI_UNIT_COST, CDI_REMARK, CDI_GST_RATE, CDI_GST_TAX_CODE, CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) VALUES ("
        '    strsql &= dtHeader.Rows(0)("Index") & ", "
        '    strsql &= "'" & Common.Parse(dr("ProductCode")) & "', "
        '    strsql &= "'" & Common.Parse(dr("VendorItemCode")) & "', "
        '    strsql &= "'" & Common.Parse(dr("ProductDesc")) & "', "
        '    strsql &= "'" & Common.Parse(dr("UOM")) & "', "
        '    strsql &= dr("UnitCost") & ", "
        '    strsql &= "'" & Common.Parse(dr("Remark")) & "', "
        '    'strsql &= dr("GST") & ", "
        '    strsql &= "'" & dr("GSTRate") & ", "
        '    strsql &= "'" & Common.Parse(dr("GSTTaxCode")) & ", "
        '    strsql &= "'" & Common.Parse(dr("CurrencyCode")) & "', "
        '    strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserID")) & "', " & Common.ConvertDate(Now()) & ")"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    InsertAuditTrail(dtHeader.Rows(0)("Index"), "Item Code", "", dr("VendorItemCode"), "B", "A", strAryQuery)
        '    'strsql = "INSERT INTO au_contract "
        '    'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '    'strsql &= dtHeader.Rows(0)("Index") & ", "
        '    'strsql &= "'Item Code: " & Common.Parse(dr("VendorItemCode")) & "', "
        '    'strsql &= "'B', "
        '    'strsql &= "'A', "
        '    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '    'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '    'strsql &= Common.ConvertDate(Now()) & ")"
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    If strAryQuery(0) <> String.Empty Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            UpdateDeletedContractCatalogue = True

        '        Else
        '            UpdateDeletedContractCatalogue = False

        '        End If
        '    End If

        '    'objDb = Nothing
        'End Function

        'Public Function DeleteItem(ByVal intIndex As Integer, ByRef strNotDeleted As String, ByRef strDeleted As String) As DataSet
        '    Dim strPRExist As String ' item exist in PR
        '    Dim strPOExist As String
        '    Dim dsAllItem As New DataSet
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim ds As New DataSet
        '    Dim dtItem As New DataTable
        '    Dim dtr As DataRow

        '    dtItem.Columns.Add("Index", Type.GetType("System.Int32"))
        '    dtItem.Columns.Add("ProductCode", Type.GetType("System.String"))
        '    dtItem.Columns.Add("VendorItemCode", Type.GetType("System.String"))
        '    dtItem.Columns.Add("ProductDesc", Type.GetType("System.String"))
        '    dtItem.Columns.Add("UOM", Type.GetType("System.String"))
        '    dtItem.Columns.Add("UnitCost", Type.GetType("System.Double"))
        '    dtItem.Columns.Add("Remark", Type.GetType("System.String"))
        '    dtItem.Columns.Add("GST", Type.GetType("System.Double")) 'Chee Hong (GST enhancement - 01/07/2014)
        '    dtItem.Columns.Add("GSTRate", Type.GetType("System.String"))
        '    dtItem.Columns.Add("GSTTaxCode", Type.GetType("System.String"))
        '    dtItem.Columns.Add("CurrencyCode", Type.GetType("System.String"))

        '    dsAllItem = getCatalogueItemList(intIndex)

        '    For i As Integer = 0 To dsAllItem.Tables(0).Rows.Count - 1
        '        strPRExist = "SELECT '*' FROM PR_DETAILS " _
        '                    & "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO " _
        '                    & "AND PRM_COY_ID = PRD_COY_ID AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '                    & "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_PRODUCT_CODE = '" & Common.Parse(dsAllItem.Tables(0).Rows(i)("CDI_PRODUCT_CODE")) & "' " _
        '                    & "And PRD_CD_GROUP_INDEX = " & intIndex & " " _
        '                    & "AND PRD_PRODUCT_CODE IN " _
        '                    & "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
        '                    & "LEFT JOIN contract_dist_mstr ON CDM_GROUP_INDEX=CDI_GROUP_INDEX " _
        '                    & "WHERE CDI_GROUP_INDEX = " & intIndex & ")"

        '        strPOExist = "SELECT '*' FROM po_details " _
        '                & "LEFT JOIN po_mstr ON POM_PO_NO = POD_PO_NO " _
        '                & "AND POM_B_COY_ID = POD_COY_ID AND POM_B_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '                & "WHERE POM_PO_STATUS IN (0,7,8,9) AND POD_PRODUCT_CODE = '" & Common.Parse(dsAllItem.Tables(0).Rows(i)("CDI_PRODUCT_CODE")) & "' " _
        '                & "And POD_CD_GROUP_INDEX = " & intIndex & " " _
        '                & "AND POD_PRODUCT_CODE IN " _
        '                & "(SELECT CDI_PRODUCT_CODE FROM CONTRACT_DIST_ITEMS " _
        '                & "LEFT JOIN contract_dist_mstr ON CDM_GROUP_INDEX=CDI_GROUP_INDEX " _
        '                & "WHERE CDI_GROUP_INDEX = " & intIndex & ")"

        '        If objDb.Exist(strPRExist) > 0 Or objDb.Exist(strPOExist) > 0 Then
        '            'Store in dataset before remove
        '            strNotDeleted &= dsAllItem.Tables(0).Rows(i)("CDI_PRODUCT_CODE") & ", "
        '            dtr = dtItem.NewRow()
        '            dtr("Index") = intIndex
        '            dtr("ProductCode") = Common.Parse(dsAllItem.Tables(0).Rows(i).Item("CDI_PRODUCT_CODE"))
        '            dtr("VendorItemCode") = Common.Parse(dsAllItem.Tables(0).Rows(i).Item("CDI_VENDOR_ITEM_CODE"))
        '            dtr("ProductDesc") = Common.Parse(dsAllItem.Tables(0).Rows(i).Item("CDI_PRODUCT_DESC"))
        '            dtr("UOM") = Common.Parse(dsAllItem.Tables(0).Rows(i).Item("CDI_UOM"))
        '            dtr("UnitCost") = dsAllItem.Tables(0).Rows(i).Item("CDI_UNIT_COST")
        '            dtr("Remark") = Common.parseNull(dsAllItem.Tables(0).Rows(i).Item("CDI_REMARK"))
        '            dtr("GST") = dsAllItem.Tables(0).Rows(i).Item("CDI_GST") 'Chee Hong (GST enhancement - 01/07/2014)
        '            dtr("GSTRate") = Common.parseNull(dsAllItem.Tables(0).Rows(i).Item("CDI_GST_RATE"))
        '            dtr("GSTTaxCode") = Common.parseNull(dsAllItem.Tables(0).Rows(i).Item("CDI_GST_TAX_CODE"))
        '            dtr("CurrencyCode") = dsAllItem.Tables(0).Rows(i).Item("CDI_CURRENCY_CODE")
        '            dtItem.Rows.Add(dtr)
        '            'DeleteItem.Tables.Add(dtItem)
        '            strsql = "DELETE FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & intIndex & " "
        '            strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsAllItem.Tables(0).Rows(i)("CDI_PRODUCT_CODE")) & "'"
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            InsertAuditTrail(intIndex, "Item Code", dsAllItem.Tables(0).Rows(i).Item("CDI_VENDOR_ITEM_CODE"), "", "B", "D", strAryQuery)
        '            'strsql = "INSERT INTO au_contract "
        '            'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '            'strsql &= intIndex & ","
        '            'strsql &= "'Item Code: " & Common.Parse(dsAllItem.Tables(0).Rows(i).Item("CDI_VENDOR_ITEM_CODE")) & "', "
        '            'strsql &= "'B', "
        '            'strsql &= "'D', "
        '            'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '            'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '            'strsql &= Common.ConvertDate(Now()) & ")"
        '            'Common.Insert2Ary(strAryQuery, strsql)
        '        Else
        '            strDeleted &= dsAllItem.Tables(0).Rows(i)("CDI_PRODUCT_CODE") & ", "

        '            strsql = "DELETE FROM CONTRACT_DIST_ITEMS "
        '            strsql &= "WHERE CDI_GROUP_INDEX = " & intIndex & " "
        '            strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsAllItem.Tables(0).Rows(i)("CDI_PRODUCT_CODE")) & "'"
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            InsertAuditTrail(intIndex, "Item Code", dsAllItem.Tables(0).Rows(i).Item("CDI_VENDOR_ITEM_CODE"), "", "B", "D", strAryQuery)
        '            'strsql = "INSERT INTO au_contract "
        '            'strsql &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '            'strsql &= intIndex & ","
        '            'strsql &= "'Item Code: " & Common.Parse(dsAllItem.Tables(0).Rows(i).Item("CDI_VENDOR_ITEM_CODE")) & "', "
        '            'strsql &= "'B', "
        '            'strsql &= "'D', "
        '            'strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '            'strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '            'strsql &= Common.ConvertDate(Now()) & ")"
        '            'Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    Next

        '    If strAryQuery(0) <> String.Empty Then
        '        objDb.BatchExecute(strAryQuery)
        '        If dtItem.Rows.Count > 0 Then
        '            ds.Tables.Add(dtItem)
        '        End If
        '    End If

        '    DeleteItem = ds
        'End Function

        'Public Function GetCoyId(ByVal byname As String) As String
        '    Dim strsql As String

        '    strsql = "SELECT CV_S_COY_ID " _
        '        & "FROM company_vendor " _
        '        & "INNER JOIN COMPANY_MSTR ON CV_S_COY_ID = CM_COY_ID AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' " _
        '        & "AND (CM_COY_TYPE = 'BOTH' OR CM_COY_TYPE = 'VENDOR') " _
        '        & "WHERE CV_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND CM_COY_NAME='" & Common.Parse(byname) & "'"

        '    If objDb.GetVal(strsql) <> "" Then
        '        GetCoyId = objDb.GetVal(strsql)

        '    Else
        '        GetCoyId = ""
        '    End If

        'End Function

        'Public Function GetConGroupIndex(ByVal byname As String, ByVal byVendor As String) As Integer
        '    Dim strsql As String

        '    strsql = "SELECT CDM_GROUP_INDEX " _
        '            & "FROM CONTRACT_DIST_MSTR " _
        '            & "LEFT JOIN CONTRACT_DIST_COY ON CDM_GROUP_INDEX = CDC_GROUP_INDEX " _
        '            & "WHERE CDM_GROUP_CODE = '" & Common.Parse(byname) & "' " _
        '            & "AND CDM_S_COY_ID = '" & Common.Parse(byVendor) & "' " _
        '            & "AND CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "'"

        '    If objDb.GetVal(strsql) <> "" Then
        '        GetConGroupIndex = objDb.GetVal(strsql)

        '    Else
        '        GetConGroupIndex = 0
        '    End If

        'End Function

        Public Function BIM(ByVal dsProduct As DataSet, ByVal strMode As String, ByVal strImageIndex As String, ByVal strNewProductCode As String, ByVal OldVendor As String, Optional ByVal OldVendorItemCode As String = "", Optional ByVal OldItemName As String = "", Optional ByVal strUpload As String = "") As String
            'Public Function BIM(ByVal dsProduct As DataSet, ByVal strMode As String, ByVal strImageIndex As String, ByRef strNewProductCode As String, ByVal OldVendor As String, Optional ByVal OldVendorItemCode As String = "", Optional ByVal OldItemName As String = "", Optional ByVal strUpload As String = "") As String
            Dim strsql, strsql2 As String
            Dim strProductCode As String
            Dim strAryQuery(0) As String
            Dim tempNull As String

            Dim dtr As DataRow

            Dim docFileName As String
            Dim imgFileName As String
            Dim docFileName2 As String

            Dim INDEX As String


            Select Case strMode
                Case "add"
                    GetLatestItemNo(strProductCode)
                    strNewProductCode = strProductCode

                    strsql = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE = SP_PARAM_VALUE + 1 WHERE SP_PARAM_NAME = 'ItemLastUsedNo' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    strsql = "INSERT INTO PRODUCT_MSTR (PM_S_COY_ID, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, "
                    strsql &= "PM_PRODUCT_DESC,PM_REF_NO,PM_LONG_DESC,PM_CATEGORY_NAME,PM_ACCT_CODE, PM_UOM,"
                    strsql &= "PM_SAFE_QTY,PM_ORD_MIN_QTY,PM_ORD_MAX_QTY,PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO, "
                    strsql &= "PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT,PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,"
                    strsql &= "PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_REMARKS, "
                    strsql &= "PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID, "
                    strsql &= "PM_DELETED,PM_PRODUCT_FOR, PM_ENT_BY, PM_ENT_DT, PM_PREFER_S_COY_ID_TAX_ID, PM_1ST_S_COY_ID_TAX_ID, PM_2ND_S_COY_ID_TAX_ID, PM_3RD_S_COY_ID_TAX_ID, PM_ITEM_TYPE, PM_IQC_IND, PM_MAX_INV_QTY, PM_MANUFACTURER, PM_CAT_CODE) VALUES ("
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
                    strsql &= "'" & Common.Parse(strProductCode) & "', " ' PM_PRODUCT_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("AccCode")) & "', " ' PM_ACCT_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("MinInv")) & "', " ' SAFE_QTY
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Min")) & "', " ' PM_ORD_MIN_QTY
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Max")) & "', " ' PM_ORD_MAX_QTY
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Packing")) & "', " ' PM_PACKING_REQ
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARKS
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Prefer")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Deleted")) & "', " ' PM_DELETED
                    strsql &= "'B', '" & HttpContext.Current.Session("UserId") & "', getdate(), " ' PM_PRODUCT_FOR
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("PreferTax")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("1stTax")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("2ndTax")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("3rdTax")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd2")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("MaxInvQty")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CatCode")) & "') "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'If Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) & "'"
                    End If
                    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
                    strsql &= "'P', "
                    strsql &= "" & tempNull & ", "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "getdate()) "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) & "'"
                    End If
                    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
                    strsql &= "'1', "
                    strsql &= "" & tempNull & ", "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "getdate()) "
                    Common.Insert2Ary(strAryQuery, strsql)
                    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) & "'"
                    End If
                    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
                    strsql &= "'2', "
                    strsql &= "" & tempNull & ", "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "getdate()) "
                    Common.Insert2Ary(strAryQuery, strsql)
                    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) & "'"
                    End If
                    strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                    strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
                    strsql &= "'3', "
                    strsql &= "" & tempNull & ", "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "getdate()) "
                    Common.Insert2Ary(strAryQuery, strsql)
                    'End If

                    ''Insert into Default Buyer Catalogue
                    'strsql = "INSERT INTO BUYER_CATALOGUE_ITEMS (BCU_CAT_INDEX, BCU_PRODUCT_CODE, BCU_SOURCE, BCU_S_COY_ID, BCU_ENT_BY, BCU_ENT_DATETIME) "
                    'strsql &= "VALUES ((SELECT BCM_CAT_INDEX FROM BUYER_CATALOGUE_MSTR WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "'), "
                    'strsql &= "'" & Common.Parse(strProductCode) & "', '', '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', "
                    'strsql &= "'" & HttpContext.Current.Session("UserId") & "', getdate())"
                    'Common.Insert2Ary(strAryQuery, strsql)


                    strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'D'"


                    docFileName = objDb.GetVal(strsql)




                    'For Document attachment
                    strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
                    strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'D' AND PA_STATUS = 'T' "
                    Common.Insert2Ary(strAryQuery, strsql)



                    strsql = "UPDATE PRODUCT_ATTACHMENT SET PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'InsertAuditTrailBIM("" & INDEX & "", "Item Code", "Modify", "File Attachment", "''", "'" & docFileName & "'", strAryQuery)

                    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
                    strsql &= "AND PA_TYPE = 'D' AND PA_STATUS = 'T' AND PA_SOURCE = 'B' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    If docFileName <> "" Then
                        InsertAuditTrailBIM(0, "Item Master", "Add", "File Attachment", "", "" & docFileName & "", strAryQuery, True)
                    End If



                Case "mod"
                    'Michelle (14/5/2011) - To update the inventory mstr if for user change the Item code or name
                    If OldVendorItemCode <> Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) Or OldItemName <> Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) Then
                        strsql = "UPDATE INVENTORY_MSTR SET "
                        strsql &= "IM_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', "
                        strsql &= "IM_INVENTORY_NAME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "' "
                        strsql &= "WHERE IM_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' AND IM_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "'"
                    End If

                    strProductCode = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))

                    strsql = "UPDATE PRODUCT_MSTR SET "
                    strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
                    strsql &= "PM_PRODUCT_DESC = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
                    strsql &= "PM_REF_NO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
                    strsql &= "PM_LONG_DESC = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
                    strsql &= "PM_CATEGORY_NAME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
                    strsql &= "PM_ACCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("AccCode")) & "', " ' PM_ACCT_CODE
                    strsql &= "PM_UOM = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
                    strsql &= "PM_SAFE_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("MinInv")) & "', " ' PM_SAFE_QTY
                    strsql &= "PM_ORD_MIN_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Min")) & "', " ' PM_ORD_MIN_QTY
                    strsql &= "PM_ORD_MAX_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Max")) & "', " ' PM_ORD_MAX_QTY
                    strsql &= "PM_PRODUCT_BRAND = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
                    strsql &= "PM_PRODUCT_MODEL = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL
                    strsql &= "PM_DRAW_NO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
                    strsql &= "PM_VERS_NO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
                    strsql &= "PM_GROSS_WEIGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
                    strsql &= "PM_NET_WEIGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
                    strsql &= "PM_LENGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
                    strsql &= "PM_WIDTH = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
                    strsql &= "PM_HEIGHT = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
                    strsql &= "PM_VOLUME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
                    strsql &= "PM_COLOR_INFO = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
                    strsql &= "PM_HSC_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
                    strsql &= "PM_PACKING_REQ = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Packing")) & "', " ' PM_PACKING_REQ
                    strsql &= "PM_REMARKS = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARKS
                    strsql &= "PM_DELETED = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Deleted")) & "', " ' PM_DELETED
                    strsql &= "PM_PREFER_S_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) & "', " ' PM_PREFER_S_COY_ID
                    strsql &= "PM_1ST_S_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) & "', " ' PM_PREFER_S_COY_ID
                    strsql &= "PM_2ND_S_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) & "', " ' PM_PREFER_S_COY_ID
                    strsql &= "PM_3RD_S_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) & "', " ' PM_PREFER_S_COY_ID
                    strsql &= "PM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', " ' PM_MOD_BY
                    strsql &= "PM_MOD_DT = getdate(), " ' PM_MOD_DT
                    strsql &= "PM_PREFER_S_COY_ID_TAX_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("PREFERTax")) & "', "
                    strsql &= "PM_1ST_S_COY_ID_TAX_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("1stTax")) & "', "
                    strsql &= "PM_2ND_S_COY_ID_TAX_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("2ndTax")) & "', "
                    strsql &= "PM_3RD_S_COY_ID_TAX_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("3rdTax")) & "', "
                    strsql &= "PM_MAX_INV_QTY = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("MaxInvQty")) & "', "
                    strsql &= "PM_MANUFACTURER = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("Manu")) & "', "
                    strsql &= "PM_ITEM_TYPE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) & "', "
                    strsql &= "PM_CAT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CatCode")) & "', "
                    strsql &= "PM_IQC_IND = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("rd2")) & "' "
                    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    ''physical delete from Default Buyer Catalogue for deactivated item
                    'If Common.Parse(dsProduct.Tables(0).Rows(0)("Deleted")) = "Y" Then
                    '    strsql = "DELETE FROM BUYER_CATALOGUE_ITEMS WHERE BCU_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                    '    strsql &= "AND BCU_CAT_INDEX IN (SELECT BCM_CAT_INDEX "
                    '    strsql &= "FROM BUYER_CATALOGUE_MSTR WHERE BCM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                    '    Common.Insert2Ary(strAryQuery, strsql)
                    'End If

                    strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
                    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"

                    INDEX = objDb.GetVal(strsql)


                    strsql = "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'T' "

                    If objDb.Exist(strsql) Then
                        strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D'"


                        docFileName = objDb.GetVal(strsql)

                        'strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                        'strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I'"

                        'imgFileName = Common.parseNull(objDb.GetVal(strsql))
                        'For Document attachment
                        strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
                        strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'T' "
                        Common.Insert2Ary(strAryQuery, strsql)

                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Add", "File Attachment", "", "" & docFileName & "", strAryQuery)
                    End If

                    strsql = "SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'D' "

                    If objDb.Exist(strsql) Then
                        strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT  "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D'"


                        docFileName = objDb.GetVal(strsql)

                        strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT  "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I'"

                        imgFileName = Common.parseNull(objDb.GetVal(strsql))

                        strsql = "DELETE FROM PRODUCT_ATTACHMENT  "
                        strsql &= "WHERE PA_ATTACH_INDEX IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'D' AND PA_STATUS = 'D') "
                        Common.Insert2Ary(strAryQuery, strsql)

                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Delete", "File Attachment", "" & docFileName & "", "", strAryQuery)


                    End If


                    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                    strsql &= "AND PA_TYPE = 'D' AND (PA_STATUS = 'T' OR PA_STATUS = 'D') AND PA_SOURCE = 'B' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'Dim INDEX As String
                    'strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
                    'strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"

                    'INDEX = objDb.GetVal(strsql)

                    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR  "
                    strsql &= "WHERE PV_PRODUCT_INDEX = '" & INDEX & "'"


                    If objDb.Exist(strsql) = 0 Then


                        'If Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) <> "" Then
                        If Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) = "" Then
                            tempNull = "NULL"
                        Else
                            tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) & "'"
                        End If
                        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                        strsql &= "" & INDEX & ", "
                        strsql &= "'P', "
                        strsql &= "" & tempNull & ", "
                        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
                        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                        strsql &= "getdate()) "
                        Common.Insert2Ary(strAryQuery, strsql)
                        'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) <> "" Then
                        If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) = "" Then
                            tempNull = "NULL"
                        Else
                            tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) & "'"
                        End If
                        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                        strsql &= "" & INDEX & ", "
                        strsql &= "'1', "
                        strsql &= "" & tempNull & ", "
                        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
                        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                        strsql &= "getdate()) "
                        Common.Insert2Ary(strAryQuery, strsql)
                        'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) <> "" Then
                        If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) = "" Then
                            tempNull = "NULL"
                        Else
                            tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) & "'"
                        End If
                        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                        strsql &= "" & INDEX & ", "
                        strsql &= "'2', "
                        strsql &= "" & tempNull & ", "
                        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
                        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                        strsql &= "getdate()) "
                        Common.Insert2Ary(strAryQuery, strsql)
                        'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) <> "" Then
                        If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) = "" Then
                            tempNull = "NULL"
                        Else
                            tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) & "'"
                        End If
                        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
                        strsql &= "" & INDEX & ", "
                        strsql &= "'3', "
                        strsql &= "" & tempNull & ", "
                        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
                        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
                        strsql &= "getdate()) "
                        Common.Insert2Ary(strAryQuery, strsql)
                        'End If

                    End If

                    'If Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) & "'"
                    End If
                    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = " & tempNull & ", "
                    strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
                    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "PV_MOD_DATETIME = getdate() "
                    strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    strsql &= "And PV_VENDOR_TYPE = 'P' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) & "'"
                    End If
                    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
                    strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
                    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "PV_MOD_DATETIME = getdate() "
                    strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    strsql &= "And PV_VENDOR_TYPE = '1' "
                    Common.Insert2Ary(strAryQuery, strsql)
                    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) & "'"
                    End If
                    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
                    strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
                    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "PV_MOD_DATETIME = getdate() "
                    strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    strsql &= "And PV_VENDOR_TYPE = '2' "
                    Common.Insert2Ary(strAryQuery, strsql)
                    'ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) <> "" Then
                    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) = "" Then
                        tempNull = "NULL"
                    Else
                        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) & "'"
                    End If
                    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
                    strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
                    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
                    strsql &= "PV_MOD_DATETIME = getdate() "
                    strsql &= "WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    strsql &= "And PV_VENDOR_TYPE = '3' "
                    Common.Insert2Ary(strAryQuery, strsql)
                    'End If
                    'insert into audit trail - edit
                    Dim strsql1 As String
                    Dim dtTemp As New DataTable
                    strsql1 = "SELECT * FROM PIM_VENDOR WHERE PV_PRODUCT_INDEX = " & INDEX & " "
                    dtTemp = objDb.FillDt(strsql1)
                    If dtTemp.Rows.Count > 0 Then
                        If dsProduct.Tables(0).Rows(0)("Lead1") <> CStr(Common.parseNull(dtTemp.Rows(0)("PV_LEAD_TIME"))) Or
                           dsProduct.Tables(0).Rows(0)("Lead2") <> CStr(Common.parseNull(dtTemp.Rows(1)("PV_LEAD_TIME"))) Or
                           dsProduct.Tables(0).Rows(0)("Lead3") <> CStr(Common.parseNull(dtTemp.Rows(2)("PV_LEAD_TIME"))) Or
                           dsProduct.Tables(0).Rows(0)("LeadP") <> CStr(Common.parseNull(dtTemp.Rows(3)("PV_LEAD_TIME"))) Then

                            If dsProduct.Tables(0).Rows(0)("Lead1") <> CStr(Common.parseNull(dtTemp.Rows(0)("PV_LEAD_TIME"))) Then

                                InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "1st Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(0)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("Lead1"), strAryQuery)
                            End If
                            If dsProduct.Tables(0).Rows(0)("Lead2") <> CStr(Common.parseNull(dtTemp.Rows(1)("PV_LEAD_TIME"))) Then

                                InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "2nd Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(1)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("Lead2"), strAryQuery)
                            End If
                            If dsProduct.Tables(0).Rows(0)("Lead3") <> CStr(Common.parseNull(dtTemp.Rows(2)("PV_LEAD_TIME"))) Then

                                InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "3rd Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(2)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("Lead3"), strAryQuery)
                            End If
                            If dsProduct.Tables(0).Rows(0)("LeadP") <> CStr(Common.parseNull(dtTemp.Rows(3)("PV_LEAD_TIME"))) Then

                                InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Modify", "Preferred Vendor Order Lead Time", Common.parseNull(dtTemp.Rows(3)("PV_LEAD_TIME")), dsProduct.Tables(0).Rows(0)("LeadP"), strAryQuery)
                            End If
                        End If
                    End If

            End Select

            'Check whether the item should add/remove to/from Default Purchaser Catalogue
            If Common.Parse(dsProduct.Tables(0).Rows(0)("Deleted")) <> "Y" Then
                Dim objDBAccess As New EAD.DBCom
                If objDBAccess.GetCount("BUYER_CATALOGUE_MSTR, BUYER_CATALOGUE_ITEMS", "WHERE BCU_CAT_INDEX = BCM_CAT_INDEX AND BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' AND BCU_PRODUCT_CODE='" & Common.Parse(strProductCode) & "' ") = 0 And
                    objDBAccess.GetCount("BUYER_CATALOGUE_MSTR", "WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' ") > 0 Then
                    'Insert into Default Buyer Catalogue
                    strsql = "INSERT INTO BUYER_CATALOGUE_ITEMS (BCU_CAT_INDEX, BCU_PRODUCT_CODE, BCU_SOURCE, BCU_S_COY_ID, BCU_ENT_BY, BCU_ENT_DATETIME) "
                    strsql &= "VALUES ((SELECT BCM_CAT_INDEX FROM BUYER_CATALOGUE_MSTR WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "'), "
                    strsql &= "'" & Common.Parse(strProductCode) & "', '', '" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', "
                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', getdate())"
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            Else
                'physical delete from Default Buyer Catalogue for deactivated item
                strsql = "DELETE FROM BUYER_CATALOGUE_ITEMS WHERE BCU_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                strsql &= "AND BCU_CAT_INDEX IN (SELECT BCM_CAT_INDEX "
                strsql &= "FROM BUYER_CATALOGUE_MSTR WHERE BCM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
                Common.Insert2Ary(strAryQuery, strsql)
            End If

            If Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) = "ST" Or Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) = "MI" Then
                strsql = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE "
                strsql &= "IM_ITEM_CODE = '" & OldVendor & "' "
                strsql &= "AND IM_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("COYID")) & "' "
                If objDb.Exist(strsql) > 0 Then
                    Dim IM_INVENTORY_INDEX As String
                    strsql = "SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR  "
                    strsql &= "WHERE IM_ITEM_CODE = '" & OldVendor & "' "
                    strsql &= "AND IM_COY_ID = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("COYID")) & "' "

                    IM_INVENTORY_INDEX = objDb.GetVal(strsql)

                    strsql = "UPDATE INVENTORY_MSTR SET IM_IQC_IND = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("RD2")) & "', IM_ITEM_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VENDORITEMCODE")) & "', IM_INVENTORY_NAME = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ITEMNAME")) & "' "
                    strsql &= "WHERE IM_INVENTORY_INDEX = '" & IM_INVENTORY_INDEX & "' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Else
                    strsql = "INSERT INTO INVENTORY_MSTR (IM_COY_ID, IM_ITEM_CODE, IM_INVENTORY_NAME, IM_IQC_IND) VALUES ("
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("COYID")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VENDORITEMCODE")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ITEMNAME")) & "', "
                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("RD2")) & "') "
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            ElseIf Common.Parse(dsProduct.Tables(0).Rows(0)("rd1")) = "SP" Then
                strsql = "SELECT '*' FROM INVENTORY_DETAIL WHERE ID_INVENTORY_INDEX = " &
                       "(SELECT IM_INVENTORY_INDEX FROM INVENTORY_MSTR WHERE IM_ITEM_CODE = '" & Common.Parse(OldVendor) & "' " &
                       "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' LIMIT 1) "

                If objDb.Exist(strsql) = 0 Then
                    strsql = "DELETE FROM INVENTORY_MSTR "
                    strsql &= "WHERE IM_ITEM_CODE = '" & Common.Parse(OldVendor) & "' "
                    strsql &= "AND IM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                    Common.Insert2Ary(strAryQuery, strsql)
                End If
            End If

            ' Should Use Function
            'Dim INDEX As String
            'strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
            'strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"

            'INDEX = objDb.GetVal(strsql)

            'strsql = "DELETE FROM PIM_VENDOR "
            'strsql &= "WHERE PV_PRODUCT_INDEX = '" & INDEX & "' "
            'Common.Insert2Ary(strAryQuery, strsql)


            'If Common.Parse(dsProduct.Tables(0).Rows(0)("PREFER")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = 'P' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("LeadP")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 And Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = 'P' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'P', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCodeP")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '    strsql &= "And PV_VENDOR_TYPE = 'P' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If

            'If Common.Parse(dsProduct.Tables(0).Rows(0)("1st")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = '1' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead1")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = '1' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'1', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode1")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '    strsql &= "And PV_VENDOR_TYPE = '1' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If

            'If Common.Parse(dsProduct.Tables(0).Rows(0)("2nd")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = '2' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead2")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = '2' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'2', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode2")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '    strsql &= "And PV_VENDOR_TYPE = '2' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If

            'If Common.Parse(dsProduct.Tables(0).Rows(0)("3rd")) <> "" Then
            '    strsql = "SELECT PV_PRODUCT_INDEX FROM PIM_VENDOR WHERE "
            '    strsql &= "PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " AND "
            '    strsql &= "PV_VENDOR_TYPE = '3' "
            '    Dim tempNull As String
            '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) = "" Then
            '        tempNull = "NULL"
            '    Else
            '        tempNull = "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Lead3")) & "'"
            '    End If
            '    If objDb.Exist(strsql) > 0 Then
            '        strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME= " & tempNull & ", "
            '        strsql &= "PV_VENDOR_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
            '        strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "PV_MOD_DATETIME = getdate() "
            '        strsql &= "WHERE PV_PRODUCT_INDEX = " & objDb.GetLatestInsertedID("PRODUCT_MSTR") & " "
            '        strsql &= "And PV_VENDOR_TYPE = '3' "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    Else
            '        strsql = "INSERT INTO PIM_VENDOR (PV_PRODUCT_INDEX, PV_VENDOR_TYPE, PV_LEAD_TIME, PV_VENDOR_CODE, PV_ENT_BY, PV_ENT_DATETIME) VALUES ("
            '        strsql &= "" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & ", "
            '        strsql &= "'3', "
            '        strsql &= "" & tempNull & ", "
            '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VenCode3")) & "', "
            '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
            '        strsql &= "getdate()) "
            '        Common.Insert2Ary(strAryQuery, strsql)
            '    End If
            'Else
            '    strsql = "UPDATE PIM_VENDOR SET PV_LEAD_TIME = NULL, "
            '    strsql &= "PV_VENDOR_CODE = '', "
            '    strsql &= "PV_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
            '    strsql &= "PV_MOD_DATETIME = getdate() "
            '    strsql &= "WHERE PV_PRODUCT_INDEX = '" & objDb.GetLatestInsertedID("PRODUCT_MSTR") & "' "
            '    strsql &= "And PV_VENDOR_TYPE = '3' "
            '    Common.Insert2Ary(strAryQuery, strsql)
            'End If
            Dim strImg As String

            If strMode = "mod" Then
                strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I' AND PA_STATUS = 'T'"
            Else
                strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'I' AND PA_STATUS = 'T'"
            End If

            strImg = Common.parseNull(objDb.GetVal(strsql))

            If strImg = "" Then
                strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT  "
                strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I'"
            Else
                If strMode = "mod" Then
                    strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I' AND PA_STATUS = 'T'"
                Else
                    strsql = "SELECT IFNULL(PA_ATTACH_FILENAME,"""") FROM PRODUCT_ATTACHMENT_TEMP  "
                    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' AND PA_TYPE = 'I'"
                End If

            End If

            imgFileName = Common.parseNull(objDb.GetVal(strsql))

            If strImageIndex <> "" Then
                strsql = "DELETE FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                strsql &= "AND PA_TYPE = 'I' "
                strsql &= "AND PA_PRODUCT_CODE IN (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                'strsql &= "AND PA_SOURCE = 'B' AND PA_STATUS = 'T' AND PA_TYPE = 'I') "
                strsql &= "AND PA_SOURCE = 'B' AND PA_TYPE = 'I') "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = "SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "'"
                strsql &= " AND PA_SOURCE = 'B' AND PA_TYPE = 'I'"


                If objDb.Exist(strsql) Then
                    If strImg = "" And strMode = "mod" Then

                        InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Delete", "Picture Attachment", "" & imgFileName & "", "", strAryQuery)

                    End If
                End If


                strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
                strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & Common.Parse(strImageIndex) & "' AND PA_STATUS <> 'D' AND PA_SOURCE = 'B' "
                Common.Insert2Ary(strAryQuery, strsql)

                If strImg <> "" And strMode = "mod" Then
                    InsertAuditTrailBIM("" & INDEX & "", "Item Master", "Add", "Picture Attachment", "", "" & imgFileName & "", strAryQuery)
                ElseIf strImg <> "" And strMode = "add" Then
                    InsertAuditTrailBIM(0, "Item Master", "Add", "Picture Attachment", "", "" & imgFileName & "", strAryQuery, True)
                End If

                strsql = "UPDATE PRODUCT_ATTACHMENT SET PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                strsql &= "WHERE PA_PRODUCT_CODE = (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & Common.Parse(strImageIndex) & "' AND PA_STATUS <> 'D' AND PA_SOURCE = 'B') "
                Common.Insert2Ary(strAryQuery, strsql)


                'strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & objDb.GetVal("SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_TYPE = 'I' AND PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' ")
                strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = "
                strsql &= "(SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' AND PA_TYPE = 'I') "
                strsql &= " WHERE PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
                Common.Insert2Ary(strAryQuery, strsql)

                strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & Common.Parse(strImageIndex) & "' "
                strsql &= "AND PA_TYPE = 'I' AND PA_STATUS = 'T' AND PA_SOURCE = 'B' "
                Common.Insert2Ary(strAryQuery, strsql)

            End If

            If objDb.BatchExecute(strAryQuery) Then
                BIM = True
            Else
                BIM = False
            End If

        End Function

        'Public Function validateItemCode(ByVal strItemCode As String, ByVal strVendorCompany As String) As Boolean
        '    'Return False if found duplicate
        '    Dim strsql As String
        '    strsql = "SELECT PM_PRODUCT_INDEX FROM product_mstr_temp WHERE PM_VENDOR_ITEM_CODE = '" & strItemCode & "' AND PM_S_COY_ID = '" & strVendorCompany & "'"
        '    objDb.Get1Column("product_mstr_temp", "PM_PRODUCT_INDEX", "WHERE PM_VENDOR_ITEM_CODE = '" & strItemCode & "' AND PM_S_COY_ID = '" & strVendorCompany & "'")
        'End Function

        'Public Function insertProductTemp(ByVal dsProduct As DataSet, ByVal strImageIndex As String) As String
        '    Dim strsql As String
        '    Dim strProductCode As String
        '    Dim strAryQuery(0) As String
        '    GetLatestItemNo(strProductCode)
        '    strsql = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE = SP_PARAM_VALUE + 1 WHERE SP_PARAM_NAME = 'ItemLastUsedNo' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' insert into PRODUCT_MSTR_TEMP table
        '    strsql = "INSERT INTO PRODUCT_MSTR_TEMP (PM_S_COY_ID, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, "
        '    strsql &= "PM_PRODUCT_DESC,PM_REF_NO,PM_LONG_DESC,PM_CATEGORY_NAME,PM_GST_CODE,PM_TAX_PERC, PM_TAX_ID, PM_UOM,"
        '    strsql &= "PM_UNIT_COST,PM_CURRENCY_CODE,PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO, "
        '    strsql &= "PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT,PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,"
        '    strsql &= "PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_REMARKS, PM_PRODUCT_FOR, PM_STATUS,PM_ACTION) VALUES ("
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '    strsql &= "'" & Common.Parse(strProductCode) & "', " ' PM_PRODUCT_CODE
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Tax")) & "', " ' PM_GST_CODE
        '    If IsDBNull(dsProduct.Tables(0).Rows(0)("Perc")) Or Common.parseNull(dsProduct.Tables(0).Rows(0)("Perc")) = "" Then
        '        strsql &= "'0', " ' PM_GST_CODE
        '    Else
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Perc")) & "', " ' PM_GST_CODE
        '    End If
        '    If IsDBNull(dsProduct.Tables(0).Rows(0)("TaxId")) Or Common.parseNull(dsProduct.Tables(0).Rows(0)("TaxId")) = "" Then
        '        strsql &= "'1', " ' PM_TAX_ID
        '    Else
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("TaxId")) & "', " ' PM_TAX_ID
        '    End If
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '    If Common.Parse(dsProduct.Tables(0).Rows(0)("Price")) = "" Then
        '        strsql &= "NULL, " ' PM_UNIT_COST
        '    Else
        '        strsql &= dsProduct.Tables(0).Rows(0)("Price") & ", " ' PM_UNIT_COST
        '    End If
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CurrencyCode")) & "', " ' PM_CURRENCY_CODE
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL

        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Packing")) & "', " ' PM_PACKING_REQ
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARKS
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("PM_PRODUCT_FOR")) & "', " ' PM_PRODUCT_FOR
        '    'strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), " ' PM_ENT_BY, PM_ENT_DT
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("status")) & "', " ' PM_STATUS
        '    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("action")) & "') " ' PM_MGMT_TEXT
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    '' insert into PRODUCT_DISCGRP_TEMP
        '    'Dim i As Integer
        '    'For i = 0 To dsProduct.Tables(1).Rows.Count - 1
        '    '    If Common.Parse(dsProduct.Tables(1).Rows(i)("chk")) = "1" Then
        '    '        strsql = "INSERT INTO PRODUCT_DISCGRP_TEMP (PDT_PRODUCT_CODE, PDT_DISC_INDEX, PDT_DISC_CODE, "
        '    '        strsql &= "PDT_DISC_PRICE, PDT_DISC_REMARK, PDT_TYPE) VALUES ("
        '    '        strsql &= "'" & Common.Parse(strProductCode) & "', "
        '    '        strsql &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ", "
        '    '        strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscCode")) & "', "
        '    '        strsql &= dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '    '        strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "', "
        '    '        strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("Type")) & "') "
        '    '        Common.Insert2Ary(strAryQuery, strsql)
        '    '    End If
        '    'Next

        '    ' upload  image
        '    If strImageIndex <> "" Then
        '        Dim sLatest As String = objDb.Get1Column("PRODUCT_MSTR_TEMP", "ISNULL(MAX(PM_PRODUCT_INDEX),0) ")

        '        strsql = "UPDATE PRODUCT_MSTR_TEMP SET PM_PRODUCT_IMAGE = '" & strImageIndex & "' "
        '        strsql &= "WHERE PM_PRODUCT_INDEX = " & sLatest & "  "
        '        strsql &= "AND PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '        'strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET "
        '        'strsql &= "PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '        'strsql &= "WHERE PA_ATTACH_INDEX = '" & strImageIndex & "' "
        '        'Common.Insert2Ary(strAryQuery, strsql)
        '    End If

        '    ' ai chu add on 19/09/2005
        '    ' delete all status = 'D' (deleted before user save the record)
        '    ' update status = 'T' to status = 'U' 
        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE "
        '    strsql &= "PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
        '    strsql &= "AND PA_STATUS = 'D' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'U' "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
        '    strsql &= "AND PA_STATUS = 'T' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' upload attachment
        '    ' need to update PA_PRODUCT_CODE because when it's uploaded, the product code is set to sessionid
        '    strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET "
        '    strsql &= "PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
        '    'strsql &= "AND PA_TYPE = 'D'"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If objDb.BatchExecute(strAryQuery) Then
        '        Dim objMail As New Email
        '        objMail.sendNotification(EmailType.ListPriceApproval, HttpContext.Current.Session("UserId"), "", Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")), strProductCode, "")
        '        objMail = Nothing
        '        insertProductTemp = strProductCode
        '    End If
        '    'Dim strsql As String
        '    'Dim strProductCode As String
        '    'Dim strAryQuery(0) As String
        '    'GetLatestItemNo(strProductCode)
        '    'strsql = "UPDATE SYSTEM_PARAM SET SP_PARAM_VALUE = SP_PARAM_VALUE + 1 WHERE SP_PARAM_NAME = 'ItemLastUsedNo' "
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    '' insert into PRODUCT_MSTR_TEMP table
        '    'strsql = "INSERT INTO PRODUCT_MSTR_TEMP (PM_S_COY_ID, PM_PRODUCT_CODE, PM_CATEGORY_NAME, "
        '    'strsql &= "PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_GST_CODE, PM_MGMT_CODE, "
        '    'strsql &= "PM_CURRENCY_CODE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL, PM_PRODUCT_TYPE, "
        '    'strsql &= "PM_ENT_BY, PM_ENT_DT, PM_STATUS, PM_MGMT_TEXT, PM_ACTION) VALUES ("
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '    'strsql &= "'" & Common.Parse(strProductCode) & "', " ' PM_PRODUCT_CODE
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CategoryName")) & "', " ' PM_CATEGORY_NAME
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '    'If Common.Parse(dsProduct.Tables(0).Rows(0)("UnitCost")) = "" Then
        '    '    strsql &= "NULL, " ' PM_UNIT_COST
        '    'Else
        '    '    strsql &= dsProduct.Tables(0).Rows(0)("UnitCost") & ", " ' PM_UNIT_COST
        '    'End If

        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("gstcode")) & "', " ' PM_GST_CODE
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmtcode")) & "', " ' PM_MGMT_CODE
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("productbrand")) & "', " ' PM_PRODUCT_BRAND
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("productmodel")) & "', " ' PM_PRODUCT_MODEL
        '    'strsql &= "NULL, " ' PM_PRODUCT_TYPE
        '    'strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), " ' PM_ENT_BY, PM_ENT_DT
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("status")) & "', " ' PM_STATUS
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmttext")) & "', " ' PM_MGMT_TEXT
        '    'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("action")) & "') " ' PM_MGMT_TEXT
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    '' insert into PRODUCT_DISCGRP_TEMP
        '    'Dim i As Integer
        '    'For i = 0 To dsProduct.Tables(1).Rows.Count - 1
        '    '    If Common.Parse(dsProduct.Tables(1).Rows(i)("chk")) = "1" Then
        '    '        strsql = "INSERT INTO PRODUCT_DISCGRP_TEMP (PDT_PRODUCT_CODE, PDT_DISC_INDEX, PDT_DISC_CODE, "
        '    '        strsql &= "PDT_DISC_PRICE, PDT_DISC_REMARK, PDT_TYPE) VALUES ("
        '    '        strsql &= "'" & Common.Parse(strProductCode) & "', "
        '    '        strsql &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ", "
        '    '        strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscCode")) & "', "
        '    '        strsql &= dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '    '        strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "', "
        '    '        strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("Type")) & "') "
        '    '        Common.Insert2Ary(strAryQuery, strsql)
        '    '    End If
        '    'Next

        '    '' upload  image
        '    'If strImageIndex <> "" Then
        '    '    strsql = "UPDATE PRODUCT_MSTR_TEMP SET PM_PRODUCT_IMAGE = '" & strImageIndex & "' "
        '    '    strsql &= "WHERE PM_PRODUCT_INDEX = (SELECT ISNULL(MAX(PM_PRODUCT_INDEX),0) FROM PRODUCT_MSTR_TEMP) "
        '    '    strsql &= "AND PM_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    '    Common.Insert2Ary(strAryQuery, strsql)

        '    '    'strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET "
        '    '    'strsql &= "PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    '    'strsql &= "WHERE PA_ATTACH_INDEX = '" & strImageIndex & "' "
        '    '    'Common.Insert2Ary(strAryQuery, strsql)
        '    'End If

        '    '' ai chu add on 19/09/2005
        '    '' delete all status = 'D' (deleted before user save the record)
        '    '' update status = 'T' to status = 'U' 
        '    'strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE "
        '    'strsql &= "PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
        '    'strsql &= "AND PA_STATUS = 'D' "
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    'strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'U' "
        '    'strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
        '    'strsql &= "AND PA_STATUS = 'T' "
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    '' upload attachment
        '    '' need to update PA_PRODUCT_CODE because when it's uploaded, the product code is set to sessionid
        '    'strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET "
        '    'strsql &= "PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    'strsql &= "WHERE PA_PRODUCT_CODE = '" & HttpContext.Current.Session.SessionID & "' "
        '    ''strsql &= "AND PA_TYPE = 'D'"
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    'If objDb.BatchExecute(strAryQuery) Then
        '    '    Dim objMail As New Email
        '    '    objMail.sendNotification(EmailType.ListPriceApproval, HttpContext.Current.Session("UserId"), "", Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")), strProductCode, "")
        '    '    objMail = Nothing
        '    '    insertProductTemp = strProductCode
        '    'End If
        'End Function

        '' for hub admin approval
        'Public Function getListPriceApproval(ByVal strVendor As String, ByVal strStatus As String, ByVal strItemId As String, ByVal strDesc As String, ByVal strVendorItemCode As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, "
        '    strsql &= "PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, PM_STATUS, STATUS_DESC, PM_REMARK, PM_ACTION, PM_ENT_DT "
        '    strsql &= "FROM PRODUCT_MSTR_TEMP "
        '    strsql &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = PM_STATUS AND STATUS_TYPE = 'LP' "
        '    strsql &= "WHERE PM_S_COY_ID = '" & Common.Parse(strVendor) & "' "

        '    If strItemId <> "" Then
        '        strsql &= " AND PM_PRODUCT_CODE " & Common.ParseSQL(strItemId) & " "
        '    End If

        '    If strDesc <> "" Then
        '        strsql &= " AND PM_PRODUCT_DESC " & Common.ParseSQL(strDesc) & " "
        '    End If

        '    If strVendorItemCode <> "" Then
        '        strsql &= " AND PM_VENDOR_ITEM_CODE " & Common.ParseSQL(strVendorItemCode) & " "
        '    End If
        '    If strStatus <> "" Then
        '        strsql &= "AND PM_STATUS IN (" & strStatus & ") "
        '        If strStatus.IndexOf("3") <> -1 Then
        '            strsql &= " UNION SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE," &
        '                        "PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, '3' AS PM_STATUS," &
        '                        "STATUS_DESC, '' AS PM_REMARK, '' AS PM_ACTION,PM_ENT_DT FROM PRODUCT_MSTR JOIN STATUS_MSTR" &
        '                        " WHERE PM_DELETED <> 'Y' AND STATUS_NO = '3' AND STATUS_TYPE = 'LP' AND PM_S_COY_ID='" & Common.Parse(strVendor) & "'"
        '        End If
        '    End If


        '    ds = objDb.FillDs(strsql)
        '    getListPriceApproval = ds
        'End Function

        '' for vendor
        'Public Function getListPrice(ByVal strVendor As String, ByVal strStatus As String, ByVal strVendorItemCode As String, ByVal strDesc As String, ByVal strCommodityType As String) As DataSet
        '    Dim strsql, strsql1, strsql2 As String
        '    Dim ds As New DataSet
        '    ' strsql1 - select from PRODUCT_MSTR_TEMP
        '    '            strsql1 = "SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, PM_STATUS, STATUS_DESC, PM_REMARK, PM_ACTION, PM_ENT_DT "
        '    'Michelle (25/10/2010) - To get the commodity type desc
        '    'strsql1 = "SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, PM_STATUS, STATUS_DESC, PM_REMARK, PM_ACTION, PM_CATEGORY_NAME "
        '    'strsql1 &= "FROM PRODUCT_MSTR_TEMP "
        '    'strsql1 &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = PM_STATUS AND STATUS_TYPE = 'LP' "
        '    'strsql1 &= "WHERE PM_S_COY_ID = '" & Common.Parse(strVendor) & "' "

        '    strsql1 = "SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, PM_STATUS, STATUS_DESC, PM_REMARK, PM_ACTION, CT_NAME AS PM_CATEGORY_NAME "
        '    strsql1 &= "FROM PRODUCT_MSTR_TEMP "
        '    strsql1 &= "LEFT JOIN STATUS_MSTR ON STATUS_NO = PM_STATUS AND STATUS_TYPE = 'LP' "
        '    strsql1 &= "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME "
        '    strsql1 &= "WHERE PM_S_COY_ID = '" & Common.Parse(strVendor) & "' "

        '    ' strsql2 - select from PRODUCT_MSTR
        '    strsql2 = "SELECT PM_PRODUCT_INDEX, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_CURRENCY_CODE, '3' AS PM_STATUS, STATUS_DESC, '' AS PM_REMARK, '' AS PM_ACTION, CT_NAME AS PM_CATEGORY_NAME "
        '    strsql2 &= "FROM PRODUCT_MSTR JOIN STATUS_MSTR "
        '    strsql2 &= "LEFT JOIN COMMODITY_TYPE ON CT_ID = PM_CATEGORY_NAME  "
        '    strsql2 &= "WHERE PM_DELETED <> 'Y' AND STATUS_NO = '3' AND STATUS_TYPE = 'LP' AND PM_S_COY_ID = '" & Common.Parse(strVendor) & "' "

        '    If strStatus = "" Or strStatus = "1,2,3" Then  ' select all status from both table
        '        strsql = strsql1 & getListPriceSearch(strCommodityType, strDesc, strVendorItemCode) & " UNION " & strsql2 & getListPriceSearch(strCommodityType, strDesc, strVendorItemCode)
        '    ElseIf strStatus = "3" Then ' select from PRODUCT_MSTR only
        '        strsql = strsql2 & getListPriceSearch(strCommodityType, strDesc, strVendorItemCode)
        '    ElseIf InStr(strStatus, "3") <= 0 Then ' select from temp table only
        '        strsql = strsql1 & getListPriceSearch(strCommodityType, strDesc, strVendorItemCode)
        '        strsql &= "AND PM_STATUS IN (" & strStatus & ") "
        '    Else
        '        strsql = strsql1 & getListPriceSearch(strCommodityType, strDesc, strVendorItemCode)
        '        strsql &= "AND PM_STATUS IN (" & strStatus & ") "
        '        strsql &= " UNION " & strsql2 & getListPriceSearch(strCommodityType, strDesc, strVendorItemCode)
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    getListPrice = ds
        'End Function

        ''Public Function getListPriceSearch(ByVal strItemId As String, ByVal strDesc As String, ByVal strVendorItemCode As String) As String
        ''    Dim strsql As String = ""
        ''    If strItemId <> "" Then
        ''        strsql &= " AND PM_PRODUCT_CODE " & Common.ParseSQL(strItemId) & " "
        ''    End If

        ''    If strDesc <> "" Then
        ''        strsql &= " AND PM_PRODUCT_DESC " & Common.ParseSQL(strDesc) & " "
        ''    End If

        ''    If strVendorItemCode <> "" Then
        ''        strsql &= " AND PM_VENDOR_ITEM_CODE " & Common.ParseSQL(strVendorItemCode) & " "
        ''    End If
        ''    getListPriceSearch = strsql
        ''End Function

        'Public Function getListPriceSearch(ByVal strCommodityType As String, ByVal strDesc As String, ByVal strVendorItemCode As String) As String
        '    Dim strsql As String = ""
        '    Dim strTemp As String

        '    If strCommodityType <> "" Then
        '        strsql &= " AND PM_CATEGORY_NAME " & Common.ParseSQL(strCommodityType) & " "
        '    End If

        '    If strDesc <> "" Then
        '        strTemp = Common.BuildWildCard(strDesc)
        '        strsql &= " AND PM_PRODUCT_DESC " & Common.ParseSQL(strTemp) & " "
        '    End If

        '    If strVendorItemCode <> "" Then
        '        strTemp = Common.BuildWildCard(strVendorItemCode)
        '        strsql &= " AND PM_VENDOR_ITEM_CODE " & Common.ParseSQL(strTemp) & " "
        '    End If
        '    getListPriceSearch = strsql
        'End Function
        '' when user modifying some of fields
        'Public Function updateListPrice(ByVal strVendor As String, ByVal dsProduct As DataSet, ByVal blnSend As Boolean, ByVal strPrice As String, ByVal strImageIndex As String, ByVal blnList As Boolean, ByVal blnDiscount As Boolean)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim strGroupIndex As String = ""
        '    Dim i As Integer
        '    Dim objMail As New Email
        '    Dim dt1 As New DataTable
        '    Dim dt2 As New DataTable

        '    dt1.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt1.Columns.Add("ItemDesc", Type.GetType("System.String"))

        '    dt2.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt2.Columns.Add("ItemDesc", Type.GetType("System.String"))
        '    dt2.Columns.Add("List", Type.GetType("System.String"))
        '    dt2.Columns.Add("Discount", Type.GetType("System.String"))

        '    Dim dtr As DataRow
        '    Dim strImage As String

        '    If strPrice <> "" Then ' update price
        '        strsql = "UPDATE PRODUCT_MSTR SET PM_UNIT_COST = " & strPrice & " "
        '        strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "'; "
        '        'Michelle (eBiz/134/08) - Update Shopping Cart also
        '        strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = " & strPrice & " "
        '        strsql &= "WHERE SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        strsql &= " AND SC_SOURCE = 'LP' AND SC_S_COY_ID ='" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If


        '    ' upload  image
        '    ' ai chu add on 19/09/2005
        '    ' update Product_Attachment table if user click save button
        '    If strImageIndex <> "" Then
        '        Dim objDb As New EAD.DBCom, tSQL As String

        '        If objDb.Exist("SELECT * FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_SOURCE = 'V' AND PA_TYPE = 'I' AND PA_STATUS = 'D' ORDER BY PA_ATTACH_INDEX DESC ") > 0 Then
        '            ' delete from existing records maintain image file
        '            strsql = "DELETE FROM PRODUCT_ATTACHMENT "
        '            strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '            strsql &= "AND PA_TYPE = 'I' "
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            'strsql &= "SELECT TOP 1 PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE  "
        '            'strsql &= "FROM PRODUCT_ATTACHMENT_TEMP "
        '            'strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_SOURCE = 'V' AND PA_TYPE = 'I' AND PA_STATUS <> 'D' ORDER BY PA_ATTACH_INDEX DESC "
        '        End If

        '        Dim tDS As DataSet = objDb.Fill1Ds("PRODUCT_ATTACHMENT_TEMP", "PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE", "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_SOURCE = 'V' AND PA_TYPE = 'I' AND PA_STATUS <> 'D' ORDER BY PA_ATTACH_INDEX DESC ")
        '        If tDS.Tables(0).Rows.Count() > 0 Then
        '            strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) VALUES ('" & tDS.Tables(0).Rows(0).Item(0) & "', '" & tDS.Tables(0).Rows(0).Item(1) & "', '" & tDS.Tables(0).Rows(0).Item(2) & "', " & tDS.Tables(0).Rows(0).Item(3) & ", '" & tDS.Tables(0).Rows(0).Item(4) & "')"
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If


        '        'strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = PA_ATTACH_INDEX "
        '        'strsql &= "FROM PRODUCT_MSTR, PRODUCT_ATTACHMENT "
        '        'strsql &= "WHERE PM_PRODUCT_CODE = PA_PRODUCT_CODE AND PA_TYPE = 'I' "
        '        'strsql &= "AND PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        'eddited by sam 22/09/2010
        '        'Michelle (8/5/2011)
        '        'strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & objDb.GetVal("SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_TYPE = 'I' AND PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' ")
        '        'strsql &= " WHERE PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        strsql = "UPDATE PRODUCT_MSTR, PRODUCT_ATTACHMENT SET PM_PRODUCT_IMAGE = PA_ATTACH_INDEX "
        '        strsql &= " WHERE PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_PRODUCT_CODE = PM_PRODUCT_CODE AND PA_TYPE = 'I'"


        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If

        '    ' maintain document file
        '    'Michelle (6/5/2011) - Hub approval
        '    'strsql = "DELETE FROM PRODUCT_ATTACHMENT "
        '    'strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    'strsql &= "AND PA_TYPE = 'D' "
        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    strsql &= "AND PA_TYPE = 'D' AND PA_PRODUCT_CODE IN (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))
        '    strsql &= "' AND PA_STATUS = 'D' AND PA_TYPE = 'D' AND PA_SOURCE = 'V') "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
        '    strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE "
        '    strsql &= "FROM PRODUCT_ATTACHMENT_TEMP "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    strsql &= "AND PA_STATUS <> 'D' AND PA_TYPE = 'D' AND PA_SOURCE = 'V' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' ai chu add on 21/09/2005
        '    ' delete image file from PRODUCT_ATTACHMENT_TEMP
        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_TYPE = 'I' AND PA_SOURCE = 'V' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If blnSend Then
        '        'strsql = "INSERT INTO PRODUCT_MSTR_TEMP (PM_S_COY_ID, PM_PRODUCT_CODE, PM_CATEGORY_NAME, "
        '        'strsql &= "PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_GST_CODE, PM_TAX_PERC, PM_TAX_ID, "
        '        'strsql &= "PM_CURRENCY_CODE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL, PM_PRODUCT_TYPE, "
        '        'strsql &= "PM_ENT_BY, PM_ENT_DT, PM_STATUS, PM_ACTION) VALUES ("
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "', " ' PM_PRODUCT_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '        'If Common.Parse(strPrice) = "" Then
        '        '    strsql &= "NULL, " ' PM_UNIT_COST
        '        'Else
        '        '    strsql &= strPrice & ", " ' PM_UNIT_COST
        '        'End If

        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("gstcode")) & "', " ' PM_GST_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Tax")) & "', " ' PM_GST_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Perc")) & "', " ' PM_GST_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("TaxId")) & "', " ' PM_TAX_ID

        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmtcode")) & "', " ' PM_MGMT_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL
        '        'strsql &= "NULL, " ' PM_PRODUCT_TYPE
        '        'strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), " ' PM_ENT_BY, PM_ENT_DT
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("status")) & "', " ' PM_STATUS
        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmttext")) & "', " ' PM_MGMT_TEXT
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("action")) & "') " ' PM_MGMT_TEXT
        '        'Common.Insert2Ary(strAryQuery, strsql)
        '        Dim stProductCode As String = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))

        '        strsql = "INSERT INTO PRODUCT_MSTR_TEMP (PM_S_COY_ID, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, "
        '        strsql &= "PM_PRODUCT_DESC,PM_REF_NO,PM_LONG_DESC,PM_CATEGORY_NAME,PM_GST_CODE,PM_TAX_PERC, PM_TAX_ID, PM_UOM,"
        '        strsql &= "PM_UNIT_COST,PM_CURRENCY_CODE,PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO, "
        '        strsql &= "PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT,PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,"
        '        strsql &= "PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_REMARKS, PM_PRODUCT_FOR, PM_STATUS,PM_ACTION) VALUES ("
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '        strsql &= "'" & stProductCode & "', " ' PM_PRODUCT_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Tax")) & "', " ' PM_GST_CODE
        '        If IsDBNull(dsProduct.Tables(0).Rows(0)("Perc")) Or Common.parseNull(dsProduct.Tables(0).Rows(0)("Perc")) = "" Then
        '            strsql &= "'0', " ' PM_GST_CODE
        '        Else
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Perc")) & "', " ' PM_GST_CODE
        '        End If
        '        If IsDBNull(dsProduct.Tables(0).Rows(0)("TaxId")) Or Common.parseNull(dsProduct.Tables(0).Rows(0)("TaxId")) = "" Then
        '            strsql &= "'1', " ' PM_TAX_ID
        '        Else
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("TaxId")) & "', " ' PM_TAX_ID
        '        End If
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '        If Common.Parse(dsProduct.Tables(0).Rows(0)("Price")) = "" Then
        '            strsql &= "NULL, " ' PM_UNIT_COST
        '        Else
        '            strsql &= dsProduct.Tables(0).Rows(0)("Price") & ", " ' PM_UNIT_COST
        '        End If
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CurrencyCode")) & "', " ' PM_CURRENCY_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL

        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Packing")) & "', " ' PM_PACKING_REQ
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARKS
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("PM_PRODUCT_FOR")) & "', " ' PM_PRODUCT_FOR
        '        'strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), " ' PM_ENT_BY, PM_ENT_DT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("status")) & "', " ' PM_STATUS
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("action")) & "') " ' PM_MGMT_TEXT
        '        Common.Insert2Ary(strAryQuery, strsql)



        '        ' upload  image
        '        'If strImageIndex <> "" Then
        '        '    strsql = "UPDATE PRODUCT_MSTR_TEMP SET PM_PRODUCT_IMAGE = '" & strImageIndex & "' "
        '        '    strsql &= "WHERE PM_PRODUCT_INDEX = (SELECT ISNULL(MAX(PM_PRODUCT_INDEX),0) FROM PRODUCT_MSTR_TEMP) "
        '        '    strsql &= "AND PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        '    Common.Insert2Ary(strAryQuery, strsql)
        '        'End If

        '        For i = 0 To dsProduct.Tables(1).Rows.Count - 1
        '            strsql = "INSERT INTO PRODUCT_MOD_TEMP (PMT_PRODUCT_INDEX, PMT_PRODUCT_CODE, PMT_FIELD_NAME, PMT_VALUE) SELECT "
        '            'strsql &= "'" & Common.Parse(dsProduct.Tables(2).Rows(i)("ProductIndex")) & "', "
        '            strsql &= "(SELECT MAX(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR_TEMP), "
        '            strsql &= "'" & stProductCode & "', "
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("FieldName")) & "', "
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("Value")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        Next
        '    End If

        '    '' insert/delete item into/from discount group table
        '    'If Not HttpContext.Current.Session("Env") = "FTN" Then
        '    '    For i = 0 To dsProduct.Tables(1).Rows.Count - 1
        '    '        If Not IsDBNull(dsProduct.Tables(1).Rows(i)("Change")) Then
        '    '            If dsProduct.Tables(1).Rows(i)("Change") = 1 Then
        '    '                blnDiscount = True
        '    '                strGroupIndex &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ","
        '    '            End If
        '    '        End If

        '    '        Select Case dsProduct.Tables(1).Rows(i)("chk")
        '    '            Case "1"
        '    '                If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " AND CDI_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' ") <= 0 Then
        '    '                    strsql = "INSERT INTO CONTRACT_DIST_ITEMS (CDI_GROUP_INDEX, CDI_PRODUCT_CODE, "
        '    '                    strsql &= "CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, CDI_UNIT_COST, CDI_REMARK, CDI_GST, "
        '    '                    strsql &= "CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) SELECT "
        '    '                    strsql &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ", "
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "', " ' PM_PRODUCT_CODE
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '    '                    strsql &= dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "', "
        '    '                    strsql &= "(SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_CATEGORY = 'GST' AND CODE_ABBR = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "'), "
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '    '                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE() "
        '    '                Else
        '    '                    ' item already exist in the catalogue
        '    '                    strsql = "UPDATE CONTRACT_DIST_ITEMS "
        '    '                    strsql &= "SET CDI_UNIT_COST = " & dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '    '                    strsql &= "CDI_REMARK = '" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "' "
        '    '                    strsql &= "WHERE CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                    strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "'; "
        '    '                    'Michelle (eBiz/134/08) - Update Shopping Cart also
        '    '                    strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = " & dsProduct.Tables(1).Rows(i)("DiscPrice") & " "
        '    '                    strsql &= "WHERE SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    '                    strsql &= "AND SC_CD_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                    strsql &= " AND SC_S_COY_ID ='" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '    '                End If
        '    '                Common.Insert2Ary(strAryQuery, strsql)

        '    '            Case "0"
        '    '                strsql = "DELETE FROM CONTRACT_DIST_ITEMS WHERE "
        '    '                strsql &= "CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "'; "
        '    '                'Michelle (eBiz/134/08) - Update Shopping Cart also
        '    '                strsql &= "DELETE FROM SHOPPING_CART WHERE "
        '    '                strsql &= "SC_CD_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                strsql &= "AND SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    '                Common.Insert2Ary(strAryQuery, strsql)
        '    '        End Select
        '    '    Next


        '    'End If

        '    If objDb.BatchExecute(strAryQuery) Then
        '        'If blnList Or blnDiscount Then
        '        '    dtr = dt2.NewRow()
        '        '    dtr("ItemId") = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))
        '        '    dtr("ItemDesc") = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc"))
        '        '    dtr("List") = IIf(blnList, "Yes", "No")
        '        '    dtr("Discount") = IIf(blnDiscount, "Yes", "No")
        '        '    dt2.Rows.Add(dtr)
        '        '    If strGroupIndex <> "" Then
        '        '        strGroupIndex = strGroupIndex.Substring(0, strGroupIndex.Length - 1)
        '        '    End If
        '        '    ' ai chu remark on 05/05/2006
        '        '    ' requested by CK - Take out email notification to PM when vendor admin modify catalogue item
        '        '    ' objMail.sendItemUpdateMail(Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")), strGroupIndex, dt1, dt2, blnList, blnDiscount)
        '        'End If
        '        If blnSend Then
        '            objMail.sendNotification(EmailType.ListPriceApproval, HttpContext.Current.Session("UserId"), "", strVendor, Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")), "")
        '        End If
        '    End If

        '    objMail = Nothing
        'End Function
        ''Added by Craven 13-5-2011 for delete Attachment issue on modify
        'Public Function updateListPriceAttachment(ByVal strVendor As String, ByVal dsProduct As DataSet, ByVal blnSend As Boolean, ByVal strPrice As String, ByVal strImageIndex As String, ByVal blnList As Boolean, ByVal blnDiscount As Boolean)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim strGroupIndex As String = ""
        '    Dim i As Integer
        '    Dim objMail As New Email
        '    Dim dt1 As New DataTable
        '    Dim dt2 As New DataTable

        '    dt1.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt1.Columns.Add("ItemDesc", Type.GetType("System.String"))

        '    dt2.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt2.Columns.Add("ItemDesc", Type.GetType("System.String"))
        '    dt2.Columns.Add("List", Type.GetType("System.String"))
        '    dt2.Columns.Add("Discount", Type.GetType("System.String"))

        '    Dim dtr As DataRow
        '    Dim strImage As String

        '    If strPrice <> "" Then ' update price
        '        strsql = "UPDATE PRODUCT_MSTR SET PM_UNIT_COST = " & strPrice & " "
        '        strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "'; "
        '        'Michelle (eBiz/134/08) - Update Shopping Cart also
        '        strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = " & strPrice & " "
        '        strsql &= "WHERE SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        strsql &= " AND SC_SOURCE = 'LP' AND SC_S_COY_ID ='" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If


        '    ' upload  image
        '    ' ai chu add on 19/09/2005
        '    ' update Product_Attachment table if user click save button
        '    If strImageIndex <> "" Then
        '        Dim objDb As New EAD.DBCom, tSQL As String

        '        If objDb.Exist("SELECT * FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_SOURCE = 'V' AND PA_TYPE = 'I' AND PA_STATUS = 'D' ORDER BY PA_ATTACH_INDEX DESC ") > 0 Then
        '            ' delete from existing records maintain image file
        '            strsql = "DELETE FROM PRODUCT_ATTACHMENT "
        '            strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '            strsql &= "AND PA_TYPE = 'I' "
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            'strsql &= "SELECT TOP 1 PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE  "
        '            'strsql &= "FROM PRODUCT_ATTACHMENT_TEMP "
        '            'strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_SOURCE = 'V' AND PA_TYPE = 'I' AND PA_STATUS <> 'D' ORDER BY PA_ATTACH_INDEX DESC "
        '        End If

        '        Dim tDS As DataSet = objDb.Fill1Ds("PRODUCT_ATTACHMENT_TEMP", "PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE", "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_SOURCE = 'V' AND PA_TYPE = 'I' AND PA_STATUS <> 'D' ORDER BY PA_ATTACH_INDEX DESC ")
        '        If tDS.Tables(0).Rows.Count() > 0 Then
        '            strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) VALUES ('" & tDS.Tables(0).Rows(0).Item(0) & "', '" & tDS.Tables(0).Rows(0).Item(1) & "', '" & tDS.Tables(0).Rows(0).Item(2) & "', " & tDS.Tables(0).Rows(0).Item(3) & ", '" & tDS.Tables(0).Rows(0).Item(4) & "')"
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If


        '        'strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = PA_ATTACH_INDEX "
        '        'strsql &= "FROM PRODUCT_MSTR, PRODUCT_ATTACHMENT "
        '        'strsql &= "WHERE PM_PRODUCT_CODE = PA_PRODUCT_CODE AND PA_TYPE = 'I' "
        '        'strsql &= "AND PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        'eddited by sam 22/09/2010
        '        'Michelle (8/5/2011)
        '        'strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = '" & objDb.GetVal("SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_TYPE = 'I' AND PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' ")
        '        'strsql &= " WHERE PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        strsql = "UPDATE PRODUCT_MSTR, PRODUCT_ATTACHMENT SET PM_PRODUCT_IMAGE = PA_ATTACH_INDEX "
        '        strsql &= " WHERE PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_PRODUCT_CODE = PM_PRODUCT_CODE AND PA_TYPE = 'I'"


        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If

        '    ' maintain document file
        '    'Michelle (6/5/2011) - Hub approval
        '    'strsql = "DELETE FROM PRODUCT_ATTACHMENT "
        '    'strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    'strsql &= "AND PA_TYPE = 'D' "
        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    strsql &= "AND PA_TYPE = 'D' AND PA_PRODUCT_CODE IN (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))
        '    strsql &= "' AND PA_STATUS = 'D' AND PA_TYPE = 'D' AND PA_SOURCE = 'V') "
        '    strsql &= "AND PA_HUB_FILENAME IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_STATUS = 'D' AND PA_TYPE = 'D' AND PA_SOURCE = 'V')"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "DELETE FROM product_attachment WHERE pa_product_code='" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND pa_type='D'"
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
        '    strsql &= "SELECT PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE "
        '    strsql &= "FROM PRODUCT_ATTACHMENT_TEMP "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    strsql &= "AND PA_STATUS <> 'D' AND PA_TYPE = 'D' AND PA_SOURCE = 'V' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' ai chu add on 21/09/2005
        '    ' delete image file from PRODUCT_ATTACHMENT_TEMP
        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' AND PA_SOURCE = 'V' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If blnSend Then
        '        'strsql = "INSERT INTO PRODUCT_MSTR_TEMP (PM_S_COY_ID, PM_PRODUCT_CODE, PM_CATEGORY_NAME, "
        '        'strsql &= "PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_GST_CODE, PM_TAX_PERC, PM_TAX_ID, "
        '        'strsql &= "PM_CURRENCY_CODE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL, PM_PRODUCT_TYPE, "
        '        'strsql &= "PM_ENT_BY, PM_ENT_DT, PM_STATUS, PM_ACTION) VALUES ("
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "', " ' PM_PRODUCT_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '        'If Common.Parse(strPrice) = "" Then
        '        '    strsql &= "NULL, " ' PM_UNIT_COST
        '        'Else
        '        '    strsql &= strPrice & ", " ' PM_UNIT_COST
        '        'End If

        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("gstcode")) & "', " ' PM_GST_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Tax")) & "', " ' PM_GST_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Perc")) & "', " ' PM_GST_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("TaxId")) & "', " ' PM_TAX_ID

        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmtcode")) & "', " ' PM_MGMT_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL
        '        'strsql &= "NULL, " ' PM_PRODUCT_TYPE
        '        'strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), " ' PM_ENT_BY, PM_ENT_DT
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("status")) & "', " ' PM_STATUS
        '        ''strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("mgmttext")) & "', " ' PM_MGMT_TEXT
        '        'strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("action")) & "') " ' PM_MGMT_TEXT
        '        'Common.Insert2Ary(strAryQuery, strsql)
        '        Dim stProductCode As String = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))

        '        strsql = "INSERT INTO PRODUCT_MSTR_TEMP (PM_S_COY_ID, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, "
        '        strsql &= "PM_PRODUCT_DESC,PM_REF_NO,PM_LONG_DESC,PM_CATEGORY_NAME,PM_GST_CODE,PM_TAX_PERC, PM_TAX_ID, PM_UOM,"
        '        strsql &= "PM_UNIT_COST,PM_CURRENCY_CODE,PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO, "
        '        strsql &= "PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT,PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,"
        '        strsql &= "PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_REMARKS, PM_PRODUCT_FOR, PM_STATUS,PM_ACTION) VALUES ("
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '        strsql &= "'" & stProductCode & "', " ' PM_PRODUCT_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Tax")) & "', " ' PM_GST_CODE
        '        If IsDBNull(dsProduct.Tables(0).Rows(0)("Perc")) Or Common.parseNull(dsProduct.Tables(0).Rows(0)("Perc")) = "" Then
        '            strsql &= "'0', " ' PM_GST_CODE
        '        Else
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Perc")) & "', " ' PM_GST_CODE
        '        End If
        '        If IsDBNull(dsProduct.Tables(0).Rows(0)("TaxId")) Or Common.parseNull(dsProduct.Tables(0).Rows(0)("TaxId")) = "" Then
        '            strsql &= "'1', " ' PM_TAX_ID
        '        Else
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("TaxId")) & "', " ' PM_TAX_ID
        '        End If
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '        If Common.Parse(dsProduct.Tables(0).Rows(0)("Price")) = "" Then
        '            strsql &= "NULL, " ' PM_UNIT_COST
        '        Else
        '            strsql &= dsProduct.Tables(0).Rows(0)("Price") & ", " ' PM_UNIT_COST
        '        End If
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("CurrencyCode")) & "', " ' PM_CURRENCY_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL

        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Packing")) & "', " ' PM_PACKING_REQ
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARKS
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("PM_PRODUCT_FOR")) & "', " ' PM_PRODUCT_FOR
        '        'strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), " ' PM_ENT_BY, PM_ENT_DT
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("status")) & "', " ' PM_STATUS
        '        strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("action")) & "') " ' PM_MGMT_TEXT
        '        Common.Insert2Ary(strAryQuery, strsql)



        '        ' upload  image
        '        'If strImageIndex <> "" Then
        '        '    strsql = "UPDATE PRODUCT_MSTR_TEMP SET PM_PRODUCT_IMAGE = '" & strImageIndex & "' "
        '        '    strsql &= "WHERE PM_PRODUCT_INDEX = (SELECT ISNULL(MAX(PM_PRODUCT_INDEX),0) FROM PRODUCT_MSTR_TEMP) "
        '        '    strsql &= "AND PM_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '        '    Common.Insert2Ary(strAryQuery, strsql)
        '        'End If

        '        For i = 0 To dsProduct.Tables(1).Rows.Count - 1
        '            strsql = "INSERT INTO PRODUCT_MOD_TEMP (PMT_PRODUCT_INDEX, PMT_PRODUCT_CODE, PMT_FIELD_NAME, PMT_VALUE) SELECT "
        '            'strsql &= "'" & Common.Parse(dsProduct.Tables(2).Rows(i)("ProductIndex")) & "', "
        '            strsql &= "(SELECT MAX(PM_PRODUCT_INDEX) FROM PRODUCT_MSTR_TEMP), "
        '            strsql &= "'" & stProductCode & "', "
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("FieldName")) & "', "
        '            strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("Value")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        Next
        '    End If

        '    ' insert/delete item into/from discount group table
        '    ' No more Discount
        '    'If Not HttpContext.Current.Session("Env") = "FTN" Then
        '    '    For i = 0 To dsProduct.Tables(1).Rows.Count - 1

        '    '        If Not IsDBNull(dsProduct.Tables(1).Rows(i)("Change")) Then
        '    '            If dsProduct.Tables(1).Rows(i)("Change") = 1 Then
        '    '                blnDiscount = True
        '    '                strGroupIndex &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ","
        '    '            End If
        '    '        End If

        '    '        Select Case dsProduct.Tables(1).Rows(i)("chk")
        '    '            Case "1"
        '    '                If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " AND CDI_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' ") <= 0 Then
        '    '                    strsql = "INSERT INTO CONTRACT_DIST_ITEMS (CDI_GROUP_INDEX, CDI_PRODUCT_CODE, "
        '    '                    strsql &= "CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, CDI_UNIT_COST, CDI_REMARK, CDI_GST, "
        '    '                    strsql &= "CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) SELECT "
        '    '                    strsql &= dsProduct.Tables(1).Rows(i)("DiscIndex") & ", "
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "', " ' PM_PRODUCT_CODE
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '    '                    strsql &= dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "', "
        '    '                    strsql &= "(SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_CATEGORY = 'GST' AND CODE_ABBR = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "'), "
        '    '                    strsql &= "'" & Common.Parse(dsProduct.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '    '                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE() "
        '    '                Else
        '    '                    ' item already exist in the catalogue
        '    '                    strsql = "UPDATE CONTRACT_DIST_ITEMS "
        '    '                    strsql &= "SET CDI_UNIT_COST = " & dsProduct.Tables(1).Rows(i)("DiscPrice") & ", "
        '    '                    strsql &= "CDI_REMARK = '" & Common.Parse(dsProduct.Tables(1).Rows(i)("DiscRemark")) & "' "
        '    '                    strsql &= "WHERE CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                    strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "'; "
        '    '                    'Michelle (eBiz/134/08) - Update Shopping Cart also
        '    '                    strsql &= "UPDATE SHOPPING_CART SET SC_UNIT_COST = " & dsProduct.Tables(1).Rows(i)("DiscPrice") & " "
        '    '                    strsql &= "WHERE SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    '                    strsql &= "AND SC_CD_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                    strsql &= " AND SC_S_COY_ID ='" & Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")) & "' "
        '    '                End If
        '    '                Common.Insert2Ary(strAryQuery, strsql)

        '    '            Case "0"
        '    '                strsql = "DELETE FROM CONTRACT_DIST_ITEMS WHERE "
        '    '                strsql &= "CDI_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "'; "
        '    '                'Michelle (eBiz/134/08) - Update Shopping Cart also
        '    '                strsql &= "DELETE FROM SHOPPING_CART WHERE "
        '    '                strsql &= "SC_CD_GROUP_INDEX = " & dsProduct.Tables(1).Rows(i)("DiscIndex") & " "
        '    '                strsql &= "AND SC_PRODUCT_CODE = '" & Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")) & "' "
        '    '                Common.Insert2Ary(strAryQuery, strsql)
        '    '        End Select
        '    '    Next


        '    'End If

        '    If objDb.BatchExecute(strAryQuery) Then
        '        'If blnList Or blnDiscount Then
        '        '    dtr = dt2.NewRow()
        '        '    dtr("ItemId") = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode"))
        '        '    dtr("ItemDesc") = Common.Parse(dsProduct.Tables(0).Rows(0)("ProductDesc"))
        '        '    dtr("List") = IIf(blnList, "Yes", "No")
        '        '    dtr("Discount") = IIf(blnDiscount, "Yes", "No")
        '        '    dt2.Rows.Add(dtr)
        '        '    If strGroupIndex <> "" Then
        '        '        strGroupIndex = strGroupIndex.Substring(0, strGroupIndex.Length - 1)
        '        '    End If
        '        '    ' ai chu remark on 05/05/2006
        '        '    ' requested by CK - Take out email notification to PM when vendor admin modify catalogue item
        '        '    ' objMail.sendItemUpdateMail(Common.Parse(dsProduct.Tables(0).Rows(0)("CoyId")), strGroupIndex, dt1, dt2, blnList, blnDiscount)
        '        'End If
        '        If blnSend Then
        '            objMail.sendNotification(EmailType.ListPriceApproval, HttpContext.Current.Session("UserId"), "", strVendor, Common.Parse(dsProduct.Tables(0).Rows(0)("ProductCode")), "")
        '        End If
        '    End If

        '    objMail = Nothing
        'End Function

        '' approved/modified by hubadmin
        'Public Function updateProductMstr(ByVal blnApproved As Boolean, ByVal strAction As String, ByVal strVendor As String, ByVal dsItem As DataSet, ByVal strUpdate() As String, ByVal blnSend As Boolean, ByVal strImageIndex As String, ByVal strOldVIC As String) As Boolean
        '    Dim strsql As String
        '    Dim strGroupIndex As String = ""
        '    Dim strAryQuery(0) As String
        '    Dim dt1 As New DataTable
        '    Dim dt2 As New DataTable
        '    Dim blnList As Boolean = False
        '    Dim blnDiscount As Boolean = False

        '    dt1.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt1.Columns.Add("ItemDesc", Type.GetType("System.String"))

        '    dt2.Columns.Add("ItemId", Type.GetType("System.String"))
        '    dt2.Columns.Add("ItemDesc", Type.GetType("System.String"))
        '    dt2.Columns.Add("List", Type.GetType("System.String"))
        '    dt2.Columns.Add("Discount", Type.GetType("System.String"))

        '    Dim dtr As DataRow

        '    Select Case blnApproved
        '        Case False
        '            strsql = "UPDATE PRODUCT_MSTR_TEMP SET "
        '            strsql &= "PM_STATUS = '" & ListPriceStatus.Rejected & "', "
        '            strsql &= "PM_REMARK = '" & Common.Parse(strUpdate(25)) & "' "
        '            strsql &= "WHERE PM_PRODUCT_INDEX = " & strUpdate(0)
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            ' delete uploaded image
        '            If strImageIndex <> "" Then
        '                strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_ATTACH_INDEX IN "
        '                strsql &= "(SELECT PM_PRODUCT_IMAGE FROM PRODUCT_MSTR_TEMP WHERE PM_PRODUCT_INDEX = '" & strUpdate(0) & "')"
        '                Common.Insert2Ary(strAryQuery, strsql)
        '            End If

        '            ' ai chu add on 20/09/2005
        '            ' delete all temporary attachment file
        '            strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = "
        '            strsql &= "(SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR_TEMP WHERE PM_PRODUCT_INDEX = '" & strUpdate(0) & "')"
        '            Common.Insert2Ary(strAryQuery, strsql)

        '        Case True
        '            'If strOldVIC <> dsItem.Tables(0).Rows(0)("VendorItemCode") Then
        '            '    strsql = "SELECT '*' FROM PRODUCT_MSTR WHERE "
        '            '    strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsItem.Tables(0).Rows(0)("VendorItemCode")) & "' "
        '            '    strsql &= "AND PM_S_COY_ID = '" & Common.Parse(dsItem.Tables(0).Rows(0)("CoyId")) & "' "
        '            '    If objDb.Exist(strsql) > 0 Then
        '            '        updateProductMstr = False
        '            '        Exit Function
        '            '    End If
        '            'End If
        '            strsql = "SELECT '*' FROM PRODUCT_MSTR WHERE "
        '            strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(dsItem.Tables(0).Rows(0)("VendorItemCode")) & "' "
        '            strsql &= "AND PM_S_COY_ID = '" & Common.Parse(dsItem.Tables(0).Rows(0)("CoyId")) & "' "
        '            strsql &= "AND PM_PRODUCT_CODE <> '" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "' "
        '            strsql &= "AND PM_DELETED <> 'Y' "
        '            If objDb.Exist(strsql) > 0 Then
        '                updateProductMstr = False
        '                Exit Function
        '            End If

        '            Select Case strAction
        '                Case "N" ' new 
        '                    blnList = True
        '                    'strsql = "INSERT INTO PRODUCT_MSTR (PM_S_COY_ID, PM_PRODUCT_CODE, PM_CATEGORY_NAME, "
        '                    'strsql &= "PM_VENDOR_ITEM_CODE, PM_PRODUCT_DESC, PM_UOM, PM_UNIT_COST, PM_GST_CODE, "
        '                    'strsql &= "PM_CURRENCY_CODE, PM_PRODUCT_BRAND, PM_PRODUCT_MODEL, "
        '                    'strsql &= "PM_PRODUCT_TYPE, PM_ENT_BY, PM_ENT_DT, PM_DELETED) VALUES ("
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("CoyId")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("CategoryName")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("VendorItemCode")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductDesc")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("UOM")) & "', "
        '                    'If Common.Parse(dsItem.Tables(0).Rows(0)("UnitCost")) = "" Then
        '                    '    strsql &= "NULL, "
        '                    'Else
        '                    '    strsql &= dsItem.Tables(0).Rows(0)("UnitCost") & ", "
        '                    'End If

        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("gstcode")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("MgmtCode")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("CurrencyCode")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductBrand")) & "', "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductModel")) & "', "
        '                    'strsql &= "NULL, "
        '                    'strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), "
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("MgmtText")) & "','N') "

        '                    strsql = "INSERT INTO PRODUCT_MSTR (PM_S_COY_ID, PM_PRODUCT_CODE, PM_VENDOR_ITEM_CODE, "
        '                    strsql &= "PM_PRODUCT_DESC,PM_REF_NO,PM_LONG_DESC,PM_CATEGORY_NAME,PM_GST_CODE,PM_TAX_PERC, PM_TAX_ID, PM_UOM,"
        '                    strsql &= "PM_UNIT_COST,PM_CURRENCY_CODE,PM_PRODUCT_BRAND, PM_PRODUCT_MODEL,PM_DRAW_NO, "
        '                    strsql &= "PM_VERS_NO,PM_GROSS_WEIGHT,PM_NET_WEIGHT,PM_LENGHT,PM_WIDTH,PM_HEIGHT,PM_VOLUME,"
        '                    strsql &= "PM_COLOR_INFO,PM_HSC_CODE,PM_PACKING_REQ,PM_REMARKS, PM_PRODUCT_FOR, PM_REMARK, PM_ENT_BY, PM_ENT_DT, PM_DELETED) VALUES ("
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("CoyId")) & "', " ' PM_S_COY_ID
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "', " ' PM_PRODUCT_CODE
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ItemName")) & "', " ' PM_PRODUCT_DESC
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ReferenceNo")) & "', " ' PM_REF_NO
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Description")) & "', " ' PM_LONG_DESC
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("CommodityType")) & "', " ' PM_CATEGORY_NAME
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Tax")) & "', " ' PM_GST_CODE
        '                    If IsDBNull(dsItem.Tables(0).Rows(0)("Perc")) Then
        '                        strsql &= "'0', " ' PM_GST_CODE
        '                    Else
        '                        strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Perc")) & "', " ' PM_GST_CODE
        '                    End If
        '                    'strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("TaxId")) & "', " ' PM_TAX_ID
        '                    If IsDBNull(dsItem.Tables(0).Rows(0)("TaxId")) Or Common.parseNull(dsItem.Tables(0).Rows(0)("TaxId")) = "" Then
        '                        strsql &= "'1', " ' PM_TAX_ID
        '                    Else
        '                        strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("TaxId")) & "', " ' PM_TAX_ID
        '                    End If
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '                    If Common.Parse(dsItem.Tables(0).Rows(0)("Price")) = "" Then
        '                        strsql &= "NULL, " ' PM_UNIT_COST
        '                    Else
        '                        strsql &= dsItem.Tables(0).Rows(0)("Price") & ", " ' PM_UNIT_COST
        '                    End If
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("CurrencyCode")) & "', " ' PM_CURRENCY_CODE
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Brand")) & "', " ' PM_PRODUCT_BRAND
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Model")) & "', " ' PM_PRODUCT_MODEL

        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("DrawingNo")) & "', " ' PM_DRAW_NO
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("VersionNo")) & "', " ' PM_VERS_NO
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("GrossWeight")) & "', " ' PM_GROSS_WEIGHT
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("NetWeight")) & "', " ' PM_NET_WEIGHT
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Length")) & "', " ' PM_LENGHT
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Width")) & "', " ' PM_WIDTH
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Height")) & "', " ' PM_HEIGHT
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Volume")) & "', " ' PM_VOLUME
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ColorInfo")) & "', " ' PM_COLOR_INFO
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("HSCode")) & "', " ' PM_HSC_CODE
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Packing")) & "', " ' PM_PACKING_REQ
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Remarks")) & "', " ' PM_REMARKS
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("PM_PRODUCT_FOR")) & "', " ' PM_PRODUCT_FOR
        '                    strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("Remark")) & "', " ' PM_REMARK
        '                    strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE(), 'N') "
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    ' uploaded image
        '                    If strImageIndex <> "" Then
        '                        strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
        '                        strsql &= "SELECT '" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "', PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE "
        '                        strsql &= "FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_ATTACH_INDEX = '" & strImageIndex & "' AND (PA_STATUS = 'U' OR PA_STATUS = 'T') "
        '                        Common.Insert2Ary(strAryQuery, strsql)

        '                        strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = (SELECT MAX(PA_ATTACH_INDEX) FROM PRODUCT_ATTACHMENT WHERE PA_TYPE = 'I') "
        '                        strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "' "
        '                        Common.Insert2Ary(strAryQuery, strsql)
        '                    End If

        '                    'Dim i As Integer
        '                    'For i = 0 To dsItem.Tables(1).Rows.Count - 1
        '                    '    'If Not IsDBNull(dsItem.Tables(1).Rows(i)("Change")) Then
        '                    '    '    If dsItem.Tables(1).Rows(i)("Change") = 1 Then
        '                    '    '        blnDiscount = True
        '                    '    '        strGroupIndex &= dsItem.Tables(1).Rows(i)("DiscIndex") & ","
        '                    '    '    End If
        '                    '    'End If
        '                    '    Select Case dsItem.Tables(1).Rows(i)("chk")
        '                    '        Case "1"
        '                    '            If objDb.Exist("SELECT '*' FROM CONTRACT_DIST_ITEMS WHERE CDI_GROUP_INDEX = " & dsItem.Tables(1).Rows(i)("DiscIndex") & " AND CDI_PRODUCT_CODE = '" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "' ") <= 0 Then
        '                    '                strsql = "INSERT INTO CONTRACT_DIST_ITEMS (CDI_GROUP_INDEX, CDI_PRODUCT_CODE, "
        '                    '                strsql &= "CDI_VENDOR_ITEM_CODE, CDI_PRODUCT_DESC, CDI_UOM, CDI_UNIT_COST, CDI_REMARK, CDI_GST, "
        '                    '                strsql &= "CDI_CURRENCY_CODE, CDI_ENT_BY, CDI_ENT_DT) SELECT "
        '                    '                strsql &= dsItem.Tables(1).Rows(i)("DiscIndex") & ", "
        '                    '                strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "', " ' PM_PRODUCT_CODE
        '                    '                strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("VendorItemCode")) & "', " ' PM_VENDOR_ITEM_CODE
        '                    '                strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductDesc")) & "', " ' PM_PRODUCT_DESC
        '                    '                strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("UOM")) & "', " ' PM_UOM
        '                    '                strsql &= dsItem.Tables(1).Rows(i)("DiscPrice") & ", "
        '                    '                strsql &= "'" & Common.Parse(dsItem.Tables(1).Rows(i)("DiscRemark")) & "', "
        '                    '                strsql &= "(SELECT CODE_VALUE FROM CODE_MSTR WHERE CODE_CATEGORY = 'GST' AND CODE_ABBR = '" & Common.Parse(dsItem.Tables(0).Rows(0)("currencycode")) & "'), "
        '                    '                strsql &= "'" & Common.Parse(dsItem.Tables(0).Rows(0)("currencycode")) & "', " ' PM_CURRENCY_CODE
        '                    '                strsql &= "'" & HttpContext.Current.Session("UserId") & "', GETDATE() "
        '                    '                Common.Insert2Ary(strAryQuery, strsql)
        '                    '            End If
        '                    '        Case "0"
        '                    '            strsql = "DELETE FROM CONTRACT_DIST_ITEMS WHERE "
        '                    '            strsql &= "CDI_GROUP_INDEX = " & dsItem.Tables(1).Rows(i)("DiscIndex") & " "
        '                    '            strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "' "
        '                    '            Common.Insert2Ary(strAryQuery, strsql)
        '                    '    End Select
        '                    'Next
        '                    ' copy uploaded attachment into PRODUCT_ATTACHMENT table
        '                    strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) SELECT "
        '                    strsql &= "PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
        '                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "' AND PA_TYPE = 'D' "
        '                    strsql &= "AND (PA_STATUS = 'U' OR PA_STATUS = 'T')"
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    ' delete from PRODUCT_MSTR_TEMP
        '                    strsql = "DELETE FROM PRODUCT_MSTR_TEMP WHERE PM_PRODUCT_INDEX = '" & strUpdate(0) & "' "
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    ' delete from PRODUCT_ATTACHMENT_TEMP for uploaded image and document
        '                    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_ATTACH_INDEX = '" & strImageIndex & "' "
        '                    strsql &= "OR PA_PRODUCT_CODE = '" & Common.Parse(dsItem.Tables(0).Rows(0)("ProductCode")) & "' "
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                Case "M" ' modify
        '                    strsql = "UPDATE PRODUCT_MSTR SET "

        '                    If strUpdate(1) <> "" Then
        '                        strsql &= "PM_CATEGORY_NAME = '" & Common.Parse(strUpdate(1)) & "', "
        '                    End If
        '                    If strUpdate(2) <> "" Then
        '                        strsql &= "PM_PRODUCT_DESC = '" & Common.Parse(strUpdate(2)) & "', "
        '                    End If
        '                    If strUpdate(3) <> "" Then
        '                        strsql &= "PM_REF_NO = '" & Common.Parse(strUpdate(3)) & "', "
        '                    End If
        '                    If strUpdate(4) <> "" Then
        '                        strsql &= "PM_VENDOR_ITEM_CODE = '" & Common.Parse(strUpdate(4)) & "', "
        '                    End If

        '                    If strUpdate(5) = "" Then
        '                        If strUpdate(5) Is Nothing Then
        '                        Else
        '                            strsql &= "PM_LONG_DESC = '', "
        '                        End If
        '                    Else
        '                        strsql &= "PM_LONG_DESC = '" & Common.Parse(strUpdate(5)) & "', "
        '                    End If

        '                    If strUpdate(6) <> "" Then
        '                        strsql &= "PM_PRODUCT_BRAND = '" & Common.Parse(strUpdate(6)) & "', "
        '                    End If
        '                    If strUpdate(7) <> "" Then
        '                        strsql &= "PM_PRODUCT_MODEL = '" & Common.Parse(strUpdate(7)) & "', "
        '                    End If
        '                    If strUpdate(8) <> "" Then
        '                        strsql &= "PM_UOM = '" & Common.Parse(strUpdate(8)) & "', "
        '                    End If
        '                    If strUpdate(9) <> "" Then
        '                        strsql &= "PM_CURRENCY_CODE = '" & Common.Parse(strUpdate(9)) & "', "
        '                    End If

        '                    'Chee Hong - GST Agora Enhancement - 10/9/2014
        '                    If strUpdate(10) <> "" Then
        '                        'strsql &= "PM_TAX_ID = '" & Common.Parse(strUpdate(10)) & "', "
        '                        strsql &= "PM_GST_CODE = '" & Common.Parse(strUpdate(10)) & "', "
        '                    End If
        '                    '---------------------------------------------


        '                    If strUpdate(12) <> "" Then
        '                        strsql &= "PM_UNIT_COST = '" & Common.Parse(strUpdate(12)) & "', "
        '                    End If

        '                    If strUpdate(13) <> "" Then
        '                        strsql &= "PM_DRAW_NO = '" & Common.Parse(strUpdate(13)) & "', "
        '                    End If

        '                    If strUpdate(14) <> "" Then
        '                        strsql &= "PM_VERS_NO = '" & Common.Parse(strUpdate(14)) & "', "
        '                    End If

        '                    If strUpdate(15) <> "" Then
        '                        strsql &= "PM_GROSS_WEIGHT = '" & Common.Parse(strUpdate(15)) & "', "
        '                    End If

        '                    If strUpdate(16) <> "" Then
        '                        strsql &= "PM_NET_WEIGHT = '" & Common.Parse(strUpdate(16)) & "', "
        '                    End If

        '                    If strUpdate(17) <> "" Then
        '                        strsql &= "PM_LENGHT = '" & Common.Parse(strUpdate(17)) & "', "
        '                    End If

        '                    If strUpdate(18) <> "" Then
        '                        strsql &= "PM_WIDTH = '" & Common.Parse(strUpdate(18)) & "', "
        '                    End If

        '                    If strUpdate(19) <> "" Then
        '                        strsql &= "PM_HEIGHT = '" & Common.Parse(strUpdate(19)) & "', "
        '                    End If
        '                    If strUpdate(20) <> "" Then
        '                        strsql &= "PM_VOLUME = '" & Common.Parse(strUpdate(20)) & "', "
        '                    End If
        '                    If strUpdate(21) <> "" Then
        '                        strsql &= "PM_COLOR_INFO = '" & Common.Parse(strUpdate(21)) & "', "
        '                    End If
        '                    If strUpdate(22) <> "" Then
        '                        strsql &= "PM_HSC_CODE = '" & Common.Parse(strUpdate(22)) & "', "
        '                    End If
        '                    If strUpdate(23) <> "" Then
        '                        strsql &= "PM_PACKING_REQ = '" & Common.Parse(strUpdate(23)) & "', "
        '                    End If
        '                    If strUpdate(23) = "" Then
        '                        If strUpdate(23) Is Nothing Then
        '                        Else
        '                            strsql &= "PM_REMARKS = '', "
        '                        End If
        '                    Else
        '                        strsql &= "PM_REMARKS = '" & Common.Parse(strUpdate(24)) & "', "
        '                    End If

        '                    If strUpdate(25) <> "" Then
        '                        strsql &= "PM_REMARK = '" & Common.Parse(strUpdate(25)) & "', "
        '                    End If
        '                    If strUpdate(26) <> "" Then
        '                        strsql &= "PM_TAX_PERC = '" & Common.Parse(strUpdate(26)) & "', "
        '                    End If
        '                    If strUpdate(27) <> "" Then
        '                        strsql &= "PM_TAX_ID = '" & Common.Parse(strUpdate(27)) & "', "
        '                    End If

        '                    strsql &= "PM_MOD_BY = '" & HttpContext.Current.Session("UserId") & "', "
        '                    strsql &= "PM_MOD_DT = GETDATE() WHERE PM_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "'; "
        '                    'Michelle (eBiz/134/08) - Update Shopping Cart also
        '                    strsql &= "UPDATE SHOPPING_CART SET  "
        '                    strsql &= "SC_VENDOR_ITEM_CODE = '" & Common.Parse(strUpdate(2)) & "', "
        '                    strsql &= "SC_PRODUCT_DESC = '" & Common.Parse(strUpdate(3)) & "', "
        '                    strsql &= "SC_CURRENCY_CODE = '" & Common.Parse(strUpdate(7)) & "', "
        '                    strsql &= "SC_UOM = '" & Common.Parse(strUpdate(4)) & "' "
        '                    strsql &= "WHERE SC_SOURCE = 'LP' AND SC_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "' "
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    ' insert uploaded image
        '                    If strImageIndex <> "" Then
        '                        If strImageIndex <> "0" Then
        '                            ' delete from PRODUCT_ATTACHMENT_TEMP
        '                            strsql = "DELETE FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "' "
        '                            strsql &= "AND PA_TYPE = 'I' "
        '                            Common.Insert2Ary(strAryQuery, strsql)

        '                            strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) "
        '                            strsql &= "SELECT '" & Common.Parse(strUpdate(11)) & "', PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE "
        '                            strsql &= "FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_ATTACH_INDEX = '" & strImageIndex & "' AND PA_STATUS <> 'D' "
        '                            Common.Insert2Ary(strAryQuery, strsql)

        '                            strsql = "UPDATE PRODUCT_MSTR SET PM_PRODUCT_IMAGE = (SELECT MAX(PA_ATTACH_INDEX) FROM PRODUCT_ATTACHMENT WHERE PA_TYPE = 'I') "
        '                            strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "' "
        '                            Common.Insert2Ary(strAryQuery, strsql)
        '                        End If
        '                    End If

        '                    ' copy uploaded attachment into PRODUCT_ATTACHMENT table
        '                    ' if attachment not exist, then insert, else update
        '                    strsql = "INSERT INTO PRODUCT_ATTACHMENT (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE) SELECT "
        '                    strsql &= "PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE FROM PRODUCT_ATTACHMENT_TEMP "
        '                    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "' "
        '                    strsql &= "AND PA_TYPE = 'D' AND (PA_STATUS = 'U' OR PA_STATUS = 'T') " ' no need to copy deleted Attachment before user save the record
        '                    strsql &= "AND PA_ATTACH_FILENAME NOT IN ("
        '                    strsql &= "SELECT PA_ATTACH_FILENAME FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "')"
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    'Michelle (21/9/2010) - To remove the FROM from the UPDATE statement
        '                    'strsql = "UPDATE PRODUCT_ATTACHMENT SET "
        '                    strsql = "UPDATE PRODUCT_ATTACHMENT,  PRODUCT_ATTACHMENT_TEMP SET "
        '                    strsql &= "PRODUCT_ATTACHMENT.PA_HUB_FILENAME = PRODUCT_ATTACHMENT_TEMP.PA_HUB_FILENAME, "
        '                    strsql &= "PRODUCT_ATTACHMENT.PA_FILESIZE = PRODUCT_ATTACHMENT_TEMP.PA_FILESIZE "
        '                    'strsql &= "FROM PRODUCT_ATTACHMENT_TEMP "
        '                    strsql &= "WHERE PRODUCT_ATTACHMENT.PA_PRODUCT_CODE = PRODUCT_ATTACHMENT_TEMP.PA_PRODUCT_CODE "
        '                    strsql &= "AND PRODUCT_ATTACHMENT.PA_ATTACH_FILENAME = PRODUCT_ATTACHMENT_TEMP.PA_ATTACH_FILENAME "
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    ' delete from PRODUCT_ATTACHMENT_TEMP for Image file only
        '                    'Michelle (8/5/2011) - To delete the product_attachment_temp base on the product_code only
        '                    'strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE (PA_ATTACH_INDEX = "
        '                    'strsql &= "(SELECT PM_PRODUCT_IMAGE FROM PRODUCT_MSTR_TEMP WHERE PM_PRODUCT_INDEX = " & strUpdate(0) & ")) "
        '                    'strsql &= "OR (PA_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "' AND PA_SOURCE = 'H')"
        '                    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE (PA_PRODUCT_CODE = "
        '                    strsql &= "(SELECT PM_PRODUCT_CODE FROM PRODUCT_MSTR_TEMP WHERE PM_PRODUCT_INDEX = " & strUpdate(0) & ")) "
        '                    strsql &= "OR (PA_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "' AND PA_TYPE = 'I')"
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    ' delete from PRODUCT_MSTR_TEMP
        '                    strsql = "DELETE FROM PRODUCT_MSTR_TEMP WHERE PM_PRODUCT_INDEX = " & strUpdate(0)
        '                    Common.Insert2Ary(strAryQuery, strsql)

        '                    ' delete from PRODUCT_ATTACHMENT_TEMP for all document files for current Item
        '                    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strUpdate(11)) & "' "
        '                    'strsql &= "AND PA_TYPE = 'D'"
        '                    Common.Insert2Ary(strAryQuery, strsql)
        '            End Select
        '    End Select

        '    If objDb.BatchExecute(strAryQuery) Then
        '        If blnSend Then
        '            Dim objMail As New Email
        '            If blnApproved Then
        '                objMail.sendNotification(EmailType.ListPricePublished, HttpContext.Current.Session("UserId"), "", strVendor, strUpdate(11), "")
        '            Else
        '                objMail.sendNotification(EmailType.ListPristRejected, HttpContext.Current.Session("UserId"), "", strVendor, strUpdate(11), "")
        '            End If

        '            If blnList Then
        '                dtr = dt1.NewRow()
        '                dtr("ItemId") = Common.Parse(strUpdate(11))
        '                dtr("ItemDesc") = Common.Parse(dsItem.Tables(0).Rows(0)("Description"))
        '                dt1.Rows.Add(dtr)
        '                If strGroupIndex <> "" Then
        '                    strGroupIndex = strGroupIndex.Substring(0, strGroupIndex.Length - 1)
        '                End If
        '                If objDb.Exist("SELECT CV_B_COY_ID FROM COMPANY_VENDOR WHERE CV_S_COY_ID = '" & HttpContext.Current.Session("CompanyIdToken") & "'") Then
        '                    objMail.sendItemUpdateMail(HttpContext.Current.Session("CompanyIdToken"), strGroupIndex, dt1, dt2, True, blnDiscount)
        '                End If
        '            End If
        '            objMail = Nothing
        '        End If
        '        updateProductMstr = True
        '    End If
        'End Function

        'Public Function getDiscountGroupList(ByVal strItemId As String, ByVal strType As String, ByVal strVendor As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT '1' AS CHK, PDT_DISC_INDEX, PDT_DISC_CODE, PDT_DISC_PRICE, PDT_DISC_REMARK FROM PRODUCT_DISCGRP_TEMP "
        '    strsql &= "WHERE PDT_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' "
        '    strsql &= "AND PDT_TYPE = '" & strType & "' "
        '    strsql &= "UNION "
        '    strsql &= "SELECT DISTINCT '1' AS CHK, CDM_GROUP_INDEX AS PDT_DISC_INDEX, CDM_GROUP_CODE AS PDT_DISC_CODE, CDI_UNIT_COST AS PDT_DISC_PRICE, CDI_REMARK AS PDT_DISC_REMARK FROM CONTRACT_DIST_MSTR "
        '    strsql &= "LEFT JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX "
        '    strsql &= "WHERE CDM_TYPE = '" & strType & "' AND CDM_S_COY_ID = '" & Common.Parse(strVendor) & "' "
        '    strsql &= "AND ((GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE + 1) OR (CDM_END_DATE IS NULL)) "
        '    strsql &= "AND CDI_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' "
        '    strsql &= "AND CDM_GROUP_CODE NOT IN "
        '    strsql &= "(SELECT PDT_DISC_CODE FROM PRODUCT_DISCGRP_TEMP WHERE PDT_PRODUCT_CODE = '" & Common.Parse(strItemId) & "') "
        '    strsql &= "UNION "
        '    strsql &= "SELECT DISTINCT '0' AS CHK, CDM_GROUP_INDEX AS PDT_DISC_INDEX, CDM_GROUP_CODE AS PDT_DISC_CODE, NULL AS PDT_DISC_PRICE, NULL AS PDT_DISC_REMARK FROM CONTRACT_DIST_MSTR "
        '    strsql &= "LEFT JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX "
        '    strsql &= "WHERE CDM_TYPE = '" & strType & "' AND CDM_S_COY_ID = '" & Common.Parse(strVendor) & "' "
        '    strsql &= "AND ((GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE + 1) OR (CDM_END_DATE IS NULL)) "
        '    strsql &= "AND CDM_GROUP_CODE NOT IN "
        '    strsql &= "(SELECT CDM_GROUP_CODE AS PDT_DISC_CODE FROM CONTRACT_DIST_MSTR LEFT JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX = CDM_GROUP_INDEX "
        '    strsql &= "WHERE CDM_TYPE = '" & strType & "' AND CDM_S_COY_ID = '" & Common.Parse(strVendor) & "' "
        '    strsql &= "AND ((GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE + 1) OR (CDM_END_DATE IS NULL)) AND CDI_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' ) "
        '    strsql &= "AND CDM_GROUP_CODE NOT IN (SELECT PDT_DISC_CODE FROM PRODUCT_DISCGRP_TEMP "
        '    strsql &= "WHERE PDT_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PDT_TYPE = '" & strType & "' )"
        '    strsql &= "ORDER BY PDT_DISC_CODE "
        '    ds = objDb.FillDs(strsql)
        '    getDiscountGroupList = ds
        'End Function

        '' ai chu add on 19/09/2005
        '' when Hub admin want to modify an item, system will copy 1 set of attachment file 
        '' into Product_Attchment_Temp table.
        '' so, we cannot use PM_PRODUCT_IMAGE field value
        '' only for approval 
        'Public Function getItemTempImage(ByVal strProductCode As String) As String
        '    Dim strsql As String

        '    strsql = "SELECT '*' FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_SOURCE = 'H' "
        '    strsql &= "AND PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    strsql &= "AND PA_TYPE = 'I' AND PA_STATUS <> 'U' "

        '    If objDb.Exist(strsql) > 0 Then
        '        ' delete hub admin file
        '        strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_SOURCE = 'H' "
        '        strsql &= "AND PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '        strsql &= "AND PA_TYPE = 'I' AND PA_STATUS <> 'U' "
        '        objDb.Execute(strsql)
        '    End If

        '    strsql = "SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    strsql &= "AND PA_TYPE = 'I' AND PA_STATUS = 'U' "
        '    Return objDb.GetVal(strsql)
        'End Function

        'Public Function getTempImageAttach(ByVal strIndex As String, ByVal blnTemp As Boolean, Optional ByVal strProdCode As String = "") As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    'strsql = "SELECT * FROM "
        '    'If blnTemp Then
        '    '    strsql &= "PRODUCT_ATTACHMENT_TEMP "
        '    '    strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND (PA_STATUS = 'U' OR PA_STATUS = 'T')"
        '    'Else
        '    '    strsql &= "PRODUCT_ATTACHMENT "
        '    '    strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' "
        '    'End If

        '    If objDb.GetCount("PRODUCT_ATTACHMENT", "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_ATTACH_INDEX NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I'  AND PA_SOURCE = 'V' AND PA_STATUS = 'D') LIMIT 1") = 1 Then
        '        'Vendor side, modify item
        '        strsql = "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT  "
        '        strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_PRODUCT_CODE NOT IN (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I'  AND PA_SOURCE = 'V' "
        '        strsql &= "AND PA_STATUS = 'D') "
        '    Else 'Vendor side, adding new item
        '        'Michelle (8/5/2011) - To add in checking of the product code
        '        'strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT_TEMP  "
        '        'strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND (PA_STATUS = 'U' OR PA_STATUS = 'T') AND PA_TYPE = 'I' "
        '        'strsql &= "UNION "
        '        'strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT  "
        '        'strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_ATTACH_INDEX NOT IN (SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I'  AND PA_SOURCE = 'V' "
        '        'strsql &= "AND PA_STATUS = 'D') "
        '        If strProdCode <> "" Then
        '            strsql = "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT_TEMP  "
        '            strsql &= "WHERE PA_PRODUCT_CODE = '" & strProdCode & "' AND PA_ATTACH_INDEX = '" & strIndex & "' AND (PA_STATUS = 'U' OR PA_STATUS = 'T') AND PA_TYPE = 'I' "
        '        Else
        '            strsql = "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT_TEMP  "
        '            strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND (PA_STATUS = 'U' OR PA_STATUS = 'T') AND PA_TYPE = 'I' "
        '        End If
        '        strsql &= "UNION "
        '        strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT  "
        '        strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_PRODUCT_CODE NOT IN (SELECT PA_PRODUCT_CODE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I'  AND PA_SOURCE = 'V' "
        '        strsql &= "AND PA_STATUS = 'D') "
        '    End If

        '    ds = objDb.FillDs(strsql)
        '    getTempImageAttach = ds
        'End Function

        'Public Function getTempAttach(ByVal strItemId As String, ByVal blnTemp As Boolean) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT * "
        '    If blnTemp Then
        '        strsql &= "FROM PRODUCT_ATTACHMENT_TEMP "
        '        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D' AND PA_SOURCE = 'V'"
        '        strsql &= "AND (PA_STATUS = 'U' OR PA_STATUS = 'T') "
        '    Else
        '        'strsql &= "FROM PRODUCT_ATTACHMENT "
        '        'strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D' "

        '        'strsql &= "FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D' AND (PA_STATUS = 'U' OR PA_STATUS = 'T') AND PA_ATTACH_INDEX NOT IN (SELECT PA_ATTACH_INDEX FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D')"
        '        strsql = "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT_TEMP "
        '        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D' AND PA_SOURCE = 'V' "
        '        strsql &= "AND (PA_STATUS = 'U' OR PA_STATUS = 'T') AND "
        '        strsql &= "PA_HUB_FILENAME NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D'  AND PA_SOURCE = 'V' "
        '        strsql &= "AND PA_STATUS = 'D') "
        '        strsql &= "UNION "
        '        strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT "
        '        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D' "
        '        strsql &= "AND PA_HUB_FILENAME NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D'  AND PA_SOURCE = 'V' "
        '        strsql &= "AND PA_STATUS = 'D') "


        '    End If
        '    ds = objDb.FillDs(strsql)
        '    getTempAttach = ds
        'End Function
        Public Function getTempAttachBuyer(ByVal strItemId As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            'strsql = "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX "
            '          If blnTemp Then
            strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT_TEMP "
            strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D' AND PA_SOURCE = 'B' "
            strsql &= "AND (PA_STATUS = 'U' OR PA_STATUS = 'T') AND "
            strsql &= "PA_ATTACH_INDEX NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D'  AND PA_SOURCE = 'B' "
            strsql &= "AND PA_STATUS = 'D') "
            strsql &= "UNION "
            strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT "
            strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D' "
            strsql &= "AND PA_ATTACH_INDEX NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' AND PA_TYPE = 'D'  AND PA_SOURCE = 'B' "
            strsql &= "AND PA_STATUS = 'D') "
            'End If"
            ds = objDb.FillDs(strsql)
            getTempAttachBuyer = ds
        End Function

        ''Michelle 25/1/2011 - Check for pending records for approval
        'Public Function WithPendingRec(ByVal pitemid As String) As Boolean
        '    Dim strsql As String
        '    Dim objDB As New EAD.DBCom

        '    strsql = "SELECT * FROM PRODUCT_MSTR_TEMP WHERE PM_PRODUCT_CODE = '" & pitemid & "' AND "
        '    strsql &= "PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_STATUS = '1' "
        '    If objDB.Exist(strsql) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function
        Public Function getTempImageAttachBuyer(ByVal strIndex As String) As DataSet
            Dim strsql As String = ""
            Dim ds As New DataSet

            'This was only used in 1 screen, BIM.aspx
            'System to check only return 1 image, select from mstr table first.

            If objDb.GetCount("PRODUCT_ATTACHMENT", "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_ATTACH_INDEX NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I'  AND PA_SOURCE = 'B' AND PA_STATUS = 'D') LIMIT 1") = 1 Then
                'Temporary solution, return only 1 image
                strsql = "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT  "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_ATTACH_INDEX NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I'  AND PA_SOURCE = 'B' "
                strsql &= "AND PA_STATUS = 'D') "
            Else
                strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT_TEMP  "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND (PA_STATUS = 'U' OR PA_STATUS = 'T') AND PA_TYPE = 'I' "
                strsql &= "UNION "
                strsql &= "SELECT PA_ATTACH_FILENAME, PA_HUB_FILENAME, PA_ATTACH_INDEX, PA_FILESIZE FROM PRODUCT_ATTACHMENT  "
                strsql &= "WHERE PA_ATTACH_INDEX = '" & strIndex & "' AND PA_TYPE = 'I' AND PA_ATTACH_INDEX NOT IN (SELECT PA_HUB_FILENAME FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I'  AND PA_SOURCE = 'B' "
                strsql &= "AND PA_STATUS = 'D') "
            End If
            ds = objDb.FillDs(strsql)
            getTempImageAttachBuyer = ds
        End Function

        '' ai chu modified on 20/09/2005
        'Public Function copyAttachFile(ByVal strItemId As String, Optional ByVal strFrm As String = "") As String
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    ' before copy, change status from 'D' to 'U' 
        '    ' delete attachment with status = 'T'

        '    strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' "
        '    strsql &= "AND PA_STATUS = 'T' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If strFrm <> "Hub" Then
        '        strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'U' "
        '        strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "' "
        '        strsql &= "AND PA_STATUS = 'D' "
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If
        '    strsql = "INSERT INTO PRODUCT_ATTACHMENT_TEMP (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE, PA_SOURCE, PA_STATUS, PA_DATE) "
        '    strsql &= "SELECT '" & Common.Parse(strItemId) & "', PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE, 'V', 'U',GETDATE() "
        '    strsql &= "FROM PRODUCT_ATTACHMENT WHERE PA_PRODUCT_CODE = '" & strItemId & "' "
        '    strsql &= "AND PA_ATTACH_FILENAME NOT IN ( "
        '    strsql &= "SELECT PA_ATTACH_FILENAME FROM PRODUCT_ATTACHMENT_TEMP "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & strItemId & "' AND PA_STATUS <> 'D' ) "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If objDb.BatchExecute(strAryQuery) Then
        '        copyAttachFile = objDb.GetVal("SELECT ISNULL(MAX(PA_ATTACH_INDEX), 0) FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_TYPE = 'I' AND PA_PRODUCT_CODE = '" & strItemId & "' ")
        '    End If
        'End Function

        ''Michelle (5/11/2010) - To delete those attachement that user didn't save
        Public Function deleteTempAttachment(ByVal strItemId As String, ByVal strSessionID As String) As String
            Dim strsql As String
            Dim strAryQuery(0) As String
            strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP WHERE (PA_PRODUCT_CODE = '" & Common.Parse(strSessionID) & "' OR PA_PRODUCT_CODE = '" & Common.Parse(strItemId) & "') "
            strsql &= "AND (PA_STATUS = 'T' or PA_STATUS = 'D')"
            'strsql &= "AND (PA_STATUS = 'T' or PA_STATUS = 'D') AND PA_SOURCE = 'B' "
            Common.Insert2Ary(strAryQuery, strsql)

            objDb.Execute(strsql)
        End Function
        'Public Function resetDeletedAttachment(ByVal strProductCode As String)
        '    Dim strsql As String
        '    strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'U' "
        '    strsql &= "WHERE PA_PRODUCT_CODE = '" & Common.Parse(strProductCode) & "' "
        '    objDb.Execute(strsql)
        'End Function

        Public Function deleteAttachment(ByVal intIndex As Integer, ByVal blnTemp As Boolean, Optional ByVal strItemId As String = "", Optional ByVal strSource As String = "", Optional ByVal strType As String = "", Optional ByVal strPrdCode As String = "")
            Dim strsql As String
            ' ai chu remark on 19/09/2005
            ' no need to delete from database
            ' just update status from 'U' or 'T' to 'D'

            If strItemId <> "" Then
                Select Case blnTemp
                    Case False
                        'strsql = "DELETE FROM PRODUCT_ATTACHMENT "
                        'strsql &= "WHERE PA_PRODUCT_CODE = '" & strItemId & "'; "
                        strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'D' "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & strItemId & "'; "
                        strsql &= "UPDATE PRODUCT_MSTR SET "
                        strsql &= "PM_PRODUCT_IMAGE = '' "
                        strsql &= "WHERE PM_PRODUCT_CODE = '" & strItemId & "' "

                    Case True
                        'strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                        'strsql &= "WHERE PA_PRODUCT_CODE = '" & strItemId & "' "
                        strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'D' "
                        strsql &= "WHERE PA_PRODUCT_CODE = '" & strItemId & "' "
                End Select
            Else

                ''Original Code
                'Select Case blnTemp
                '    Case False
                '        'strsql = "DELETE FROM PRODUCT_ATTACHMENT "
                '        'strsql &= "WHERE PA_ATTACH_INDEX = " & intIndex
                '        strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'D' "
                '        strsql &= "WHERE PA_ATTACH_INDEX = " & intIndex
                '    Case True
                '        'strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
                '        'strsql &= "WHERE PA_ATTACH_INDEX = " & intIndex
                '        strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'D' "
                '        strsql &= "WHERE PA_ATTACH_INDEX = " & intIndex
                'End Select

                'Michelle (6/5/2011) - To cater for deletion of image
                If strType = "I" Then
                    If objDb.Exist("SELECT * FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & strPrdCode & "' AND PA_SOURCE = '" & strSource & "' AND PA_TYPE = '" & strType & "'") > 0 Then
                        'The following path is for new item
                        strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'D' "
                        strsql &= "WHERE PA_SOURCE = '" & strSource & "' AND PA_TYPE = '" & strType & "'"
                    Else
                        'The following path is for modifying existing item
                        Dim tDS As DataSet = objDb.Fill1Ds("PRODUCT_ATTACHMENT", "PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE", "WHERE PA_PRODUCT_CODE = '" & strPrdCode & "' AND PA_TYPE = '" & strType & "'")

                        strsql = "INSERT INTO PRODUCT_ATTACHMENT_TEMP (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE, PA_SOURCE, PA_STATUS, PA_DATE) VALUES (" &
                        "'" & tDS.Tables(0).Rows(0).Item(0) & "', '" & tDS.Tables(0).Rows(0).Item(1) & "', '" & tDS.Tables(0).Rows(0).Item(2) & "', '" & tDS.Tables(0).Rows(0).Item(3) & "', '" & tDS.Tables(0).Rows(0).Item(4) & "', 'V', 'D',GETDATE())"
                    End If
                Else 'For document
                    'If objDb.Exist("SELECT * FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_PRODUCT_CODE = '" & strPrdCode & "' AND PA_SOURCE = '" & strSource & "' AND PA_TYPE = '" & strType & "' AND PA_ATTACH_FILENAME = ''") > 0 Then
                    If objDb.Exist("SELECT * FROM PRODUCT_ATTACHMENT_TEMP WHERE PA_ATTACH_INDEX = " & intIndex & " AND PA_PRODUCT_CODE = '" & strPrdCode & "' AND PA_SOURCE = '" & strSource & "' AND PA_TYPE = '" & strType & "'") > 0 Then
                        strsql = "UPDATE PRODUCT_ATTACHMENT_TEMP SET PA_STATUS = 'D' "
                        strsql &= "WHERE PA_ATTACH_INDEX = " & intIndex & " AND PA_PRODUCT_CODE = '" & strPrdCode & "' AND PA_SOURCE = '" & strSource & "' AND PA_TYPE = '" & strType & "'"
                    Else
                        Dim tDS As DataSet = objDb.Fill1Ds("PRODUCT_ATTACHMENT", "PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE", "WHERE PA_ATTACH_INDEX = '" & intIndex & "' AND PA_PRODUCT_CODE = '" & strPrdCode & "'")

                        strsql = "INSERT INTO PRODUCT_ATTACHMENT_TEMP (PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_ATTACH_FILENAME, PA_FILESIZE, PA_TYPE, PA_SOURCE, PA_STATUS, PA_DATE) VALUES (" &
                        "'" & tDS.Tables(0).Rows(0).Item(0) & "', '" & tDS.Tables(0).Rows(0).Item(1) & "', '" & tDS.Tables(0).Rows(0).Item(2) & "', '" & tDS.Tables(0).Rows(0).Item(3) & "', '" & tDS.Tables(0).Rows(0).Item(4) & "', 'V', 'D',GETDATE())"
                        'testing
                    End If

                End If
            End If

            objDb.Execute(strsql)
        End Function
        ''Michelle (5/11/2010)
        Public Function deleteAttachmentBuyer(ByVal intIndex As Integer, ByVal strPrdCode As String, ByVal strType As String)
            Dim strsql As String

            'Delete from temp table
            strsql = "DELETE FROM PRODUCT_ATTACHMENT_TEMP "
            strsql &= "WHERE PA_ATTACH_INDEX = " & intIndex & " AND PA_PRODUCT_CODE = '" & strPrdCode & "' AND PA_TYPE = '" & strType & "' "
            objDb.Execute(strsql)

            'PA_HUB_FILENAME will store the PA_ATTACH_INDEX
            strsql = "INSERT INTO PRODUCT_ATTACHMENT_TEMP "
            strsql &= "(PA_PRODUCT_CODE, PA_HUB_FILENAME, PA_TYPE, PA_STATUS, PA_SOURCE, PA_DATE) "
            strsql &= "SELECT PA_PRODUCT_CODE, PA_ATTACH_INDEX, PA_TYPE, 'D', 'B', GETDATE() FROM PRODUCT_ATTACHMENT "
            strsql &= "WHERE PA_ATTACH_INDEX = " & intIndex & " AND PA_PRODUCT_CODE = '" & strPrdCode & "' AND PA_TYPE = '" & strType & "' "

            objDb.Execute(strsql)
        End Function

        'Public Function deteleProductMstr(ByVal dtItem As DataTable, ByVal strVendor As String, ByRef strDeleted As String, ByRef strNotDeleted As String)
        '    Dim strsql As String
        '    Dim strExist1, strExist2, strExist3, strExist4 As String ' item exist in PR
        '    Dim i As Integer
        '    Dim blnDelete As Boolean = False
        '    Dim strAryQuery(0) As String

        '    strNotDeleted = ""
        '    strDeleted = ""

        '    For i = 0 To dtItem.Rows.Count - 1
        '        ' check item exists in outstanding PR (status = 1,2,3,4)
        '        strExist1 = "SELECT '*' FROM PR_DETAILS LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRM_S_COY_ID = '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "' "
        '        strExist1 &= "WHERE PRM_PR_STATUS IN (1,2,3,4) AND PRD_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "

        '        ' check item exists in outstanding PO (status = 1,2)
        '        strExist2 = "SELECT '*' FROM PO_DETAILS LEFT JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO "
        '        strExist2 &= "WHERE POM_PO_STATUS IN (1,2) AND POD_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "'"

        '        ' check item exists under contract period
        '        strExist3 = "SELECT '*' FROM CONTRACT_DIST_ITEMS LEFT JOIN CONTRACT_DIST_MSTR ON CDI_GROUP_INDEX = CDM_GROUP_INDEX AND CDM_S_COY_ID = '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "' "
        '        strExist3 &= "WHERE ((GETDATE() BETWEEN CDM_START_DATE AND CDM_END_DATE + 1 ) OR CDM_END_DATE IS NULL) AND CDI_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '        strExist3 &= "AND CDI_GROUP_INDEX IN (SELECT CDM_GROUP_INDEX FROM CONTRACT_DIST_MSTR) "

        '        If (objDb.Exist(strExist1) > 0) Or (objDb.Exist(strExist2) > 0) Or (objDb.Exist(strExist3) > 0) Then
        '            strNotDeleted &= dtItem.Rows(i)("VIC") & ","
        '        Else
        '            blnDelete = True
        '            ' delete from PRODUCT_MSTR table
        '            strDeleted &= dtItem.Rows(i)("VIC") & ", "
        '            '---- physical delete --------
        '            'strsql = "DELETE FROM PRODUCT_MSTR "
        '            'strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '            '-----------------------------

        '            '---- logical delete ---------
        '            strsql = "UPDATE PRODUCT_MSTR "
        '            strsql &= "SET PM_DELETED = 'Y' "
        '            strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)
        '        End If
        '    Next

        '    If blnDelete Then
        '        If objDb.BatchExecute(strAryQuery) Then
        '            'Dim objMail As New WheelMail
        '            'Dim strAryDeleted() As String
        '            'strAryDeleted = strDeleted.Split(",")
        '            'For i = 0 To strAryDeleted.Length - 2
        '            '    objMail.sendNotification(EmailType.ListPriceDeleted, HttpContext.Current.Session("UserId"), "", strVendor, strAryDeleted(i), "")
        '            'Next
        '            'objMail = Nothing
        '            deteleProductMstr = WheelMsgNum.Delete
        '        End If
        '    End If
        'End Function
        ''Michelle (29/10/2010) - For BIM
        'Public Function UpdBuyerProductMstr(ByVal dtItem As DataTable, ByVal strCoyId As String, ByVal strActive As String)
        '    Dim strsql, strsql1 As String
        '    Dim i As Integer
        '    Dim blnDelete As Boolean = False
        '    Dim strAryQuery(0) As String
        '    Dim strPRExist As String ' item exist in PR
        '    Dim strPOExist As String

        '    For i = 0 To dtItem.Rows.Count - 1
        '        If strActive = "Y" Then 'Deactivate the item - physical delete from all the Default Purchaser Catalogue
        '            strPRExist = "SELECT '*' " _
        '                   & "FROM PR_DETAILS " _
        '                   & "LEFT JOIN PR_MSTR ON PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID " _
        '                   & "WHERE PRM_PR_STATUS IN (1) AND PRD_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '            strPOExist = "SELECT '*' " _
        '                    & "FROM PO_DETAILS " _
        '                    & "LEFT JOIN PO_MSTR ON POD_COY_ID = POM_B_COY_ID AND POM_PO_NO = POD_PO_NO " _
        '                    & "WHERE POM_PO_STATUS IN (0) AND POD_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '            If objDb.Exist(strPRExist) > 0 Or objDb.Exist(strPOExist) > 0 Then
        '                blnDelete = False

        '            Else
        '                blnDelete = True
        '                ' logical delete from PRODUCT_MSTR table
        '                strsql = "UPDATE PRODUCT_MSTR "
        '                strsql &= "SET PM_DELETED = '" & strActive & "' "
        '                strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '                Common.Insert2Ary(strAryQuery, strsql)

        '                strsql1 = "DELETE FROM BUYER_CATALOGUE_ITEMS WHERE BCU_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '                strsql1 &= "AND BCU_CAT_INDEX IN (SELECT BCM_CAT_INDEX "
        '                strsql1 &= "FROM BUYER_CATALOGUE_MSTR WHERE BCM_B_COY_ID = '" & strCoyId & "') "
        '                Common.Insert2Ary(strAryQuery, strsql1)
        '            End If
        '        Else    'Activate the item
        '            ' logical delete from PRODUCT_MSTR table
        '            blnDelete = True
        '            strsql = "UPDATE PRODUCT_MSTR "
        '            strsql &= "SET PM_DELETED = '" & strActive & "' "
        '            strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            Dim objDBAccess As New EAD.DBCom
        '            If objDBAccess.GetCount("BUYER_CATALOGUE_MSTR, BUYER_CATALOGUE_ITEMS", "WHERE BCU_CAT_INDEX = BCM_CAT_INDEX AND BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "' AND BCU_PRODUCT_CODE='" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "'") = 0 And
        '                objDBAccess.GetCount("BUYER_CATALOGUE_MSTR", "WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "' ") > 0 Then

        '                strsql = "INSERT INTO BUYER_CATALOGUE_ITEMS (BCU_CAT_INDEX, BCU_PRODUCT_CODE, BCU_SOURCE, BCU_S_COY_ID, BCU_ENT_BY, BCU_ENT_DATETIME) "
        '                strsql &= "VALUES ((SELECT BCM_CAT_INDEX FROM BUYER_CATALOGUE_MSTR WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "'), "
        '                strsql &= "'" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "', '', '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "', "
        '                strsql &= "'" & HttpContext.Current.Session("UserId") & "', getdate())"
        '                Common.Insert2Ary(strAryQuery, strsql)
        '            End If
        '        End If

        '        'blnDelete = True
        '        '' logical delete from PRODUCT_MSTR table
        '        'strsql = "UPDATE PRODUCT_MSTR "
        '        'strsql &= "SET PM_DELETED = '" & strActive & "' "
        '        'strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '        'Common.Insert2Ary(strAryQuery, strsql)

        '        'If strActive = "Y" Then 'physical delete from all the Default Purchaser Catalogue
        '        '    strsql1 = "DELETE FROM BUYER_CATALOGUE_ITEMS WHERE BCU_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "' "
        '        '    strsql1 &= "AND BCU_CAT_INDEX IN (SELECT BCM_CAT_INDEX "
        '        '    strsql1 &= "FROM BUYER_CATALOGUE_MSTR WHERE BCM_B_COY_ID = '" & strCoyId & "') "
        '        '    Common.Insert2Ary(strAryQuery, strsql1)
        '        'Else
        '        '    Dim objDBAccess As New EAD.DBCom
        '        '    If objDBAccess.GetCount("BUYER_CATALOGUE_MSTR, BUYER_CATALOGUE_ITEMS", "WHERE BCU_CAT_INDEX = BCM_CAT_INDEX AND BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "' AND BCU_PRODUCT_CODE='" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "'") = 0 Then
        '        '        strsql = "INSERT INTO BUYER_CATALOGUE_ITEMS (BCU_CAT_INDEX, BCU_PRODUCT_CODE, BCU_SOURCE, BCU_S_COY_ID, BCU_ENT_BY, BCU_ENT_DATETIME) "
        '        '        strsql &= "VALUES ((SELECT BCM_CAT_INDEX FROM BUYER_CATALOGUE_MSTR WHERE BCM_GRP_DESC = 'Default Purchaser Catalogue' AND BCM_B_COY_ID = '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "'), "
        '        '        strsql &= "'" & Common.Parse(dtItem.Rows(i)("ProductCode")) & "', '', '" & Common.Parse(dtItem.Rows(i)("CoyId")) & "', "
        '        '        strsql &= "'" & HttpContext.Current.Session("UserId") & "', getdate())"
        '        '        Common.Insert2Ary(strAryQuery, strsql)
        '        '    End If
        '        'End If
        '    Next

        '    If blnDelete Then
        '        If strAryQuery(0) <> String.Empty Then
        '            If objDb.BatchExecute(strAryQuery) Then
        '                UpdBuyerProductMstr = WheelMsgNum.Delete
        '            Else
        '                UpdBuyerProductMstr = WheelMsgNum.NotSave
        '            End If
        '        End If
        '    Else
        '        UpdBuyerProductMstr = WheelMsgNum.NotDelete
        '    End If
        'End Function
        'Public Function BuyerDisallowedMsg(ByVal strType As String, ByVal blnApproval As Boolean) As String
        '    Dim strMsg As String
        '    Select Case strType
        '        Case "L" ' list price
        '            strMsg = "List Price Catalogue"
        '        Case "C" ' contract catalogue
        '            strMsg = "Contract Catalogue"
        '        Case "D" ' discount catalogue
        '            strMsg = "Discount Catalogue"
        '    End Select

        '    If blnApproval Then
        '        BuyerDisallowedMsg = strMsg & " Approval can only be done for Vendor Company."
        '    Else
        '        BuyerDisallowedMsg = "Can only create " & strMsg & " for Vendor Company."
        '    End If

        'End Function

        'Public Function BuyerDisallowedMsg1(ByVal strType As String, ByVal blnApproval As Boolean) As String
        '    Dim strMsg As String
        '    Select Case strType
        '        Case "L" ' list price
        '            strMsg = "List Price Catalogue"
        '        Case "C" ' contract catalogue
        '            strMsg = "Contract Catalogue"
        '        Case "D" ' discount catalogue
        '            strMsg = "Discount Catalogue"
        '    End Select

        '    If blnApproval Then
        '        BuyerDisallowedMsg1 = strMsg & " Approval can only be done for Buyer Company."
        '    Else
        '        BuyerDisallowedMsg1 = "Can only create " & strMsg & " for Buyer Company."
        '    End If

        'End Function

        'Public Function getCategory() As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CM_CATEGORY_DESC, CM_CATEGORY_NAME, (" & objDb.Concat(" ", "", "CM_CATEGORY_NAME", "CM_CATEGORY_DESC") & ") AS CATEGORY_DESC "
        '    strsql &= "FROM CATEGORY_MSTR, CATEGORY_CATEGORY "
        '    strsql &= "WHERE CC_SUBCATEGORY_NAME = CM_CATEGORY_NAME "
        '    strsql &= "AND CC_CATEGORY_NAME = '[Root Category]' "
        '    strsql &= "ORDER BY CM_CATEGORY_NAME "
        '    ds = objDb.FillDs(strsql)
        '    getCategory = ds
        'End Function

        'Public Function getCategoryType(ByVal strType As String, ByVal strName As String, ByVal strDesc As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    Select Case strType
        '        Case "0"
        '            strsql = "SELECT CM_CATEGORY_DESC, CM_CATEGORY_NAME, CM_CATEGORY_TYPE, (" & objDb.Concat(" ", "", "CM_CATEGORY_NAME", "CM_CATEGORY_DESC") & ") AS CATEGORY_DESC "
        '            strsql &= "FROM CATEGORY_MSTR, CATEGORY_CATEGORY "
        '            strsql &= "WHERE CC_SUBCATEGORY_NAME = CM_CATEGORY_NAME "
        '            'strsql &= "AND CC_CATEGORY_NAME = '" & strName & "' "
        '            strsql &= "AND CC_CATEGORY_NAME " & Common.ParseSQL(strName) & " "
        '            strsql &= "ORDER BY CC_CATEGORY_NAME"
        '        Case "1"
        '            strsql = "SELECT M1.CM_CATEGORY_DESC, M1.CM_CATEGORY_NAME, M1.CM_CATEGORY_TYPE, "
        '            strsql &= "(" & objDb.Concat(" ", "", "M1.CM_CATEGORY_NAME", "M1.CM_CATEGORY_DESC") & " AS CATEGORY_DESC "
        '            strsql &= "FROM (SELECT * FROM CATEGORY_MSTR "
        '            ' ai chu modified on 06/09/2005 - search criteria should follow standard
        '            'strsql &= "WHERE CM_CATEGORY_NAME LIKE '" & Common.Parse(strName) & "%' "
        '            'strsql &= "AND CM_CATEGORY_DESC LIKE '" & Common.Parse(strDesc) & "%' "
        '            If strName = "" Then
        '                strsql &= "WHERE CM_CATEGORY_NAME LIKE '" & Common.Parse(strName) & "%' "
        '            Else
        '                strsql &= "WHERE CM_CATEGORY_NAME " & Common.ParseSQL(strName) & " "
        '            End If

        '            If strDesc = "" Then
        '                strsql &= "AND CM_CATEGORY_DESC LIKE '" & Common.Parse(strDesc) & "%' "
        '            Else
        '                strsql &= "AND CM_CATEGORY_DESC " & Common.ParseSQL(strDesc) & " "
        '            End If

        '            strsql &= "AND CM_UNSPSC_TYPE IS NOT NULL) M2, "
        '            strsql &= "CATEGORY_MSTR M1 WHERE (" & objDb.Concat("", "", "(M1.CM_CATEGORY_NAME = LEFT(M2.CM_CATEGORY_NAME,2)", "000000")
        '            strsql &= " OR " & objDb.Concat("", "", "M1.CM_CATEGORY_NAME = LEFT(M2.CM_CATEGORY_NAME,4)", "0000")
        '            strsql &= " OR " & objDb.Concat("", "", "M1.CM_CATEGORY_NAME = LEFT(M2.CM_CATEGORY_NAME,6)", "00")
        '            strsql &= " OR M1.CM_CATEGORY_NAME = M2.CM_CATEGORY_NAME) AND M1.CM_STATUS = 'A' "
        '            strsql &= "AND M1.CM_UNSPSC_TYPE IS NOT NULL "
        '            strsql &= "GROUP BY M1.CM_CATEGORY_NAME, M1.CM_CATEGORY_DESC, M1.CM_CATEGORY_TYPE, M1.CM_UNSPSC_TYPE "
        '            strsql &= "ORDER BY M1.CM_CATEGORY_NAME "
        '    End Select
        '    ds = objDb.FillDs(strsql)
        '    getCategoryType = ds
        'End Function

        'Public Function Download_CatalogueExcel(ByVal intIndex As Integer) As DataSet
        '    Dim dload As New DataSet
        '    Dim strSQL As String

        '    strSQL = "SELECT CDI_VENDOR_ITEM_CODE, IFNULL(CDI_PRODUCT_DESC,''), IFNULL(CDI_UOM,''), " _
        '        & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY = 'CU' AND CODE_ABBR = CDI_CURRENCY_CODE AND CODE_DELETED='N') AS CDI_CURRENCY_CODE," _
        '        & "CDI_UNIT_COST, " _
        '        & "IF((TAX_PERC IS NULL OR TAX_PERC = ''), IFNULL(CODE_DESC,''), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST_RATE, " _
        '        & "CASE WHEN (CDI_GST_TAX_CODE IS NULL OR CDI_GST_TAX_CODE = 'N/A') THEN '' ELSE CDI_GST_TAX_CODE END AS CDI_GST_TAX_CODE, " _
        '        & "IFNULL(CDI_REMARK ,'') AS CDI_REMARK " _
        '        & "FROM CONTRACT_DIST_ITEMS " _
        '        & "INNER JOIN CONTRACT_DIST_MSTR ON CDM_GROUP_INDEX = CDI_GROUP_INDEX " _
        '        & "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " _
        '        & "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = CDI_GST_RATE " _
        '        & "LEFT JOIN TAX ON TAX_CODE = CDI_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " _
        '        & "WHERE CDI_GROUP_INDEX =" & intIndex

        '    dload = objDb.FillDs(strSQL)
        '    Download_CatalogueExcel = dload
        'End Function

        'Public Function Write_CatalogueExcel(ByVal intIndex As Integer) As DataSet
        '    Dim dload As New DataSet
        '    Dim strSQL As String

        '    strSQL = "SELECT CM_COY_NAME,CDM_GROUP_CODE,CDM_GROUP_DESC,CDM_START_DATE,CDM_END_DATE " _
        '            & "FROM CONTRACT_DIST_MSTR " _
        '            & "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID " _
        '            & "AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' " _
        '            & "WHERE CDM_GROUP_INDEX = " & intIndex

        '    dload = objDb.FillDs(strSQL)
        '    Write_CatalogueExcel = dload
        'End Function

        Public Sub FillGLCode(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView

            strSql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC FROM company_b_gl_code " _
                   & "WHERE cbg_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"

            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CBG_B_GL_CODE", "CBG_B_GL_CODE", drw)
                lstItem.Value = ""
                lstItem.Text = "N/A"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Text = "N/A"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            'objDb = Nothing
        End Sub
        Public Function FillGLCode_data(ByVal CompanyId As String, Optional ByVal pDropDownList As String = "", Optional ByVal strConn As String = "") As String
            Dim strSql As String
            'Dim strDefaultValue As String

            'strSql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC FROM company_b_gl_code " _
            '       & "WHERE cbg_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "'"
            strSql = "SELECT CBG_B_GL_CODE, CBG_B_GL_DESC FROM company_b_gl_code " _
                   & "WHERE cbg_b_coy_id = '" & CompanyId & "'"
            'pDropDownList = objDb.GetView(strSql)
            Return strSql

            'Dim lstItem As New ListItem
            'If Not pDropDownList Is Nothing Then
            '    Common.FillDdl_(pDropDownList, "CBG_B_GL_CODE", "CBG_B_GL_CODE", pDropDownList)
            '    lstItem.Value = ""
            '    lstItem.Text = "N/A"
            '    strDefaultValue = lstItem.Text

            'Else
            '    '//no suppose to happen
            '    lstItem.Text = "N/A"
            '    pDropDownList.Items.Insert(0, lstItem)
            'End If
            ''objDb = Nothing
        End Function

        Public Sub FillCatCode(ByRef pDropDownList As UI.WebControls.DropDownList, Optional ByVal strConn As String = "")
            Dim strDefaultValue As String
            Dim strSql As String
            Dim drw As DataView
            'Dim objDb As New EAD.DBCom

            strSql = "SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE " _
              & "WHERE CBC_B_COY_ID  = '" & HttpContext.Current.Session("CompanyId") & "'"


            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CBC_B_CATEGORY_CODE", "CBC_B_CATEGORY_CODE", drw)
                lstItem.Value = ""
                lstItem.Text = "N/A"
                strDefaultValue = lstItem.Text
                pDropDownList.Items.Insert(0, lstItem)
            Else
                '//no suppose to happen
                lstItem.Value = ""
                lstItem.Text = "N/A"
                pDropDownList.Items.Insert(0, lstItem)
            End If
            'objDb = Nothing
        End Sub
        Public Function FillCatCode_data(ByVal pDropDownList As DataView, Optional ByVal strConn As String = "")
            Dim strSql As String
            strSql = "SELECT CBC_B_CATEGORY_CODE FROM COMPANY_B_CATEGORY_CODE " _
              & "WHERE CBC_B_COY_ID  = '" & HttpContext.Current.Session("CompanyId") & "'"
            pDropDownList = objDb.GetView(strSql)
        End Function

        'Function bindlistbox_ContCatSearchData(ByVal strCatName As String) As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom
        '    'Dim SSOdbName As String
        '    'SSOdbName = ConfigurationManager.AppSettings.Get("SSOdbName")
        '    'SSOdbName = SSOdbName & "."

        '    '"SELECT C.CDM_DEPT_NAME,U.UM_USER_ID, U.UM_USER_NAME, " & _
        '    '"(C.CDM_DEPT_NAME + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
        '    strSql = "SELECT  distinct U.UM_USER_ID, U.UM_USER_NAME, " &
        '             objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " &
        '             "FROM USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM " &
        '             "where UM_USER_ID NOT IN (SELECT K.CDU_USER_ID from CONTRACT_DIST_MSTR B ,contract_dist_user K " &
        '             "where B.CDM_GROUP_INDEX = k.CDU_GROUP_INDEX AND B.CDM_GROUP_INDEX = '" & strCatName & "' ) " &
        '             "AND G.UU_USER_ID = U.UM_USER_ID AND U.UM_DELETED <> 'Y' AND U.UM_STATUS='A' " &
        '             "AND G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND UM_COY_ID=UU_COY_ID " &
        '             "AND (GM.UGM_FIXED_ROLE ='Buyer' OR GM.UGM_FIXED_ROLE= 'Purchasing Manager' OR GM.UGM_FIXED_ROLE= 'Purchasing Officer') " &
        '             "AND UM_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' ORDER BY U.UM_USER_ID"

        '    '"FROM USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM,COMPANY_DEPT_MSTR C " & _

        '    '"AND C.CDM_DEPT_CODE=U.UM_DEPT_ID " & _
        '    '"AND U.UM_COY_ID = C.CDM_COY_ID"
        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Function bindlistbox_SearchDataPur() As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom
        '    'Dim SSOdbName As String
        '    'SSOdbName = ConfigurationManager.AppSettings.Get("SSOdbName")
        '    'SSOdbName = SSOdbName & "."

        '    '"SELECT C.CDM_DEPT_NAME,U.UM_USER_ID, U.UM_USER_NAME, " & _
        '    '"(C.CDM_DEPT_NAME + ' : ' + U.UM_USER_ID + ' : ' + U.UM_USER_NAME) as three " & _
        '    strSql = "SELECT  distinct U.UM_USER_ID, U.UM_USER_NAME, " &
        '             objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " &
        '             "FROM USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM " &
        '             "where G.UU_USER_ID = U.UM_USER_ID AND U.UM_DELETED <> 'Y' AND U.UM_STATUS='A' " &
        '             "AND G.UU_USRGRP_ID = GM.UGM_USRGRP_ID AND UM_COY_ID=UU_COY_ID " &
        '             "AND (GM.UGM_FIXED_ROLE ='Buyer' OR GM.UGM_FIXED_ROLE= 'Purchasing Manager' OR GM.UGM_FIXED_ROLE= 'Purchasing Officer') " &
        '             "AND UM_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' ORDER BY U.UM_USER_ID"

        '    '"FROM USER_MSTR U, USERS_USRGRP G, USER_GROUP_MSTR GM,COMPANY_DEPT_MSTR C " & _

        '    '"AND C.CDM_DEPT_CODE=U.UM_DEPT_ID " & _
        '    '"AND U.UM_COY_ID = C.CDM_COY_ID"
        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Function bindlistbox_BuyerSearchData(ByVal strBuyerName As String) As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom

        '    strSql = "SELECT DISTINCT CDM_GROUP_INDEX, CDM_GROUP_CODE " &
        '            "FROM CONTRACT_DIST_MSTR " &
        '            "LEFT JOIN CONTRACT_DIST_USER ON CDM_GROUP_INDEX = CDU_GROUP_INDEX " &
        '            "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
        '            "WHERE CDC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CDM_TYPE = 'C' " &
        '            "AND CDM_GROUP_CODE NOT IN ( " &
        '            "SELECT DISTINCT CDM_GROUP_CODE FROM CONTRACT_DIST_MSTR " &
        '            "INNER JOIN CONTRACT_DIST_USER ON CDM_GROUP_INDEX = CDU_GROUP_INDEX " &
        '            "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
        '            "WHERE CDC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CDM_TYPE = 'C' " &
        '            "AND CDU_USER_ID = '" & strBuyerName & "' ORDER BY CDM_GROUP_CODE) ORDER BY CDM_GROUP_CODE "

        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Function CheckContCatAsgBuyer(ByVal strCat As String, ByVal strUser As String) As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom

        '    strSql = "SELECT DISTINCT CDM_GROUP_INDEX, CDM_GROUP_CODE as three " &
        '            "FROM CONTRACT_DIST_MSTR " &
        '            "LEFT JOIN CONTRACT_DIST_USER ON CDM_GROUP_INDEX = CDU_GROUP_INDEX " &
        '            "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
        '            "WHERE CDC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CDM_TYPE = 'C' " &
        '            "AND CDU_USER_ID = '" & strUser & "' AND CDU_GROUP_INDEX = '" & strCat & "' ORDER BY CDM_GROUP_CODE "

        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Function bindlistbox_SearchDataCat() As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom

        '    strSql = "SELECT DISTINCT CDM_GROUP_INDEX, CDM_GROUP_CODE as three " &
        '            "FROM CONTRACT_DIST_MSTR " &
        '            "LEFT JOIN CONTRACT_DIST_USER ON CDM_GROUP_INDEX = CDU_GROUP_INDEX " &
        '            "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
        '            "WHERE CDC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CDM_TYPE = 'C' " &
        '            "ORDER BY CDM_GROUP_CODE "

        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Function bindlistbox_ContCatSelectedData(ByVal strCatName As String) As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom
        '    Dim SSOdbName As String
        '    SSOdbName = ConfigurationManager.AppSettings.Get("SSOdbName")
        '    SSOdbName = SSOdbName & "."

        '    strSql = "SELECT distinct U.UM_USER_ID, U.UM_USER_NAME, " &
        '                 objDB.Concat(" : ", "", "U.UM_USER_ID", "U.UM_USER_NAME") & " as three " &
        '                 "FROM CONTRACT_DIST_USER K, " & SSOdbName & "USER_MSTR U " &
        '                 "WHERE k.CDU_GROUP_INDEX = '" & strCatName & "' " &
        '                 "AND K.CDU_USER_ID = U.UM_USER_ID AND U.UM_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' " &
        '                 "AND U.UM_DELETED <> 'Y' AND U.UM_STATUS='A'"

        '    '"FROM CONTRACT_DIST_USER K, USER_MSTR U,COMPANY_DEPT_MSTR C " & _

        '    strSql &= " ORDER BY U.UM_USER_ID"

        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Function bindlistbox_BuyerSelectedData(ByVal strBuyerName As String) As DataView
        '    Dim strSql As String
        '    Dim drw As DataView
        '    Dim objDB As New EAD.DBCom

        '    strSql = "SELECT DISTINCT CDM_GROUP_INDEX, CDM_GROUP_CODE " &
        '        "FROM CONTRACT_DIST_MSTR " &
        '        "INNER JOIN CONTRACT_DIST_USER ON CDM_GROUP_INDEX = CDU_GROUP_INDEX " &
        '        "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
        '        "WHERE CDC_B_COY_ID='" & HttpContext.Current.Current.Session("CompanyID") & "' AND CDM_TYPE = 'C' " &
        '        "AND CDU_USER_ID = '" & strBuyerName & "' ORDER BY CDM_GROUP_CODE "

        '    drw = objDB.GetView(strSql)
        '    Return drw
        'End Function

        'Function DelContcatAsg(ByVal strCatName As String)
        '    Dim strSQL As String
        '    Dim query(0) As String
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    strSQL = "Delete FROM CONTRACT_DIST_USER where CDU_GROUP_INDEX='" & strCatName & "'"
        '    Common.Insert2Ary(query, strSQL)

        '    'Modified by Joon on 1st Aug 2011
        '    'Insert into audit trail au_contract_items - delete
        '    strsql1 = "SELECT * FROM CONTRACT_DIST_USER where CDU_GROUP_INDEX='" & strCatName & "'"
        '    dtTemp = objDb.FillDt(strsql1)
        '    For j As Integer = 0 To dtTemp.Rows.Count - 1
        '        InsertAuditTrail(strCatName, "User ID", dtTemp.Rows(j)("CDU_USER_ID"), "", "U", "D", query)
        '        'strSQL = "INSERT INTO au_contract "
        '        'strSQL &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '        'strSQL &= "'" & strCatName & "',"
        '        'strSQL &= "'" & Common.Parse(dtTemp.Rows(j)("CDU_USER_ID")) & "', "
        '        'strSQL &= "'U', "
        '        'strSQL &= "'D', "
        '        'strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '        'strSQL &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '        'strSQL &= Common.ConvertDate(Now()) & ")"
        '        'Common.Insert2Ary(query, strSQL)
        '    Next

        '    If query(0) <> String.Empty Then
        '        objDb.BatchExecute(query)
        '    End If

        'End Function

        'Function DelBuyerAsg(ByVal strBuyerName As String)
        '    Dim strSQL As String
        '    Dim query(0) As String
        '    Dim strsql1 As String
        '    Dim dtTemp As New DataTable

        '    strSQL = " DELETE FROM CONTRACT_DIST_USER " &
        '            " USING CONTRACT_DIST_USER " &
        '            " INNER JOIN CONTRACT_DIST_MSTR ON CDM_GROUP_INDEX = CDU_GROUP_INDEX " &
        '            " LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
        '            " WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CDM_TYPE = 'C' " &
        '            " AND CDU_USER_ID = '" & strBuyerName & "' "

        '    Common.Insert2Ary(query, strSQL)

        '    strsql1 = " SELECT * FROM CONTRACT_DIST_USER " &
        '            " INNER JOIN CONTRACT_DIST_MSTR ON CDM_GROUP_INDEX = CDU_GROUP_INDEX " &
        '            " LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " &
        '            " WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND CDM_TYPE = 'C' " &
        '            " AND CDU_USER_ID = '" & strBuyerName & "' "

        '    dtTemp = objDb.FillDt(strsql1)
        '    For j As Integer = 0 To dtTemp.Rows.Count - 1
        '        InsertAuditTrail(dtTemp.Rows(j)("CDU_GROUP_INDEX"), "User ID", dtTemp.Rows(j)("CDU_USER_ID"), "", "U", "D", query)
        '    Next

        '    If query(0) <> String.Empty Then
        '        objDb.BatchExecute(query)
        '    End If

        'End Function

        'Function AddContCatAsgBuyer(ByVal strCatName As String, ByVal strli As String)
        '    Dim strSQL As String
        '    Dim query(0) As String
        '    strSQL = "INSERT INTO CONTRACT_DIST_USER (CDU_GROUP_INDEX, CDU_USER_ID )VALUES('" & strCatName & "','" & strli & "')"
        '    Common.Insert2Ary(query, strSQL)

        '    'Modified by Joon on 1st Aug 2011
        '    'Insert into audit trail au_contract_items 
        '    InsertAuditTrail(strCatName, "User ID", "", strli, "U", "A", query)
        '    'strSQL = "INSERT INTO au_contract "
        '    'strSQL &= "(AU_GROUP_INDEX,AU_DESC,AU_BEFORE,AU_AFTER,AU_MODULE,AU_ACTION,AU_USER,AU_COY,AU_DATE) VALUES ("
        '    'strSQL &= "'" & strCatName & "',"
        '    'strSQL &= "'" & Common.Parse(strli) & "', "
        '    'strSQL &= "'U', "
        '    'strSQL &= "'A', "
        '    'strSQL &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
        '    'strSQL &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
        '    'strSQL &= Common.ConvertDate(Now()) & ")"
        '    'Common.Insert2Ary(query, strSQL)

        '    objDb.BatchExecute(query)
        'End Function

        'Function AddAsgBuyer(ByVal strBuyerName As String, ByVal strli As String)
        '    Dim strSQL As String
        '    Dim query(0) As String

        '    strSQL = "INSERT INTO CONTRACT_DIST_USER (CDU_GROUP_INDEX, CDU_USER_ID )VALUES(" & strli & ",'" & strBuyerName & "')"
        '    Common.Insert2Ary(query, strSQL)

        '    InsertAuditTrail(strli, "User ID", "", strBuyerName, "U", "A", query)

        '    objDb.BatchExecute(query)
        'End Function

        'Public Function getConRefNoByUser() As DataSet
        '    'Added by Joon on 7th June 2011
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT CDM_GROUP_INDEX, CDM_GROUP_CODE " _
        '        & "FROM CONTRACT_DIST_MSTR " _
        '        & "LEFT JOIN CONTRACT_DIST_COY ON CDC_GROUP_INDEX = CDM_GROUP_INDEX " _
        '        & "LEFT JOIN contract_dist_user ON cdu_group_index = CDM_GROUP_INDEX " _
        '        & "WHERE CDC_B_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyID")) & "' " _
        '        & "AND CDM_TYPE = 'C' and cdu_user_id = '" & Common.Parse(HttpContext.Current.Session("UserID")) & "' "
        '    ds = objDb.FillDs(strsql)
        '    getConRefNoByUser = ds
        'End Function

        Public Function InsertAuditTrailBIM(ByVal intRefID As Integer, ByVal strModule As String, ByVal strAction As String, ByVal strFieldName As String, ByVal strBefore As String, ByVal strAfter As String, ByRef pQuery() As String, Optional ByVal blnGetLastIndex As Boolean = False)
            Dim strsql As String

            If blnGetLastIndex = True Then
                strsql = "INSERT INTO au_product_log "
                strsql &= "(aup_refer_ID,aup_module,aup_action,aup_fieldName,aup_old_value,aup_new_value,aup_enterby,aup_coy_id, aup_changed_date) VALUES ("
                strsql &= "(SELECT ISNULL(MAX(PM_PRODUCT_INDEX),1) FROM PRODUCT_MSTR),"
                strsql &= "'" & strModule & "', "
                strsql &= "'" & strAction & "', "
                strsql &= "'" & strFieldName & "', "
                strsql &= "'" & strBefore & "', "
                strsql &= "'" & strAfter & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
                strsql &= Common.ConvertDate(Now()) & ")"
            Else
                strsql = "INSERT INTO au_product_log "
                strsql &= "(aup_refer_ID,aup_module,aup_action,aup_fieldName,aup_old_value,aup_new_value,aup_enterby,aup_coy_id,aup_changed_date) VALUES ("
                strsql &= intRefID & ","
                strsql &= "'" & strModule & "', "
                strsql &= "'" & strAction & "', "
                strsql &= "'" & strFieldName & "', "
                strsql &= "'" & Common.Parse(strBefore) & "', "
                strsql &= "'" & Common.Parse(strAfter) & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("UserId")) & "', "
                strsql &= "'" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "', "
                strsql &= Common.ConvertDate(Now()) & ")"
            End If
            Common.Insert2Ary(pQuery, strsql)
            'objDb.Execute(strsql)
        End Function
    End Class
End Namespace