Imports System
Imports System.Configuration
Imports System.Text
Imports System.Web
Imports System.Collections
Imports SSO.Component
Imports System.IO

Namespace AgoraNEO

    Public Class POValue

        Public bill_adds As String
        Public PO_Number As String
        Public PO_type As String
        Public PO_Status As String
        Public PO_Date As String
        Public PO_Line As String
        Public vendor_CoyId As String
        Public vendor_Coy As String
        Public vendor_adds As String
        Public vendor_tel As String
        Public vendor_fax As String
        Public vendor_contact As String
        Public vendor_email As String
        Public vendor_Item_Code As String
        Public Product_Desc As String
        Public Order_Qty As String
        Public Rec_Qty As String
        Public Rej_Qty As String
        Public Unit_Cost As String
        Public remarks As String
        Public buyer_adds As String
        Public cur As String
        Public buyer_tel As String
        Public buyer_fax As String
        Public buyer_contact As String
        Public buyer_email As String
        Public pay_term As String
        Public ship_term As String
        Public del_term As String
        Public vendor_code As String
        Public gst_code As String
        Public ship_via As String
        Public pay_meth As String
        Public ship_meth As String
        Public ex_rate As String
        Public tax As String
        Public CR_NO As String
        Public CR_ReqDate As String
        Public CR_ReqBy As String
        '//add by Moo
        Public buyer_coy As String
        Public BCoyName As String
        Public TermAndCond As String
        Public FreightTerm As String
        Public Vendor_remark As String
        '----New code Added By Praveen to get The line on 17/07/2007
        Public lineval As String
        Public linevalue(0) As String
        'Michelle (23/11/2010)
        Public POIndex As Integer
        '-----End 
        Public POM_CREATED_DATE As String
        Public urgent As String
        Public POM_ACCEPTED_DATE As String
        Public intremarks As String
        'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
        Public Submit_PO_Date As String
        Public FFPO_Type As String
        Public Created_By As String
    End Class
    Public Class PurchaseOrder
        Dim objDb As New EAD.DBCom
        'Dim v_com_id As String = HttpContext.Current.Session("CompanyId")
        'Dim com_id As String = HttpContext.Current.Session("CompanyId")
        'Dim userid As String = HttpContext.Current.Session("UserId")
        Public Enum EnumShoppingCart
            icChk = 0
            icNo = 1
            'icBuyerItemCode = 2
            icProductCode = 2
            icVendorItemCode = 3

            'Jules 2018.05.07 - PAMB Scrum 2 - Added Gift and Analysis Codes.
            icGift = 4
            icGift1 = 5
            icFundType = 6
            icPersonCode = 7
            icProjectCode = 8
            icGLCode = 9 '4
            icGLCode1 = 10 '5
            icCategoryCode1 = 11 '6
            icTaxCode = 12 '7
            icAssetGroup = 13 '8
            icItemDesc = 14 '9 '8
            icMOQ = 15 ' 10 '9
            icMPQ = 16 '11 '10
            icRfqQty = 17 '12 '11
            icTolerance = 18 '13 '12
            icQty = 19 '14 '13
            icUOM = 20 '15 '14
            icPrice = 21 '16 '15
            icTotal = 22 '17 '16
            icTaxRate = 23 '18
            icTax = 24 '19 '18 '17
            icGstTaxCode = 25 '20 'Stage 3 Enhancement (GST-0010) - 14/07/2015 - CH 
            icSource = 26 '21 '20 '19 '18
            icCategoryCode = 27 '22 '21 '20 '19
            icCDGroup = 28 '23 '22 '21 '20
            icBudget = 29 '24 '23 '22 '21
            icDelivery = 30 '25 '24 '23 '22
            icEstDate = 31 '26 '25 '24 '23
            icWarranty = 32 '27 '26 '25 '24
            ' Yap : Re-Add this 2 columns
            'icMOQ = 23 '9
            'icMPQ = 24 '10
            icRemark = 33 ' 28 '27 '26 '25        
        End Enum
        Public Function get_postatus(ByVal po_id As String) As String
            Dim strsql As String = "select STATUS_DESC from STATUS_MSTR,PO_MSTR WHERE STATUS_TYPE='PO' AND STATUS_NO=POM_PO_STATUS and POM_PO_INDEX= '" & Common.Parse(po_id) & "' "
            get_postatus = objDb.GetVal(strsql)

        End Function

        'Public Function get_po_StatusNo(ByVal po_id As String) As String
        '    Dim strsql As String = "select STATUS_NO from STATUS_MSTR,PO_MSTR WHERE STATUS_TYPE='PO' AND STATUS_NO=POM_PO_STATUS and POM_PO_INDEX= '" & Common.Parse(po_id) & "' "
        '    Return objDb.GetVal(strsql)
        'End Function
        Public Function get_po_Fulfilment(ByVal po_id As String) As String
            Dim strsql As String = "select POM_FULFILMENT FROM PO_MSTR WHERE POM_PO_INDEX= '" & Common.Parse(po_id) & "' "
            Return objDb.GetVal(strsql)
        End Function

        Public Function get_prdate(ByVal pr_id As String) As Date
            Dim strsql As String = "select PRM_CREATED_DATE from PR_MSTR where PRM_PR_Index='" & pr_id & "'"
            Return objDb.GetVal(strsql)
        End Function

        'Public Function get_comid(ByVal po_index As String) As String
        '    Dim strsql As String = "select POM_B_COY_ID from PO_MSTR WHERE POM_PO_INDEX= '" & Common.Parse(po_index) & "'"
        '    get_comid = objDb.GetVal(strsql)
        'End Function

        Public Function get_PreferVendor(ByVal pProdCode As String, ByVal pCompId As String) As String
            Dim strsql As String, ds As New DataSet, sVendorCode As String

            strsql = "select PM_PREFER_S_COY_ID from PRODUCT_MSTR "
            strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
            strsql &= "PM_PREFER_S_COY_ID = '" & Common.Parse(pCompId) & "'"
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                sVendorCode = Common.Parse(ds.Tables(0).Rows(0)("PM_PREFER_S_COY_ID"))
            End If

            If sVendorCode = "" Then 'Chk whether to get the tax from from the 1st Vendor
                strsql = "select PM_1ST_S_COY_ID from PRODUCT_MSTR "
                strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
                strsql &= "PM_1ST_S_COY_ID = '" & Common.Parse(pCompId) & "'"
                ds = objDb.FillDs(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    sVendorCode = Common.Parse(ds.Tables(0).Rows(0)("PM_1ST_S_COY_ID"))
                End If
            End If

            If sVendorCode = "" Then 'Chk whether to get the tax from from the 2nd Vendor
                strsql = "select PM_2ND_S_COY_ID from PRODUCT_MSTR "
                strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
                strsql &= "PM_2ND_S_COY_ID = '" & Common.Parse(pCompId) & "'"
                ds = objDb.FillDs(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    sVendorCode = Common.Parse(ds.Tables(0).Rows(0)("PM_2ND_S_COY_ID"))
                End If
            End If

            If sVendorCode = "" Then 'Chk whether to get the tax from from the 3rd Vendor
                strsql = "select PM_3RD_S_COY_ID from PRODUCT_MSTR "
                strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
                strsql &= "PM_3RD_S_COY_ID = '" & Common.Parse(pCompId) & "'"
                ds = objDb.FillDs(strsql)
                If ds.Tables(0).Rows.Count > 0 Then
                    sVendorCode = Common.Parse(ds.Tables(0).Rows(0)("PM_3RD_S_COY_ID"))
                End If
            End If

        End Function

        'Public Function GetQuotationPrice(ByVal sRFQ As String, ByVal sQuo As String, ByVal pComId As String, ByVal pItemNo As String, Optional ByVal vCompanyid As String = "", Optional ByVal vCode As String = "", Optional ByVal vCode2 As String = "") As DataSet
        '    Dim strSql As String
        '    Dim objDB As New EAD.DBCom

        '    If sRFQ Is Nothing Then
        '        sRFQ = ""
        '    End If

        '    strSql = "SELECT rm_rfq_id, rm_rfq_no, rrd_line_no, rrd_unit_price "
        '    strSql &= "FROM rfq_mstr "
        '    strSql &= "INNER JOIN rfq_replies_mstr ON rm_rfq_id = rrm_rfq_id "
        '    strSql &= "INNER JOIN rfq_replies_detail ON rrd_rfq_id = rrm_rfq_id  "
        '    strSql &= "WHERE rm_coy_id = '" & pComId & "' "
        '    strSql &= "AND rrm_actual_quot_num = '" & sQuo & "' "
        '    ' Modified by craven to get the selected vendor company


        '    If vCode = "&nbsp;" Then
        '        strSql &= " AND rrd_product_desc='" & vCode2 & "' "
        '    ElseIf vCode <> "" Then
        '        strSql &= " AND rrd_vendor_item_code='" & vCode & "' "
        '    End If

        '    If sRFQ <> "" Then
        '        strSql &= " AND rm_rfq_no = '" & sRFQ & "' "
        '    End If

        '    If vCompanyid <> "" Then
        '        strSql &= " AND RRD_V_COY_ID='" & vCompanyid & "'"
        '    End If

        '    Return objDB.FillDs(strSql)
        'End Function

        'Public Function get_TaxPerc(ByVal pProdCode As String, ByVal pCompID As String, ByRef pTaxId As String) As String
        '    Dim strsql As String, strPerc As String = ""
        '    Dim ds As New DataSet

        '    'Chk whether to get the tax from from the Prefer Vendor
        '    strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '    strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '    strsql &= "PM_PREFER_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '    strsql &= "PM_PREFER_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '    ds = objDb.FillDs(strsql)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '        pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '    End If


        '    If strPerc = "" Then 'Chk whether to get the tax from from the 1st Vendor
        '        strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '        strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '        strsql &= "PM_1ST_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '        strsql &= "PM_1ST_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '            pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '        End If
        '    End If

        '    If strPerc = "" Then 'Chk whether to get the tax from from the 2nd Vendor
        '        strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '        strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '        strsql &= "PM_2ND_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '        strsql &= "PM_2ND_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '            pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '        End If
        '    End If

        '    If strPerc = "" Then 'Chk whether to get the tax from from the 3rd Vendor
        '        strsql = "select TAX_PERC, TAX_AUTO_NO from PRODUCT_MSTR, TAX "
        '        strsql &= "WHERE PM_PRODUCT_CODE= '" & Common.Parse(pProdCode) & "' AND "
        '        strsql &= "PM_3RD_S_COY_ID = '" & Common.Parse(pCompID) & "' AND "
        '        strsql &= "PM_3RD_S_COY_ID_TAX_ID = TAX_AUTO_NO "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            strPerc = Common.Parse(ds.Tables(0).Rows(0)("TAX_PERC"))
        '            pTaxId = Common.Parse(ds.Tables(0).Rows(0)("TAX_AUTO_NO"))
        '        End If
        '    End If

        '    If strPerc = "" Then
        '        Return "0"
        '    Else
        '        Return strPerc
        '    End If
        'End Function

        'Public Function get_EDDPerc(ByVal pProdCode As String, ByVal pCompID As String) As String
        '    Dim strsql As String, iEDD As String = ""
        '    Dim ds As New DataSet

        '    Dim INDEX As String
        '    strsql = "SELECT PM_PRODUCT_INDEX FROM PRODUCT_MSTR  "
        '    strsql &= "WHERE PM_PRODUCT_CODE = '" & Common.parseNull(pProdCode) & "'"

        '    INDEX = objDb.GetVal(strsql)

        '    'Chk whether to get the tax from from the Prefer Vendor
        '    strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '    strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '    strsql &= "PM_PREFER_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '    strsql &= "PV_VENDOR_TYPE = 'P' AND "
        '    strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '    ds = objDb.FillDs(strsql)
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '    End If


        '    If iEDD = "" Then 'Chk whether to get the tax from from the 1st Vendor
        '        strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '        strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '        strsql &= "PM_1ST_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '        strsql &= "PV_VENDOR_TYPE = '1' AND "
        '        strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '        End If

        '    End If

        '    If iEDD = "" Then 'Chk whether to get the tax from from the 2nd Vendor
        '        strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '        strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '        strsql &= "PM_2ND_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '        strsql &= "PV_VENDOR_TYPE = '2' AND "
        '        strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '        End If
        '    End If

        '    If iEDD = "" Then 'Chk whether to get the tax from from the 3rd Vendor
        '        strsql = "select PV_LEAD_TIME from PRODUCT_MSTR, PIM_VENDOR "
        '        strsql &= "WHERE PV_PRODUCT_INDEX= '" & INDEX & "' AND "
        '        strsql &= "PM_3RD_S_COY_ID = '" & Common.parseNull(pCompID) & "' AND "
        '        strsql &= "PV_VENDOR_TYPE = '3' AND "
        '        strsql &= "PM_PRODUCT_INDEX = PV_PRODUCT_INDEX "
        '        ds = objDb.FillDs(strsql)
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            iEDD = Common.parseNull(ds.Tables(0).Rows(0)("PV_LEAD_TIME"))
        '        End If
        '    End If

        '    If iEDD = "" Then
        '        Return "0"
        '    Else
        '        Return iEDD
        '    End If
        'End Function

        'Michelle (19/11/2010) - To get the PO for Vendor to accept and acknowledge
        'Public Function getPOForAck() As DataSet
        '    Dim ds As DataSet
        '    Dim strsql As String
        '    strsql = "SELECT POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE, " & _
        '    "PCM_CR_NO AS CR_NO, " & _
        '    "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " & _
        '    "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " & _
        '    "AS STATUS_DESC FROM PO_MSTR POM INNER JOIN COMPANY_MSTR CM ON POM.POM_B_COY_ID = CM.CM_COY_ID " & _
        '    "LEFT OUTER JOIN PO_CR_MSTR ON PCM_PO_INDEX = POM.POM_PO_INDEX " & _
        '    "WHERE (POM.POM_PO_STATUS IN('1','2') OR (POM.POM_PO_STATUS = '5' AND POM.POM_FULFILMENT = '4') OR (POM.POM_PO_STATUS = '3' AND POM.POM_FULFILMENT = '4')) " & _
        '    "AND POM_S_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' ORDER BY POM.POM_PO_DATE "

        '    ds = objDb.FillDs(strsql)
        '    Return ds

        'End Function
        Public Function VIEW_POList(ByVal userID As String, ByVal companyID As String, ByVal PO_Status As String, ByVal fulfilment As String, ByVal side As String, Optional ByVal ven_name As String = "", Optional ByVal po_no As String = "", Optional ByVal startdate As String = "",
                                    Optional ByVal enddate As String = "", Optional ByVal BUYER_COMNAME As String = "", Optional ByVal Buyer_Status As String = "", Optional ByVal str_itemcode As String = "", Optional ByVal PO_type As String = "") As String

            Dim ds As DataSet
            Dim strTemp As String
            Dim strsql As String
            '//Remark By Moo, query wrong
            'strsql = "select Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE,POM.POM_PO_STATUS" & _
            '",POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT ," & _
            '"(SELECT STATUS_DESC FROM STATUS_MSTR WHERE STATUS_TYPE IN ('Fufilment') and POM.POM_FULFILMENT IN(" & fulfilment & ")) AS REMARK1," & _
            '"B.STATUS_DESC" & _
            '" FROM PO_MSTR POM " & _
            '" LEFT JOIN STATUS_MSTR AS A ON A.STATUS_NO = POM_FULFILMENT AND A.STATUS_TYPE = 'Fufilment' " & _
            '" LEFT JOIN STATUS_MSTR AS B ON B.STATUS_NO = POM_PO_STATUS AND B.STATUS_TYPE = 'PO' " & _
            '" WHERE (POM.POM_FULFILMENT IN(" & fulfilment & ")" & _
            '" and POM.POM_PO_STATUS IN(" & PO_Status & ")) "

            If Buyer_Status = "ByBuyerAdmin" Then
                strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," &
                           "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_CREATED_DATE,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , pom.pom_po_type, " &
                           "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
                           "AS REMARK1, " &
                           "CASE WHEN (PCM.PCM_REQ_BY) ='" & Common.Parse(userID) & "'" &
                           "THEN 'CANCELED BY BA' ELSE (SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND " &
                           "C.STATUS_NO=POM.POM_PO_STATUS) END AS STATUS_DESC, (SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_PO_INDEX = POM.POM_PO_INDEX AND PRM_COY_ID = POM.POM_B_COY_ID AND PRM_PR_TYPE='CC') PR_NO, PCM.PCM_REQ_BY, POM.POM_URGENT, " &
                           "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = POM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(companyID) & "') AS NAME " &
                           "FROM PO_MSTR POM " &
                           "JOIN COMPANY_MSTR CM " &
                           "LEFT JOIN PO_CR_MSTR PCM ON PCM.PCM_PO_INDEX = POM.POM_PO_INDEX " &
                           "WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" &
                           " And POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID "
                'strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," & _
                '          "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " & _
                '          "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " & _
                '          "AS REMARK1, " & _
                '          "CASE WHEN (PCM.PCM_REQ_BY) ='" & Common.Parse(userid) & "'" & _
                '          "THEN 'CANCELED BY BA' ELSE (SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND " & _
                '          "C.STATUS_NO=POM.POM_PO_STATUS) END AS STATUS_DESC, PCM.PCM_REQ_BY " & _
                '          "FROM PO_MSTR POM " & _
                '          "JOIN COMPANY_MSTR CM " & _
                '          "LEFT JOIN PO_CR_MSTR PCM ON PCM.PCM_PO_INDEX = POM.POM_PO_INDEX " & _
                '          "WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" & _
                '          " And POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID "
            Else
                strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," &
                           "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_CREATED_DATE,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , pom.pom_po_type, " &
                           "IFNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
                           "AS REMARK1," &
                           "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " &
                           "AS STATUS_DESC, (SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_PO_INDEX = POM.POM_PO_INDEX AND PRM_COY_ID = POM.POM_B_COY_ID AND PRM_PR_TYPE='CC') PR_NO, POM.POM_URGENT, " &
                           "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = POM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(companyID) & "') AS NAME " &
                           "FROM PO_MSTR POM,COMPANY_MSTR CM "

                If str_itemcode <> "" Then
                    strTemp = Common.BuildWildCard(str_itemcode)
                    strsql = strsql & ", (SELECT DISTINCT POD_PO_NO FROM PO_Details WHERE POD_COY_ID = '" & Common.Parse(companyID) & "' AND POD_VENDOR_ITEM_CODE " & Common.ParseSQL(strTemp) & ") AS POD "
                End If
                strsql = strsql & "WHERE 1=1 "
                strsql = strsql & "AND POM.POM_FULFILMENT IN(" & fulfilment & ")"
                strsql = strsql & " And POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID "



                If str_itemcode <> "" Then
                    strsql = strsql & "AND POD.POD_PO_NO = POM.POM_PO_NO "
                End If
                'strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," & _
                '           "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " & _
                '           "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " & _
                '           "AS REMARK1," & _
                '           "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " & _
                '           "AS STATUS_DESC FROM PO_MSTR POM,COMPANY_MSTR CM " & _
                '           "WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" & _
                '           " And POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID "
            End If


            If side = "u" Then
                strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "' and POM_BUYER_ID = '" & Common.Parse(userID) & "'"
                If ven_name <> "" Then
                    strTemp = Common.BuildWildCard(ven_name)
                    strsql = strsql & " POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    'strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
                End If
            End If

            If side = "b" Then
                Dim objUser As New Users
                Dim IsBoolean As Boolean

                If PO_type = "AllPO" Then
                    IsBoolean = objUser.BAdminRole(Common.Parse(userID), Common.Parse(companyID))
                Else
                    IsBoolean = False
                End If

                If IsBoolean Then
                    If Buyer_Status = "ByBuyerAdmin" Then

                        strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "' and PCM_REQ_BY = '" & Common.Parse(userID) & "' "

                    Else
                        strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "'"
                    End If
                Else
                    strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "'"
                    strsql = strsql & " and POM.POM_BUYER_ID = '" & Common.Parse(userID) & "' "
                End If
                If ven_name <> "" Then
                    strTemp = Common.BuildWildCard(ven_name)
                    strsql = strsql & " AND POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    'strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
                End If
            ElseIf side = "v" Then
                If ven_name <> "" Then ' ven_name have buyer name value
                    strTemp = Common.BuildWildCard(ven_name)
                    strsql = strsql & " AND POM.POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    'strsql = strsql & " and POM.POM_BUYER_NAME" & Common.ParseSQL(ven_name)
                End If
                If BUYER_COMNAME <> "" Then
                    strTemp = Common.BuildWildCard(BUYER_COMNAME)
                    strsql = strsql & " and CM.CM_COY_NAME" & Common.ParseSQL(strTemp)
                End If
                strsql = strsql & " AND POM.POM_S_COY_ID='" & Common.Parse(companyID) & "'"
            End If

            If po_no <> "" Then
                strTemp = Common.BuildWildCard(po_no)
                strsql = strsql & " and POM.POM_PO_NO " & Common.ParseSQL(strTemp) & " "
            End If
            If startdate <> "" Then
                strsql &= "AND POM.POM_CREATED_DATE >= " & Common.ConvertDate(startdate) & " "
            End If

            If enddate <> "" Then
                strsql &= "AND POM.POM_CREATED_DATE <= " & Common.ConvertDate(enddate & " 23:59:59.000") & " "
            End If
            strsql &= "ORDER BY POM_CREATED_DATE DESC"
            'ds = objDb.FillDs(strsql)
            Return strsql

        End Function

        Public Function VIEW_POListTrx(ByVal userID As String, ByVal companyID As String, ByVal PO_Status As String, ByVal fulfilment As String, ByVal side As String, Optional ByVal ven_name As String = "", Optional ByVal po_no As String = "", Optional ByVal startdate As String = "", Optional ByVal enddate As String = "", Optional ByVal BUYER_COMNAME As String = "", Optional ByVal Buyer_Status As String = "") As DataSet


            Dim ds As DataSet
            Dim strTemp As String
            Dim strsql As String

            If Buyer_Status = "ByBuyerAdmin" Then
                strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_CREATED_DATE,POM.POM_SUBMIT_DATE,POM.POM_PO_DATE," &
                           "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_COY_ID = '" & Common.Parse(companyID) & "' AND UM_USER_ID = POM.POM_CREATED_BY) AS POM.POM_CREATED_BY,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " &
                           "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
                           "AS REMARK1, " &
                           "CASE WHEN (PCM.PCM_REQ_BY) ='" & Common.Parse(userID) & "'" &
                           "THEN 'CANCELED BY BA' ELSE (SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND " &
                           "C.STATUS_NO=POM.POM_PO_STATUS) END AS STATUS_DESC, (SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_PO_INDEX = POM.POM_PO_INDEX AND PRM_COY_ID = POM.POM_B_COY_ID AND PRM_PR_TYPE='CC') PR_NO, PCM.PCM_REQ_BY, POM.POM_URGENT, " &
                           "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = POM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(companyID) & "') AS NAME " &
                           "FROM PO_MSTR POM " &
                           "JOIN COMPANY_MSTR CM " &
                           "LEFT JOIN PO_CR_MSTR PCM ON PCM.PCM_PO_INDEX = POM.POM_PO_INDEX " &
                           "WHERE POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID "
            Else
                'Zulham 30112018
                strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_CREATED_DATE,POM.POM_SUBMIT_DATE,POM.POM_PO_DATE," &
                           "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_COY_ID = '" & Common.Parse(companyID) & "' AND UM_USER_ID = POM.POM_CREATED_BY) AS POM_CREATED_BY,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " &
                           "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
                           "AS REMARK1," &
                           "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " &
                           "AS STATUS_DESC, (SELECT PRM_PR_NO FROM PR_MSTR WHERE PRM_PO_INDEX = POM.POM_PO_INDEX AND PRM_COY_ID = POM.POM_B_COY_ID AND PRM_PR_TYPE='CC') PR_NO, POM.POM_URGENT, " &
                           "(SELECT UM_USER_NAME FROM USER_MSTR WHERE UM_USER_ID = POM.POM_STATUS_CHANGED_BY AND UM_COY_ID  = '" & Common.Parse(companyID) & "') AS NAME " &
                           "FROM PO_MSTR POM,COMPANY_MSTR CM " &
                           "WHERE POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID AND POM.POM_CREATED_BY = '" & Common.Parse(userID) & "' AND POM_BUYER_ID = '" & Common.Parse(userID) & "' "
            End If


            If side = "u" Then
                strsql = strsql & " AND POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "' "
                If ven_name <> "" Then
                    strTemp = Common.BuildWildCard(ven_name)
                    strsql = strsql & " AND POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    'strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
                End If
            End If

            'If side = "b" Then
            '    Dim objUser As New Users
            '    Dim IsBoolean As String = objUser.BAdminRole(Common.Parse(userid), Common.Parse(com_id))

            '    If IsBoolean Then
            '        If Buyer_Status = "ByBuyerAdmin" Then

            '            strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and PCM_REQ_BY = '" & Common.Parse(userid) & "' "

            '        Else
            '            strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
            '        End If
            '    Else
            '        strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
            '        strsql = strsql & " and POM.POM_BUYER_ID = '" & HttpContext.Current.Session("UserId") & "' "
            '    End If
            '    If ven_name <> "" Then
            '        strTemp = Common.BuildWildCard(ven_name)
            '        strsql = strsql & " AND POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
            '        'strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
            '    End If
            'ElseIf side = "v" Then
            '    If ven_name <> "" Then ' ven_name have buyer name value
            '        strTemp = Common.BuildWildCard(ven_name)
            '        strsql = strsql & " AND POM.POM_BUYER_NAME" & Common.ParseSQL(strTemp)
            '        'strsql = strsql & " and POM.POM_BUYER_NAME" & Common.ParseSQL(ven_name)
            '    End If
            '    If BUYER_COMNAME <> "" Then
            '        strTemp = Common.BuildWildCard(BUYER_COMNAME)
            '        strsql = strsql & " and CM.CM_COY_NAME" & Common.ParseSQL(strTemp)
            '    End If
            '    strsql = strsql & " AND POM.POM_S_COY_ID='" & Common.Parse(v_com_id) & "'"
            'End If

            If po_no <> "" Then
                strTemp = Common.BuildWildCard(po_no)
                strsql = strsql & " and POM.POM_PO_NO " & Common.ParseSQL(strTemp) & " "
            End If
            If startdate <> "" Then
                strsql &= "AND POM.POM_CREATED_DATE >= " & Common.ConvertDate(startdate) & " "
            End If

            If enddate <> "" Then
                strsql &= "AND POM.POM_CREATED_DATE <= " & Common.ConvertDate(enddate & " 23:59:59.000") & " "
            End If
            ds = objDb.FillDs(strsql)
            Return ds

        End Function

        Public Function VIEW_POList_NoPR(ByVal userID As String, ByVal companyID As String, ByVal PO_Status As String, ByVal fulfilment As String, ByVal side As String, Optional ByVal ven_name As String = "", Optional ByVal po_no As String = "", Optional ByVal startdate As String = "", Optional ByVal enddate As String = "", Optional ByVal BUYER_COMNAME As String = "", Optional ByVal Buyer_Status As String = "") As DataSet


            Dim ds As DataSet
            Dim strTemp As String
            Dim strsql As String
            '//Remark By Moo, query wrong
            'strsql = "select Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE,POM.POM_PO_STATUS" & _
            '",POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT ," & _
            '"(SELECT STATUS_DESC FROM STATUS_MSTR WHERE STATUS_TYPE IN ('Fufilment') and POM.POM_FULFILMENT IN(" & fulfilment & ")) AS REMARK1," & _
            '"B.STATUS_DESC" & _
            '" FROM PO_MSTR POM " & _
            '" LEFT JOIN STATUS_MSTR AS A ON A.STATUS_NO = POM_FULFILMENT AND A.STATUS_TYPE = 'Fufilment' " & _
            '" LEFT JOIN STATUS_MSTR AS B ON B.STATUS_NO = POM_PO_STATUS AND B.STATUS_TYPE = 'PO' " & _
            '" WHERE (POM.POM_FULFILMENT IN(" & fulfilment & ")" & _
            '" and POM.POM_PO_STATUS IN(" & PO_Status & ")) "

            If Buyer_Status = "ByBuyerAdmin" Then
                strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," &
                           "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " &
                           "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
                           "AS REMARK1, " &
                           "CASE WHEN (PCM.PCM_REQ_BY) ='" & Common.Parse(userID) & "'" &
                           "THEN 'CANCELED BY BA' ELSE (SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND " &
                           "C.STATUS_NO=POM.POM_PO_STATUS) END AS STATUS_DESC, PCM.PCM_REQ_BY, POM.POM_URGENT " &
                           "FROM PO_MSTR POM " &
                           "JOIN COMPANY_MSTR CM " &
                           "LEFT JOIN PO_CR_MSTR PCM ON PCM.PCM_PO_INDEX = POM.POM_PO_INDEX " &
                           "WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" &
                           " And POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID "
            Else
                strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," &
                           "POM.POM_PO_STATUS, CM.CM_COY_NAME,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " &
                           "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " &
                           "AS REMARK1," &
                           "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " &
                           "AS STATUS_DESC, POM.POM_URGENT FROM PO_MSTR POM,COMPANY_MSTR CM " &
                           "WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" &
                           " And POM.POM_PO_STATUS IN(" & PO_Status & ") AND CM_COY_ID=POM_B_COY_ID "
            End If


            If side = "u" Then
                strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "' and POM_BUYER_ID = '" & Common.Parse(userID) & "'"
                If ven_name <> "" Then
                    strTemp = Common.BuildWildCard(ven_name)
                    strsql = strsql & " POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    'strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
                End If
            End If

            If side = "b" Then
                Dim objUser As New Users
                Dim IsBoolean As String = objUser.BAdminRole(Common.Parse(userID), Common.Parse(companyID))

                If IsBoolean Then
                    If Buyer_Status = "ByBuyerAdmin" Then

                        strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "' and PCM_REQ_BY = '" & Common.Parse(userID) & "' "

                    Else
                        strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "'"
                    End If
                Else
                    strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(companyID) & "'"
                    strsql = strsql & " and POM.POM_BUYER_ID = '" & Common.Parse(userID) & "' "
                End If
                If ven_name <> "" Then
                    strTemp = Common.BuildWildCard(ven_name)
                    strsql = strsql & " AND POM.POM_S_COY_NAME" & Common.ParseSQL(strTemp)
                    'strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
                End If
            ElseIf side = "v" Then
                If ven_name <> "" Then ' ven_name have buyer name value
                    strTemp = Common.BuildWildCard(ven_name)
                    strsql = strsql & " AND POM.POM_BUYER_NAME" & Common.ParseSQL(strTemp)
                    'strsql = strsql & " and POM.POM_BUYER_NAME" & Common.ParseSQL(ven_name)
                End If
                If BUYER_COMNAME <> "" Then
                    strTemp = Common.BuildWildCard(BUYER_COMNAME)
                    strsql = strsql & " and CM.CM_COY_NAME" & Common.ParseSQL(strTemp)
                End If
                strsql = strsql & " AND POM.POM_S_COY_ID='" & Common.Parse(companyID) & "'"
            End If

            If po_no <> "" Then
                strTemp = Common.BuildWildCard(po_no)
                strsql = strsql & " and POM.POM_PO_NO " & Common.ParseSQL(strTemp) & " "
            End If
            If startdate <> "" Then
                strsql &= "AND POM.POM_PO_DATE >= " & Common.ConvertDate(startdate) & " "
            End If

            If enddate <> "" Then
                strsql &= "AND POM.POM_PO_DATE <= " & Common.ConvertDate(enddate & " 23:59:59.000") & " "
            End If
            ds = objDb.FillDs(strsql)
            Return ds

        End Function

        ''//PO List - for PO Cancellation
        'Public Function VIEW_POList2(ByVal PO_Status As String, ByVal fulfilment As String, ByVal side As String, Optional ByVal ven_name As String = "", Optional ByVal po_no As String = "", Optional ByVal startdate As String = "", Optional ByVal enddate As String = "") As DataSet
        '    Dim ds As DataSet

        '    Dim strsql As String

        '    strsql = "SELECT Distinct POM.POM_S_COY_ID, POM.POM_PO_INDEX,POM.POM_PO_NO,POM.POM_PO_DATE," & _
        '    "POM.POM_PO_STATUS,POM.POM_BUYER_NAME,POM.POM_S_COY_NAME,POM.POM_ACCEPTED_DATE,POM.POM_B_COY_ID,POM.POM_FULFILMENT , " & _
        '    "ISNULL((SELECT STATUS_DESC FROM STATUS_MSTR B WHERE STATUS_TYPE ='Fulfilment' AND B.STATUS_NO=POM.POM_FULFILMENT),'-') " & _
        '    "AS REMARK1," & _
        '    "(SELECT STATUS_DESC FROM STATUS_MSTR C WHERE STATUS_TYPE ='PO' AND C.STATUS_NO=POM.POM_PO_STATUS) " & _
        '    "AS STATUS_DESC FROM PO_MSTR POM " & _
        '    " WHERE POM.POM_FULFILMENT IN(" & fulfilment & ")" & _
        '    " And POM.POM_PO_STATUS IN(" & PO_Status & ") " & _
        '    " and " & _
        '    " ( (POM_BILLING_METHOD='DO' )" & _
        '    " or ( POM_BILLING_METHOD in('FPO','GRN') AND NOT EXISTS (SELECT '*' FROM DO_MSTR DOM WHERE POM.POM_PO_INDEX=DOM.DOM_PO_INDEX " & _
        '    " AND DOM.DOM_DO_STATUS=2)))"
        '    If side = "u" Then
        '        strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and POM_BUYER_ID = '" & Common.Parse(userid) & "'"
        '        If ven_name <> "" Then
        '            strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
        '        End If
        '    End If

        '    If side = "b" Then
        '        'Michelle (6/2/2010) - For BA, retrieve all POs
        '        Dim objUser As New Users
        '        Dim IsBoolean As String = objUser.BAdminRole(Common.Parse(userid), Common.Parse(com_id))

        '        If IsBoolean Then
        '            strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '        Else
        '            strsql = strsql & " and POM.POM_B_COY_ID = '" & Common.Parse(com_id) & "' and POM_BUYER_ID = '" & Common.Parse(userid) & "' "
        '        End If

        '        If ven_name <> "" Then
        '            strsql = strsql & " AND POM.POM_S_COY_NAME " & Common.ParseSQL(ven_name) & " "
        '        End If
        '    ElseIf side = "v" Then
        '        strsql = strsql & " AND POM.POM_S_COY_ID= '" & Common.Parse(v_com_id) & "'"
        '        '//add by Moo, to filter buyer name
        '        If ven_name <> "" Then
        '            strsql = strsql & " AND POM.POM_BUYER_NAME " & Common.ParseSQL(ven_name) & " "
        '        End If
        '    End If

        '    If po_no <> "" Then
        '        strsql = strsql & " and POM.POM_PO_NO " & Common.ParseSQL(po_no) & " "
        '    End If
        '    If startdate <> "" Then
        '        strsql &= "AND POM.POM_PO_DATE >= " & Common.ConvertDate(startdate) & " "
        '    End If

        '    If enddate <> "" Then
        '        strsql &= "AND POM.POM_PO_DATE <= " & Common.ConvertDate(enddate & " 23:59:59.000") & " "
        '    End If
        '    ds = objDb.FillDs(strsql)
        '    Return ds

        'End Function

        Public Function get_docitem(ByVal po_no As String, ByVal side As String, ByVal b_com_id As String, ByVal companyID As String) As String

            Dim strsql As String
            Dim ds As DataSet

            'strsql = "select DISTINCT DOM.DOM_DO_NO,DOM.DOM_CREATED_DATE AS CREATIONDATE, DOM_DO_DATE AS SUBMITIONDATE,DOM.DOM_CREATED_BY," & _
            '        "GM.GM_GRN_NO, GM.GM_CREATED_DATE, GM.GM_CREATED_BY " & _
            '        "FROM  GRN_MSTR GM INNER JOIN " & _
            '          "COMPANY_DOC_MATCH CDM ON GM.GM_GRN_NO = CDM.CDM_GRN_NO INNER JOIN " & _
            '          "DO_MSTR DOM ON CDM.CDM_DO_NO = DOM.DOM_DO_NO " & _
            '        "WHERE CDM.CDM_PO_NO='" & po_no & "'"

            '//MODIFY BY MOO 10/12/2004
            '//modify by meilai 14/2/2005 should display user name for Created By
            'strsql = "select DISTINCT DOM.DOM_DO_NO,DOM.DOM_CREATED_DATE AS CREATIONDATE, DOM_DO_DATE AS SUBMITIONDATE,DOM.DOM_CREATED_BY," _
            '& "GM.GM_GRN_NO, GM.GM_CREATED_DATE, GM.GM_CREATED_BY,GM_B_COY_ID,DOM_S_COY_ID FROM  COMPANY_DOC_MATCH CDM INNER JOIN GRN_MSTR GM ON " _
            '& "GM.GM_GRN_NO = CDM.CDM_GRN_NO And CDM.CDM_B_COY_ID = GM.GM_B_COY_ID " _
            '& "INNER JOIN DO_MSTR DOM ON CDM.CDM_DO_NO = DOM.DOM_DO_NO AND CDM.CDM_S_COY_ID = DOM.DOM_S_COY_ID " _
            '& "WHERE CDM.CDM_PO_NO='" & po_no & "' and CDM_B_COY_ID='" & b_com_id & "'"

            strsql = "select DISTINCT DOM.DOM_DO_NO,DOM.DOM_CREATED_DATE AS CREATIONDATE, DOM_DO_DATE AS SUBMITIONDATE," &
                     "DOM.DOM_CREATED_BY,GM.GM_GRN_NO, GM.GM_CREATED_DATE, GM.GM_DATE_RECEIVED,GM.GM_CREATED_BY,GM_B_COY_ID,DOM_S_COY_ID," &
                     "UM.UM_USER_NAME AS DO_CREATED_BY, UM2.UM_USER_NAME AS GM1_CREATED_BY " &
                     "FROM COMPANY_DOC_MATCH CDM " &
                     "INNER JOIN DO_MSTR DOM ON CDM.CDM_DO_NO = DOM.DOM_DO_NO AND CDM.CDM_S_COY_ID = DOM.DOM_S_COY_ID " &
                     " left JOIN GRN_MSTR GM ON GM.GM_GRN_NO = CDM.CDM_GRN_NO " &
                     " And CDM.CDM_B_COY_ID = GM.GM_B_COY_ID " &
                     "LEFT JOIN USER_MSTR UM ON UM.UM_USER_ID = DOM.DOM_CREATED_BY AND UM.UM_COY_ID = DOM.DOM_S_COY_ID " &
                     "LEFT JOIN USER_MSTR UM2 ON UM2.UM_USER_ID = GM.GM_CREATED_BY AND UM2.UM_COY_ID = GM.GM_B_COY_ID " &
                     "WHERE CDM.CDM_PO_NO= '" & Common.Parse(po_no) & "' and CDM_B_COY_ID= '" & Common.Parse(b_com_id) & "'"
            '//close meilai

            ' BY Gary 31/12/2004
            If side = "b" Then
                'strsql = strsql & " and CDM_B_COY_ID='" & com_id & "'"
            ElseIf side = "v" Then
                strsql = strsql & " and CDM_S_COY_ID= '" & Common.Parse(companyID) & "'"

            End If

            ds = objDb.FillDs(strsql)
            Return strsql

        End Function

        'Public Function update_ack(ByVal DS As DataSet) As String
        '    Dim strsql As String
        '    Dim I As Integer
        '    Dim strarray(0) As String
        '    Dim objDO As New DeliveryOrder
        '    For I = 0 To DS.Tables(0).Rows.Count - 1
        '        strsql = "update PO_CR_MSTR set PCM_CR_STATUS= '" & Common.Parse(DS.Tables(0).Rows(I)("CRStatus")) & "' " & _
        '                " where PCM_CR_NO= '" & Common.Parse(DS.Tables(0).Rows(I)("cr_num")) & "' and PCM_S_COY_ID = '" & Common.Parse(v_com_id) & "' " & _
        '                " and PCM_B_COY_ID= '" & Common.Parse(DS.Tables(0).Rows(I)("bcomid")) & "'"
        '        Common.Insert2Ary(strarray, strsql)
        '        strsql = objDO.SetPOFulFilment(DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("bcomid"))
        '        Common.Insert2Ary(strarray, strsql)
        '    Next
        '    objDO = Nothing
        '    If objDb.BatchExecute(strarray) Then
        '        '//lacking -- PO No and Buyer Company
        '        Dim objMail As New Email
        '        Dim objPO As New PurchaseOrder
        '        Dim objUsers As New Users

        '        For I = 0 To DS.Tables(0).Rows.Count - 1
        '            'Michelle (6/2/2010) - If PO is cancelled by Buyer Admin, then send email to Buyer Admin and Buyer
        '            Dim strCRUser As String = objPO.get_cancelReq(DS.Tables(0).Rows(I)("cr_num"), DS.Tables(0).Rows(I)("bcomid"))
        '            Dim IsBA As Boolean
        '            If HttpContext.Current.Session("Env") <> "FTN" Then

        '                IsBA = objUsers.BAdminRole(strCRUser, DS.Tables(0).Rows(I)("bcomid"))
        '            Else
        '                IsBA = False
        '            End If

        '            If IsBA Then
        '                objMail.sendNotification(EmailType.AckPOCancellationRequest, userid, DS.Tables(0).Rows(I)("bcomid"), v_com_id, DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("cr_num"), "ToBuyer", strCRUser)
        '                objMail.sendNotification(EmailType.AckPOCancellationRequest, userid, DS.Tables(0).Rows(I)("bcomid"), v_com_id, DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("cr_num"), "BACancel", strCRUser)
        '            Else
        '                objMail.sendNotification(EmailType.AckPOCancellationRequest, userid, DS.Tables(0).Rows(I)("bcomid"), v_com_id, DS.Tables(0).Rows(I)("po_no"), DS.Tables(0).Rows(I)("cr_num"), "BuyerCancel", "")
        '            End If

        '            If update_ack = "" Then
        '                update_ack = DS.Tables(0).Rows(I)("cr_num")
        '            Else
        '                update_ack = update_ack & "," & Common.Parse(DS.Tables(0).Rows(I)("cr_num")) & ""
        '            End If
        '            'update_ack = update_ack & " has been Acknowledged."
        '        Next
        '        objMail = Nothing
        '    Else
        '        update_ack = "Error occurs. Kindly contact the Administrator to resolve this."
        '    End If
        'End Function
        Public Function get_CRView(ByVal po_no As String, ByVal cr_no As String, ByVal side As String, ByVal bcom_id As String, ByVal userID As String, ByVal companyID As String, Optional ByVal cr_status As String = "") As String

            Dim strsql As String

            strsql = "select DISTINCT PCM.*,SM.STATUS_DESC,UM.UM_USER_NAME,CM.CM_COY_NAME,POM.POM_PO_INDEX,POM.POM_PO_STATUS,POM.POM_PO_NO,POM_B_COY_ID,POM_S_COY_NAME" &
                    " from PO_CR_MSTR PCM,STATUS_MSTR SM,USER_MSTR UM,COMPANY_MSTR CM,PO_MSTR POM " &
                    " WHERE SM.STATUS_NO=PCM.PCM_CR_STATUS AND SM.STATUS_TYPE='CR' " &
                    "AND CM_COY_ID=PCM_B_COY_ID AND POM.POM_PO_INDEX=PCM.PCM_PO_INDEX and UM_USER_ID=PCM_REQ_BY AND UM_COY_ID=PCM_B_COY_ID "
            If bcom_id <> "" Then
                strsql = strsql & " AND PCM_B_COY_ID = '" & Common.Parse(bcom_id) & "'"
            End If

            If side = "b" Then
                strsql = strsql & " AND  PCM.PCM_REQ_BY = '" & Common.Parse(userID) & "' " &
                                    " and UM_COY_ID= PCM_B_COY_ID "
            ElseIf side = "v" Then
                strsql = strsql & " and PCM_S_COY_ID= '" & Common.Parse(companyID) & "'" &
                                 " and UM_COY_ID= PCM_B_COY_ID"
            End If
            If cr_status <> "" Then
                strsql = strsql & " and  PCM_CR_STATUS in(" & cr_status & ")"
            End If
            If po_no <> "" Then
                strsql = strsql & " and POM.POM_PO_NO" & Common.ParseSQL(po_no)
            End If
            If cr_no <> "" Then
                strsql = strsql & " and PCM.PCM_CR_NO" & Common.ParseSQL(cr_no)
            End If
            Dim DS As DataSet
            DS = objDb.FillDs(strsql)

            Return strsql

        End Function

        Public Function get_CRView2(ByVal userID As String, ByVal companyID As String, ByVal po_no As String, ByVal cr_no As String, ByVal side As String, ByVal bcom_id As String, Optional ByVal cr_status As String = "") As DataSet

            Dim strsql As String

            strsql = "select DISTINCT PCM.*,SM.STATUS_DESC,UM.UM_USER_NAME,CM.CM_COY_NAME,POM.POM_PO_NO" &
                    " from PO_CR_MSTR PCM,STATUS_MSTR SM,USER_MSTR UM,COMPANY_MSTR CM,PO_MSTR POM " &
                    " WHERE SM.STATUS_NO=PCM.PCM_CR_STATUS AND SM.STATUS_TYPE='CR' " &
                    "AND CM_COY_ID=PCM_B_COY_ID AND POM.POM_PO_INDEX=PCM.PCM_PO_INDEX AND UM_USER_ID=PCM_REQ_BY AND UM_COY_ID=PCM_B_COY_ID "
            If bcom_id <> "" Then
                strsql = strsql & " AND PCM_B_COY_ID = '" & Common.Parse(bcom_id) & "'"
            End If

            If side = "b" Then
                strsql = strsql & " AND  PCM.PCM_REQ_BY = '" & Common.Parse(userID) & "' " &
                                " and UM_COY_ID= PCM_B_COY_ID "
            ElseIf side = "v" Then
                strsql = strsql & " and PCM_S_COY_ID= '" & Common.Parse(companyID) & "'" &
                                 " and UM_COY_ID= PCM_B_COY_ID"
            End If
            If cr_status <> "" Then
                strsql = strsql & " and  PCM_CR_STATUS in(" & cr_status & ")"
            End If
            If po_no <> "" Then
                strsql = strsql & " and POM.POM_PO_NO" & Common.ParseSQL(po_no)
            End If
            If cr_no <> "" Then
                strsql = strsql & " and PCM.PCM_CR_NO" & Common.ParseSQL(cr_no)
            End If
            Dim DS As DataSet
            DS = objDb.FillDs(strsql)

            Return DS

        End Function

        'Public Function update_cancellation(ByVal ds As DataSet, ByRef CR_num As String, ByRef CancelBfToVendor As String) As Boolean
        '    Dim OBJGLB As New AppGlobals
        '    Dim strArray(0), strArray1(0) As String
        '    Dim strsql As String
        '    Dim prefix As String
        '    Dim qty As Integer
        '    Dim crstatus1 As Integer = CRStatus.newCR

        '    OBJGLB.GetLatestDocNo("CR", strArray, CR_num, prefix)
        '    strsql = "SELECT '*' FROM PO_CR_MSTR WHERE PCM_CR_NO= '" & Common.Parse(CR_num) & "' AND PCM_B_COY_ID= '" & Common.Parse(com_id) & "'"
        '    If objDb.Exist(strsql) Then
        '        Return False
        '    End If
        '    strsql = "insert into PO_CR_MSTR (PCM_CR_NO,PCM_B_COY_ID,PCM_S_COY_ID,PCM_PO_INDEX,PCM_CR_STATUS,PCM_REQ_BY,PCM_REQ_DATE,PCM_CR_REMARKS) values " & _
        '                "('" & Common.Parse(CR_num) & "','" & Common.Parse(com_id) & "','" & Common.Parse(ds.Tables(0).Rows(0)("vendor")) & "','" & Common.Parse(ds.Tables(0).Rows(0)("INDEX")) & "','" & Common.Parse(crstatus1) & "','" & Common.Parse(userid) & "'," & Common.ConvertDate(Date.Today) & ",'" & Common.Parse(ds.Tables(0).Rows(0)("REMARK")) & "') "

        '    Common.Insert2Ary(strArray, strsql)

        '    '-- str(2)' update po status 
        '    strsql = "update PO_MSTR SET POM_FULFILMENT='" & Common.parseNull(ds.Tables(1).Rows(0)("status")) & "' "

        '    If Common.parseNull(ds.Tables(1).Rows(0)("Cancelled")) <> "" Then
        '        strsql = strsql & " , POM_PO_STATUS= '" & Common.Parse(ds.Tables(1).Rows(0)("Cancelled")) & "'"
        '    End If

        '    'Michelle (6/2/2010) - POM_BUYER_ID might not be the login user
        '    strsql = strsql & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '    Common.Insert2Ary(strArray, strsql)


        '    '-- str (3) ' insert CR detail 
        '    Dim i As Integer
        '    For i = 0 To ds.Tables(2).Rows.Count - 1

        '        Dim line1 As Integer = ds.Tables(2).Rows(i)("lineno")
        '        If check_qty(ds.Tables(2).Rows(i)("qty_cancel"), ds.Tables(1).Rows(0)("po_no"), line1) Then
        '            ' check line QTY if QTY < 0 
        '            HttpContext.Current.Response.Redirect("errorpage.aspx?line=line1&PO_NO=" & Common.Parse(ds.Tables(1).Rows(0)("po_no")))
        '        End If

        '        strsql = "Insert into PO_CR_DETAILS (PCD_CR_NO,PCD_COY_ID,PCD_PO_LINE,PCD_CANCELLED_QTY,PCD_REMARKS) values" & _
        '                " ('" & Common.Parse(CR_num) & "','" & Common.Parse(com_id) & "','" & Common.Parse(ds.Tables(2).Rows(i)("lineno")) & "','" & Common.Parse(ds.Tables(2).Rows(i)("qty_cancel")) & "','" & Common.Parse(ds.Tables(2).Rows(i)("remarks")) & "') "
        '        Common.Insert2Ary(strArray, strsql)
        '        strsql = "  update PO_DETAILS SET POD_CANCELLED_QTY= POD_CANCELLED_QTY+" & Common.Parse(ds.Tables(2).Rows(i)("qty_cancel")) & " WHERE POD_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' AND POD_PO_LINE = '" & Common.Parse(ds.Tables(2).Rows(i)("lineno")) & "' And POD_COY_ID = '" & Common.Parse(com_id) & "'"
        '        Common.Insert2Ary(strArray, strsql)
        '    Next

        '    If objDb.BatchExecute(strArray) Then
        '        '//Send Mail
        '        Dim objMail As New Email
        '        Dim objMail1 As New Email
        '        Dim objMail2 As New Email

        '        If CancelBfToVendor = "" Then 'ie cancel after sending to Vendor
        '            objMail.sendNotification(EmailType.POCancellationRequest, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "")
        '            objMail = Nothing
        '            'Michelle (eBiz/134/08) - Send email to all AOs upon PO cancellation
        '            objMail1.sendNotification(EmailType.POCancellationRequestToAOBuyer, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "AO")
        '            objMail1 = Nothing
        '        Else
        '            objMail1.sendNotification(EmailType.POCancellationRequestToAO, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "AO")
        '            objMail1 = Nothing

        '        End If
        '        'Michelle (6/2/2010) - Check whether need to send email to Buyer (ie. PO cancel by BA)
        '        Dim objUser As New Users
        '        Dim IsBA As String = objUser.BAdminRole(userid, com_id)

        '        If HttpContext.Current.Session("Env") <> "FTN" Then
        '            IsBA = objUser.BAdminRole(userid, com_id)
        '        Else
        '            IsBA = False
        '        End If
        '        If IsBA Then
        '            objMail2.sendNotification(EmailType.POCancellationRequestToAOBuyer, userid, com_id, ds.Tables(0).Rows(0)("vendor"), ds.Tables(1).Rows(0)("po_no"), "", "", "BUYER")
        '            objMail2 = Nothing
        '        End If

        '        Dim objBCM As New BudgetControl
        '        If Common.parseNull(ds.Tables(1).Rows(0)("Cancelled")) = "" Then
        '            strsql = "Select COUNT('*') from PO_Details where POD_ORDERED_QTY > POD_CANCELLED_QTY AND POD_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' And POD_COY_ID = '" & Common.Parse(com_id) & "'"
        '            If objDb.GetVal(strsql) = 0 Then 'ALL ITEM US CANCELLE
        '                strsql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & Common.Parse(POStatus_new.Cancelled) & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '                Common.Insert2Ary(strArray1, strsql)
        '            End If

        '            strsql = "SELECT '*' FROM TotalQtyInvPaidByPO A,TotalQtyToBeDeliveredByPO B "
        '            strsql &= " WHERE A.IM_PO_INDEX = B.POM_PO_INDEX And A.Qty_Inv = B.QtyToBeDelivered And B.POM_PO_NO='" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "'"
        '            strsql &= " AND POM_B_COY_ID='" & Common.Parse(com_id) & "'"
        '            If objDb.Exist(strsql) = 1 Then
        '                strsql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & Common.Parse(POStatus_new.Close) & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(1).Rows(0)("po_no")) & "' and POM_B_COY_ID = '" & Common.Parse(com_id) & "'"
        '                Common.Insert2Ary(strArray1, strsql)
        '            End If
        '            If HttpContext.Current.Session("Env") <> "FTN" Then
        '                objBCM.BCMCalc("CR", CR_num, EnumBCMAction.CancelPO_AF, strArray1)
        '            End If
        '        Else
        '            If HttpContext.Current.Session("Env") <> "FTN" Then
        '                objBCM.BCMCalc("CR", CR_num, EnumBCMAction.CancelPO_BF, strArray1)
        '            End If
        '        End If
        '        If strArray1(0) <> "" Then
        '            objDb.BatchExecute(strArray1)
        '        End If
        '        objMail = Nothing
        '        objBCM = Nothing
        '        Return True
        '    End If
        'End Function
        'Function check_qty(ByVal cancelitem As Integer, ByVal po_no As String, ByVal line As Integer) As Boolean

        '    Dim strsql As String = "select count(*)AS counter from PO_DETAILS WHERE (POD_ORDERED_QTY - POD_CANCELLED_QTY - (POD_RECEIVED_QTY-POD_REJECTED_QTY))>=" & Common.Parse(cancelitem) & " " & _
        '                            " and POD_PO_LINE = '" & Common.Parse(line) & "' and POD_PO_NO = '" & Common.Parse(po_no) & "' AND POD_COY_ID= '" & Common.Parse(com_id) & "' "

        '    Dim tDS As DataSet = objDb.FillDs(strsql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        If tDS.Tables(0).Rows(0).Item("counter").ToString.Trim > 0 Then
        '            Return 0 ' cancel QTY < Outstading QTY  
        '        Else
        '            Return 1 ' Camcel QTY > Outstanding QTY 
        '        End If
        '    End If


        'End Function

        Function check_CanDetail(ByVal companyID As String, ByVal check As String, ByVal line As String) As String
            Dim strsql As String = "select count(*) as counter from PO_CR_DETAILS where PCD_CR_NO= '" & Common.Parse(check) & "' and PCD_COY_ID= '" & Common.Parse(companyID) & "' and PCD_PO_LINE= '" & Common.Parse(line) & "' "

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                If tDS.Tables(0).Rows(0).Item("counter").ToString.Trim > 0 Then
                    Return 1
                Else
                    Return 0
                End If
            End If

        End Function

        Public Function update_crdetail(ByVal companyID As String, ByVal CR_NO As String, ByVal lineno As String, ByVal qtycancel As String, Optional ByVal check As String = "0") As String
            Dim qty As Integer
            Dim strsql As String
            If check = "0" Then
                strsql = "insert into PO_CR_DETAILS (PCD_CR_NO,PCD_COY_ID,PCD_PO_LINE,PCD_CANCELLED_QTY) values" &
                " ('" & Common.Parse(CR_NO) & "','" & Common.Parse(companyID) & "','" & Common.Parse(lineno) & "','" & Common.Parse(qtycancel) & "')"
            ElseIf check = "1" Then
                qty = get_cancelQty(companyID, CR_NO)
                strsql = "update PO_CR_DETAILS set PCD_CANCELLED_QTY='" & qty + qtycancel & "' where PCD_CR_NO= '" & Common.Parse(CR_NO) & "' and PCD_PO_LINE= '" & Common.Parse(lineno) & "' and PCD_COY_ID = '" & Common.Parse(companyID) & "'"
            End If
            objDb.Execute(strsql)
            'update_crdetail = strsql

        End Function
        Public Function get_cancelQty(ByVal companyID As String, ByVal CR_NO As String) As Integer
            Dim strsql As String = "select PCD_CANCELLED_QTY from PO_CR_DETAILS where PCD_CR_NO= '" & Common.Parse(CR_NO) & "' and PCD_COY_ID= '" & Common.Parse(companyID) & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                get_cancelQty = tDS.Tables(0).Rows(0).Item("PCD_CANCELLED_QTY").ToString.Trim
            End If

        End Function

        Public Function get_cancelReq(ByVal CR_NO As String, ByVal strBCoy As String) As String
            Dim strsql As String = "select PCM_REQ_BY from PO_CR_MSTR WHERE PCM_CR_NO= '" & Common.Parse(CR_NO) & "' and PCM_B_COY_ID ='" & Common.Parse(strBCoy) & "'"
            get_cancelReq = objDb.GetVal(strsql)
        End Function

        Public Function check_crexist(ByVal companyID As String, ByVal po_index As String, Optional ByVal line As String = "") As String
            Dim strsql As String = "select PCM_CR_NO from PO_CR_MSTR WHERE PCM_PO_INDEX = '" & Common.Parse(po_index) & "' and PCM_B_COY_ID= '" & Common.Parse(companyID) & "'"

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                check_crexist = tDS.Tables(0).Rows(0).Item("PCM_CR_NO").ToString.Trim
            End If

        End Function
        Public Function check_fulfilment(ByVal po_no As String, ByVal companyID As String) As Boolean

            Dim strsql As String = "select '*' from PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(po_no) & "' and POM_B_COY_ID= '" & Common.Parse(companyID) & "' and POM_FULFILMENT<>''"
            If objDb.Exist(strsql) Then
                check_fulfilment = 1
            Else
                check_fulfilment = 0
            End If
        End Function
        Public Function get_delivery_add(ByVal strBCoyId As String, ByVal PO_No As String) As String

            Dim strsql As String = "select distinct POD_D_ADDR_LINE1,POD_D_ADDR_LINE2,POD_D_ADDR_LINE3," &
            " POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY from PO_DETAILS WHERE POD_PO_NO= '" & Common.Parse(PO_No) & "' " &
            "AND POD_COY_ID= '" & Common.Parse(strBCoyId) & "'"
            Dim strTempAddr As String
            Dim i As Integer
            Dim tDS As DataSet = objDb.FillDs(strsql)
            Dim objGlobal As New AppGlobals
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1

                strTempAddr = Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE1").ToString.Trim)

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE2")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE2").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE3")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE3").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_POSTCODE")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(j).Item("POD_D_POSTCODE").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_CITY")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(j).Item("POD_D_CITY").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_STATE")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(j).Item("POD_D_STATE"))
                End If
                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_COUNTRY")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(j).Item("POD_D_COUNTRY"))
                End If

                i = i + 1
            Next
            If i > 1 Then
                get_delivery_add = "See Detail"
            Else
                get_delivery_add = strTempAddr
            End If

        End Function
        Public Function get_delivery_add_multiline(ByVal strBCoyId As String, ByVal PO_No As String, Optional ByVal blnEnterpriseVersion As Boolean = True) As String

            Dim strsql As String = "select distinct POD_D_ADDR_CODE, POD_D_ADDR_LINE1,POD_D_ADDR_LINE2,POD_D_ADDR_LINE3," &
            " POD_D_POSTCODE,POD_D_CITY,POD_D_STATE,POD_D_COUNTRY from PO_DETAILS WHERE POD_PO_NO= '" & Common.Parse(PO_No) & "' " &
            "AND POD_COY_ID= '" & Common.Parse(strBCoyId) & "'"
            Dim strTempAddr As String
            Dim i As Integer
            Dim tDS As DataSet = objDb.FillDs(strsql)
            Dim objGlobal As New AppGlobals
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1

                'If HttpContext.Current.Session("env") = "FTN" Then
                If blnEnterpriseVersion = False Then
                    strTempAddr = Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE1").ToString.Trim)
                Else
                    strTempAddr = Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_ADDR_CODE").ToString.Trim & "<br />")
                    strTempAddr = strTempAddr & Common.parseNull("&nbsp;" & tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE1").ToString.Trim)
                End If
                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE2")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE2").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE3")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(j).Item("POD_D_ADDR_LINE3").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_POSTCODE")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(j).Item("POD_D_POSTCODE").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_CITY")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(j).Item("POD_D_CITY").ToString.Trim
                End If

                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_STATE")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(j).Item("POD_D_STATE"))
                End If
                If Common.parseNull(tDS.Tables(0).Rows(j).Item("POD_D_COUNTRY")).ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(j).Item("POD_D_COUNTRY"))
                End If

                i = i + 1
            Next
            If i > 1 Then
                get_delivery_add_multiline = "See Detail"
            Else
                get_delivery_add_multiline = strTempAddr
            End If

        End Function
        Public Function getlineitem(ByVal userID As String, ByVal strSCoyID As String, ByVal strPONo As String, ByVal side As String, ByVal blnPreview As Boolean, ByVal strBCoyId As String, Optional ByVal blnShowAddr As Boolean = True) As String
            Dim strsql As String
            Dim DS As DataSet
            '//Remark by Moo, to cater for pr consolidation
            '//add  & "' AND POM_B_COY_ID='" & strBCoyId & "'"
            '//PO is a Buyer Company Document
            If Not blnPreview Then
                'strsql = "select POM_PO_INDEX,POM_CURRENCY_CODE,POD_PR_INDEX,POD_PR_LINE, POD_PO_NO,POD_MIN_ORDER_QTY,POD_UNIT_COST,POD_GST,POD_ETD,POD_WARRANTY_TERMS,POD_PO_LINE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_CODE," & _
                '        " POD_UOM,POD_ETD,POD_MIN_PACK_QTY,POD_PRODUCT_DESC,POD_ORDERED_QTY,POD_RECEIVED_QTY," & _
                '        "POD_DELIVERED_QTY,POD_CANCELLED_QTY,POD_REJECTED_QTY ,POD_PR_LINE,POD_REMARK FROM PO_DETAILS,PO_MSTR WHERE " & _
                '        "POD_PO_NO = POM_PO_NO AND POM_B_COY_ID=POD_COY_ID AND POM_PO_NO='" & PO_No & "' AND POM_B_COY_ID='" & strBCoyId & "'"
                'strsql = "select POM_PO_INDEX,POM_ACCEPTED_DATE,POM_CURRENCY_CODE,POM_PO_DATE,POM_S_COY_NAME,PO_DETAILS.*,CONCAT(PO_DETAILS.POD_ASSET_GROUP, CONCAT(' ',PO_DETAILS.POD_ASSET_NO)) AS ASSET_CODE FROM PO_DETAILS,PO_MSTR WHERE " & _
                '        "POD_PO_NO = POM_PO_NO AND POM_B_COY_ID=POD_COY_ID AND POM_PO_NO= '" & Common.Parse(PO_No) & "' AND POM_B_COY_ID= '" & Common.Parse(strBCoyId) & "'"
                'strsql = "SELECT POM_PO_INDEX,POM_ACCEPTED_DATE,POM_CURRENCY_CODE,POM_PO_DATE,POM_S_COY_NAME,PO_DETAILS.*,CONCAT(PO_DETAILS.POD_ASSET_GROUP, CONCAT(' ',PO_DETAILS.POD_ASSET_NO)) AS ASSET_CODE, " & _
                '        "CASE WHEN CBG_B_GL_CODE IS NULL THEN '' ELSE CONCAT('(', POD_B_GL_CODE, ')', CBG_B_GL_DESC) END AS GL_CODE " & _
                '        "FROM PO_MSTR " & _
                '        "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " & _
                '        "LEFT JOIN COMPANY_B_GL_CODE ON CBG_B_GL_CODE = POD_B_GL_CODE " & _
                '        "AND CBG_B_COY_ID = POD_COY_ID " & _
                '        "WHERE POM_PO_NO= '" & Common.Parse(PO_No) & "' AND POM_B_COY_ID = '" & Common.Parse(strBCoyId) & "' "

                'Jules 2018.11.05 - Swapped GL Code and Analysis Code display: "GL Description (GL Code)", "Analysis Code Description : Analysis Code"
                'Jules 2018.05.07 - PAMB Scrum 2 & 3 - Added Gift and Analysis Codes.
                'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
                strsql = "SELECT POM_PO_INDEX,POM_ACCEPTED_DATE,POM_CURRENCY_CODE,POM_PO_DATE,POM_S_COY_NAME,PO_DETAILS.*,CONCAT(PO_DETAILS.POD_ASSET_GROUP, CONCAT(' ',PO_DETAILS.POD_ASSET_NO)) AS ASSET_CODE, " &
                        "CASE WHEN CBG_B_GL_CODE IS NULL THEN '' ELSE CONCAT(CBG_B_GL_DESC,' (', POD_B_GL_CODE, ')') END AS gl_codedesc, " &
                        "CASE WHEN POD_GST_RATE = 'N/A' THEN POD_GST_RATE ELSE " &
                        "IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,''), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE, " &
                        "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PO_MSTR.POM_B_STATE AND CODE_VALUE=PO_MSTR.POM_B_COUNTRY) AS STATE, " &
                        "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PO_MSTR.POM_B_COUNTRY) AS CT, " &
                        "(SELECT DISTINCT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_FUND_TYPE) AS FUNDTYPE, " &
                        "CASE WHEN POD_PERSON_CODE = 'N/A' THEN POD_PERSON_CODE ELSE (SELECT DISTINCT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PERSON_CODE) END personcodedesc, " &
                        "(SELECT DISTINCT CONCAT(AC_ANALYSIS_CODE_DESC,' : ',AC_ANALYSIS_CODE) FROM ANALYSIS_CODE WHERE AC_ANALYSIS_CODE = POD_PROJECT_CODE) AS projectcodedesc " &
                        "FROM PO_MSTR " &
                        "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POM_B_COY_ID = POD_COY_ID " &
                        "LEFT JOIN COMPANY_B_GL_CODE ON CBG_B_GL_CODE = POD_B_GL_CODE AND CBG_B_COY_ID = POD_COY_ID " &
                        "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_B_COY_ID " &
                        "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = POD_GST_RATE " &
                        "LEFT JOIN TAX ON TAX_CODE = POD_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                        "WHERE POM_PO_NO= '" & Common.Parse(strPONo) & "' AND POM_B_COY_ID = '" & Common.Parse(strBCoyId) & "' "
                ''---------------------Adding code For Line no on 13/07/2007      
                'Dim dst As DataTable
                'dst = objDb.FillDt(strsql)
                'If dst.Rows.Count > 0 Then
                '    Dim str As String
                '    str = dst.Rows(0).Item("POM_S_COY_NAME")
                '    Dim POLine As Integer
                '    POLine = 0
                '    Dim i As Integer
                '    For i = 0 To dst.Rows.Count - 1
                '        If str = dst.Rows(0).Item("POM_S_COY_NAME") And dst.Rows(0).Item("POD_PR_LINE") > 1 Then
                '            POLine = 1
                '        Else
                '            POLine = POLine + 1
                '        End If
                '    Next
                'End If
                ''----end
            Else
                strsql = "select POM_PO_INDEX,POM_CURRENCY_CODE,POM_ACCEPTED_DATE,POM_PO_DATE,PO_DETAILS.*,CONCAT(PO_DETAILS.POD_ASSET_GROUP, CONCAT(' ',PO_DETAILS.POD_ASSET_NO)) AS ASSET_CODE FROM PO_DETAILS,PO_MSTR WHERE " &
                        "POD_PO_NO = POM_PO_NO AND POM_B_COY_ID=POD_COY_ID AND POM_PO_NO= '" & Common.Parse(strPONo) & "' AND POM_B_COY_ID= '" & Common.Parse(strBCoyId) & "'"
                'strsql = "select POM_PO_INDEX,POM_CURRENCY_CODE,POD_PR_INDEX,POD_PR_LINE, POD_PO_NO,POD_MIN_ORDER_QTY,POD_UNIT_COST,POD_GST,POD_ETD,POD_WARRANTY_TERMS,POD_PO_LINE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_CODE," & _
                '       " POD_UOM,POD_ETD,POD_MIN_PACK_QTY,POD_PRODUCT_DESC,POD_PR_LINE,POD_ORDERED_QTY,POD_RECEIVED_QTY," & _
                '       "POD_DELIVERED_QTY,POD_CANCELLED_QTY,POD_REJECTED_QTY ,POD_REMARK FROM PO_DETAILS_PREVIEW,PO_MSTR_PREVIEW WHERE " & _
                '       "POD_PO_NO = POM_PO_NO AND POM_B_COY_ID=POD_COY_ID AND POM_PO_NO='" & PO_No & "' AND POM_B_COY_ID='" & strBCoyId & "'"
            End If

            If side = "b" Then
                '   strsql = strsql & " AND POM_B_COY_ID ='" & com_id & "' ORDER BY POD_PO_LINE"
            ElseIf side = "v" Then
                strsql = strsql & " AND POM_S_COY_ID= '" & Common.Parse(strSCoyID) & "' "
            End If

            If blnShowAddr Then
                '//used for Preview PO, display address at datagrid
                strsql = strsql & " ORDER BY POD_D_ADDR_CODE,POD_PO_LINE"
            Else
                strsql = strsql & " ORDER BY POD_PO_LINE"
            End If

            'DS = objDb.FillDs(strsql)
            Return strsql

        End Function

        'Public Sub getVendorViaProductCode(ByVal sProductCode As String, ByRef sP As String, ByRef s1 As String, ByRef s2 As String, ByRef s3 As String)
        '    Dim tSQL As String, tDS As DataSet
        '    tSQL = "SELECT PM_PREFER_S_COY_ID, PM_1ST_S_COY_ID, PM_2ND_S_COY_ID, PM_3RD_S_COY_ID FROM product_mstr WHERE PM_PRODUCT_CODE = '" & sProductCode & "'"
        '    tDS = objDb.FillDs(tSQL)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        sP = tDS.Tables(0).Rows(0).Item("PM_PREFER_S_COY_ID")
        '        s1 = tDS.Tables(0).Rows(0).Item("PM_1ST_S_COY_ID")
        '        s2 = tDS.Tables(0).Rows(0).Item("PM_2ND_S_COY_ID")
        '        s3 = tDS.Tables(0).Rows(0).Item("PM_3RD_S_COY_ID")
        '    End If
        '    tDS = Nothing
        'End Sub

        'Public Sub getVendorViaPO(ByVal sPONo As String, ByRef sCoyId As String)
        '    Dim tSQL As String, tDS As DataSet
        '    tSQL = "SELECT POM_S_COY_ID FROM po_mstr WHERE POM_PO_NO = '" & sPONo & "'"
        '    tDS = objDb.FillDs(tSQL)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        sCoyId = tDS.Tables(0).Rows(0).Item("POM_S_COY_ID")
        '    End If
        '    tDS = Nothing
        'End Sub

        'Public Function getVendorList(ByVal aryVendor As ArrayList) As DataView 'Michelle (14/11/2010) 
        '    Dim drw As DataView
        '    'Dim aryVendorList As New ArrayList
        '    Dim strVendList As String = ""
        '    Dim i As Integer
        '    Dim objDB As New EAD.DBCom

        '    For i = 0 To aryVendor.Count - 1
        '        If strVendList = "" Then
        '            strVendList = "'" & aryVendor(i) & "'"
        '        Else
        '            strVendList &= ", '" & aryVendor(i) & "'"
        '        End If
        '    Next
        '    Dim strSQL As String = "SELECT CM_COY_ID, CM_COY_NAME " & _
        '             "FROM COMPANY_MSTR " & _
        '             "WHERE CM_COY_ID IN (" & strVendList & ")"
        '    drw = objDB.GetView(strSQL)
        '    Return drw
        'End Function
        'Public Function insertPO(ByVal dsPO As DataSet, ByRef strPONo As String) As Integer

        '    Dim strPrefix, strName, strPhone, strFax As String
        '    Dim strsql, strTermFile As String
        '    Dim strAryQuery(0) As String
        '    Dim strAryAdd(8) As String
        '    Dim i, j As Integer
        '    Dim blnAdd As Boolean = False
        '    Dim strDeptIndex As String = ""
        '    Dim objGlobal As New AppGlobals

        '    'GetLatestDocNo
        '    objGlobal.GetLatestDocNo("PO", strAryQuery, strPONo, strPrefix)

        '    strsql = "SELECT * FROM PO_MSTR WHERE POM_PO_NO = '" & Common.Parse(strPONo) & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

        '    If objDb.Exist(strsql) > 0 Then
        '        insertPO = WheelMsgNum.Duplicate
        '        Exit Function
        '    End If

        '    ' to check whether vendor company is inactive
        '    strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_STATUS <> 'A'  "
        '    strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
        '    If objDb.Exist(strsql) > 0 Then
        '        insertPO = -1
        '        Exit Function
        '    End If

        '    ' to check whether vendor company is being deleted
        '    strsql = "SELECT * FROM COMPANY_MSTR WHERE CM_DELETED = 'Y' "
        '    strsql &= "AND CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
        '    If objDb.Exist(strsql) > 0 Then
        '        insertPO = -2
        '        Exit Function
        '    End If

        '    ' get dept index
        '    'Michelle (26/4/2011) - To cater for those without department
        '    strsql = "SELECT CDM_DEPT_INDEX, UM_USER_NAME, UM_FAX_NO, UM_TEL_NO FROM USER_MSTR "
        '    'strsql &= "INNER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
        '    strsql &= "LEFT OUTER JOIN COMPANY_DEPT_MSTR ON CDM_DEPT_CODE = UM_DEPT_ID "
        '    strsql &= "AND UM_COY_ID = CDM_COY_ID "
        '    strsql &= "WHERE UM_USER_ID = '" & HttpContext.Current.Session("UserId") & "' "
        '    strsql &= "AND UM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    Dim tDS As DataSet = objDb.FillDs(strsql)
        '    For q As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '        strDeptIndex = Common.parseNull(tDS.Tables(0).Rows(q).Item("CDM_DEPT_INDEX"))
        '        strName = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_USER_NAME"))
        '        strPhone = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_TEL_NO"))
        '        strFax = Common.parseNull(tDS.Tables(0).Rows(q).Item("UM_FAX_NO"))
        '    Next

        '    'Michelle (27/12/2010) - To store the Term & Condition
        '    Dim objWheelFile As New FileManagement
        '    strTermFile = objWheelFile.copyTermCondToPO(strPONo)

        '    ' PO_MSTR table
        '    strsql = "INSERT INTO PO_MSTR (POM_PO_NO, POM_B_COY_ID, POM_BUYER_ID, POM_BUYER_NAME, POM_BUYER_PHONE, POM_BUYER_FAX, "
        '    strsql &= "POM_CREATED_DATE, POM_CREATED_BY, POM_STATUS_CHANGED_BY, POM_STATUS_CHANGED_ON, "
        '    strsql &= "POM_S_COY_ID, POM_S_ATTN, POM_S_COY_NAME, POM_S_ADDR_LINE1, "
        '    strsql &= "POM_S_ADDR_LINE2, POM_S_ADDR_LINE3, POM_S_POSTCODE, POM_S_CITY, POM_S_STATE, "
        '    strsql &= "POM_S_COUNTRY, POM_S_PHONE, POM_S_FAX, POM_S_EMAIL, "
        '    strsql &= "POM_PAYMENT_METHOD, POM_SHIPMENT_TERM, POM_SHIPMENT_MODE, POM_CURRENCY_CODE, "
        '    strsql &= "POM_EXCHANGE_RATE, POM_PAYMENT_TERM, POM_SHIP_VIA, POM_BILLING_METHOD, POM_INTERNAL_REMARK, "
        '    strsql &= "POM_EXTERNAL_REMARK, POM_PO_STATUS, POM_FULFILMENT, POM_PO_INDEX, POM_ARCHIVE_IND, "
        '    strsql &= "POM_PRINT_CUSTOM_FIELDS, POM_PRINT_REMARK, POM_SHIP_AMT, POM_PO_PREFIX, POM_B_ADDR_CODE, "
        '    strsql &= "POM_B_ADDR_LINE1, POM_B_ADDR_LINE2, POM_B_ADDR_LINE3, POM_B_POSTCODE, "
        '    strsql &= "POM_B_STATE, POM_B_CITY, POM_B_COUNTRY, "
        '    strsql &= "POM_DUP_FROM, POM_EXTERNAL_IND, POM_REFERENCE_NO, "
        '    strsql &= "POM_PO_COST, POM_RFQ_INDEX, POM_DEPT_INDEX, POM_QUOTATION_NO, POM_TERMANDCOND) SELECT "
        '    strsql &= "'" & Common.Parse(strPONo) & "', "
        '    strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', "
        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        '    strsql &= "'" & Common.Parse(strName) & "', "
        '    strsql &= "'" & Common.Parse(strPhone) & "', "
        '    strsql &= "'" & Common.Parse(strFax) & "', " 'BUYER_FAX
        '    strsql &= Common.ConvertDate(Now) & ", "  'CREATED_DATE
        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', "
        '    strsql &= "'" & HttpContext.Current.Session("UserId") & "', " 'STATUS_CHANGED_BY
        '    strsql &= Common.ConvertDate(Now) & ", " 'STATUS_CHANGED_ON
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "', " 'S_COY_ID
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("Attn")) & "', " 'S_ATTN
        '    strsql &= "CM_COY_NAME, CM_ADDR_LINE1, CM_ADDR_LINE2, CM_ADDR_LINE3, CM_POSTCODE, "
        '    strsql &= "CM_CITY, CM_STATE, CM_COUNTRY, CM_PHONE, CM_FAX, CM_EMAIL, "
        '    'strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("FreightTerms")) & "', " ' FREIGHT_TERMS
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentType")) & "', " ' PAYMENT_TYPE
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentTerm")) & "', " ' SHIPMENT_TERM
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentMode")) & "', " ' SHIPMENT_MODE
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("CurrencyCode")) & "', " ' CURRENCY_CODE
        '    strsql &= Common.Parse(dsPO.Tables(0).Rows(0)("ExchangeRate")) & ", "  ' EXCHANGE_RATE
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentTerm")) & "', " ' PAYMENT_TERM
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipVia")) & "', " ' SHIP_VIA
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillingMethod")) & "', " ' Billing_Method
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("InternalRemark")) & "', " ' INTERNAL_REMARK
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ExternalRemark")) & "', " 'EXTERNAL_REMARK
        '    strsql &= "'" & POStatus_new.Draft & "', " ' PO_STATUS
        '    strsql &= "'" & Fulfilment.null & "', "
        '    strsql &= "NULL, '', "   ' PO_INDEX, ARCHIVE_IND
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintCustom")) & "', " 'PRINT_CUSTOM_FIELDS
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintRemark")) & "', " 'PRINT_REMARK
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipAmt")) & "', " 'POM_SHIP_AMT
        '    strsql &= "'" & Common.Parse(strPrefix) & "', "
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCode")) & "', " ' B_ADDR_CODE
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine1")) & "', " ' B_ADDR_LINE1
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine2")) & "', " ' B_ADDR_LINE2
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine3")) & "', " ' B_ADDR_LINE3
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrPostCode")) & "', " ' B_POSTCODE
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrState")) & "', " ' B_STATE
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCity")) & "', " ' B_CITY
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCountry")) & "', " ' B_COUNTRY
        '    strsql &= "'', '', '', " ' DUP_FROM, EXTERNAL_IND, REFERENCE_NO
        '    strsql &= dsPO.Tables(0).Rows(0)("POCost") & ", " ' PO_COST
        '    strsql &= dsPO.Tables(0).Rows(0)("RfqIndex") & ", " ' RFQ_INDEX

        '    If strDeptIndex = "" Then ' PRM_DEPT_INDEX
        '        strsql &= "NULL, "
        '    Else
        '        strsql &= strDeptIndex & ", "
        '    End If
        '    strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("QuoNo")) & "' " ' QUOTATION_NO
        '    strsql &= ", '" & strTermFile & "' " 'Term & Condition
        '    strsql &= "FROM COMPANY_MSTR "
        '    strsql &= "WHERE CM_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "' "
        '    strsql &= " AND CM_DELETED <> 'Y' AND CM_STATUS = 'A' "

        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' PO_DETAILS table
        '    Dim dd As New System.Web.UI.WebControls.DropDownList
        '    Dim dds As DataTable
        '    For i = 0 To dsPO.Tables(1).Rows.Count - 1
        '        strsql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
        '        strsql &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
        '        strsql &= "POD_ETD, POD_REMARK, POD_GST, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
        '        strsql &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
        '        strsql &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE "
        '        If HttpContext.Current.Session("Env") <> "FTN" Then
        '            strsql &= ", POD_B_ITEM_CODE, POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_CD_GROUP_INDEX, POD_ACCT_INDEX, POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY) SELECT "
        '        Else
        '            strsql &= ", POD_MIN_ORDER_QTY, POD_MIN_PACK_QTY) SELECT "
        '        End If
        '        strsql &= "'" & Common.Parse(strPONo) & "', " ' PR_No
        '        strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' COY_ID
        '        strsql &= Common.Parse(dsPO.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("UOM")) & "', " ' UOM
        '        strsql &= dsPO.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
        '        strsql &= dsPO.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
        '        strsql &= dsPO.Tables(1).Rows(i)("ETD") & ", " ' ETD
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Remark")) & "', " ' REMARK
        '        strsql &= dsPO.Tables(1).Rows(i)("GST") & ", " ' GST
        '        strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
        '        strsql &= "AM_STATE, AM_COUNTRY, "
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Source")) & "', " ' SOURCE
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("TaxCode")) & "' "
        '        strsql &= IIf(dsPO.Tables(1).Rows(i)("MOQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MOQ")) & ", "
        '        strsql &= IIf(dsPO.Tables(1).Rows(i)("MPQ") = "&nbsp;", "NULL", dsPO.Tables(1).Rows(i)("MPQ")) & " "

        '        If HttpContext.Current.Session("Env") <> "FTN" Then
        '            strsql &= ", " & dsPO.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
        '            If Common.Parse(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
        '                strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
        '            Else
        '                strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
        '            End If
        '            strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
        '            strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
        '            strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
        '            If Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) <> "" Then
        '                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) & "' " ' ACCT_INDEX
        '            Else
        '                strsql &= "null " ' ACCT_INDEX
        '            End If
        '        End If

        '        strsql &= "FROM ADDRESS_MSTR "
        '        strsql &= "WHERE AM_ADDR_TYPE = 'D' "
        '        strsql &= "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '        strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPO.Tables(1).Rows(i)("DeliveryAddr")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '    Next

        '    ' PR_CUSTOM_FIELD_MSTR table
        '    For i = 0 To dsPO.Tables(2).Rows.Count - 1
        '        strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
        '        'Michelle (16/11/2010) - To cater for MYSQL
        '        strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
        '        'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
        '        strsql &= dsPO.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
        '        strsql &= "'" & Common.Parse(dsPO.Tables(2).Rows(i)("FieldName")) & "', 'PO'" ' FIELD_NAME
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next

        '    ' PR_CUSTOM_FIELD_DETAILS table
        '    For i = 0 To dsPO.Tables(3).Rows.Count - 1
        '        strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
        '        strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
        '        'Michelle (16/11/2010) - To cater for MYSQL
        '        strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
        '        'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
        '        strsql &= dsPO.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
        '        strsql &= dsPO.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
        '        strsql &= "'" & Common.Parse(dsPO.Tables(3).Rows(i)("FieldValue")) & "', 'PO' " ' FIELD_VALUE
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next

        '    If dsPO.Tables(0).Rows(0)("RfqIndex") = "NULL" Then
        '    Else
        '        strsql = "UPDATE RFQ_MSTR SET RM_Status = '2' "
        '        strsql &= "WHERE RM_RFQ_ID = " & dsPO.Tables(0).Rows(0)("RfqIndex")
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    End If

        '    ' update COMPANY_DOC_ATTACHMENT table
        '    strsql = "UPDATE COMPANY_DOC_ATTACHMENT SET "
        '    strsql &= "CDA_DOC_NO = '" & strPONo & "' "
        '    strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "AND CDA_DOC_TYPE = 'PO' "
        '    strsql &= "AND CDA_DOC_NO = '" & HttpContext.Current.Session.SessionID & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    If objDb.BatchExecute(strAryQuery) Then
        '        insertPO = WheelMsgNum.Save
        '    Else
        '        insertPO = WheelMsgNum.NotSave
        '    End If
        'End Function

        'Public Sub updatePO(ByVal dsPO As DataSet)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    Dim i As Integer

        '    strsql = "UPDATE PO_MSTR SET "
        '    strsql &= "POM_S_COY_ID = '" & Common.Parse(dsPO.Tables(0).Rows(0)("VendorID")) & "', "
        '    strsql &= "POM_S_ATTN = '" & Common.Parse(dsPO.Tables(0).Rows(0)("Attn")) & "', "
        '    'strsql &= "POM_FREIGHT_TERMS = '" & Common.Parse(dsPO.Tables(0).Rows(0)("FreightTerms")) & "', "
        '    strsql &= "POM_PAYMENT_METHOD = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentType")) & "', "
        '    strsql &= "POM_SHIPMENT_TERM = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentTerm")) & "', "
        '    strsql &= "POM_SHIPMENT_MODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipmentMode")) & "', "
        '    strsql &= "POM_EXCHANGE_RATE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ExchangeRate")) & "', "
        '    strsql &= "POM_PAYMENT_TERM = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PaymentTerm")) & "', "
        '    strsql &= "POM_SHIP_VIA = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipVia")) & "', "
        '    strsql &= "POM_INTERNAL_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("InternalRemark")) & "', "
        '    strsql &= "POM_EXTERNAL_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ExternalRemark")) & "', "
        '    strsql &= "POM_PRINT_CUSTOM_FIELDS = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintCustom")) & "', "
        '    strsql &= "POM_PRINT_REMARK = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PrintRemark")) & "', "
        '    strsql &= "POM_SHIP_AMT = '" & Common.Parse(dsPO.Tables(0).Rows(0)("ShipAmt")) & "', "
        '    strsql &= "POM_B_ADDR_CODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCode")) & "', "
        '    strsql &= "POM_B_ADDR_LINE1 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine1")) & "', "
        '    strsql &= "POM_B_ADDR_LINE2 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine2")) & "', "
        '    strsql &= "POM_B_ADDR_LINE3 = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrLine3")) & "', "
        '    strsql &= "POM_B_POSTCODE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrPostCode")) & "', "
        '    strsql &= "POM_B_STATE = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrState")) & "', "
        '    strsql &= "POM_B_CITY = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCity")) & "', "
        '    strsql &= "POM_B_COUNTRY = '" & Common.Parse(dsPO.Tables(0).Rows(0)("BillAddrCountry")) & "', "
        '    strsql &= "POM_PO_COST = " & Common.Parse(dsPO.Tables(0).Rows(0)("POCost")) & " "
        '    'strsql &= "POM_GST = " & Common.Parse(dsPO.Tables(0).Rows(0)("GST")) & " "
        '    strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' delete from PR_DETAILS table
        '    strsql = "DELETE FROM PO_DETAILS "
        '    strsql &= "WHERE POD_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
        '    strsql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)


        '    ' PO_DETAILS table
        '    Dim dd As New System.Web.UI.WebControls.DropDownList
        '    Dim dds As DataTable
        '    For i = 0 To dsPO.Tables(1).Rows.Count - 1
        '        strsql = "INSERT INTO PO_DETAILS (POD_PO_NO, POD_COY_ID, POD_PO_LINE, POD_PRODUCT_CODE, "
        '        strsql &= "POD_VENDOR_ITEM_CODE, POD_PRODUCT_DESC, POD_UOM, POD_ORDERED_QTY, POD_UNIT_COST, "
        '        strsql &= "POD_ETD, POD_REMARK, POD_GST, POD_D_ADDR_CODE, POD_D_ADDR_LINE1, "
        '        strsql &= "POD_D_ADDR_LINE2, POD_D_ADDR_LINE3, POD_D_POSTCODE, POD_D_CITY, POD_D_STATE, "
        '        strsql &= "POD_D_COUNTRY, POD_PRODUCT_TYPE, POD_SOURCE "
        '        If HttpContext.Current.Session("Env") <> "FTN" Then
        '            strsql &= ", POD_B_ITEM_CODE, POD_WARRANTY_TERMS, POD_B_CATEGORY_CODE, POD_B_GL_CODE, POD_CD_GROUP_INDEX, POD_ACCT_INDEX) SELECT "
        '        Else
        '            strsql &= ") SELECT "
        '        End If
        '        strsql &= "'" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "', " ' PR_No
        '        strsql &= "'" & HttpContext.Current.Session("CompanyId") & "', " ' COY_ID
        '        strsql &= Common.Parse(dsPO.Tables(1).Rows(i)("Line")) & ", " ' PR_LINE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductCode")) & "', " ' PRODUCT_CODE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("VendorItemCode")) & "', " ' VENDOR_ITEM_CODE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductDesc")) & "', " ' PRODUCT_DESC
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("UOM")) & "', " ' UOM
        '        strsql &= dsPO.Tables(1).Rows(i)("Qty") & ", " ' ORDERED_QTY
        '        strsql &= dsPO.Tables(1).Rows(i)("UnitCost") & ", " ' UNIT_COST
        '        strsql &= dsPO.Tables(1).Rows(i)("ETD") & ", " ' ETD
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Remark")) & "', " ' REMARK
        '        strsql &= dsPO.Tables(1).Rows(i)("GST") & ", " ' GST
        '        strsql &= "AM_ADDR_CODE, AM_ADDR_LINE1, AM_ADDR_LINE2, AM_ADDR_LINE3, AM_POSTCODE, AM_CITY, "
        '        strsql &= "AM_STATE, AM_COUNTRY, "
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ProductType")) & "', " ' PRODUCT_TYPE
        '        strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("Source")) & "' " ' SOURCE
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("RfqQty")) & "', " ' PRD_RFQ_QTY
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("QtyTolerance")) & "', " ' PRD_QTY_TOLERANCE
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("SupplierCompanyId")) & "', " ' PRD_S_COY_ID
        '        'strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("TaxCode")) & "' "

        '        If HttpContext.Current.Session("Env") <> "FTN" Then
        '            strsql &= ", " & dsPO.Tables(1).Rows(i)("WarrantyTerms") & ", " ' WARRANTY_TERMS
        '            If Common.Parse(dsPO.Tables(1).Rows(i)("CDGroup")) = "" Then
        '                strsql &= "NULL, " ' PRD_CD_GROUP_INDEX
        '            Else
        '                strsql &= dsPO.Tables(1).Rows(i)("CDGroup") & ", " ' PRD_CD_GROUP_INDEX
        '            End If
        '            strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("ItemCode")) & "', " ' PRD_B_ITEM_CODE
        '            strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("CategoryCode")) & "', " ' PRD_B_CATEGORY_CODE
        '            strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("GLCode")) & "', " ' PRD_B_GL_CODE
        '            If Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) <> "" Then
        '                strsql &= "'" & Common.Parse(dsPO.Tables(1).Rows(i)("AcctIndex")) & "' " ' ACCT_INDEX
        '            Else
        '                strsql &= "null " ' ACCT_INDEX
        '            End If
        '        End If

        '        strsql &= "FROM ADDRESS_MSTR "
        '        strsql &= "WHERE AM_ADDR_TYPE = 'D' "
        '        strsql &= "AND AM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '        strsql &= "AND AM_ADDR_CODE = '" & Common.Parse(dsPO.Tables(1).Rows(i)("DeliveryAddr")) & "' "
        '        Common.Insert2Ary(strAryQuery, strsql)

        '    Next

        '    ' delete from PR_CUSTOM_FIELD_MSTR
        '    strsql = "DELETE FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_PR_INDEX IN "
        '    strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
        '    strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' PR_CUSTOM_FIELD_MSTR table
        '    For i = 0 To dsPO.Tables(2).Rows.Count - 1
        '        strsql = "INSERT INTO PR_CUSTOM_FIELD_MSTR (PCM_PR_INDEX, PCM_FIELD_NO, PCM_FIELD_NAME, PCM_TYPE) SELECT "
        '        'Michelle (16/11/2010) - To cater for MYSQL
        '        strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
        '        'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
        '        strsql &= dsPO.Tables(2).Rows(i)("FieldNo") & ", " ' FIELD_NO
        '        strsql &= "'" & Common.Parse(dsPO.Tables(2).Rows(i)("FieldName")) & "', 'PO'" ' FIELD_NAME
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next

        '    ' delete from PR_CUSTOM_FIELD_DETAILS
        '    strsql = "DELETE FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_PR_INDEX IN "
        '    strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
        '    strsql &= "WHERE POM_PO_NO = '" & Common.Parse(dsPO.Tables(0).Rows(0)("PONo")) & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "') "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    ' PR_CUSTOM_FIELD_DETAILS table
        '    For i = 0 To dsPO.Tables(3).Rows.Count - 1
        '        strsql = "INSERT INTO PR_CUSTOM_FIELD_DETAILS (PCD_PR_INDEX, PCD_PR_LINE, "
        '        strsql &= "PCD_FIELD_NO, PCD_FIELD_VALUE, PCD_TYPE) SELECT "
        '        'Michelle (16/11/2010) - To cater for MYSQL
        '        strsql &= "(SELECT MAX(POM_PO_Index) FROM PO_MSTR), "
        '        'strsql &= objDb.GetLatestInsertedID2("PO_MSTR", "POM_PO_INDEX") & ", "
        '        strsql &= dsPO.Tables(3).Rows(i)("Line") & ", "  ' PR_LINE
        '        strsql &= dsPO.Tables(3).Rows(i)("FieldNo") & ", " ' FIELD_NO
        '        strsql &= "'" & Common.Parse(dsPO.Tables(3).Rows(i)("FieldValue")) & "', 'PO' " ' FIELD_VALUE
        '        Common.Insert2Ary(strAryQuery, strsql)
        '    Next

        '    objDb.BatchExecute(strAryQuery)
        'End Sub

        Public Function get_username(ByVal user_id As String) As String

            Dim strsql As String = "select um_user_name from user_mstr where um_user_id = '" & Common.Parse(user_id) & "'"
            get_username = objDb.GetVal(strsql)
        End Function

        Public Function get_status(ByVal po_status As String) As String
            Dim strsql As String = "select STATUS_DESC from STATUS_MSTR WHERE STATUS_TYPE='PO' AND STATUS_NO= '" & Common.Parse(po_status) & "' "
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                get_status = tDS.Tables(0).Rows(0).Item("STATUS_DESC").ToString.Trim
            End If


        End Function

        Public Function get_poindex(ByVal po_no As String, ByVal side As String, ByVal bcom_id As String) As String

            Dim strsql As String = "select POM_PO_INDEX from PO_MSTR WHERE POM_PO_NO= '" & Common.Parse(po_no) & "'"

            If side = "b" Then
                strsql = strsql & " and POM_B_COY_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
            ElseIf side = "v" Then
                strsql = strsql & " and POM_B_COY_ID= '" & Common.Parse(bcom_id) & "' and POM_S_COY_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "'"
            ElseIf side = "u" Then
                strsql = strsql & " and POM_B_COY_ID= '" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' and POM_BUYER_ID= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "'"

            End If
            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                get_poindex = tDS.Tables(0).Rows(0).Item("POM_PO_INDEX").ToString.Trim
            End If


        End Function
        Public Function get_cancelLineitem(ByVal PO_No As String, ByVal b_com_id As String, ByVal side As String, ByVal cr_no As String, Optional ByVal blnShowAddr As Boolean = True) As DataSet
            Dim strsql As String
            Dim DS As DataSet

            'strsql = "select DISTINCT POM_PO_NO,PCD_PO_LINE,PCM_CR_NO,PCD_CANCELLED_QTY,POD_MIN_ORDER_QTY,POD_UNIT_COST, " & _
            '"POD_GST,POD_ETD,POD_WARRANTY_TERMS,POD_PO_LINE,POD_VENDOR_ITEM_CODE,POD_PRODUCT_CODE," & _
            '"POD_UOM,POD_ETD,POD_MIN_PACK_QTY,POD_PRODUCT_DESC,POD_ORDERED_QTY,  " & _
            '"POD_RECEIVED_QTY, POD_DELIVERED_QTY, POD_CANCELLED_QTY," & _
            '"POD_REJECTED_QTY, POD_REMARK " & _
            '"FROM PO_MSTR " & _
            '"LEFT JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO " & _
            '"LEFT JOIN PO_CR_MSTR ON POM_PO_INDEX = PCM_PO_INDEX " & _
            '"LEFT JOIN PO_CR_DETAILS ON PCD_CR_NO = PCM_CR_NO AND PCD_PO_LINE = POD_PO_LINE " & _
            '"WHERE POM_PO_NO='" & PO_No & "' and POM_B_COY_ID='" & b_com_id & "' and  PCD_PO_LINE<>'' and PCM_CR_NO='" & cr_no & "'"

            strsql = "select DISTINCT POM_PO_NO,PCD_PO_LINE,PCM_CR_NO,PCM_CR_REMARKS,PCD_CANCELLED_QTY,PCD_REMARKS,PO_DETAILS.* " &
            "FROM PO_MSTR " &
            "LEFT JOIN PO_DETAILS ON POM_PO_NO = POD_PO_NO AND POM_B_COY_ID=POD_COY_ID " &
            "LEFT JOIN PO_CR_MSTR ON POM_PO_INDEX = PCM_PO_INDEX " &
            "LEFT JOIN PO_CR_DETAILS ON PCD_CR_NO = PCM_CR_NO AND PCM_B_COY_ID=PCD_COY_ID AND PCD_PO_LINE = POD_PO_LINE " &
            "WHERE POM_PO_NO= '" & Common.Parse(PO_No) & "' and POM_B_COY_ID= '" & Common.Parse(b_com_id) & "' and  PCD_PO_LINE<>'' and PCM_CR_NO= '" & Common.Parse(cr_no) & "'"

            If side = "v" Then
                strsql = strsql & " AND POM_B_COY_ID= '" & Common.Parse(b_com_id) & "'"
            End If

            If blnShowAddr Then
                strsql = strsql & " AND POD_COY_ID= '" & Common.Parse(b_com_id) & "' order by POD_D_ADDR_CODE,POD_PO_LINE"
            Else
                strsql = strsql & " AND POD_COY_ID= '" & Common.Parse(b_com_id) & "' order by POD_PO_LINE"
            End If

            DS = objDb.FillDs(strsql)

            Return DS
        End Function


        'Public Function get_poDetail2(ByVal PO_No As String, ByVal v_comid As String) As DataSet
        '    Dim strsql As String
        '    Dim DS As DataSet

        '    strsql = "select * from PO_MSTR,PO_DETAILS Where POM_PO_NO= '" & Common.Parse(PO_No) & "'" & _
        '                      " AND POM_B_COY_ID= '" & Common.Parse(com_id) & "' AND POM_B_COY_ID=POD_COY_ID " & _
        '                      " AND POD_PO_NO=POM_PO_NO"

        '    strsql = strsql & " order by POD_PO_LINE"
        '    DS = objDb.FillDs(strsql)
        '    get_poDetail2 = DS
        'End Function
        'Michelle (19/11/2010) - To get the approval index for PO 
        'Public Function getPOApprFlow(ByVal isFTN As Boolean) As DataTable
        '    Dim strsql As String
        '    Dim dt As DataTable

        '    If isFTN Then
        '        strsql = "SELECT AGM_GRP_INDEX AS GrpIndex, ISNULL(AGA_AO,'') AS AO, ISNULL(AGA_A_AO,'') AS AAO, CAST(ISNULL(AGA_SEQ, 1) AS CHAR) AS SEQ, ISNULL(AGM_TYPE, '') AS Type, ISNULL(AGA_RELIEF_IND, '') AS Relief  FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO "
        '        strsql &= "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '        strsql &= "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' AND AGM_TYPE = 'PO'"
        '    Else
        '        strsql = "SELECT AGM_GRP_INDEX, AGA_AO, AGA_A_AO FROM APPROVAL_GRP_MSTR, APPROVAL_GRP_BUYER, APPROVAL_GRP_AO "
        '        strsql &= "WHERE AGM_GRP_INDEX = AGB_GRP_INDEX AND AGM_GRP_INDEX = AGA_GRP_INDEX AND AGM_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '        strsql &= "AND AGB_BUYER = '" & HttpContext.Current.Session("UserId") & "' "
        '    End If

        '    dt = objDb.FillDt(strsql)
        '    getPOApprFlow = dt
        'End Function

        'Michelle (23/11/2010) - To get the PO approval list
        'Function getPOListForApproval(ByVal strPoNo As String, ByVal strVendor As String, _
        'ByVal dteDateFr As String, ByVal dteDateTo As String, ByVal strReliefOn As String, _
        'Optional ByVal strAction As String = "new", Optional ByVal strStatus As String = "", Optional ByVal strAOAction As String = "") As DataSet

        '    Dim strSql, strSqlReliefO, strSqlReliefC, strCondition, strCondition1, strSqlAttached As String
        '    Dim strUser As String = HttpContext.Current.Session("UserId")
        '    Dim strCoyId As String = HttpContext.Current.Session("CompanyId")
        '    Dim ds As DataSet

        '    If strAction = "new" Then
        '        '//current approval AO of the PO is login user and PO not rejected by previous AO
        '        'strCondition = " AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status NOT IN(" & POStatus_new.RejectedBy & "," & POStatus_new.CancelledBy & "))"
        '        strCondition = " AND (PA.PRA_Seq - 1 = PA.PRA_AO_Action AND PM.POM_PO_Status = '" & strStatus & "')"
        '        strCondition1 = "(PA.PRA_AO = '" & strUser & "' " _
        '        & "OR (PA.PRA_A_AO = '" & strUser & "' AND PA.PRA_Relief_Ind='O'))"
        '    ElseIf strAction = "app" Then
        '        If strAOAction = "" Then
        '            strCondition = " AND (PA.PRA_Seq <= PA.PRA_AO_Action)"
        '        Else
        '            strCondition = " AND (PA.PRA_Seq <= PA.PRA_AO_Action) AND SUBSTRING(PRA_AO_REMARK,1,8) = '" & strAOAction & "' "
        '        End If
        '        '//ownership already taken by AAO, no need to check for relief ind
        '        strCondition1 = "(((PA.PRA_AO = '" & strUser & "' " _
        '        & "OR PA.PRA_A_AO = '" & strUser & "') AND PRA_RELIEF_IND='O') OR ((PA.PRA_AO = '" & strUser & "' " _
        '        & "OR PA.PRA_ON_BEHALFOF = '" & strUser & "') AND PRA_RELIEF_IND<>'O'))"
        '        '//set to empty string to prevent mistake during parameter passing
        '        strReliefOn = ""
        '    End If

        '    '// 1 PR MANY ATTACHMENT
        '    '//MAYBE USE ATTCHED IND
        '    strSqlAttached = "SELECT CDA_DOC_NO FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyId & "' AND CDA_DOC_TYPE='PR'"

        '    strSqlReliefO = "SELECT DISTINCT PM.POM_PO_Index,PM.POM_PO_No, PM.POM_PO_DATE, PM.POM_STATUS_CHANGED_ON, PM.POM_S_Coy_ID, PM.POM_CREATED_Date,PM.POM_BUYER_ID, PM.POM_RFQ_INDEX, " _
        '   & "PM.POM_CURRENCY_CODE, PM.POM_S_COY_NAME, PM.POM_PO_STATUS,PM.POM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '  & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC," _
        '   & "PM.POM_PO_COST AS PO_AMT, UM1.UM_USER_NAME " _
        '   & "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX " _
        '   & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID " _
        '   & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
        '   & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.POM_STATUS_CHANGED_BY " _
        '   & "AND UM.UM_COY_ID='" & strCoyId & "' WHERE PA.PRA_FOR = 'PO' AND " & strCondition1 & _
        '   " AND PM.POM_B_COY_ID='" & strCoyId & "'" & strCondition

        '    If strPoNo <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.POM_PO_No" & Common.ParseSQL(strPoNo)
        '    End If

        '    If strVendor <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.POM_S_COY_NAME" & Common.ParseSQL(strVendor)
        '    End If

        '    If dteDateFr <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.POM_CREATED_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
        '    End If

        '    'If dteDateFr = "" And dteDateTo <> "" Then
        '    If dteDateTo <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.POM_CREATED_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
        '    End If

        '    If strAction = "app" And strStatus <> "" Then
        '        strSqlReliefO = strSqlReliefO & " AND PM.POM_PO_Status IN(" & strStatus & ")"
        '    End If
        '    '//For Relief Ind=Open


        '    Dim strSqlCheck, strReliefList As String

        '    If UCase(strReliefOn) = "WHEELALL" Then '//in case need to show all PR
        '        Dim dvCheck As DataView
        '        Dim objPR2 As New PurchaseReq2
        '        '//For Relief Ind=Controlled             
        '        dvCheck = objPR2.getReliefList("PO")
        '        Dim intCnt, intLoop As Integer
        '        If Not dvCheck Is Nothing Then
        '            intCnt = dvCheck.Count
        '            For intLoop = 0 To intCnt - 1
        '                If Not IsDBNull(dvCheck.Item(intLoop)("RAM_USER_ID")) Then
        '                    If strReliefList = "" Then
        '                        strReliefList = "'" & dvCheck.Item(intLoop)("RAM_USER_ID") & "'"
        '                    Else
        '                        strReliefList = strReliefList & ",'" & dvCheck(intLoop)("RAM_USER_ID") & "'"
        '                    End If
        '                End If
        '            Next
        '        End If
        '    ElseIf strReliefOn <> "" Then
        '        strReliefList = "'" & strReliefOn & "'"
        '    Else
        '        strReliefList = ""
        '    End If

        '    If strReliefList <> "" Then
        '        strSqlReliefC = "SELECT DISTINCT PM.POM_PO_Index,PM.POM_PO_No,PM.POM_S_Coy_ID, PM.POM_CREATED_Date,PM.POM_BUYER_ID, PM.POM_RFQ_INDEX, " _
        '        & "PM.POM_CURRENCY_CODE, PM.POM_S_COY_NAME, PM.POM_PO_STATUS,PM.POM_STATUS_CHANGED_BY,UM.UM_USER_NAME AS CHANGED_BY_NAME, " _
        '        & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC," _
        '        & "PM.POM_PO_COST AS PR_AMT, UM1.UM_USER_NAME " _
        '        & "FROM PR_Approval PA INNER JOIN PO_MSTR PM ON PA.PRA_PR_INDEX = PM.POM_PO_INDEX " _
        '        & "LEFT OUTER JOIN USER_MSTR UM1 ON UM1.UM_USER_ID = PM.POM_BUYER_ID " _
        '        & "AND UM1.UM_COY_ID='" & strCoyId & "' " _
        '        & "LEFT OUTER JOIN USER_MSTR UM ON UM.UM_USER_ID=PM.POM_STATUS_CHANGED_BY " _
        '        & "AND UM.UM_COY_ID='" & strCoyId & "' " _
        '        & "WHERE PA.PRA_AO IN(" & strReliefList & ") AND PA.PRA_FOR = 'PO' " _
        '        & "AND PA.PRA_A_AO = '" & strUser & "' " _
        '        & "And PA.PRA_Relief_Ind='C' AND PM.POM_B_COY_ID='" & strCoyId & "'" & strCondition


        '        If strPoNo <> "" Then
        '            strSqlReliefC = strSqlReliefC & " AND PM.POM_PO_No" & Common.ParseSQL(strPoNo)
        '        End If

        '        If strVendor <> "" Then
        '            strSqlReliefC = strSqlReliefC & " AND PM.POM_S_COY_NAME" & Common.ParseSQL(strVendor)
        '        End If

        '        If dteDateFr <> "" Then
        '            strSqlReliefC = strSqlReliefC & " AND POM_CREATED_Date >= " & Common.ConvertDate(dteDateFr & " 00:00:00")
        '        End If

        '        'If dteDateFr = "" And dteDateTo <> "" Then
        '        If dteDateTo <> "" Then
        '            strSqlReliefC = strSqlReliefC & " AND POM_CREATED_Date <= " & Common.ConvertDate(dteDateTo & " 23:59:59")
        '        End If


        '        If strAction = "app" And strStatus <> "" Then
        '            strSqlReliefO = strSqlReliefO & " AND PM.POM_PO_Status IN(" & strStatus & ")"
        '        End If
        '    End If
        '    '//For Relief Ind=Controlled


        '    If UCase(strReliefOn) = "WHEELALL" Then '//show all
        '        If strSqlReliefC <> "" Then
        '            strSql = strSqlReliefO & " UNION " & strSqlReliefC
        '        Else
        '            strSql = strSqlReliefO
        '        End If
        '    ElseIf strReliefOn <> "" Then '//show other ao's
        '        strSql = strSqlReliefC
        '    Else '//show own pr
        '        strSql = strSqlReliefO
        '    End If

        '    ds = objDb.FillDs(strSql)
        '    Return ds
        'End Function

        'rendika(10/05/2021)
        Public Function SubmitPO(ByVal userID As String, ByVal dtAO As DataTable,  ByVal companyID As String, ByVal strPONo As String) As String
            Dim dt As New DataTable
            Dim objPO As New PurchaseOrder
            Dim objPO1 As New PurchaseOrder_Buyer
            Dim intIndex, intMsg As Integer

            'If Session("Env") = "FTN" Then
            '    dt = objPO1.getPOApprFlow(True)
            'Else
            '    dt = objPO1.getPOApprFlow(False)
            'End If

            Dim strRFQ_No, strPR_Index, strGrp_Index
            Dim strDept_Code As String = ""


            Dim strPO_No As String = objDb.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRD_CONVERT_TO_DOC),""'"")) AS CHAR(2000)),'') AS PRD_CONVERT_TO_DOC FROM PR_DETAILS WHERE PRD_COY_ID = '" & companyID & "' AND PRD_CONVERT_TO_DOC = '" & strPONo & "'")
            strPR_Index = objDb.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRM_PR_INDEX),""'"")) AS CHAR(2000)),'') AS PRM_PR_INDEX FROM PR_DETAILS, PR_MSTR WHERE PRM_COY_ID = PRD_COY_ID AND PRM_PR_NO = PRD_PR_NO AND PRD_COY_ID = '" & companyID & "' AND (PRD_CONVERT_TO_DOC IN (" & strPO_No & "))")
            strGrp_Index = objDb.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",PRA_APPROVAL_GRP_INDEX),""'"")) AS CHAR(2000)),'') AS PRA_APPROVAL_GRP_INDEX FROM PR_APPROVAL WHERE PRA_FOR = 'PR' AND PRA_PR_INDEX IN (" & strPR_Index & ")")
            strDept_Code = objDb.GetVal("SELECT IFNULL(CAST(GROUP_CONCAT(CONCAT(CONCAT(""'"",AGM_DEPT_CODE),""'"")) AS CHAR(2000)),'') AS AGM_DEPT_CODE FROM APPROVAL_GRP_MSTR WHERE AGM_DEPT_CODE IS NOT NULL AND AGM_DEPT_CODE <> '' AND AGM_GRP_INDEX IN (" & strGrp_Index & ")")
            dt = objPO1.getPOApprFlow(userID, companyID, True, strDept_Code)
            If dt.Rows.Count = 0 Then
                Return "There is no Approval Flow defined for you."
                'ElseIf dt.Rows.Count > 1 Then
                '    Return "Error"
            Else
                intMsg = objPO1.submitPO(userID, companyID, strPONo, dt, "0")
                Select Case intMsg
                    Case WheelMsgNum.Save
                        intMsg = objPO1.updatePRStatus(userID, companyID, strPR_Index, "")
                        Return MsgRecordSave
                    Case WheelMsgNum.NotSave
                        Return MsgRecordNotSave
                    Case WheelMsgNum.Delete
                        Return MsgRecordDelete
                End Select
            End If

        End Function
        Private Function bindPO(ByVal data As ArrayList) As DataSet
            Dim ds As New DataSet
            Dim i As Integer

            ' columns for PR_MSTR table
            Dim dtMaster As New DataTable
            dtMaster.Columns.Add("PONo", Type.GetType("System.String"))
            'dtMaster.Columns.Add("ReqName", Type.GetType("System.String"))
            'dtMaster.Columns.Add("ReqPhone", Type.GetType("System.String"))
            dtMaster.Columns.Add("VendorID", Type.GetType("System.String"))
            dtMaster.Columns.Add("Attn", Type.GetType("System.String"))
            dtMaster.Columns.Add("FreightTerms", Type.GetType("System.String"))
            dtMaster.Columns.Add("PaymentType", Type.GetType("System.String"))
            dtMaster.Columns.Add("ShipmentTerm", Type.GetType("System.String"))
            dtMaster.Columns.Add("ShipmentMode", Type.GetType("System.String"))
            dtMaster.Columns.Add("CurrencyCode", Type.GetType("System.String"))
            dtMaster.Columns.Add("ExchangeRate", Type.GetType("System.Double"))
            dtMaster.Columns.Add("PaymentTerm", Type.GetType("System.String"))
            dtMaster.Columns.Add("ShipVia", Type.GetType("System.String"))
            dtMaster.Columns.Add("InternalRemark", Type.GetType("System.String"))
            dtMaster.Columns.Add("ExternalRemark", Type.GetType("System.String"))
            dtMaster.Columns.Add("Urgent", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrCode", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrLine1", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrLine2", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrLine3", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrPostCode", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrState", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrCity", Type.GetType("System.String"))
            dtMaster.Columns.Add("BillAddrCountry", Type.GetType("System.String"))
            dtMaster.Columns.Add("PrintCustom", Type.GetType("System.String"))
            dtMaster.Columns.Add("PrintRemark", Type.GetType("System.String"))
            dtMaster.Columns.Add("ShipAmt", Type.GetType("System.String"))
            'dtMaster.Columns.Add("GST", Type.GetType("System.String"))
            dtMaster.Columns.Add("POCost", Type.GetType("System.Double"))
            dtMaster.Columns.Add("RfqIndex", Type.GetType("System.String"))
            dtMaster.Columns.Add("QuoNo", Type.GetType("System.String"))
            dtMaster.Columns.Add("POM_EXCHANGE_RATE", Type.GetType("System.Double"))
            dtMaster.Columns.Add("BillingMethod", Type.GetType("System.String"))
            Dim dtr As DataRow
            dtr = dtMaster.NewRow()
            dtr("PONo") = data("pom_po_no")
            'dtr("ReqName") = txtRequestedName.Text
            'dtr("ReqPhone") = txtRequestedContact.Text
            dtr("VendorID") = data("pom_s_coy_id")
            dtr("Attn") = data("pom_s_attn")
            'dtr("FreightTerms") = txtFreightTerm.Text
            dtr("PaymentType") = data("pom_payment_method")
            dtr("ShipmentTerm") = data("pom_shipment_term")
            dtr("ShipmentMode") = data("pom_shipment_mode")
            dtr("CurrencyCode") = data("pom_currency_code")
            dtr("PaymentTerm") = data("pom_payment_term")
            dtr("ShipVia") = data("pom_ship_via")
            dtr("InternalRemark") = data("pom_internal_remark")
            dtr("ExternalRemark") = data("pom_external_remark")
            dtr("BillAddrCode") = data("pom_b_addr_code")
            dtr("BillAddrLine1") = data("pom_b_addr_line1")
            dtr("BillAddrLine2") = data("pom_b_addr_line2")
            dtr("BillAddrLine3") = data("pom_b_addr_line3")
            dtr("BillAddrPostCode") = data("pom_b_postcode")
            dtr("BillAddrState") = data("pom_b_state")
            dtr("BillAddrCity") = data("pom_b_city")
            dtr("BillAddrCountry") = data("pom_b_country")
            dtr("PrintCustom") = data("pom_print_custom_fields")
            dtr("Urgent") = data("pom_urgent")
            dtr("ExchangeRate") = 1
            dtr("BillingMethod") = data("pom_billing_method")
            dtr("PrintRemark") = data("pom_print_remark")
            dtr("ShipAmt") = Replace(data("pom_ship_amt").Text, ",", "")
            dtr("RfqIndex") = "NULL"
            dtr("QuoNo") = ""

            ' columns for PR_DETAILS
            Dim dtDetails As New DataTable
            dtDetails.Columns.Add("Line", Type.GetType("System.Int32"))
            dtDetails.Columns.Add("ProductCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("VendorItemCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("TaxCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("ProductDesc", Type.GetType("System.String"))
            dtDetails.Columns.Add("UOM", Type.GetType("System.String"))
            dtDetails.Columns.Add("Qty", Type.GetType("System.Double"))
            dtDetails.Columns.Add("UnitCost", Type.GetType("System.Double"))
            dtDetails.Columns.Add("ETD", Type.GetType("System.Int32"))
            dtDetails.Columns.Add("Remark", Type.GetType("System.String"))
            dtDetails.Columns.Add("GST", Type.GetType("System.String"))
            dtDetails.Columns.Add("SelectedGST", Type.GetType("System.String"))
            dtDetails.Columns.Add("GSTTaxAmount", Type.GetType("System.Double"))
            dtDetails.Columns.Add("DeliveryAddr", Type.GetType("System.String"))
            dtDetails.Columns.Add("AcctIndex", Type.GetType("System.String"))
            dtDetails.Columns.Add("ProductType", Type.GetType("System.String"))
            dtDetails.Columns.Add("Source", Type.GetType("System.String"))
            dtDetails.Columns.Add("TAXID", Type.GetType("System.String"))
            dtDetails.Columns.Add("MOQ", Type.GetType("System.String"))
            dtDetails.Columns.Add("MPQ", Type.GetType("System.String"))
            dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
            dtDetails.Columns.Add("POD_RFQ_ITEM_LINE", Type.GetType("System.String"))
            'Stage 3 Enhancement (GST-0010) - 16/07/2015 - CH
            dtDetails.Columns.Add("GSTTaxCode", Type.GetType("System.String"))

            ' _Yap: For Interface
            dtDetails.Columns.Add("ItemCode", Type.GetType("System.String"))

            dtDetails.Columns.Add("WarrantyTerms", Type.GetType("System.Int32"))
            'dtDetails.Columns.Add("CDGroup", Type.GetType("System.String"))
            ' _Yap: For Interface
            'dtDetails.Columns.Add("ItemCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("CategoryCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("GLCode", Type.GetType("System.String"))

            dtDetails.Columns.Add("RfqQty", Type.GetType("System.Int32"))
            dtDetails.Columns.Add("QtyTolerance", Type.GetType("System.Int32"))
            dtDetails.Columns.Add("SupplierCompanyId", Type.GetType("System.String"))

            dtDetails.Columns.Add("AssetGroup", Type.GetType("System.String"))
            dtDetails.Columns.Add("AssetGroupNo", Type.GetType("System.String"))

            'Jules 2018.05.07 - PAMB Scrum 2 & 3
            dtDetails.Columns.Add("Gift", Type.GetType("System.String"))
            dtDetails.Columns.Add("FundType", Type.GetType("System.String"))
            dtDetails.Columns.Add("PersonCode", Type.GetType("System.String"))
            dtDetails.Columns.Add("ProjectCode", Type.GetType("System.String"))
            'End modification.


            dtMaster.Rows.Add(dtr)
            ds.Tables.Add(dtMaster)
            ds.Tables.Add(dtDetails)
        End Function
        'Michelle (18/11/2010) 
        'Public Function submitPO(ByVal userID As String, ByVal companyID As String, ByVal strPO As String, ByVal dtAO As DataTable, Optional ByVal strType As String = "", Optional ByVal frmApprSetup As Boolean = False) As Integer
        '    Dim strsql As String
        '    Dim i As Integer
        '    Dim strAryQuery(0) As String
        '    Dim strIndex As String
        '    Dim ds As New DataSet
        '    Dim objPR As New PR

        '    strsql = "SELECT POM_PO_INDEX, POM_PO_STATUS FROM PO_MSTR WHERE POM_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & companyID & "' "
        '    ds = objDb.FillDs(strsql)

        '    If ds.Tables(0).Rows.Count > 0 Then
        '        strIndex = ds.Tables(0).Rows(0)("POM_PO_INDEX")
        '        ' check PO status still is draft
        '        If Common.parseNull(ds.Tables(0).Rows(0)("POM_PO_STATUS")) = "0" Then
        '            strsql = "UPDATE PO_MSTR SET "
        '            strsql &= "POM_STATUS_CHANGED_ON = " & Common.ConvertDate(Now) & ", "
        '            strsql &= "POM_STATUS_CHANGED_BY = '" & userID & "', "
        '            strsql &= "POM_PO_STATUS = '" & POStatus_new.Submitted & "' "
        '            strsql &= "WHERE POM_PO_INDEX = '" & strIndex & "' "
        '            Common.Insert2Ary(strAryQuery, strsql)

        '            ' insert into PR_APPROVAL
        '            For i = 0 To dtAO.Rows.Count - 1
        '                strsql = "INSERT INTO PR_APPROVAL (PRA_PR_INDEX, PRA_AO, PRA_A_AO, PRA_SEQ, PRA_AO_ACTION, "
        '                strsql &= "PRA_APPROVAL_TYPE, PRA_APPROVAL_GRP_INDEX, PRA_RELIEF_IND, PRA_FOR ) VALUES ("
        '                strsql &= strIndex & ", "                        ' PRA_PR_INDEX
        '                strsql &= "'" & Common.Parse(dtAO.Rows(i)("AO")) & "', "                         ' PRA_AO
        '                strsql &= "'" & Common.Parse(dtAO.Rows(i)("AAO")) & "', "                        ' PRA_A_AO
        '                strsql &= dtAO.Rows(i)("Seq") & ", 0, "                      ' PRA_SEQ, PRA_AO_ACTION
        '                If frmApprSetup Then
        '                    strsql &= "'" & Common.Parse(dtAO.Rows(i)("Type")) & "', "   ' PRA_APPROVAL_TYPE
        '                Else
        '                    strsql &= "'1', "
        '                End If
        '                strsql &= dtAO.Rows(i)("GrpIndex") & ", "                        ' PRA_APPROVAL_GRP_INDEX
        '                strsql &= "'" & Common.Parse(dtAO.Rows(i)("Relief")) & "', 'PO' ) "                         ' PRA_RELIEF_IND
        '                Common.Insert2Ary(strAryQuery, strsql)
        '            Next

        '            Dim objBudget As New BudgetControl
        '            Dim strBCM, strCurrency As String
        '            Dim blnExceed As Boolean
        '            Dim dtBCM As New DataTable
        '            If HttpContext.Current.Session("Env") <> "FTN" Then
        '                objBudget.BCMCalc("PO", companyID, strPO, EnumBCMAction.SubmitPR, strAryQuery)
        '                blnExceed = objBudget.checkBCM(strPO, companyID, dtBCM, strBCM)
        '                strsql = "SELECT POM_CURRENCY_CODE FROM PO_MSTR "
        '                strsql &= "WHERE POM_PO_NO = '" & Common.Parse(strPO) & "' AND POM_B_COY_ID = '" & companyID & "' "
        '                Dim tDS As DataSet = objDb.FillDs(strsql)
        '                For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
        '                    strCurrency = tDS.Tables(0).Rows(j).Item("POM_CURRENCY_CODE")
        '                Next

        '                Dim objUsers As New Users
        '                objUsers.Log_UserActivity(strAryQuery, WheelModule.PRMod, WheelUserActivity.B_SubmitPR, strPO)
        '                objUsers = Nothing
        '            End If

        '            If objDb.BatchExecute(strAryQuery) Then
        '                If HttpContext.Current.Session("Env") <> "FTN" Then
        '                    'Michelle (19/3/2008) - To allow mail to send to AO evenif budget has been bust when the budget mode is 'Advisory'
        '                    If strType = "0" Or strType = "1" Then
        '                        objPR.sendMailToAO(strPO, CLng(strIndex), 1)
        '                    End If
        '                    If strType <> "0" Then
        '                        ' PR value more than operating budget
        '                        objPR.RequestBudgetTopup(strBCM, "", "", strCurrency, dtBCM)

        '                    End If


        '                End If
        '                submitPO = WheelMsgNum.Save
        '                objBudget = Nothing
        '            Else
        '                objPR.sendMailToAO(strPO, CLng(strIndex), 1, "PO")
        '            End If
        '        Else
        '            submitPO = WheelMsgNum.NotSave
        '        End If
        '    Else
        '        submitPO = WheelMsgNum.Delete
        '    End If

        'End Function
        'Function ApprovePO(ByVal strPONo As String, ByVal intPOIndex As Long, ByVal intCurrentSeq As Integer, _
        'ByVal blnHighestLevel As Boolean, ByVal strAORemark As String, _
        'ByVal strBuyer As String, ByVal blnRelief As Boolean, ByVal strApprType As String, ByVal strSCoyID As String) As String
        '    Dim strSql, strSql1 As String
        '    Dim strSqlAry(0) As String
        '    Dim strCoyID, strMsg, strPO, strVendor, strLoginUser As String
        '    Dim intPOStatus As Integer
        '    Dim strvendorname As String
        '    Dim strMsg1 As String
        '    Dim strVendorNameList As String
        '    Dim arrPOInfo, arrPo As Array
        '    Dim intLowBound, intUpBound, intLoop As Integer
        '    Dim strVen, strVenList As String
        '    Dim objPR2 As New PurchaseReq2

        '    If strApprType = "1" Then
        '        strApprType = "approved"
        '    Else
        '        strApprType = "endorsed"
        '    End If
        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strLoginUser = HttpContext.Current.Session("UserId") 'is a AO
        '    strSql = "SELECT POM_PO_STATUS,POM_STATUS_CHANGED_BY,POM_S_COY_ID,POM_S_COY_NAME, POM_BUYER_NAME FROM PO_MSTR WHERE POM_PO_Index=" & intPOIndex
        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        intPOStatus = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS")
        '        strVendor = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_S_COY_ID"))
        '        If intPOStatus = POStatus_new.Cancelled Or intPOStatus = POStatus_new.CancelledBy Then
        '            strMsg = "The PO has already been cancelled. Approving of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.RejectedBy Then
        '            strMsg = "You have already rejected this PO. Approving of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.Approved Then
        '            strMsg = "You have already approved/endorsed this PO."
        '        End If
        '    End If
        '    strCoyID = HttpContext.Current.Session("CompanyId")

        '    'If (intPOStatus = POStatus_new.PendingApproval Or intPOStatus = POStatus_new.Submitted) And objPR2.isApproved(intPOIndex, intCurrentSeq, strLoginUser, "PO") Then
        '    '    strMsg = "You have already approved/endorsed this PO. Approving of this PO is not allowed."
        '    'End If

        '    If strMsg <> "" Then
        '        Return strMsg
        '    End If
        '    If blnHighestLevel Then
        '        '//update PO status to Approved/Endorsed, status_changed_by,status_changed_date
        '        strSql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & POStatus_new.NewPO & _
        '        ",POM_STATUS_CHANGED_BY='" & strLoginUser & "',POM_STATUS_CHANGED_ON=" & _
        '        Common.ConvertDate(Now) & ", POM_PO_DATE=" & Common.ConvertDate(Now) & " WHERE POM_PO_Index=" & intPOIndex

        '        Common.Insert2Ary(strSqlAry, strSql)

        '        strSql = "UPDATE PO_DETAILS, PO_MSTR " & _
        '                "SET POD_ETD = DateDiff(CURDATE(), POM_CREATED_DATE) " & _
        '                "WHERE POM_PO_INDEX = " & intPOIndex & " AND POM_B_COY_ID = POD_COY_ID " & _
        '                "AND POM_PO_NO = POD_PO_NO AND ADDDATE(POM_CREATED_DATE, POD_ETD) < CURDATE() "

        '        Common.Insert2Ary(strSqlAry, strSql)

        '        'Update last transacted price
        '        strSql = "UPDATE PRODUCT_MSTR, PO_DETAILS, PO_MSTR " & _
        '                "SET PM_LAST_TXN_PRICE = POD_UNIT_COST, " & _
        '                "PM_LAST_TXN_PRICE_CURR = POM_CURRENCY_CODE " & _
        '                "WHERE POM_PO_INDEX = '" & intPOIndex & "' AND POM_B_COY_ID = POD_COY_ID " & _
        '                "AND POM_PO_NO = POD_PO_NO AND PM_S_COY_ID = POM_B_COY_ID " & _
        '                "AND POD_PRODUCT_CODE = PM_PRODUCT_CODE "
        '        Common.Insert2Ary(strSqlAry, strSql)

        '        objPR2.updateAOAction(intPOIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PO")
        '        '//send mail to consolidator
        '        '//return message = PO Approved/endorsed
        '        strMsg = "PO No. " & strPONo & " has been " & strApprType & ". "
        '    Else
        '        '//update PO status, status_changed_by,status_changed_date
        '        strSql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & POStatus_new.PendingApproval & _
        '        ",POM_STATUS_CHANGED_BY='" & strLoginUser & "',POM_STATUS_CHANGED_ON=" & _
        '        Common.ConvertDate(Now) & " WHERE POM_PO_Index=" & intPOIndex

        '        Common.Insert2Ary(strSqlAry, strSql)
        '        objPR2.updateAOAction(intPOIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PO")
        '        '//notify next AO
        '        '//return message = PO Approved/endorsed
        '        strMsg = "PO No. " & strPONo & " has been " & strApprType & ". "
        '    End If
        '    Dim objUsers As New Users
        '    objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_ApprovePO, strPONo)
        '    objUsers = Nothing

        '    If Not objDb.BatchExecute(strSqlAry) Then
        '        strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
        '    Else
        '        '//only send mail if transaction successfully created
        '        Dim objMail As New Email
        '        If blnHighestLevel Then
        '            Dim strName, strPoList As String
        '            strName = tDS.Tables(0).Rows(0).Item("POM_BUYER_NAME")

        '            'For intLoop = 0 To intUpBound 'Sending email to vendors
        '            '    arrPo = Split(arrPOInfo(intLoop), "!")
        '            '    strPoNo = arrPo(0) 'Capture the PO No.
        '            '    strVen = arrPo(1)  'Caputre the Vendor code
        '            '    If intLoop = 0 Then
        '            '        strPoList = strPoNo
        '            '    Else
        '            '        strPoList = strPoList & ", " & strPoNo
        '            '    End If

        '            objMail.sendNotification(EmailType.PORaised, "", strCoyID, strSCoyID, strPONo, "")
        '            'Next
        '            'If intUpBound > 0 Then
        '            '    strVendorNameList = strVendorNameList & " respectively"
        '            'End If
        '            objMail.sendNotification(EmailType.POApproved, strLoginUser, strCoyID, strSCoyID, strPONo, strPoList, strName, strVendorNameList)
        '        Else
        '            '//next ao
        '            Dim objPR As New PR
        '            objPR.sendMailToAO(strPONo, intPOIndex, intCurrentSeq + 1, "PO")
        '            objPR = Nothing
        '        End If
        '    End If

        '    Return strMsg
        'End Function

        'Public Function getPOTempAttach(ByVal strDocNo As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "SELECT * FROM COMPANY_DOC_ATTACHMENT "
        '    strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PO' "
        '    ds = objDb.FillDs(strsql)
        '    getPOTempAttach = ds
        'End Function

        'Public Function delete_Attachment(ByVal strDocNo As String) As DataSet
        '    Dim strsql As String
        '    Dim ds As New DataSet
        '    strsql = "DELETE FROM COMPANY_DOC_ATTACHMENT "
        '    strsql &= "WHERE CDA_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "AND CDA_DOC_NO = '" & strDocNo & "' AND CDA_DOC_TYPE = 'PO' "
        '    ds = objDb.FillDs(strsql)

        '    ds = Nothing
        'End Function

        'Public Function deletePO(ByVal strPO As String)
        '    Dim strsql As String
        '    Dim strAryQuery(0) As String
        '    '' delete from PR_DETAILS table
        '    'strsql = "DELETE FROM PO_DETAILS "
        '    'strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '    'strsql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    'Common.Insert2Ary(strAryQuery, strsql)

        '    ' delete from PR_MST table
        '    strsql = "UPDATE PO_MSTR SET POM_PO_STATUS = " & POStatus_new.Void & ", POM_STATUS_CHANGED_BY = '" & HttpContext.Current.Session("UserId") & "', POM_STATUS_CHANGED_ON=" & Common.ConvertDate(Now)
        '    strsql &= " WHERE POM_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    Common.Insert2Ary(strAryQuery, strsql)

        '    objDb.BatchExecute(strAryQuery)
        'End Function

        'Public Function deleteRFQItemSQL(ByVal strRFQ As String, ByVal sProductCode As Integer) As String
        '    Dim strsql As String
        '    ' delete from PR_DETAILS table
        '    strsql = "DELETE FROM RFQ_REPLIES_DETAIL "
        '    strsql &= "WHERE RRD_RFQ_ID = '" & strRFQ & "' "
        '    strsql &= "AND RRD_Product_Code = '" & sProductCode & "'; "

        '    deleteRFQItemSQL = strsql
        'End Function

        'Public Function deletePOItemSQL(ByVal strPO As String, ByVal sProductCode As Integer, ByVal sProductIndex As Integer) As String
        '    Dim strsql As String
        '    ' delete from PR_DETAILS table
        '    strsql = "DELETE FROM PO_DETAILS "
        '    strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "AND POD_PO_LINE = '" & sProductIndex & "' "
        '    strsql &= "AND POD_PRODUCT_CODE = '" & sProductCode & "'; "

        '    '' delete from PR_CUSTOM_FIELD_DETAILS table
        '    'strsql &= "DELETE FROM PR_CUSTOM_FIELD_DETAILS "
        '    'strsql &= "WHERE PCD_PR_LINE = " & sProductCode & " "
        '    'strsql &= "AND PCD_PR_INDEX IN "
        '    'strsql &= "(SELECT POM_PO_INDEX FROM PO_MSTR "
        '    'strsql &= "WHERE POM_PO_NO = '" & strPO & "'); "

        '    deletePOItemSQL = strsql
        'End Function

        'Public Function updatePODIndex(ByVal strPO As String, ByVal sProductCode As Integer, ByVal sProductIndex As Integer) As String
        '    Dim strsql As String

        '    strsql = "UPDATE PO_DETAILS "
        '    strsql &= "SET POD_PO_LINE = '" & sProductIndex & "' "
        '    strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "AND POD_PO_LINE >= '" & sProductIndex & "' "
        '    strsql &= "AND POD_PRODUCT_CODE = '" & sProductCode & "' "
        '    strsql &= "ORDER BY POD_PO_LINE "
        '    strsql &= "LIMIT 1 ; "

        '    updatePODIndex = strsql
        'End Function

        'Function RejectPO(ByVal strPONo As String, ByVal intPOIndex As Long, ByVal intCurrentSeq As Integer, ByVal strAORemark As String, ByVal strUserID As String, ByVal blnRelief As Boolean) As String
        '    Dim strSql, strSqlAry(0) As String
        '    Dim strCoyID, strLoginUser As String
        '    Dim intPOStatus As Integer
        '    Dim strMsg As String
        '    Dim objPR2 As New PurchaseReq2

        '    strCoyID = HttpContext.Current.Session("CompanyId")
        '    strSql = "SELECT POM_PO_STATUS,POM_STATUS_CHANGED_BY FROM PO_MSTR WHERE POM_PO_Index=" & intPOIndex
        '    Dim tDS As DataSet = objDb.FillDs(strSql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        intPOStatus = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS")
        '        If intPOStatus = POStatus_new.Cancelled Or intPOStatus = POStatus_new.CancelledBy Then
        '            strMsg = "The PO has already been cancelled. Rejecting of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.Void Then
        '            strMsg = "The PO has already been void. Rejecting of this PO is not allowed. "
        '        ElseIf intPOStatus = POStatus_new.RejectedBy And tDS.Tables(0).Rows(0).Item("POM_STATUS_CHANGED_BY") = strUserID Then
        '            strMsg = "You have already rejected this PO."
        '        ElseIf intPOStatus = POStatus_new.Approved Then '//approved '//NEED TO PR-APPROVAL
        '            strMsg = "You have already approved this PO. Rejecting of this PO is not allowed."
        '        End If
        '    End If

        '    If intPOStatus = POStatus_new.PendingApproval And objPR2.isApproved(intPOIndex, intCurrentSeq, strLoginUser, "PO") Then
        '        strMsg = "You have already approved this PO. Rejecting of this PO is not allowed."
        '    End If

        '    strLoginUser = HttpContext.Current.Session("UserId") 'is a AO
        '    If strMsg = "" Then
        '        Dim objUsers As New Users
        '        objUsers.Log_UserActivity(strSqlAry, WheelModule.PRMod, WheelUserActivity.AO_RejectPO, strPONo)
        '        objUsers = Nothing
        '        strSql = "UPDATE PO_MSTR SET POM_PO_STATUS=" & POStatus_new.RejectedBy & _
        '        ",POM_STATUS_CHANGED_BY='" & strUserID & "',POM_STATUS_CHANGED_ON=" & _
        '        Common.ConvertDate(Now) & " WHERE POM_PO_Index=" & intPOIndex
        '        Common.Insert2Ary(strSqlAry, strSql)
        '        objPR2.updateAOAction(intPOIndex, intCurrentSeq, strAORemark, strSqlAry, blnRelief, "PO")

        '        'Dim objBCM As New BudgetControl
        '        'objBCM.BCMCalc("PR", strPRNo, EnumBCMAction.RejectPR, strSqlAry)
        '        'objBCM = Nothing

        '        If objDb.BatchExecute(strSqlAry) Then
        '            Dim objMail As New Email
        '            Dim objPR As New PR
        '            Dim strName As String
        '            strName = objPR.getRequestorName("PO", strPONo, strCoyID)
        '            objMail.sendNotification(EmailType.PORejectedBy, strUserID, strCoyID, "", strPONo, strName)
        '            objMail = Nothing
        '            objPR = Nothing
        '            strMsg = "Purchase Order Number " & strPONo & " has been rejected."
        '            '//SEND MAIL TO BUYER
        '        Else
        '            strMsg = "Error occurs. Kindly contact the Administrator to resolve this. "
        '        End If
        '        Return strMsg

        '    Else
        '        Return strMsg
        '    End If
        'End Function
        'Function MassApprovalPO(ByVal strAryPONo() As String, ByVal strAryPOIndex() As String, ByVal strAO As String, ByRef strReturnMsg() As String, ByVal blnRelief As Boolean) As Boolean
        '    Dim strSql, strSql1, strAryQuery(0), strApprType As String
        '    Dim strCoyID, strAllPO, strAllPOIndex As String
        '    Dim intLoop As Integer
        '    Dim ds As DataSet
        '    Dim strMsg As String

        '    strCoyID = HttpContext.Current.Session("CompanyId")

        '    For intLoop = 0 To strAryPONo.GetUpperBound(0)
        '        If intLoop = 0 Then
        '            strAllPO = "'" & strAryPONo(intLoop) & "'"
        '            strAllPOIndex = strAryPOIndex(intLoop)
        '        Else
        '            strAllPO = strAllPO & ",'" & strAryPONo(intLoop) & "'"
        '            strAllPOIndex = strAllPOIndex & "," & strAryPOIndex(intLoop)
        '        End If
        '    Next

        '    strSql = "SELECT POM_PO_INDEX,POM_PO_NO,POM_S_COY_NAME,POM_S_EMAIL,POM_PO_STATUS,POM_STATUS_CHANGED_BY,POM_BUYER_ID, POM_S_COY_ID FROM PO_MSTR WHERE POM_PO_NO IN (" & strAllPO & ") And POM_B_COY_ID='" & strCoyID & "'"
        '    strSql1 = "SELECT * FROM PR_APPROVAL WHERE PRA_PR_INDEX IN (" & strAllPOIndex & ") AND PRA_FOR = 'PO' "

        '    ds = objDb.FillDs(strSql & ";" & strSql1)
        '    ds.Tables(0).TableName = "PO_MSTR"
        '    ds.Tables(1).TableName = "PR_APPROVAL"

        '    If Not ds Is Nothing Then
        '        Dim parentCol As DataColumn
        '        Dim childCol As DataColumn
        '        Dim dvChildView, dvParentView As DataView
        '        Dim POrow, POApprrow As DataRow
        '        Dim intCurrentSeq, intLastSeq As Integer
        '        Dim blnHighestLevel, blnCanApprove As Boolean
        '        Dim strActiveAO As String


        '        parentCol = ds.Tables("PO_MSTR").Columns("POM_PO_INDEX")
        '        childCol = ds.Tables("PR_APPROVAL").Columns("PRA_PR_INDEX")

        '        ' Create DataRelation.
        '        Dim relPO As DataRelation
        '        relPO = New DataRelation("acct", parentCol, childCol)
        '        ' Add the relation to the DataSet.
        '        ds.Relations.Add(relPO)
        '        For Each POrow In ds.Tables("PO_MSTR").Rows
        '            blnCanApprove = True
        '            If POrow("POM_PO_STATUS") = POStatus_new.PendingApproval Or _
        '            POrow("POM_PO_STATUS") = POStatus_new.Submitted Or _
        '            POrow("POM_PO_STATUS") = POStatus_new.HeldBy Then
        '                For Each POApprrow In POrow.GetChildRows(relPO)
        '                    intLastSeq = POApprrow("PRA_AO_Action")
        '                    intCurrentSeq = intLastSeq + 1

        '                    '//check whether the PO was already approved by the same AO at the
        '                    '//same approving level.
        '                    If POApprrow("PRA_Seq") = intCurrentSeq Then
        '                        strApprType = POApprrow("PRA_APPROVAL_TYPE")
        '                        strActiveAO = Common.parseNull(POApprrow("PRA_ACTIVE_AO"))
        '                        If strActiveAO <> "" Then
        '                            strMsg = "You have already approved/endorsed PO No. " & POrow("POM_PO_NO") & ". Approving of this PO is not allowed."
        '                            Common.Insert2Ary(strReturnMsg, strMsg)
        '                            blnCanApprove = False
        '                            Exit For
        '                        Else
        '                            If Not (UCase(POApprrow("PRA_AO")) = UCase(strAO) Or _
        '                             UCase(Common.parseNull(POApprrow("PRA_A_AO"))) = UCase(strAO)) Then
        '                                strMsg = "You are not a authorised person to approve PO No. " & POrow("POM_PO_NO")
        '                                Common.Insert2Ary(strReturnMsg, strMsg)
        '                                blnCanApprove = False
        '                                Exit For
        '                            End If
        '                        End If
        '                    End If

        '                    If intCurrentSeq = POApprrow("PRA_SEQ") Then
        '                        blnHighestLevel = True
        '                    Else
        '                        blnHighestLevel = False
        '                    End If
        '                Next
        '                If blnCanApprove Then
        '                    If strApprType = "1" Then
        '                        strMsg = ApprovePO(POrow("POM_PO_NO"), POrow("POM_PO_INDEX"), intCurrentSeq, blnHighestLevel, "Approved : ", Common.parseNull(POrow("POM_BUYER_ID")), blnRelief, strApprType, POrow("POM_S_COY_ID"))
        '                    Else
        '                        strMsg = ApprovePO(POrow("POM_PO_NO"), POrow("POM_PO_INDEX"), intCurrentSeq, blnHighestLevel, "Endorsed : ", Common.parseNull(POrow("POM_BUYER_ID")), blnRelief, strApprType, POrow("POM_S_COY_ID"))
        '                    End If

        '                    Common.Insert2Ary(strReturnMsg, strMsg)
        '                End If
        '            Else '//ERROR
        '                '//PO APPROVED/REJECTED/CANCELLED                       
        '                If POrow("POM_PO_STATUS") = POStatus_new.Cancelled Or POrow("POM_PO_STATUS") = POStatus_new.CancelledBy Then
        '                    strMsg = "PO No. " & POrow("POM_PO_NO") & " has already been cancelled. Approving of this PO is not allowed. "
        '                ElseIf POrow("POM_PO_STATUS") = POStatus_new.RejectedBy Then
        '                    strMsg = "You have already rejected PO No. " & POrow("POM_PO_NO") & ". Approving of this PO is not allowed. "
        '                ElseIf POrow("POM_PO_STATUS") = POStatus_new.Approved Then
        '                    strMsg = "You have already approved PO No. " & POrow("POM_PO_NO") & " to PO. Approving of this PO is not allowed."
        '                End If
        '                Common.Insert2Ary(strReturnMsg, strMsg)
        '            End If
        '        Next
        '    End If
        '    Return True
        'End Function
        '//modify By Moo
        '//add one more checking - Buyer Company ID
        'Public Function update_POStatus(ByVal ds As DataSet) As String
        '    Dim strSql As String
        '    Dim date2day As String = Date.Now
        '    Dim remark As String
        '    Dim strarray(0), strMsg As String
        '    Dim objBCM As New BudgetControl
        '    Dim objMail As New Email
        '    Dim objUsers As New Users
        '    Dim i As Integer
        '    Dim strPOStatus As String
        '    Dim fulfilopen As Integer = Fulfilment.Open

        '    For i = 0 To ds.Tables(0).Rows.Count - 1
        '        strSql = "SELECT ISNULL(POM_PO_STATUS,1) FROM PO_MSTR WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & "' AND POM_S_COY_ID= '" & Common.Parse(v_com_id) & "' AND POM_B_COY_ID= '" & Common.Parse(ds.Tables(0).Rows(i)("BCoyID")) & "'"
        '        strPOStatus = objDb.GetVal(strSql)

        '        If ds.Tables(0).Rows(i)("status") = POStatus_new.Accepted Or ds.Tables(0).Rows(i)("status") = POStatus_new.Rejected Then
        '            If CInt(strPOStatus) = POStatus_new.Accepted Then
        '                strMsg = "You have already accepted this PO."
        '            ElseIf CInt(strPOStatus) = POStatus_new.Rejected Then
        '                strMsg = "You have already rejected this PO."
        '            ElseIf CInt(strPOStatus) = POStatus_new.Cancelled Or CInt(strPOStatus) = POStatus_new.CancelledBy Then
        '                strMsg = "This PO already cancelled."
        '            End If
        '        End If

        '        If strMsg <> "" Then Return strMsg
        '        remark = ds.Tables(0).Rows(i)("remark")
        '        If remark = "" Then
        '            If ds.Tables(0).Rows(i)("status") <> POStatus_new.Accepted Then
        '                strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & ",POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "
        '            Else
        '                strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & ",POM_ACCEPTED_DATE= " & Common.ConvertDate(date2day) & " ,POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' , POM_FULFILMENT= '" & Common.Parse(fulfilopen) & "'"
        '            End If

        '        Else
        '            If ds.Tables(0).Rows(i)("status") <> POStatus_new.Accepted Then
        '                strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & ",POM_S_REMARK= '" & Common.Parse(remark) & "',POM_ACCEPTED_DATE= " & Common.ConvertDate(date2day) & " ,POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' "

        '            Else
        '                strSql = "update PO_MSTR SET POM_PO_STATUS= '" & Common.Parse(ds.Tables(0).Rows(i)("status")) & "',POM_STATUS_CHANGED_ON=" & Common.ConvertDate(date2day) & " " & _
        '                " ,POM_S_REMARK= '" & Common.Parse(remark) & "',POM_ACCEPTED_DATE= " & Common.ConvertDate(date2day) & " ,POM_STATUS_CHANGED_BY= '" & Common.Parse(HttpContext.Current.Session("UserId")) & "' , POM_FULFILMENT= '" & Common.Parse(fulfilopen) & "'"
        '            End If

        '        End If
        '        strSql = strSql & " WHERE POM_PO_NO= '" & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & "' and POM_S_COY_ID= '" & Common.Parse(v_com_id) & "' AND POM_B_COY_ID= '" & Common.Parse(ds.Tables(0).Rows(i)("BCoyID")) & "'"
        '        Common.Insert2Ary(strarray, strSql)

        '        '//ADD BY MOO,
        '        If ds.Tables(0).Rows(i)("status") = POStatus_new.Accepted Then
        '            objBCM.BCMCalc("PO", ds.Tables(0).Rows(i)("datakey"), EnumBCMAction.AcceptPO, strarray, ds.Tables(0).Rows(i)("BCoyID"))
        '            '//for audit trail
        '            objUsers.Log_UserActivity(strarray, WheelModule.OrderMgnt, WheelUserActivity.V_AcceptPO, ds.Tables(0).Rows(i)("datakey"))
        '        ElseIf ds.Tables(0).Rows(i)("status") = POStatus_new.Rejected Then
        '            objBCM.BCMCalc("PO", ds.Tables(0).Rows(i)("datakey"), EnumBCMAction.RejectPO, strarray, ds.Tables(0).Rows(i)("BCoyID"))
        '            objUsers.Log_UserActivity(strarray, WheelModule.OrderMgnt, WheelUserActivity.V_RejectPO, ds.Tables(0).Rows(i)("datakey"))
        '        End If
        '    Next

        '    If objDb.BatchExecute(strarray) Then
        '        For i = 0 To ds.Tables(0).Rows.Count - 1
        '            If ds.Tables(0).Rows(i)("status") = POStatus_new.Accepted Then
        '                Dim objPR As New PR
        '                Dim strName As String
        '                strName = objPR.getRequestorName("PO", ds.Tables(0).Rows(i)("datakey"), ds.Tables(0).Rows(i)("BCoyID"))

        '                objMail.sendNotification(EmailType.POAccepted, "", ds.Tables(0).Rows(i)("BCoyID"), v_com_id, ds.Tables(0).Rows(i)("datakey"), strName)
        '                strMsg = "PO Number " & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & " has been accepted."
        '                objPR = Nothing
        '            ElseIf ds.Tables(0).Rows(i)("status") = POStatus_new.Rejected Then
        '                objMail.sendNotification(EmailType.PORejected, "", ds.Tables(0).Rows(i)("BCoyID"), v_com_id, ds.Tables(0).Rows(i)("datakey"), "")
        '                strMsg = "PO Number " & Common.Parse(ds.Tables(0).Rows(i)("datakey")) & " has been rejected."
        '            End If
        '        Next
        '    Else
        '        strMsg = "Error occurs. Kindly contact the Administrator to resolve this."
        '    End If
        '    objBCM = Nothing
        '    objMail = Nothing
        '    objUsers = Nothing
        '    Return strMsg
        'End Function

        Public Function get_customfield(ByRef value1() As String, ByRef value2() As String, ByVal pr_line As String, ByVal pr_index As String, ByRef i As Integer)

            '//Remark by Moo, to cater for pr consolidation
            'Dim strsql As String = "select PCM_FIELD_NAME,PCD_FIELD_VALUE " & _
            '                        "from PO_DETAILS left join " & _
            '                        " PR_CUSTOM_FIELD_MSTR on PCM_PR_INDEX = POD_PR_INDEX  " & _
            '                        "left join PR_CUSTOM_FIELD_DETAILS on " & _
            '                        "PCD_PR_INDEX = PCM_PR_INDEX And PCD_PR_LINE = POD_PR_LINE " & _
            '                        "and PCD_FIELD_NO=PCM_FIELD_NO where " & _
            '                        "POD_PR_INDEX='" & po_index & "' AND POD_PO_LINE='" & po_line & "'"

            Dim strsql As String = "select PCM_FIELD_NAME,PCD_FIELD_VALUE " &
                                   "from PR_CUSTOM_FIELD_MSTR INNER JOIN PR_CUSTOM_FIELD_DETAILS on " &
                                   "PCD_PR_INDEX = PCM_PR_INDEX And PCD_FIELD_NO=PCM_FIELD_NO where " &
                                   "PCD_TYPE = 'PO' AND PCM_PR_INDEX= '" & Common.Parse(pr_index) & "' AND PCD_PR_LINE= '" & Common.Parse(pr_line) & "'"
            Dim tDS As DataSet = objDb.FillDs(strsql)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                value1(i) = tDS.Tables(0).Rows(j).Item("PCM_FIELD_NAME").ToString.Trim
                value2(i) = tDS.Tables(0).Rows(j).Item("PCD_FIELD_VALUE").ToString.Trim
                i = i + 1
            Next

        End Function
        Public Function get_comadd(ByVal item As POValue, ByVal side As String, ByVal blnPreview As Boolean) As String
            Dim strsql As String '="select * from company_mstr where cm_coy_ID='"&  &"'"
            Dim objGlobal As New AppGlobals

            If Not blnPreview Then
                If item.CR_NO <> "" Then
                    strsql = "Select PO_MSTR.*,PO_CR_MSTR.*,COMPANY_MSTR.* From PO_MSTR INNER JOIN PO_CR_MSTR on PCM_PO_INDEX =POM_PO_INDEX " &
                    " INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_B_COY_ID INNER JOIN USER_MSTR UMA ON UMA.UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UMA.UM_COY_ID " &
                    " Inner Join USER_MSTR UMB ON UMB.UM_USER_ID=PCM_REQ_BY AND PCM_B_COY_ID=UMB.UM_COY_ID Where " &
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
                Else
                    strsql = "Select PO_MSTR.*,COMPANY_MSTR.*,UM_EMAIL from PO_MSTR INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_B_COY_ID INNER JOIN USER_MSTR ON UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UM_COY_ID Where " &
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
                End If
            Else
                strsql = "Select PO_MSTR_PREVIEW.*,COMPANY_MSTR.*,UM_EMAIL from PO_MSTR_PREVIEW INNER JOIN COMPANY_MSTR ON " &
                "CM_COY_ID=POM_S_COY_ID INNER JOIN USER_MSTR ON UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UM_COY_ID Where " &
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
            End If
            Dim strTempAddr As String

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then

                'item.PO_Status = read2("POM_PO_STATUS")

                'item.buyer_adds = read2("POM_B_ADDR_LINE1").ToString.Trim & "," & read2("POM_B_ADDR_LINE2").ToString.Trim & "," & read2("POM_B_ADDR_LINE3").ToString.Trim & " " & read2("POM_B_POSTCODE").ToString.Trim & "," & read2("POM_B_CITY").ToString.Trim
                strTempAddr = tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_POSTCODE").ToString.Trim
                End If
                If tDS.Tables(0).Rows(0).Item("CM_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("CM_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("CM_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("CM_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("CM_COUNTRY").ToString.Trim)
                End If

                get_comadd = strTempAddr
            End If
            objGlobal = Nothing
        End Function
        Public Function get_PODetail(ByVal item As POValue, ByVal side As String, ByVal blnPreview As Boolean) As String

            Dim objGlobal As New AppGlobals
            Dim strsql As String
            '//MODIFY BY MOO
            '//TO CATER FOR PR CONSOLIDATION
            '//ADD pom_b_coy_id
            If Not blnPreview Then
                If item.CR_NO <> "" Then
                    strsql = "Select PO_MSTR.*,PO_CR_MSTR.*,COMPANY_MSTR.*,COMPANY_DELIVERY_TERM.*,UMA.UM_EMAIL,UMB.UM_USER_NAME,(SELECT CM_COY_NAME FROM COMPANY_MSTR A WHERE A.CM_COY_ID=POM_B_COY_ID) AS BCOY,C.STATUS_DESC AS PO_STATUS  From PO_MSTR INNER JOIN PO_CR_MSTR on PCM_PO_INDEX =POM_PO_INDEX " &
                    " INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_S_COY_ID INNER JOIN USER_MSTR UMA ON UMA.UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UMA.UM_COY_ID " &
                    " Inner Join USER_MSTR UMB ON UMB.UM_USER_ID=PCM_REQ_BY AND PCM_B_COY_ID=UMB.UM_COY_ID " &
                    " LEFT JOIN COMPANY_DELIVERY_TERM ON CDT_COY_ID = POM_B_COY_ID AND CDT_DEL_CODE = POM_DEL_CODE 
                    LEFT JOIN STATUS_MSTR C ON C.STATUS_NO = PO_MSTR.POM_PO_STATUS AND STATUS_TYPE = 'PO' WHERE " &
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
                Else
                    strsql = "Select PO_MSTR.*,COMPANY_MSTR.*,UM_EMAIL, COMPANY_DELIVERY_TERM.*, (SELECT CM_COY_NAME FROM COMPANY_MSTR A WHERE A.CM_COY_ID=POM_B_COY_ID) AS BCOY,C.STATUS_DESC AS PO_STATUS from PO_MSTR INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_S_COY_ID INNER JOIN USER_MSTR ON UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UM_COY_ID " &
                    " LEFT JOIN COMPANY_DELIVERY_TERM ON CDT_COY_ID = POM_B_COY_ID AND CDT_DEL_CODE = POM_DEL_CODE 
                    LEFT JOIN STATUS_MSTR C ON C.STATUS_NO = PO_MSTR.POM_PO_STATUS AND STATUS_TYPE = 'PO' WHERE " &
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
                End If
            Else
                strsql = "Select PO_MSTR.*,COMPANY_MSTR.*,UM_EMAIL,(SELECT CM_COY_NAME FROM COMPANY_MSTR A WHERE A.CM_COY_ID=POM_B_COY_ID) AS BCOY ,C.STATUS_DESC AS PO_STATUS from PO_MSTR INNER JOIN COMPANY_MSTR ON CM_COY_ID=POM_S_COY_ID INNER JOIN USER_MSTR ON UM_USER_ID=POM_BUYER_ID AND POM_B_COY_ID=UM_COY_ID 
                          LEFT JOIN STATUS_MSTR C ON C.STATUS_NO = PO_MSTR.POM_PO_STATUS AND STATUS_TYPE = 'PO' Where " &
                    " POM_PO_NO= '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "'"
            End If

            '//Remark By Moo, Unnecessary filtering
            'If side = "b" Then
            '    ' strsql = strsql & " AND POM_B_COY_ID= '" & com_id & "'"
            'ElseIf side = "v" Then
            '    strsql = strsql & " AND POM_S_COY_ID= '" & v_com_id & "'"
            'End If

            Dim strTempAddr As String

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then

                'item.PO_Status = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS")

                'item.buyer_adds = tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim & "," & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim & "," & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim & " " & tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim & "," & tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim
                strTempAddr = tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("CM_POSTCODE").ToString.Trim
                End If
                If tDS.Tables(0).Rows(0).Item("CM_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("CM_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("CM_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("CM_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("CM_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("CM_COUNTRY").ToString.Trim)
                End If

                item.buyer_adds = strTempAddr

                strTempAddr = tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_B_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_B_POSTCODE").ToString.Trim
                End If
                If tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("POM_B_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("POM_B_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("POM_B_COUNTRY").ToString.Trim)
                End If

                item.bill_adds = strTempAddr

                item.buyer_contact = tDS.Tables(0).Rows(0).Item("POM_BUYER_NAME").ToString.Trim
                'item.buyer_email = tDS.Tables(0).Rows(0).Item("")
                item.buyer_fax = tDS.Tables(0).Rows(0).Item("POM_BUYER_FAX").ToString.Trim
                item.buyer_tel = tDS.Tables(0).Rows(0).Item("POM_BUYER_PHONE").ToString.Trim
                item.cur = tDS.Tables(0).Rows(0).Item("POM_CURRENCY_CODE").ToString.Trim
                item.ex_rate = tDS.Tables(0).Rows(0).Item("POM_EXCHANGE_RATE").ToString.Trim
                item.BCoyName = Common.parseNull(tDS.Tables(0).Rows(0).Item("BCOY"))
                'item.gst_code = tDS.Tables(0).Rows(0).Item("POD_GST")
                item.POIndex = tDS.Tables(0).Rows(0).Item("POM_PO_INDEX")
                item.pay_meth = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_METHOD").ToString.Trim
                item.pay_term = tDS.Tables(0).Rows(0).Item("POM_PAYMENT_TERM").ToString.Trim
                If Not blnPreview Then
                    If IsDBNull(tDS.Tables(0).Rows(0).Item("CDT_DEL_NAME")) Then
                        item.del_term = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_DEL_CODE"))
                    Else
                        item.del_term = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_DEL_CODE")) & " (" & Common.parseNull(tDS.Tables(0).Rows(0).Item("CDT_DEL_NAME")) & ")"
                    End If
                End If

                item.vendor_code = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_VENDOR_CODE"))
                item.PO_Date = tDS.Tables(0).Rows(0).Item("POM_PO_DATE").ToString.Trim
                item.POM_ACCEPTED_DATE = tDS.Tables(0).Rows(0).Item("POM_ACCEPTED_DATE").ToString.Trim
                If Not blnPreview Then
                    item.POM_CREATED_DATE = tDS.Tables(0).Rows(0).Item("POM_CREATED_DATE").ToString.Trim
                End If
                item.PO_Status = tDS.Tables(0).Rows(0).Item("POM_PO_STATUS").ToString.Trim
                item.ship_meth = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_MODE").ToString.Trim
                item.ship_term = tDS.Tables(0).Rows(0).Item("POM_SHIPMENT_TERM").ToString.Trim
                item.ship_via = tDS.Tables(0).Rows(0).Item("POM_SHIP_VIA").ToString.Trim
                'item.tax = tDS.Tables(0).Rows(0).Item("POD_GST")
                'item.vendor_adds = tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE1").ToString.Trim & "," & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE2").ToString.Trim & "," & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE3").ToString.Trim & " " & tDS.Tables(0).Rows(0).Item("POM_S_POSTCODE").ToString.Trim & "," & tDS.Tables(0).Rows(0).Item("POM_S_CITY").ToString.Trim

                strTempAddr = tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE1").ToString.Trim

                If tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE2").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE2").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE3").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_ADDR_LINE3").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_POSTCODE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & tDS.Tables(0).Rows(0).Item("POM_S_POSTCODE").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_CITY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & tDS.Tables(0).Rows(0).Item("POM_S_CITY").ToString.Trim
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_STATE").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & "<BR>" & objGlobal.getCodeDesc(CodeTable.State, tDS.Tables(0).Rows(0).Item("POM_S_STATE").ToString.Trim)
                End If

                If tDS.Tables(0).Rows(0).Item("POM_S_COUNTRY").ToString.Trim <> "" Then
                    strTempAddr = strTempAddr & " " & objGlobal.getCodeDesc(CodeTable.Country, tDS.Tables(0).Rows(0).Item("POM_S_COUNTRY").ToString.Trim)
                End If
                item.vendor_adds = strTempAddr
                item.vendor_contact = tDS.Tables(0).Rows(0).Item("POM_S_ATTN").ToString.Trim
                item.vendor_CoyId = tDS.Tables(0).Rows(0).Item("POM_S_COY_ID").ToString.Trim
                item.vendor_Coy = tDS.Tables(0).Rows(0).Item("POM_S_COY_NAME").ToString.Trim
                item.vendor_email = tDS.Tables(0).Rows(0).Item("POM_S_EMAIL").ToString.Trim
                item.vendor_fax = tDS.Tables(0).Rows(0).Item("POM_S_FAX").ToString.Trim
                item.vendor_tel = tDS.Tables(0).Rows(0).Item("POM_S_PHONE").ToString.Trim
                item.Vendor_remark = tDS.Tables(0).Rows(0).Item("POM_S_REMARK").ToString.Trim
                item.TermAndCond = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_TERMANDCOND").ToString.Trim)
                If item.CR_NO <> "" Then
                    item.CR_NO = tDS.Tables(0).Rows(0).Item("PCM_CR_NO").ToString.Trim
                    item.CR_ReqDate = tDS.Tables(0).Rows(0).Item("PCM_REQ_DATE").ToString.Trim
                    item.CR_ReqBy = tDS.Tables(0).Rows(0).Item("UM_USER_NAME").ToString.Trim
                    item.remarks = tDS.Tables(0).Rows(0).Item("PCM_CR_REMARKS").ToString.Trim
                Else
                    item.remarks = tDS.Tables(0).Rows(0).Item("POM_EXTERNAL_REMARK").ToString.Trim
                    item.intremarks = tDS.Tables(0).Rows(0).Item("POM_INTERNAL_REMARK").ToString.Trim
                End If
                'item.FreightTerm = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_FREIGHT_TERMS"))
                item.tax = Common.parseNull(tDS.Tables(0).Rows(0).Item("CM_TAX_REG_NO"))
                item.buyer_email = Common.parseNull(tDS.Tables(0).Rows(0).Item("UM_EMAIL"))
                '//hardcode for temporary
                item.PO_type = "Regular"
                item.urgent = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_URGENT"))
                'Stage 3 (Enhancement) (GST-0006) - 09/07/2015 - CH
                item.FFPO_Type = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_PO_TYPE"))
                item.Submit_PO_Date = tDS.Tables(0).Rows(0).Item("POM_SUBMIT_DATE").ToString.Trim
                item.Created_By = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_CREATED_BY"))
            End If

            Return strsql


        End Function
        'Michelle (23/11/2010) - For PO
        'Function getPOForAppr(ByVal strPoNo As String, ByVal strPOIndex As String) As DataSet
        '    Dim strSql, strSqlPOM, strSqlPOD, strSqlCustomM, strSqlCustomD, strSqlAttach, strCoyID As String
        '    Dim strPOField As String
        '    Dim ds, ds1 As DataSet
        '    strCoyID = HttpContext.Current.Session("CompanyId")

        '    strPOField = "PM.POM_PO_NO, PM.POM_PO_INDEX, PM.POM_STATUS_CHANGED_ON, PM.POM_PO_DATE, PM.POM_BUYER_NAME, PM.POM_BUYER_PHONE, PM.POM_B_ADDR_LINE1, PM.POM_B_ADDR_LINE2, PM.POM_B_ADDR_LINE3, " _
        '    & "PM.POM_B_POSTCODE, PM.POM_B_CITY, PM.POM_INTERNAL_REMARK, PM.POM_S_COY_ID, PM.POM_S_COY_NAME, PM.POM_CURRENCY_CODE,PM.POM_BUYER_ID, PM.POM_PAYMENT_TERM, PM.POM_PAYMENT_METHOD, PM.POM_SHIPMENT_TERM, PM.POM_SHIPMENT_MODE, " _
        '    & "PM.POM_S_ADDR_LINE1,PM.POM_S_ADDR_LINE2,PM.POM_S_ADDR_LINE3,PM.POM_S_POSTCODE,PM.POM_S_CITY,PM.POM_S_STATE,PM.POM_S_COUNTRY,PM.POM_S_PHONE,PM.POM_S_FAX,PM.POM_S_EMAIL,PM.POM_SHIP_VIA,PM.POM_FREIGHT_TERMS,PM.POM_S_ATTN,PM.POM_CREATED_DATE, " _
        '    & "PM.POM_External_Remark, PM.POM_PO_Status,PM.POM_RFQ_INDEX, PM.POM_SHIP_AMT "

        '    strSqlPOM = "SELECT " & strPOField & ", CM.CM_TAX_CALC_BY, " _
        '    & "(SELECT STATUS_DESC FROM STATUS_MSTR SM WHERE SM.STATUS_NO=PM.POM_PO_STATUS AND STATUS_TYPE='PO') as STATUS_DESC, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PM.POM_B_COUNTRY) AS CT, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PM.POM_B_STATE AND CODE_VALUE=PM.POM_B_COUNTRY) AS STATE " _
        '    & "FROM PO_MSTR PM LEFT JOIN COMPANY_MSTR CM ON PM.POM_S_COY_ID=CM.CM_COY_ID " _
        '    & "LEFT JOIN RFQ_MSTR ON PM.POM_RFQ_INDEX=RM_RFQ_ID " _
        '    & "WHERE " _
        '    & "POM_B_COY_ID='" & strCoyID & "' AND POM_PO_No='" & strPoNo & "'"

        '    strSqlPOD = "SELECT PD.*, COMPANY_B_GL_CODE.CBG_B_GL_DESC , " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='CT' AND CODE_ABBR=PD.POD_D_COUNTRY) AS CT, " _
        '    & "(SELECT CODE_DESC FROM CODE_MSTR WHERE CODE_CATEGORY='S' AND CODE_ABBR=PD.POD_D_STATE AND CODE_VALUE=PD.POD_D_COUNTRY) AS STATE " _
        '    & "FROM PO_DETAILS PD " _
        '    & "LEFT JOIN COMPANY_B_GL_CODE " _
        '    & "ON COMPANY_B_GL_CODE.CBG_B_GL_CODE = PD.POD_B_GL_CODE " _
        '    & "WHERE POD_COY_ID='" & strCoyID & "' AND POD_PO_NO='" & strPoNo & "' ORDER BY POD_PO_LINE"

        '    strSqlCustomD = "SELECT * FROM PR_CUSTOM_FIELD_DETAILS WHERE PCD_TYPE = 'PO' AND PCD_PR_INDEX=" & strPOIndex
        '    strSqlCustomM = "SELECT * FROM PR_CUSTOM_FIELD_MSTR WHERE PCM_TYPE = 'PO' AND PCM_PR_INDEX=" & strPOIndex
        '    strSqlAttach = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID='" & strCoyID & "' AND CDA_DOC_NO='" & strPoNo & "' AND CDA_DOC_TYPE='PO'"

        '    strSql = strSqlPOM & ";" & strSqlPOD & ";" & strSqlCustomM & ";" & strSqlCustomD & ";" & strSqlAttach
        '    ds = objDb.FillDs(strSql)

        '    ds.Tables(0).TableName = "PO_MSTR"
        '    ds.Tables(1).TableName = "PO_DETAILS"
        '    ds.Tables(2).TableName = "PR_CUSTOM_FIELD_MSTR"
        '    ds.Tables(3).TableName = "PR_CUSTOM_FIELD_DETAILS"
        '    ds.Tables(4).TableName = "COMPANY_DOC_ATTACHMENT"
        '    Return ds
        'End Function


        'Public Function get_PO(ByVal strPO As String, ByVal sProductCode As Integer, ByVal sProductIndex As Integer) As Boolean

        '    Dim strsql As String

        '    strsql = "select * from PO_DETAILS "
        '    strsql &= "WHERE POD_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "
        '    strsql &= "AND POD_PO_LINE = '" & sProductIndex & "' "
        '    strsql &= "AND POD_PRODUCT_CODE = '" & sProductCode & "'; "

        '    Dim tDS As DataSet = objDb.FillDs(strsql)
        '    If tDS.Tables(0).Rows.Count > 0 Then
        '        Return True
        '    End If

        'End Function

        'Public Function get_PO_Quo(ByVal strPO As String) As DataSet

        '    Dim strsql As String

        '    strsql = "select * from PO_MSTR "
        '    strsql &= "WHERE POM_PO_NO = '" & strPO & "' "
        '    strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'; "

        '    Return objDb.FillDs(strsql)

        'End Function


        Public Function get_POLineList(ByVal companyID As String, ByVal item As POValue)

            Dim strSql As String

            'Chee Hong - 30/09/2014 - GST Enhancement
            'strsql = "select * from PO_MSTR,PO_DETAILS WHEre POM_PO_NO= '" & Common.Parse(item.PO_Number) & "'" & _
            '         " AND POD_PO_LINE = '" & Common.Parse(item.PO_Line) & "' AND POM_B_COY_ID= '" & Common.Parse(item.buyer_coy) & "' " & _
            '         " AND POD_PO_NO=POM_PO_NO AND POM_B_COY_ID=POD_COY_ID AND POM_S_COY_ID= '" & Common.Parse(v_com_id) & "'"
            strSql = "SELECT PO_MSTR.*, PO_DETAILS.*, " &
                    "CASE WHEN POD_GST_RATE = 'N/A' THEN POD_GST_RATE ELSE " &
                    "IF(TAX_PERC IS NULL OR TAX_PERC = '', IFNULL(CODE_DESC,''), CONCAT(CODE_DESC, ' (', TAX_PERC, '%)')) END AS GST_RATE " &
                    "FROM PO_MSTR " &
                    "INNER JOIN PO_DETAILS ON POD_PO_NO = POM_PO_NO AND POD_COY_ID = POM_B_COY_ID " &
                    "LEFT JOIN COMPANY_MSTR ON CM_COY_ID = POM_B_COY_ID " &
                    "LEFT JOIN CODE_MSTR ON CODE_CATEGORY = 'SST' AND CODE_ABBR = POD_GST_RATE " &
                    "LEFT JOIN TAX ON TAX_CODE = POD_GST_RATE AND TAX_COUNTRY_CODE = CM_COUNTRY " &
                    "WHERE POM_PO_NO = '" & Common.Parse(item.PO_Number) & "' AND POM_B_COY_ID = '" & Common.Parse(item.buyer_coy) & "' " &
                    "AND POM_S_COY_ID = '" & Common.Parse(companyID) & "' AND POD_PO_LINE = '" & Common.Parse(item.PO_Line) & "' "
            '----------------------------------------

            Dim tDS As DataSet = objDb.FillDs(strSql)
            If tDS.Tables(0).Rows.Count > 0 Then
                item.PO_Number = tDS.Tables(0).Rows(0).Item("POM_PO_NO").ToString.Trim
                item.PO_Date = tDS.Tables(0).Rows(0).Item("POM_PO_DATE")
                item.PO_Line = tDS.Tables(0).Rows(0).Item("POD_PO_LINE")
                item.vendor_Item_Code = tDS.Tables(0).Rows(0).Item("POD_VENDOR_ITEM_CODE").ToString.Trim
                item.Product_Desc = tDS.Tables(0).Rows(0).Item("POD_PRODUCT_DESC").ToString.Trim
                item.cur = Common.parseNull(tDS.Tables(0).Rows(0).Item("POM_CURRENCY_CODE"))
                item.Order_Qty = tDS.Tables(0).Rows(0).Item("POD_ORDERED_QTY")
                item.Rec_Qty = tDS.Tables(0).Rows(0).Item("POD_RECEIVED_QTY")
                item.Rej_Qty = tDS.Tables(0).Rows(0).Item("POD_REJECTED_QTY")
                item.Unit_Cost = tDS.Tables(0).Rows(0).Item("POD_UNIT_COST")
                item.tax = tDS.Tables(0).Rows(0).Item("POD_GST")
                item.remarks = tDS.Tables(0).Rows(0).Item("POD_REMARK").ToString.Trim
                item.POM_CREATED_DATE = tDS.Tables(0).Rows(0).Item("POM_CREATED_DATE")
                item.gst_code = tDS.Tables(0).Rows(0).Item("GST_RATE")
            End If

        End Function

        'Name       : isConvertedFromRFQ
        'Author     : Tan Ai Chu
        'Descption  : Check whether PO is converted from RFQ
        'Remark     : 
        'ReturnValue: True is PO is converted from RFQ      
        'LastUpadte : 06 Dec 2005
        'Version    : 1.00
        Public Function isConvertedFromRFQ(ByVal intPOIndex As Integer, ByRef ds As DataSet) As Boolean
            Dim strsql As String
            strsql = "SELECT POM_RFQ_INDEX, POM_S_COY_ID, RM_RFQ_No, RM_RFQ_Name, RRM_Actual_Quot_Num FROM PO_MSTR "
            strsql &= "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID "
            strsql &= "LEFT JOIN RFQ_REPLIES_MSTR ON RRM_RFQ_ID = POM_RFQ_INDEX AND RRM_V_Company_ID = POM_S_COY_ID "
            strsql &= "WHERE POM_PO_INDEX = " & intPOIndex & " AND POM_RFQ_INDEX IS NOT NULL "
            ' to cater for old data
            strsql &= " UNION SELECT PRM_RFQ_INDEX AS POM_RFQ_INDEX, PRM_S_COY_ID AS POM_S_COY_ID,RM_RFQ_No, RM_RFQ_Name, RRM_Actual_Quot_Num FROM PR_MSTR "
            strsql &= "LEFT JOIN RFQ_MSTR ON PRM_RFQ_INDEX = RM_RFQ_ID "
            strsql &= "LEFT JOIN RFQ_REPLIES_MSTR ON RRM_RFQ_ID = PRM_RFQ_INDEX AND RRM_V_Company_ID = PRM_S_COY_ID "
            strsql &= "WHERE PRM_PO_INDEX = " & intPOIndex & " AND PRM_RFQ_INDEX IS NOT NULL "
            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                isConvertedFromRFQ = True
            Else
                isConvertedFromRFQ = False
            End If
        End Function

        Public Function isConvertedFromRFQ(ByVal sPONo As String) As Boolean
            Dim strsql As String
            'Michelle (19/11/2010) - To cater fo PO
            'strsql = "SELECT PRM_RFQ_INDEX, PRM_S_COY_ID, RM_RFQ_No, RM_RFQ_Name, RRM_Actual_Quot_Num FROM PR_MSTR "
            strsql = "SELECT POM_PO_INDEX FROM PO_MSTR "
            strsql &= "LEFT JOIN RFQ_MSTR ON POM_RFQ_INDEX = RM_RFQ_ID "
            strsql &= "LEFT JOIN RFQ_REPLIES_MSTR ON RRM_RFQ_ID = POM_RFQ_INDEX AND RRM_V_Company_ID = POM_S_COY_ID "
            strsql &= "WHERE POM_PO_NO = '" & sPONo & "' AND POM_RFQ_INDEX IS NOT NULL "
            strsql &= "AND POM_B_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "' "

            Dim tDS As DataSet = objDb.FillDs(strsql)
            If tDS.Tables(0).Rows.Count > 0 Then
                isConvertedFromRFQ = True
            Else
                isConvertedFromRFQ = False
            End If
            tDS = Nothing
        End Function

        Function getPoAttachment(ByVal strPONo As String, ByVal strBCoyID As String) As String
            Dim ds As DataSet
            Dim strSqlAttach As String = "SELECT * FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strBCoyID) & "' AND CDA_DOC_NO= '" & Common.Parse(strPONo) & "' AND CDA_DOC_TYPE='PO'"
            'ds = objDb.FillDs(strSqlAttach)
            Return strSqlAttach
        End Function
        Function HasAttachment(ByVal strPONo As String, ByVal strBCoyID As String) As Boolean
            Dim ds As DataSet
            Dim strSqlAttach As String = "SELECT '*' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strBCoyID) & "' AND CDA_DOC_NO= '" & Common.Parse(strPONo) & "' AND CDA_DOC_TYPE='PO'"
            If objDb.Exist(strSqlAttach) >= 1 Then
                Return True
            Else
                Return False
            End If
        End Function
        Function HasAttachmentVen(ByVal strPONo As String) As Boolean
            Dim ds As DataSet
            Dim strCoyID As String
            strCoyID = HttpContext.Current.Session("CompanyId")
            Dim strSqlAttach As String = "SELECT '*' FROM COMPANY_DOC_ATTACHMENT WHERE CDA_COY_ID= '" & Common.Parse(strCoyID) & "' AND CDA_DOC_NO= '" & Common.Parse(strPONo) & "' AND CDA_DOC_TYPE='PO'"
            If objDb.Exist(strSqlAttach) >= 1 Then
                Return True
            Else
                Return False
            End If
        End Function
        'Public Function getPreferVendor(ByVal pProdCode As ArrayList) As Arraylist
        '    Dim strsql As String
        '    Dim ds As DataSet
        '    Dim strProCode As String
        '    Dim i As Integer
        '    Dim aVendor As New Arraylist


        '    For i = 0 To pProdCode.Count - 1
        '        If strProCode = "" Then
        '            strProCode = "'" & pProdCode(i) & "'"
        '        Else
        '            strProCode &= ", '" & pProdCode(i) & "'"
        '        End If
        '    Next

        '    strsql = "SELECT PM_PREFER_S_COY_ID FROM PRODUCT_MSTR " & _
        '             "WHERE PM_PRODUCT_CODE IN (" & strProCode & ")" & _
        '             "UNION " & _
        '             "SELECT PM_1ST_S_COY_ID FROM PRODUCT_MSTR " & _
        '             "WHERE PM_PRODUCT_CODE IN (" & strProCode & ")" & _
        '             "UNION " & _
        '             "SELECT PM_2ND_S_COY_ID FROM PRODUCT_MSTR " & _
        '             "WHERE PM_PRODUCT_CODE IN (" & strProCode & ")" & _
        '             "UNION " & _
        '             "SELECT PM_3RD_S_COY_ID FROM PRODUCT_MSTR " & _
        '             "WHERE PM_PRODUCT_CODE IN (" & strProCode & ")"

        '    ds = objDb.FillDs(strsql)



        '    For i = 0 To ds.Tables(0).Rows.Count - 1

        '        aVendor.add(ds.Tables(0).Rows(i).Item("PM_PREFER_S_COY_ID"))

        '    Next

        '    Return aVendor


        'End Function

        Public Function getPreferVendorList(ByVal pProdCode As ArrayList) As DataTable
            Dim strsql As String
            Dim dt As DataTable
            Dim strProCode As String
            Dim i As Integer
            Dim aVendor As New ArrayList


            For i = 0 To pProdCode.Count - 1
                If strProCode = "" Then
                    strProCode = "'" & pProdCode(i) & "'"
                Else
                    strProCode &= ", '" & pProdCode(i) & "'"
                End If
            Next

            strsql = "SELECT PM_PREFER_S_COY_ID AS Prefer,PM_1ST_S_COY_ID AS 1ST, PM_2ND_S_COY_ID AS 2ND, PM_3RD_S_COY_ID AS 3RD  FROM PRODUCT_MSTR " &
                     "WHERE PM_PRODUCT_CODE IN (" & strProCode & ")"

            dt = objDb.FillDt(strsql)



            'For i = 0 To dt.Tables(0).Rows.Count - 1

            '    aVendor.Add(dt.Tables(0).Rows(i).Item("PM_PREFER_S_COY_ID"))

            'Next

            Return dt


        End Function

        Public Function getPRNo(ByVal po_id As String) As String
            Dim strsql As String = "SELECT IFNULL(CAST(GROUP_CONCAT(DISTINCT PRD_PR_NO) AS CHAR(10000)),'') AS PRD_PR_NO FROM PR_DETAILS WHERE PRD_CONVERT_TO_IND = 'PO' AND PRD_CONVERT_TO_DOC = '" & Common.Parse(po_id) & "' and PRD_COY_ID = '" & HttpContext.Current.Session("CompanyId") & "'"
            getPRNo = objDb.GetVal(strsql)

        End Function

        Public Sub GetReportTotal(ByVal strPONo As String, ByVal strCoyID As String, ByRef decSubTotal As Decimal, ByRef decTax As Decimal)
            Dim strsql As String
            Dim ds As DataSet

            ' yAP - 23April2014: Put Rounding, follow screen where round by itemize
            strsql = "SELECT SUM(ROUND(((POD_ORDERED_QTY*POD_UNIT_COST)),2)) AS 'SubTot'," _
                & "SUM((ROUND(((POD_ORDERED_QTY*POD_UNIT_COST*POD_GST)/100),2))) AS 'Tax' " _
                & "FROM PO_MSTR " _
                & "INNER JOIN PO_DETAILS ON PO_MSTR.POM_PO_NO = PO_DETAILS.POD_PO_NO " _
                & "AND PO_MSTR.POM_B_COY_ID = PO_DETAILS.POD_COY_ID " _
                & "INNER JOIN COMPANY_MSTR ON PO_MSTR.POM_B_COY_ID = COMPANY_MSTR.CM_COY_ID " _
                & "INNER JOIN COMPANY_MSTR AS COMPANY_MSTR_1 ON PO_MSTR.POM_S_COY_ID = COMPANY_MSTR_1.CM_COY_ID " _
                & "INNER JOIN USER_MSTR ON PO_MSTR.POM_BUYER_ID = USER_MSTR.UM_USER_ID AND PO_MSTR.POM_B_COY_ID = USER_MSTR.UM_COY_ID " _
                & "WHERE PO_MSTR.POM_B_COY_ID = '" & Common.Parse(strCoyID) & "' " _
                & "AND PO_MSTR.POM_PO_NO = '" & Common.Parse(strPONo) & "'"

            ds = objDb.FillDs(strsql)
            If ds.Tables(0).Rows.Count > 0 Then
                decSubTotal = ds.Tables(0).Rows(0).Item("SubTot")
                decTax = ds.Tables(0).Rows(0).Item("Tax")
            End If

        End Sub

        Public Function GetPRNo(ByVal strPONo As String, ByRef PRNo() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = "SELECT DISTINCT PRM_PR_NO " _
                & "FROM pr_mstr " _
                & "INNER JOIN pr_details ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
                & "WHERE PRM_PR_TYPE='CC' AND PRD_CONVERT_TO_IND = 'PO' AND PRD_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                & "AND PRD_CONVERT_TO_DOC='" & Common.Parse(strPONo) & "' AND PRD_CONVERT_TO_DOC IS NOT NULL"

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                PRNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRM_PR_NO")
                COUNT = COUNT + 1
            Next
        End Function

        Public Function GetPRNoAll(ByVal strPONo As String, ByRef PRNo() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = "SELECT DISTINCT PRM_PR_NO " _
                & "FROM pr_mstr " _
                & "INNER JOIN pr_details ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
                & "WHERE PRD_CONVERT_TO_IND = 'PO' AND PRD_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                & "AND PRD_CONVERT_TO_DOC='" & Common.Parse(strPONo) & "' AND PRD_CONVERT_TO_DOC IS NOT NULL"

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                PRNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRM_PR_NO")
                COUNT = COUNT + 1
            Next
        End Function

        Public Function GetPRNoAllRFQ(ByVal strPONo As String, ByRef PRNo() As String, ByRef COUNT As Integer)
            COUNT = 0
            Dim strSQL As String

            strSQL = "SELECT DISTINCT PRM_PR_NO " _
                & "FROM pr_mstr " _
                & "INNER JOIN pr_details ON PRD_PR_NO=PRM_PR_NO AND PRD_COY_ID=PRM_COY_ID " _
                & "WHERE PRD_CONVERT_TO_IND = 'RFQ' AND PRD_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                & "AND PRD_CONVERT_TO_DOC='" & Common.Parse(strPONo) & "' AND PRD_CONVERT_TO_DOC IS NOT NULL"

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                PRNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRM_PR_NO")
                COUNT = COUNT + 1
            Next
        End Function

        Public Function GetPRNo1(ByVal intPOIdx As Integer, ByRef PRNo() As String, ByRef COUNT As Integer) As String
            COUNT = 0
            Dim strSQL As String

            strSQL = "SELECT PRM_PR_NO FROM PR_MSTR " _
                    & "WHERE PRM_COY_ID='" & Common.Parse(HttpContext.Current.Session("CompanyId")) & "' " _
                    & "AND PRM_PO_INDEX = " & intPOIdx

            Dim tDS As DataSet = objDb.FillDs(strSQL)
            For j As Integer = 0 To tDS.Tables(0).Rows.Count - 1
                PRNo(COUNT) = tDS.Tables(0).Rows(j).Item("PRM_PR_NO")
                COUNT = COUNT + 1
            Next

        End Function

        Public Function chkPOItemStk(ByVal PO_No As String, ByVal side As String, ByVal strBCoyId As String) As Boolean
            Dim strSql As String = ""

            strSql = " SELECT '*' " &
                    "FROM PO_DETAILS,PO_MSTR WHERE POD_PO_NO = POM_PO_NO AND POM_B_COY_ID=POD_COY_ID AND POM_PO_NO= '" & Common.Parse(PO_No) & "' " &
                    "AND POM_B_COY_ID= '" & Common.Parse(strBCoyId) & "' AND POD_ITEM_TYPE = 'ST' "

            If objDb.Exist(strSql) Then
                chkPOItemStk = True
            Else
                chkPOItemStk = False
            End If

        End Function

    End Class
End Namespace
