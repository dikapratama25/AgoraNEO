Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Web.UI.WebControls
Imports AgoraNEO
Imports SSO.Component

Namespace AgoraNEO
    Public Class PR
        Dim objDb As New EAD.DBCom
        ''Dim objGlobal As New AppGlobals
        Public dtMaster As New DataTable
        Public dtDetails As New DataTable
        Public dtCustomMaster As New DataTable
        Public dtCustomDetails As New DataTable

        Public dtAO As New DataTable

        Public Sub Init()

            dtMaster.Columns.Add("PRNo", Type.GetType("System.String"))
            dtMaster.Columns.Add("ReqName", Type.GetType("System.String"))
            dtMaster.Columns.Add("Attn", Type.GetType("System.String"))
            dtMaster.Columns.Add("ReqPhone", Type.GetType("System.String"))
            dtMaster.Columns.Add("InternalRemark", Type.GetType("System.String"))
            dtMaster.Columns.Add("ExternalRemark", Type.GetType("System.String"))
            dtMaster.Columns.Add("PrintCustom", Type.GetType("System.String"))
            dtMaster.Columns.Add("PrintRemark", Type.GetType("System.String"))
            dtMaster.Columns.Add("Urgent", Type.GetType("System.String"))
            dtMaster.Columns.Add("ShipmentTerm", Type.GetType("System.String"))
            dtMaster.Columns.Add("ShipmentMode", Type.GetType("System.String"))
            dtMaster.Columns.Add("PRType", Type.GetType("System.String"))
            dtMaster.Columns.Add("PRCost", Type.GetType("System.Double"))
            dtMaster.Columns.Add("PRSEmail", Type.GetType("System.String"))

            dtMaster.Columns.Add("BillAddrCode", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrLine1", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrLine2", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrLine3", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrPostCode", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrState", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrCity", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrCountry", Type.GetType("System.String"))


            dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
            dtDetails.Columns.Add("ProductCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("VendorItemCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("ProductDesc", Type.GetType("System.String"))
            dtDetails.Columns.Add("Qty", Type.GetType("System.Double"))
            dtDetails.Columns.Add("Commodity", Type.GetType("System.String"))
            dtDetails.Columns.Add("UOM", Type.GetType("System.String"))
            dtDetails.Columns.Add("Currency", Type.GetType("System.String"))
            dtDetails.Columns.Add("UnitCost", Type.GetType("System.Double"))
            dtDetails.Columns.Add("GST", Type.GetType("System.String"))
            dtDetails.Columns.Add("GSTRate", Type.GetType("System.String"))
            'Stage 3 Enhancement (GST-0010) - 13/07/2015 - CH
            dtDetails.Columns.Add("GSTTaxCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("DeliveryAddr", Type.GetType("System.String"))
            dtDetails.Columns.Add("ETD", Type.GetType("System.Int32"))
            dtDetails.Columns.Add("WarrantyTerms", Type.GetType("System.Int32"))
            dtDetails.Columns.Add("Remark", Type.GetType("System.String"))
            dtDetails.Columns.Add("Source", Type.GetType("System.String"))
            dtDetails.Columns.Add("VendorID", Type.GetType("System.String"))
            dtDetails.Columns.Add("GLCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("CategoryCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
            dtDetails.Columns.Add("AcctIndex", Type.GetType("System.String"))

            dtDetails.Columns.Add("AssetGroup", Type.GetType("System.String"))

            'Jules 2018.05.04 - PAMB Scrum 2
            dtDetails.Columns.Add("Gift", Type.GetType("System.String"))
            dtDetails.Columns.Add("FundType", Type.GetType("System.String"))
            dtDetails.Columns.Add("PersonCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("ProjectCode", Type.GetType("System.String"))
            'End modification.

            dtCustomMaster.Columns.Add("FieldNo", Type.GetType("System.Int32"))
            dtCustomMaster.Columns.Add("FieldName", Type.GetType("System.String"))

            dtCustomDetails.Columns.Add("Line", Type.GetType("System.Int32"))
            dtCustomDetails.Columns.Add("FieldNo", Type.GetType("System.Int32"))
            dtCustomDetails.Columns.Add("FieldValue", Type.GetType("System.String"))

            dtAO.Columns.Add("prid", Type.GetType("System.String"))
            dtAO.Columns.Add("AO", Type.GetType("System.String"))
            dtAO.Columns.Add("AAO", Type.GetType("System.String"))
            dtAO.Columns.Add("Seq", Type.GetType("System.Int32"))
            dtAO.Columns.Add("Type", Type.GetType("System.String"))
            dtAO.Columns.Add("GrpIndex", Type.GetType("System.String"))
            dtAO.Columns.Add("Relief", Type.GetType("System.String"))

        End Sub

        Public Function PopulatePRList(ByVal strStatus As String, ByVal strVendor As String) As DataSet
            Dim strSql As String
            Dim dsPR As DataSet
            strSql = "SELECT PRM_PR_NO "
            strSql &= "FROM PR_MSTR "
            'strSql &= "WHERE PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            'strSql &= "AND PRM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "

            ''Dim test As String = HttpContext.Current.Session("test")

            strSql &= "WHERE PRM_COY_ID = 'pamb' "
            strSql &= "AND PRM_BUYER_ID = 'req264095' "

            ' ai chu modified on 24/02/2006
            ' only draft PR not raised from quotation will be displayed
            strSql &= "AND PRM_RFQ_INDEX IS NULL "

            'Dim blnMultipleSupplier As Boolean = True
            '---------------New Code Added for CM_MULTI_PO on 04/July/2007  Start------------
            Dim blnMultipleSupplier As Boolean
            Dim Strmulti As String
            Dim strSql1 As String
            'Dim dd As New System.Web.UI.WebControls.DropDownList
            Dim dds As DataTable
            'strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
            strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='pamb'"

            Dim tDS As DataSet = objDb.FillDs(strSql1)
            If tDS.Tables(0).Rows.Count > 0 Then
                Strmulti = tDS.Tables(0).Rows(0).Item("CM_MULTI_PO")
            End If
            If Strmulti = "Y" Then
                blnMultipleSupplier = True
            Else
                blnMultipleSupplier = False
            End If
            '---------------New Code Added for CM_MULTI_PO on 04/July/2007  END------------    

            If strVendor = "" Then
                If Not blnMultipleSupplier Then
                    strSql &= "AND 1 = 2"
                End If
            Else
                strSql &= "AND PRM_S_COY_ID = '" & Common.Parse(strVendor) & "' "
            End If

            If strStatus <> "" Then
                strSql &= "AND PRM_PR_STATUS = '" & Common.Parse(strStatus) & "' "
            End If

            dsPR = objDb.FillDs(strSql)
            PopulatePRList = dsPR
        End Function

        Public Function updatePR(ByVal userID As String, ByVal companyID As String, ByVal dsPR As DataSet, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True)
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i As Integer

            strsql = "UPDATE PR_MSTR SET "
            'strsql &= "PRM_REQ_NAME = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) & "', "

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) <> "" Then
                strsql &= "PRM_REQ_NAME = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) & "', " ' REQ_NAME
            Else
                Dim PR_Req_Name As String = objDb.GetVal("SELECT IFNULL(UM_USER_NAME,'') FROM USER_MSTR WHERE UM_USER_ID = '" & userID & "' AND UM_COY_ID = '" & companyID & "'")
                strsql &= "PRM_REQ_NAME = '" & Common.Parse(PR_Req_Name) & "', " ' REQ_NAME
            End If

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) <> "" Then
                strsql &= "PRM_REQ_PHONE = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) & "', " 'REQ_PHONE
            Else
                Dim PR_Req_Tel As String = objDb.GetVal("SELECT IFNULL(UM_TEL_NO,'') FROM USER_MSTR WHERE UM_USER_ID = '" & userID & "' AND UM_COY_ID = '" & companyID & "'")
                strsql &= "PRM_REQ_PHONE = '" & Common.Parse(PR_Req_Tel) & "', " 'REQ_PHONE
            End If

            'strsql &= "PRM_REQ_PHONE = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) & "', "
            'strsql &= "PRM_PR_DATE = " & Common.ConvertDate(Now) & ", "
            strsql &= "PRM_S_ATTN = '" & Common.Parse(dsPR.Tables(0).Rows(0)("Attn")) & "', "
            strsql &= "PRM_INTERNAL_REMARK = '" & Common.Parse(dsPR.Tables(0).Rows(0)("InternalRemark")) & "', "
            strsql &= "PRM_EXTERNAL_REMARK = '" & Common.Parse(dsPR.Tables(0).Rows(0)("ExternalRemark")) & "', "
            strsql &= "PRM_PRINT_CUSTOM_FIELDS = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintCustom")) & "', "
            strsql &= "PRM_PRINT_REMARK = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintRemark")) & "', "
            strsql &= "PRM_URGENT = '" & Common.Parse(dsPR.Tables(0).Rows(0)("Urgent")) & "', "
            strsql &= "PRM_PR_COST = " & Common.Parse(dsPR.Tables(0).Rows(0)("PRCost")) & " "
            strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
            strsql &= "AND PRM_COY_ID = '" & companyID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' delete from PR_DETAILS table
            strsql = "DELETE FROM PR_DETAILS "
            strsql &= "WHERE PRD_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
            strsql &= "AND PRD_COY_ID = '" & companyID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            ' PR_DETAILS table

            For i = 0 To dsPR.Tables(1).Rows.Count - 1

                strsql = "INSERT INTO PR_DETAILS (PRD_PR_NO, PRD_COY_ID, PRD_PR_LINE, PRD_PRODUCT_CODE, "
                strsql &= "PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_UOM, PRD_ORDERED_QTY, PRD_UNIT_COST, "
                strsql &= "PRD_ETD, PRD_WARRANTY_TERMS, PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY, PRD_REMARK, PRD_GST, PRD_GST_RATE, PRD_GST_INPUT_TAX_CODE, PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, "
                strsql &= "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, "

                'Jules 2018.05.04 - PAMB Scrum 2
                If companyID.ToString.ToLower = "pamb" Then
                    If blnEnterpriseVersion = True Then
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_ASSET_GROUP, PRD_GIFT, PRD_FUND_TYPE, PRD_PERSON_CODE, PRD_PROJECT_CODE) SELECT "
                    Else
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_GIFT, PRD_FUND_TYPE, PRD_PERSON_CODE, PRD_PROJECT_CODE) SELECT "
                    End If
                Else
                    If blnEnterpriseVersion = True Then
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_ASSET_GROUP) SELECT "
                    Else
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE) SELECT "
                    End If
                End If
                'End modification.                

                strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "', " ' PR_No
                strsql &= "'" & companyID & "', " ' COY_ID
                strsql &= Common.Parse(dsPR.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= dsPR.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
                strsql &= dsPR.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
                strsql &= dsPR.Tables(1).Rows(i)("ETD") & ", " ' ETD
                strsql &= dsPR.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                strsql &= "1, 1, " 'PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Remark")) & "', " ' REMARK
                strsql &= dsPR.Tables(1).Rows(i)("GST") & ", " ' GST
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTRATE")) & "', " ' GSTRATE
                'Stage 3 (Enhancement) (GST-0010) - 13/07/2015 - CH
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTTaxCode")) & "', " ' GSTTaxCode

                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "

                If Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) <> "" Then '_Yap
                    strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) & "', " ' ACCT_INDEX
                Else
                    strsql &= "null, " ' ACCT_INDEX
                End If

                'strsql &= "NULL, " ' ACCT_INDEX '_Yap
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Source")) & "', " ' SOURCE

                If Common.Parse(dsPR.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPR.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
                End If

                'strsql &= "NULL, " 'PRD_CD_GROUP_INDEX
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE

                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorID")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("TaxCode")) & "' "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Commodity")) & "', "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Currency")) & "' "

                If blnEnterpriseVersion = True Then
                    strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("AssetGroup")) & "' "
                End If

                'Jules 2018.05.04 - PAMB Scrum 2
                If companyID.ToString.ToLower = "pamb" Then
                    strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("Gift")) & "', '" & Common.Parse(dsPR.Tables(1).Rows(i)("FundType")) & "', '" & Common.Parse(dsPR.Tables(1).Rows(i)("PersonCode")) & "', '" & Common.Parse(dsPR.Tables(1).Rows(i)("ProjectCode")) & "' "
                End If
                'End modification.

                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & companyID & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPR.Tables(1).Rows(i)("DeliveryAddr")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If modePR = True Then
                ' delete from PR_CUSTOM_FIELD_DETAILS
                strsql = "DELETE FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PR' AND PCD_PR_INDEX IN "
                strsql &= "(SELECT PRM_PR_INDEX FROM PR_MSTR "
                strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                strsql &= "AND PRM_COY_ID = '" & companyID & "') "
                Common.Insert2Ary(strAryQuery, strsql)

                ' delete from PR_CUSTOM_FIELD_MSTR
                strsql = "DELETE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PR' AND PCM_PR_INDEX IN "
                strsql &= "(SELECT PRM_PR_INDEX FROM PR_MSTR "
                strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                strsql &= "AND PRM_COY_ID = '" & companyID & "') "
                Common.Insert2Ary(strAryQuery, strsql)

                ' PR_CUSTOM_FIELD_MSTR table
                For i = 0 To dsPR.Tables(2).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                    strsql &= "(SELECT PRM_PR_Index FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                    strsql &= "AND PRM_COY_ID = '" & companyID & "'), "
                    strsql &= dsPR.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(2).Rows(i)("FieldName")) & "', " ' FIELD_NAME
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                ' PR_CUSTOM_FIELD_DETAILS table
                For i = 0 To dsPR.Tables(3).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                    strsql &= "(SELECT PRM_PR_Index FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(dsPR.Tables(0).Rows(0)("PRNo")) & "' "
                    strsql &= "AND PRM_COY_ID = '" & companyID & "'), "
                    strsql &= dsPR.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                    strsql &= dsPR.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(3).Rows(i)("FieldValue")) & "', " ' FIELD_VALUE
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If
            objDb.BatchExecute(strAryQuery)
        End Function

        Public Function deletePR(ByVal userID As String, ByVal companyID As String, ByVal strPR As String) As Boolean
            Dim strsql As String
            Dim strAryQuery(0) As String

            strsql = "UPDATE PR_MSTR SET PRM_PR_STATUS = " & PRStatus.Void & ", PRM_STATUS_CHANGED_BY = '" & userID & "', PRM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now)
            strsql &= " WHERE PRM_PR_NO = '" & strPR & "' "
            strsql &= "AND PRM_COY_ID = '" & companyID & "' "
            Common.Insert2Ary(strAryQuery, strsql)

            objDb.BatchExecute(strAryQuery)
            deletePR = True
        End Function

        '     Public Function updatePRDIndex(ByVal strPR As String, ByVal sProductCode As String, ByVal sProductIndex As Integer, ByVal sProductDesc As String) As String
        '         Dim strsql As String

        '         If sProductCode = "&nbsp;" Or sProductCode = "" Then
        '             strsql = "UPDATE PR_DETAILS "
        '             strsql &= "SET PRD_PR_LINE = '" & sProductIndex & "' "
        '             strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
        '             strsql &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             strsql &= "AND PRD_PR_LINE >= '" & sProductIndex & "' "
        '             strsql &= "AND PRD_PRODUCT_DESC = '" & Common.Parse(sProductDesc) & "' "
        '             strsql &= "ORDER BY PRD_PR_LINE "
        '             strsql &= "LIMIT 1 ; "

        '         Else
        '             strsql = "UPDATE PR_DETAILS "
        '             strsql &= "SET PRD_PR_LINE = '" & sProductIndex & "' "
        '             strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
        '             strsql &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             strsql &= "AND PRD_PR_LINE >= '" & sProductIndex & "' "
        '             strsql &= "AND PRD_PRODUCT_CODE = '" & sProductCode & "' "
        '             strsql &= "ORDER BY PRD_PR_LINE "
        '             strsql &= "LIMIT 1 ; "
        '         End If


        '         updatePRDIndex = strsql
        '     End Function

        Public Function deletePRItemSQL(ByVal companyID As String, ByVal strPR As String, ByVal sProductCode As String, ByVal sProductIndex As Integer, ByVal sProductDesc As String) As String
            Dim strsql As String
            ' delete from PR_DETAILS table
            If sProductCode = "&nbsp;" Or sProductCode = "" Then
                strsql = "DELETE FROM PR_DETAILS "
                strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
                strsql &= "AND PRD_COY_ID = '" & companyID & "' "
                strsql &= "AND PRD_PR_LINE = '" & sProductIndex & "' "
                strsql &= "AND PRD_PRODUCT_DESC = '" & Common.Parse(sProductDesc) & "'; "
            Else
                strsql = "DELETE FROM PR_DETAILS "
                strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
                strsql &= "AND PRD_COY_ID = '" & companyID & "' "
                strsql &= "AND PRD_PR_LINE = '" & sProductIndex & "' "
                strsql &= "AND PRD_PRODUCT_CODE = '" & sProductCode & "'; "
            End If

            deletePRItemSQL = strsql
        End Function

        '     Public Function updatePRHeaderSQL(ByVal strPR As String, ByVal sProductCode As String, ByVal sProductIndex As Integer, ByVal sProductDesc As String) As String
        '         Dim strsql As String
        '         ' delete from PR_DETAILS table
        '         If sProductCode = "&nbsp;" Or sProductCode = "" Then
        '             strsql = "UPDATE PR_MSTR A, PR_DETAILS B "
        '             strsql &= "SET A.PRM_PR_COST = A.PRM_PR_COST - (B.PRD_ORDERED_QTY * B.PRD_UNIT_COST) "
        '             strsql &= "WHERE A.PRM_PR_NO = B.PRD_PR_NO AND A.PRM_COY_ID = B.PRD_COY_ID "
        '             strsql &= "AND PRD_PR_NO = '" & strPR & "' "
        '             strsql &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             strsql &= "AND PRD_PR_LINE = '" & sProductIndex & "' "
        '             strsql &= "AND PRD_PRODUCT_DESC = '" & Common.Parse(sProductDesc) & "'; "
        '         Else
        '             strsql = "UPDATE PR_MSTR A, PR_DETAILS B "
        '             strsql &= "SET A.PRM_PR_COST = A.PRM_PR_COST - (B.PRD_ORDERED_QTY * B.PRD_UNIT_COST) "
        '             strsql &= "WHERE A.PRM_PR_NO = B.PRD_PR_NO AND A.PRM_COY_ID = B.PRD_COY_ID "
        '             strsql &= "AND PRD_PR_NO = '" & strPR & "' "
        '             strsql &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             strsql &= "AND PRD_PR_LINE = '" & sProductIndex & "' "
        '             strsql &= "AND PRD_PRODUCT_CODE = '" & sProductCode & "'; "
        '         End If

        '         updatePRHeaderSQL = strsql
        '     End Function

        Public Function get_PR(ByVal companyID As String, ByVal strPR As String, ByVal sProductCode As String, ByVal sProductIndex As String, ByVal sProductDesc As String) As String
            Dim strsql As String
            strsql = "SELECT PRD_PR_LINE AS LINE
                            ,PRD_PR_NO AS PRNO
                            ,PRD_PRODUCT_CODE AS PRODUCTCODE
                            ,PRD_VENDOR_ITEM_CODE AS VENDORITEMCODE
                            ,PRD_PRODUCT_DESC AS PRODUCTDESC
                            ,PRD_ORDERED_QTY AS QTY
                            ,PRD_UOM AS UOM
                            ,PRD_CURRENCY_CODE AS CURRENCY
                            ,PRD_UNIT_COST AS UNITCOST
                            ,PRD_GST_INPUT_TAX_CODE AS GST
                            ,PRD_GST_RATE AS GSTRate
                            ,PRD_GST_INPUT_TAX_CODE AS GSTTaxCode
                            ,PRD_ETD AS ETD
                            ,PRD_WARRANTY_TERMS AS WarrantyTerms
                            ,PRD_REMARK AS REMARK
                            ,PRD_SOURCE AS SOURCE
                            ,PRD_S_COY_ID AS VendorID
                            ,PRD_B_GL_CODE AS GLCode
                            ,PRD_B_CATEGORY_CODE AS CategoryCode
                            ,PRD_CD_GROUP_INDEX AS CDGroup
                            ,PRD_ACCT_INDEX AS AcctIndex
                            ,PRD_ASSET_GROUP AS AssetGroup
                            ,PRD_FUND_TYPE AS FundType
                            ,PRD_PERSON_CODE AS PersonCode
                            ,PRD_PROJECT_CODE AS ProjectCode
                            ,PRD_ORDERED_QTY AS QUANTITY
                        FROM PR_DETAILS WHERE 1=1 AND PRD_COY_ID ='" & companyID & "'"

            If strPR = "" Then
                Return False
            Else
                strsql &= "AND PRD_PR_NO = '" & strPR & "' "
            End If
            If sProductCode <> "" Then
                strsql &= "AND PRD_PRODUCT_CODE = '" & sProductCode & "'; "
            End If
            If sProductIndex <> "" Then
                strsql &= "AND PRD_PR_LINE = '" & Common.Parse(sProductIndex) & "' "
            End If
            If sProductDesc <> "" Then
                strsql &= "AND PRD_PRODUCT_DESC = '" & Common.Parse(sProductDesc) & "'; "
            End If
            'If sProductCode = "&nbsp;" Or sProductCode = "" Then
            '    strsql = "select * from PR_DETAILS "
            '    strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
            '    strsql &= "AND PRD_COY_ID = '" & companyID & "' "
            '    strsql &= "AND PRD_PR_LINE = '" & sProductIndex & "' "
            '    strsql &= "AND PRD_PRODUCT_DESC = '" & Common.Parse(sProductDesc) & "'; "
            'Else
            '    strsql = "select * from PR_DETAILS "
            '    strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
            '    strsql &= "AND PRD_COY_ID = '" & companyID & "' "
            '    strsql &= "AND PRD_PR_LINE = '" & sProductIndex & "' "
            '    strsql &= "AND PRD_PRODUCT_CODE = '" & sProductCode & "'; "
            'End If


            Return strsql

        End Function
        Public Function getPRDetailHead(ByVal strPRNo As String, ByVal CompanyID As String) As String
            Dim strSQL As String
            'strSQL = "SELECT 
            '        B.PRM_CREATED_DATE AS DTCREATEDATE
            '        ,B.PRM_PR_NO AS PRNO
            '        ,B.PRM_URGENT AS URGENT
            '        ,C.STATUS_REMARK AS 'STATUS'
            '        ,B.PRM_REQ_NAME AS REQNAME
            '        ,B.PRM_SUBMIT_DATE AS PRDATE
            '        ,B.PRM_REQ_PHONE AS REQCON
            '        ,B.PRM_S_ATTN AS ATT
            '        ,B.PRM_INTERNAL_REMARK AS INTERNAL
            '        ,B.PRM_EXTERNAL_REMARK AS EXTERNAL
            '        ,B.PRM_CREATED_DATE AS CRDATE
            '        ,B.PRM_PR_TYPE AS PRTYPE
            '        ,B.PRM_BUYER_ID AS REQUESTOR
            '        ,B.PRM_PR_INDEX AS PRINDEX
            '        ,B.PRM_CREATED_DATE AS GSTPR
            '         FROM EPROCURE.PR_MSTR B 
            '        JOIN EPROCURE.STATUS_MSTR C ON B.PRM_PR_STATUS = C.STATUS_NO AND C.STATUS_TYPE = 'PR'
            '        WHERE 1=1 AND B.PRM_PR_NO = '" & strPRNo & "' GROUP BY PRM_PR_NO"
            strSQL = "SELECT 
                    B.PRM_CREATED_DATE AS DTCREATEDATE
                    ,B.PRM_PR_NO AS PRNO
                    ,B.PRM_URGENT AS URGENT
                    ,B.PRM_REQ_NAME AS REQNAME
                    ,B.PRM_SUBMIT_DATE AS PRDATE
                    ,B.PRM_REQ_PHONE AS REQCON
                    ,B.PRM_S_ATTN AS ATT
                    ,B.PRM_INTERNAL_REMARK AS INTERNAL
                    ,B.PRM_EXTERNAL_REMARK AS EXTERNAL
                    ,B.PRM_CREATED_DATE AS CRDATE
                    ,B.PRM_PR_TYPE AS PRTYPE
                    ,B.PRM_BUYER_ID AS REQUESTOR
                    ,B.PRM_PR_INDEX AS PRINDEX
                    ,B.PRM_CREATED_DATE AS GSTPR
                    ,(SELECT IFNULL(STATUS_REMARK, STATUS_DESC) FROM STATUS_MSTR SM WHERE SM.STATUS_NO=B.PRM_PR_STATUS AND STATUS_TYPE='PR') as STATUS
                    ,PRM_PR_STATUS AS StatusCode
                     FROM EPROCURE.PR_MSTR B LEFT JOIN COMPANY_MSTR CM ON B.PRM_S_COY_ID=CM.CM_COY_ID 
                    WHERE PRM_COY_ID  =  '" & CompanyID & "'  AND B.PRM_PR_NO = '" & strPRNo & "'"
            Return strSQL

        End Function
        Public Function getPRDetailBody(ByVal strPRNo As String) As String
            Dim strSQL As String
            strSQL = "   SELECT CDA_ATTACH_INDEX AS ATTACHINDEX
                        ,CDA_COY_ID AS COYID
                        ,CDA_DOC_NO AS DOCNO
                        ,CDA_DOC_TYPE AS DOCTYPE
                        ,CDA_HUB_FILENAME AS HUBFILENAME
                        ,CDA_ATTACH_FILENAME AS ATTACHFILENAME
                        ,CDA_FILESIZE AS FILESIZE
                        ,CDA_TYPE AS TYPE
                        ,CDA_STATUS AS STATUS
                        FROM EPROCURE.COMPANY_DOC_ATTACHMENT WHERE CDA_DOC_NO ='" & strPRNo & "'"
            Return strSQL
        End Function
        Public Function getPRDetailFoot(ByVal intPRIndex As String) As String
            Dim strSQL As String
            strSQL = "    SELECT PRA_AO_ACTION, DATE_FORMAT(PRA_ACTION_DATE, ""%d/%m/%Y %H:%i"") AS PRA_ACTION_DATE, PRA_AO_REMARK , UMA.UM_USER_NAME AS AO_NAME
                         ,UMB.UM_USER_NAME AS AAO_NAME
                         ,UMA.UM_APP_LIMIT AS AO_LIMIT
                         ,UMB.UM_APP_LIMIT AS AAO_LIMIT
                         ,CASE WHEN PRA_APPROVAL_TYPE = 1 THEN 'Approval' ELSE 'None' END AS Type
                         ,PRA_SEQ 
                         FROM EPROCURE.PR_APPROVAL PA LEFT OUTER JOIN EPROCURE.USER_MSTR UMA ON PA.PRA_AO=UMA.UM_USER_ID 
                         LEFT OUTER JOIN EPROCURE.USER_MSTR UMB ON PA.PRA_A_AO = UMB.UM_USER_ID
                         WHERE PRA_PR_INDEX ='" & intPRIndex & "'"
            Return strSQL
        End Function
        Public Function getCountData(ByVal status As String) As String
            Dim strSQL As String
            strSQL = "     SELECT  DATA.*,DATADATE.* FROM (SELECT COUNT(1) AS COUNTDATA FROM EPROCURE.PR_MSTR A WHERE PRM_PR_STATUS = '" & status & "') AS DATA
                         ,(SELECT COUNT(1) AS COUNTDATADATE FROM EPROCURE.PR_MSTR A WHERE PRM_PR_STATUS = '" & status & "'
                         AND (
                         PRM_PR_DATE BETWEEN DATE_SUB(NOW(), INTERVAL 30 DAY) and now() or
                         PRM_CREATED_DATE BETWEEN DATE_SUB(NOW(), INTERVAL 30 DAY) and now() or
                         PRM_SUBMIT_DATE BETWEEN DATE_SUB(NOW(), INTERVAL 30 DAY) and now() 
                         )) AS DATADATE"
            Return strSQL
        End Function
        Public Function insertPR(ByVal userID As String, ByVal companyID As String, ByVal dsPR As DataSet, ByRef strPRNo As String, ByVal attachmentID As String, Optional ByVal modePR As Boolean = False, Optional ByVal blnEnterpriseVersion As Boolean = True) As Integer

            Dim strPrefix As String
            Dim strsql As String
            Dim strAryQuery(0) As String
            Dim i, j As Integer
            Dim strDeptIndex As String = ""
            Dim intIncrementNo As Integer = 0

            strsql = " SET @T_NO = ''; UPDATE company_param SET cp_param_value = cp_param_value WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PR' "
            Common.Insert2Ary(strAryQuery, strsql)

            'GetLatestDocNo
            '' ''objGlobal.GetLatestDocNo("PR", strAryQuery, strPRNo, strPrefix)

            intIncrementNo = 1

            strPRNo = " (SELECT CAST(GROUP_CONCAT(CASE WHEN cp_param_name = 'prefix' THEN cp_param_value ELSE " _
                       & " CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) END " _
                       & " SEPARATOR '') AS CHAR(1000)) cp_param_value FROM (SELECT * FROM company_param WHERE CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PR' ORDER BY CP_PARAM_NAME DESC) ZZZ ) "

            strPrefix = " (SELECT cp_param_value FROM company_param WHERE CP_PARAM_NAME = 'prefix' AND CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PR') "

            strsql = "SELECT * FROM PR_MSTR WHERE PRM_PR_NO = " & strPRNo & " "
            strsql &= "AND PRM_COY_ID = '" & companyID & "' "

            If objDb.Exist(strsql) > 0 Then
                insertPR = WheelMsgNum.Duplicate
                Exit Function
            End If

            ' to check whether vendor company is inactive
            strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
            strsql &= "AND CM_COY_ID = '" & companyID & "' "
            If objDb.Exist(strsql) > 0 Then
                insertPR = -1
                Exit Function
            End If

            For Each dr As DataRow In dsPR.Tables(1).Rows
                'to check whether vendor company is being deleted
                strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED = 'Y' "
                strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPR.Tables(1).Rows(0)("VendorID")) & "' "
                If objDb.Exist(strsql) > 0 Then
                    insertPR = -2
                    Exit Function
                End If
            Next

            ' get dept index
            strsql = "SELECT CDM_DEPT_INDEX FROM USER_MSTR "
            strsql &= "INNER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
            strsql &= "AND UM_COY_ID = CDM_COY_ID "
            strsql &= "WHERE UM_USER_ID = '" & userID & "' "
            strsql &= "AND UM_COY_ID = '" & companyID & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For q As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                strDeptIndex = Common.parseNull(tDS.Tables(0).Rows(q).Item("CDM_DEPT_INDEX"))
            Next

            ' PR_MSTR table
            strsql = "INSERT INTO PR_MSTR (PRM_PR_NO, PRM_COY_ID, PRM_REQ_NAME, PRM_REQ_PHONE, "
            strsql &= "PRM_PR_DATE, PRM_CREATED_DATE, PRM_STATUS_CHANGED_BY, PRM_STATUS_CHANGED_ON, "
            strsql &= "PRM_BUYER_ID, PRM_S_COY_ID, PRM_S_ATTN, PRM_S_COY_NAME, PRM_S_ADDR_LINE1, "
            strsql &= "PRM_S_ADDR_LINE2, PRM_S_ADDR_LINE3, PRM_S_POSTCODE, PRM_S_CITY, PRM_S_STATE, "
            strsql &= "PRM_S_COUNTRY, PRM_S_PHONE, PRM_S_FAX, PRM_S_EMAIL, PRM_FREIGHT_TERMS, "
            strsql &= "PRM_PAYMENT_TYPE, PRM_SHIPMENT_TERM, PRM_SHIPMENT_MODE, PRM_CURRENCY_CODE, "
            strsql &= "PRM_EXCHANGE_RATE, PRM_PAYMENT_TERM, PRM_SHIP_VIA, PRM_INTERNAL_REMARK, "
            strsql &= "PRM_EXTERNAL_REMARK, PRM_PR_STATUS, PRM_PO_INDEX, PRM_ARCHIVE_IND, "
            strsql &= "PRM_PRINT_CUSTOM_FIELDS, PRM_PRINT_REMARK, PRM_PR_PREFIX, PRM_B_ADDR_CODE, "
            strsql &= "PRM_B_ADDR_LINE1, PRM_B_ADDR_LINE2, PRM_B_ADDR_LINE3, PRM_B_POSTCODE, "
            strsql &= "PRM_B_STATE, PRM_B_CITY, PRM_B_COUNTRY, "
            strsql &= "PRM_DUP_FROM, PRM_EXTERNAL_IND, PRM_REFERENCE_NO, PRM_CONSOLIDATOR, PRM_GST, "
            strsql &= "PRM_PR_COST, PRM_RFQ_INDEX, PRM_DEPT_INDEX, PRM_QUOTATION_NO, PRM_PR_TYPE, PRM_URGENT) SELECT "
            strsql &= "" & strPRNo & ", " ' PR_No
            strsql &= "'" & companyID & "', " ' COY_ID

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) <> "" Then
                strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqName")) & "', " ' REQ_NAME
            Else
                Dim PR_Req_Name As String = objDb.GetVal("SELECT IFNULL(UM_USER_NAME,'') FROM USER_MSTR WHERE UM_USER_ID = '" & userID & "' AND UM_COY_ID = '" & companyID & "'")
                strsql &= "'" & Common.Parse(PR_Req_Name) & "', " ' REQ_NAME
            End If

            If Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) <> "" Then
                strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ReqPhone")) & "', " 'REQ_PHONE
            Else
                Dim PR_Req_Tel As String = objDb.GetVal("SELECT IFNULL(UM_TEL_NO,'') FROM USER_MSTR WHERE UM_USER_ID = '" & userID & "' AND UM_COY_ID = '" & companyID & "'")
                strsql &= "'" & Common.Parse(PR_Req_Tel) & "', " 'REQ_PHONE
            End If

            strsql &= "NULL, " 'Common.ConvertDate(Now) & ", "  'PR_DATE
            strsql &= Common.ConvertDate(Now) & ", "  'CREATED_DATE
            strsql &= "'" & userID & "', " 'STATUS_CHANGED_BY
            strsql &= Common.ConvertDate(Now) & ", " 'STATUS_CHANGED_ON
            strsql &= "'" & userID & "', " 'BUYER_ID

            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("VendorID")) & "', " 'S_COY_ID
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("Attn")) & "', " 'S_ATTN
            strsql &= "CM_COY_NAME, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, "
            strsql &= "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, " & If(Common.Parse(dsPR.Tables(0).Rows(0)("PRSEmail")) <> "", "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PRSEmail")) & "'", "CM_EMAIL") & ", "

            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("FreightTerms")) & "', " ' FREIGHT_TERMS
            strsql &= "CM_PAYMENT_METHOD, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("PaymentType")) & "', " ' PAYMENT_TYPE
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ShipmentTerm")) & "', " ' SHIPMENT_TERM
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ShipmentMode")) & "', " ' SHIPMENT_MODE
            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("CurrencyCode")) & "', " ' CURRENCY_CODE
            strsql &= "NULL, " 'Common.Parse(dsPR.Tables(0).Rows(0)("ExchangeRate")) & ", "  ' EXCHANGE_RATE
            strsql &= "CM_PAYMENT_TERM, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("PaymentTerm")) & "', " ' PAYMENT_TERM
            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("ShipVia")) & "', " ' SHIP_VIA
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("InternalRemark")) & "', " ' INTERNAL_REMARK
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("ExternalRemark")) & "', " 'EXTERNAL_REMARK
            strsql &= "'" & PRStatus.Draft & "', " ' PR_STATUS
            strsql &= "NULL, '', " ' PO_INDEX, ARCHIVE_IND
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintCustom")) & "', " 'PRINT_CUSTOM_FIELDS
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("PrintRemark")) & "', " 'PRINT_REMARK
            strsql &= "" & strPrefix & ", "
            strsql &= "'" & Common.Parse(dsPR.Tables(0).Rows(0)("BillAddrCode")) & "', " ' B_ADDR_CODE
            strsql &= "'" & Common.Parse(Common.parseNull(dsPR.Tables(0).Rows(0)("BillAddrLine1"))) & "', " ' B_ADDR_LINE1
            strsql &= "'" & Common.Parse(Common.parseNull(dsPR.Tables(0).Rows(0)("BillAddrLine2"))) & "', " ' B_ADDR_LINE2
            strsql &= "'" & Common.Parse(Common.parseNull(dsPR.Tables(0).Rows(0)("BillAddrLine3"))) & "', " ' B_ADDR_LINE3
            strsql &= "'" & Common.Parse(Common.parseNull(dsPR.Tables(0).Rows(0)("BillAddrPostCode"))) & "', " ' B_POSTCODE
            strsql &= "'" & Common.Parse(Common.parseNull(dsPR.Tables(0).Rows(0)("BillAddrState"))) & "', " ' B_STATE
            strsql &= "'" & Common.Parse(Common.parseNull(dsPR.Tables(0).Rows(0)("BillAddrCity"))) & "', " ' B_CITY
            strsql &= "'" & Common.Parse(Common.parseNull(dsPR.Tables(0).Rows(0)("BillAddrCountry"))) & "', " ' B_COUNTRY
            strsql &= "'', '', '', '', " ' DUP_FROM, EXTERNAL_IND, REFERENCE_NO, PRM_CONSOLIDATOR
            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("GST")) & "', " ' PRM_GST
            strsql &= dsPR.Tables(0).Rows(0)("PRCost") & ", " ' PRM_PR_COST
            strsql &= "NULL, " 'dsPR.Tables(0).Rows(0)("RfqIndex") & ", " ' PRM_RFQ_INDEX

            If strDeptIndex = "" Then ' PRM_DEPT_INDEX
                strsql &= "NULL, "
            Else
                strsql &= strDeptIndex & ", "
            End If
            strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(0).Rows(0)("QuoNo")) & "' " ' PRM_QUOTATION_NO
            strsql &= "'" & If(Not IsDBNull(dsPR.Tables(0).Rows(0)("PRType")), Common.Parse(dsPR.Tables(0).Rows(0)("PRType")), String.Empty) & "'"
            strsql &= ", '" & If(Not IsDBNull(dsPR.Tables(0).Rows(0)("Urgent")), Common.Parse(dsPR.Tables(0).Rows(0)("Urgent")), String.Empty) & "' "
            strsql &= "FROM COMPANY_MSTR " ', USER_MSTR "
            strsql &= "WHERE CM_COY_ID = '" & companyID & "' "
            strsql &= " AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "

            Common.Insert2Ary(strAryQuery, strsql)

            ' PR_DETAILS table
            For i = 0 To dsPR.Tables(1).Rows.Count - 1
                strsql = "INSERT INTO PR_DETAILS (PRD_PR_NO, PRD_COY_ID, PRD_PR_LINE, PRD_PRODUCT_CODE, "
                strsql &= "PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_UOM, PRD_ORDERED_QTY, PRD_UNIT_COST, "
                strsql &= "PRD_ETD, PRD_WARRANTY_TERMS, PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY, PRD_REMARK, PRD_GST, PRD_GST_RATE, PRD_GST_INPUT_TAX_CODE, PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, "
                strsql &= "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, "

                'Jules 2018.05.04 - PAMB Scrum 2 - Added Gift and Analysis Codes.
                If companyID.ToString.ToLower = "pamb" Then
                    If blnEnterpriseVersion = True Then
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_ASSET_GROUP, PRD_GIFT, PRD_FUND_TYPE, PRD_PERSON_CODE, PRD_PROJECT_CODE) SELECT "
                    Else
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_GIFT, PRD_FUND_TYPE, PRD_PERSON_CODE, PRD_PROJECT_CODE) SELECT "
                    End If
                Else
                    If blnEnterpriseVersion = True Then
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE, PRD_ASSET_GROUP) SELECT "
                    Else
                        strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE) SELECT "
                    End If
                End If
                'End modification.



                strsql &= "" & strPRNo & ", " ' PR_No
                strsql &= "'" & companyID & "', " ' COY_ID
                strsql &= Common.Parse(dsPR.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("UOM")) & "', " ' UOM
                strsql &= dsPR.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
                strsql &= dsPR.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
                strsql &= dsPR.Tables(1).Rows(i)("ETD") & ", " ' ETD
                strsql &= dsPR.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
                strsql &= "1, 1, " 'PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Remark")) & "', " ' REMARK
                strsql &= dsPR.Tables(1).Rows(i)("GST") & ", " ' GST
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTRate")) & "', " ' GST_RATE
                'Stage 3 (Enhancement) (GST-0010) - 13/07/2015 - CH
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GSTTaxCode")) & "', " ' GST Input Tax Code

                strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
                strsql &= "AM_STATE, AM_COUNTRY, "

                If Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) <> "" Then '_Yap
                    strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("AcctIndex")) & "', " ' ACCT_INDEX
                Else
                    strsql &= "null, " ' ACCT_INDEX
                End If

                ' strsql &= "NULL, " ' ACCT_INDEX '_Yap
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorItemCode")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Source")) & "', " ' SOURCE

                If Common.Parse(dsPR.Tables(1).Rows(i)("CDGroup")) = "" Then
                    strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
                Else
                    strsql &= dsPR.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
                End If

                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE
                'strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("SupplierCompanyId")) & "' " ' PRD_S_COY_ID
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("VendorID")) & "', " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
                strsql &= "NULL, " '"'" & Common.Parse(dsPR.Tables(1).Rows(i)("TaxCode")) & "' "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Commodity")) & "', "
                strsql &= "'" & Common.Parse(dsPR.Tables(1).Rows(i)("Currency")) & "' "

                If blnEnterpriseVersion = True Then
                    strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("AssetGroup")) & "' "
                End If

                'Jules 2018.05.04 - PAMB Scrum 2
                If companyID.ToString.ToLower = "pamb" Then
                    strsql &= ", '" & Common.Parse(dsPR.Tables(1).Rows(i)("Gift")) & "', '" & Common.Parse(dsPR.Tables(1).Rows(i)("FundType")) & "', '" & Common.Parse(dsPR.Tables(1).Rows(i)("PersonCode")) & "', '" & Common.Parse(dsPR.Tables(1).Rows(i)("ProjectCode")) & "' "
                End If
                'End modification.

                strsql &= "FROM ADDRESS_MSTR "
                strsql &= "WHERE AM_ADDR_TYPE = 'D' "
                strsql &= "AND AM_COY_ID = '" & companyID & "' "
                strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPR.Tables(1).Rows(i)("DeliveryAddr")) & "' "
                Common.Insert2Ary(strAryQuery, strsql)
            Next

            If modePR = True Then
                ' PR_CUSTOM_FIELD_MSTR table 
                For i = 0 To dsPR.Tables(2).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
                    strsql &= "(SELECT MAX(PRM_PR_Index) FROM PR_MSTR), "
                    strsql &= dsPR.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(2).Rows(i)("FieldName")) & "', " ' FIELD_NAME
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next

                ' PR_CUSTOM_FIELD_DETAILS table 
                For i = 0 To dsPR.Tables(3).Rows.Count - 1
                    strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
                    strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
                    strsql &= "(SELECT MAX(PRM_PR_Index) FROM PR_MSTR), "
                    strsql &= dsPR.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
                    strsql &= dsPR.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
                    strsql &= "'" & Common.Parse(dsPR.Tables(3).Rows(i)("FieldValue")) & "', " ' FIELD_VALUE
                    strsql &= "'PR' "
                    Common.Insert2Ary(strAryQuery, strsql)
                Next
            End If

            'no workarround for attachment: will be replaced by plxside - ghesa 07/04/2021
            If Not String.IsNullOrEmpty(attachmentID) Then
                'update COMPANY_DOC_ATTACHMENT table
                strsql = "UPDATE COMPANY_DOC_ATTACHMENT SET "
                strsql &= "CDA_DOC_NO = " & strPRNo & " "
                strsql &= "WHERE CDA_COY_ID = '" & companyID & "' "
                strsql &= "AND CDA_DOC_TYPE = 'PR' "
                strsql &= "AND CDA_DOC_NO IN (" & attachmentID & ") " 'r
                Common.Insert2Ary(strAryQuery, strsql)
            End If


            strsql = " SET @T_NO = " & strPRNo & "; "
            Common.Insert2Ary(strAryQuery, strsql)

            strsql = "UPDATE company_param SET cp_param_value = CONCAT(REPEAT('0',LENGTH(cp_param_value) - LENGTH(cp_param_value + '" & intIncrementNo & "')), (cp_param_value + '" & intIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & companyID & "' AND CP_PARAM_TYPE = 'PR' "
            Common.Insert2Ary(strAryQuery, strsql)

            'If objDb.BatchExecute(strAryQuery) Then
            Dim strTPRNo As String = ""
            If objDb.BatchExecuteNew(strAryQuery, , strTPRNo, "T_NO") Then
                strPRNo = strTPRNo
                insertPR = WheelMsgNum.Save
            Else
                insertPR = WheelMsgNum.NotSave
            End If
        End Function

        '     Public Function getRPVendorItemList(ByVal strPR As String) As DataSet
        '         Dim strsql As String
        '         Dim ds As New DataSet
        '         strsql = "SELECT PRD_VENDOR_ITEM_CODE FROM PR_DETAILS "
        '         strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
        '         strsql &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         ds = objDb.FillDs(strsql)
        '         getRPVendorItemList = ds
        '     End Function

        '     Public Function updatePRCost(ByVal strPR As String) As Double
        '         Dim strsql As String
        '         strsql = "UPDATE PR_MSTR SET PRM_PR_COST = (SELECT ISNULL(SUM(PRD_UNIT_COST * PRD_ORDERED_QTY),0) FROM PR_DETAILS WHERE PRD_PR_NO = '" & strPR & "' ) "
        '         strsql &= "WHERE PRM_PR_NO = '" & strPR & "' AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

        '         If objDb.Execute(strsql) Then
        '             strsql = "SELECT PRM_PR_COST FROM PR_MSTR "
        '             strsql &= "WHERE PRM_PR_NO = '" & strPR & "' AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             updatePRCost = CDbl(objDb.GetVal(strsql))
        '         End If
        '     End Function

        '     Public Function insertPRItem(ByVal dtItem As DataTable, ByRef strProductExist As String, ByRef strProductAdded As String)
        '         Dim strsql As String
        '         Dim strExist As String
        '         Dim strAryAdd(8) As String
        '         Dim strAryQuery(0) As String
        '         Dim blnAdd As Boolean = False
        '         Dim blnInsert As Boolean = False
        '         Dim i, j As Integer
        '         strProductExist = ""
        '         strProductAdded = ""

        '         strsql = "SELECT AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, AM_STATE, AM_COUNTRY "
        '         strsql &= "FROM ADDRESS_MSTR WHERE AM_ADDR_TYPE = 'D' AND AM_ADDR_CODE IN "
        '         strsql &= "(SELECT UDA_ADDR_CODE FROM USERS_DEFAULT_ADDR "
        '         strsql &= "WHERE UDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         strsql &= "AND UDA_USER_ID = '" & HttpContext.Current.Session("UserId") & "')"
        '         Dim tDS As DataSet = objDb.FillDs(strsql)

        '         For q As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '             blnAdd = True
        '             strAryAdd(0) = tDS.Tables(0).Rows(q).Item("AM_ADDR_CODE")
        '             strAryAdd(1) = tDS.Tables(0).Rows(q).Item("AM_ADDR_LINE1")
        '             strAryAdd(2) = tDS.Tables(0).Rows(q).Item("AM_ADDR_LINE2")
        '             strAryAdd(3) = tDS.Tables(0).Rows(q).Item("AM_ADDR_LINE3")
        '             strAryAdd(4) = tDS.Tables(0).Rows(q).Item("AM_POSTCODE")
        '             strAryAdd(5) = tDS.Tables(0).Rows(q).Item("AM_CITY")
        '             strAryAdd(6) = tDS.Tables(0).Rows(q).Item("AM_STATE")
        '             strAryAdd(7) = tDS.Tables(0).Rows(q).Item("AM_COUNTRY")
        '         Next

        '         ' Dim blnMultipleSupplier As Boolean = False
        '         '---------------New Code Added for CM_MULTI_PO on 04/July/2007  Start------------
        '         Dim blnMultipleSupplier As Boolean
        '         Dim Strmulti As String
        '         Dim strSql1 As String

        '         Dim dd As New System.Web.UI.WebControls.DropDownList
        '         Dim dds As DataTable
        '         strSql1 = "Select  CM_MULTI_PO FROM COMPANY_MSTR where CM_COY_ID='" & HttpContext.Current.Session("CompanyId") & "'"
        '         tDS = objDb.FillDs(strSql1)

        '         If tDS.Tables(0).Rows.Count > 0 Then
        '             Strmulti = tDS.Tables(0).Rows(0).Item("CM_MULTI_PO")
        '         End If
        '         If Strmulti = "Y" Then
        '             blnMultipleSupplier = True
        '         Else
        '             blnMultipleSupplier = False
        '         End If
        '         '---------------New Code Added for CM_MULTI_PO on 04/July/2007  END------------   

        '         For j = 0 To dtItem.Rows.Count - 1
        '             ' ai chu modified on 22/11/2005
        '             ' no need to check same product code exists 
        '             ' requested by user since same product code may need to be delivered to different location
        '             strExist = "SELECT PRD_PR_NO FROM PR_DETAILS "
        '             strExist &= "WHERE PRD_PR_NO = '" & Common.Parse(dtItem.Rows(j)("PRId")) & "' "
        '             strExist &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             strExist &= "AND PRD_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(j)("ProductCode")) & "' "

        '             If objDb.Exist(strExist) > 0 Then
        '                 strProductExist &= Common.Parse(dtItem.Rows(j)("ProductCode")) & ","
        '             Else
        '                 strExist = "SELECT PRD_PR_NO FROM PR_DETAILS "
        '                 strExist &= "WHERE PRD_PR_NO = '" & Common.Parse(dtItem.Rows(j)("PRId")) & "' "
        '                 strExist &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '                 'strExist &= "AND PRD_PRODUCT_CODE = '" & Common.Parse(dtItem.Rows(j)("ProductCode")) & "' "
        '                 strExist &= "AND PRD_S_COY_ID <> '" & Common.Parse(dtItem.Rows(j)("SupplierCompanyId")) & "' "
        '                 strExist &= "AND PRD_S_COY_ID IS NOT NULL "

        '                 blnMultipleSupplier = objDb.Exist(strExist) > 0

        '                 ' Remove Supplier id from PR master

        '                 If blnMultipleSupplier Then
        '                     strsql = "UPDATE PR_MSTR SET PRM_S_COY_ID = '',"
        '                     strsql &= " PRM_S_COY_NAME = '',"
        '                     strsql &= " PRM_S_ADDR_LINE1 = '',"
        '                     strsql &= " PRM_S_ADDR_LINE2 = '',"
        '                     strsql &= " PRM_S_ADDR_LINE3 = '',"
        '                     strsql &= " PRM_S_POSTCODE = '',"
        '                     strsql &= " PRM_S_CITY = '',"
        '                     strsql &= " PRM_S_STATE = '',"
        '                     strsql &= " PRM_S_COUNTRY = '',"
        '                     strsql &= " PRM_S_PHONE = '',"
        '                     strsql &= " PRM_S_FAX = '',"
        '                     strsql &= " PRM_S_EMAIL = ''"
        '                     strsql &= " WHERE "
        '                     strsql &= "PRM_PR_NO = '" & Common.Parse(dtItem.Rows(j)("PRId")) & "' " ' PR_No
        '                     strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " ' COY_ID

        '                     Common.Insert2Ary(strAryQuery, strsql)
        '                 End If

        '                 blnInsert = True
        '                 strProductAdded &= Common.Parse(dtItem.Rows(j)("ProductCode")) & ","
        '                 strsql = "INSERT INTO PR_DETAILS (PRD_PR_NO, PRD_COY_ID, PRD_PR_LINE, PRD_PRODUCT_CODE, "
        '                 strsql &= "PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_UOM, PRD_ORDERED_QTY, PRD_UNIT_COST, "
        '                 strsql &= "PRD_ETD, PRD_WARRANTY_TERMS, PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY, PRD_REMARK, PRD_GST, PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, "
        '                 strsql &= "PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, "
        '                 'Michelle (16/6/2010) - To include the Tax Code
        '                 'strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_S_COY_ID) SELECT "
        '                 strsql &= "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE,PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_S_COY_ID, PRD_B_TAX_CODE) SELECT "
        '                 strsql &= "'" & Common.Parse(dtItem.Rows(j)("PRId")) & "', "
        '                 strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
        '                 strsql &= "(SELECT ISNULL(MAX(PRD_PR_LINE), 0) + 1 FROM PR_DETAILS WHERE PRD_PR_NO = '" & Common.Parse(dtItem.Rows(j)("PRId")) & "'), "
        '                 strsql &= "SC_PRODUCT_CODE, SC_VENDOR_ITEM_CODE, SC_PRODUCT_DESC, SC_UOM, "
        '                 strsql &= dtItem.Rows(j)("Qty") & ", "
        '                 strsql &= dtItem.Rows(j)("Price") & ", "
        '                 strsql &= "0, 0, 1, 1, "
        '                 strsql &= "'" & Common.Parse(dtItem.Rows(j)("Remark")) & "', D.CODE_VALUE, "

        '                 If blnAdd Then ' default delivery address exists
        '                     For i = 0 To 7
        '                         strsql &= "'" & Common.Parse(strAryAdd(i)) & "', " 'Michelle (CR0027) - To cater for single quote
        '                     Next
        '                 Else
        '                     strsql &= "NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, "
        '                 End If

        '                 strsql &= "NULL, '', CBC_B_ITEM_CODE, CBC_B_CATEGORY_CODE, CBC_B_GL_CODE, SC_SOURCE, SC_CD_GROUP_INDEX, SC_S_COY_ID, CBC_B_TAX_CODE "
        '                 strsql &= "FROM SHOPPING_CART "
        '                 strsql &= "LEFT JOIN COMPANY_B_ITEM_CODE ON SC_PRODUCT_CODE = CBC_PRODUCT_CODE AND CBC_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '                 strsql &= "LEFT JOIN PRODUCT_MSTR ON PM_VENDOR_ITEM_CODE = SC_VENDOR_ITEM_CODE AND PM_PRODUCT_CODE = SC_PRODUCT_CODE AND PM_DELETED <> 'Y' "
        '                 strsql &= "LEFT JOIN CODE_MSTR AS D ON PM_GST_CODE = D.CODE_ABBR "
        '                 strsql &= "WHERE SC_CART_INDEX = " & dtItem.Rows(j)("CartIndex")
        '                 Common.Insert2Ary(strAryQuery, strsql)

        '                 strsql = "DELETE FROM SHOPPING_CART "
        '                 strsql &= "WHERE SC_CART_INDEX = " & dtItem.Rows(j)("CartIndex")
        '                 Common.Insert2Ary(strAryQuery, strsql)
        '             End If
        '         Next

        '         If blnInsert Then
        '             If strAryQuery(0) <> String.Empty Then
        '                 If objDb.BatchExecute(strAryQuery) Then
        '                     updatePRCost(dtItem.Rows(0)("PRId"))
        '                 End If
        '             End If
        '         End If
        '     End Function

        '     Public Function deletePRItemSQL(ByVal strPR As String, ByVal intLine As Integer) As String
        '         Dim strsql As String
        '         ' delete from PR_DETAILS table
        '         strsql = "DELETE FROM PR_DETAILS "
        '         strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
        '         strsql &= "AND PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         strsql &= "AND PRD_PR_LINE = " & intLine & "; "

        '         ' delete from PR_CUSTOM_FIELD_DETAILS table
        '         strsql &= "DELETE FROM PR_CUSTOM_FIELD_DETAILS "
        '         strsql &= "WHERE PCD_TYPE = 'PR' AND PCD_PR_LINE = " & intLine & " "
        '         strsql &= "AND PCD_PR_INDEX IN "
        '         strsql &= "(SELECT PRM_PR_INDEX FROM PR_MSTR "
        '         strsql &= "WHERE PRM_PR_NO = '" & strPR & "'); "

        '         deletePRItemSQL = strsql
        '     End Function

        '     Public Function getVendorDetail(ByVal strVendor As String) As DataSet
        '         Dim strSql As String
        '         Dim dsGST As New DataSet
        '         strSql = "SELECT CM_COY_NAME, CM_TAX_CALC_BY, CV_Payment_Term, CV_Payment_Method, CM_CURRENCY_CODE, CV_BILLING_METHOD FROM COMPANY_MSTR "
        '         strSql &= "LEFT JOIN COMPANY_VENDOR ON CM_COY_ID = CV_S_COY_ID AND CV_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         strSql &= "WHERE CM_COY_ID = '" & strVendor & "' "
        '         strSql &= "AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "
        '         dsGST = objDb.FillDs(strSql)
        '         getVendorDetail = dsGST
        '     End Function

        '     Public Function getApprovalType() As String
        '         Dim strsql As String
        '         Dim ds As New DataSet
        '         strsql = "SELECT CS_FLAG_VALUE FROM COMPANY_SETTING "
        '         strsql &= "WHERE CS_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         strsql &= "AND CS_FLAG_NAME = 'Approval Rule' "
        '         ds = objDb.FillDs(strsql)
        '         If ds.Tables(0).Rows.Count > 0 Then
        '             getApprovalType = ds.Tables(0).Rows(0)(0)
        '         Else
        '             getApprovalType = "B+C"
        '         End If
        '     End Function

        'Public Function getAppovalList(ByVal strApprovalType As String, ByVal dblPRTotal As Double) As DataSet
        'Michelle (21/2/2011) - To also cater for PO
        Public Function getAppovalListstr(ByVal userID As String, ByVal companyID As String, ByVal strApprovalType As String, ByVal dblPRTotal As Double, Optional ByVal strType As String = "", Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal strDept As String = "", Optional getStrSQL As Boolean = False) As String
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT AGA_GRP_INDEX, AGM_GRP_NAME, ISNULL(AGM_CONSOLIDATOR) AS AGM_CONSOLIDATOR "

            If strApprovalType = "A" Then
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN USER_MSTR ON UM_USER_ID = AGA_AO " 'AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX "
                strsql &= "AND AGB_GRP_INDEX = AGM_GRP_INDEX AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                If strType = "PO" Then
                    strsql &= " GROUP BY AGA_GRP_INDEX, AGM_GRP_NAME, UM_PO_APP_LIMIT, AGM_CONSOLIDATOR "
                Else
                    strsql &= " GROUP BY AGA_GRP_INDEX, AGM_GRP_NAME, UM_APP_LIMIT, AGM_CONSOLIDATOR "
                End If
                strsql &= "HAVING AGA_GRP_INDEX IN ( "
                strsql &= "SELECT DISTINCT AGA_GRP_INDEX "
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN USER_MSTR ON UM_USER_ID = AGA_AO " 'AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX  AND AGB_GRP_INDEX = AGM_GRP_INDEX AND UM_COY_ID = AGM_COY_ID AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                strsql &= "AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGM_TYPE = '" & strType & "' "

                'Jules 2018.11.13 - Disregard department checking
                'If strDept <> "" Then
                '    strsql &= " AND AGM_DEPT_CODE IN (" & strDept & ") "
                'End If
                'End modification.

                strsql &= "GROUP BY AGA_GRP_INDEX "
                If strType = "PO" Then
                    strsql &= " , UM_PO_APP_LIMIT "
                Else
                    strsql &= " , UM_APP_LIMIT "
                End If
                'If strFTN <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    If strType = "PO" Then
                        strsql &= " HAVING MAX(UM_PO_APP_LIMIT) >= " & dblPRTotal & " "
                    Else
                        strsql &= " HAVING MAX(UM_APP_LIMIT) >= " & dblPRTotal & " "
                    End If
                End If

                ' strsql &= " AND COUNT(AGA_GRP_INDEX) = 1)"
                strsql &= "UNION "
                strsql &= "SELECT DISTINCT AGA_GRP_INDEX "
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN USER_MSTR ON UM_USER_ID = AGA_AO " 'AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX  AND AGB_GRP_INDEX = AGM_GRP_INDEX AND UM_COY_ID = AGM_COY_ID AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                strsql &= "AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGM_TYPE = '" & strType & "' "

                'Jules 2018.11.13 - Disregard department checking
                'If strDept <> "" Then
                '    strsql &= " AND AGM_DEPT_CODE IN (" & strDept & ") "
                'End If
                'End modification.

                ''If strFTN <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    strsql &= "GROUP BY AGA_GRP_INDEX "
                    If strType = "PO" Then
                        strsql &= " HAVING MIN(UM_PO_APP_LIMIT) >= " & dblPRTotal & " "
                    Else
                        strsql &= " HAVING MIN(UM_APP_LIMIT) >= " & dblPRTotal & " "
                    End If

                    strsql &= " AND COUNT(AGA_GRP_INDEX) = 1) "
                Else
                    strsql &= " GROUP BY AGA_GRP_INDEX  "
                    strsql &= " HAVING COUNT(AGA_GRP_INDEX) = 1) "
                End If
            Else
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX "
                strsql &= "AND AGB_GRP_INDEX = AGM_GRP_INDEX AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                strsql &= "AND AGM_TYPE = '" & strType & "' "

                'Jules 2018.11.13 - Disregard department checking
                'If strDept <> "" Then
                '    strsql &= " AND AGM_DEPT_CODE IN (" & strDept & ") "
                'End If
                'End modification.

            End If


            Return strsql
        End Function
        Public Function getAppovalList(ByVal userID As String, ByVal companyID As String, ByVal strApprovalType As String, ByVal dblPRTotal As Double, Optional ByVal strType As String = "PR", Optional ByVal blnEnterpriseVersion As Boolean = True, Optional ByVal strDept As String = "", Optional getStrSQL As Boolean = False) As Result
            Dim strsql As String
            Dim ds As New DataSet

            strsql = "SELECT DISTINCT AGA_GRP_INDEX, AGM_GRP_NAME, ISNULL(AGM_CONSOLIDATOR, '') AS AGM_CONSOLIDATOR "

            If strApprovalType = "A" Then
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN USER_MSTR ON UM_USER_ID = AGA_AO " 'AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX "
                strsql &= "AND AGB_GRP_INDEX = AGM_GRP_INDEX AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                If strType = "PO" Then
                    strsql &= " GROUP BY AGA_GRP_INDEX, AGM_GRP_NAME, UM_PO_APP_LIMIT, AGM_CONSOLIDATOR "
                Else
                    strsql &= " GROUP BY AGA_GRP_INDEX, AGM_GRP_NAME, UM_APP_LIMIT, AGM_CONSOLIDATOR "
                End If
                strsql &= "HAVING AGA_GRP_INDEX IN ( "
                strsql &= "SELECT DISTINCT AGA_GRP_INDEX "
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN USER_MSTR ON UM_USER_ID = AGA_AO " 'AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX  AND AGB_GRP_INDEX = AGM_GRP_INDEX AND UM_COY_ID = AGM_COY_ID AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                strsql &= "AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGM_TYPE = '" & strType & "' "

                'Jules 2018.11.13 - Disregard department checking
                'If strDept <> "" Then
                '    strsql &= " AND AGM_DEPT_CODE IN (" & strDept & ") "
                'End If
                'End modification.

                strsql &= "GROUP BY AGA_GRP_INDEX "
                If strType = "PO" Then
                    strsql &= " , UM_PO_APP_LIMIT "
                Else
                    strsql &= " , UM_APP_LIMIT "
                End If
                'If strFTN <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    If strType = "PO" Then
                        strsql &= " HAVING MAX(UM_PO_APP_LIMIT) >= " & dblPRTotal & " "
                    Else
                        strsql &= " HAVING MAX(UM_APP_LIMIT) >= " & dblPRTotal & " "
                    End If
                End If

                strsql &= "UNION "
                strsql &= "SELECT DISTINCT AGA_GRP_INDEX "
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN USER_MSTR ON UM_USER_ID = AGA_AO " 'AND UM_STATUS = 'A' AND UM_DELETED <> 'Y' "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX  AND AGB_GRP_INDEX = AGM_GRP_INDEX AND UM_COY_ID = AGM_COY_ID AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                strsql &= "AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGM_TYPE = '" & strType & "' "

                'Jules 2018.11.13 - Disregard department checking
                'If strDept <> "" Then
                '    strsql &= " AND AGM_DEPT_CODE IN (" & strDept & ") "
                'End If
                'End modification.

                'If strFTN <> "FTN" Then
                If blnEnterpriseVersion = True Then
                    strsql &= "GROUP BY AGA_GRP_INDEX "
                    If strType = "PO" Then
                        strsql &= " HAVING MIN(UM_PO_APP_LIMIT) >= " & dblPRTotal & " "
                    Else
                        strsql &= " HAVING MIN(UM_APP_LIMIT) >= " & dblPRTotal & " "
                    End If

                    strsql &= " AND COUNT(AGA_GRP_INDEX) = 1) "
                Else
                    strsql &= " GROUP BY AGA_GRP_INDEX  "
                    strsql &= " HAVING COUNT(AGA_GRP_INDEX) = 1) "
                End If
            Else
                strsql &= "FROM APPROVAL_GRP_AO "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGA_GRP_INDEX = AGM_GRP_INDEX "
                strsql &= "AND AGB_GRP_INDEX = AGM_GRP_INDEX AND AGM_TYPE<>'INV' "
                strsql &= "WHERE AGB_BUYER = '" & userID & "' "
                strsql &= "AND AGM_TYPE = '" & strType & "' "

                'Jules 2018.11.13 - Disregard department checking
                'If strDept <> "" Then
                '    strsql &= " AND AGM_DEPT_CODE IN (" & strDept & ") "
                'End If
                'End modification.

            End If

            Dim result As Result = New Result
            If Not getStrSQL Then
                ds = objDb.FillDs(strsql)
            Else
                ds = Nothing
            End If
            result.strSql = strsql
            result.ds = ds
            getAppovalList = result
        End Function
        'Michelle (19/11/2010) - To cater for PO 
        'Public Function getAOList(ByVal intIndex As Integer) As DataSet
        Public Function getAOList(ByVal intIndex As Integer, ByVal UserID As String, ByVal CompanyID As String, Optional ByVal strType As String = "") As String
            Dim strsql As String = ""
            Dim ds As New DataSet
            'Zulham 03082018 - PAMB
            strsql &= "SELECT DISTINCT AGA_SEQ as Seq, AGA_AO as AO, AGA_GRP_INDEX as GrpIndex, ISNULL(AGA_A_AO) AS AAO, AGA_RELIEF_IND AS relief , B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME , CASE WHEN B.UM_APP_LIMIT >= 100000 THEN 'None' ELSE 'Approval'  end as Type "
            If strType = "PO" Then
                strsql &= " , B.UM_PO_APP_LIMIT AS UM_APP_LIMIT "
            Else
                strsql &= " , B.UM_APP_LIMIT "
            End If
            strsql &= ", 'AO' AS AOSource " 'Jules 2018.11.22
            strsql &= "FROM APPROVAL_GRP_AO "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGA_AO AND B.UM_COY_ID = '" & CompanyID & "' " 'AND B.UM_DELETED <> 'Y' AND B.UM_STATUS = 'A'
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGA_A_AO AND C.UM_COY_ID = '" & CompanyID & "' " 'AND C.UM_DELETED <> 'Y' AND C.UM_STATUS = 'A'
            strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX "
            strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
            strsql &= "WHERE AGA_GRP_INDEX = " & intIndex & " "
            strsql &= "AND AGB_BUYER = '" & UserID & "' "
            'Zulham 03082018 - PAMB
            If strType = "PO" Or strType = "PR" Then
                strsql &= " AND AGM_TYPE = '" & strType & "' "

            Else
                strsql &= " AND AGM_TYPE <> '" & strType & "' "
            End If

            'Zulham 03082018 - PAMB
            If strType = "PO" Or strType = "PR" Then
                strsql &= "UNION ALL "
                strsql &= "(SELECT DISTINCT AGPAO_SEQ AS seq, AGPAO_AO as AO, AGPAO_GRP_INDEX as GrpIndex,   ISNULL(AGPAO_A_AO) AS AAO, AGPAO_RELIEF_IND as relief, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME,   "
                strsql &= "CASE WHEN B.UM_APP_LIMIT >= 100000 THEN 'None' ELSE 'Approval'  end as Type,   "
                strsql &= "B.UM_PO_APP_LIMIT AS UM_APP_LIMIT "
                strsql &= ", 'PAO' AS AOSource " 'Jules 2018.11.22
                strsql &= "FROM APPROVAL_GRP_PAO "
                strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGPAO_AO AND B.UM_COY_ID = '" & CompanyID & "'"
                strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGPAO_A_AO AND C.UM_COY_ID = '" & CompanyID & "' "
                strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGPAO_GRP_INDEX "
                strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGPAO_GRP_INDEX = AGB_GRP_INDEX "
                strsql &= "WHERE AGPAO_GRP_INDEX = " & intIndex & " "
                strsql &= "AND AGB_BUYER = '" & UserID & "' "
                strsql &= "AND AGM_TYPE = '" & strType & "' "
                strsql &= "ORDER BY B.UM_PO_APP_LIMIT,AGPAO_SEQ)"
            End If

            ' ai chu add 
            ' to get consolidator name
            'strsql &= "SELECT AGM_CONSOLIDATOR, UM_USER_NAME "
            'strsql &= "FROM APPROVAL_GRP_MSTR "
            'strsql &= "INNER JOIN USER_MSTR ON AGM_CONSOLIDATOR = UM_USER_ID AND AGM_COY_ID = UM_COY_ID AND AGM_TYPE<>'INV' "
            'strsql &= "WHERE AGM_GRP_INDEX = " & intIndex & " "
            'If strType = "PO" Then
            '    strsql &= " AND AGM_TYPE = '" & strType & "' "
            'Else
            '    strsql &= " AND AGM_TYPE <> '" & strType & "' "
            'End If

            'ds = objDb.FillDs(strsql)
            'getAOList = ds
            Return strsql
        End Function

        '     'Michelle (19/11/2010) - To cater for PO 
        '     'Public Function getAOList(ByVal intIndex As Integer) As DataSet
        '     Public Function getAOList(ByVal intIndex As Integer, Optional ByVal strType As String = "") As DataSet
        '         Dim strsql As String
        '         Dim ds As New DataSet
        ''Zulham 03082018 - PAMB
        '         strsql = "(SELECT DISTINCT AGA_SEQ, AGA_AO, ISNULL(AGA_A_AO, '') AS AGA_A_AO, AGA_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME "
        '         If strType = "PO" Then
        '             strsql &= " , B.UM_PO_APP_LIMIT AS UM_APP_LIMIT "
        '         Else
        '             strsql &= " , B.UM_APP_LIMIT "
        '         End If
        '         strsql &= ", 'AO' AS AOSource " 'Jules 2018.11.22
        '         strsql &= "FROM APPROVAL_GRP_AO "
        '         strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGA_AO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " 'AND B.UM_DELETED <> 'Y' AND B.UM_STATUS = 'A'
        '         strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGA_A_AO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " 'AND C.UM_DELETED <> 'Y' AND C.UM_STATUS = 'A'
        '         strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGA_GRP_INDEX "
        '         strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGA_GRP_INDEX = AGB_GRP_INDEX "
        '         strsql &= "WHERE AGA_GRP_INDEX = " & intIndex & " "
        '         strsql &= "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' "
        '         'Zulham 03082018 - PAMB
        '         If strType = "PO" Or strType = "PR" Then
        '             strsql &= " AND AGM_TYPE = '" & strType & "' "
        '             strsql &= "ORDER BY B.UM_PO_APP_LIMIT,AGA_SEQ) "
        '         Else
        '             strsql &= " AND AGM_TYPE <> '" & strType & "' "
        '             strsql &= "ORDER BY B.UM_APP_LIMIT,AGA_SEQ); "
        '         End If

        '         'Zulham 03082018 - PAMB
        '         If strType = "PO" Or strType = "PR" Then
        '             strsql &= "UNION ALL "
        '             strsql &= "(SELECT DISTINCT AGPAO_SEQ, AGPAO_AO, ISNULL(AGPAO_A_AO, '') AS AGA_A_AO, AGPAO_RELIEF_IND, B.UM_USER_NAME AS AO_NAME, C.UM_USER_NAME AS AAO_NAME, B.UM_PO_APP_LIMIT AS UM_APP_LIMIT "
        '             strsql &= ", 'PAO' AS AOSource " 'Jules 2018.11.22
        '             strsql &= "FROM APPROVAL_GRP_PAO "
        '             strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = AGPAO_AO AND B.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = AGPAO_A_AO AND C.UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '             strsql &= "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX = AGPAO_GRP_INDEX "
        '             strsql &= "INNER JOIN APPROVAL_GRP_BUYER ON AGPAO_GRP_INDEX = AGB_GRP_INDEX "
        '             strsql &= "WHERE AGPAO_GRP_INDEX = " & intIndex & " "
        '             strsql &= "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' "
        '             strsql &= "AND AGM_TYPE = '" & strType & "' "
        '             strsql &= "ORDER BY B.UM_PO_APP_LIMIT,AGPAO_SEQ); "
        '         End If

        '         ' ai chu add 
        '         ' to get consolidator name
        '         strsql &= "SELECT AGM_CONSOLIDATOR, UM_USER_NAME "
        '         strsql &= "FROM APPROVAL_GRP_MSTR "
        '         strsql &= "INNER JOIN USER_MSTR ON AGM_CONSOLIDATOR = UM_USER_ID AND AGM_COY_ID = UM_COY_ID AND AGM_TYPE<>'INV' "
        '         strsql &= "WHERE AGM_GRP_INDEX = " & intIndex & " "
        '         If strType = "PO" Then
        '             strsql &= " AND AGM_TYPE = '" & strType & "' "
        '         Else
        '             strsql &= " AND AGM_TYPE <> '" & strType & "' "
        '         End If

        '         ds = objDb.FillDs(strsql)
        '         getAOList = ds
        '     End Function

        '     Public Function getPRTempAttach(ByVal strDocNo As String, Optional ByVal strInternalExternal As String = "E") As DataSet
        '         Dim strsql As String
        '         Dim ds As New DataSet
        '         strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
        '         strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PR' "
        '         strsql &= "AND CDA_TYPE = '" & strInternalExternal & "' "
        '         ds = objDb.FillDs(strsql)
        '         getPRTempAttach = ds
        '     End Function
        '     Public Function getUserAttach(ByVal strUserRole As String, ByVal strDocType As String, ByVal strDocNo As String, ByVal seq As String) As DataSet
        '         Dim strsql As String
        '         Dim ds As New DataSet

        '         'Yik Foong
        '         Dim userID = CStr(HttpContext.Current.Session("UserId"))

        '         Dim seqID As String = userID
        '         If seq <> "" Then
        '             seqID = userID & "-" & seq
        '         End If

        '         strsql = "SELECT * FROM USER_ATTACHMENT "
        '         strsql &= "WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         'strsql &= "AND UA_USER_ID = '" & HttpContext.Current.Session("UserId") & "' AND UA_USER_ROLE = '" & strUserRole & "'"
        '         strsql &= "AND UA_USER_ID = '" & seqID & "' AND UA_USER_ROLE = '" & strUserRole & "'"
        '         strsql &= "AND UA_DOC_NO = '" & strDocNo & "' AND UA_DOC_TYPE = '" & strDocType & "'"
        '         ds = objDb.FillDs(strsql)
        '         getUserAttach = ds
        '     End Function

        '     Public Function getUserAttach(ByVal strUserRole As String, ByVal strDocType As String, ByVal strDocNo As String, ByVal id1 As String, ByVal id2 As String, ByVal seq As String) As DataSet
        '         Dim strsql As String
        '         Dim ds As New DataSet

        '         Dim seqNumID1 As String = id1 & "-" & seq
        '         Dim seqNumID2 As String = id2 & "-" & seq


        '         strsql = "SELECT * FROM USER_ATTACHMENT "
        '         strsql &= "WHERE UA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         strsql &= "AND (UA_USER_ID = '" & seqNumID1 & "' OR UA_USER_ID ='" & seqNumID2 & "') AND UA_USER_ROLE = '" & strUserRole & "'"
        '         strsql &= "AND UA_DOC_NO = '" & strDocNo & "' AND UA_DOC_TYPE = '" & strDocType & "'"
        '         ds = objDb.FillDs(strsql)
        '         getUserAttach = ds
        '     End Function

        Public Function submitPR(ByVal strPR As String, ByVal CompanyID As String, ByVal strStatus As String, ByVal strConsolidator As String, ByVal dtAO As DataTable, ByVal strType As String, ByVal PR_Type As String, ByVal blnBuyerWNoWork As Boolean, Optional ByVal blnEnterpriseVersion As Boolean = True) As Integer
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            Dim strIndex As String
            Dim ds As New DataSet

            strsql = "SELECT PRM_PR_Index, PRM_PR_STATUS FROM PR_MSTR WHERE PRM_PR_NO = '" & strPR & "' "
            strsql &= "AND PRM_COY_ID = '" & CompanyID & "' "
            ds = objDb.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                strIndex = ds.Tables(0).Rows(0)("PRM_PR_Index")
                ' check PR status still is draft
                If Common.parseNull(ds.Tables(0).Rows(0)("PRM_PR_STATUS")) = "1" Then
                    strsql = "UPDATE PR_MSTR SET "
                    strsql &= "PRM_SUBMIT_DATE = " & Common.ConvertDate(Now) & ", "

                    If blnBuyerWNoWork = False Then
                        strsql &= "PRM_PR_STATUS = '" & Common.Parse(strStatus) & "', "
                    Else
                        strsql &= "PRM_PR_DATE = " & Common.ConvertDate(Now) & ", PRM_PR_STATUS = '" & PRStatus.Approved & "', "
                    End If

                    strsql &= "PRM_CONSOLIDATOR = '" & Common.Parse(strConsolidator) & "' "
                    strsql &= "WHERE PRM_PR_NO = '" & strPR & "' "
                    strsql &= "AND PRM_COY_ID = '" & CompanyID & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    ' insert into PR_APPROVAL
                    If blnBuyerWNoWork = False Then
                        For i = 0 To dtAO.Rows.Count - 1
                            strsql = "INSERT INTO PR_APPROVAL (PRA_PR_INDEX, PRA_AO, PRA_A_AO, PRA_SEQ, PRA_AO_ACTION, "
                            strsql &= "PRA_APPROVAL_TYPE, PRA_APPROVAL_GRP_INDEX, PRA_RELIEF_IND, PRA_FOR) VALUES ("
                            strsql &= strIndex & ", " ' PRA_PR_INDEX
                            strsql &= "'" & Common.Parse(dtAO.Rows(i)("AO")) & "', " ' PRA_AO
                            strsql &= "'" & Common.Parse(dtAO.Rows(i)("AAO")) & "', " ' PRA_A_AO
                            'Modified by Joon on 28th Sept 2011 for issue 934
                            'strsql &= dtAO.Rows(i)("Seq") & ", 0, " ' PRA_SEQ, PRA_AO_ACTION
                            strsql &= i + 1 & ", 0, " ' PRA_SEQ, PRA_AO_ACTION

                            If PR_Type = "true" Then
                                ' _Yap below sentense need futher checking is any error, tempory comment out
                                strsql &= "'" & Common.Parse(dtAO.Rows(i)("Type")) & "', " ' PRA_APPROVAL_TYPE
                            Else
                                strsql &= "'1', "
                            End If

                            strsql &= dtAO.Rows(i)("GrpIndex") & ", " ' PRA_APPROVAL_GRP_INDEX
                            strsql &= "'" & Common.Parse(dtAO.Rows(i)("Relief")) & "', 'PR' ) " ' PRA_RELIEF_IND
                            Common.Insert2Ary(strAryQuery, strsql)
                        Next
                    End If

                    Dim objBudget As New BudgetControl
                    Dim strBCM, strCurrency As String
                    Dim blnExceed As Boolean
                    Dim dtBCM As New DataTable
                    'If HttpContext.Current.Session("Env") <> "FTN" Then
                    'If blnEnterpriseVersion = False Then
                    ' Yap:14Feb2013: Change this to "True". Mistake.
                    If blnEnterpriseVersion = True Then
                        objBudget.BCMCalc("PR", CompanyID, strPR, EnumBCMAction.SubmitPR, strAryQuery)
                        blnExceed = objBudget.checkBCM(strPR, CompanyID, dtBCM, strBCM)
                        strsql = "SELECT PRM_CURRENCY_CODE FROM PR_MSTR "
                        strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(strPR) & "' AND PRM_COY_ID = '" & CompanyID & "' "
                        Dim tDS As DataSet = objDb.FillDs(strsql)
                        For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                            strCurrency = Common.parseNull(tDS.Tables(0).Rows(j).Item("PRM_CURRENCY_CODE"))
                        Next
                    End If


                    If blnEnterpriseVersion = True Then
                        strsql = "SELECT PRD_PR_NO, PRD_PR_LINE, PRD_ASSET_GROUP FROM PR_DETAILS WHERE PRD_PR_NO = '" & strPR & "' "
                        strsql &= "AND (PRD_ASSET_GROUP IS NOT NULL AND PRD_ASSET_GROUP <> '') AND PRD_COY_ID = '" & CompanyID & "' "
                        ds = objDb.FillDs(strsql)

                        Dim strAssetNo As String
                        Dim intAssetIncrementNo As Integer
                        intAssetIncrementNo = 1
                        For i = 0 To ds.Tables(0).Rows.Count - 1
                            Common.parseNull(ds.Tables(0).Rows(i)("PRD_ASSET_GROUP"))

                            Dim strParam As String = objDb.GetVal("SELECT IFNULL(CP_PARAM_VALUE,'') AS CP_PARAM_VALUE FROM COMPANY_PARAM WHERE CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' AND CP_COY_ID = '" & CompanyID & "'")
                            If strParam = "" Then
                                strsql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                                strsql &= "'" & CompanyID & "', "
                                strsql &= "'Prefix', "
                                strsql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
                                strsql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
                                strsql &= "'eProcure' ) "
                                Common.Insert2Ary(strAryQuery, strsql)

                                strsql = "INSERT INTO COMPANY_PARAM (CP_COY_ID, CP_PARAM_NAME, CP_PARAM_VALUE, CP_PARAM_TYPE, CP_APP_PKG) VALUES ( "
                                strsql &= "'" & CompanyID & "', "
                                strsql &= "'Last Used No', "
                                strsql &= "'00000000', "
                                strsql &= "'" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "', "
                                strsql &= "'eProcure' ) "
                                Common.Insert2Ary(strAryQuery, strsql)
                            End If

                            strAssetNo = " (SELECT CAST( CONCAT(RIGHT(YEAR(CURRENT_DATE()),2), " _
                                        & " CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), " _
                                        & " (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')))  AS CHAR(1000)) AS CP_PARAM_VALUE FROM (SELECT * FROM COMPANY_PARAM " _
                                        & " WHERE CP_COY_ID = '" & CompanyID & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' AND CP_PARAM_NAME = 'Last Used No') ZZZ )"

                            strsql = "UPDATE PR_DETAILS SET "
                            strsql &= "PRD_ASSET_NO = " & strAssetNo & " "
                            strsql &= "WHERE PRD_PR_NO = '" & strPR & "' "
                            strsql &= "AND PRD_COY_ID = '" & CompanyID & "' "
                            strsql &= "AND PRD_PR_LINE = '" & ds.Tables(0).Rows(i)("PRD_PR_LINE") & "' "
                            Common.Insert2Ary(strAryQuery, strsql)

                            strsql = "UPDATE COMPANY_PARAM SET CP_PARAM_VALUE = CONCAT(REPEAT('0',LENGTH(CP_PARAM_VALUE) - LENGTH(CP_PARAM_VALUE + '" & intAssetIncrementNo & "')), (CP_PARAM_VALUE + '" & intAssetIncrementNo & "')) WHERE CP_PARAM_NAME = 'LAST USED NO' AND CP_COY_ID = '" & CompanyID & "' AND CP_PARAM_TYPE = '" & ds.Tables(0).Rows(i)("PRD_ASSET_GROUP") & "' "
                            Common.Insert2Ary(strAryQuery, strsql)

                            'intAssetIncrementNo += 1
                        Next
                    End If

                    Dim objUsers As New Users
                    'objUsers.Log_UserActivity(strAryQuery, WheelModule.PRMod, WheelUserActivity.B_SubmitPR, strPR)
                    objUsers = Nothing

                    If objDb.BatchExecute(strAryQuery) Then
                        'If strType = "0" Then
                        '    sendMailToAO(strPR, CLng(strIndex), 1)
                        'Else
                        '    ' PR value more than operating budget
                        '    RequestBudgetTopup(strBCM, "", "", strCurrency, dtBCM)
                        '    read.Close()
                        'End If

                        'Michelle (19/3/2008) - To allow mail to send to AO evenif budget has been bust when the budget mode is 'Advisory'
                        'If strType = "0" Or strType = "1" Then
                        '    sendMailToAO(strPR, CLng(strIndex), 1)
                        'Else
                        '    sendMailToPO(strPR, 1)
                        'End If
                        'If strType <> "0" Then
                        '    ' PR value more than operating budget
                        '    'RequestBudgetTopup(strBCM, "", "", strCurrency, dtBCM)
                        'End If

                        'If strType <> "0" And blnExceed = True Then
                        '    RequestBudgetTopup(strBCM, "", "", strCurrency, dtBCM)
                        'End If

                        submitPR = WheelMsgNum.Save
                    End If
                    objBudget = Nothing
                Else
                    submitPR = WheelMsgNum.NotSave
                End If
            Else
                submitPR = WheelMsgNum.Delete
            End If

        End Function

        Public Function getPRApprFlow(ByVal userID As String, ByVal companyID As String, ByVal isFTN As Boolean) As DataTable
            Dim strsql As String
            Dim dt As DataTable

            If isFTN Then
                strsql = "SELECT AGM_GRP_INDEX AS GrpIndex, ISNULL(AGA_AO,'') AS AO, ISNULL(AGA_A_AO,'') AS AAO, CAST(ISNULL(AGA_SEQ, 1) AS CHAR) AS SEQ, ISNULL(AGM_TYPE, '') AS Type, ISNULL(AGA_RELIEF_IND, '') AS Relief  FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO "
                strsql &= "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGB_BUYER = '" & userID & "' AND AGM_TYPE = 'PR'"
            Else
                'strsql = "SELECT AGM_GRP_INDEX, AGA_AO, AGA_A_AO FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO "
                'strsql &= "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                'strsql &= "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' AND AGM_TYPE = 'PR'"

                ' _Yap : Unknow differenciate it
                strsql = "SELECT AGM_GRP_INDEX AS GrpIndex, ISNULL(AGA_AO,'') AS AO, ISNULL(AGA_A_AO,'') AS AAO, CAST(ISNULL(AGA_SEQ, 1) AS CHAR) AS SEQ, ISNULL(AGM_TYPE, '') AS Type, ISNULL(AGA_RELIEF_IND, '') AS Relief  FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO "
                strsql &= "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & companyID & "' "
                strsql &= "AND AGB_BUYER = '" & userID & "' AND AGM_TYPE = 'PR'"

            End If

            dt = objDb.FillDt(strsql)
            getPRApprFlow = dt
        End Function

        Public Function updatePRStatus(ByVal userID As String, ByVal strPR As String, ByVal CompanyID As String, ByVal strStatus As String, ByVal strConsolidator As String, ByVal dtAO As DataTable, ByVal strType As String) As Integer
            Dim strsql As String
            Dim i As Integer
            Dim strAryQuery(0) As String
            Dim strIndex As String
            Dim ds As New DataSet

            strsql = "SELECT PRM_PR_Index, PRM_PR_STATUS FROM PR_MSTR WHERE PRM_PR_NO = '" & strPR & "' "
            strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
            ds = objDb.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                strIndex = ds.Tables(0).Rows(0)("PRM_PR_Index")
                ' check PR status still is draft
                If Common.parseNull(ds.Tables(0).Rows(0)("PRM_PR_STATUS")) = "1" Then
                    strsql = "UPDATE PR_MSTR SET "
                    strsql &= "PRM_PR_DATE = " & Common.ConvertDate(Now) & ", "
                    strsql &= "PRM_PR_STATUS = '" & Common.Parse(strStatus) & "', "
                    strsql &= "PRM_CONSOLIDATOR = '" & Common.Parse(strConsolidator) & "' "
                    strsql &= "WHERE PRM_PR_NO = '" & strPR & "' "
                    strsql &= "AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    Common.Insert2Ary(strAryQuery, strsql)

                    ' insert into PR_APPROVAL
                    For i = 0 To dtAO.Rows.Count - 1
                        strsql = "INSERT INTO PR_APPROVAL (PRA_PR_INDEX, PRA_AO, PRA_A_AO, PRA_SEQ, PRA_AO_ACTION, "
                        strsql &= "PRA_APPROVAL_TYPE, PRA_APPROVAL_GRP_INDEX, PRA_RELIEF_IND) VALUES ("
                        strsql &= strIndex & ", "                        ' PRA_PR_INDEX
                        strsql &= "'" & Common.Parse(dtAO.Rows(i)("AO")) & "', "                         ' PRA_AO
                        strsql &= "'" & Common.Parse(dtAO.Rows(i)("AAO")) & "', "                        ' PRA_A_AO
                        strsql &= dtAO.Rows(i)("Seq") & ", 0, "                      ' PRA_SEQ, PRA_AO_ACTION
                        strsql &= "'" & Common.Parse(dtAO.Rows(i)("Type")) & "', "                       ' PRA_APPROVAL_TYPE
                        strsql &= dtAO.Rows(i)("GrpIndex") & ", "                        ' PRA_APPROVAL_GRP_INDEX
                        strsql &= "'" & Common.Parse(dtAO.Rows(i)("Relief")) & "') "                         ' PRA_RELIEF_IND
                        Common.Insert2Ary(strAryQuery, strsql)
                    Next

                    Dim objBudget As New BudgetControl
                    objBudget.BCMCalc("PR", CompanyID, strPR, EnumBCMAction.SubmitPR, strAryQuery)
                    Dim strBCM, strCurrency As String
                    Dim blnExceed As Boolean
                    Dim dtBCM As New DataTable
                    blnExceed = objBudget.checkBCM(strPR, CompanyID, dtBCM, strBCM)
                    strsql = "SELECT PRM_CURRENCY_CODE FROM PR_MSTR "
                    strsql &= "WHERE PRM_PR_NO = '" & Common.Parse(strPR) & "' AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
                    Dim tDS As DataSet = objDb.FillDs(strsql)
                    For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                        strCurrency = Common.parseNull(tDS.Tables(0).Rows(j).Item("PRM_CURRENCY_CODE"))
                    Next

                    Dim objUsers As New Users
                    objUsers.Log_UserActivity(userID, CompanyID, strAryQuery, WheelModule.PRMod, WheelUserActivity.B_SubmitPR, strPR)
                    objUsers = Nothing

                    If objDb.BatchExecute(strAryQuery) Then
                        'If strType = "0" Then
                        '    sendMailToAO(strPR, CLng(strIndex), 1)
                        'Else
                        '    ' PR value more than operating budget
                        '    RequestBudgetTopup(strBCM, "", "", strCurrency, dtBCM)
                        '    read.Close()
                        'End If

                        'Michelle (19/3/2008) - To allow mail to send to AO evenif budget has been bust when the budget mode is 'Advisory'
                        If strType = "0" Or strType = "1" Then
                            sendMailToAO(userID, CompanyID, strPR, CLng(strIndex), 1)
                        End If
                        If strType <> "0" Then
                            ' PR value more than operating budget
                            RequestBudgetTopup(userID, CompanyID, strBCM, "", "", strCurrency, dtBCM)

                        End If

                        updatePRStatus = WheelMsgNum.Save
                    End If
                    objBudget = Nothing
                Else
                    updatePRStatus = WheelMsgNum.NotSave
                End If
            Else
                updatePRStatus = WheelMsgNum.Delete
            End If

        End Function

        '     Public Function getPRCurrency(ByVal strPR As String) As String
        '         Dim ds As New DataSet
        '         ds = objDb.FillDs("SELECT PRM_CURRENCY_CODE FROM PR_MSTR WHERE PRM_PR_NO = '" & strPR & "' AND PRM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' ")

        '         If ds.Tables(0).Rows.Count > 0 Then
        '             getPRCurrency = Common.parseNull(ds.Tables(0).Rows(0)(0))
        '         End If
        '     End Function

        '     Public Function deletePRAttachment(ByVal intIndex As Integer)
        '         Dim strsql As String
        '         strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
        '         strsql &= "WHERE CDA_ATTACH_INDEX = " & intIndex
        '         objDb.Execute(strsql)
        '     End Function
        '     Public Function deleteUserAttachment(ByVal intIndex As Integer)
        '         Dim strsql As String
        '         strsql = "DELETE FROM USER_ATTACHMENT "
        '         strsql &= "WHERE UA_ATTACH_INDEX = " & intIndex
        '         objDb.Execute(strsql)
        '     End Function
        Public Function checkBCM(ByVal companyID As String) As String
            Dim strsql As String
            strsql = "SELECT ISNULL(CM_BCM_SET,0) FROM COMPANY_MSTR WHERE CM_COY_ID = '" & companyID & "' "
            checkBCM = objDb.GetVal(strsql)
        End Function

        '     Public Function getUserAcc() As DataSet
        '         Dim strsql As String
        '         Dim ds As New DataSet
        '         strsql = "SELECT AU_ACCT_INDEX FROM ACCOUNT_USERS "
        '         strsql &= "RIGHT JOIN ACCOUNT_MSTR ON AM_ACCT_INDEX = AU_ACCT_INDEX AND AM_DELETED <> 'Y' "
        '         strsql &= "AND AU_COY_ID = AM_COY_ID "
        '         strsql &= "WHERE AU_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
        '         strsql &= "AND AU_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '         ds = objDb.FillDs(strsql)
        '         getUserAcc = ds
        '     End Function

        '     Public Function checkUserAccExist() As Boolean
        '         Dim ds As New DataSet
        '         ds = getUserAcc()
        '         If ds.Tables(0).Rows.Count > 0 Then
        '             checkUserAccExist = True
        '         Else
        '             checkUserAccExist = False
        '         End If
        '     End Function

        'Michelle (19/11/2010) - To cater for PO approval
        'Public Function sendMailToAO(ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer)
        Public Function sendMailToAO(ByVal userID As String, ByVal companyID As String, ByVal strDocNo As String, ByVal intIndex As Long, ByVal intSeq As Integer, Optional ByVal strType As String = "")
            Dim strsql, strcond As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String

            If strType = "PO" Then
                strDocType = "PO"
            Else
                strDocType = "PR"
            End If
            strsql = ""

            'strBody = "Dear Approving Officer, <BR>"
            strBody &= "<P>You have an outstanding " & strDocType & " (" & strDocNo & ") waiting for approval. <BR>"
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'strBody &= "<P>For more details, please login to "
            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
            '    strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
            '    strBody &= objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooterENT & Common.EmailCompGen
            'End If
            strsql = "SELECT RAM_USER_ID FROM RELIEF_ASSIGNMENT_MSTR "
            strsql &= "WHERE RAM_USER_ROLE = 'Approving Officer' "
            strsql &= "AND RAM_COY_ID = '" & companyID & "' "
            strsql &= "AND GETDATE() BETWEEN RAM_START_DATE AND RAM_END_DATE + 1 "
            strsql &= "AND RAM_USER_ID = "
            'Michelle (22/9/2010) - To change the TOP 1 syntax
            'strsql &= "(SELECT TOP 1 PRA_AO FROM PR_APPROVAL "
            'strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " "
            'strsql &= "AND PRA_SEQ = " & intSeq & " "
            'strsql &= "ORDER BY PRA_SEQ )"

            'Michelle (20/2/2012) - Issue 1512
            'strcond = " WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_SEQ = " & intSeq & " ORDER BY PRA_SEQ "
            strcond = " WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_SEQ = " & intSeq & " AND PRA_FOR = '" & strDocType & "' ORDER BY PRA_SEQ "
            strsql &= "(SELECT PRA_AO FROM PR_APPROVAL " & strcond & " LIMIT 1)"
            If objDB.Exist(strsql) > 0 Then
                blnRelief = True
            Else
                blnRelief = False
            End If

            strsql = "SELECT PRA_AO, ISNULL(PRA_A_AO, '') AS PRA_A_AO, B.UM_EMAIL AS AO_EMAIL, ISNULL(C.UM_EMAIL, '') AS AAO_EMAIL, "
            strsql &= "B.UM_USER_NAME AS AO_NAME, ISNULL(C.UM_USER_NAME, '') AS AAO_NAME "
            strsql &= "FROM PR_APPROVAL "
            strsql &= "INNER JOIN USER_MSTR AS B ON B.UM_USER_ID = PRA_AO AND B.UM_DELETED <> 'Y' AND B.UM_COY_ID = '" & companyID & "' "
            strsql &= "LEFT JOIN USER_MSTR AS C ON C.UM_USER_ID = PRA_A_AO AND C.UM_DELETED <> 'Y' AND C.UM_COY_ID = '" & companyID & "' "
            'Michelle (20/2/2012) - Issue 1512
            'strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_SEQ = " & intSeq
            strsql &= "WHERE PRA_PR_INDEX = " & intIndex & " AND PRA_FOR = '" & strDocType & "' AND PRA_SEQ = " & intSeq
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                Dim objMail As New AppMail
                If blnRelief Then
                    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
                    Else
                        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (Approving Officer), <BR>" & strBody
                    End If
                Else
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Approving Officer), <BR>" & strBody
                End If

                objMail.Subject = "Agora : " & strDocType & " Approval"
                objMail.SendMail()
            End If
            objCommon = Nothing
        End Function

        Public Function RequestBudgetTopup(ByVal userID As String, ByVal companyID As String, ByVal strBCM As String, ByVal strDept As String, ByVal strRemark As String, ByVal strCurrency As String, ByVal dt As DataTable)
            Dim strsql As String
            Dim ds As New DataSet
            Dim strBody, strTo, strExtra, strLine As String
            Dim objUser As New User
            Dim objUsers As New Users
            Dim i As Integer
            Dim blnAttn As Boolean = False
            Dim objCommon As New Common
            Dim strBCMSetting As String

            strsql = "SELECT CM_BCM_SET FROM COMPANY_MSTR WHERE CM_COY_ID = '" & Common.Parse(companyID) & "'"
            strBCMSetting = objDb.GetVal(strsql)

            Dim dtAttn As New DataTable
            dtAttn.Columns.Add("UserId", Type.GetType("System.String"))
            dtAttn.Columns.Add("UserName", Type.GetType("System.String"))
            dtAttn.Columns.Add("Email", Type.GetType("System.String"))
            dtAttn.Columns.Add("CoyName", Type.GetType("System.String"))
            Dim dtr As DataRow

            objUser = objUsers.GetUserDetails(userID, companyID)
            ds = objUsers.getUserEmailList(FixedRole.Finance_Manager, companyID)

            If ds.Tables("USERTO").Rows.Count > 0 Then
                Dim objMail As New AppMail
                'strBody = "Dear Finance Manager, <BR><BR>"

                '<a href='mailto:tanac@10.20.30.115'>tanac@10.20.30.115</a>
                If strBCM = "1" Then
                    objMail.Subject = "Agora : Request for Budget Top-up"
                    strBody &= "Buyer (" & objUser.Name & ", <a href='mailto:" & objUser.Email & "'>" & objUser.Email & "</a>) is requesting for a budget top-up. <BR>"
                    strBody &= "<P>The details of the request as follow: <BR>"
                    strBody &= "<P>Department: " & strDept

                    If dt.Rows.Count > 0 Then
                        strBody &= "<TABLE WIDTH='50%'>"
                        strBody &= "<TR><TD ALIGN='left' width='40%'>Account Code</TD><TD ALIGN='right' WIDTH='60%'>Amount</TD></TR>"
                        For i = 0 To dt.Rows.Count - 1
                            strBody &= "<TR>"
                            strBody &= "<TD>" & dt.Rows(i)("Acct_Code") & "</TD><TD ALIGN='right'>" & strCurrency & " " & Format(CDbl(dt.Rows(i)("Acct_Amount")), "###,###,##0.00") & "</TD>"
                            strBody &= "</TR>"
                        Next
                        strBody &= "</TABLE>"
                    End If

                    strBody &= "<P>Buyer's remarks: <BR>"
                    strBody &= "<P>" & strRemark & " <BR>"
                    strBody &= "<P>To approve, please login to " & objCommon.EmailHomeEhubAddr & " and update the budget. <BR>"
                    'strBody &= "<P>To approve, please login to "
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= objCommon.EmailHomeEhubAddr
                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= objCommon.EmailHomeEhubENTAddr
                    'End If
                    'strBody &= " and update the budget. <BR>"
                Else
                    If strBCMSetting <> "1" Then
                        Exit Function
                    End If
                    objMail.Subject = "Agora : Budget Advisory : Budget exceeded"
                    strBody &= "This is an advisory email. The following accounts have exceeded their operating budget. <P>"

                    If dt.Rows.Count > 0 Then
                        strBody &= "<TABLE WIDTH='50%'>"
                        strBody &= "<TR><TD ALIGN='left' width='40%'>Account Code</TD><TD ALIGN='right' WIDTH='60%'>Amount</TD></TR>"
                        For i = 0 To dt.Rows.Count - 1
                            strBody &= "<TR>"
                            strBody &= "<TD>" & dt.Rows(i)("Acct_Code") & "</TD><TD ALIGN='right'>" & strCurrency & " " & Format(CDbl(dt.Rows(i)("Acct_Amount")), "###,###,##0.00") & "</TD>"
                            strBody &= "</TR>"
                        Next
                        strBody &= "</TABLE>"
                    End If
                    strBody &= "<BR><P>For more details, please login to " & objCommon.EmailHomeAddr & " to view it. <BR>"
                    'strBody &= "<BR><P>For more details, please login to "
                    'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                    '    strBody &= objCommon.EmailHomeAddr

                    'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                    '    strBody &= objCommon.EmailHomeENTAddr
                    'End If
                    'strBody &= " to view it. <BR>"
                End If
                strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
                'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
                'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                '    strBody &= "<P>" & objCommon.EmailFooterENT & Common.EmailCompGen
                'End If

                For i = 0 To ds.Tables("USERTO").Rows.Count - 1
                    If Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) <> "" And Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_STATUS")) = "A" Then
                        blnAttn = True
                        ' strTo &= Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & ","  'Michelle (12/3/2008) - CR0024
                        strTo = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL")) & ","

                        objMail.MailTo = strTo
                        objMail.Body = "Dear " & Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME")) & " (Finance Manager), <BR>" & strBody
                        objMail.SendMail()
                    Else
                        blnAttn = False
                        dtr = dtAttn.NewRow()
                        dtr("UserId") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_ID"))
                        dtr("UserName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_USER_NAME"))
                        dtr("Email") = Common.parseNull(ds.Tables("USERTO").Rows(i)("UM_EMAIL"))
                        dtr("CoyName") = Common.parseNull(ds.Tables("USERTO").Rows(i)("CM_COY_NAME"))
                        dtAttn.Rows.Add(dtr)
                    End If
                Next

                If Not blnAttn Then
                    '    objMail.MailTo = strTo
                    '    objMail.Body = strBody
                    'Else
                    strLine = Strings.StrDup(Common.EmailCompGen.Length, ".")
                    If dtAttn.Rows.Count = 0 Then
                        strExtra = "Dear Hub Admin, <BR>"
                        strExtra &= "User not found from " & ds.Tables("COMPANY").Rows(0)("CM_COY_NAME") & ". <BR>"
                        objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin")                        ' EmailHub ' ConfigurationSettings.AppSettings("HubAdminEmail")

                        objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & Common.EmailCompGen & "<BR><P>" & strLine
                        'objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>"
                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                        '    objMail.Body &= objCommon.EmailFooter
                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                        '    objMail.Body &= objCommon.EmailFooterENT
                        'End If
                        'objMail.Body &= Common.EmailCompGen & "<BR><P>" & strLine
                        objMail.Subject = "Agora : No User Exists - " & objMail.Subject
                    Else
                        objMail.MailTo = ConfigurationSettings.AppSettings("EmailToHubAdmin")                        'EmailHub ' ConfigurationSettings.AppSettings("HubAdminEmail")
                        strExtra = "Dear Hub Admin, <BR>"
                        For i = 0 To dtAttn.Rows.Count - 1
                            strExtra &= "User account for " & dtAttn.Rows(i)("UserId") & " (" & dtAttn.Rows(i)("UserId") & ", " & dtAttn.Rows(i)("Email") & ") of " & dtAttn.Rows(i)("CoyName") & " company has been deactivated. <BR>"
                        Next

                        objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>" & objCommon.EmailFooter & Common.EmailCompGen & "<BR><P>" & strLine
                        'objMail.Body = strExtra & "<P><U>Email Sent Un-Successfully:</U><BR><P>" & strLine & "<BR><P>" & strBody & "<P>"
                        'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
                        '    objMail.Body &= objCommon.EmailFooter
                        'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
                        '    objMail.Body &= objCommon.EmailFooterENT
                        'End If
                        'objMail.Body &= Common.EmailCompGen & "<BR><P>" & strLine
                        objMail.Subject = "Agora : Deactivated User - " & objMail.Subject
                    End If
                    objMail.SendMail()
                End If
            End If
            objCommon = Nothing
        End Function

        '     Public Function getPOAttn(ByVal intPOIndex As Integer) As String
        '         Dim strsql As String
        '         Dim strAttn As String = ""

        '         strsql = "SELECT PRM_CONSOLIDATOR FROM PR_MSTR WHERE PRM_PO_INDEX = " & intPOIndex & " "
        '         strsql &= "AND PRM_CONSOLIDATOR <> '' AND PRM_CONSOLIDATOR IS NOT NULL"
        '         If objDb.Exist(strsql) > 0 Then
        '             strsql = "SELECT PRM_CONSOLIDATOR, UM_USER_NAME FROM PR_MSTR "
        '             strsql &= "INNER JOIN USER_MSTR ON UM_USER_ID = PRM_CONSOLIDATOR AND UM_COY_ID = PRM_COY_ID "
        '             strsql &= "WHERE PRM_PO_INDEX = " & intPOIndex & " "
        '         Else
        '             strsql = "SELECT PRM_S_ATTN, PRM_S_ATTN FROM PR_MSTR "
        '             strsql &= "WHERE PRM_PO_INDEX = " & intPOIndex & " "
        '         End If

        '         Dim tDS As DataSet = objDb.FillDs(strsql)
        '         For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '             strAttn = Common.parseNull(tDS.Tables(0).Rows(j).Item(1))
        '         Next

        '         getPOAttn = strAttn
        '     End Function

        ' ai chu added on 06/12/2005
        ' need to pass in requestor name
        Public Function getRequestorName(ByVal strType As String, ByVal strDocNo As String, ByVal strCoyId As String) As String
            Dim strsql As String
            Dim ds As DataSet
            Dim strName As String = ""
            Dim dsName As DataSet

            Select Case strType.ToUpper
                Case "PO"
                    strsql = "SELECT PRM_REQ_NAME FROM PO_MSTR  "
                    strsql &= "INNER JOIN PR_MSTR ON PRM_PO_INDEX = POM_PO_INDEX "
                    strsql &= "WHERE POM_PO_NO = '" & Common.Parse(strDocNo) & "' "
                    strsql &= "AND POM_B_COY_ID = '" & Common.Parse(strCoyId) & "' "
                Case "PR"
                    strsql = "SELECT PRM_REQ_NAME FROM PR_MSTR WHERE PRM_PR_NO = '" & Common.Parse(strDocNo) & "' "
                    strsql &= "AND PRM_COY_ID = '" & Common.Parse(strCoyId) & "'"
            End Select

            dsName = objDb.FillDs(strsql)

            ' if returns more than 1 record means PRs were consolidated into 1 PO
            ' so no need to pass the requestor name
            If dsName.Tables(0).Rows.Count = 1 Then
                strName = dsName.Tables(0).Rows(0)("PRM_REQ_NAME")
            End If

            Return strName
        End Function

        '     'Name       : getPRFromQuot
        '     'Author     : Tan Ai Chu
        '     'Descption  : to retrieve PR List by quotation number
        '     'Remark     : used in RFQ_List (SR U30025)
        '     'ReturnValue: 
        '     'LastUpadte : 06 Dec 2005
        '     'Version    : 1.00
        '     Public Function getPRFromQuot(ByVal strQuoNo As String, ByVal strSCoyId As String, ByVal strBCoyId As String) As DataSet
        '         Dim strsql As String
        '         Dim ds As DataSet
        '         strsql = "SELECT PRM_PR_INDEX, PRM_PR_NO, PRM_QUOTATION_NO, PRM_COY_ID "
        '         strsql &= "FROM RFQ_REPLIES_MSTR, RFQ_MSTR, PR_MSTR "
        '         strsql &= "WHERE RRM_RFQ_ID = RM_RFQ_ID AND RRM_V_COMPANY_ID = PRM_S_COY_ID "
        '         strsql &= "AND RRM_ACTUAL_QUOT_NUM = PRM_QUOTATION_NO AND RM_COY_ID = PRM_COY_ID "
        '         strsql &= "AND PRM_RFQ_INDEX = RM_RFQ_ID AND PRM_QUOTATION_NO = '" & Common.Parse(strQuoNo) & "' "
        '         strsql &= "AND RRM_V_Company_ID = '" & Common.Parse(strSCoyId) & "' "
        '         strsql &= "AND PRM_COY_ID = '" & Common.Parse(strBCoyId) & "' "
        '         ds = objDb.FillDs(strsql)
        '         getPRFromQuot = ds
        '     End Function

        '     Function DupPRItem(ByVal strNewPR As String, ByVal intPRLine As Int32) As String
        '         Dim strSQL, strMsg As String
        '         Dim strAryQuery(0) As String

        '         '//Shift PR Line down
        '         strSQL = "UPDATE PR_DETAILS SET PRD_PR_LINE=PRD_PR_LINE+1 WHERE PRD_PR_NO='" & _
        '         strNewPR & "' AND PRD_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE>=" & intPRLine + 1 & " ORDER BY PRD_PR_LINE DESC "
        '         Common.Insert2Ary(strAryQuery, strSQL)

        '         '//Insert PR Line below selected PR Line
        '         'Michelle (16/6/2010) - To include the Tax Code

        '         strSQL = "INSERT INTO PR_DETAILS (PRD_PR_NO, PRD_COY_ID, PRD_PR_LINE, PRD_PRODUCT_CODE, " & _
        '         "PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_UOM, PRD_ORDERED_QTY, PRD_UNIT_COST, " & _
        '         "PRD_ETD, PRD_WARRANTY_TERMS, PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY, PRD_REMARK, PRD_GST, " & _
        '         "PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, " & _
        '         "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, " & _
        '         "PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE) SELECT " & _
        '         "PRD_PR_NO, PRD_COY_ID, " & intPRLine & "+1, PRD_PRODUCT_CODE, " & _
        '         "PRD_VENDOR_ITEM_CODE, PRD_PRODUCT_DESC, PRD_UOM, PRD_ORDERED_QTY, PRD_UNIT_COST, " & _
        '         "PRD_ETD, PRD_WARRANTY_TERMS, PRD_MIN_ORDER_QTY, PRD_MIN_PACK_QTY, PRD_REMARK, PRD_GST, " & _
        '         "PRD_D_ADDR_CODE, PRD_D_ADDR_LINE1, PRD_D_ADDR_LINE2, PRD_D_ADDR_LINE3, PRD_D_POSTCODE, PRD_D_CITY, PRD_D_STATE, " & _
        '         "PRD_D_COUNTRY, PRD_ACCT_INDEX, PRD_PRODUCT_TYPE, PRD_B_ITEM_CODE, PRD_B_CATEGORY_CODE, PRD_B_GL_CODE, " & _
        '         "PRD_SOURCE, PRD_CD_GROUP_INDEX, PRD_RFQ_QTY, PRD_QTY_TOLERANCE, PRD_S_COY_ID, PRD_B_TAX_CODE, PRD_CT_ID, PRD_CURRENCY_CODE " & _
        '         "FROM PR_DETAILS WHERE PRD_PR_NO='" & _
        '         strNewPR & "' AND PRD_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND PRD_PR_LINE=" & intPRLine
        '         Common.Insert2Ary(strAryQuery, strSQL)

        '         If objDb.BatchExecute(strAryQuery) Then
        '             DupPRItem = "1"
        '         Else
        '             DupPRItem = "2"
        '         End If
        '     End Function

        Public Function sendMailToPO(ByVal strDocNo As String, ByVal intSeq As Integer, Optional ByVal strType As String = "")
            Dim strsql, strcond, strPO As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim dsPO As New DataSet
            Dim i, j As Integer


            If strType = "PO" Then
                strDocType = "PO"
            Else
                strDocType = "PR"
            End If

            'strBody = "Dear Approving Officer, <BR>"
            strBody &= "<P>You have an outstanding " & strDocType & " (" & strDocNo & ") waiting for your action. <BR>"
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'strBody &= "<P>For more details, please login to "
            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
            '    strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
            '    strBody &= objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooterENT & Common.EmailCompGen
            'End If

            'Jules 2018.11.08 - Filtered by usergroup.
            strsql = "SELECT DISTINCT ua_user_id FROM user_assign " &
            "LEFT JOIN users_usrgrp ON ua_user_id = uu_user_id " &
            "LEFT JOIN user_group_mstr ON ugm_usrgrp_id = uu_usrgrp_id WHERE " &
            "ua_fixed_role = 'Purchasing Officer' AND ugm_fixed_role = 'Purchasing Officer' AND ua_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' " &
            "AND uu_coy_id='" & HttpContext.Current.Session("CompanyId") & "' AND ugm_usrgrp_name='P2P Purchasing Officer' " &
            "AND ua_type = 'CT' AND ua_assign_value IN " &
            "(SELECT distinct CT_ROOT_PREFIX FROM PR_DETAILS INNER JOIN " &
            "commodity_type ON ct_id = prd_ct_id WHERE PRD_PR_NO = '" & strDocNo & "') "
            'strsql = "SELECT DISTINCT um_user_name, um_email FROM user_assign "
            'strsql &= "INNER JOIN user_mstr ON um_user_id = ua_user_id AND um_coy_id = ua_coy_id "
            'strsql &= "WHERE ua_user_id = '" & HttpContext.Current.Session("UserId") & "' AND ua_coy_id = '" & HttpContext.Current.Session("CompanyId") & "' AND ua_type = 'CT' AND ua_assign_value IN (SELECT PRD_CT_ID FROM PR_DETAILS WHERE PRD_PR_NO = '" & strDocNo & "') "

            dsPO = objDB.FillDs(strsql)

            For i = 0 To dsPO.Tables(0).Rows.Count - 1
                strPO &= ",'" & dsPO.Tables(0).Rows(i)("UA_USER_ID") & "'"
            Next
            strPO = strPO.Substring(1)


            strsql = "SELECT UM_USER_NAME,UM_EMAIL FROM user_mstr WHERE UM_STATUS = 'A' AND um_deleted = 'N' and UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' and um_user_id IN (" & strPO & ")"
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                For j = 0 To ds.Tables(0).Rows.Count - 1
                    Dim objMail As New AppMail
                    'If blnRelief Then
                    '    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                    '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Purchasing Officer), <BR>" & strBody
                    '    Else
                    '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                    '        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (Purchasing Officer), <BR>" & strBody
                    '    End If
                    'Else
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(j)("UM_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(j)("UM_USER_NAME")) & " (Purchasing Officer), <BR>" & strBody
                    'End If

                    objMail.Subject = "Agora : " & strDocType & " Created"
                    objMail.SendMail()
                Next

            End If
            objCommon = Nothing
        End Function

        Public Function sendMailToBuyer(ByVal strDocNo As String, ByVal intSeq As Integer, Optional ByVal strType As String = "")
            Dim strsql, strcond, strPO As String
            Dim blnRelief As Integer
            Dim ds As New DataSet
            Dim strBody As String
            Dim objCommon As New Common
            Dim objDB As New EAD.DBCom
            Dim strDocType As String
            Dim dsPO As New DataSet
            Dim i, j As Integer


            If strType = "PO" Then
                strDocType = "PO"
                strPO = objDB.Get1ColumnCheckNull("po_mstr", "pom_created_by", " WHERE pom_po_no='" & strDocNo & "' AND pom_b_coy_id='" & HttpContext.Current.Session("CompanyId") & "'")
            Else
                strDocType = "PR"
                strPO = objDB.Get1ColumnCheckNull("pr_mstr", "prm_buyer_id", " WHERE prm_pr_no='" & strDocNo & "' AND prm_coy_id='" & HttpContext.Current.Session("CompanyId") & "'")
            End If

            'strBody = "Dear Approving Officer, <BR>"
            strBody &= "<P>Your " & strDocType & " (" & strDocNo & ") has been approved. <BR>"
            strBody &= "<P>For more details, please login to " & objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'strBody &= "<P>For more details, please login to "
            'If HttpContext.Current.Session("CompanyType") = "Buyer-FTN" Then
            '    strBody &= objCommon.EmailHomeEhubAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooter & Common.EmailCompGen
            'ElseIf HttpContext.Current.Session("CompanyType") = "Buyer-Enterprise" Then
            '    strBody &= objCommon.EmailHomeEhubENTAddr & " to view it. <BR>"
            '    strBody &= "<P>" & objCommon.EmailFooterENT & Common.EmailCompGen
            'End If

            strsql = "SELECT UM_USER_NAME,UM_EMAIL FROM user_mstr WHERE " &
            "UM_STATUS = 'A' AND um_deleted = 'N' and UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' " &
            "and um_user_id = '" & strPO & "'"
            ds = objDB.FillDs(strsql)

            If ds.Tables(0).Rows.Count > 0 Then
                For j = 0 To ds.Tables(0).Rows.Count - 1
                    Dim objMail As New AppMail
                    'If blnRelief Then
                    '    If Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL")) = "" Then
                    '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AO_NAME")) & " (Purchasing Officer), <BR>" & strBody
                    '    Else
                    '        objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(0)("AAO_EMAIL"))
                    '        objMail.MailCc = Common.parseNull(ds.Tables(0).Rows(0)("AO_EMAIL"))
                    '        objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(0)("AAO_NAME")) & " (Purchasing Officer), <BR>" & strBody
                    '    End If
                    'Else
                    objMail.MailTo = Common.parseNull(ds.Tables(0).Rows(j)("UM_EMAIL"))
                    objMail.Body = "Dear " & Common.parseNull(ds.Tables(0).Rows(j)("UM_USER_NAME")) & " (Buyer), <BR>" & strBody
                    'End If

                    objMail.Subject = "Agora : " & strDocType & " Approved"
                    objMail.SendMail()
                Next

            End If
            objCommon = Nothing
        End Function

        '     Public Function getPRDetailsforPOApprovalSetup(ByVal strPONo As String) As DataSet
        '         Dim strsql As String
        '         Dim ds As New DataSet

        '         strsql = "SELECT PRM_BUYER_ID,PRM_REQ_NAME, AGM_GRP_NAME FROM PO_MSTR " &
        '                 "INNER JOIN RFQ_MSTR ON RM_RFQ_ID=POM_RFQ_INDEX AND RM_COY_ID=POM_B_COY_ID " &
        '                 "INNER JOIN RFQ_DETAIL ON RM_RFQ_ID=RD_RFQ_ID " &
        '                 "INNER JOIN PR_DETAILS ON PRD_PR_LINE_INDEX=RD_PR_LINE_INDEX " &
        '                 "INNER JOIN PR_MSTR ON PRM_PR_NO=PRD_PR_NO AND PRM_COY_ID=PRD_COY_ID " &
        '                 "INNER JOIN PR_APPROVAL ON PRA_PR_INDEX=PRM_PR_INDEX " &
        '                 "INNER JOIN APPROVAL_GRP_MSTR ON AGM_GRP_INDEX=PRA_APPROVAL_GRP_INDEX " &
        '                 "WHERE POM_PO_NO='" & strPONo & "' AND POM_B_COY_ID='" & HttpContext.Current.Session("CompanyId") & "' AND PRA_FOR='PR' "

        '         ds = objDb.FillDs(strsql)
        '         getPRDetailsforPOApprovalSetup = ds
        '     End Function
    End Class

End Namespace