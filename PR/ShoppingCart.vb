Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls

Namespace AgoraNEO
    Public Class ShoppingCart
        Dim objDb As New EAD.DBCom

        Public Sub FillSCVendor(ByRef pDropDownList As UI.WebControls.DropDownList)
            Dim strSql As String
            Dim drw As DataView
            strSql = "SELECT DISTINCT SC_S_COY_ID, CM_COY_NAME  "
            strSql &= "FROM SHOPPING_CART "
            strSql &= "LEFT JOIN COMPANY_MSTR ON SC_S_COY_ID = CM_COY_ID "
            strSql &= "WHERE SC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "AND SC_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            drw = objDb.GetView(strSql)

            pDropDownList.Items.Clear()
            Dim lstItem As New ListItem
            If Not drw Is Nothing Then
                Common.FillDdl(pDropDownList, "CM_COY_NAME", "SC_S_COY_ID", drw)
            End If

            ' Add ---Select---
            lstItem.Value = ""
            ' Dim blnMultipleSupplier As Boolean = True
            '---------------New Code Added for CM_MULTI_PO on 04/July/2007  Start------------
            Dim blnMultipleSupplier As Boolean
            Dim Strmulti As String
            Dim strSql1 As String
            Dim dd As New System.Web.UI.WebControls.DropDownList
            Dim dds As DataTable
            strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            dds = objDb.FillDt(strSql1)
            Common.FillDdl(dd, "CM_MULTI_PO", "CM_MULTI_PO", dds)
            dd.DataBind()
            If dd.SelectedItem.Value = "Y" Then
                blnMultipleSupplier = True
            Else
                blnMultipleSupplier = False
            End If
            '---------------New Code Added for CM_MULTI_PO on 04/July/2007  End------------ 
            If blnMultipleSupplier Then
                lstItem.Text = "---Merged PR---"
            Else
                lstItem.Text = "---Select---"
            End If

            pDropDownList.Items.Insert(0, lstItem)
        End Sub

        Public Function PopulateShoppingCart(ByVal strVendor As String) As DataSet
            Dim strUser As String
            Dim strSql As String
            Dim dsCart As DataSet
            strSql = "SELECT SC_CART_INDEX, CBC_B_ITEM_CODE, SC_B_COY_ID, SC_USER_ID, SC_S_COY_ID, SC_PRODUCT_CODE, "
            strSql &= "SC_VENDOR_ITEM_CODE, SC_PRODUCT_DESC, SC_UNIT_COST, SC_QUANTITY, (SC_UNIT_COST * SC_QUANTITY) AS TOTAL, "
            strSql &= "SC_CURRENCY_CODE, SC_UOM, SC_REMARK, C.CM_COY_NAME, D.CODE_VALUE AS GST " ', B.CODE_DESC
            strSql &= "FROM SHOPPING_CART "
            strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON SC_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "LEFT JOIN PRODUCT_MSTR ON PM_PRODUCT_CODE = SC_PRODUCT_CODE AND PM_DELETED <> 'Y' "
            strSql &= "INNER JOIN COMPANY_MSTR AS C ON SC_S_COY_ID = C.CM_COY_ID "
            'strSql &= "LEFT JOIN CODE_MSTR AS B ON SC_CURRENCY_CODE = B.CODE_ABBR "
            strSql &= "LEFT JOIN CODE_MSTR AS D ON PM_GST_CODE = D.CODE_ABBR "
            strSql &= "WHERE SC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            strSql &= "AND SC_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            strSql &= "AND D.CODE_CATEGORY = 'SST' "

            'Dim blnMultipleSupplier As Boolean = True

            If strVendor <> "" Then
                strSql &= "AND SC_S_COY_ID = '" & Common.Parse(strVendor) & "' "
            End If

            dsCart = objDb.FillDs(strSql)
            PopulateShoppingCart = dsCart
        End Function

        Public Function delShoppingCartItem(ByVal strIndex As String) As String
            Dim strDel As String
            strDel = "DELETE FROM SHOPPING_CART WHERE SC_CART_INDEX = '" & strIndex & "'; "
            delShoppingCartItem = strDel
        End Function

        Public Function getModShoppingCartSQL(ByVal strIndex As String, ByVal dblUnitPrice As Double, ByVal intQty As Integer,
                                        ByVal strRemark As String) As String
            Dim strCart As String

            strCart = "UPDATE SHOPPING_CART SET "
            strCart &= "SC_UNIT_COST = " & dblUnitPrice & ", "
            strCart &= "SC_QUANTITY = " & IIf(intQty = -1, "NULL", intQty) & ", "
            strCart &= "SC_REMARK = '" & Common.Parse(strRemark) & "' "
            strCart &= "WHERE SC_CART_INDEX = '" & strIndex & "' "
            getModShoppingCartSQL = strCart
        End Function

        Public Function ProductCodeAlreadyExist(ByVal sPONo As String, ByVal sProductCode As String, ByVal sTable As String) As Boolean
            Dim tSQL As String
            If sTable = "PO" Then
                tSQL = "SELECT POD_PO_NO FROM PO_DETAILS WHERE POD_PO_NO = '" & sPONo & "' AND POD_PRODUCT_CODE = '" & sProductCode & "' AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            ElseIf sTable = "RFQ" Then
                'tSQL = "SELECT POD_PO_NO FROM RFQ_REPLIES_DETAIL WHERE RRD_RFQ_ID = '" & sPONo & "' AND RRD_Product_Code = '" & sProductCode & "' AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
                tSQL = "SELECT RRD_V_Coy_Id FROM RFQ_REPLIES_DETAIL LEFT JOIN COMPANY_B_ITEM_CODE ON RRD_Product_Code = CBC_PRODUCT_CODE AND CBC_B_COY_ID =  '" & HttpContext.Current.Session("CompanyId") & "' WHERE RRD_RFQ_ID ='" & sPONo & "' AND RRD_Product_Code = '" & sProductCode & "'"
            End If

            Dim tDS As DataSet = objDb.FillDs(tSQL)
            If tDS.Tables(0).Rows.Count > 0 Then
                ProductCodeAlreadyExist = True
            Else
                ProductCodeAlreadyExist = False
            End If
            tDS = Nothing
        End Function

        Public Function RemoveProductCodeFromList(ByVal sProductCode As String, ByVal aryProdCode As ArrayList) As ArrayList
            aryProdCode.Remove(sProductCode)
            RemoveProductCodeFromList = aryProdCode
        End Function

        Public Function RemovePRProductCodeFromList(ByVal sProductCode As String, ByVal aryProdCode As ArrayList, ByVal sProductIndex As Integer, Optional ByVal sProductDesc As String = "", Optional ByRef InternalCount As Integer = 0) As ArrayList
            Dim i As Integer
            If sProductCode = "" Then
                For i = 0 To aryProdCode.Count - 1
                    If aryProdCode(i)(1) = sProductDesc Then
                        aryProdCode.RemoveAt(i)
                        RemovePRProductCodeFromList = aryProdCode
                        Exit For
                    End If
                Next
            Else
                For i = 0 To aryProdCode.Count - 1
                    If aryProdCode(i)(0) = sProductCode AndAlso i = sProductIndex - InternalCount Then
                        aryProdCode.RemoveAt(i)
                        RemovePRProductCodeFromList = aryProdCode
                        InternalCount = InternalCount + 1
                        Exit For
                    End If
                Next
                'aryProdCode.RemoveAt(sProductIndex)
            End If
            RemovePRProductCodeFromList = aryProdCode
        End Function

        Public Function getPRItemList(ByVal companyId As String, ByVal strType As String, ByVal strIndex As String, ByVal strPRNo As String, Optional ByVal vcomid As String = "", Optional ByVal LineNumber As ArrayList = Nothing, Optional ByVal strDesc As String = "", Optional ByVal strUOM As String = "", Optional ByVal strComm As String = "", Optional ByVal strCatGroup As String = "", Optional getStrSQL As Boolean = False) As Result
            Dim strSql As String = String.Empty
            Dim ds As DataSet = New DataSet
            Select Case strType
                Case "BuyerCat"
                    If strIndex <> "" And strIndex <> "&nbsp;" Then
                        strSql = "SELECT CURDATE() AS EST, PM_PRODUCT_CODE AS PRODUCTCODE, PM_LAST_TXN_PRICE AS UNITCOST, PM_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "PM_UOM AS UOM, 'BC' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        'strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, IFNULL(PM_LAST_TXN_TAX,0) AS GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, PM_LAST_TXN_S_COY_ID AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO "
                        ' yAP: '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE For FTN
                        strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, '' AS GSTRateDesc, '' AS GSTRate, 0.00 AS GST, 0.00 AS CDI_GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, PM_LAST_TXN_S_COY_ID AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE', "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "'' AS GstTaxCode "

                        'Jules 2018.05.07 - PAMB Scrum 2
                        If Common.Parse(companyId).ToString.ToUpper = "PAMB" Then
                            strSql &= ", 'N' AS GIFT, 'ITNP' AS FUNDTYPE, '' AS PERSONCODE, '' AS PROJECTCODE, "
                            strSql &= "(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_ANALYSIS_CODE = 'ITNP') AS FUNDTYPEDESC, '' AS PERSONCODEDESC, '' AS PROJECTCODEDESC, "
                            strSql &= "IF(PM_ACCT_CODE='','',(SELECT CONCAT(CBG_B_GL_DESC,' (',CBG_B_GL_CODE,')') AS PM_ACCT_CODE  FROM COMPANY_B_GL_CODE "
                            strSql &= "WHERE CBG_B_COY_ID = PM_S_COY_ID AND CBG_B_GL_CODE = PM_ACCT_CODE AND CBG_STATUS='A' )) AS GLDESCRIPTION "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS FUNDTYPE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PERSONCODE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PROJECTCODE "
                        End If
                        'End modification.

                        strSql &= "FROM PRODUCT_MSTR LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & Common.Parse(companyId) & "' "
                        strSql &= "WHERE PM_PRODUCT_CODE IN (" & strIndex & ") AND PM_S_COY_ID = '" & Common.Parse(companyId) & "' AND PM_DELETED <> 'Y' "
                    Else
                        strSql = "SELECT DATE_FORMAT(CURDATE(), ""%d/%m/%Y"") AS EST, '' AS PRODUCTCODE, 0.00 AS UNITCOST, '" & Common.Parse(strDesc) & "' AS PRODUCTDESC, '' AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "'" & strUOM & "' AS UOM, 'FF' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        'strSql &= "'' AS CATEGORYCODE, '" & Common.Parse(strComm) & "' AS COMMODITY, '' AS GLCODE, '' AS TAXCODE, 0.00 AS GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, '' AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO "
                        ' yAP: '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE For FTN

                        'Jules 2018.11.03 - Set currency to MYR
                        'strSql &= "'' AS CATEGORYCODE, '" & Common.Parse(strComm) & "' AS COMMODITY, '' AS GLCODE, '' AS TAXCODE, '' AS GSTRateDesc, '' AS GSTRate, 0.00 AS GST, 0.00 AS CDI_GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PM_LAST_TXN_PRICE_CURR AS CURRENCY, '' AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE', "
                        strSql &= "'' AS CATEGORYCODE, '" & Common.Parse(strComm) & "' AS COMMODITY, '' AS GLCODE, '' AS TAXCODE, '' AS GSTRateDesc, '' AS GSTRate, 0.00 AS GST, 0.00 AS CDI_GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, 'MYR' AS CURRENCY, '' AS VENDOR, '' AS ACCT, '' AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_GST_RATE, 0.00 as 'POD_TAX_VALUE', "

                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "'' AS GstTaxCode "

                        'Jules 2018.05.07 - PAMB Scrum 2
                        If Common.Parse(companyId).ToString.ToUpper = "PAMB" Then
                            strSql &= ", 'N' AS GIFT, 'ITNP' AS FUNDTYPE, '' AS PERSONCODE, '' AS PROJECTCODE, "
                            strSql &= "(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_ANALYSIS_CODE = 'ITNP' and ac_b_coy_id = '" & Common.Parse(companyId) & "') AS FUNDTYPEDESC, '' AS PERSONCODEDESC, '' AS PROJECTCODEDESC, '' AS GLDESCRIPTION "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS FUNDTYPE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PERSONCODE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PROJECTCODE "
                        End If
                        'End modification.

                        'strSql &= "FROM PRODUCT_MSTR LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE LIMIT 1 " 'Jules commented 2018.11.03 - Should not be reliant on past data.
                    End If

                Case "ConCat"
                    If strIndex <> "" And strIndex <> "&nbsp;" Then
                        strSql = "SELECT DATE_FORMAT(CURDATE(), ""%d/%m/%Y"") AS EST, PM_PRODUCT_CODE AS PRODUCTCODE, CDI_UNIT_COST AS UNITCOST, CDI_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "CDI_UOM AS UOM, 'CP' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, "
                        'strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, IFNULL(CDI_GST,0) AS GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO "

                        strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, "

                        ' GST Rate Desc
                        strSql &= "IFNULL((SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 'N/A') AS GSTRateDesc, "
                        ' GST Rate Code
                        strSql &= "IFNULL((SELECT CODE_ABBR "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 'N/A') AS GSTRate, "
                        ' GST Perc
                        strSql &= "IFNULL((SELECT CAST(IF(TAX_PERC='', 0, TAX_PERC) AS DECIMAL) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 0) AS GST, CDI_GST AS CDI_GST, "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "CDI_GST_TAX_CODE AS GstTaxCode, "
                        strSql &= "'' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO, 0.00 as 'POD_TAX_VALUE' "

                        'Jules 2018.05.07 - PAMB Scrum 2
                        If Common.Parse(companyId).ToString.ToUpper = "PAMB" Then
                            strSql &= ", 'N' AS GIFT, 'ITNP' AS FUNDTYPE, '' AS PERSONCODE, '' AS PROJECTCODE, "
                            strSql &= "(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_ANALYSIS_CODE = 'ITNP') AS FUNDTYPEDESC, '' AS PERSONCODEDESC, '' AS PROJECTCODEDESC, "
                            strSql &= "IF(PM_ACCT_CODE='','',(SELECT CONCAT(CBG_B_GL_DESC,' (',CBG_B_GL_CODE,')') AS PM_ACCT_CODE  FROM COMPANY_B_GL_CODE "
                            strSql &= "WHERE  CBG_B_COY_ID = PM_S_COY_ID AND CBG_B_GL_CODE = PM_ACCT_CODE AND CBG_STATUS='A' )) AS GLDESCRIPTION "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS FUNDTYPE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PERSONCODE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PROJECTCODE "
                        End If
                        'End modification.

                        strSql &= "FROM CONTRACT_DIST_COY "
                        strSql &= "INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
                        strSql &= "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
                        strSql &= "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX "
                        strSql &= "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "WHERE CDC_B_COY_ID = '" & companyId & "' "
                        strSql &= "AND CDM_GROUP_INDEX = '" & strCatGroup & "' "
                        strSql &= "AND PM_PRODUCT_CODE IN (" & strIndex & ") AND PM_DELETED <> 'Y' "
                    Else
                        strSql = "SELECT DATE_FORMAT(CURDATE(), ""%d/%m/%Y"") AS EST, PM_PRODUCT_CODE AS PRODUCTCODE, CDI_UNIT_COST AS UNITCOST, CDI_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, CAST(500 AS SIGNED) AS ITEMLINE, "
                        strSql &= "CDI_UOM AS UOM, 'CP' AS SOURCE, 0.00 AS PR_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                        strSql &= "PM_CAT_CODE AS CATEGORYCODE, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PRODUCT_MSTR.PM_CATEGORY_NAME) AS COMMODITY, "
                        'strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, IFNULL(CDI_GST,0) AS GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, , '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO "
                        strSql &= "PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, "

                        ' GST Rate Desc
                        strSql &= "IFNULL((SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), '') AS GSTRateDesc, "
                        ' GST Rate Code
                        strSql &= "IFNULL((SELECT CODE_ABBR "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), '') AS GSTRate, "
                        ' GST Perc
                        strSql &= "IFNULL((SELECT IF(TAX_PERC='', 0, TAX_PERC) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR = CONTRACT_DIST_ITEMS.CDI_GST_RATE), 0) AS GST, CDI_GST AS CDI_GST, "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "CDI_GST_TAX_CODE AS GstTaxCode, "
                        strSql &= "'' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, CDI_CURRENCY_CODE AS CURRENCY, CDM_S_COY_ID AS VENDOR, '' AS ACCT, CAST(CDM_GROUP_INDEX AS CHAR(100)) AS CDM_GROUP_INDEX, 0.00 AS RFQ_QTY, '' AS PRD_ASSET_GROUP, '' AS POD_ASSET_GROUP, '' AS PRD_ASSET_NO, '' AS POD_ASSET_NO, 0.00 as 'POD_TAX_VALUE' "

                        'Jules 2018.05.07 - PAMB Scrum 2
                        If Common.Parse(companyId).ToString.ToUpper = "PAMB" Then
                            strSql &= "'N' AS GIFT, 'ITNP' AS FUNDTYPE, '' AS PERSONCODE, '' AS PROJECTCODE, "
                            strSql &= "(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_ANALYSIS_CODE = 'ITNP') AS FUNDTYPEDESC, '' AS PERSONCODEDESC, '' AS PROJECTCODEDESC, "
                            strSql &= "IF(PM_ACCT_CODE='','',(SELECT CONCAT(CBG_B_GL_DESC,' (',CBG_B_GL_CODE,')') AS PM_ACCT_CODE  FROM COMPANY_B_GL_CODE "
                            strSql &= "WHERE  CBG_B_COY_ID = PM_S_COY_ID AND CBG_B_GL_CODE = PM_ACCT_CODE AND CBG_STATUS='A' )) AS GLDESCRIPTION "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS FUNDTYPE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PERSONCODE, "
                            'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PROJECTCODE "
                        End If
                        'End modification.

                        strSql &= "FROM CONTRACT_DIST_COY "
                        strSql &= "INNER JOIN CONTRACT_DIST_MSTR ON CDC_GROUP_INDEX = CDM_GROUP_INDEX "
                        strSql &= "INNER JOIN COMPANY_MSTR ON CM_COY_ID = CDM_S_COY_ID AND CM_STATUS = 'A' AND CM_DELETED <> 'Y' "
                        strSql &= "INNER JOIN CONTRACT_DIST_ITEMS ON CDI_GROUP_INDEX=CDM_GROUP_INDEX "
                        strSql &= "LEFT JOIN PRODUCT_MSTR ON CDI_PRODUCT_CODE=PM_PRODUCT_CODE AND CDC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = PM_S_COY_ID "
                        strSql &= "WHERE CDC_B_COY_ID = '" & Common.Parse(companyId) & "' "
                        strSql &= "AND CDM_GROUP_INDEX = '" & strCatGroup & "' "
                        strSql &= "AND PM_DELETED <> 'Y' "
                    End If

                Case "PR"
                    If vcomid.Contains("0") Then
                        strSql &= "SELECT * FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(strPRNo) & "' "
                        strSql &= "AND PRM_COY_ID = '" & Common.Parse(companyId) & "'; "
                    End If

                    If vcomid.Contains("1") Then
                        strSql &= "SELECT PRD_PR_NO, PRD_COY_ID AS COYID, CAST(PRD_PR_LINE AS SIGNED ) AS LINE, PRD_PRODUCT_CODE AS PRODUCTCODE, PRD_VENDOR_ITEM_CODE AS VENDORITEMCODE, PRD_PRODUCT_DESC AS PRODUCTDESC, PRD_B_CATEGORY_CODE AS CATEGORYCODE, PRD_B_GL_CODE AS GLCODE, PRD_UOM AS UOM, "
                        'strSql &= "PRD_ORDERED_QTY AS QUANTITY, PRD_UNIT_COST AS UNITCOST, PRD_REMARK AS REMARK, IFNULL(PRD_GST,0) AS GST, "
                        strSql &= "PRD_ORDERED_QTY AS Qty, PRD_UNIT_COST AS UNITCOST, PRD_REMARK AS REMARK, CURDATE() AS EST, "

                        ' GST Rate Desc
                        strSql &= "IFNULL((SELECT IF(TAX_PERC = '', CODE_DESC, CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) AS GST "
                        strSql &= "FROM CODE_MSTR "
                        strSql &= "INNER JOIN TAX ON TAX_CODE = CODE_ABBR AND TAX_COUNTRY_CODE = 'MY' "
                        strSql &= "WHERE CODE_CATEGORY = 'SST' AND CODE_DELETED = 'N' AND CODE_ABBR = PR_DETAILS.PRD_GST_RATE), 'N/A') AS GSTRateDesc, "

                        strSql &= "PRD_GST_RATE AS GSTRate, IFNULL(PRD_GST,0) AS GST, 0.00 AS CDI_GST, "
                        ' GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
                        strSql &= "PRD_GST_INPUT_TAX_CODE AS GstTaxCode, "
                        strSql &= "PRD_D_ADDR_CODE AS DeliveryAddr, PRD_B_ITEM_CODE AS ITEMCODE, (PRD_UNIT_COST * PRD_ORDERED_QTY) AS AMOUNT, CAST(PRD_PR_LINE AS SIGNED) AS ITEMLINE, PRD_ETD AS ETD, PRD_WARRANTY_TERMS AS WARRANTYTERMS, CAST(PRD_ACCT_INDEX AS CHAR(1000)) AS AcctIndex, PRD_SOURCE AS SOURCE, PRD_COY_ID, '' AS TAXCODE, 0.00 AS RFQ_QTY, '' AS TOLERANCE, '' AS CDGROUP, "
                        strSql &= "PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, CONCAT(PRD_D_ADDR_LINE1, ', ', PRD_D_ADDR_LINE2, ', ', PRD_D_ADDR_LINE3) AS DeliveryAddrDesc, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, PRD_D_COUNTRY, (SELECT CT_NAME FROM COMMODITY_TYPE WHERE CT_ID = PR_DETAILS.PRD_CT_ID) AS COMMODITY, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, PRD_CURRENCY_CODE AS CURRENCY, PRD_S_COY_ID AS VENDOR, CAST(PRD_CD_GROUP_INDEX as CHAR(100)) AS CDM_GROUP_INDEX, PRD_ASSET_GROUP, PRD_ASSET_NO "

                        'Jules 2018.05.04 - PAMB Scrum 2
                        strSql &= ", IFNULL(PRD_GIFT,'N') AS GIFT, "
                        'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS FUNDTYPE, "
                        'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PERSONCODE, "
                        'strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE AND PRD_PR_NO = '" & Common.Parse(strPRNo) & "' AND PRD_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS PROJECTCODE "
                        strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE) AS FUNDTYPE, "
                        strSql &= "CASE WHEN PRD_PERSON_CODE = 'N/A' THEN PRD_PERSON_CODE ELSE (SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE) END PERSONCODE, "
                        strSql &= "(SELECT AC_ANALYSIS_CODE FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE) AS PROJECTCODE,  "
                        strSql &= "IF(PRD_FUND_TYPE ='',PRD_FUND_TYPE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE)) AS FUNDTYPEDESC, "
                        strSql &= "IF(PRD_PERSON_CODE ='',PRD_PERSON_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE)) AS PERSONCODEDESC, "
                        strSql &= "IF(PRD_PROJECT_CODE ='',PRD_PROJECT_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE)) AS PROJECTCODEDESC, "
                        strSql &= "IF(PRD_B_GL_CODE='','',(SELECT CONCAT(CBG_B_GL_DESC,' (',CBG_B_GL_CODE,')') AS PRD_B_GL_CODE  FROM COMPANY_B_GL_CODE "
                        strSql &= "WHERE CBG_B_COY_ID = PRD_COY_ID AND CBG_B_GL_CODE = PRD_B_GL_CODE AND CBG_STATUS='A' )) AS GLDESCRIPTION "
                        'End modification.

                        'Ghesa 2021.05.03: add select VendorItemCodeDesc
                        strSql &= ", Acct.Acct_List AS VendorItemCodeDesc "
                        strSql &= "FROM PR_DETAILS "
                        strSql &= " LEFT JOIN PR_MSTR PM ON PM.PRM_PR_NO = PRD_PR_NO
                                    LEFT JOIN (
 				                    SELECT  AU_USER_ID,  CASE     WHEN A.AM_ACCT_Desc IS NOT NULL THEN A.AM_ACCT_Desc     WHEN A.Q_AM_ACCT_Desc IS NOT NULL THEN A.Q_AM_ACCT_Desc     ELSE A.AM_ACCT_Code END AS Acct_List,     CASE     WHEN A.W_AM_ACCT_INDEX IS NOT NULL THEN A.W_AM_ACCT_INDEX     WHEN A.Q_AM_ACCT_INDEX IS NOT NULL THEN A.Q_AM_ACCT_INDEX     ELSE A.AM_ACCT_INDEX END AS Acct_Index,     CASE     WHEN A.W_AM_ACCT_CODE IS NOT NULL THEN A.W_AM_ACCT_CODE     WHEN A.Q_AM_ACCT_CODE IS NOT NULL THEN A.Q_AM_ACCT_CODE     ELSE A.AM_ACCT_CODE END AS Acct_Code,     A.AM_ACCT_Desc AS L1,     A.Q_AM_ACCT_Desc AS L2,     A.W_AM_ACCT_Desc AS L3     
                                    FROM (        select Y.*, U.* from (SELECT DISTINCT CDM_DEPT_INDEX,CDM_DEPT_CODE FROM ACCOUNT_MSTR A,  COMPANY_DEPT_MSTR C WHERE A.AM_DEPT_INDEX = C.CDM_DEPT_INDEX AND AM_COY_ID='" & Common.Parse(companyId) & "' AND AM_DELETED='N' AND CDM_DELETED = 'N' ORDER BY CDM_DEPT_CODE) Y 
                                    INNER JOIN ( select E.AM_DEPT_INDEX, E.AM_ACCT_INDEX, E.AM_ACCT_CODE, E.AM_ACCT_DESC, E.AM_PARENT_ACCT_INDEX, E.AU_USER_ID, R.* from (SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX, B.AU_USER_ID FROM ACCOUNT_MSTR A LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX WHERE AM_COY_ID='" & Common.Parse(companyId) & "' AND AM_LEVEL=1 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE) E 
                                    LEFT JOIN ( select Q.AM_DEPT_INDEX AS Q_AM_DEPT_INDEX, Q.AM_ACCT_INDEX AS Q_AM_ACCT_INDEX, Q.AM_ACCT_CODE AS Q_AM_ACCT_CODE, Q.AM_ACCT_DESC AS Q_AM_ACCT_DESC, Q.AM_PARENT_ACCT_INDEX AS Q_AM_PARENT_ACCT_INDEX, Q.AU_USER_ID AS Q_AU_USER_ID, W.AM_DEPT_INDEX AS W_AM_DEPT_INDEX, W.AM_ACCT_INDEX AS W_AM_ACCT_INDEX, W.AM_ACCT_CODE AS W_AM_ACCT_CODE, W.AM_ACCT_DESC AS W_AM_ACCT_DESC, W.AM_PARENT_ACCT_INDEX AS W_AM_PARENT_ACCT_INDEX, W.AU_USER_ID AS W_AU_USER_ID from (SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A LEFT OUTER JOIN ACCOUNT_USERS B 
                                    ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX WHERE AM_COY_ID='" & Common.Parse(companyId) & "' AND AM_LEVEL=2  AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE) Q LEFT JOIN (SELECT AM_DEPT_INDEX,AM_ACCT_INDEX,AM_ACCT_CODE,AM_ACCT_DESC,AM_PARENT_ACCT_INDEX,AU_USER_ID FROM ACCOUNT_MSTR A LEFT OUTER JOIN ACCOUNT_USERS B ON A.AM_ACCT_INDEX = B.AU_ACCT_INDEX  WHERE AM_COY_ID='" & Common.Parse(companyId) & "' AND AM_LEVEL=3 AND AM_DELETED='N' ORDER BY AM_ACCT_DESC,AM_ACCT_CODE) W ON Q.AM_ACCT_INDEX = W.AM_PARENT_ACCT_INDEX ) R ON E.AM_ACCT_INDEX = R.Q_AM_PARENT_ACCT_INDEX ) U 
                                    ON Y.CDM_DEPT_INDEX = U.AM_DEPT_INDEX         WHERE U.AU_User_id IS NOT NULL ) A ) Acct
				                    ON PM.PRM_BUYER_ID = Acct.AU_USER_ID AND PRD_ACCT_INDEX = Acct.Acct_Index "
                        'End modification. 2021.05.03

                        strSql &= "WHERE PRD_PR_NO = '" & Common.Parse(strPRNo) & "'"
                        strSql &= "AND PRD_COY_ID = '" & Common.Parse(companyId) & If(getStrSQL, "'", "'; ")
                    End If

                    If vcomid.Contains("2") Then
                        strSql &= "SELECT PCD_PR_INDEX, PCD_PR_LINE, PCD_FIELD_NO, PCD_FIELD_VALUE FROM PR_CUSTOM_FIELD_DETAILS LEFT JOIN PR_MSTR ON "
                        strSql &= "PRM_PR_INDEX = PCD_PR_INDEX WHERE "
                        strSql &= "PRM_PR_NO = '" & Common.Parse(strPRNo) & "' "
                        strSql &= "AND PRM_COY_ID = '" & Common.Parse(companyId) & "' AND PCD_TYPE = 'PR'; "
                    End If
            End Select

            'ds = objDb.FillDs(strSql)
            'getPRItemList = ds

            Dim result As Result = New Result
            If Not getStrSQL Then
                ds = objDb.FillDs(strSql)
            End If
            result.strSql = strSql
            result.ds = ds
            getPRItemList = result
        End Function

        Public Function getPOItemList(ByVal strType As String, ByVal strIndex As String, ByVal strPONo As String, Optional ByVal vcomid As String = "", Optional ByVal LineNumber As ArrayList = Nothing, Optional ByVal RFQListing As String = "", Optional ByVal FFPO As Boolean = False) As DataSet
            Dim strSql As String
            Dim ds As DataSet
            Select Case strType
                'Michelle (15/11/2010) - Shopping Cart is obselete, replace by Purchasing Catalogue (PC)
                Case "PC"
                    strSql = "SELECT PM_PRODUCT_CODE AS PRODUCTCODE, PM_LAST_TXN_PRICE AS UNITCOST, PM_PRODUCT_DESC AS PRODUCTDESC, PM_VENDOR_ITEM_CODE AS VENDORITEMCODE, 500 AS ITEMLINE, "
                    strSql &= "PM_UOM AS UOM, 'PC' AS SOURCE, 0.00 AS RFQ_QTY, 0.00 AS AMOUNT, '0' AS TOLERANCE, '' AS REMARK, '' AS CDGROUP, 0.00 AS QUANTITY, 0 AS ITEMINDEX, "
                    strSql &= "PM_CAT_CODE AS CATEGORYCODE, PM_ACCT_CODE AS GLCODE, CBC_B_TAX_CODE AS TAXCODE, '' AS GSTRATE, 0.00 AS GST, '' AS MOQ, CAST(1.00 AS CHAR(10)) AS MPQ, '' AS CDM_GROUP_INDEX, '' AS ACCT, '' AS POD_ASSET_GROUP, '' AS POD_ASSET_NO, 0.00 AS POD_TAX_VALUE, 0.00 AS CDI_GST, "
                    'GST Tax Code
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    strSql &= "'' AS GstTaxCode "
                    strSql &= "FROM PRODUCT_MSTR LEFT JOIN COMPANY_B_ITEM_CODE ON PM_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "WHERE PM_PRODUCT_CODE IN (" & strIndex & ") AND PM_S_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND PM_DELETED <> 'Y' "

                Case "PO" ' from PO_DETAILS table
                    If FFPO = False Then
                        strSql = "SELECT * FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo & "' "
                        strSql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "

                        strSql &= "SELECT POD_PO_NO, POD_COY_ID AS COYID, POD_PO_LINE AS ITEMINDEX, POD_PRODUCT_CODE AS PRODUCTCODE, POD_VENDOR_ITEM_CODE AS VENDORITEMCODE, POD_PRODUCT_DESC AS PRODUCTDESC, POD_B_CATEGORY_CODE AS CATEGORYCODE, POD_B_GL_CODE AS GLCODE, POD_UOM AS UOM, "
                        'strSql &= "POD_ORDERED_QTY AS QUANTITY, POD_UNIT_COST AS UNITCOST, POD_REMARK AS REMARK,  ISNULL(POD_GST,0) AS GST, "
                        strSql &= "POD_ORDERED_QTY AS QUANTITY, POD_UNIT_COST AS UNITCOST, POD_REMARK AS REMARK, POD_GST_RATE AS GSTRATE,  ISNULL(POD_GST,0) AS GST, "
                        strSql &= "POD_D_ADDR_CODE AS DADDRCODE, POD_B_ITEM_CODE AS ITEMCODE, (POD_UNIT_COST * POD_ORDERED_QTY) AS AMOUNT, POD_PO_LINE AS ITEMLINE, POD_ETD AS ETD, POD_WARRANTY_TERMS AS WARRANTYTERMS, CAST(POD_ACCT_INDEX AS CHAR(1000)) AS ACCT, POD_SOURCE AS SOURCE, POD_COY_ID, '' AS TAXCODE, 0.00 AS RFQ_QTY, '' AS TOLERANCE, '' AS CDGROUP, POD_GST AS CDI_GST,  "
                        strSql &= "POD_D_ADDR_CODE, POD_D_ADDR_LINE1, POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, POD_D_COUNTRY, '' AS COMMODITY, "
                        strSql &= "CAST((CASE WHEN POD_MIN_ORDER_QTY IS NULL THEN '' ELSE POD_MIN_ORDER_QTY END) AS CHAR(10)) AS MOQ, "
                        strSql &= "CAST((CASE WHEN POD_MIN_PACK_QTY IS NULL THEN 1.00 ELSE POD_MIN_PACK_QTY END) AS CHAR(10)) AS MPQ, "
                        strSql &= "CAST(IFNULL(POD_CD_GROUP_INDEX, '') AS CHAR(100)) AS CDM_GROUP_INDEX, CAST(POD_ACCT_INDEX AS CHAR(1000)) AS ACCT, POD_RFQ_ITEM_LINE, POD_ASSET_GROUP, POD_ASSET_NO, IFNULL(POD_TAX_VALUE,0.00) AS POD_TAX_VALUE, "
                        'strSql &= "POD_MIN_ORDER_QTY AS MOQ,POD_MIN_PACK_QTY AS MPQ "

                        'GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        strSql &= "POD_GST_INPUT_TAX_CODE AS GstTaxCode "

                        'Jules 2018.05.07 - PAMB Scrum 2 & 3
                        If HttpContext.Current.Session("CompanyId").ToString.ToUpper = "PAMB" Then
                            strSql &= ", IFNULL(POD_GIFT,'N') AS GIFT, "
                            'strSql &= "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_FUND_TYPE) AS FUNDTYPE, "
                            'strSql &= "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PERSON_CODE) AS PERSONCODE, "
                            'strSql &= "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PROJECT_CODE) AS PROJECTCODE, "                            

                            'Jules 2018.10.18
                            strSql &= "POD_FUND_TYPE AS FUNDTYPE, POD_PERSON_CODE AS PERSONCODE, POD_PROJECT_CODE AS PROJECTCODE, "
                            strSql &= "IF(POD_FUND_TYPE ='',POD_FUND_TYPE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = POD_FUND_TYPE)) AS FUNDTYPEDESC, "
                            strSql &= "IF(POD_PERSON_CODE ='',POD_PERSON_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = POD_PERSON_CODE)) AS PERSONCODEDESC, "
                            strSql &= "IF(POD_PROJECT_CODE ='',POD_PROJECT_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = POD_PROJECT_CODE)) AS PROJECTCODEDESC "
                            'End modification.
                        End If
                        'End modification.

                        strSql &= "FROM PO_DETAILS "
                        strSql &= "WHERE POD_PO_NO = '" & strPONo & "' "
                        strSql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "

                        strSql &= "SELECT PCD_PR_INDEX, PCD_PR_LINE, PCD_FIELD_NO, PCD_FIELD_VALUE FROM PR_CUSTOM_FIELD_DETAILS LEFT JOIN PO_MSTR ON  "
                        strSql &= "POM_PO_INDEX = PCD_PR_INDEX WHERE "
                        strSql &= " POM_PO_NO = '" & strPONo & "' "
                        strSql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PCD_TYPE = 'PO'; "
                    Else
                        strSql = "SELECT * FROM PO_MSTR WHERE POM_PO_NO = '" & strPONo & "' "
                        strSql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "

                        strSql &= "SELECT POD_PO_NO, POD_COY_ID AS COYID, POD_PO_LINE AS ITEMINDEX, POD_PRODUCT_CODE AS PRODUCTCODE, POD_VENDOR_ITEM_CODE AS VENDORITEMCODE, POD_PRODUCT_DESC AS PRODUCTDESC, POD_B_CATEGORY_CODE AS CATEGORYCODE, POD_B_GL_CODE AS GLCODE, POD_UOM AS UOM, "
                        strSql &= "POD_ORDERED_QTY AS QUANTITY, POD_UNIT_COST AS UNITCOST, POD_REMARK AS REMARK, ISNULL(POD_GST,0) AS GST, "
                        strSql &= "POD_D_ADDR_CODE AS DADDRCODE, POD_B_ITEM_CODE AS ITEMCODE, (POD_UNIT_COST * POD_ORDERED_QTY) AS AMOUNT, POD_PO_LINE AS ITEMLINE, POD_ETD AS ETD, POD_WARRANTY_TERMS AS WARRANTYTERMS, CAST(POD_ACCT_INDEX AS CHAR(1000)) AS ACCT, POD_SOURCE AS SOURCE, POD_COY_ID, '' AS TAXCODE, 0.00 AS RFQ_QTY, '' AS TOLERANCE, '' AS CDGROUP,  "
                        strSql &= "POD_D_ADDR_CODE, POD_D_ADDR_LINE1, POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, POD_D_COUNTRY, '' AS COMMODITY, "
                        strSql &= "CAST((CASE WHEN POD_MIN_ORDER_QTY IS NULL THEN '' ELSE POD_MIN_ORDER_QTY END) AS CHAR(10)) AS MOQ, "
                        strSql &= "CAST((CASE WHEN POD_MIN_PACK_QTY IS NULL THEN 1.00 ELSE POD_MIN_PACK_QTY END) AS CHAR(10)) AS MPQ, "
                        ' yAP: POD_GST_RATE for FFPO
                        strSql &= "CAST(IFNULL(POD_CD_GROUP_INDEX, '') AS CHAR(100)) AS CDM_GROUP_INDEX, CAST(POD_ACCT_INDEX AS CHAR(1000)) AS ACCT, POD_RFQ_ITEM_LINE, POD_ASSET_GROUP, POD_ASSET_NO, POD_GST_RATE, POD_TAX_VALUE, "
                        'strSql &= "POD_MIN_ORDER_QTY AS MOQ,POD_MIN_PACK_QTY AS MPQ "

                        'GST Tax Code
                        'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                        strSql &= "POD_GST_INPUT_TAX_CODE AS GstTaxCode "

                        'Jules 2018.05.07 - PAMB Scrum 2 & 3
                        If HttpContext.Current.Session("CompanyId").ToString.ToUpper = "PAMB" Then
                            strSql &= ", IFNULL(POD_GIFT,'N') AS GIFT, "
                            'strSql &= "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_FUND_TYPE) AS FUNDTYPE, "
                            'strSql &= "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PERSON_CODE) AS PERSONCODE, "
                            'strSql &= "(SELECT CONCAT(AC_ANALYSIS_CODE,' : ',AC_ANALYSIS_CODE_DESC) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PROJECT_CODE) AS PROJECTCODE "                            

                            'Jules 2018.10.18
                            strSql &= "POD_FUND_TYPE AS FUNDTYPE, POD_PERSON_CODE AS PERSONCODE, POD_PROJECT_CODE AS PROJECTCODE, "
                            strSql &= "IF(POD_FUND_TYPE ='',POD_FUND_TYPE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = POD_FUND_TYPE)) AS FUNDTYPEDESC, "
                            strSql &= "IF(POD_PERSON_CODE ='',POD_PERSON_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = POD_PERSON_CODE)) AS PERSONCODEDESC, "
                            strSql &= "IF(POD_PROJECT_CODE ='',POD_PROJECT_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = POD_PROJECT_CODE)) AS PROJECTCODEDESC "
                            'End modification.
                        End If
                        'End modification.

                        strSql &= "FROM PO_DETAILS "
                        strSql &= "WHERE POD_PO_NO = '" & strPONo & "' "
                        strSql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "

                        strSql &= "SELECT PCD_PR_INDEX, PCD_PR_LINE, PCD_FIELD_NO, PCD_FIELD_VALUE FROM PR_CUSTOM_FIELD_DETAILS LEFT JOIN PO_MSTR ON  "
                        strSql &= "POM_PO_INDEX = PCD_PR_INDEX WHERE "
                        strSql &= " POM_PO_NO = '" & strPONo & "' "
                        strSql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PCD_TYPE = 'PO'; "
                    End If
                Case "RFQ"
                    Dim i As Int16, sLineNumber As String = ""
                    If Not IsNothing(LineNumber) Then
                        For i = 0 To LineNumber.Count - 1
                            sLineNumber = sLineNumber & " OR RRD_Line_No = '" & LineNumber(i) & "' "
                        Next
                    End If

                    If sLineNumber.Length > 0 Then
                        sLineNumber = " AND (" & Mid(sLineNumber, 5) & ")"
                    End If

                    strSql = "SELECT distinct RRD_V_Coy_Id AS COYID, RRD_Line_No AS ITEMINDEX, RRD_Product_Code AS PRODUCTCODE, RRD_Vendor_Item_Code AS VENDORITEMCODE, RRD_Product_Desc AS PRODUCTDESC, "
                    strSql &= "RRD_UOM AS UOM, RRD_Quantity AS RFQ_QTY, RRD_Quantity AS QUANTITY, RRD_Unit_Price AS UNITCOST, RRD_Min_Order_Qty AS MOQ, CAST((CASE WHEN RRD_Min_Pack_Qty IS NULL THEN 1 ELSE RRD_Min_Pack_Qty END) AS CHAR(10)) AS MPQ, "
                    'If RFQListing <> "" Then
                    '    strSql &= " (SELECT PRD_REMARK FROM PR_MSTR, PR_DETAILS "
                    '    strSql &= " WHERE PRM_PR_NO = PRD_PR_NO AND PRM_COY_ID = PRD_COY_ID AND PRD_PR_LINE_INDEX = RFQ_DETAIL.RD_PR_LINE_INDEX AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "') AS REMARK, "
                    'Else
                    '    strSql &= " RRD_Remarks AS REMARK, "
                    'End If
                    strSql &= " RRD_Remarks AS REMARK, RM_Remark AS POM_EXTERNAL_REMARK, "
                    strSql &= " RRD_GST_RATE AS GSTRATE, "
                    strSql &= "ISNULL(CAST(RRD_GST AS DECIMAL),0.00) AS GST, "
                    strSql &= "CBC_B_ITEM_CODE AS ITEMCODE, (RRD_Unit_Price * RRD_Quantity) AS AMOUNT, RRD_Line_No AS ITEMLINE, RRD_Delivery_Lead_Time AS ETD, RRD_Warranty_Terms AS WARRANTYTERMS, "
                    'Michelle (16/6/2010) - To include the Tax Code
                    'strSql &= "CBC_B_CATEGORY_CODE AS CATEGORYCODE, CBC_B_GL_CODE AS GLCODE, "
                    'strSql &= "CBC_B_CATEGORY_CODE AS CATEGORYCODE, "
                    strSql &= "(SELECT PM_CAT_CODE FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS CATEGORYCODE, "
                    strSql &= "(SELECT PM_ACCT_CODE FROM PRODUCT_MSTR WHERE PM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PM_PRODUCT_CODE = RRD_PRODUCT_CODE) AS GLCODE, "
                    strSql &= "CBC_B_TAX_CODE AS TAXCODE, "
                    strSql &= "CAST(RRD_Tolerance AS CHAR) AS TOLERANCE, 'RF' AS SOURCE, '' AS CDGROUP, '' AS CDM_GROUP_INDEX, '' AS ACCT, '' AS POD_ASSET_GROUP, '' AS POD_ASSET_NO, 0.00 AS POD_TAX_VALUE, "
                    'strSql &= "(SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE = RFQ_REPLIES_DETAIL.RRD_Line_No) AS ACCT "
                    'strSql &= "(SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PRODUCT_CODE = RFQ_REPLIES_DETAIL.RRD_Product_Code) AS ACCT "
                    '' ''strSql &= "CAST(IF( RFQ_REPLIES_DETAIL.RRD_Product_Code = '&nbsp;' OR RFQ_REPLIES_DETAIL.RRD_Product_Code = '', (SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PRODUCT_DESC = RFQ_REPLIES_DETAIL.RRD_Product_Desc), (SELECT PRD_ACCT_INDEX FROM pr_details WHERE PRD_CONVERT_TO_DOC = RFQ_MSTR.RM_RFQ_NO AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE_INDEX = RFQ_DETAIL.RD_PR_LINE_INDEX )) AS CHAR(100)) AS ACCT "
                    'If sLineNumber.Length > 0 Then
                    '    strSql &= " AND PRD_PR_LINE = RRD_Line_No )) AS CHAR(100)) AS ACCT "
                    'Else
                    '    strSql &= " )) AS CHAR(100)) AS ACCT "
                    'End If

                    'GST Tax Code
                    'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
                    strSql &= "'' AS GstTaxCode "

                    'Jules 2018.05.07 - PAMB Scrum 2
                    If RFQListing <> "" Then
                        strSql &= ", PRD_GIFT AS GIFT, PRD_FUND_TYPE AS FUNDTYPE, PRD_PERSON_CODE AS PERSONCODE, PRD_PROJECT_CODE AS PROJECTCODE, "

                        'Jules 2018.10.19
                        strSql &= "IF(PRD_FUND_TYPE ='',PRD_FUND_TYPE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_FUND_TYPE)) AS FUNDTYPEDESC, "
                        strSql &= "IF(PRD_PERSON_CODE ='',PRD_PERSON_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PERSON_CODE)) AS PERSONCODEDESC, "
                        strSql &= "IF(PRD_PROJECT_CODE ='',PRD_PROJECT_CODE,(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), AC_ANALYSIS_CODE) FROM analysis_code WHERE AC_ANALYSIS_CODE = PRD_PROJECT_CODE)) AS PROJECTCODEDESC "
                        'End modification.
                    Else
                        'Jules 2018.10.18
                        strSql &= ", 'N' AS GIFT, 'ITNP' AS FUNDTYPE, '' AS PERSONCODE, '' AS PROJECTCODE, "
                        strSql &= "(SELECT CONCAT(CONCAT(IFNULL(AC_ANALYSIS_CODE_DESC, ''),' : '), IFNULL(AC_ANALYSIS_CODE, '')) FROM analysis_code WHERE AC_ANALYSIS_CODE = 'ITNP' and ac_b_coy_id = '" & HttpContext.Current.Session("CompanyId") & "') AS FUNDTYPEDESC, '' AS PERSONCODEDESC, '' AS PROJECTCODEDESC "
                        'End modification.
                    End If

                    'End modification.

                    strSql &= "FROM RFQ_MSTR "
                    '' ''strSql &= "INNER JOIN RFQ_DETAIL ON RD_RFQ_ID = RM_RFQ_ID "
                    strSql &= "INNER JOIN RFQ_REPLIES_MSTR ON RRM_RFQ_ID = RM_RFQ_ID AND RM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    strSql &= "INNER JOIN RFQ_REPLIES_DETAIL ON RRM_RFQ_ID = RRD_RFQ_ID AND RRM_V_COMPANY_ID = '" & vcomid & "' "

                    'Jules 2018.05.07 - PAMB Scrum 2
                    If RFQListing <> "" Then
                        strSql &= "INNER JOIN RFQ_DETAIL ON RD_RFQ_ID = RM_RFQ_ID AND RD_RFQ_LINE = RRD_Line_No "
                        strSql &= "INNER JOIN PR_DETAILS ON PRD_COY_ID = RM_COY_ID AND PRD_CONVERT_TO_DOC = RM_RFQ_NO AND RD_PR_LINE_INDEX = PRD_PR_LINE_INDEX "
                    End If
                    'End modification.

                    strSql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON RRD_Product_Code = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    'Michelle (23/2/2011) - To enable PO to raise when the unit price = 0 (ie. free)
                    'strSql &= "WHERE RRD_RFQ_ID = '" & strPONo & "' and  RRD_Unit_Price <> 0 "
                    strSql &= "WHERE RRD_RFQ_ID = '" & strPONo & "' and  RRD_Unit_Price >= 0 "
                    'strSql &= "AND RRD_Line_No IN (" & strIndex & ") and RRD_V_Coy_Id='" & vcomid & "' "
                    strSql &= "AND RRD_V_Coy_Id='" & vcomid & "' " & sLineNumber & "; "

                    If RFQListing <> "" Then
                        'strSql &= "SELECT PCD_PR_INDEX, PCD_PR_LINE, PCD_FIELD_NO, PCD_FIELD_VALUE FROM PR_CUSTOM_FIELD_DETAILS LEFT JOIN PR_MSTR ON "
                        'strSql &= "PRM_PR_INDEX = PCD_PR_INDEX WHERE "
                        'strSql &= "PRM_PR_INDEX = '" & Common.Parse(RFQListing) & "' "
                        'strSql &= "AND PRM_COY_ID = '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' AND PCD_TYPE = 'PR'; "
                        Dim sLineNumber2 As String = ""
                        If Not IsNothing(LineNumber) Then
                            For i = 0 To LineNumber.Count - 1
                                sLineNumber2 = sLineNumber2 & " OR PCD_PR_LINE = '" & LineNumber(i) & "' "
                            Next
                        End If

                        If sLineNumber2.Length > 0 Then
                            sLineNumber2 = " AND (" & Mid(sLineNumber2, 5) & ")"
                        End If

                        'strSql &= " SET @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
                        'strSql &= " SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE, "
                        'strSql &= " PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM ( "
                        'strSql &= " SELECT PCD_PR_INDEX, @num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, "
                        'strSql &= " @num AS LINE_NO, @PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, "
                        'strSql &= " @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE "
                        'strSql &= " FROM RFQ_DETAIL "
                        'strSql &= " INNER JOIN PR_DETAILS ON PRD_PR_LINE_INDEX = RD_PR_LINE_INDEX "
                        'strSql &= " INNER JOIN PR_MSTR ON PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO "
                        'strSql &= " INNER JOIN PR_CUSTOM_FIELD_DETAILS ON PCD_PR_INDEX = PRM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE "
                        'strSql &= " WHERE RD_RFQ_ID = '" & strPONo & "' AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PCD_TYPE = 'PR' " & sLineNumber2 & " "
                        'strSql &= " ORDER BY RD_RFQ_LINE, PCD_PR_LINE, PCD_FIELD_NO "
                        'strSql &= " ) ZZZZ "

                        strSql &= " SET @num  = 0; SET @PCD_FIELD_NO = 0; SET @PCD_PR_LINE = 0; SET @PCD_PR_INDEX = 0; "
                        strSql &= " SELECT PCD_PR_INDEX, LINE_NO AS PCD_PR_LINE, "
                        strSql &= " PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE FROM ( "
                        strSql &= " SELECT PCD_PR_INDEX, @num := IF(PCD_PR_LINE<> @PCD_PR_LINE, @num + 1, IF(PCD_PR_INDEX <> @PCD_PR_INDEX, @num +1 , @num)) AS d4, "
                        strSql &= " @num AS LINE_NO, @PCD_FIELD_NO := IF(@PCD_FIELD_NO=1,1,IF(PCD_PR_LINE-1=0, 1, 0)) AS VPCD_FIELD_NO, "
                        strSql &= " @PCD_PR_LINE := PCD_PR_LINE, @PCD_PR_INDEX := PCD_PR_INDEX, PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE "
                        strSql &= " FROM RFQ_DETAIL "
                        strSql &= " INNER JOIN PR_DETAILS ON PRD_PR_LINE_INDEX = RD_PR_LINE_INDEX "
                        strSql &= " INNER JOIN PR_MSTR ON PRD_COY_ID = PRM_COY_ID AND PRD_PR_NO = PRM_PR_NO "
                        strSql &= " INNER JOIN PR_CUSTOM_FIELD_DETAILS ON PCD_PR_INDEX = PRM_PR_INDEX AND PRD_PR_LINE = PCD_PR_LINE "
                        strSql &= " WHERE RD_RFQ_ID = '" & strPONo & "' AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' AND PCD_TYPE = 'PR' "
                        strSql &= " ORDER BY RD_RFQ_LINE, PCD_PR_LINE, PCD_FIELD_NO "
                        strSql &= " ) ZZZZ "

                    End If
            End Select

            ds = objDb.FillDs(strSql)
            getPOItemList = ds
        End Function

        Public Function getPRHeader(ByVal strPRNo As String) As DataSet
            Dim strsql As String
            Dim ds As New DataSet
            strsql = "SELECT * FROM PR_MSTR WHERE PRM_PR_NO = '" & strPRNo & "' "
            strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            ds = objDb.FillDs(strsql)
            getPRHeader = ds
        End Function

        ' SR AS0048 - Vendor company deactivation process
        '           - remove the items inside Shopping cart to prevent item being created into PR
        Public Sub deleteDeactivatedCompanyShoppingCartItem(ByVal strCoyId As String)
            Dim strsql As String
            Dim objDb As New EAD.DBCom(ConfigurationSettings.AppSettings("eProcurePath"))
            strsql = "DELETE FROM SHOPPING_CART WHERE SC_S_COY_ID = '" & Common.Parse(strCoyId) & "' "
            objDb.Execute(strsql)
            objDb = Nothing
        End Sub

    End Class

End Namespace
